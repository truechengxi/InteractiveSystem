using System.Collections;
using System.Collections.Generic;
using InteractiveSystem;
using Unity.VisualScripting;
using UnityEngine;

public class TestPoint : MonoBehaviour, ISimpleVisible
{
    private void OnEnable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        SimpleVisualManagement.Instance.Register(this);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool Enable => enabled;
    public Vector3 Position => transform.position;
}