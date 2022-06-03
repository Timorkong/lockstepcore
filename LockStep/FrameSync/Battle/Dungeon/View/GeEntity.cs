using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GeEntity
{
    public BeEntity beEntity = null;

    public GeScence geScence = null;

    public GameObject model = null;

    public Animator animator = null;

    protected string curClipName = "";

    public GeEntity(BeEntity beEntity, GeScence geScence)
    {
        this.beEntity = beEntity;

        this.geScence = geScence;

        geScence.geEntities.Add(this);

        Init();
    }

    void Init()
    {
        GameApplaction.Instance.StartCoroutine(IE_LoadPlayer());
    }

    IEnumerator IE_LoadPlayer()
    {
        var op = Addressables.LoadAssetAsync<GameObject>("Char1");

        yield return op;

        var go = op.Result;

        model = GameObject.Instantiate(go);

        model.layer = LayerMask.NameToLayer("Level");

        model.transform.localPosition = beEntity.psm.playerStateData.pos.EncodeVec3();

        animator = model.GetComponent<Animator>();

        if(beEntity.seat == GameApplaction.Instance.playerSeat)
        {
            FollowPlayer.Instance.SetMainPlayer(this);
        }
    }

    public void Update()
    {
        if (model == null || animator == null) return;

        model.transform.localPosition = beEntity.psm.playerStateData.pos.EncodeVec3();

        if (curClipName != beEntity.psm.playerStateData.ClipName)
        {
            if (Global.Setting.ShowPlayerStateLog)
            {
                Debug.LogError(string.Format("GeEntity form {0} to {1}", curClipName, beEntity.psm.playerStateData.ClipName));
            }

            curClipName = beEntity.psm.playerStateData.ClipName;

            animator.Play(curClipName);
        }
    }
}
