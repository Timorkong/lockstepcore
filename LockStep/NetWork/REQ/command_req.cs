using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using PROTOCOL;

public partial class command_req
{
    static int id = 0;

    public static void HeartBeatReq()
    {
        HeartBeatReq req = new HeartBeatReq();

        req.Id = id ++;

        req.Name = "fdafdafdafd";

        var sendLen = NetManager.Instance.SendMsg<HeartBeatReq>(req, Cmd.ID.CMD.HeartBeatReq);
    }
}