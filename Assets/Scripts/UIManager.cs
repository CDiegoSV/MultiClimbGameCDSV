using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    #region References

    [SerializeField] TextMeshProUGUI playersNameTextMesh, timerTextMesh;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] GameObject loadingPanel, menuPanel, playPanel, okButton;

    #endregion

    #region Runtime Variables

    private float _timer;
    private bool _runTimer = false;


    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (_runTimer && _timer > 0)
        {
            _timer -= Time.deltaTime;
            timerTextMesh.text = _timer.ToString();
        }
    }

    #endregion

    #region OnClick Methods
    public void OnClickOkButton()
    {
        if(nameInputField.text != "")
        {
            okButton.SetActive(false);
            nameInputField.gameObject.SetActive(false);
            playersNameTextMesh.gameObject.SetActive(true);
            playersNameTextMesh.text = nameInputField.text;
            PhotonConnection.Instance.SetPlayerNickName(playersNameTextMesh.text);
        }
    }

    public void OnClickPlayButton()
    {
        if(PhotonConnection.Instance.GetPlayerNickName != "")
        {
            menuPanel.SetActive(false);
            playPanel.SetActive(true);
        }
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    public void OnClickVSOrCoopButton(bool playVSMode)
    {
        PhotonConnection.Instance.StartGame(playVSMode);
    }


    public void OnClickBackButton()
    {
        playPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    #endregion

    #region Public Methods
    public void LoadingPanelTransition()
    {
        loadingPanel.SetActive(false);
        menuPanel.SetActive(true);
    }


    #endregion


    #region Getters and Setters

    public float TimerFloat
    {
        get
        {
            return _timer;
        }
        set
        {
            _timer = value;
        }
    }

    public bool RunTimerBool
    {
        get
        {
            return _runTimer;
        }
        set
        {
            _runTimer = value;
        }
    }

    #endregion
}
