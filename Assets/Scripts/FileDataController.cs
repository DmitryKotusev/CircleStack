using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileDataController : MonoBehaviour
{
    public string maxScoreKey = "MaxScore";
    public void SaveMaxScore(int score)
    {
        PlayerPrefs.SetInt(maxScoreKey, score);
    }

    public int ReadMaxScore()
    {
        return PlayerPrefs.GetInt(maxScoreKey, 0);
    }
}
