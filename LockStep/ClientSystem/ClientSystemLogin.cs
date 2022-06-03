using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSystemLogin : ClientSystem
{
    

    public override void OnStart(SystemContent systemContent)
    {
        base.OnStart(systemContent);

        

        NetManager.Instance.Connect2Server(Global.Setting.ip, Global.Setting.port, 10000);
    }
}
