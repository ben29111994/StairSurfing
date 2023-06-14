using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerP : MonoBehaviour
{
    public int ID;
    public bool isBuy;
    public bool isChest;
    public bool isSelected;

    [Header("References")]
    public GameObject lockObject;
    public GameObject unlockObject;
    public Image iconImg;
    public Text nameText;
    public Animator outlineAnimator;
    public Animator mainAnimator;
    public Image lockImg;
    public Sprite questionSpr;
    public Sprite chestSpr;
    public GameObject selectObject;
    public Button selectButton;

    public void Init(int _id, Sprite _icon,Sprite _opacity, string _name, bool _isChest)
    {
        ID = _id;
        iconImg.sprite = _icon;
        nameText.text = _name;
        isChest = _isChest;
        //lockImg.sprite = (isChest) ? questionSpr : questionSpr;
        lockImg.sprite = _opacity;
        selectButton.onClick.AddListener(() => SelectButton());

        if (isSelected == false)
        {
            selectObject.SetActive(false);
        }
    }

    public void UpdatePlayerP()
    {
        int _number = PlayerPrefs.GetInt("playerp" + ID);

        if (_number == 0)
        {
            isBuy = false;
            lockObject.SetActive(true);
            unlockObject.SetActive(false);
        }
        else
        {
            if (UI.Instance.justBuy)
            {
                isBuy = true;
                lockObject.SetActive(true);
                unlockObject.SetActive(false);
            }
            else
            {
                isBuy = true;
                lockObject.SetActive(false);
                unlockObject.SetActive(true);
            }
        }
    }

    public void ShowAnimationOutline(float time)
    {
        StartCoroutine(C_ShowAnimationOutline(time));
    }

    private IEnumerator C_ShowAnimationOutline(float time)
    {
        yield return new WaitForSeconds(time);

        outlineAnimator.SetTrigger("Active");

        if (isChest)
        {
            lockObject.SetActive(true);
            unlockObject.SetActive(true);
            mainAnimator.SetTrigger("Active");
        }
        else
        {
            lockObject.SetActive(true);
            unlockObject.SetActive(true);
            mainAnimator.SetTrigger("Active2");

        }

        UI.Instance.justBuy = false;
        SelectButton();
    }

    public void SelectButton()
    {
        if (isBuy == false) return;
        if (isSelected) return;

        UI.Instance.UnSelectAllPlayerP();

        isSelected = true;
        selectObject.SetActive(true);
        GameManager.Instance.stickmanSkinIndex = ID;
        UI.Instance.CurrentSelectPlayer = ID;
    }

    public void UnselectIngredient()
    {
        isSelected = false;
        selectObject.SetActive(false);
    }
}
