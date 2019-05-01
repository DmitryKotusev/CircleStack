using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator : MonoBehaviour
{
    [SerializeField]
    int r;
    [SerializeField]
    int g;
    [SerializeField]
    int b;
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
        int choose = Random.Range(0, 3);
        switch (choose)
        {
            // Red color to be changed
            case 0:
                {
                    if (directionRedFlag)
                    {
                        r = Mathf.Clamp(r + colorChangeStep, minRValue, maxRValue);
                        if (r == maxRValue)
                        {
                            directionRedFlag = !directionRedFlag;
                        }
                    }
                    else
                    {
                        r = Mathf.Clamp(r - colorChangeStep, minRValue, maxRValue);
                        if (r == minRValue)
                        {
                            directionRedFlag = !directionRedFlag;
                        }
                    }
                    // Debug.Log("Red: " + r);
                    break;
                }
            // Green color to be changed
            case 1:
                {
                    if (directionGreenFlag)
                    {
                        g = Mathf.Clamp(g + colorChangeStep, minGValue, maxGValue);
                        if (r == maxGValue)
                        {
                            directionGreenFlag = !directionGreenFlag;
                        }
                    }
                    else
                    {
                        g = Mathf.Clamp(g - colorChangeStep, minGValue, maxGValue);
                        if (r == minGValue)
                        {
                            directionGreenFlag = !directionGreenFlag;
                        }
                    }
                    // Debug.Log("Green: " + g);
                    break;
                }
            // Blue color to be changed
            case 2:
                {
                    if (directionBlueFlag)
                    {
                        b = Mathf.Clamp(b + colorChangeStep, minBValue, maxBValue);
                        if (b == maxBValue)
                        {
                            directionBlueFlag = !directionBlueFlag;
                        }
                    }
                    else
                    {
                        b = Mathf.Clamp(b - colorChangeStep, minBValue, maxBValue);
                        if (b == minBValue)
                        {
                            directionBlueFlag = !directionBlueFlag;
                        }
                    }
                    // Debug.Log("Blue: " + b);
                    break;
                }
        }
        return new Color(r / 255f, g / 255f, b / 255f, 1f);
    }
}
