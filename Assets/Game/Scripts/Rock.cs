using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public Rigidbody rigid;
    public float moveSpeed;
    public bool isRun;

    private void FixedUpdate()
    {
        float myZ = transform.position.z;
        float tarZ = GameManager.Instance.listStickMan[0].transform.position.z;
        if (myZ - tarZ < 50.0f)
        {
            if (isRun == false) isRun = true;
        }
           
        if (isRun == false || GameManager.Instance.isStart == false)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            return;
        }
        
        rigid.AddTorque(Vector3.right * moveSpeed * 5.0f);
    }
}
