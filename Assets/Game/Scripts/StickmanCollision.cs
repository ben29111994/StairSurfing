using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanCollision : MonoBehaviour
{
    public Stickman myStickman;

    private void Awake()
    {
        myStickman = transform.parent.GetComponent<Stickman>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Stickman"))
        {
            Stickman targetStickman = other.transform.parent.GetComponent<Stickman>();

            int myStep = myStickman.listStep.Count;
            int targetStep = targetStickman.listStep.Count;

            if (myStep > targetStep)
            {
                myStickman.GenerateStair(targetStep);
                targetStickman.DestructStickman();
                // effect destruct here
            }
            else if (myStep == targetStep)
            {
                myStickman.DestructStickman();
            }
        }
    }
}
