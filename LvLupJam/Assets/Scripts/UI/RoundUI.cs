using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundUI : UI
{
    [SerializeField] private PlayerWinDisplay _player1WinDisplay;
    [SerializeField] private PlayerWinDisplay _player2WinDisplay;
    
    [SerializeField] private GameObject nextRoundButton;
    [SerializeField] private GameObject gameEndButton;

    [SerializeField] private TMP_Text roundCounter;

    private int _roundNumber;

    public void EndGame()
    {
        if (_player1WinDisplay.IsWinner)
        {
            MatchController.Instance.GameOver(1);
        }
        else
        {
            MatchController.Instance.GameOver(2);
        }
        
        _player1WinDisplay.Reset();
        _player2WinDisplay.Reset();

        _roundNumber = 0;
    }
    public void SetRoundWinner(int playerNumber)
    {
        _roundNumber++;

        roundCounter.text = $"Round {_roundNumber} complete\nPlayer {playerNumber} wins";
        
        nextRoundButton.SetActive(false);
        gameEndButton.SetActive(false);
        if (playerNumber == 1)
        {
            if (_player1WinDisplay.IncrementWinAmount())
            {
                gameEndButton.SetActive(true);
                return;
            }
        }
        else
        {
            if (_player2WinDisplay.IncrementWinAmount())
            {
                gameEndButton.SetActive(true);
                return;
            }
        }
        nextRoundButton.SetActive(true);
    }
}