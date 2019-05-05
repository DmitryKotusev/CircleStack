using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator : MonoBehaviour
{
    [SerializeField]
    float r;
    [SerializeField]
    float g;
    [SerializeField]
    float b;
    [SerializeField]
    bool directionRedFlag;
    [SerializeField]
    bool directionGreenFlag;
    [SerializeField]
    bool directionBlueFlag;

    public int colorChangeStep = 10;
    public int minRValue = 30;
    public int minGValue = 30;
    public int minBValue = 30;
    public int maxRValue = 220;
    public int maxGValue = 220;
    public int maxBValue = 220;

    public void InitGenerator(Color color)
    {
        r = Mathf.Clamp(color.r * 255, minRValue, maxRValue);
        g = Mathf.Clamp(color.g * 255, minGValue, maxGValue);
        b = Mathf.Clamp(color.b * 255, minBValue, maxBValue);
        Debug.Log("R: " + r + " G: " + g + " B: " + b);
        directionRedFlag = true;
        directionGreenFlag = true;
        directionBlueFlag = true;
    }

    public void ResetGenerator()
    {
        r = Random.Range(minRValue, maxRValue + 1);
        g = Random.Range(minGValue, maxGValue + 1);
        b = Random.Range(minBValue, maxBValue + 1);
        Debug.Log("R: " + r + " G: " + g + " B: " + b);
        directionRedFlag = true;
        directionGreenFlag = true;
        directionBlueFlag = true;
    }

    public Color GenerateNewColor()
    {
        //int choose = Random.Range(0, 3);
        int redWeight = Random.Range(0, 11);
        int greenWeight = Random.Range(0, 11);
        int blueWeight = Random.Range(0, 11);
        int sumWeight = redWeight + greenWeight + blueWeight;
        if (sumWeight <= 0)
        {
            int choose = Random.Range(0, 3);
            switch (choose)
            {
                case 0:
                    {
                        redWeight = 1;
                        sumWeight = 1;
                        break;
                    }
                case 1:
                    {
                        greenWeight = 1;
                        sumWeight = 1;
                        break;
                    }
                case 2:
                    {
                        blueWeight = 1;
                        sumWeight = 1;
                        break;
                    }
            }
        }
        float redSpeed = redWeight / (float)sumWeight * colorChangeStep;
        float greenSpeed = greenWeight / (float)sumWeight * colorChangeStep;
        float blueSpeed = blueWeight / (float)sumWeight * colorChangeStep;
        // Red color to be changed
        if (directionRedFlag)
        {
            r = Mathf.Clamp(r + redSpeed, minRValue, maxRValue);
            if (r == maxRValue)
            {
                directionRedFlag = !directionRedFlag;
            }
        }
        else
        {
            r = Mathf.Clamp(r - redSpeed, minRValue, maxRValue);
            if (r == minRValue)
            {
                directionRedFlag = !directionRedFlag;
            }
        }
        // Debug.Log("Red: " + r);
        // Green color to be changed
        if (directionGreenFlag)
        {
            g = Mathf.Clamp(g + greenSpeed, minGValue, maxGValue);
            if (g == maxGValue)
            {
                directionGreenFlag = !directionGreenFlag;
            }
        }
        else
        {
            g = Mathf.Clamp(g - greenSpeed, minGValue, maxGValue);
            if (g == minGValue)
            {
                directionGreenFlag = !directionGreenFlag;
            }
        }
        // Debug.Log("Green: " + g);
        if (directionBlueFlag)
        {
            b = Mathf.Clamp(b + blueSpeed, minBValue, maxBValue);
            if (b == maxBValue)
            {
                directionBlueFlag = !directionBlueFlag;
            }
        }
        else
        {
            b = Mathf.Clamp(b - blueSpeed, minBValue, maxBValue);
            if (b == minBValue)
            {
                directionBlueFlag = !directionBlueFlag;
            }
        }
        // Debug.Log("Blue: " + b);
        return new Color(r / 255f, g / 255f, b / 255f, 1f);
    }

    public Color GetCurrentColor()
    {
        return new Color(r / 255f, g / 255f, b / 255f, 1f);
    }

    public Color GetReverseColor()
    {
        return new Color((255 - r) / 255f, (255 - g) / 255f, (255 - b) / 255f, 1f);
    }
}
