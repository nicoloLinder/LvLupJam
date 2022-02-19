using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthContainer : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts;
    private int _health = 3;

    public bool RemoveHealth()
    {
        _health -= 1;
        
        if (_health >= 0)
        {
            hearts[_health].SetActive(false);
        }

        return _health > 0;
    }
}
