using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VacuumShaders.CurvedWorld;

public class UpdateBoundingBox : MonoBehaviour
{
    public CurvedWorld_BoundingBox cb;

    private float min;
    private float max;
    private int n;

    private void Start()
    {
        StartCoroutine(C_Update());
    }

    private void Update()
    {
  
    }

    private IEnumerator C_Update()
    {
        yield return new WaitForSeconds(1.0f);

        if (GameManager.Instance.isCurveWorld == false)
        {
            gameObject.SetActive(false);
            yield break;
        }

        cb = GetComponent<CurvedWorld_BoundingBox>();

        n = 1;
        cb.scale = 500;
        min = 495;
        max = 505;

        bool isUpdate = true;

        while (isUpdate)
        {
            if(GameManager.Instance != null)
            {

                if (GameManager.Instance.isCurveWorld == false)
                {
                    yield break;
                }
                else if(gameObject.name == "key" && GameManager.Instance.keyObject == null)
                {
                    yield return null;
                }
                else
                {
                    if (cb.scale > max)
                    {
                        n = -1;
                        cb.scale = max;
                    }
                    else if (cb.scale < min)
                    {
                        n = 1;
                        cb.scale = min;
                    }

                    cb.scale += 1 * n;

                    float time = Random.Range(3.0f, 5.0f);

                    yield return new WaitForSeconds(time);

              
                }
            }
        }
    }
}
