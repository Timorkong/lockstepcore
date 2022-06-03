using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GlobalSetting", order = 1)]
public class GlobalSetting : ScriptableObject
{
    public EnumClientSystem startSystem = EnumClientSystem.Login;

    public int TestLevel;

    public bool ShowNetWorkLog = false;

    public bool ShowSequence = false;

    public bool ShowPlayerStateLog = false;

    public EnumSyncMode SyncMode = EnumSyncMode.SyncFrame;

    public string ip = "127.0.0.1";

    public int port = 3333;
}
