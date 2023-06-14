using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchUI : MonoBehaviour
{
    public Image playerFlag;

    public Text enemyNameText;
    public Image enemyFlag;

    public Text enemyNameText2;
    public Image enemyFlag2;

    public GameObject matchCharactor;

    public SkinnedMeshRenderer rend;
    public Material defaultPlayer;
    public Material skinPlayer;
    public Texture[] skinTexture;

    public void ChangeSkin()
    {
        int index = GameManager.Instance.stickmanSkinIndex;
        index++;

        if (index < 1)
        {
            rend.material = defaultPlayer;
        }
        else
        {
            rend.material = skinPlayer;
            skinPlayer.SetTexture("_MainTex", skinTexture[GameManager.Instance.stickmanSkinIndex]);
        }
    }
        
    public void OnEnable()
    {
        ChangeSkin();
        Match();
    }

    public void Match()
    {
        StartCoroutine(C_Match());
    }

    private IEnumerator C_Match()
    {
        playerFlag.sprite = UIManager.Instance.flagPlayer;

        // random name - flag enemy
        string enemyname = UIManager.Instance.listName[Random.Range(0, (int)(UIManager.Instance.listName.Length / 2.0f))];
        UIManager.Instance.nameEnemy = enemyname;
        enemyNameText.text = enemyname;

        Sprite enemyflag = UIManager.Instance.flagsSpr[Random.Range(0, (int)(UIManager.Instance.flagsSpr.Length / 2.0f))];
        UIManager.Instance.flagEnemy = enemyflag;
        enemyFlag.sprite = enemyflag;


        string enemyname2 = UIManager.Instance.listName[Random.Range((int)(UIManager.Instance.listName.Length / 2.0f) + 1, UIManager.Instance.listName.Length)];
        UIManager.Instance.nameEnemy2 = enemyname2;
        enemyNameText2.text = enemyname2;

        Sprite enemyflag2 = UIManager.Instance.flagsSpr[Random.Range((int)(UIManager.Instance.flagsSpr.Length / 2.0f) + 1, UIManager.Instance.flagsSpr.Length)];
        UIManager.Instance.flagEnemy2 = enemyflag2;
        enemyFlag2.sprite = enemyflag2;

        yield return new WaitForSeconds(1.5f);
        matchCharactor.SetActive(true);


        yield return new WaitForSeconds(2.0f);

        UIManager.Instance.Show_Play_UI();
    }

    public void OnDisable()
    {
        matchCharactor.SetActive(false);
    }
}
