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

        Debug.Log("See " + roomList.Count + " available rooms");

        GameObject content = GameObject.Find("Content");
        float csx = content.GetComponent<RectTransform>().sizeDelta.x;
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(csx, Mathf.Max(600, roomList.Count * 100));

        
        GameObject base_button = GameObject.Find("BaseButton");
        base_button.GetComponent<Image>().enabled = true;

        List<GameObject> buttons = new List<GameObject>();

        int y = 200;
        for (int i = 0; i < roomList.Count; i++)
        {
            buttons.Add(Instantiate(base_button));
            buttons[i].name = "ScrollButton" + (i + 1);
            buttons[i].transform.parent = GameObject.Find("Content").transform;
            buttons[i].transform.position = base_button.transform.position;
            buttons[i].transform.localScale = base_button.transform.localScale;
            buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(8.5f, y);
                //.rect.Set(br.x, y, br.width, br.height);
            buttons[i].transform.GetChild(0).GetComponent<Text>().text = roomList[i].Name;

            y -= 100;
        }

        base_button.GetComponent<Image>().enabled = false;
    }
}
