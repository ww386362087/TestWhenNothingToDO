﻿using System;
using UnityEngine;

public class RotateInWorldSpace : MonoBehaviour
{
    private Vector3 currentAngle;
    public Vector3 initAngle;
    private Vector3 speedAnglePerFrame;
    public Vector3 speedPerSecond;

    private void LateUpdate()
    {
        this.currentAngle += this.speedAnglePerFrame;
        base.get_gameObject().get_transform().set_rotation(Quaternion.Euler(this.currentAngle));
    }

    private void OnEnable()
    {
        this.currentAngle = this.initAngle;
        int num = Application.get_targetFrameRate();
        this.speedAnglePerFrame = (Vector3) (this.speedPerSecond / ((float) num));
    }
}

