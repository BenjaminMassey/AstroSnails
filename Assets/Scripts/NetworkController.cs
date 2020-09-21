using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject name_input_text;
    [SerializeField]
    private GameObject server_selection_obj;

    public void StartConnect()
    {
        if (name_input_text.GetComponent<Text>().text.Length == 0)
        {
            return;
        }

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Reconnect();
        }
        //if (!PhotonNetwork.IsConnected)
        else
        {
            Dropdown dd = server_selection_obj.GetComponent<Dropdown>();
            string option = dd.options[dd.value].text;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = RegionConvert(option);
            PhotonNetwork.ConnectUsingSettings();
        }
        GameObject.Find("ButtonText").GetComponent<Text>().text = "Connecting...";
        Debug.Log("Connecting...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the " + PhotonNetwork.CloudRegion + " server!");

        string name = name_input_text.GetComponent<Text>().text;
        ExitGames.Client.Photon.Hashtable properties =
                PhotonNetwork.LocalPlayer.CustomProperties;
        if (properties.ContainsKey("player_name"))
        {
            properties["player_name"] = name;
        }
        else
        {
            properties.Add("player_name", name);
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);//
    }

    private string RegionConvert(string raw)
    {
        string converted = "";
        switch (raw)
        {
            case "US (West)":
                converted = "usw";
                break;
            case "US (East)":
                converted = "us";
                break;
            case "Asia":
                converted = "asia";
                break;
            case "Australia":
                converted = "au";
                break;
            case "Canada":
                converted = "cae";
                break;
            case "China":
                converted = "cn";
                break;
            case "Europe":
                converted = "eu";
                break;
            case "India":
                converted = "in";
                break;
            case "Japan":
                converted = "jp";
                break;
            case "Russia":
                converted = "ru";
                break;
            case "Russia (East)":
                converted = "rue";
                break;
            case "South Africa":
                converted = "za";
                break;
            case "South America":
                converted = "sa";
                break;
            case "South Korea":
                converted = "kr";
                break;
            default:
                converted = "usw";
                break;
        }
        return converted;
    }

    private void OnApplicationQuit()
    {
        PhotonNetwork.Disconnect();
    }
}
