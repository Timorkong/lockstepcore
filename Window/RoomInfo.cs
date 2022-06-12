using UnityEngine;
using UnityEngine.UI;

public class RoomInfo : SingleWindow<RoomInfo>
{
    public GameObject UserPrefab;

    public GameObject UserParent;

    public Text RoomName;

    protected override void Awake()
    {
        base.Awake();

        UserPrefab.SetActive(false);
    }

    public void Refresh(PROTOCOLCOMMON.RoomInfo room_info)
    {
        Util.DestroyAllChildren(UserParent);

        RoomName.text = room_info.RoomName;

        foreach (var userInfo in room_info.UserList)
        {
            GameObject prefab = GameObject.Instantiate(UserPrefab, UserParent.transform);

            prefab.SetActive(true);

            Text UserName = prefab.transform.FindNode<Text>("UserName/UserName");

            UserName.text = userInfo.UserName;
        }
    }

    public override void Request()
    {
        base.Request();
    }

    public void OnClickClose()
    {
        Hide();

        command_req.LeaveRoomReq();

        RoomList.Instance.Show();
    }

    public void OnClickEnterGame()
    {
        command_req.EnterGameReq();

        Hide();

        Loading.Instance.Show();
    }
}
