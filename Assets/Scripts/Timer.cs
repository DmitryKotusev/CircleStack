using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float countDown;
    private bool isTimeOut;

    private void Start()
    {
        isTimeOut = true;
    }

    void Update()
    {
        CheckCountDown();
    }

    private void CheckCountDown()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0)
        {
            countDown = 0;
            isTimeOut = true;
        }
    }

    public void SetCountDown(float newCountDown)
    {
        if(newCountDown >= 0)
        {
            countDown = newCountDown;
            isTimeOut = false;
        }
    }

    public bool GetIsTimeOut()
    {
        return isTimeOut;
    }
}
