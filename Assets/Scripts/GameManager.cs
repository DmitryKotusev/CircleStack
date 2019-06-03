using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameStates gameState;
    [SerializeField]
    int maxScore;
    CylinderManager cylinderManager;
    InputController inputController;
    ClipPlayer endClipPlayer;
    ClipPlayer restartClipPlayer;
    BackGroundColorChanger backGroundColorChanger;
    FileDataController fileDataController;
    InGameCashCounter inGameCashCounter;

    public GameObject roofContainer;
    public ParticleSystem backGroundParticles;
    public Material backGroundMaterial;
    public Text cylinderScore;
    public Text currentTry;
    public Text bestScore;
    public Text earnedCash;
    public Text globalCash;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public Vector3 cinemachineStartOffset;
    public GameObject restartButton;
    public GameObject startButton;
    public GameObject cylinderScoreGameObject;
    public GameObject appNameGameObject;
    public GameObject tutorialButtonContainer;
    public GameObject bottomButtonsContainer;
    public GameObject cashButtonContainer;

    public void RestartGame()
    {
        restartButton.SetActive(false);
        gameState = GameStates.REQUIRE_RESTART;
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        gameState = GameStates.REQUIRE_START_PLAY;
    }

    private void Start()
    {
        InitCylinderManagerStartSettings();

        inputController = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>();
        endClipPlayer = GameObject.FindGameObjectWithTag("EndClipPlayer").GetComponent<ClipPlayer>();
        restartClipPlayer = GameObject.FindGameObjectWithTag("RestartClipPlayer").GetComponent<ClipPlayer>();

        InitBackGroundColorChangerStartSettings();

        fileDataController = GetComponent<FileDataController>();

        inGameCashCounter = GetComponent<InGameCashCounter>();
        InitInGameCashCounterStartSettings();

        InitMaxScoreStartSettings();
        InitCinemachineStartSettings();

        InitRoofStartSettings();

        SynchronizeUICashWithStorageData();
    }

    private void SynchronizeUICashWithStorageData()
    {
        globalCash.text = fileDataController.ReadCurrencyAmount().ToString();
    }

    private void InitInGameCashCounterStartSettings()
    {
        cylinderManager.CylinderFixedAccurately += inGameCashCounter.OnCylinderFixedAccurately;
        cylinderManager.CylinderFixedNotAccurately += inGameCashCounter.OnCylinderFixedNotAccurately;
        inGameCashCounter.SetCylinderManager(cylinderManager);
    }

    private void InitRoofStartSettings()
    {
        roofContainer.GetComponent<RoofContainer>().ClipPlayed += OnFallRoofClipPlayed;
    }

    private void InitCinemachineStartSettings()
    {
        CinemachineTransposer cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        cinemachineTransposer.m_FollowOffset = cinemachineStartOffset;
    }

    private void InitMaxScoreStartSettings()
    {
        maxScore = fileDataController.ReadMaxScore();
        bestScore.text = "Best score: " + maxScore;
    }

    private void InitBackGroundColorChangerStartSettings()
    {
        backGroundColorChanger = GameObject.FindGameObjectWithTag("BackGround").GetComponent<BackGroundColorChanger>();
        backGroundColorChanger.SetBackGroundMaterial(backGroundMaterial);
        backGroundColorChanger.SetBackGroundFixedStartColor();
    }

    private void InitCylinderManagerStartSettings()
    {
        cylinderManager = GameObject.FindGameObjectWithTag("CylinderManager").GetComponent<CylinderManager>();
        cylinderManager.GameOver += OnCylinderManagerGameOver;
    }

    private void Update()
    {
        ShowUIInfo();
        switch (gameState)
        {
            case GameStates.APP_STARTED:
                {
                    StartTower();
                    roofContainer.SetActive(false);
                    gameState = GameStates.NOT_PLAYING;
                    break;
                }
            case GameStates.REQUIRE_START_PLAY:
                {
                    gameState = GameStates.REQUIRE_PLAYING_START_CLIP;
                    roofContainer.SetActive(false);
                    break;
                }
            case GameStates.REQUIRE_PLAYING_START_CLIP:
                {
                    restartClipPlayer.ClipPlayed += OnStartClipPlayed;
                    restartClipPlayer.SetCylinderManager(cylinderManager);
                    restartClipPlayer.SetCinemachineTransposer(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>());
                    restartClipPlayer.SetCinemachineStartOffset(cinemachineStartOffset);
                    PlayRestartClip();
                    cylinderScore.enabled = false;
                    gameState = GameStates.PLAYING_START_CLIP;
                    break;
                }
            case GameStates.PLAYING_START_CLIP:
                {
                    break;
                }
            case GameStates.REQUIRE_START_CYLINDER_MANAGER:
                {
                    cylinderScore.enabled = true;
                    cylinderManager.Init();
                    inGameCashCounter.InitCounter();
                    gameState = GameStates.PLAYING;
                    break;
                }
            case GameStates.PLAYING:
                {
                    CheckCylinderTowerInput();
                    break;
                }
            case GameStates.PAUSE:
                {
                    break;
                }
            case GameStates.REQUIRE_PLAYING_END_CLIP:
                {
                    endClipPlayer.ClipPlayed += OnEndClipPlayed;
                    endClipPlayer.SetCylinderManager(cylinderManager);
                    endClipPlayer.SetCinemachineTransposer(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>());
                    endClipPlayer.SetCinemachineStartOffset(cinemachineStartOffset);
                    PlayEndClip();
                    gameState = GameStates.PLAYING_END_CLIP;
                    break;
                }
            case GameStates.PLAYING_END_CLIP:
                {
                    break;
                }
            case GameStates.NOT_PLAYING:
                {
                    break;
                }
            case GameStates.REQUIRE_PLAYING_RESTART_CLIP:
                {
                    restartClipPlayer.ClipPlayed += OnRestartClipPlayed;
                    restartClipPlayer.SetCylinderManager(cylinderManager);
                    restartClipPlayer.SetCinemachineTransposer(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>());
                    restartClipPlayer.SetCinemachineStartOffset(cinemachineStartOffset);
                    PlayRestartClip();
                    cylinderScore.enabled = false;
                    gameState = GameStates.PLAYING_RESTART_CLIP;
                    break;
                }
            case GameStates.PLAYING_RESTART_CLIP:
                {
                    break;
                }
            case GameStates.REQUIRE_RESTART_CYLINDER_MANAGER:
                {
                    cylinderScore.enabled = true;
                    cylinderManager.Init();
                    inGameCashCounter.InitCounter();
                    gameState = GameStates.PLAYING;
                    break;
                }
            case GameStates.REQUIRE_RESTART:
                {
                    ResetTower();
                    bestScore.enabled = false;
                    earnedCash.enabled = false;
                    roofContainer.SetActive(false);
                    gameState = GameStates.REQUIRE_PLAYING_RESTART_CLIP;
                    break;
                }
        }
    }

    private void ShowUIInfo()
    {
        cylinderScore.text = cylinderManager.GetCylinderAmount() >= 0 ? cylinderManager.GetCylinderAmount().ToString() : "0";
        currentTry.text = cylinderManager.GetCurrentTry().ToString();
    }

    private void OnEndClipPlayed()
    {
        endClipPlayer.ClipPlayed -= OnEndClipPlayed;
        restartButton.SetActive(true);
        gameState = GameStates.NOT_PLAYING;
    }

    private void OnRestartClipPlayed()
    {
        restartClipPlayer.ClipPlayed -= OnRestartClipPlayed;
        gameState = GameStates.NOT_PLAYING;
        startButton.SetActive(true);
        cylinderScoreGameObject.SetActive(false);
        appNameGameObject.SetActive(true);
        tutorialButtonContainer.SetActive(true);
        bottomButtonsContainer.SetActive(true);
    }

    private void OnStartClipPlayed()
    {
        restartClipPlayer.ClipPlayed -= OnStartClipPlayed;
        gameState = GameStates.REQUIRE_START_CYLINDER_MANAGER;
        cylinderScoreGameObject.SetActive(true);
        backGroundColorChanger.StartChanger();
        backGroundParticles.Play();
    }

    private void ResetTower()
    {
        cylinderManager.ResetDefaultCylinderColor();
        cylinderManager.CleanTower();
        cylinderManager.ResetCameraTargetPosition();
    }

    private void StartTower()
    {
        cylinderManager.SetDefaultCylinderColor();
        cylinderManager.CleanTower();
        cylinderManager.ResetCameraTargetPosition();
    }

    private void CheckCylinderTowerInput()
    {
        if (inputController.leftMouseButtonTouchClick)
        {
            cylinderManager.FixCylinder();
        }
    }

    private void OnCylinderManagerGameOver()
    {
        gameState = GameStates.PLAYING_ROOF_FALLING_CLIP;
        roofContainer.SetActive(true);
    }

    private void OnFallRoofClipPlayed()
    {
        gameState = GameStates.REQUIRE_PLAYING_END_CLIP;
        backGroundColorChanger.StopChanger();
        backGroundParticles.Stop();
        int currentScore = cylinderManager.GetCylinderAmount();
        if (currentScore > maxScore)
        {
            // Saving max score
            fileDataController.SaveMaxScore(currentScore);
            maxScore = currentScore;
            bestScore.text = "Best score: " + maxScore;
        }
        bestScore.enabled = true;

        // Saving cash
        earnedCash.text = "Earned cash: " + inGameCashCounter.GetCurrentCash();
        // Save to local storage and global cash UI update required
        AddEarnedCashToGlobal(inGameCashCounter.GetCurrentCash());
        // Here it is
        cashButtonContainer.SetActive(true);
        earnedCash.enabled = true;
    }

    private void OnChangeCylinderColor()
    {
        backGroundMaterial.color = cylinderManager.GetColorGenerator().GetReverseColor();
    }

    private void PlayEndClip()
    {
        endClipPlayer.enabled = true;
    }

    private void PlayRestartClip()
    {
        restartClipPlayer.enabled = true;
    }

    private void AddEarnedCashToGlobal(float earnedCash)
    {
        fileDataController.AddCurrencyAmount(earnedCash);
        SynchronizeUICashWithStorageData();
    }
}
