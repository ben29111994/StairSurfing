﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    void Start()
    {
        SetFOV();
    }

    void SetFOV()
    {
        float ratio = Camera.main.aspect;

        if (ratio >= 0.74) // 3:4
        {
            Camera.main.fieldOfView = 60;
        }
        else if (ratio >= 0.56) // 9:16
        {
            Camera.main.fieldOfView = 60;
        }
        else if (ratio >= 0.45) // 9:19
        {
            Camera.main.fieldOfView = 65;
        }
    }

}