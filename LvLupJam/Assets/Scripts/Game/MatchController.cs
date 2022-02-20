using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MatchController : MonoBehaviour
{
    [SerializeField] private GameObject wasdCar;
    [SerializeField] private GameObject udlrCar;

    [SerializeField] private UI mainMenu;
    [SerializeField] private UI gameMenu;
    [SerializeField] private RoundUI roundMenu;
    [SerializeField] private GameOverUI gameOverMenu;
    [SerializeField] private Animator cameraAnimator;

    private static MatchController _instance;
    private List<CarController> _carControllers;
    
   [SerializeField] private List<Transform> demoCarControllers;
   
   [SerializeField] private AudioSource crashFinalAudio;
    

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

    private void ToggleDemo(bool active)
    {
        foreach (Transform demoCarController in demoCarControllers)
        {
            demoCarController.gameObject.SetActive(active);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        _carControllers = new List<CarController>();
        Reset();
    }

    void RoundOver()
    {
        crashFinalAudio.Play();
        cameraAnimator.SetTrigger("ZoomOut");
        ActivateUI(UIsAvailable.RoundMenu);
        roundMenu.SetRoundWinner(_carControllers[0].PlayerNumber);
    }

    public void GameOver(int winner)
    {
        gameOverMenu.SetWinnerName($"Player {winner}");
        ActivateUI(UIsAvailable.GameOverMenu);
        ToggleDemo(true);
    }

    public void Reset()
    {
        ClearGame();
        ActivateUI(UIsAvailable.MainMenu);
    }

    public void StartGame()
    {
        cameraAnimator.SetTrigger("ZoomIn");
        Reset();
        SpawnCars();
        ActivateUI(UIsAvailable.GameMenu);
        Dome.Instance.DomeStart();
        ToggleDemo(false);
    }
    
    public enum UIsAvailable
    {
        MainMenu,
        GameMenu,
        RoundMenu,
        GameOverMenu
    }

    public void ActivateUI(UIsAvailable uIsAvailable)
    {
        mainMenu.Hide();
        gameMenu.Hide();
        gameOverMenu.Hide();
        roundMenu.Hide();
        
        switch (uIsAvailable)
        {
            case UIsAvailable.MainMenu:
                mainMenu.Show();
                break;
            case UIsAvailable.GameMenu:
                gameMenu.Show();
                break;
            case UIsAvailable.RoundMenu:
                roundMenu.Show();
                break;
            case UIsAvailable.GameOverMenu:
                gameOverMenu.Show();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(uIsAvailable), uIsAvailable, null);
        }
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
            RoundOver();
        }
    }

   
}
