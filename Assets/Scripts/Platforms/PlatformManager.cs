using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviourPunCallbacks
{
    public static PlatformManager Instance;

    #region References

    [SerializeField] GameObject prefabPlatform;
    PhotonView myPV;

    #endregion

    #region Unity Methods

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

    private void Start()
    {
        myPV = GetComponent<PhotonView>();
    }

    #endregion

    #region Public Methods

    //public void StartPlatformSpawnForAllPlayers()
    //{
    //    myPV.RPC("InvokeRPCMethod", RpcTarget.All, 2f);
    //}

    #endregion

    #region Private Methods

    private void PlatformSpawn()
    {
        //Instantiate(prefabPlatform, new Vector3(UnityEngine.Random.Range(-5, 5), gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
        PhotonNetwork.Instantiate("Platform", new Vector3(UnityEngine.Random.Range(-3, 3), gameObject.transform.position.y), Quaternion.identity);
    }

    #endregion

    #region RPC Methods

    
    public void InvokePlatformsMethod(float repeatInSeconds)
    {
        InvokeRepeating("PlatformSpawn", repeatInSeconds, repeatInSeconds);
    }

    #endregion
}
