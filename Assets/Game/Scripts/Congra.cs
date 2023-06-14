using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Congra : MonoBehaviour
{
    public Text congraText;
    public string[] congraArray;

    public void Active(int numberStair)
    {
        if(numberStair > 10 && numberStair < 20)
        {
            int r = Random.Range(0, 2);

            if(r == 0)
            {
                StartCoroutine(C_AnimationCongratulation());
            }
        }
        else
        {
            StartCoroutine(C_AnimationCongratulation());
        }
    }

    private IEnumerator C_AnimationCongratulation()
    {
        if (congraText.gameObject.activeInHierarchy) yield break;

        congraText.text = congraArray[Random.Range(0, congraArray.Length)];
        congraText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        congraText.gameObject.SetActive(false);
    }
}