using TMPro;
using UnityEngine;

public class GameOverUI : UI
{
    [SerializeField] private TMP_Text text;

    public void SetWinnerName(string winner)
    {
        text.text = $"winner is {winner}";
    }
}