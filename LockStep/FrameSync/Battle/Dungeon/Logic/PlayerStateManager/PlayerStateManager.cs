using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class PlayerStateData
{
    public FixedVec3 moveDir;

    public FixedVec3 pos;

    public FixedNumber speed;

    public string ClipName = "";

    public int ClipFrame = 0;

    public PlayerStateData()
    {
        moveDir = FixedVec3.GetPool();

        pos = FixedVec3.GetPool();

        speed = FixedNumber.GetPool().One();
    }

    public static PlayerStateData GetPool()
    {
        var ret = ObjectPool<PlayerStateData>.Instance.GetItem();

        return ret;
    }

    public void PoolRecover()
    {
        moveDir.PoolRecover();

        pos.PoolRecover();

        speed.PoolRecover();

        moveDir = null;

        pos = null;

        speed = null;
    }
}

public class PlayerStateManager
{
    Dictionary<PlayerStateType, IPlayerState> dicState = new Dictionary<PlayerStateType, IPlayerState>();

    IPlayerState curState = null;

    IPlayerState nextState = null;

    public PlayerStateData playerStateData;

    protected BeEntity beEntity;

    public static PlayerStateManager GetPool()
    {
        var ret = ObjectPool<PlayerStateManager>.Instance.GetItem();

        return ret;
    }

    public void PoolRecover()
    {
        this.playerStateData.PoolRecover();

        playerStateData = null;

        curState = null;

        nextState = null;
    }

    public PlayerStateManager()
    {
    }

    public void InitStateManager(BeEntity beEntity)
    {
        playerStateData = PlayerStateData.GetPool();
        this.beEntity = beEntity;
        dicState.Add(PlayerStateType.None, new NoneState(this));
        dicState.Add(PlayerStateType.Idle, new IdleState(this));
        dicState.Add(PlayerStateType.Walk, new WalkState(this));
        SwitchStete(PlayerStateType.None);
    }

    public void SwitchStete(PlayerStateType type)
    {
        if (curState != null)
        {
            if (Global.Setting.ShowPlayerStateLog)
            {
                Debug.LogError(string.Format("curtype = {0} to {1}", curState.stateType, type));
            }
           
            if (curState.stateType == type)
            {
                return;
            }
        }

        if (dicState.TryGetValue(type, out nextState) == false)
        {
            Debug.LogError("状态切换失败，状态机中没有 type = " + type);

            return;
        }

        if (curState != null)
        {
            curState.Exit();
        }

        nextState.Enter();

        curState = nextState;

        nextState = null;
    }

    public void Update()
    {
        if (curState != null)
        {
            curState.Update();
        }
    }
}
