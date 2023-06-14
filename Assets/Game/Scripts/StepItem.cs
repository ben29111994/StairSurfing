using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepItem : MonoBehaviour
{
    public int ID;
    public Renderer rend;

    public void Init(Vector3 pos,int id)
    {
        ID = id;
        transform.localPosition = pos;
    }

    private void Start()
    {
        StartCoroutine(C_Delay());
    }

    private IEnumerator C_Delay()
    {
        while (GameManager.Instance.listStickMan.Count == 0)
        {
            yield return null;
        }

        while (GameManager.Instance.listStickMan[0].currentStepMaterial == null)
        {
            yield return null;
        }
        rend.material = GameManager.Instance.listStickMan[ID].currentStepMaterial;
    }
}
