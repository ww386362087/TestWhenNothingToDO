﻿using System;
using UnityEngine;

public class Moba_Camera_Boundary : MonoBehaviour
{
    public bool isActive = true;
    public Moba_Camera_Boundaries.BoundaryType type = Moba_Camera_Boundaries.BoundaryType.none;

    private void OnDestroy()
    {
        Moba_Camera_Boundaries.RemoveBoundary(this, this.type);
    }

    private void Start()
    {
        Moba_Camera_Boundaries.AddBoundary(this, this.type);
        if (LayerMask.NameToLayer(Moba_Camera_Boundaries.boundaryLayer) != -1)
        {
            base.get_gameObject().set_layer(LayerMask.NameToLayer(Moba_Camera_Boundaries.boundaryLayer));
        }
        else
        {
            Moba_Camera_Boundaries.SetBoundaryLayerExist(false);
            base.GetComponent<Collider>().set_isTrigger(true);
        }
    }
}

