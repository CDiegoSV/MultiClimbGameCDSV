using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSGameManager : MonoBehaviour
{
    #region Enums

    public enum GameStates { WAITINGFORPLAYERS, GAME, VICTORY, DEATH, NONE}

    #endregion

    #region Knobs

    [SerializeField] GameStates currentGameState;

    [SerializeField] bool gameStarted = false;

    #endregion

    public static VSGameManager instance;

    #region References

    PhotonView myPV;

    #endregion

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
        myPV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (LevelNetworkManager.Instance.getCurrentPlayerCount == 1 && gameStarted == false && myPV.IsMine)
        {
            StartPreparationCorutine();
            gameStarted = true;
        }
    }

    #endregion

    #region Public Methods

    public void StartPreparationCorutine()
    {
        StartCoroutine(PreparationCorutine());
    }

    #endregion

    #region Private Methods

    private IEnumerator PreparationCorutine()
    {
        UIManager.Instance.TimerFloat = 3f;
        UIManager.Instance.RunTimerBool = true;
        yield return new WaitForSeconds(UIManager.Instance.TimerFloat);
        currentGameState = GameStates.GAME;
        PlatformManager.Instance.InvokePlatformsMethod(1.7f);
        UIManager.Instance.TimerFloat = 30f;
    }

    #endregion
}
