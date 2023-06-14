using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool isCheckPoint;

    public ParticleSystem[] confetti;
    public Animator flagAnimator;
    public GameObject checkPointText;

    private void Awake()
    {
        gameObject.SetActive(false);
        transform.localEulerAngles = Vector3.zero;
    }

    private void Update()
    {
        if(GameManager.Instance.listStickMan[0] != null)
        {
            float z = GameManager.Instance.listStickMan[0].transform.position.z;
            if(transform.position.z - z < 20.0f)
            {
                checkPointText.SetActive(true);
            }
            else
            {
                checkPointText.SetActive(false);
            }
        }
    }

    public void ActiveCheckPoint()
    {
        if (isCheckPoint) return;

        isCheckPoint = true;

        for(int i = 0; i < confetti.Length; i++)
        {
            confetti[i].Play();
        }

        flagAnimator.SetTrigger("Active");
        GameManager.Instance.ShakeCamera();
        GameManager.Instance.Vibration();
    }
}