using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resolution : MonoBehaviour
{
    void Awake()
    {
        //Screen.SetResolution(1280, 720, true);
        Application.targetFrameRate = 30;
    }
}
