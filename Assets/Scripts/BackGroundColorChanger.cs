using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundColorChanger : MonoBehaviour
{
    // public float changeColorSpeed;
    public float redChangeColorSpeed;
    public float greenChangeColorSpeed;
    public float blueChangeColorSpeed;
    public int minRValue = 0;
    public int minGValue = 0;
    public int minBValue = 0;
    public int maxRValue = 255;
    public int maxGValue = 255;
    public int maxBValue = 255;
    public Color startBackGroundColor;
    Material backGroundMaterial;
    bool directionRedFlag;
    bool directionGreenFlag;
    bool directionBlueFlag;

    private void ChangeBackGroundColor(float delta)
    {
        float r = backGroundMaterial.color.r * 255;
        float g = backGroundMaterial.color.g * 255;
        float b = backGroundMaterial.color.b * 255;
        //int redWeight = Random.Range(0, 11);
        //int greenWeight = Random.Range(0, 11);
        //int blueWeight = Random.Range(0, 11);
        //int sumWeight = redWeight + greenWeight + blueWeight;
        //if (sumWeight <= 0)
        //{
        //    return;
        //}
        //float redSpeed = redWeight / (float)sumWeight * changeColorSpeed;
        //float greenSpeed = greenWeight / (float)sumWeight * changeColorSpeed;
        //float blueSpeed = blueWeight / (float)sumWeight * changeColorSpeed;
        float redSpeed = Random.Range(0, redChangeColorSpeed);
        float greenSpeed = Random.Range(0, greenChangeColorSpeed);
        float blueSpeed = Random.Range(0, blueChangeColorSpeed);
        // Red channel
        if (directionRedFlag)
        {
            r = Mathf.Clamp(r + redSpeed * delta, minRValue, maxRValue);
            if (r == maxRValue)
            {
                directionRedFlag = !directionRedFlag;
            }
        }
        else
        {
            r = Mathf.Clamp(r - redSpeed * delta, minRValue, maxRValue);
            if (r == minRValue)
            {
                directionRedFlag = !directionRedFlag;
            }
        }
        // Green channel
        if (directionGreenFlag)
        {
            g = Mathf.Clamp(g + greenSpeed * delta, minGValue, maxGValue);
            if (g == maxGValue)
            {
                directionGreenFlag = !directionGreenFlag;
            }
        }
        else
        {
            g = Mathf.Clamp(g - greenSpeed * delta, minGValue, maxGValue);
            if (g == minGValue)
            {
                directionGreenFlag = !directionGreenFlag;
            }
        }
        // Blue channel
        if (directionBlueFlag)
        {
            b = Mathf.Clamp(b + blueSpeed * delta, minBValue, maxBValue);
            if (b == maxBValue)
            {
                directionBlueFlag = !directionBlueFlag;
            }
        }
        else
        {
            b = Mathf.Clamp(b - blueSpeed * delta, minBValue, maxBValue);
            if (b == minBValue)
            {
                directionBlueFlag = !directionBlueFlag;
            }
        }
        // Debug.Log("R: " + r + " G: " + g + " B: " + b);
        backGroundMaterial.color = new Color(r / 255f, g / 255f, b / 255f, 1);
    }

    void OnEnable()
    {
        
    }

    void Update()
    {
        if (backGroundMaterial != null)
        {
            ChangeBackGroundColor(Time.deltaTime);
        }
    }

    public void SetBackGroundRandomStartColor()
    {
        backGroundMaterial.color = new Color(
            Random.Range(0, 256) / 255f,
            Random.Range(0, 256) / 255f,
            Random.Range(0, 256) / 255f,
            1);
    }

    public void SetBackGroundFixedStartColor()
    {
        backGroundMaterial.color = startBackGroundColor;
    }

    public void StopChanger()
    {
        enabled = false;
    }

    public void StartChanger()
    {
        enabled = true;
    }

    public void SetBackGroundMaterial(Material material)
    {
        backGroundMaterial = material;
    }

    public Material GetBackGroundMaterial()
    {
        return backGroundMaterial;
    }
}
