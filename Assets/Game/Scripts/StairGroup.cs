using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class StairGroup : MonoBehaviour
{
    public bool isMove;
    public int numberActive;
    public List<Stair> listStair = new List<Stair>();

    private void Start()
    {
        if (isMove) Move();
    }

    public void StopMove()
    {
        StopAllCoroutines();
    }

    public void Move()
    {
        if(listStair.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                listStair.Add(transform.GetChild(i).GetComponent<Stair>());
            }
        }
  

        StartCoroutine(C_Move());
    }

    private IEnumerator C_Move()
    {
        bool isLoop = true;

        numberActive = (int)(listStair.Count * 0.5f);

        List<int> listNumber = new List<int>();
        for(int i = 0; i < numberActive; i++)
        {
            listNumber.Add(i);
        }

        int _d = 1;

        while (isLoop)
        {
            for(int i = 0; i < listStair.Count; i++)
            {
                listStair[i].Init(true);
            }

            for (int i = 0; i < listNumber.Count; i++)
            {
                listStair[listNumber[i]].Init(false);
            }

            for(int i = 0; i < listNumber.Count; i++)
            {
                listNumber[i] += _d;
            }

            if (listNumber[listNumber.Count - 1] == listStair.Count - 1)
            {
                _d = -1;
            }

            if (listNumber[0] == 0)
            {
                _d = 1;
            }

            yield return new WaitForSeconds(0.4f);
        }
    }

#if UNITY_EDITOR
    [NaughtyAttributes.Button]
    public void FixBottom()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform _t = transform.GetChild(i);

            for (int n = 1; n < _t.childCount; n++)
            {
                _t.GetChild(n).gameObject.SetActive(false);
            }

            Transform _c = _t.transform.GetChild(0).transform;
            _c.transform.localPosition = Vector3.up * -1.3f;
            _c.transform.localScale = Vector3.one + Vector3.up * 11.0f;
        }
    }
#endif

}