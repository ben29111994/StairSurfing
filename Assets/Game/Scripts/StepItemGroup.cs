using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

public class StepItemGroup : MonoBehaviour
{
    [Header("Input")]
    public int length;
    public float distanceZ;


    [Header("References")]
    public StepItem stepItemPrefab;


    [NaughtyAttributes.Button]
    public void GenerateStepItem()
    {
        int[] _randomLane = RandomIntArray(5);

        int n = 0;
        for (float x = -3.0f; x <= 3.0f; x += 1.5f)
        {
            for (int z = 0; z < length; z++)
            {
                StepItem _st = PrefabUtility.InstantiatePrefab(stepItemPrefab) as StepItem;
                Vector3 pos = Vector3.zero;
                pos.z = z * distanceZ;
                pos.x = x;
                _st.transform.SetParent(transform);
                _st.Init(pos, _randomLane[n]);
            }

            n++;
        }       
    }

    public int[] RandomIntArray(int lenght)
    {
        int[] newArray = new int[lenght];

        for (int i = 0; i < lenght; i++)
        {
            int r = Random.Range(0, lenght);

            if (i != 0)
            {
                for (int j = 0; j < i; j++)
                {
                    if (newArray[j] == r)
                    {
                        r = Random.Range(0, lenght);
                        j = -1;
                    }
                }
            }

            newArray[i] = r;
        }

        return newArray;
    }
}
#endif