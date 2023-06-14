using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishStair : MonoBehaviour
{
    private void Start()
    {


        GameManager.Instance.finishPositionZ = transform.position.z;
     //   transform.GetChild(1).gameObject.SetActive(false);
       // transform.GetChild(1).gameObject.GetComponent<BoxCollider>().size = new Vector3(0.8f, 1.0f, 4.8f);
  //      Vector3 temp = transform.GetChild(1).gameObject.transform.position;
   //     temp.z += 0.75f;
 //       transform.GetChild(1).gameObject.transform.position = temp;
    }
}
