using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeEntity
{
    public EntityType entityType = EntityType.Player;

    public BeScence beScence = null;

    public PlayerStateManager psm = null;

    public int seat = 0;

    public BeEntity(BeScence beScence, int seat)
    {
        this.beScence = beScence;

        this.seat = seat;

        psm = PlayerStateManager.GetPool();

        psm.InitStateManager(this);

        beScence.AddEntity(this);
    }

    public void SetMoveDir(float x , float y)
    {
        psm.playerStateData.moveDir.x.SetFloat(x);

        psm.playerStateData.moveDir.y.SetFloat(y);

        psm.playerStateData.moveDir.NormalizeSelf();
    }   

    public void Update()
    {
        if(psm != null)
        {
            psm.Update();
        }
    }
}
