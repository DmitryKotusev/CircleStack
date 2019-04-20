using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool leftMouseButtonClick = false;

    void Update()
    {
        leftMouseButtonClick = Input.GetMouseButtonDown(0);
    }
}
