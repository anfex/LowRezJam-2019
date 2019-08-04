using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

public class AF_Message
{
	public enum MsgType
	{
        [Description("Empty message. Should be used only for testing purposes.")]
		NONE = 0,



        [Description("Restart the level.")]
        Restart,                        

		COUNT
	}


    #region Properties

    /// <summary>
    /// Message type.
    /// </summary>
    public MsgType Type { get; set; }

    /// <summary>
	/// This is the MsgHandler of the message sender. Identifies the toy that sent the message.
	/// </summary>
    public AF_MessageHandler Sender { get; set; }

    #endregion

    /// <summary>
    /// Constructor for base message type.
    /// </summary>
    /// <param name="msgType">Message Type.</param>
    /// <param name="sender">Object ID of the sender.</param>
    public AF_Message(MsgType msgType, AF_MessageHandler sender)
	{
		Type    = msgType;
        Sender  = sender;
	}

    public AF_Message Clone(AF_Message msgToClone)
    {
        return new AF_Message(msgToClone.Type, msgToClone.Sender);
    }
}

/// <summary>
/// Class for Vector3 message. 
/// </summary>
public class AF_MessageVector3 : AF_Message
{
	public Vector3 Value_1 { get; set; }
	public Vector3 Value_2 { get; set; }

    /// <summary>
    /// Constructor for Vector3 message. 
    /// </summary>
    /// <param name="msgType">Message Type.</param>
    /// <param name="value_1">Vector 3 Value to include in the message.</param>
    /// <param name="sender">MsgHandler of the toy that is sending the message.</param>
    public AF_MessageVector3(   AF_Message.MsgType msgType, Vector3 value_1, AF_MessageHandler sender) 
                                : base(msgType, sender)
	{
        Value_1 = value_1;
		Value_2 = Vector3.zero;
	}

    /// <summary>
    /// Constructor for message that includes 2 Vector3 values. 
    /// </summary>
    /// <param name="msgType">Message Type.</param>
    /// <param name="value_1">Vector 3 Value 1 to include in the message.</param>
    /// <param name="value_2">Vector 3 Value 2 to include in the message.</param>
    /// <param name="sender">MsgHandler of the toy that is sending the message.</param>
    public AF_MessageVector3(   AF_Message.MsgType msgType, Vector3 value_1, Vector3 value_2, AF_MessageHandler sender) 
                                : this(msgType, value_1, sender)
	{
		this.Value_2 = value_2;
	}

    public AF_MessageVector3 Clone(AF_MessageVector3 msgToClone)
    {
        return new AF_MessageVector3(msgToClone.Type, msgToClone.Value_1, msgToClone.Value_2, msgToClone.Sender);
    }
}

/// <summary>
/// Class for Object ID message. 
/// </summary>
public class AF_MessageObjectID : AF_Message
{
	public int ObjectHitID { get; set; }

    /// <summary>
    /// Class for message that include an Object ID. 
    /// </summary>
    /// <param name="msgType">Message Type.</param>
    /// <param name="objectHitID">Object ID included in the message.</param>
    /// <param name="sender">Actor's ID that is sending the message.</param>
    public AF_MessageObjectID(AF_Message.MsgType msgType, int objectHitID, AF_MessageHandler sender) : base(msgType, sender)
	{
        ObjectHitID = objectHitID;
	}

    public AF_MessageObjectID Clone(AF_MessageObjectID msgToClone)
    {
        return new AF_MessageObjectID(msgToClone.Type, msgToClone.ObjectHitID, msgToClone.Sender);
    }
}

/// <summary>
/// Class for generic message with an object. 
/// The object must be casted to be used.
/// </summary>
public class AF_MessageGenericObject : AF_Message
{
    public object GenericObject { get; set; }

    /// <summary>
    /// Message including a generic type of object to send. 
    /// </summary>
    /// <param name="msgType">Message Type.</param>
    /// <param name="genericObject">Object included in the message.</param>
    /// <param name="sender">Actor's ID that is sending the message.</param>
    public AF_MessageGenericObject(AF_Message.MsgType msgType, object genericObject, AF_MessageHandler sender) : base(msgType, sender)
    {
        GenericObject = genericObject;
    }

    public AF_MessageGenericObject Clone(AF_MessageGenericObject msgToClone)
    {
        return new AF_MessageGenericObject(msgToClone.Type, msgToClone.GenericObject, msgToClone.Sender);
    }
}