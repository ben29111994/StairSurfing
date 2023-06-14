using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteUI : MonoBehaviour
{
    public GameObject headerMiss;
    public GameObject headerNeverMiss;
    public GameObject recordObject;

    public Text feetText;
    public Text coinEarnText;
    private int extraCoin;
    private bool isCollect;

    public void ShowComplete(int feet, int coinEarn, bool isMiss, bool isNewHeight)
    {
        if (isMiss)
        {
            headerMiss.SetActive(true);
            headerNeverMiss.SetActive(false);
        }
        else
        {
            headerMiss.SetActive(false);
            headerNeverMiss.SetActive(true);
        }

        recordObject.SetActive(isNewHeight);
        feetText.text = feet + " FEET";
        coinEarnText.text = coinEarn.ToString();
        extraCoin = coinEarn;
        isCollect = false;

        gameObject.SetActive(true);
    }

    public void OnClick_Collect()
    {
        if (isCollect) return;

        isCollect = true;
        StartCoroutine(C_Collect());
    }

    private IEnumerator C_Collect()
    {
        Debug.Log("COLLECT COIN " + extraCoin);
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
   
}
