﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjActivator : MonoBehaviour
{
    [Tooltip("The object to send the ACTIVATE message to. Default is self.")]
    public GameObject target;

    protected virtual void Awake() {
        if (!target) target = gameObject;
    }
}
