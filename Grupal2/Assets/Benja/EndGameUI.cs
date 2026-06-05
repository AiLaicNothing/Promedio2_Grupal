using TMPro;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text resultText;

    private void Start()
    {
        panel.SetActive(false);
        GameManager.OnGameFinished += ShowResult;
    }

    private void OnDestroy()
    {
        GameManager.OnGameFinished -= ShowResult;
    }

    private void ShowResult(bool won)
    {
        panel.SetActive(true);

        resultText.text = won ? "ganate" : "perdite";
    }
}