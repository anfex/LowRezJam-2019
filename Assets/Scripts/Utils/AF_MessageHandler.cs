using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This class handles inbound and outbound messages for a specific entity. 
/// The manager adds messages here.
/// The handler is used from the entity:
/// - to register to specific events
/// - to send new messages
/// - to consume messages
/// </summary>
public class AF_MessageHandler
{
	#region Properties

	/// <summary>
	/// Current messages to be consumed by the owner.
	/// </summary>
    public List<AF_Message> MessagesToConsume { get; set; }

    #endregion

	#region Constructors

	/// <summary>
	/// Initializes a new instance of the <see cref="AF_MessageHandler"/> class.
	/// </summary>
	/// <param name="in_iHandlerID">Actor ID.</param>
	public AF_MessageHandler() 
	{
		MessagesToConsume = new List<AF_Message>();
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="AF_MessageHandler"/> class and registers to the Message Managers on a list of events.
	/// </summary>
	/// <param name="listEventsToListen">List events to listen.</param>
	public AF_MessageHandler(List<AF_Message.MsgType> in_listOfEventsToListen, int in_iHandlerID) : this()
	{
		if (in_listOfEventsToListen != null) 
			RegisterToManager (in_listOfEventsToListen);
		else if (Debug.isDebugBuild) 
			Debug.LogWarning ("in_listOfEventsToListen is null.");
	}

	#endregion

	/// <summary>
	/// Registers this listener to the manager on a single event.
	/// </summary>
	/// <param name="eventToListen">Event to listen.</param>
	public void RegisterToManager(AF_Message.MsgType in_eEventToListen)
	{
		if (in_eEventToListen != AF_Message.MsgType.NONE && in_eEventToListen != AF_Message.MsgType.COUNT)
            AF_MessageManager.Instance.AddListener (this, in_eEventToListen);
		else if (Debug.isDebugBuild) 
			Debug.LogWarning ("Parameter passed \"eventToListen\" has some invalid values.");
	}

	/// <summary>
	/// Registers this listener to the the manager on multiple events.
	/// </summary>
	/// <param name="listOfEventsToListen">List of events to listen.</param>
	public void RegisterToManager(List<AF_Message.MsgType> in_listOfEventsToListen)
	{
		foreach (AF_Message.MsgType msgType in in_listOfEventsToListen)
			RegisterToManager(msgType);
	}

	/// <summary>
	/// After a cycle all the messages should be consumed and the queue should be cleared by the Handler's owner.
	/// </summary>
	public void ClearMessages()
	{
		MessagesToConsume.Clear ();
	}

	/// <summary>
	/// Sends the message to the manager.
	/// </summary>
	/// <param name="eType">Message type.</param>
	public void SendMessage(AF_Message in_rMsg)
	{
        AF_MessageManager.Instance.AddMessageInManager(in_rMsg);
	}

    /// <summary>
    /// Checks if there is a message of a certain type, returns it and removes it from the handler's messages.
    /// </summary>
    /// <param name="msgType">Type of message to search.</param>
    /// <returns>Message object. This should be casted based on the type to get all the needed info.</returns>
    public AF_Message ReceiveMessage(AF_Message.MsgType msgType)
    {
        AF_Message msgToReturn = null;

        if (MessagesToConsume.Count > 0)
        {
            foreach (AF_Message msg in MessagesToConsume)
            {
                if (msg.Type == msgType)
                {
                    msgToReturn = msg;
                    MessagesToConsume.Remove(msg);
                    break;
                }
            }
        }

        return msgToReturn;
    }

    /// <summary>
    /// Clears all messages of a certain type from the handler's messages.
    /// </summary>
    /// <param name="msgType">Messages type to be removed from the handler's messages.</param>
    /// <returns>Numbers of messages found and removed.</returns>
    public int ClerMessagesOfType(AF_Message.MsgType msgType)
    {
        int iDeletedMessagesCount = 0;

        foreach (AF_Message msg in MessagesToConsume)
        {
            if (msg.Type == msgType)
            {
                MessagesToConsume.Remove(msg);
                iDeletedMessagesCount++;
            }
        }

        return iDeletedMessagesCount;
    }
}