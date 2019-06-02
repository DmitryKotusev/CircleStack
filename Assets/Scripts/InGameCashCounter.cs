using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCashCounter : MonoBehaviour
{
    // Тут все настройки для достижений и прогрессий
    [Header("Geometric progression settings")]
    public uint requiredAmountOfStepsWithoutMistake;
    public float progressionDenominator;
    [Header("Achievement settings")]
    public int firstRewardRequiredAmount;
    public int standardRewardStartRequiredAmount;
    public int standardRewardRequiredAmountStep;
    public float firstReward;
    public float standardReward;

    private uint progressionIndex;
    private uint stepsWithoutMistake = 0; // !!!!
    private bool isFirstStandardRewardReached;
    [SerializeField]
    private float currentCash;
    private CylinderManager cylinderManager;

    public void InitCounter()
    {
        progressionIndex = 0;
        stepsWithoutMistake = 0;
        isFirstStandardRewardReached = false;
        currentCash = 0;
    }

    public void OnCylinderFixedAccurately()
    {
        stepsWithoutMistake++;
        float currentAddReward = 0;
        currentAddReward += ProgressionLogic();
        currentAddReward += AchievementLogic();
        
        currentCash += currentAddReward;
    }

    private float AchievementLogic()
    {
        float currentAchievementReward = 0;
        int currentTowerHeight = cylinderManager.GetCylinderAmount() + 1;
        if (currentTowerHeight == firstRewardRequiredAmount)
        {
            currentAchievementReward += firstReward;
        }
        if (currentTowerHeight == standardRewardStartRequiredAmount)
        {
            isFirstStandardRewardReached = true;
            currentAchievementReward += standardReward;
        }
        else if (isFirstStandardRewardReached)
        {
            if ((currentTowerHeight - standardRewardStartRequiredAmount) % standardReward == 0)
            {
                currentAchievementReward += standardReward;
            }
        }
        Debug.Log("Achievement reward: " + currentAchievementReward);

        return currentAchievementReward;
    }

    private float ProgressionLogic()
    {
        if (stepsWithoutMistake / requiredAmountOfStepsWithoutMistake > progressionIndex)
        {
            progressionIndex = stepsWithoutMistake / requiredAmountOfStepsWithoutMistake;
            Debug.Log("Progression reward: " + CountProgressionMember());
            return CountProgressionMember();
        }
        return 0;
    }

    public void OnCylinderFixedNotAccurately()
    {
        stepsWithoutMistake = 0;
        progressionIndex = 0;
        float currentAddReward = 0;
        currentAddReward += AchievementLogic();
        
        currentCash += currentAddReward;
    }

    public void SetCylinderManager(CylinderManager cylinderManager)
    {
        this.cylinderManager = cylinderManager;
    }

    private float CountProgressionMember()
    {
        return Mathf.Pow(progressionDenominator, progressionIndex);
    }
}
