using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChestRoom : MonoBehaviour
{
    public bool isAnimation;
    public Animator[] chestAnimator;
    public Animator btnUnlockAnimator;
    public Button[] btnChest;
    public List<bool> isChestOpen = new List<bool>();
    public int skin;
    int n = 0;

    private void Start()
    {
        for(int i = 0; i < btnChest.Length; i++)
        {
            int n = i;
            btnChest[n].onClick.AddListener(() => UnlockChest(n));
            isChestOpen.Add(false);
        }
    }

    public void OnClick_UnlockChest()
    {
        btnUnlockAnimator.SetTrigger("Bubble");
    }

    public void UnlockChest(int number)
    {
        if (isAnimation) return;
        if (isChestOpen[number]) return;
        if (UIManager.Instance.playUIScript.Key <= 0) return;
        UIManager.Instance.playUIScript.Key--;
        n++;
        isAnimation = true;

        float _percent = Random.value;

        bool isCoin = (_percent <= 0.7f) ? true : false;

        if (UI.Instance.CanUnlock() == false || skin == 1)
        {
            isCoin = true;
        }

        if (isCoin)
        {
            chestAnimator[number].SetTrigger("Active");
            isChestOpen[number] = true;
            int randomCoin = (int)(100 * Random.Range(0.6f, 1.4f));
            int a = randomCoin % 10;
            randomCoin -= a;

            chestAnimator[number].gameObject.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = "+" + randomCoin;
            Image _spr = chestAnimator[number].gameObject.transform.GetChild(0).GetComponent<Image>();

            StartCoroutine(C_EarnCoin(randomCoin, _spr));
        }
        else
        {
            StartCoroutine(C_Ingredient(number));
        }
    }

    private IEnumerator C_Ingredient(int number)
    {
        skin++;
        chestAnimator[number].SetTrigger("Active2");
        yield return new WaitForSeconds(1.4f);
        UI.Instance.UnlockIngredientWhenComplete();
    }

    private IEnumerator C_EarnCoin(int _coin, Image _spr)
    {
        yield return new WaitForSeconds(2.25f);

        int a = _coin / 10;
        int b = _coin % 10;

        for (int i = 0; i < 10; i++)
        {
            UIManager.Instance.Coin += a;
            yield return new WaitForSeconds(0.04f);
        }

        UIManager.Instance.Coin += b;

        yield return new WaitForSeconds(0.5f);

        isAnimation = false;
        _spr.DOColor(new Color(100.0f, 100.0f, 100.0f,255.0f),0.5f);


        if (UIManager.Instance.playUIScript.Key == 0)
        {
            GameManager.Instance.isChesting = false;
        }
    }

}
