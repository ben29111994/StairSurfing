using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    void Start()
    {
        Vector3 temp = transform.localScale;
        temp.x *= 0.99f;
        transform.localScale = temp;

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material = GameManager.Instance.redStair;

            if(transform.GetChild(i).childCount != 2)
            transform.GetChild(i).GetComponent<Stair>().FixSize();

            if(transform.GetChild(i).childCount == 2)
            {
                Transform c = transform.GetChild(i).GetChild(1);
                Vector3 scale = c.transform.localScale;
                scale.y *= 1.02f;
                c.transform.localScale = scale;
            }

            for (int k = 0; k < transform.GetChild(i).childCount; k++)
            {
                Renderer _s = transform.GetChild(i).GetChild(k).GetComponent<Renderer>();

                if(_s != null)
                {
                    _s.material = GameManager.Instance.redStair;
                }
            }
        }
    }
}
