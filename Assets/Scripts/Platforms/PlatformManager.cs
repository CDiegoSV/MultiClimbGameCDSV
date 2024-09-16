using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviourPunCallbacks
{
    #region References

    [SerializeField] GameObject prefabPlatform;
    PhotonView myPV;

    #endregion

    #region Unity Methods

    private void Start()
    {
        myPV = GetComponent<PhotonView>();

        myPV.RPC("InvokeRPCMethod", RpcTarget.AllBuffered, 2f);
    }

    #endregion

    #region RPC Methods

    [PunRPC]
    public void InvokeRPCMethod(float repeatInSeconds)
    {
        InvokeRepeating("PlatformSpawn", repeatInSeconds, repeatInSeconds);
    }
    public void PlatformSpawn()
    {
        Instantiate(prefabPlatform, new Vector3(UnityEngine.Random.Range(-5, 5), gameObject.transform.position.y), Quaternion.identity, gameObject.transform);
    }

    #endregion
}
