
using UnityEngine;

public class NoneState : BasePlayerState
{
    public override PlayerStateType stateType => PlayerStateType.None;

    public NoneState(PlayerStateManager psm) : base(psm)
    {

    }

    public override void Update()
    {
        base.Update();

        psm.SwitchStete(PlayerStateType.Idle);
    }
}

public class IdleState : BasePlayerState
{
    public override PlayerStateType stateType => PlayerStateType.Idle;

    public IdleState(PlayerStateManager psm) : base(psm)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        psm.playerStateData.ClipName = "Idle";

        psm.playerStateData.ClipFrame = 0;

        if (Global.Setting.ShowPlayerStateLog)
        {
            Debug.LogError("change clip Idle");
        }
    }

    public override void Update()
    {
        base.Update();

        if (psm.playerStateData.moveDir.x.IsZero() == false || psm.playerStateData.moveDir.z.IsZero() == false)
        {
            psm.SwitchStete(PlayerStateType.Walk);

            return;
        }
    }
}

public class WalkState : BasePlayerState
{
    public override PlayerStateType stateType => PlayerStateType.Walk;

    public WalkState(PlayerStateManager psm) : base(psm)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        psm.playerStateData.ClipName = "Walk";

        psm.playerStateData.ClipFrame = 0;

        if (Global.Setting.ShowPlayerStateLog)
        {
            Debug.LogError("change clip Walk");
        }
    }

    public override void Update()
    {
        base.Update();

        if(psm.playerStateData.moveDir.x.IsZero() && psm.playerStateData.moveDir.z.IsZero())
        {
            psm.SwitchStete(PlayerStateType.Idle);

            return;
        }

        var deltaPos = FixedVec3.GetPool().CopyFrom(psm.playerStateData.moveDir);

        deltaPos *= psm.playerStateData.speed;

        psm.playerStateData.pos += deltaPos;

        deltaPos.PoolRecover();
    }
}
