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
    CylinderManager cylinderManager;
    InputController inputController;
    ClipPlayer endClipPlayer;
    ClipPlayer restartClipPlayer;

    public Text cylinderScore;
    public Text currentTry;
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public Vector3 cinemachineStartOffset;
    public GameObject restartButton;
    public GameObject startButton;
    // public string restartText = "Tap\to\nRestart";

    public void RestartGame()
    {
        restartButton.SetActive(false);
        gameState = GameStates.REQUIRE_RESTART;
    }

    public void StartGame()
    {
        startButton.SetActive(false);
        gameState = GameStates.REQUIRE_START;
    }

    private void Start()
    {
        // gameState = GameStates.PLAYING;
        cylinderManager = GameObject.FindGameObjectWithTag("CylinderManager").GetComponent<CylinderManager>();
        cylinderManager.GameOver += OnCylinderManagerGameOver;
        inputController = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>();
        endClipPlayer = GameObject.FindGameObjectWithTag("EndClipPlayer").GetComponent<ClipPlayer>();
        restartClipPlayer = GameObject.FindGameObjectWithTag("RestartClipPlayer").GetComponent<ClipPlayer>();
        CinemachineTransposer cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        cinemachineTransposer.m_FollowOffset = cinemachineStartOffset;
    }

    private void Update()
    {
        ShowUIInfo();
        switch (gameState)
        {
            case GameStates.APP_STARTED:
                {
                    ResetTower();
                    gameState = GameStates.NOT_PLAYING;
                    break;
                }
            case GameStates.REQUIRE_START:
                {
                    gameState = GameStates.REQUIRE_PLAYING_RESTART_CLIP;
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
                    gameState = GameStates.PLAYING;
                    Debug.Log("Game restarted");
                    break;
                }
            case GameStates.REQUIRE_RESTART:
                {
                    ResetTower();
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
        Debug.Log("Game over");
    }

    private void OnRestartClipPlayed()
    {
        restartClipPlayer.ClipPlayed -= OnRestartClipPlayed;
        gameState = GameStates.REQUIRE_RESTART_CYLINDER_MANAGER;
    }

    private void ResetTower()
    {
        cylinderManager.ResetDefaultCylinderColor();
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
        gameState = GameStates.REQUIRE_PLAYING_END_CLIP;
    }

    private void PlayEndClip()
    {
        endClipPlayer.enabled = true;
    }

    private void PlayRestartClip()
    {
        restartClipPlayer.enabled = true;
    }
}
