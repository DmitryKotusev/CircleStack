using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderManager : MonoBehaviour
{
    [SerializeField]
    public AudioClip missAccuracyClip;
    [SerializeField]
    public AudioClip gotAccuracy1Clip;
    [SerializeField]
    private float deafultpitch = 1f;
    [SerializeField]
    private float pitch1Value = 1f;
    [SerializeField]
    private float pitch2Value = 1f;
    [SerializeField]
    private float pitch3Value = 1f;
    [SerializeField]
    private float pitch4Value = 1f;
    [SerializeField]
    private float pitch5Value = 1f;
    [SerializeField]
    private float pitch6Value = 1f;
    [SerializeField]
    private float pitch7Value = 1f;
    [SerializeField]
    public AudioClip gotAccuracy2Clip;
    [SerializeField]
    public AudioClip gotAccuracy3Clip;
    [SerializeField]
    public AudioClip gotAccuracy4Clip;
    [SerializeField]
    public AudioClip gotAccuracy5Clip;
    [SerializeField]
    public AudioClip gotAccuracy6Clip;
    [SerializeField]
    public AudioClip gotAccuracy7Clip;
    [SerializeField]
    public AudioClip loseClip;
    [SerializeField]
    uint cylindersPassed = 1;
    [SerializeField]
    GameObject cameraTarget;
    [SerializeField]
    GameObject newCylindersContainer;
    [SerializeField]
    Material originalMaterial;
    [SerializeField]
    GameObject defaultCylinder;
    [SerializeField]
    Color defaultCylinderStartColor;
    [SerializeField]
    GameObject particleNReachPrefab;
    [SerializeField]
    GameObject textureAccuracySatisfy;
    [SerializeField]
    float textureStepScaleMultiplier = 0.1f;
    public float particlesDestroyTime = 2f;
    public float textureDestroyTime = 2f;

    public Vector3 startPosition = new Vector3(0, 0.5f, 0);
    // public Color color;
    public GameObject cylinderPrefab;
    public GameObject internalCircle;
    public float prepareCylinderTime = 1f;
    public float moveUpStep = 1;
    public float cylinderBaseRadius = 0.5f;
    public float minScale;
    public float startMaxScale = 5f;
    public float maxScaleMultiplier = 1.2f;
    public float startScaleSpeed = 1f;
    public float scaleAcceleration = 1f;
    public float maxScaleSpeed = 8f;
    public uint increaseSpeedCylindersThreshold = 5;
    public uint triesAmount = 5;
    public float tapAccuracy = 0.1f;

    public uint increaseSpeedThreshold = 7;
    public uint boostThreshold = 5;
    public float boostAmount = 0.4f;

    public event Action GameOver;
    // public event Action ChangeCylinderColor;

    private uint increaseScaleSpeedCounter = 0;
    private uint stepsWithoutMistake = 0;
    private uint boostCounter = 0;
    private Vector3 startCameraTargetPosition;
    private Vector3 currentPosition;
    private bool isOutsideDirected = true;
    [SerializeField]
    private float currentScaleSpeed = 1f;
    private float oldScaleSpeed;
    private bool isPlayerHavingSpeedReward;
    private float currentMaxScale = 5f;
    private uint currentTry;
    private Timer timer;
    private InputController inputController;
    private ColorGenerator colorGenerator;
    private GameObject currentCylinder;
    private AudioSource audioPlayer;
    [SerializeField]
    CylinderStates currentCylinderTowerState;
    [SerializeField]
    GameObject roofContainer;

    public ColorGenerator GetColorGenerator()
    {
        return colorGenerator;
    }

    public void FixCylinder()
    {
        if (currentCylinderTowerState == CylinderStates.SCALING)
        {
            if (isPlayerHavingSpeedReward)
            {
                // Reward turn off block
                isPlayerHavingSpeedReward = false;
                currentScaleSpeed = oldScaleSpeed;
                ////////////////////////
            }
            ControlAccuracy();
            if (CheckIsGameOver())
            {
                return;
            }
            increaseScaleSpeedCounter++;
            if (increaseScaleSpeedCounter >= increaseSpeedThreshold)
            {
                increaseScaleSpeedCounter = 0;
                if (!isPlayerHavingSpeedReward)
                {
                    IncreaseSpeed();
                }
            }
            SetNewMaxScale(currentCylinder.transform.localScale.x);
            cylindersPassed++;
            PrepareNewCylinder();
        }
    }

    private void IncreaseSpeed()
    {
        currentScaleSpeed = Mathf.Clamp(scaleAcceleration + currentScaleSpeed, startScaleSpeed, maxScaleSpeed);
    }

    private void ControlAccuracy()
    {
        // Debug.Log("Current mistake: " + Mathf.Abs(currentCylinder.transform.localScale.x - currentMaxScale));
        if (Mathf.Abs(currentCylinder.transform.localScale.x - currentMaxScale) < tapAccuracy)
        {
            boostCounter++;
            stepsWithoutMistake++;
            if (boostCounter >= boostThreshold)
            {
                boostCounter = 0;
                BoostScale();
                // Тут происходит награда за n раз подряд
                BoostParticlesReward();
                Play7ThClip();
                // Speed reward
                isPlayerHavingSpeedReward = true;
                oldScaleSpeed = currentScaleSpeed;
                currentScaleSpeed = startScaleSpeed;
            }
            else
            {
                AccuracySatisfyReward();
                // Итоговый звук
                Play1Desh6Clips();
            }
            currentCylinder.transform.localScale
                = new Vector3(currentMaxScale, currentCylinder.transform.localScale.y, currentMaxScale);
            // Place for sound and animation
            // ...
            //
        }
        else
        {
            stepsWithoutMistake = 0;
            boostCounter = 0;
            // Звук промаха
            PlayMissClip();
        }
    }

    private void Play7ThClip()
    {
        if (gotAccuracy7Clip != null)
        {
            audioPlayer.Stop();
            audioPlayer.clip = gotAccuracy7Clip;
            audioPlayer.pitch = pitch7Value;
            audioPlayer.Play();
        }
    }

    private void Play1Desh6Clips()
    {
        switch (boostCounter)
        {
            case 1:
                {
                    if (gotAccuracy1Clip != null)
                    {
                        audioPlayer.Stop();
                        audioPlayer.clip = gotAccuracy1Clip;
                        audioPlayer.pitch = pitch1Value;
                        audioPlayer.Play();
                    }
                    break;
                }
            case 2:
                {
                    if (gotAccuracy2Clip != null)
                    {
                        audioPlayer.Stop();
                        audioPlayer.clip = gotAccuracy2Clip;
                        audioPlayer.pitch = pitch2Value;
                        audioPlayer.Play();
                    }
                    break;
                }
            case 3:
                {
                    if (gotAccuracy3Clip != null)
                    {
                        audioPlayer.Stop();
                        audioPlayer.clip = gotAccuracy3Clip;
                        audioPlayer.pitch = pitch3Value;
                        audioPlayer.Play();
                    }
                    break;
                }
            case 4:
                {
                    if (gotAccuracy4Clip != null)
                    {
                        audioPlayer.Stop();
                        audioPlayer.clip = gotAccuracy4Clip;
                        audioPlayer.pitch = pitch4Value;
                        audioPlayer.Play();
                    }
                    break;
                }
            case 5:
                {
                    if (gotAccuracy5Clip != null)
                    {
                        audioPlayer.Stop();
                        audioPlayer.clip = gotAccuracy5Clip;
                        audioPlayer.pitch = pitch5Value;
                        audioPlayer.Play();
                    }
                    break;
                }
            case 6:
                {
                    if (gotAccuracy6Clip != null)
                    {
                        audioPlayer.Stop();
                        audioPlayer.clip = gotAccuracy6Clip;
                        audioPlayer.pitch = pitch6Value;
                        audioPlayer.Play();
                    }
                    break;
                }
        }
    }

    private void PlayLoseClip()
    {
        if (loseClip != null)
        {
            audioPlayer.Stop();
            audioPlayer.clip = loseClip;
            audioPlayer.pitch = deafultpitch;
            audioPlayer.Play();
        }
    }

    private void PlayMissClip()
    {
        if (missAccuracyClip != null)
        {
            audioPlayer.Stop();
            audioPlayer.clip = missAccuracyClip;
            audioPlayer.pitch = deafultpitch;
            audioPlayer.Play();
        }
    }

    private void BoostParticlesReward()
    {
        GameObject particlesClone = Instantiate(particleNReachPrefab,
            currentPosition - new Vector3(0, moveUpStep / 2, 0),
            particleNReachPrefab.transform.rotation);
        ParticleSystem particleSystem = particlesClone.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particleSystem.main;
        main.startColor = colorGenerator.GetCurrentColor();
        var shapeSettings = particleSystem.shape;
        shapeSettings.radius = cylinderBaseRadius * currentMaxScale;
        Destroy(particlesClone, particlesDestroyTime);
    }

    private void AccuracySatisfyReward()
    {
        GameObject textureClone = Instantiate(textureAccuracySatisfy,
            currentPosition - new Vector3(0, moveUpStep / 2, 0),
            textureAccuracySatisfy.transform.rotation);

        textureClone.transform.localScale = new Vector3(
            currentMaxScale * (1 + (boostCounter - 1) * textureStepScaleMultiplier),
            1,
            currentMaxScale * (1 + (boostCounter - 1) * textureStepScaleMultiplier));
        Renderer textureRenderer = textureClone.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>();
        textureRenderer.material.color = new Color(
            textureRenderer.material.color.r,
            textureRenderer.material.color.g,
            textureRenderer.material.color.b,
            (float)boostCounter / (increaseSpeedThreshold - 1)
            );
        // ParticleSystem particleSystem = particlesClone.GetComponent<ParticleSystem>();
        // var shapeSettings = particleSystem.shape;
        // shapeSettings.radius = cylinderBaseRadius * currentMaxScale;
        Destroy(textureClone, textureDestroyTime);
    }

    private void BoostScale()
    {
        currentMaxScale = Mathf.Clamp(currentMaxScale + boostAmount, minScale, startMaxScale);
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
                if (CheckIsGameOver())
                {
                    currentTry = 0;
                }
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
            // Звук поражения
            PlayLoseClip();
        }
        return currentCylinderTowerState == CylinderStates.GAME_OVER;
    }

    private void Start()
    {
        startCameraTargetPosition = cameraTarget.transform.position;
        timer = gameObject.AddComponent<Timer>();
        inputController = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>();
        colorGenerator = GetComponent<ColorGenerator>();
        audioPlayer = GetComponent<AudioSource>();
        boostCounter = 0;
        stepsWithoutMistake = 0;
        increaseScaleSpeedCounter = 0;
        isPlayerHavingSpeedReward = false;
        // colorGenerator.ResetGenerator();
        // SetNewMaterialToObject(defaultCylinder);
        // Init();
    }

    public void Init()
    {
        internalCircle.SetActive(true);
        enabled = true;
        currentMaxScale = startMaxScale;
        currentCylinderTowerState = CylinderStates.PREPARING_NEW_CYLINDER;
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
                    internalCircle.SetActive(false);
                    GameOver?.Invoke();
                    Debug.Log("Tower height: " + GetCurrentTowerHeight());
                    Debug.Log(cameraTarget.transform.position);
                    enabled = false;
                    break;
                }
            case CylinderStates.NOT_STARTED:
                {
                    internalCircle.SetActive(false);
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
            newCylindersContainer.transform);
        currentCylinder.transform.localScale = new Vector3(
            0,
            currentCylinder.transform.localScale.y,
            0);
        // Material setting
        SetNewMaterialToObject(currentCylinder);
        // Material setting end
        cameraTarget.transform.position = currentPosition;
        internalCircle.transform.position = currentPosition;
        roofContainer.transform.position = currentPosition + new Vector3(0, moveUpStep / 2, 0);
    }

    private void SetNewMaterialToObject(GameObject currentCylinder)
    {
        Renderer rend = currentCylinder.GetComponent<Renderer>();
        rend.material = new Material(originalMaterial);
        rend.material.color = colorGenerator.GenerateNewColor();
        // ChangeCylinderColor?.Invoke();
    }

    private void SetStartMaterialToObject(GameObject currentCylinder)
    {
        Renderer rend = currentCylinder.GetComponent<Renderer>();
        rend.material = new Material(originalMaterial);
        rend.material.color = colorGenerator.GetCurrentColor();
        // ChangeCylinderColor?.Invoke();
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

    public void ResetDefaultCylinderColor()
    {
        colorGenerator.ResetGenerator();
        SetNewMaterialToObject(defaultCylinder);
    }

    public void SetDefaultCylinderColor()
    {
        colorGenerator.InitGenerator(defaultCylinderStartColor);
        SetStartMaterialToObject(defaultCylinder);
    }

    public void CleanTower()
    {
        foreach (Transform child in newCylindersContainer.transform)
        {
            Destroy(child.gameObject);
        }
        currentPosition = startPosition;
    }

    public void ResetCameraTargetPosition()
    {
        cameraTarget.transform.position = startCameraTargetPosition;
    }

    public int GetCylinderAmount()
    {
        return newCylindersContainer.transform.childCount - 1;
    }

    public uint GetCurrentTry()
    {
        return currentTry;
    }

    public uint GetStepsWithoutMistake()
    {
        return stepsWithoutMistake;
    }
}
