using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndMenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtResult;

    void Start()
    {
        if (GameWinner.isWin) txtResult.text = GameWinner.WinPlayer.PlayerID == 0 ? "Player One Win" : "Player Two Win";

        txtResult.transform.DOShakePosition(3f, 10f, 10);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
