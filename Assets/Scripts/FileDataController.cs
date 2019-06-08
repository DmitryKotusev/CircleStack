using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class FileDataController : MonoBehaviour
{
    public string maxScoreKey = "MaxScore";
    public string coinsKey = "CurrencyAmount";
    public string roofsDataKey = "RoofsData";

    public void InitRootPrefabsContainer()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, maxScoreKey)))
        {
            RoofsPrefabsContainer roofsPrefabsContainer =
            GameObject.FindGameObjectWithTag("RoofsPrefabsContainer").GetComponent<RoofsPrefabsContainer>();

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Path.Combine(Application.persistentDataPath, roofsDataKey), FileMode.Open);
            RoofSerializableData[] serializableDataArray = (RoofSerializableData[])bf.Deserialize(file);
            // Записать в дефолтный массив
            foreach (RoofSerializableData data in serializableDataArray)
            {
                RoofPrefabInfo roofPrefabInfo = roofsPrefabsContainer.roofPrefabInfos.Find((roofInfo) =>
                {
                    return roofInfo.roofsName == data.roofsName;
                });
                roofPrefabInfo.isEquiped = data.isEquiped;
                roofPrefabInfo.isBought = data.isBought;
            }
            file.Close();
        }
    }

    public void SynchronizeRootPrefabsContainerWithDataStotage()
    {
        RoofsPrefabsContainer roofsPrefabsContainer =
            GameObject.FindGameObjectWithTag("RoofsPrefabsContainer").GetComponent<RoofsPrefabsContainer>();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, roofsDataKey));
        RoofSerializableData[] serializableDataArray = new RoofSerializableData[roofsPrefabsContainer.roofPrefabInfos.Count];
        for (int i = 0; i < roofsPrefabsContainer.roofPrefabInfos.Count; i++)
        {
            serializableDataArray[i].isBought = roofsPrefabsContainer.roofPrefabInfos[i].isBought;
            serializableDataArray[i].isEquiped = roofsPrefabsContainer.roofPrefabInfos[i].isEquiped;
            serializableDataArray[i].roofsName = roofsPrefabsContainer.roofPrefabInfos[i].roofsName;
        }
        bf.Serialize(file, serializableDataArray);
        file.Close();
    }

    public void SaveMaxScore(int score)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, maxScoreKey));
        bf.Serialize(file, score);
        file.Close();
    }

    public int ReadMaxScore()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, maxScoreKey)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Path.Combine(Application.persistentDataPath, maxScoreKey), FileMode.Open);
            int maxScore = (int)bf.Deserialize(file);
            file.Close();
            return maxScore;
        }
        return 0;
    }

    public void DeleteMaxScoreFile()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, maxScoreKey)))
        {
            File.Delete(Path.Combine(Application.persistentDataPath, maxScoreKey));
        }
    }

    public float ReadCurrencyAmount()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, coinsKey)))
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Path.Combine(Application.persistentDataPath, coinsKey), FileMode.Open);
                float currencyAmount = (float)bf.Deserialize(file);
                file.Close();
                return currencyAmount;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        return 0;
    }

    public void SaveCurrencyAmount(float amount)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, coinsKey));
        bf.Serialize(file, amount);
        file.Close();
    }

    public void AddCurrencyAmount(float amount)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, coinsKey));
        bf.Serialize(file, ReadCurrencyAmount() + amount);
        file.Close();
    }

    public void DeleteCurrencyFile()
    {
        if (File.Exists(Path.Combine(Application.persistentDataPath, coinsKey)))
        {
            File.Delete(Path.Combine(Application.persistentDataPath, coinsKey));
        }
    }

    public bool SpendCurrencyAmount(float amount)
    {
        float currentCurrencyAmount = ReadCurrencyAmount();
        if (currentCurrencyAmount - amount < 0)
        {
            return false;
        }

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Path.Combine(Application.persistentDataPath, coinsKey));
        bf.Serialize(file, ReadCurrencyAmount() - amount);
        file.Close();
        return true;
    }
}
