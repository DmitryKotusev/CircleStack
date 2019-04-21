using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameStates gameState;
    CylinderManager cylinderManager;
    InputController inputController;
    EndClipPlayer endClipPlayer;

    public CinemachineVirtualCamera cinemachineVirtualCamera;
    public Vector3 cinemachineStartOffset;
    public Vector3 cinemachineFinishOffset;

    private void Start()
    {
        gameState = GameStates.PLAYING;
        cylinderManager = GameObject.FindGameObjectWithTag("CylinderManager").GetComponent<CylinderManager>();
        inputController = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>();
        endClipPlayer = GameObject.FindGameObjectWithTag("EndClipPlayer").GetComponent<EndClipPlayer>();
        CinemachineTransposer cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        cinemachineTransposer.m_FollowOffset = cinemachineStartOffset;
    }

    private void Update()
    {
        switch (gameState)
        {
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
                    endClipPlayer.ClipPlayed += OnClipPlayed;
                    endClipPlayer.SetCylinderManager(cylinderManager);
                    endClipPlayer.SetCinemachineTransposer(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>());
                    endClipPlayer.SetCinemachineFinishOffset(cinemachineFinishOffset);
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
        }
    }

    private void OnClipPlayed()
    {
        endClipPlayer.ClipPlayed -= OnClipPlayed;
        gameState = GameStates.NOT_PLAYING;
        Debug.Log("Game over");
    }

    private void CheckCylinderTowerInput()
    {
        if (cylinderManager.GetCylinderState() != CylinderStates.GAME_OVER)
        {
            if (inputController.leftMouseButtonClick)
            {
                cylinderManager.FixCylinder();
            }
        }
        else
        {
            gameState = GameStates.REQUIRE_PLAYING_END_CLIP;
        }
    }

    private void PlayEndClip()
    {
        endClipPlayer.enabled = true;
    }
}
