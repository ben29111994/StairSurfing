using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFix : MonoBehaviour
{
    [NaughtyAttributes.Button]
    public void FixBottom()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(i < 200)
            {
                Transform _t = transform.GetChild(i);

                int m = (_t.gameObject.name == "Stair Finish") ? 2 : 1;

                for (int n = m; n < _t.childCount; n++)
                {
                    _t.GetChild(n).gameObject.SetActive(false);
                }

                int h = (_t.gameObject.name == "Stair Finish") ? 1 : 0;

                if(h == 1)
                {
                    Transform _c = _t.transform.GetChild(h).transform;
                    _c.transform.localPosition = Vector3.up * -1.25f;
                    _c.transform.localScale = Vector3.one + Vector3.up * 10.5f;
                }
                else
                {
                    Transform _c = _t.transform.GetChild(h).transform;
                    _c.transform.localPosition = Vector3.up * -1.2f;
                    _c.transform.localScale = Vector3.one + Vector3.up * 11.0f;
                }
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
