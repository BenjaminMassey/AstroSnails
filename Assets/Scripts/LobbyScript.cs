using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject first_screen;
    [SerializeField]
    private GameObject second_screen;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        first_screen.SetActive(false);
        second_screen.SetActive(true);
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        GameObject content = GameObject.Find("Content");
        Rect cr = content.GetComponent<RectTransform>().rect;
        content.GetComponent<RectTransform>().rect.Set(cr.x, cr.y, cr.width, roomList.Count * 100);

        GameObject scroll_button = GameObject.Find("ScrollButton1");

        if (roomList.Count == 0)
        {
            Destroy(scroll_button);
            return;
        }

        int y = 200;
        for (int i = 0; i < roomList.Count; i++)
        {
            Rect br = scroll_button.GetComponent<RectTransform>().rect;
            scroll_button.GetComponent<RectTransform>().rect.Set(br.x, br.y, br.width, y);
            scroll_button.transform.GetChild(0).GetComponent<Text>().text = roomList[i].Name;
            if (i != roomList.Count - 1)
            {
                scroll_button = Instantiate(scroll_button);
                scroll_button.name = "ScrollButton" + (i + 2);
            }
            y -= 100;
        }
        
    }
}
