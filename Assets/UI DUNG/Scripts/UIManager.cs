using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Text timmerText;
    public float timmer;

    [Header("References")]
    public GameObject coinUI;
    public GameObject homeUI;
    public GameObject matchUI;
    public GameObject playUI;
    public GameObject winUI;
    public GameObject loseUI;
    public GameObject resultUI;
    public Text tutorialText;
    public Text coinText;
    public Sprite[] flagsSpr;
    public string[] listName;
    public Text headerLoseText;
    public PlayUI playUIScript;

    [Header("Player")]
    public Sprite flagPlayer;

    [Header("Enemy")]
    public Sprite flagEnemy;
    public string nameEnemy;
    public Sprite flagEnemy2;
    public string nameEnemy2;

    [Header("TopUI")]
    public RectTransform[] topUI;

    [Header("Effect")]
    public ParticleSystem[] fireWork;

    [Header("BarFill")]
    public Image fill_Y;
    public Image fill_B;
    public Image fill_R;

    public RectTransform icon_Y;
    public RectTransform icon_B;
    public RectTransform icon_R;

    public int Coin
    {
        get
        {
            return PlayerPrefs.GetInt("Coin");
        }
        set
        {
            PlayerPrefs.SetInt("Coin", value);
            coinText.text = value.ToString();
            coinText.transform.DOScale(Vector2.one * 1.2f, 0.02f).SetLoops(2, LoopType.Yoyo);
        }
    }

    private void Start()
    {
        Coin += 0;
        FixedTopUI();
        Show_Home_UI();
    }

    private void Update()
    {
        if (GameManager.Instance.isStart && GameManager.Instance.isComplete == false)
        {
            timmer += Time.unscaledDeltaTime;
            timmerText.text = (Mathf.Round(timmer*100)/100).ToString();
        }

        if (Input.GetKeyDown(KeyCode.H)) Show_Home_UI();
        if (Input.GetKeyDown(KeyCode.M)) Show_Match_UI();
        if (Input.GetKeyDown(KeyCode.W)) Show_Win_UI(3);
        if (Input.GetKeyDown(KeyCode.L)) Show_Lose_UI(1);
        if (Input.GetKeyDown(KeyCode.F)) FireWorkEffect();
        if (Input.GetKeyDown(KeyCode.D)) PlayerPrefs.DeleteAll();
    }

    public void Show_Home_UI()
    {
        coinUI.SetActive(true);
        homeUI.SetActive(true);

        matchUI.SetActive(false);
        playUI.SetActive(false);
        winUI.SetActive(false);
        resultUI.SetActive(false);
        loseUI.SetActive(false);
    }

    public void Show_Match_UI()
    {
        UI.Instance.HideMainUI();

        matchUI.SetActive(true);

        coinUI.SetActive(false);
        homeUI.SetActive(false);
        playUI.SetActive(false);
        winUI.SetActive(false);
        resultUI.SetActive(false);
        loseUI.SetActive(false);
    }

    public void Show_Play_UI()
    {
        playUI.SetActive(true);

        matchUI.SetActive(false);
        coinUI.SetActive(true);
        homeUI.SetActive(false);
        winUI.SetActive(false);
        resultUI.SetActive(false);
        loseUI.SetActive(false);

        GameManager.Instance.ActiveStickMan(true);
    }

    public void Show_Win_UI(int star)
    {
        winUI.GetComponent<WinUI>().ShowWin(star,true);

        matchUI.SetActive(false);
        coinUI.SetActive(false);
        homeUI.SetActive(false);
        resultUI.SetActive(false);
    }

    public void Show_Lose_UI(int star)
    {
        //  winUI.GetComponent<WinUI>().ShowWin(star,false);
        loseUI.SetActive(true);

        winUI.SetActive(false);
        matchUI.SetActive(false);
        coinUI.SetActive(false);
        homeUI.SetActive(false);
        resultUI.SetActive(false);
    }

    public void Show_Result_UI()
    {
        resultUI.SetActive(true);
        coinUI.SetActive(true);

        playUI.SetActive(false);
        matchUI.SetActive(false);
        homeUI.SetActive(false);
        winUI.SetActive(false);
    }

    public void OnClick_ReLoadScene()
    {
        SceneManager.LoadScene(0);
    }

    private void FixedTopUI()
    {
        float ratio = Camera.main.aspect;

        if (ratio >= 0.74) // 3:4
        {
           
        }
        else if (ratio >= 0.56) // 9:16
        {
           
        }
        else if (ratio >= 0.45) // 9:19
        {
            for(int i = 0; i < topUI.Length; i++)
            {
                if(i == 0)
                {
                    topUI[i].anchoredPosition -= Vector2.up * 100.0f;
                }
                else if (i == 1)
                {
                    topUI[i].anchoredPosition -= Vector2.up * 100.0f;
                }
                else if (i == 2)
                {
                    topUI[i].anchoredPosition -= Vector2.up * 40.0f;
                }
            }
        }
    }

    public void FireWorkEffect()
    {
        if (fireWork[0].isPlaying) return;

        for(int i = 0; i < fireWork.Length; i++)
        {
            fireWork[i].Play();
        }
    }

    public void OnClick_TapToPlay()
    {
        Show_Play_UI();

        //if (GameManager.Instance.levelGame == 0)
        //{
        //    UI.Instance.HideMainUI();
        //    Show_Play_UI();
        //}
        //else
        //{
        //    Show_Match_UI();
        //}
    }

    public void UpdateFillBarStep()
    {
 //       if (GameManager.Instance.isStart == false) return;

 //       float zFinish = GameManager.Instance.finishPositionZ;

 //       float zPlayer = GameManager.Instance.stickMan.transform.position.z;
 //       float player_t = zPlayer / zFinish;
 //       fill_Y.fillAmount = player_t;
 //       icon_Y.anchoredPosition = new Vector2(Mathf.Lerp(-183.0f, 183.0f, player_t), 0.0f);

 //       float zBot1 = GameManager.Instance.stickManBot_1.transform.position.z + 0.02f;
 //       float bot1_t = zBot1 / zFinish;
 //       fill_B.fillAmount = bot1_t;
 //       icon_B.anchoredPosition = new Vector2(Mathf.Lerp(-183.0f, 183.0f, bot1_t), 0.0f);

 //       //float zBot2 = GameManager.Instance.stickManBot_2.transform.position.z + 0.04f;
 //       //float bot2_t = zBot2 / zFinish;
 //       //fill_R.fillAmount = bot2_t;
 //       //icon_R.anchoredPosition = new Vector2(Mathf.Lerp(-183.0f, 183.0f, bot2_t), 0.0f);

 //       Dictionary<float, Transform> dic = new Dictionary<float, Transform>();
 //       dic.Add(player_t, fill_Y.transform);
 //       dic.Add(bot1_t, fill_B.transform);
 //  //     dic.Add(bot2_t, fill_R.transform);

 //       List<float> listT = new List<float>();
 //       listT.Add(player_t);
 //       listT.Add(bot1_t);
 ////       listT.Add(bot2_t);
 //       listT.Sort();
 //       listT.Reverse();

 //       for(int i = 0; i < dic.Count; i++)
 //       {
 //           Transform _t;
 //           if (dic.TryGetValue(listT[i],out _t))
 //           {
 //               _t.SetSiblingIndex(i);
 //           }
 //       }
    }
}
