using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMap : MonoBehaviour
{
    [Header("Input")]
    public GameObject[] levelObject;

    [NaughtyAttributes.Button]
    public void GenerateMap()
    {
        for(int y = 0;y < levelObject.Length; y++)
        {
            GameObject _levelObject = Instantiate(levelObject[y]);


            for (int i = 0; i < _levelObject.transform.childCount; i++)
            {
                Transform _child = _levelObject.transform.GetChild(i);

                if (_child.CompareTag("CheckPoint"))
                {
                    Transform _cp = _levelObject.transform.GetChild(i);
                    _cp.transform.localScale = new Vector3(1.0f, 2.0f, 1.0f);

                    float h = (_cp.position.y / 0.2f);
                    Vector3 pos = _cp.position;
                    pos.y = h * 0.4f;
                    _cp.position = pos;
                }
                else
                {
                    for (int k = 0; k < _levelObject.transform.GetChild(i).childCount; k++)
                    {
                        Transform stairTransform = _levelObject.transform.GetChild(i).GetChild(k);
                        BoxCollider bc = stairTransform.GetComponent<BoxCollider>();
                        bc.center = new Vector3(0.0f, -1.05f, 0.0f);
                        bc.size = new Vector3(0.8f, 2.3f, 4.8f);

                        float h = (stairTransform.position.y / 0.2f);
                        Vector3 pos = stairTransform.position;
                        pos.y = h * 0.4f;
                        stairTransform.position = pos;

                        Vector3 scale = stairTransform.localScale;
                        scale.y = 2.0f;
                        stairTransform.localScale = scale;

                        Transform bottom = stairTransform.GetChild(0);
                        bottom.localPosition = Vector3.down * 1.1f;
                        bottom.localScale = Vector3.one + Vector3.up * 10.0f;
                        bottom.gameObject.tag = "Stair";
                    }
                }
            }
        }   
    }

    [Header("Duplicate")]
    [Range(0.0f,1.0f)]
    public float percent;
    public Transform _target;
    private List<Transform> listStair = new List<Transform>();
    private List<Vector3> listNewPosition = new List<Vector3>();
    public List<Transform> listCheckPoint = new List<Transform>();
    public LayerMask layer;

    [NaughtyAttributes.Button]
    public void DuplicateGroup()
    {
        if (_target == null) return;

        listStair.Clear();
        listNewPosition.Clear();
        listCheckPoint.Clear();

        int stairCount = (int)(_target.transform.childCount * percent);
        Vector3 currentPosition = _target.transform.GetChild(_target.transform.childCount - 1).position;
        currentPosition.z += 0.8f;
        currentPosition.y = 1912997.0f;
        FixStair(currentPosition);

        for(int i = 0;i < listStair.Count; i++)
        {
            Vector3 temp = listStair[i].transform.position;
            temp.z += 0.8f * stairCount;
            listStair[i].transform.position = temp;
        }

        Vector3 curPos = _target.transform.GetChild(_target.transform.childCount - 1).position;
        Transform stair = _target.transform.GetChild(_target.transform.childCount - 1);
        for (int i = 0;i < stairCount; i++)
        {
            curPos.z += 0.8f;
            Transform newStair = Instantiate(stair, _target);
            newStair.transform.position = curPos;
        }

        Transform parent = _target.parent;
        for(int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).CompareTag("CheckPoint"))
            {
                listCheckPoint.Add(parent.GetChild(i));
            }
        }

        for(int i = 0; i < listCheckPoint.Count; i++)
        {
            Vector3 temp = listCheckPoint[i].transform.position;
            temp.z += 0.8f * stairCount;
            listCheckPoint[i].transform.position = temp;
        }
        
    }

    private void FixStair(Vector3 _pos)
    {
        Ray ray = new Ray(_pos, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, Mathf.Infinity,layer))
        {
            if(hit.collider != null)
            {
                listStair.Add(hit.collider.gameObject.transform);
                _pos.z += 0.8f;
                FixStair(_pos);
            }
        }
    }
}