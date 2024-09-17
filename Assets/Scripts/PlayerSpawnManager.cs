using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager instance;

    PhotonView myPV;

    #region Unity Methods

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        GameObject playerInstance = PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity);
        TextMeshProUGUI nameTextMesh = playerInstance.GetComponent<PlayerController>().playerNameTextMesh;
        //myPV.RPC("SetNickName", RpcTarget.AllBuffered, nameTextMesh);
        playerInstance.GetComponentInChildren<TextMeshProUGUI>().text = PhotonNetwork.NickName;
    }

    //[PunRPC] 
    //public void SetNickName(TextMeshProUGUI playerTextMesh)
    //{
    //    playerTextMesh.text = myPV.Owner.NickName;
    //}

    #endregion
}
