using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoSingleton<FollowPlayer>
{
    private GeEntity geEntity = null;

    private FixedVec3 targetPos = null; 

    Vector3 pos = new Vector3();

    protected override void Awake()
    {
        base.Awake();

        pos.z = transform.position.z;
    }

    public void SetMainPlayer(GeEntity geEntity)
    {
        this.geEntity = geEntity;

        this.targetPos = geEntity.beEntity.psm.playerStateData.pos;
    }

    private void Update()
    {
        if (geEntity == null) return;

        pos.x = targetPos.x.rawVal;
        pos.y = targetPos.y.rawVal;
        transform.position = pos;
    }
}
