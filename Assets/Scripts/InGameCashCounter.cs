using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCashCounter : MonoBehaviour
{
    // Тут все настройки для достижений и прогрессий
    [Header("Geometric progression settings")]
    public int requiredAmountOfStepsWithoutMistake;
    public float progressionDenominator;
    [Header("Achievment settings")]
    public int firstRewardRequiredAmount;
    public int standardRewardStartRequiredAmount;
    public int standardRewardRequiredAmountStep;
}
