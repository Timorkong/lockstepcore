using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using PROTOCOL;
using PROTOCOLROOM;

public partial class command_req
{
    public static void RoomListReq()
    {
        RoomListReq req = new RoomListReq();

        var sendLen = NetManager.Instance.SendMsg(req, Cmd.ID.CMD.RoomListReq);
    }

    public static void CreateRoomReq()
    {
        CreateRoomReq req = new CreateRoomReq();

        req.RoomName = "default player enter room";

        NetManager.Instance.SendMsg(req, Cmd.ID.CMD.CreateRoomReq);
    }

    public static void LeaveRoomReq()
    {
        LeaveRoomReq req = new LeaveRoomReq();

        NetManager.Instance.SendMsg(req, Cmd.ID.CMD.LeaveRoomReq);
    }

    public static void JoinRoomReq(int room_unique_id)
    {
        JoinRoomReq req = new JoinRoomReq();

        req.RoomUnquieId = room_unique_id;

        NetManager.Instance.SendMsg(req, Cmd.ID.CMD.JoinRoomReq);
    }
}