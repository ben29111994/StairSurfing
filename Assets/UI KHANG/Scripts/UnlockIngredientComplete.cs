using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockIngredientComplete : MonoBehaviour
{
    public Text nameText;
    public Image iconImg;
    public Sprite[] challengeSpr;

    public void OnEnable()
    {
        iconImg.sprite = challengeSpr[GameManager.Instance.levelGame];
        Invoke("HideObject", 2.6f);
    }

    private void HideObject()
    {
        gameObject.SetActive(false);
    }
}
