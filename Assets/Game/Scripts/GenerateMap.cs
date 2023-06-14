using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GenerateMap : MonoBehaviour
{
    [Header("References")]
    public GameObject levelObject;
    private List<Transform> listStairs = new List<Transform>();

    [Header("Stair")]
    public GameObject finishPrefab;
    public GameObject stairPrefab;
    public Vector3 stairSize;

    [Header("Input")]
    public int currentLevel;
    public List<Level> listLevel = new List<Level>();

    [System.Serializable]
    public class Level
    {
        public TypeStair typeStair;
        public int amount;
        public bool isHide;
        public bool isMove;
    }

    public enum TypeStair
    {
        UpStair,
        Forward
    }

#if UNITY_EDITOR

    [NaughtyAttributes.Button]
    public void GenerateStairLevel()
    {
        if (levelObject != null) DestroyImmediate(levelObject);
        listStairs.Clear();
        int _lvl = currentLevel;
        levelObject = new GameObject();
        levelObject.name = "Level " + _lvl;

        Vector3 currentPosition = Vector3.forward * 0.8f * 3.0f;

        for(int i = 0; i < listLevel.Count; i++)
        {
            Level _level = listLevel[i];
            GameObject _stairGroup = new GameObject();
            StairGroup _stairGroupScript = _stairGroup.AddComponent<StairGroup>();
            _stairGroupScript.isMove = _level.isMove;
            _stairGroup.transform.SetParent(levelObject.transform);
            _stairGroup.name = "Stair Group " + i;

            for(int k = 0; k < _level.amount; k++)
            {
                GameObject _stair = PrefabUtility.InstantiatePrefab(stairPrefab, _stairGroup.transform) as GameObject;
                listStairs.Add(_stair.transform);
                Stair _stairScript = _stair.GetComponent<Stair>();

                switch (_level.typeStair)
                {
                    case TypeStair.UpStair:
                        _stair.transform.position = currentPosition;

                        if(k == _level.amount - 1)
                        {
                            _stairScript.isYellow = true;
                            currentPosition.z += stairSize.z;
                        }
                        else
                        {
                            currentPosition += stairSize;
                        }
                        break;
                    case TypeStair.Forward:
                        _stair.transform.position = currentPosition;
                        currentPosition.z += stairSize.z;
                        break;
                }

                _stairScript.Init(_level.isHide);

                //for (int n = 0; n < 1; n++)
                //{
                //    GameObject _stair2 = PrefabUtility.InstantiatePrefab(stairPrefab, _stair.transform) as GameObject;
                //    Stair _stairScript2 = _stair2.GetComponent<Stair>();
                //    _stairScript2.Init(_level.isHide);
                //    _stair2.transform.localPosition = Vector3.down * 1.3f;
                //    _stair2.transform.localRotation = Quaternion.identity;
                //    _stair2.transform.localScale = Vector3.one + Vector3.up * 11;
                //}
            }
        }

        GameObject _lastGroup = new GameObject();
        _lastGroup.name = "Last Group";
        _lastGroup.transform.SetParent(levelObject.transform);

        for (int i = 0; i < 200; i++)
        {
            GameObject _stair = null;

            if (i == 10 || i == 11)
            {
                _stair = PrefabUtility.InstantiatePrefab(finishPrefab, _lastGroup.transform) as GameObject;
                _stair.transform.position = currentPosition;
                currentPosition.z += stairSize.z;

                if(i == 10)
                {
                    _stair.AddComponent<FinishStair>();
                }
            }
            else
            {
                _stair = PrefabUtility.InstantiatePrefab(stairPrefab, _lastGroup.transform) as GameObject;
                _stair.transform.position = currentPosition;
                currentPosition.z += stairSize.z;
            }

            listStairs.Add(_stair.transform);

            //for (int n = 0; n < 1; n++)
            //{
            //    GameObject _stair2 = PrefabUtility.InstantiatePrefab(stairPrefab, _stair.transform) as GameObject;
            //    Stair _stairScript2 = _stair2.GetComponent<Stair>();
            //    _stair2.transform.localPosition = Vector3.down * 1.3f;
            //    _stair2.transform.localRotation = Quaternion.identity;
            //    _stair2.transform.localScale = Vector3.one + Vector3.up * 11;
            //}
        }

     //   GenerateTopStair();
    }

    private void GenerateTopStair()
    {    
        GameObject _topStairParent = new GameObject();
        _topStairParent.name = "TopStair";
        _topStairParent.transform.SetParent(levelObject.transform);

        for (int i = 0;i < listStairs.Count; i++)
        {
            GameObject _stair = PrefabUtility.InstantiatePrefab(stairPrefab, _topStairParent.transform) as GameObject;
            _stair.transform.position = listStairs[i].transform.position + Vector3.up * 20.0f;

            GameObject _stair2 = PrefabUtility.InstantiatePrefab(stairPrefab, _stair.transform) as GameObject;
            _stair2.transform.localPosition = Vector3.up * 1.6f;
            _stair2.transform.localRotation = Quaternion.identity;
            _stair2.transform.localScale = Vector3.one + Vector3.up * 14;
        }
    }

#endif
}