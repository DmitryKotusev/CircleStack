using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileDataController : MonoBehaviour
{
    public string maxScoreKey = "MaxScore";
    public string coinsKey = "CurrencyAmount";
    public string roofsDataKey = "RoofsData";

    public void InitRootPrefabsContainer()
    {
        RoofsPrefabsContainer roofsPrefabsContainer =
            GameObject.FindGameObjectWithTag("RoofsPrefabsContainer").GetComponent<RoofsPrefabsContainer>();
        string roofsDataJsonString = PlayerPrefs.GetString(roofsDataKey, "{\"roofsData\":[]}");
        RoofsDataWrapper arrayWrapper = JsonUtility.FromJson<RoofsDataWrapper>(roofsDataJsonString);
        if (arrayWrapper.roofsData.Length == 0)
        {
            return;
        }
        roofsPrefabsContainer.roofPrefabInfos = new List<RoofPrefabInfo>(arrayWrapper.roofsData);
    }

    public void SynchronizeRootPrefabsContainerWithDataStotage()
    {

    }

    public void SaveMaxScore(int score)
    {
        PlayerPrefs.SetInt(maxScoreKey, score);
    }

    public int ReadMaxScore()
    {
        return PlayerPrefs.GetInt(maxScoreKey, 0);
    }

    public float ReadCurrencyAmount()
    {
        return PlayerPrefs.GetFloat(coinsKey, 0);
    }

    public void SaveCurrencyAmount(float amount)
    {
        PlayerPrefs.SetFloat(coinsKey, amount);
    }

    public void AddCurrencyAmount(float amount)
    {
        PlayerPrefs.SetFloat(coinsKey, ReadCurrencyAmount() + amount);
    }

    public bool SpendCurrencyAmount(float amount)
    {
        float currentCurrencyAmount = ReadCurrencyAmount();
        if (currentCurrencyAmount - amount < 0)
        {
            return false;
        }

        PlayerPrefs.SetFloat(coinsKey, ReadCurrencyAmount() - amount);
        return true;
    }
}
