using Cmd.ID;
using PROTOCOLWAR;

public partial class command_rsp
{
    [MessageHandle((uint)CMD.EnterGameRsp)]
    public static void EnterGameRsp(MsgData msg)
    {
        EnterGameRsp rsp = NetUtil.DeserializeMsg<EnterGameRsp>(msg);

        RoomInfo.Instance.Hide();

        Loading.Instance.Show();

        BattleMain.data = rsp.Data;

        ClientSystemManager.Instance.SwitchSystem<ClientSystemBattle>();
    }

    [MessageHandle((uint)CMD.StartGameRsp)]
    public static void StartGameRsp(MsgData msg)
    {
        StartGameRsp rsp = NetUtil.DeserializeMsg<StartGameRsp>(msg);

        foreach(var user in rsp.RoomInfo.UserList)
        {
            BattleMain.Instance.mBattle.CreateEntity(user.UserSeat);
        }

        ClientSystemManager.Instance.CurrentSystem.curState = EnumClienSystemState.onTick;

        FrameSync.Instance.StartFrameSync(Global.Setting.SyncMode);

        Loading.Instance.Hide();

        InputManager.Instance.Show();
    }
}
