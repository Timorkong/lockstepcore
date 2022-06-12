using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PROTOCOLWAR;

public class MoveFrameCommand : BaseFrameCommand, IFrameCommand
{
    private WarMove cmd = null;

    public Cmd.ID.CMD CmdId
    {
        get
        {
            return Cmd.ID.CMD.WarMove;
        }
    }

    public MoveFrameCommand(MsgData msgData) : base(msgData)
    {
        cmd = NetUtil.DeserializeMsg<WarMove>(msgData);

        this.PlayerSeat = cmd.Seat;

        if (Global.Setting.ShowNetWorkLog)
        {
            Debug.LogError(ToString());
        }
    }

    public void ExecCommand()
    {
        var beEntity = GetActorBySeat();

        if (beEntity != null)
        {
            beEntity.SetMoveDir(cmd.MoveX, cmd.MoveY);
        }
    }

    public string GetString
    {
        get
        {
            var ret = string.Format("收到战斗帧移动 seat = {0} move_x = {1} move_y = {2}", cmd.Seat, cmd.MoveX, cmd.MoveY);

            return ret;
        }
    }
}

public class SyncSequenceCommand : BaseFrameCommand, IFrameCommand
{
    private PROTOCOLWAR.WarSequenceNotice cmd = null;

    public Cmd.ID.CMD CmdId
    {
        get { return Cmd.ID.CMD.WarSequenceNotice; }
    }

    public SyncSequenceCommand(MsgData msgData) : base(msgData)
    {
        cmd = NetUtil.DeserializeMsg<WarSequenceNotice>(msgData);

        if (Global.Setting.ShowSequence)
        {
            Debug.LogError(ToString());
        }
    }

    public override uint Sequence => (uint)cmd.Sequence;

    public void ExecCommand()
    {
        
    }

    public string GetString
    {
        get
        {
            var ret = string.Format("收到帧同步数据 sequence = {0} ", cmd.Sequence);

            return ret;
        }
    }
}
