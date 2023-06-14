using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    public Transform playerResultParent;
    public PlayerResult playerResultPrefab;
    public PlayerResult myResult;
    public List<PlayerResult> listPlayerResult = new List<PlayerResult>();
    public Text levelBoardText;
    public string[] levelBoardString;
    public CoinUI coinUI;
    public Text giftCoin;
    public GameObject buttonContinue;
    public GameObject buttonTextContinue;

    private int LevelBoard
    {
        get
        {
            return PlayerPrefs.GetInt("LevelBoard");
        }
        set
        {
            PlayerPrefs.SetInt("LevelBoard", value);
        }
    }

    private void OnEnable()
    {
        ShowResult();
    }

    public void ShowResult()
    {
        if(PlayerPrefs.GetInt("PlayTheFirstTime") == 0)
        {
            SetPlayerSult();
            PlayerPrefs.SetInt("PlayTheFirstTime", 1);
        }

        buttonContinue.SetActive(false);
        buttonTextContinue.SetActive(false);

        if (LevelBoard >= levelBoardString.Length) LevelBoard = levelBoardString.Length - 1;
        levelBoardText.text = levelBoardString[LevelBoard];

        if(listPlayerResult.Count != 0)
        {
            for(int i = 0; i < listPlayerResult.Count; i++)
            {
                Destroy(listPlayerResult[i].gameObject);
            }
        }

        listPlayerResult.Clear();

        playerResultParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, 0.0f);

        for(int i = 0; i < 25; i++)
        {
            PlayerResult _pr = Instantiate(playerResultPrefab, playerResultParent);
            int _score = (25 - i) * 200;
            string _name = UIManager.Instance.listName[PlayerPrefs.GetInt("player_result" + i)];
            Sprite _flag = UIManager.Instance.flagsSpr[PlayerPrefs.GetInt("player_result" + i)];
            _pr.Init(i + 1, _name, _score, _flag);
            listPlayerResult.Add(_pr);         
        }
            
        gameObject.SetActive(true);

        SetMyResult();
    }

    private void SetMyResult()
    {
        StartCoroutine(C_SetMyResult());
    }

    private IEnumerator C_SetMyResult()
    {
        int myStep = PlayerPrefs.GetInt("myStep");

        if (myStep == 0)
        {
            giftCoin.text = "Gift: " + 50;
            playerResultParent.GetComponent<RectTransform>().anchoredPosition = Vector2.up * 3360f;
            myResult.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -405.0f);

            int stt = 24;
            int score = (25 - stt) * 200;
            int targetStt = Random.Range(15, 19);
            PlayerPrefs.SetInt("CurrentStt", targetStt);

            myResult.Init(stt + 1, "Player", score, UIManager.Instance.flagPlayer);
            listPlayerResult[stt].HideProfile(false);
            listPlayerResult[targetStt].HideProfile(false);

            Vector2 targetPos = Vector2.up * (targetStt * 175.0f - 525.0f + (175.0f / 2.0f));
            Vector2 myTargetPos = Vector2.up * -30.0f;

            yield return new WaitForSeconds(0.25f);
            myResult.transform.DOScale(Vector2.one * 1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
            yield return new WaitForSeconds(0.5f);
            listPlayerResult[stt].HideProfile(true);
            coinUI.CoinAnimation(50);
            score = (25 - targetStt) * 200;
            myResult.Init(targetStt + 1, "Player", score, UIManager.Instance.flagPlayer);
            playerResultParent.GetComponent<RectTransform>().DOAnchorPos(targetPos, 1.0f).SetEase(Ease.Flash);
            myResult.GetComponent<RectTransform>().DOAnchorPos(myTargetPos, 1.0f).SetEase(Ease.Flash).OnComplete(() => myResult.transform.DOScale(Vector2.one * 1.1f, 0.2f).SetLoops(2, LoopType.Yoyo));
        }
        else if (myStep == 1)
        {
            giftCoin.text = "Gift: " + 100;
            int stt = PlayerPrefs.GetInt("CurrentStt");
            int score = (25 - stt) * 200;
            int targetStt = Random.Range(5, 8);
            PlayerPrefs.SetInt("CurrentStt", targetStt);

            Vector2 targetPos2 = Vector2.up * (stt * 175.0f - 525.0f + (175.0f / 2.0f));

            playerResultParent.GetComponent<RectTransform>().anchoredPosition = Vector2.up * (stt * 175.0f - 525.0f + (175.0f / 2.0f));
            myResult.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -30.0f);
      
            myResult.Init(stt + 1, "Player", score, UIManager.Instance.flagPlayer);
            listPlayerResult[stt].HideProfile(false);
            listPlayerResult[targetStt].HideProfile(false);

            Vector2 targetPos = Vector2.up * (targetStt * 175.0f - 475.0f + (175.0f / 2.0f));

            yield return new WaitForSeconds(0.25f);
            myResult.transform.DOScale(Vector2.one * 1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
            yield return new WaitForSeconds(0.5f);
            listPlayerResult[stt].HideProfile(true);
            coinUI.CoinAnimation(100);

            score = (25 - targetStt) * 200;
            myResult.Init(targetStt + 1, "Player", score, UIManager.Instance.flagPlayer);
            playerResultParent.GetComponent<RectTransform>().DOAnchorPos(targetPos, 1.0f).SetEase(Ease.Flash).OnComplete(() => myResult.transform.DOScale(Vector2.one * 1.1f, 0.2f).SetLoops(2, LoopType.Yoyo));
        }
        else if ( myStep == 2)
        {
            giftCoin.text = "Gift: " + 200;
            int stt = PlayerPrefs.GetInt("CurrentStt");
            int score = (25 - stt) * 200;
            int targetStt = 0;
            PlayerPrefs.SetInt("CurrentStt", targetStt);

            Vector2 targetPos2 = Vector2.up * (stt * 175.0f - 525.0f + (175.0f / 2.0f));

            playerResultParent.GetComponent<RectTransform>().anchoredPosition = Vector2.up * (stt * 175.0f - 475.0f + (175.0f / 2.0f));
            myResult.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, -30.0f);

            myResult.Init(stt + 1, "Player", score, UIManager.Instance.flagPlayer);
            listPlayerResult[stt].HideProfile(false);
            listPlayerResult[targetStt].HideProfile(false);

            Vector2 targetPos = Vector2.zero;
            Vector2 myTargetPos = Vector2.up * 320.0f;

            yield return new WaitForSeconds(0.25f);
            myResult.transform.DOScale(Vector2.one * 1.1f, 0.2f).SetLoops(2, LoopType.Yoyo);
            yield return new WaitForSeconds(0.5f);
            listPlayerResult[stt].HideProfile(true);
            coinUI.CoinAnimation(200);

            score = (25 - targetStt) * 200;
            myResult.Init(targetStt + 1, "Player", score, UIManager.Instance.flagPlayer);
            playerResultParent.GetComponent<RectTransform>().DOAnchorPos(targetPos, 1.0f).SetEase(Ease.Flash);
            myResult.GetComponent<RectTransform>().DOAnchorPos(myTargetPos, 1.0f).SetEase(Ease.Flash).OnComplete(() => myResult.transform.DOScale(Vector2.one * 1.1f, 0.2f).SetLoops(2, LoopType.Yoyo));

            yield return new WaitForSeconds(1.2f);

            for(int i = 0; i < 10; i++)
            {
                Transform t = listPlayerResult[i].transform;
                t.DOScale(Vector2.one * 1.15f, 0.2f).SetLoops(2, LoopType.Yoyo);
                yield return new WaitForSeconds(0.1f);
            }
        }

        myStep++;
        if (myStep >= 3)
        {
            LevelBoard++;
            myStep = 0;
            SetPlayerSult();
        }
        PlayerPrefs.SetInt("myStep",myStep);

        yield return new WaitForSeconds(1.2f);
        buttonContinue.SetActive(true);
        buttonTextContinue.SetActive(true);
    }

    public void SetPlayerSult()
    {
        int[] _sps = RandomIntArray(25,150);

        for(int i = 0; i < _sps.Length; i++)
        {
            PlayerPrefs.SetInt("player_result" + i, _sps[i]);
        }
    }

    private int[] RandomIntArray(int lenght,int maxNumber)
    {
        int[] newArray = new int[lenght];

        for (int i = 0; i < lenght; i++)
        {
            int r = Random.Range(0, maxNumber);

            if (i != 0)
            {
                for (int j = 0; j < i; j++)
                {
                    if (newArray[j] == r)
                    {
                        r = Random.Range(0, maxNumber);
                        j = -1;
                    }
                }
            }

            newArray[i] = r;
        }

        return newArray;
    }
}
