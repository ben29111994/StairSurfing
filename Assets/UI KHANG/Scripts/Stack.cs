using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stack : MonoBehaviour
{
    public int ID;
    public int price;

    [Header("References")]
    public Text priceText;
    public Image icon;
    public Text stackNumberText;
    public Animator plusAnimator;
    public Animator minusAnimator;
    public Animator stackNumberAnimator;

    public int StackNumber
    {
        get
        {
            return PlayerPrefs.GetInt("stack" + ID);
        }
        set
        {
            int _valueFixed = Mathf.Clamp(value,1, 999);
            PlayerPrefs.SetInt("stack" + ID, _valueFixed);
            stackNumberText.text = _valueFixed.ToString();
        }
    }

    public void Init(int _ID,Sprite _sprite,int _price)
    {
        ID = _ID;
        price = _price;
        priceText.text = _price.ToString();
        icon.sprite = _sprite;
        StackNumber += 0;
    }

    public void OnClick_Plus()
    {
        stackNumberAnimator.SetTrigger("Bubble");
        plusAnimator.SetTrigger("Bubble");

        int myCoin = 1000;

        if (myCoin < price) return;

      //  myCoin -= price;

        Debug.Log("minue " + price + " coin ");

        StackNumber++;
    }

    public void OnClick_Minus()
    {
        stackNumberAnimator.SetTrigger("Bubble");
        minusAnimator.SetTrigger("Bubble");

        int stackNumber = StackNumber;

        if (stackNumber <= 1) return;

        Debug.Log("plus " + price + " coin ");

        // mycoin += price;
        StackNumber--;
    }
}
