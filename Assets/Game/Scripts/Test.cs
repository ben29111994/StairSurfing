using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using VacuumShaders.CurvedWorld;
public class Test : MonoBehaviour
{
    public CurvedWorld_BoundingBox a;
    private void Start()
    {
       // transform.DOMove(Vector3.forward * 10.0f, 1.0f).OnComplete(CP).SetEase(Ease.Linear);

    }

    private void Update()
    {
        if (Input.GetKeyDown(0))
        {
            a.enabled = !a.enabled;
        }
    }
}
