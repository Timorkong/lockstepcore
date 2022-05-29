using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    None,
    Idle,
    Walk,
    Run,
}

public interface IPlayerState 
{
    PlayerStateType stateType { get; }

    void Reset();

    void Enter();

    void Update();

    void Exit();
}

public class BasePlayerState : IPlayerState
{
    protected PlayerStateManager psm = null;

    public void Reset()
    {

    }

    public BasePlayerState(PlayerStateManager psm)
    {
        this.psm = psm;
    }

    public virtual PlayerStateType stateType
    {
        get { return PlayerStateType.None; }
    }

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }
}