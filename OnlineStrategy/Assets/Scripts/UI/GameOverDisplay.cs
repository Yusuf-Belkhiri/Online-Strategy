using System;
using Mirror;
using TMPro;
using UnityEngine;

/// <summary>
/// Completely Client side, it doesn't need to be NetworkBehaviour
/// </summary>
public class GameOverDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverDisplayContainer;
    [SerializeField] private TMP_Text _winnerNameText;

    private void Start()
    {
        GameOverHandler.ClientOnGameOver += ClientHandleGameOver;
    }

    private void OnDestroy()
    {
        GameOverHandler.ClientOnGameOver -= ClientHandleGameOver;
    }

    private void ClientHandleGameOver(string winner)
    {
        _winnerNameText.text = $"{winner} Has Won!";
        _gameOverDisplayContainer.SetActive(true);
    }

    // Button action
    public void ExitGame()
    {
        if (NetworkServer.active && NetworkClient.isConnected)      // case: host
        {
            // Stop hosting
            NetworkManager.singleton.StopHost();
        }
        else        // case: client
        {
            // Stop Client
            NetworkManager.singleton.StopClient();
        }
    }
}
