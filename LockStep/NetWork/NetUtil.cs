using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Protobuf;
using System.IO;


public class NetUtil
{
    public static T DeserializeMsg<T>(MsgData data , bool showLog = false) where T:IMessage<T>,new ()
    {
        T rsp = Deserialize<T>(data.msg);
        return rsp;
    }

    public static T Deserialize<T>(byte[] msg, bool showLog = false) where T : IMessage<T>, new()
    {
        T rsp;
        using (MemoryStream stream = new MemoryStream(msg))
        {
            Google.Protobuf.MessageParser<T> paser = new MessageParser<T>(() => new T());
            rsp = paser.ParseFrom(msg);
            //rsp = ProtoBuf.Serializer.Deserialize<T>(stream);
        }
        return rsp;
    }

    public static byte[] Serialize<T>(T msg) where T:IMessage
    {
        byte[] ret = null;

        using (MemoryStream stream = new MemoryStream())
        {
            ret = Google.Protobuf.MessageExtensions.ToByteArray(msg);
        }

        return ret;
    }
}
