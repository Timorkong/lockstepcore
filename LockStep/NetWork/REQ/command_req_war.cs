using PROTOCOLWAR;

public partial class command_req
{

    public static void EnterGameReq()
    {
        EnterGameReq req = new EnterGameReq();
        req.Data = new PROTOCOLCOMMON.PreBattleData();
        req.Data.LevelName = "Level1.level";
        NetManager.Instance.SendMsg(req, Cmd.ID.CMD.EnterGameReq);
    }

    public static void StartGameReq()
    {
        StartGameReq req = new StartGameReq();
        NetManager.Instance.SendMsg(req, Cmd.ID.CMD.StartGameReq);
    }

    public static void WarMove(float x, float y)
    {
        WarMove req = new WarMove();

        if (BattleMain.Instance.mBattle.mainBeEntity != null)
        {
            req.Seat = BattleMain.Instance.mBattle.mainBeEntity.seat;

            req.MoveX = x;

            req.MoveY = y;

            NetManager.Instance.SendMsg(req, Cmd.ID.CMD.WarMove);
        }

    }
}