using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EndClipPlayer : MonoBehaviour
{
    public float cameraTargetSpeed;
    // x = basicCameraDistance / basicHeight * currentTowerHeight
    public float basicCameraDistance;
    public float basicHeight;
    public event System.Action ClipPlayed;

    private Vector3 newCameraTargetPosition;
    private Transform cameraTargetTransform;
    private CylinderManager cylinderManager;
    private CinemachineTransposer cinemachineTransposer;
    private float basicCameraDistanceBasicHeightRelation = 3f;
    private Vector3 cinemachineStartOffset;
    private Vector3 cinemachineFinishOffset;
    void OnEnable()
    {
        basicCameraDistanceBasicHeightRelation = basicCameraDistance / basicHeight;
        CountNewCameraTargetPosition();
        cinemachineTransposer.m_FollowOffset = cinemachineFinishOffset;
    }

    private void Update()
    {
        MoveCameraTarget();
        if (cameraTargetTransform.position == newCameraTargetPosition)
        {
            FinishPlay();
        }
    }

    void CountNewCameraTargetPosition()
    {
        Transform mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        
        float currentTowerHeight = cylinderManager.GetCurrentTowerHeight();

        cameraTargetTransform = cylinderManager.GetCameraTargetTransform();

        Vector3 requiredDirection = (mainCameraTransform.position - new Vector3(
            cylinderManager.startPosition.x,
            mainCameraTransform.position.y,
            cylinderManager.startPosition.z
            )).normalized;

        float requiredDistance = basicCameraDistanceBasicHeightRelation * currentTowerHeight;
        newCameraTargetPosition = new Vector3(
            cylinderManager.startPosition.x,
            currentTowerHeight / 2,
            cylinderManager.startPosition.z
            ) + requiredDirection * requiredDistance;
    }

    void MoveCameraTarget()
    {
        cameraTargetTransform.position
            = Vector3.MoveTowards(cameraTargetTransform.position, newCameraTargetPosition, cameraTargetSpeed * Time.deltaTime);
    }

    public void SetCinemachineTransposer(CinemachineTransposer cinemachineTransposer)
    {
        this.cinemachineTransposer = cinemachineTransposer;
    }

    public void SetCinemachineStartOffset(Vector3 cinemachineStartOffset)
    {
        this.cinemachineStartOffset = cinemachineStartOffset;
    }

    public void SetCinemachineFinishOffset(Vector3 cinemachineFinishOffset)
    {
        this.cinemachineFinishOffset = cinemachineFinishOffset;
    }

    public void SetCylinderManager(CylinderManager cylinderManager)
    {
        this.cylinderManager = cylinderManager;
    }

    void FinishPlay()
    {
        ClipPlayed?.Invoke();
        enabled = false;
    }
}
