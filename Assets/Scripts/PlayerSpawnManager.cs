using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager instance;

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
        playerInstance.GetComponentInChildren<TextMeshProUGUI>().text = PhotonNetwork.NickName;
    }

    #endregion
}
