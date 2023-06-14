using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResult : MonoBehaviour
{
    public Text sttText;
    public Text nameText;
    public Text scoreText;
    public Image flagImg;

    public void Init(int _stt, string _name, int _score, Sprite _flag)
    {
        sttText.text = _stt.ToString();
        nameText.text = _name;
        scoreText.text = _score.ToString();
        flagImg.sprite = _flag;
    }

    public void HideProfile(bool isHide)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isHide);
        }
    }
}
