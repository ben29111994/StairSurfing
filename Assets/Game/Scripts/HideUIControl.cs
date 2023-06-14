using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUIControl : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance.isComplete) return;

        if (GameManager.Instance.listStickMan[0] != null)
        {
            float z = GameManager.Instance.listStickMan[0].transform.position.z;
            if (transform.position.z - z < 10.0f)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}
