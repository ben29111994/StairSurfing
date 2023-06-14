using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Stair : MonoBehaviour
{
    public bool isYellow;
    public bool isHide;
    public Renderer rend;
    private StairSpace stairSpaceParent;
    private Wall wallParent;

    public void Init(bool _isHide)
    {
        isHide = _isHide;
        rend.enabled = !isHide;

        //for(int i = 0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).GetComponent<Renderer>().enabled = !isHide;
        //}
    }

    private void Start()
    {
        wallParent = transform.parent.GetComponent<Wall>();
        stairSpaceParent = transform.parent.GetComponent<StairSpace>();

        if (gameObject.name == "Stair Finish")
        {
            Vector3 pos = transform.position;
            pos.y += 0.001f;
            transform.position = pos;
        }

        if (isYellow)
        {
            rend.material = GameManager.Instance.yellowStair;
        }
      //  SetYellowStair();
    }

    public bool isFade;

    private void Update()
    {
        if (isFade) return;

        if (wallParent == null) return;

        if (GameManager.Instance.listStickMan[0] == null) return;

        if (GameManager.Instance.listStickMan[0].transform.position.z > transform.position.z + 1.0f)
        {
            ChangeRedStairFade();
        }
    }

    public void FixSize()
    {
        Vector3 scale = transform.localScale;
        scale.z = 0.999f;
        transform.localScale = scale;
    }

    public void ChangeRedStair()
    {
        if (isFade) return;

        if (stairSpaceParent != null)
        {
            Vector3 s = transform.localScale;
            s.x *= 0.99f;
            transform.localScale = s;
        }

        rend.material = GameManager.Instance.redStair;

        if (transform.childCount != 0)
            transform.GetChild(0).GetComponent<Renderer>().material = rend.material;
    }

    public void ChangeRedStairFade()
    {
        if (isFade) return;

        rend.material = GameManager.Instance.redStairFade;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Renderer>().material = GameManager.Instance.redStairFade;
        }
    }


    public bool aa;
    public bool bb;

    private void SetYellowStair()
    {
        bool a = false;
        bool b = false;

        Vector3 origin = transform.position + Vector3.up * 2.0f + Vector3.back * GameManager.Instance.generateMap.stairSize.z;
        Ray ray = new Ray(origin, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Stair"))
            {
                if(transform.position.y > hit.collider.gameObject.transform.position.y)
                {
                    a = true;
                }
            }
        }

        origin = transform.position + Vector3.up * 2.0f + Vector3.forward * GameManager.Instance.generateMap.stairSize.z;
        ray = new Ray(origin, Vector3.down);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Stair"))
            {
                if (transform.position.y == hit.collider.gameObject.transform.position.y)
                {
                    b = true;
                }
            }
        }

        if (a && b)
        {
            rend.material = GameManager.Instance.yellowStair;
        }

        aa = a;
        bb = b;
    }

#if UNITY_EDITOR
    public void GenerateSingleStair()
    {

    }
#endif
}
