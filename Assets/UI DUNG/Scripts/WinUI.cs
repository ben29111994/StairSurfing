using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    public Text headerText;
    public GameObject[] stars;
    public GameObject[] starsEffect;
    public GameObject[] starsEffect2;

    public void ShowWin(int star,bool isWin)
    {
        gameObject.SetActive(true);
        StartCoroutine(C_ShowWin(star, isWin));
    }

    private IEnumerator C_ShowWin(int star,bool isWin)
    {
        if(isWin)
        {
            headerText.text = "YOU WIN";
        }
        else
        {
            headerText.text = "YOU\nLOSE";
        }

        for (int i = 0; i < 3; i++)
        {
            starsEffect[i].SetActive(false);
            starsEffect2[i].SetActive(false);
            stars[i].SetActive(false);
            stars[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        for (int i = 0; i < 3; i++)
        {
            stars[i].SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < star; i++)
        {
            GameObject s = stars[i].transform.GetChild(1).gameObject;
            starsEffect[i].SetActive(true);
            starsEffect2[i].SetActive(true);
            s.SetActive(true);
            yield return new WaitForSeconds(0.15f);
            stars[i].GetComponent<Animator>().SetTrigger("Scale");
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.4f);

        if (isWin)
        {
            Debug.Log("Win -> show ResultUI");
            UIManager.Instance.Show_Result_UI();
        }
        else
        {
            Debug.Log("LOSE -> show HomeUI");
            yield return new WaitForSeconds(1.0f);
            UIManager.Instance.Show_Home_UI();
        }
    }
}
