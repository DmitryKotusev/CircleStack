using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    CylinderManager cylinderManager;
    InputController inputController;

    private void Start()
    {
        cylinderManager = GameObject.FindGameObjectWithTag("CylinderManager").GetComponent<CylinderManager>();
        inputController = GameObject.FindGameObjectWithTag("InputController").GetComponent<InputController>();
    }

    private void Update()
    {
        if (inputController.leftMouseButtonClick)
        {
            cylinderManager.FixCylinder();
        }
    }
}
