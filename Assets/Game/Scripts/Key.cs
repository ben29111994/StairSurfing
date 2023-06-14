using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.keyObject = gameObject;
    }

    void Start()
    {
        Vector3 pos = transform.position;
        pos.x = 1.0f;
     //   pos.y += Random.Range(2.0f, 4.0f);
        transform.position = pos;
    }
}
