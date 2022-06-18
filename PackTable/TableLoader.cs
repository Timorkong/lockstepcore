using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;

public delegate void OnLoad(byte[] data);

public class TableDesc
{
    public string tableName;

    public TableDesc(string tableName)
    {
        this.tableName = tableName;
    }
}

public class TableLoader : MonoSingleton<TableLoader>
{
    public List<TableDesc> tables = new List<TableDesc>
    {
        new TableDesc("AVATAR"),
    };

    public IEnumerator LoadTables()
    {
        foreach (var desc in tables)
        {
            var op1 = Addressables.LoadAssetAsync<UnityEngine.TextAsset>(desc.tableName);
            yield return op1;
            // Get TableManager Instance
            string tableManName = desc.tableName + "TableManager";
            Type type = Type.GetType(tableManName);
            if (type == null)
            {
                Debug.LogError(tableManName + " is Not Defined!");
                continue;
            }
            PropertyInfo pinfo = null;
            while (type != null)
            {
                pinfo = type.GetProperty("Instance");

                if (pinfo != null)
                {
                    break;
                }

                type = type.BaseType;
            }

            if (pinfo == null)
            {
                continue;

            }

            MethodInfo instMethod = pinfo.GetGetMethod();
            if (instMethod == null)
            {
                continue;
            }

            System.Object tblManInst = instMethod.Invoke(null, null);
            if (tblManInst == null)
            {
                continue;
            }
            Delegate dele = Delegate.CreateDelegate(typeof(byte[]), tblManInst, "OnResourceLoaded");
            var func = (OnLoad)dele;
            func(op1.Result.bytes);
        }
    }
}
