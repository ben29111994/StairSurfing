using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockIngredient : MonoBehaviour
{
    public Text nameText;
    public Image iconImg;

    public void OnAwake(string _name,Sprite _icon)
    {
        nameText.text = _name + " UNLOCKED";
        nameText.text = "UNLOCKED";
        iconImg.sprite = _icon;
        gameObject.SetActive(true);
    }

    public void Onclick_OK()
    {
        if (UI.Instance.chestUI.activeInHierarchy)
        {
            UI.Instance.chestUI.GetComponent<ChestRoom>().isAnimation = false;

            if (UIManager.Instance.playUIScript.Key == 0)
            {
                GameManager.Instance.isChesting = false;
            }
            gameObject.SetActive(false);
            return;
        }

        if (UI.Instance.isPlayerP == false)
        {
            UI.Instance.MoveToTargetPage();
        }
        else
        {
            UI.Instance.MoveToTargetPagePlayerP();
        }

        gameObject.SetActive(false);
    }
}
