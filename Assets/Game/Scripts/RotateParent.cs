using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateParent : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 160.0f);
    }
}
