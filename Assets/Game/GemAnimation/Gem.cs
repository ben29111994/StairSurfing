using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public Transform targets;
    Vector3 currentPosition;
    Vector3[] paths;
    private bool isRandom;
    Vector3 maxScale;
    float timeAnim;

    private void Awake()
    {
        if (isRandom == false)
        {
            isRandom = true;
            paths = new Vector3[2];
            paths[0] = transform.position + new Vector3(Random.Range(-10.0f,10.0f), Random.Range(-10.0f,10.0f), 0);
            paths[1] = targets.position;

            currentPosition = transform.position;
        }
    }

    private void OnEnable()
    {
        Move();
    }

    private void Move()
    {
        transform.position = currentPosition;
        timeAnim = (float)Random.Range(120, 160) / 100.0f;

        iTween.MoveTo(gameObject, iTween.Hash("path", paths, "time", timeAnim, "easetype", iTween.EaseType.easeOutSine));

        float perScale = (float)Random.Range(70, 100) / 100.0f;
        maxScale = Vector3.one * perScale;

        transform.localScale = Vector3.one * .3f;
        iTween.ScaleTo(gameObject, iTween.Hash("scale", maxScale, "time", timeAnim / 2, "easetype", iTween.EaseType.easeOutSine, "oncomplete", "C_"));
    }


    private void C_()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.one * 0.3f, "time", timeAnim/2, "easetype", iTween.EaseType.easeOutSine,"oncomplete", "CompleteAnimation"));
    }

    private void CompleteAnimation()
    {
        transform.localScale = Vector3.zero;
    }
}
