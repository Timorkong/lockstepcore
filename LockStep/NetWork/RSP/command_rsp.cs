using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cmd.ID;
using PROTOCOL;
public partial class command_rsp 
{
    [MessageHandle((uint) CMD.HeartBeatRsp)]
    public static void HeartBeatRsp(MsgData msg)
    {
        HeartBeatRsp rsp = NetUtil.DeserializeMsg<HeartBeatRsp>(msg);
    }
}
