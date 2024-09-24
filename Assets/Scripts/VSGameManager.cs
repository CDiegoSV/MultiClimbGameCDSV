using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VSGameManager : MonoBehaviour
{
    #region Enums

    public enum GameStates { NONE, WAITINGFORPLAYERS, GAME, GAME_FINISHED}

    #endregion

    #region Knobs

    [SerializeField] GameStates currentGameState;

    [SerializeField] bool gameStarted = false;

    #endregion

    public static VSGameManager instance;

    #region References

    [SerializeField] PhotonView myPV;
    [SerializeField] PlatformBehaviour[] platforms;

    #endregion

    #region RunTime Variables

    private int _alivePlayers;

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
        currentGameState = GameStates.WAITINGFORPLAYERS;
    }

    private void Update()
    {
        
        switch(currentGameState)
        {
            case GameStates.WAITINGFORPLAYERS:
                if (LevelNetworkManager.Instance.getCurrentPlayerCount == 2 && gameStarted == false && myPV.IsMine)
                {
                    StartPreparationCorutine();
                    gameStarted = true;
                }
                break;

            case GameStates.GAME:
                if(UIManager.Instance.TimerFloat <= 0)
                {
                    if(_alivePlayers != 1)
                    {
                        UIManager.Instance.DisplayWinner("Draw");
                    }
                    StartCoroutine(FinishedGameCorutine());
                    currentGameState = GameStates.GAME_FINISHED;
                }
                else if(_alivePlayers == 1)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    UIManager.Instance.DisplayWinner(player.GetPhotonView().Owner.NickName + " Wins");
                    StartCoroutine(FinishedGameCorutine());
                    currentGameState = GameStates.GAME_FINISHED;
                }
                break;

        }
    }

    #endregion

    #region Public Methods

    public void StartPreparationCorutine()
    {
        StartCoroutine(PreparationCorutine());
    }

    public void DecreaseAlivePlayersForAllPlayers()
    {
        myPV.RPC("DecreaseAlivePlayersInt", RpcTarget.All);
    }

    #endregion

    #region Private Methods

    private IEnumerator FinishedGameCorutine()
    {
        yield return new WaitForSeconds(10f);
        LevelNetworkManager.Instance.DisconnectCurrentRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    private IEnumerator PreparationCorutine()
    {
        UIManager.Instance.CallRPCTimerMethod(3f);
        yield return new WaitForSeconds(3f);
        myPV.RPC("SetCurrentAlivePlayersInScene", RpcTarget.All);
        myPV.RPC("SetGameStateForAllPlayers", RpcTarget.All, GameStates.GAME);
        Invoke("EnablePlatformScripts", 2f);
        PlatformManager.Instance.InvokePlatformsMethod(1.7f);
        UIManager.Instance.CallRPCTimerMethod(60f);
    }

    private void EnablePlatformScripts()
    {
        foreach (PlatformBehaviour platformBehaviour in platforms)
        {
            platformBehaviour.enabled = true;
        }
    }

    [PunRPC]
    private void SetGameStateForAllPlayers(GameStates gameStateToSet)
    {
        currentGameState = gameStateToSet;
    }

    [PunRPC]
    private void SetCurrentAlivePlayersInScene()
    {
        _alivePlayers = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    [PunRPC]
    private void DecreaseAlivePlayersInt()
    {
        currentAlivePlayers--;
    }
    #endregion

    #region Getters And Setters

    public GameStates CurrentGameState
    {
        get { return currentGameState; }
    }

    public int currentAlivePlayers
    {
        get { return _alivePlayers; }
        set { _alivePlayers = value; }
    }

    #endregion
}
