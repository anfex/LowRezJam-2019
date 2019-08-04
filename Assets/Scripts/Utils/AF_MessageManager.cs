using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class AF_MessageManager
{
	#region Members

	/// <summary>
	/// This member is the actual instance of the manager. 
	/// It is declared as static so that it exists only one in the program 
	/// and is created here to be multi-thread safe.
	/// </summary>
	private static readonly AF_MessageManager m_instance = new AF_MessageManager();

	/// <summary>
	/// This contains all the messages that need to be dispatched to the listeners.
	/// </summary>
	private Queue<AF_Message> m_qMessagesToDispatch = new Queue<AF_Message>();

	/// <summary>
	/// This dictionary stores a dynamic list of event and for each one has a list of listeners where the events are dispatched.
	/// </summary>
	private Dictionary<AF_Message.MsgType, List<AF_MessageHandler>> m_dictEventListenersRegistry = new Dictionary<AF_Message.MsgType, List<AF_MessageHandler>> ();

	#endregion

	#region Properties 

	public static AF_MessageManager Instance { get { return m_instance; } }

	#endregion

	private AF_MessageManager() {}

	public void Update() 
	{
		// dispatch the current messages and clear it
		while (m_qMessagesToDispatch.Count > 0)
		{
			AF_Message msgToDispatch = m_qMessagesToDispatch.Dequeue();

            // check if there is any record for the event type in the registry
			if (m_dictEventListenersRegistry.ContainsKey(msgToDispatch.Type)) 
			{
                // send the current message to all listeners
				foreach (AF_MessageHandler currListener in m_dictEventListenersRegistry[msgToDispatch.Type]) 
                    currListener.MessagesToConsume.Add(msgToDispatch);
			}
		}
	}

	#region Listeners Management

	/// <summary>
	/// Adds the Message Handler as listener to a specific event in the Message Manager.
	/// </summary>
	/// <param name="rListener">MessageHandler that needs to listen to the event.</param>
	/// <param name="eType">Event that the listener needs to listen to.</param>
	public void AddListener(AF_MessageHandler rListener, AF_Message.MsgType eType)
	{
		// check if the input values are valid
		if (rListener != null && eType != AF_Message.MsgType.NONE && eType != AF_Message.MsgType.COUNT) 
		{
			// check if the event that need to be listened is already in the registry
			if (m_dictEventListenersRegistry.ContainsKey (eType)) 
			{
				// check if the listener is not already registered to the event
				if (!m_dictEventListenersRegistry [eType].Contains (rListener)) 
				{
					m_dictEventListenersRegistry [eType].Add (rListener);
				} 
				else if (Debug.isDebugBuild) 
				{
					Debug.LogError ("Add Listener: listener already added for the event " + eType);
				}
			}	
			// otherwise I will create a new entry for the event in the registry and add the listener
			else 
			{
				m_dictEventListenersRegistry.Add (eType, new List<AF_MessageHandler> () { rListener });
			}
		} 
		else if (Debug.isDebugBuild) 
		{
			Debug.LogError ("Add Listener: some value is invalid. Can't add listener.");
		}
	}

	/// <summary>
	/// Removes the listener from all the events. Usually this is used when the listener's owner is going to be destroyed.
	/// </summary>
	/// <param name="rListener">Listener to remove.</param>
	public void RemoveListenerFromAllEvents(AF_MessageHandler rListener)
	{
		// scroll the dictionary looking for the listener in all events' list
		foreach (KeyValuePair<AF_Message.MsgType, List<AF_MessageHandler>> rList in m_dictEventListenersRegistry) 
		{
			// if this event list has at least one listener
			if (rList.Value.Count > 0) 
			{
				// perform the check to see if the listener exists
				if (rList.Value.Contains (rListener)) 
				{
					// and remove it
					rList.Value.Remove(rListener);
				}
			}
		}
	}

	/// <summary>
	/// Removes the listener from an event's registry.
	/// </summary>
	/// <param name="rListener">Listener to remove.</param>
	/// <param name="eType">Event from whom the listener will be removed.</param>
	public void RemoveListenerFromEvent(AF_MessageHandler rListener, AF_Message.MsgType eType)
	{
		// check if the registry contains that event
		if (m_dictEventListenersRegistry.ContainsKey (eType)) 
		{
			// perform the check to see if the listener exists
			if (m_dictEventListenersRegistry [eType].Contains (rListener)) 
			{
				// and remove it
				m_dictEventListenersRegistry [eType].Remove (rListener);
			}
		} 
		else if (Debug.isDebugBuild) 
		{
			Debug.LogWarning ("Remove listener: a listener asked to be removed from the event " + eType + " but the event is not in the registry.");
		}
	}

	#endregion

	#region Message Management

	/// <summary>
	/// Add a message into the manager to dispatch.
	/// </summary>
	/// <param name="in_rMsg">Message to dispatch.</param>
	/// <param name="in_iActorID">Actor's ID sending the message.</param>
	public void AddMessageInManager(AF_Message in_rMsg) 
	{
		if (in_rMsg != null) 
		{
			m_qMessagesToDispatch.Enqueue (in_rMsg);
		}
		else if (Debug.isDebugBuild)
		{
			Debug.LogWarning ("rMsg is null.");
		}
	}

	#endregion
}