using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWinDisplay : MonoBehaviour
{
    private int _winAmount = 0;
    private List<Image> _tokens;

    [SerializeField] private Sprite tokenImage;
    [SerializeField] private Sprite winTokenImage;


    public bool IsWinner => _winAmount > 4;

    public bool IncrementWinAmount()
    {
        SetWinCount(++_winAmount);

        return _winAmount > 4;
    }

    public void DecrementWinAmount()
    {
        SetWinCount(--_winAmount);
    }

    public void Reset()
    {
        _winAmount = 0;
        SetWinCount(_winAmount);
    }

    void Awake()
    {
        _tokens = new List<Image>();

        var index = 0;

        foreach (Transform child in transform)
        {
            if (index > 0)
            {
                _tokens.Add(child.GetComponent<Image>());
            }
            else
            {
                index++;
            }
        }

        Reset();
    }

    private void SetWinCount(int winAmount)
    {
        int index = 0;
        foreach (Image token in _tokens)
        {
            token.sprite = index >= winAmount ? tokenImage : winTokenImage;

            index++;
        }
    }
}