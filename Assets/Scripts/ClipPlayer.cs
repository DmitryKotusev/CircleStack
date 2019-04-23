using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ClipPlayer : MonoBehaviour
{
    public float cameraTargetSpeed;
    // x = basicCameraDistance / basicHeight * currentTowerHeight
    public float basicCameraDistance;
    public float basicHeight;
    public float thresholdCameraDistance = 17;
    public event System.Action ClipPlayed;

    private Vector3 newCameraTransposerFollowOffset;
    private Transform cameraTargetTransform;
    private CylinderManager cylinderManager;
    private CinemachineTransposer cinemachineTransposer;
    private float basicCameraDistanceBasicHeightRelation = 3f;
    private Vector3 cinemachineStartOffset;
    void OnEnable()
    {
        basicCameraDistanceBasicHeightRelation = basicCameraDistance / basicHeight;
        CountNewCameraTargetPosition();
        // cinemachineTransposer.m_FollowOffset = cinemachineFinishOffset;
    }

    private void Update()
    {
        ChangeCameraTransposerFollowOffset();
        if (cinemachineTransposer.m_FollowOffset == newCameraTransposerFollowOffset)
        {
            FinishPlay();
        }
    }

    void CountNewCameraTargetPosition()
    {
        Transform mainCameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;

        float currentTowerHeight = cylinderManager.GetCurrentTowerHeight();

        cameraTargetTransform = cylinderManager.GetCameraTargetTransform();

        //Vector3 requiredDirection = (mainCameraTransform.position - new Vector3(
        //    cylinderManager.startPosition.x,
        //    mainCameraTransform.position.y,
        //    cylinderManager.startPosition.z
        //    )).normalized;

        Vector3 requiredDirection = cinemachineTransposer.m_FollowOffset.normalized;

        float requiredDistance = basicCameraDistanceBasicHeightRelation * currentTowerHeight;
        if (requiredDistance < thresholdCameraDistance)
        {
            requiredDistance = thresholdCameraDistance;
        }

        newCameraTransposerFollowOffset = requiredDirection * requiredDistance;
    }

    void ChangeCameraTransposerFollowOffset()
    {
        cinemachineTransposer.m_FollowOffset
            = Vector3.MoveTowards(cinemachineTransposer.m_FollowOffset,
            newCameraTransposerFollowOffset,
            cameraTargetSpeed * Time.deltaTime);
    }

    public void SetCinemachineTransposer(CinemachineTransposer cinemachineTransposer)
    {
        this.cinemachineTransposer = cinemachineTransposer;
    }

    public void SetCinemachineStartOffset(Vector3 cinemachineStartOffset)
    {
        this.cinemachineStartOffset = cinemachineStartOffset;
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
