using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonConnection : MonoBehaviourPunCallbacks
{
    public static PhotonConnection Instance { get; private set; }

    bool playVSMode = false;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Se ha conectado al Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        print("Ha entrado al lobby Ab");
        if(GetPlayerNickName != "")
        {
            UIManager.Instance.OnClickOkButton();
        }
        UIManager.Instance.LoadingPanelTransition();
    }

    public override void OnJoinedRoom()
    {
        print("Entro a Room: " + PhotonNetwork.CurrentRoom.Name);
        if(playVSMode == true)
        {
            PhotonNetwork.LoadLevel("VSGame");
        }
        else
        {
            PhotonNetwork.LoadLevel("CoopGame");
        }
    }

    //public override void OnCreateRoomFailed(short returnCode, string message)
    //{
    //    base.OnCreateRoomFailed(returnCode, message);
    //    print("Error al crear Room: " + message);
    //    m_warningTextMesh.text = "Error al crear Room: " + message;
    //}

    //public override void OnJoinRoomFailed(short returnCode, string message)
    //{
    //    base.OnJoinRoomFailed(returnCode, message);
    //    print("Error al intentar unirse al Room: " + message);
    //    m_warningTextMesh.text = "Error al intentar unirse al Room: " + message;
    //}

    public void SetPlayerNickName(string nickName)
    {
        PhotonNetwork.NickName = nickName;
    }

    public string GetPlayerNickName
    {
        get
        {
            return PhotonNetwork.NickName;
        }
    }

    public void StartGame(bool playVSModeBool)
    {
        PhotonNetwork.JoinRandomOrCreateRoom(null, 4, MatchmakingMode.FillRoom, null, null, null, newRoomInfo());
        playVSMode = playVSModeBool; 
    }

    RoomOptions newRoomInfo()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        return roomOptions;
    }

    //public void joinRoom()
    //{
    //    if (m_roomInputField.text != "" && m_playerNickNameInputField.text != "")
    //    {
    //        PhotonNetwork.NickName = m_playerNickNameInputField.text;
    //        PhotonNetwork.JoinRoom(m_roomInputField.text);
    //    }
    //    else
    //    {
    //        m_warningTextMesh.text = "Los campos no pueden estar vacios.";
    //    }

    //}

    //public void createRoom()
    //{
    //    if (m_roomInputField.text != "" && PhotonNetwork.NickName != "")
    //    {
    //        PhotonNetwork.CreateRoom(m_roomInputField.text, newRoomInfo(), null);
    //    }
    //    else
    //    {
    //        m_warningTextMesh.text = "Los campos no pueden estar vacios.";
    //    }
    //}
}
