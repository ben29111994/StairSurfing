using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FixController : MonoBehaviour
{
    public List<Transform> listStairs = new List<Transform>();
    public GameObject stairPrefab;

#if UNITY_EDITOR
    [NaughtyAttributes.Button]
    public void Fix()
    {
        listStairs.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            StairGroup _stairGroup = transform.GetChild(i).GetComponent<StairGroup>();

            if (_stairGroup != null)
            {
                _stairGroup.FixBottom();

                for (int k = 0; k < _stairGroup.transform.childCount; k++)
                {
                    listStairs.Add(_stairGroup.transform.GetChild(k));
                }
            }

            if (transform.GetChild(i).gameObject.name == "Last Group")
            {
                AutoFix _autoFix = transform.GetChild(i).gameObject.AddComponent<AutoFix>();
                _autoFix.FixBottom();

                for (int k = 0; k < _autoFix.transform.childCount; k++)
                {
                    if (k < 200)
                        listStairs.Add(_autoFix.transform.GetChild(k));
                }
            }
        }

        GenerateTopStair();
    }

    private void GenerateTopStair()
    {
        GameObject _topStairParent = new GameObject();
        _topStairParent.name = "TopStair";
        _topStairParent.transform.SetParent(transform);

        for (int i = 0; i < listStairs.Count; i++)
        {
            GameObject _stair = PrefabUtility.InstantiatePrefab(stairPrefab, _topStairParent.transform) as GameObject;
            _stair.transform.position = listStairs[i].transform.position + Vector3.up * 20.0f;

            GameObject _stair2 = PrefabUtility.InstantiatePrefab(stairPrefab, _stair.transform) as GameObject;
            _stair2.transform.localPosition = Vector3.up * 5.1f;
            _stair2.transform.localRotation = Quaternion.identity;
            _stair2.transform.localScale = Vector3.one + Vector3.up * 50;
        }
    }

    private List<Transform> listStairGroup = new List<Transform>();

    [NaughtyAttributes.Button]
    public void FixStairFoward()
    {
        listStairGroup.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform _transform = transform.GetChild(i);

            if (_transform.gameObject.name == "Last Group")
            {
            //    listStairGroup.Add(_transform);
            }
            else if(_transform.gameObject.name == "TopStair")
            {
                FixTopStair(_transform);
            }
            else
            {
                if (_transform.GetComponent<StairGroup>() != null)
                {
                    listStairGroup.Add(_transform);
                }
            }
        }
    }

    private void FixTopStair(Transform _t)
    {
        while (_t.childCount > 0)
        {
            DestroyImmediate(_t.GetChild(0).gameObject);
        }

        for(int i = 0; i < listStairGroup.Count; i++)
        {
            for(int k = 0; k < listStairGroup[i].transform.childCount; k++)
            {
                GameObject _stair = PrefabUtility.InstantiatePrefab(stairPrefab,_t) as GameObject;
                _stair.transform.localScale = new Vector3(1.0f, 30.0f, 1.0f);
                _stair.transform.position = listStairGroup[i].transform.GetChild(k).transform.position;
            }
        }

        Vector3 pos = _t.position;
        pos.y += 22.8f;
        _t.position = pos;
    }

#endif
}
