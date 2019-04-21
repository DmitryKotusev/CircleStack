using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderManager : MonoBehaviour
{
    [SerializeField]
    uint cylindersPassed = 1;
    [SerializeField]
    GameObject cameraTarget;

    public Vector3 startPosition = new Vector3(0, 0.5f, 0);
    public Color color;
    public GameObject cylinderPrefab;
    public GameObject internalCircle;
    public float prepareCylinderTime = 1f;
    public float moveUpStep = 1;
    public float minScale;
    public float startMaxScale = 5f;
    public float maxScaleMultiplier = 1.2f;
    public float startScaleSpeed = 1f;
    public float scaleAcceleration = 1f;
    public uint increaseSpeedCylindersThreshold = 5;
    public uint triesAmount = 5;
    public float tapAccuracy = 0.1f;

    private Vector3 currentPosition;
    private bool isOutsideDirected = true;
    private float currentScaleSpeed = 1f;
    private float currentMaxScale = 5f;
    private uint currentTry;
    private Timer timer;
    private InputController inputController;
    private GameObject currentCylinder;
    [SerializeField]
    CylinderStates currentCylinderTowerState;

    public void FixCylinder()
    {
        if (currentCylinderTowerState == CylinderStates.SCALING)
        {
            ControlAccuracy();
            if (CheckIsGameOver())
            {
                return;
            }
            SetNewMaxScale(currentCylinder.transform.localScale.x);
            cylindersPassed++;
            PrepareNewCylinder();
        }
    }

    private void ControlAccuracy()
    {
        Debug.Log("Current mistake: " + Mathf.Abs(currentCylinder.transform.localScale.x - currentMaxScale));
        if (Mathf.Abs(currentCylinder.transform.localScale.x - currentMaxScale) < tapAccuracy)
        {
            currentCylinder.transform.localScale
                = new Vector3(currentMaxScale, currentCylinder.transform.localScale.y, currentMaxScale);
            // Place for sound and animation
            // ...
            //
        }
    }

    void ScaleCylinder()
    {
        Vector3 newScale;
        if (isOutsideDirected)
        {
            newScale = currentCylinder.transform.localScale
                + new Vector3(currentScaleSpeed * Time.deltaTime, 0, currentScaleSpeed * Time.deltaTime);
            currentCylinder.transform.localScale = new Vector3(Mathf.Clamp(newScale.x, 0, currentMaxScale * maxScaleMultiplier),
                newScale.y, Mathf.Clamp(newScale.z, 0, currentMaxScale * maxScaleMultiplier));
            return;
        }
        newScale = currentCylinder.transform.localScale
                - new Vector3(currentScaleSpeed * Time.deltaTime, 0, currentScaleSpeed * Time.deltaTime);
        currentCylinder.transform.localScale = new Vector3(Mathf.Clamp(newScale.x, 0, currentMaxScale * maxScaleMultiplier),
            newScale.y, Mathf.Clamp(newScale.z, 0, currentMaxScale * maxScaleMultiplier));
        return;
    }

    private void CheckFinalTryIsGameOver(float lowerBorder, float upperBorder)
    {
        if (currentTry == 1)
        {
            if (!CheckScaleIsInWithInBorders(
            lowerBorder,
            upperBorder))
            {
                CheckIsGameOver();
            }
        }
    }

    private bool CheckScaleIsInWithInBorders(float lowerBorder, float upperBorder)
    {
        return !(currentCylinder.transform.localScale.x >= lowerBorder && currentCylinder.transform.localScale.x <= upperBorder);
    }

    private void CheckTryFinish(float toCompareWith)
    {
        if (currentCylinder.transform.localScale.x
                == toCompareWith)
        {
            isOutsideDirected = !isOutsideDirected;
            currentTry--;
        }
    }

    private bool CheckIsGameOver()
    {
        if (currentCylinder.transform.localScale.x < minScale
            || currentCylinder.transform.localScale.x > currentMaxScale)
        {
            currentCylinderTowerState = CylinderStates.GAME_OVER;
        }
        return currentCylinderTowerState == CylinderStates.GAME_OVER;
    }

    private void Start()
    {
        timer = gameObject.AddComponent<Timer>();
        inputController = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>();
        currentPosition = startPosition;
        currentScaleSpeed = startScaleSpeed;
        internalCircle.transform.localScale = new Vector3(minScale, internalCircle.transform.localScale.y, minScale);
        ShowNewCylinder();
    }

    private void Update()
    {
        switch (currentCylinderTowerState)
        {
            case CylinderStates.SCALING:
                {
                    ScalingProcess();
                    break;
                }
            case CylinderStates.PREPARING_NEW_CYLINDER:
                {
                    if (timer.GetIsTimeOut())
                    {
                        ShowNewCylinder();
                    }
                    break;
                }
            case CylinderStates.GAME_OVER:
                {
                    Debug.Log("Tower height: " + GetCurrentTowerHeight());
                    Debug.Log(cameraTarget.transform.position);
                    enabled = false;
                    break;
                }
        }
    }

    private void PrepareNewCylinder()
    {
        // Transfer to preparing new cylinder state
        currentCylinderTowerState = CylinderStates.PREPARING_NEW_CYLINDER;
        timer.SetCountDown(prepareCylinderTime);
    }

    private void ScalingProcess()
    {
        if (currentTry > 0)
        {
            ScaleCylinder();
            if (isOutsideDirected)
            {
                CheckFinalTryIsGameOver(currentMaxScale, currentMaxScale * maxScaleMultiplier);

                CheckTryFinish(currentMaxScale * maxScaleMultiplier);
            }
            else
            {
                CheckFinalTryIsGameOver(0, minScale);

                CheckTryFinish(0);
            }
        }
    }

    private void SetNewMaxScale(float newScale)
    {
        currentMaxScale = newScale;
    }

    private void ShowNewCylinder()
    {
        // Transfer to scaling state
        currentCylinderTowerState = CylinderStates.SCALING;
        isOutsideDirected = true;
        currentTry = triesAmount;
        currentPosition += new Vector3(0, moveUpStep, 0);
        currentCylinder = Instantiate(cylinderPrefab,
            currentPosition,
            cylinderPrefab.transform.rotation,
            transform);
        currentCylinder.transform.localScale = new Vector3(
            0,
            currentCylinder.transform.localScale.y,
            0);
        cameraTarget.transform.position = currentPosition;
        internalCircle.transform.position = currentPosition;
    }

    public CylinderStates GetCylinderState()
    {
        return currentCylinderTowerState;
    }

    public GameObject GetCameraTarget()
    {
        return cameraTarget;
    }

    public float GetCurrentTowerHeight()
    {
        return Vector3.Distance(currentPosition, transform.position) + moveUpStep / 2;
    }

    public Transform GetCameraTargetTransform()
    {
        return cameraTarget.transform;
    }
}
