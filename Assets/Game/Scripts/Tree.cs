using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public void Start()
    {
        RandomScale();
    }

    private void RandomScale()
    {
        float _value = Random.Range(2.0f, 5.0f);
        transform.localScale = Vector3.one * _value;
        
        if(Random.value > 0.5f)
        {
            gameObject.SetActive(false);
        }

        if(transform.position.x < 8 || transform.position.x > -8)
        {
            transform.localScale = Vector3.one * 3.0f;

        }
    }
}