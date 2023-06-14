using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimeStart : MonoBehaviour
{
    public Text timeText;
    public Text tutorialText;

    private void Start()
    {
        StartCoroutine(C_Start());
    }

    private IEnumerator C_Start()
    {
        timeText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);

        timeText.gameObject.SetActive(true);
        tutorialText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);


        for (int i = 3; i > 0; i--)
        {
            timeText.text = i.ToString();
            timeText.transform.DOScale(Vector3.one * 1.2f, 0.2f).SetLoops(2, LoopType.Yoyo);
            yield return new WaitForSeconds(.8f);
        }

        tutorialText.gameObject.SetActive(false);

        GameManager.Instance.PlayGame();
        gameObject.SetActive(false);
    }
}
