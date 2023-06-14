using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(C_Rotate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator C_Rotate()
    {
        bool isLoop = true;

        while (isLoop)
        {
            transform.Rotate(Vector3.back * 360.0f / 9.0f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
