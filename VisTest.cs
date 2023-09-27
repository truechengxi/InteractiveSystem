using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InteractiveSystem;

public class VisTest : MonoBehaviour, IVisible
{
    public string Name => gameObject.name;
    public bool Enable => enabled;

    private void OnEnable()
    {
        VisualManager<VisTest>.Instance.Register(this);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}