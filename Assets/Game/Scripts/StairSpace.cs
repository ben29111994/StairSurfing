using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StairSpace : MonoBehaviour
{
    [Header("References")]
    public GenerateMap generateMap;
    public Stair stairSinglePrefab;
    public List<Stair> listStair = new List<Stair>();

    [Header("Input")]
    public int numberStairSingle;
    public int numberThick;

    private void Start()
    {
      //  GetComponent<Renderer>().material = GameManager.Instance.redStair;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material = GameManager.Instance.redStair;
        }
    }

#if UNITY_EDITOR
    [NaughtyAttributes.Button]
    public void GenerateStairSingle()
    {
        StartCoroutine(C_GenerateStairSingle());
    }

    private IEnumerator C_GenerateStairSingle()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        listStair.Clear();

        for (int i = 0; i < numberStairSingle; i++)
        {
            for (int k = 0; k < numberThick; k++)
            {
                Stair _stairSingle = PrefabUtility.InstantiatePrefab(stairSinglePrefab, transform) as Stair;
                listStair.Add(_stairSingle);

                _stairSingle.transform.localPosition = Vector3.zero;
                Vector3 pos = _stairSingle.transform.localPosition;
                pos.z = i * generateMap.stairSize.z;
                pos.y = k * -1.0f * generateMap.stairSize.y;
                _stairSingle.transform.localPosition = pos;
            }
        }

        yield return null;
    }
#endif

}
