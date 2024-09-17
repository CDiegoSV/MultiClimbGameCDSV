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
        
    }

    private void Update()
    {
        if (LevelNetworkManager.Instance.getCurrentPlayerCount == 2 && gameStarted == false)
        {
            VSGameManager.instance.StartPreparationCorutine();
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
        PlatformManager.Instance.StartPlatformSpawnForAllPlayers();
        UIManager.Instance.TimerFloat = 30f;
    }

    #endregion
}
