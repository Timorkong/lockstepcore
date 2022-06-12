using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class GeScence
{
    public BeScence beScence = null;

    public GeDungeon geDungeon = null;

    public List<GeEntity> geEntities = new List<GeEntity>();

    private bool mCanUpdate = false;

    public GeScence(BeScence beScence, GeDungeon geDungeon)
    {
        this.beScence = beScence;

        this.geDungeon = geDungeon;

        geDungeon.geScence = this;

        mCanUpdate = false;

        Init();
    }

    private void Init()
    {
        GameApplaction.Instance.StartCoroutine(this.IE_LoadScene());
    }

    IEnumerator IE_LoadScene()
    {
        var op = Addressables.LoadSceneAsync(BattleMain.data.LevelName);

        yield return op;

        var scence = op.Result.Scene;

        this.OnSceneLoad(scence);
    }

    public void Update()
    {
        if (mCanUpdate == false) return;

        foreach (var entity in geEntities)
        {
            entity.Update();
        }
    }

    void OnSceneLoad(Scene scene)
    {
        mCanUpdate = true;

        command_req.StartGameReq();
    }
}
