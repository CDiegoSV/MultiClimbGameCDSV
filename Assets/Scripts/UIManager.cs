using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    #region References

    [SerializeField] TextMeshProUGUI playersNameTextMesh;
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] GameObject loadingPanel, menuPanel, playPanel, okButton;

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

    #endregion

    #region Public Methods
    public void LoadingPanelTransition()
    {
        loadingPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    #endregion

}
