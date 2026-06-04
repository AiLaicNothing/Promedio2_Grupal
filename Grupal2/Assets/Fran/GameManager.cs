using UnityEngine;

public class GameManager : MonoBehaviour


{
    public static GameManager Instance;

    public bool gameOver = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void WinGame()
    {
        if (gameOver) return;

        gameOver = true;
        Debug.Log("Victoria");
    }

    public void LoseGame()
    {
        if (gameOver) return;

        gameOver = true;
        Debug.Log("Derrota");
    }
}

// Ejemplo de uso:
// GameManager.Instance.WinGame();
// GameManager.Instance.LoseGame();