using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCommandFactory
{
    public static IFrameCommand CreateCommand(MsgData msg)
    {
        IFrameCommand ret = null;

        switch ((Cmd.ID.CMD)msg.msgID)
        {
            case Cmd.ID.CMD.WarMove:
                {
                    ret = new MoveFrameCommand(msg);

                    break;
                }
            case Cmd.ID.CMD.WarSequenceNotice:
                {
                    ret = new SyncSequenceCommand(msg);

                    break;
                }
        }

        return ret;
    }
}
