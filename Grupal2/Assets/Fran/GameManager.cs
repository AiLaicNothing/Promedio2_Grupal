using System;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public static Action<bool> OnGameFinished;

    private bool gameEnded;

    private void Awake()
    {
        Instance = this;
    }

    public void EndGame(Teams winnerTeam)
    {
        if (gameEnded)
            return;

        gameEnded = true;

        FinishGameClientRpc(winnerTeam);
    }

    [ClientRpc]
    private void FinishGameClientRpc(Teams winnerTeam)
    {
        PlayerController[] players =
            FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (PlayerController player in players)
        {
            if (player.IsOwner)
            {
                bool won = player.teamSide.Value == winnerTeam;

                OnGameFinished?.Invoke(won);

                break;
            }
        }
    }
}