using Cmd.ID;
using UnityEngine;
using UnityEngine.UI;
using Google.Protobuf.Collections;

public class RoomList : SingleWindow<RoomList>
{
    public GameObject RoomPrefab;

    public GameObject RoomParent;

    public override CMD ShowCMD => CMD.RoomListReq;

    protected override void Awake()
    {
        base.Awake();

        this.RoomPrefab.SetActive(false);
    }

    public void Refresh(RepeatedField<PROTOCOLCOMMON.RoomInfo> room_list)
    {
        Util.DestroyAllChildren(RoomParent);

        foreach (var roomInfo in room_list)
        {
            GameObject prefab = GameObject.Instantiate(RoomPrefab, RoomParent.transform);

            prefab.SetActive(true);

            prefab.name = roomInfo.RoomUniqueId.ToString();

            Text RoomName = prefab.transform.FindNode<Text>("RoomName/RoomName");

            RoomName.text = roomInfo.RoomName;

            Button joinRoom = prefab.transform.FindNode<Button>("JoinRoom");

            joinRoom.onClick.AddListener(() => { this.OnClickJoinRoom(roomInfo.RoomUniqueId); });
        }
    }

    public override void Request()
    {
        base.Request();

        command_req.RoomListReq();
    }

    public void OnClickClose()
    {
        Hide();

        Login.Instance.Show();
    }

    public void OnClickCreateRoom()
    {
        Hide();

        command_req.CreateRoomReq();

        RoomInfo.Instance.Show();
    }

    public void OnClickJoinRoom(int room_unique_id)
    {
        Hide();

        command_req.JoinRoomReq(room_unique_id);
    }
}
