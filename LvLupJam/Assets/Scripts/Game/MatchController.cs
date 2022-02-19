using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchController : MonoBehaviour
{
    [SerializeField] private GameObject wasdCar;
    [SerializeField] private GameObject udlrCar;

    [SerializeField] private UI mainMenu;
    [SerializeField] private UI gameMenu;
    [SerializeField] private GameOverUI gameOverMenu;
    
    private static MatchController _instance;
    private List<CarController> _carControllers;

    public static MatchController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MatchController>();
                if (_instance == null)
                {
                    throw new Exception();
                }
            }
            return _instance;
        }
    }

    private void Start()
    {
        _carControllers = new List<CarController>();
        Reset();
    }

    void GameOver()
    {
        gameOverMenu.SetWinnerName(_carControllers[0].name);
        gameOverMenu.Show();
        mainMenu.Hide();
        gameMenu.Hide();
    }

    public void Reset()
    {
        ClearGame();
        gameOverMenu.Hide();
        mainMenu.Show();
        gameMenu.Hide();
    }

    public void StartGame()
    {
        Reset();
        SpawnCars();
        mainMenu.Hide();
        gameMenu.Show();
        gameOverMenu.Hide();
    }

    private void ClearGame()
    {
        foreach (var carController in _carControllers)
        {
            carController.SetForDeletion();
        }
        
        _carControllers.Clear();
    }

    private void SpawnCars()
    {
        _carControllers.Add(Instantiate(wasdCar, Vector3.right*2, Quaternion.identity).GetComponent<CarController>());
        _carControllers.Add(Instantiate(udlrCar, Vector3.left*2, Quaternion.identity).GetComponent<CarController>());
    }

    public void ReportCarDead(CarController carController)
    {
        _carControllers.Remove(carController);
        if (_carControllers.Count == 1)
        {
            GameOver();
        }
    }
}
