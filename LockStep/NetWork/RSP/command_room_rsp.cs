using UnityEngine;
using Cmd.ID;
using PROTOCOLROOM;

public partial class command_rsp 
{
    [MessageHandle((uint) CMD.RoomListRsp)]
    public static void RoomListRsp(MsgData msg)
    {
        RoomListRsp rsp = NetUtil.DeserializeMsg<RoomListRsp>(msg);

        RoomList.Instance.Refresh(rsp.RoomList);
    }

    [MessageHandle((uint)CMD.CreateRoomRsp)]
    public static void CreateRoomRsp(MsgData msg)
    {
        CreateRoomRsp rsp = NetUtil.DeserializeMsg<CreateRoomRsp>(msg);

        GameApplaction.Instance.playerSeat = rsp.PlayerSeat;

        if (Global.Setting.ShowNetWorkLog)
        {
            Debug.Log(string.Format("创建房间 player_seat = {0}", rsp.PlayerSeat));
        }

        RoomInfo.Instance.Refresh(rsp.RoomInfo);
    }

    [MessageHandle((uint)CMD.LeaveRoomRsp)]
    public static void LeaveRoomRsp(MsgData msg)
    {
        LeaveRoomRsp rsp = NetUtil.DeserializeMsg<LeaveRoomRsp>(msg);

        RoomList.Instance.Refresh(rsp.RoomList);
    }

    [MessageHandle((uint)CMD.UpdateRoomInfoNotice)]
    public static void UpdateRoomInfoNotice(MsgData msg)
    {
        UpdateRoomInfoNotice rsp = NetUtil.DeserializeMsg<UpdateRoomInfoNotice>(msg);

        RoomInfo.Instance.Refresh(rsp.RoomInfo);
    }

    [MessageHandle((uint)CMD.JoinRoomRsp)]
    public static void JoinRoomRsp(MsgData msg)
    {
        JoinRoomRsp rsp = NetUtil.DeserializeMsg<JoinRoomRsp>(msg);

        GameApplaction.Instance.playerSeat = rsp.PlayerSeat;

        if (Global.Setting.ShowNetWorkLog)
        {
            Debug.Log(string.Format("加入房间 seat = {0}", rsp.PlayerSeat));
        }

        RoomInfo.Instance.Show();

        RoomInfo.Instance.Refresh(rsp.RoomInfo);
    }
}
