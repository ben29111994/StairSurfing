using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanielLochner.Assets.SimpleScrollSnap;

public class UI : MonoBehaviour
{
    public static UI Instance;

    public Sprite[] ingredientOpacity;
    public Sprite[] playerOpacity;
    public List<Ingredient> listIngredient = new List<Ingredient>();
    public List<PlayerP> listPlayerP = new List<PlayerP>();
    public List<Stack> listStack = new List<Stack>();

    public bool isPlayerP;

    public int LastIngredient
    {
        get
        {
            return PlayerPrefs.GetInt("last_ingredient");
        }
        set
        {
            PlayerPrefs.SetInt("last_ingredient",value);
        }
    }

    public int LastPlayerP
    {
        get
        {
            return PlayerPrefs.GetInt("LastPlayerP");
        }
        set
        {
            PlayerPrefs.SetInt("LastPlayerP", value);
        }
    }

    [Header("Input Data")]
    public List<IngredientTableObject> ingredientData = new List<IngredientTableObject>();
    public List<PlayerTableObject> playerData = new List<PlayerTableObject>();

    [Header("References")]
    public CompleteUI completeUI;
    public Wheel wheelScript;
    public GameObject chestUI;
    public SimpleScrollSnap simpleScrollSnap_Ingredient;
    public SimpleScrollSnap simpleScrollSnap_playerp;
    public UnlockIngredient unlockIngredientScript;
    public UnlockIngredientComplete unlockIngredientCompleteScript;
    public Animator mainAnimator;
    public Animator btnBuyCoinAnimator;
    public Animator btnBuyKeyAnimator;
    public bool justBuy;

    public GameObject ingredientTab;
    public GameObject stackTab;
    public GameObject offlineTab;

    public GameObject botIngredient;
    public GameObject botStack;
    public GameObject botOffline;

    public Ingredient ingredientPrefab;
    public GameObject pageIngredientPrefab;
    public Transform pageIngredientParent;
    public GameObject btnBuy_Coin;
    public GameObject btnBuy_Key;

    public PlayerP playerPPrefab;
    public GameObject pagePlayerPrefab;
    public Transform pagePlayerParent;

    public GameObject dotPrefab;
    public Transform paginationIngredientParent;
    public Transform paginationStackParent;

    public GameObject popup3keys;
    public GameObject dailyQuestOject;


    public int CurrentSelectStep
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentSelectStep");
        }
        set
        {
            PlayerPrefs.SetInt("CurrentSelectStep", value);
        }
    }

    public int CurrentSelectPlayer
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentSelectPlayer");
        }
        set
        {
            PlayerPrefs.SetInt("CurrentSelectPlayer", value);
        }
    }

    public void SetIngredient(int number)
    {
        PlayerPrefs.SetInt("ingredient" + number,1);
    }

    public int GetIngredientNumber(int number)
    {
        return PlayerPrefs.GetInt("ingredient" + number);
    }

    public void SetPlayerP(int number)
    {
        PlayerPrefs.SetInt("playerp" + number, 1);
    }

    public int GetPlayerPNumber(int number)
    {
        return PlayerPrefs.GetInt("playerp" + number);
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("FirstTimePlay") == 0)
        {
            PlayerPrefs.SetInt("FirstTimePlay", 1);
            LastIngredient = -1;
             CurrentSelectPlayer = -1;
              CurrentSelectStep = -1;
        }

        GenerateIngredientFromData();
        GeneratePlayerFromData();
        UpdateShop();

        if(CurrentSelectStep == -1)
        {
            GameManager.Instance.stepSkinIndex = -1;
        }
        else
        {
            listIngredient[CurrentSelectStep].SelectButton();
        }

        if(CurrentSelectPlayer == -1)
        {
            GameManager.Instance.stickmanSkinIndex = -1;
        }
        else
        {
            listPlayerP[CurrentSelectPlayer].SelectButton();
        }

        ShowMainUI();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.D)) PlayerPrefs.DeleteAll();
        //if (Input.GetKeyDown(KeyCode.S)) GetIngredientData();
        //if (Input.GetKeyDown(KeyCode.L)) Debug.Log("Last Ingredient (NEW)   " + LastIngredient);
        //if (Input.GetKeyDown(KeyCode.R)) ShowComplete(200,500,false,true,true);
        //if (Input.GetKeyDown(KeyCode.P)) ShowPopup3Keys();
        //if (Input.GetKeyDown(KeyCode.C)) UIManager.Instance.Coin += 9999;
    }

    private void GenerateIngredientFromData()
    {
        GameObject page = null;

        int indexPage = 0;
        int maxPage = (int)(ingredientData.Count / 9);

        for(int i = 0; i < ingredientData.Count; i++)
        {
            if(i % 9 == 0)
            {
                indexPage++;
                page = Instantiate(pageIngredientPrefab, pageIngredientParent);
                Instantiate(dotPrefab, paginationIngredientParent);
            }

            Ingredient _ingredient = Instantiate(ingredientPrefab, page.transform.GetChild(0));
            int ID = i;
            Sprite icon = ingredientData[i].iconIngredient;
            Sprite opacity = ingredientOpacity[i];
            string name = ingredientData[i].nameIngredient;
            // bool isChest = (indexPage < maxPage - 1) ? false : false;
            bool isChest = false;
            _ingredient.Init(ID, icon, opacity, name,isChest);
            listIngredient.Add(_ingredient);
        }
    }

    private void GeneratePlayerFromData()
    {
        GameObject page = null;

        int indexPage = 0;
        int maxPage = (int)(playerData.Count / 9);

        for (int i = 0; i < playerData.Count; i++)
        {
            if (i % 9 == 0)
            {
                indexPage++;
                page = Instantiate(pageIngredientPrefab, pagePlayerParent);
                Instantiate(dotPrefab, paginationStackParent);
            }

            PlayerP _playerP = Instantiate(playerPPrefab, page.transform.GetChild(0));
            int ID = i;
            Sprite icon = playerData[i].iconIngredient;
            Sprite opacity = playerOpacity[i];
            string name = playerData[i].nameIngredient;
            // bool isChest = (indexPage < maxPage - 1) ? false : false;
            bool isChest = false;
            _playerP.Init(ID, icon, opacity, name, isChest);
            listPlayerP.Add(_playerP);
        }
    }

    public void UnSelectAllPlayerP()
    {
        for (int i = 0; i < listPlayerP.Count; i++)
        {
            listPlayerP[i].UnselectIngredient();
        }
    }

    public void UnselectAllIngredient()
    {
        for(int i = 0; i < listIngredient.Count; i++)
        {
            listIngredient[i].UnselectIngredient();
        }
    }

    public void UpdateShop()
    {
        int count = listIngredient.Count;

        for(int i = 0; i < count; i++)
        {
            listIngredient[i].UpdateIngredient();
        }

        for(int i = 0; i < listPlayerP.Count; i++)
        {
            listPlayerP[i].UpdatePlayerP();
        }
    }

    public void OnClick_UnlockCoin()
    {
        btnBuyCoinAnimator.SetTrigger("Bubble");

        // check xem da unlock het chua , neu unlock het roi return
        if (CanUnlock() == false) return;

        // kiem tra coin , neu ko du coin return
        int price = 400;
        int myCoin = UIManager.Instance.Coin;

        if (myCoin < price) return;

        Debug.Log("unlock random ingredient _ COIN");
        // myCoin -= price;
        UIManager.Instance.Coin -= price;

        UnlockIngredient(false);
    }


    public void OnClick_UnlockCoin_PlayerP()
    {
        btnBuyCoinAnimator.SetTrigger("Bubble");

        // check xem da unlock het chua , neu unlock het roi return
        if (CanUnlockPlayer() == false) return;

        // kiem tra coin , neu ko du coin return
        int price = 400;
        int myCoin = UIManager.Instance.Coin;

        if (myCoin < price) return;

        Debug.Log("unlock random ingredient _ COIN");
        // myCoin -= price;
        UIManager.Instance.Coin -= price;

        UnlockPlayerP(false);
    }

    public void OnClick_UnlockKey()
    {
        btnBuyKeyAnimator.SetTrigger("Bubble");

        // check xem da unlock het chua , neu unlock het roi return
        if (CanUnlock() == false) return;

        // kiem tra coin , neu ko du coin return
        int price = 3;
        int myKey = 999;

        if (myKey < price) return;

        Debug.Log("unlock random ingredient _ KEY");
        // myKey -= price;

        UnlockIngredient(true);
    }

    public bool CanUnlock()
    {
        int n = 0;

        for (int i = 0; i < ingredientData.Count; i++)
        {
            if (GetIngredientNumber(i) == 1)
            {
                n++;
            }
        }

        if (n == ingredientData.Count)
        {
            Debug.Log("ban da unlock het ingredient");
            return false;
        }

        return true;
    }

    private bool CanUnlockPlayer()
    {
        int n = 0;


        for (int i = 0; i < playerData.Count; i++)
        {
            if (GetPlayerPNumber(i) == 1)
            {
                n++;
            }
        }

        if (n == playerData.Count)
        {
            Debug.Log("ban da unlock het ingredient");
            return false;
        }

        return true;
    }

    public void UnlockIngredientWhenComplete()
    {
        if (CanUnlock() == false) return;

        // random value
        int randomValue = 0;
        int numberLoop = 0;
        bool isChest = false; // only random normal ingredient (not chest)

        for (int i = 0; i < 1; i++)
        {
            randomValue = Random.Range(0, ingredientData.Count);

            if (GetIngredientNumber(randomValue) == 1 || listIngredient[randomValue].isChest != isChest)
            {
                randomValue = Random.Range(0, ingredientData.Count);
                i--;

                numberLoop++;
                if (numberLoop >= 100) return;
            }
        }

        Debug.Log("UNLOCKED");

        LastIngredient = randomValue;

        // unlock
        SetIngredient(randomValue);
        listIngredient[randomValue].UpdateIngredient();
        // chay animation (popup)
        unlockIngredientScript.OnAwake("", listIngredient[randomValue].iconImg.sprite);
       // unlockIngredientCompleteScript.OnAwake(listIngredient[randomValue].nameText.text, listIngredient[randomValue].iconImg.sprite);
    }

    private void UnlockPlayerP(bool isChest)
    {
        justBuy = true;

        // random value
        int randomValue = 0;
        int numberLoop = 0;

        for (int i = 0; i < 1; i++)
        {
            randomValue = Random.Range(0, listPlayerP.Count);

            if (GetPlayerPNumber(randomValue) == 1 || listPlayerP[randomValue].isChest != isChest)
            {
                randomValue = Random.Range(0, listPlayerP.Count);
                i--;
                numberLoop++;
                Debug.Log(numberLoop);
                if (numberLoop >= 200) return;
            }
        }

        Debug.Log("ingredient  " + randomValue + "  UNLOCKED");

        LastPlayerP = randomValue;

        // unlock
        SetPlayerP(randomValue);
        listPlayerP[randomValue].UpdatePlayerP();

        // chay animation
        unlockIngredientScript.OnAwake("", listPlayerP[randomValue].iconImg.sprite);

        // set value when onlick OK will be call move to target page
        currentPage = simpleScrollSnap_playerp.TargetPanel;
        targetPage = (int)(randomValue / 9);
        step = Mathf.Abs(currentPage - targetPage);
        isNext = (targetPage - currentPage > 0) ? true : false;
    }

    private void UnlockIngredient(bool isChest)
    {
        justBuy = true;

        // random value
        int randomValue = 0;
        int numberLoop = 0;

        for (int i = 0; i < 1; i++)
        {
            randomValue = Random.Range(0, ingredientData.Count);

            if (GetIngredientNumber(randomValue) == 1 || listIngredient[randomValue].isChest != isChest)
            {
                randomValue = Random.Range(0, ingredientData.Count);
                i--;

                if (numberLoop >= 200) return;
            }
        }

        Debug.Log("ingredient  " + randomValue + "  UNLOCKED");
        
        LastIngredient = randomValue;

        // unlock
        SetIngredient(randomValue);
        listIngredient[randomValue].UpdateIngredient();

        // chay animation
        unlockIngredientScript.OnAwake(listIngredient[randomValue].nameText.text, listIngredient[randomValue].iconImg.sprite);

        // set value when onlick OK will be call move to target page
        currentPage = simpleScrollSnap_Ingredient.TargetPanel;
        targetPage = (int)(randomValue / 9);
        step = Mathf.Abs(currentPage - targetPage);
        isNext = (targetPage - currentPage > 0) ? true : false;
    }

    private int currentPage;
    private int targetPage;
    private int step;
    private bool isNext;

    public void MoveToTargetPage()
    {
        StartCoroutine(C_MovePageToTarget(step, isNext));
    }

    private IEnumerator C_MovePageToTarget(int step,bool isNext)
    {
        if (step == 0)
        {
            listIngredient[LastIngredient].ShowAnimationOutline(0.1f);
            yield break;
        }

        if (isNext)
        {
            for (int i = 0; i < step; i++)
            {
                simpleScrollSnap_Ingredient.GoToNextPanel();
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            for (int i = 0; i < step; i++)
            {
                simpleScrollSnap_Ingredient.GoToPreviousPanel();
                yield return new WaitForSeconds(0.1f);
            }
        }

        listIngredient[LastIngredient].ShowAnimationOutline(0.2f);
    }

    public void MoveToTargetPagePlayerP()
    {
        listPlayerP[LastPlayerP].ShowAnimationOutline(0.2f);
    }

    //private IEnumerator C_MoveToTargetPagePlayerP()
    //{

    //}

    public void OnClick_OpenShop_IngredientTab()
    {
        OnClick_ShowIngredientTab();
        mainAnimator.SetTrigger("Active");
    }

    public void OnClick_OpenShop_StackTab()
    {
        OnClick_ShowStackTab();
        mainAnimator.SetTrigger("Active");
    }

    public void OnClick_OpenShop_OfflineTab()
    {
        OnClick_ShowOfflineTab();
        mainAnimator.SetTrigger("Active");
    }

    public void OnClick_CloseShop()
    {
        mainAnimator.SetTrigger("UnActive");
    }

    public void ShowMainUI()
    {
        mainAnimator.SetTrigger("ShowBottom");
    }

    public void HideMainUI()
    {
        mainAnimator.SetTrigger("HideBottom");
    }

    public void OnClick_ShowIngredientTab()
    {
        isPlayerP = false;
        ingredientTab.SetActive(true);
        stackTab.SetActive(false);
      //  offlineTab.SetActive(false);

        botIngredient.SetActive(false);
        botStack.SetActive(true);
    //    botOffline.SetActive(true);
    }

    public void OnClick_ShowStackTab()
    {
        isPlayerP = true;
        ingredientTab.SetActive(false);
        stackTab.SetActive(true);
//offlineTab.SetActive(false);

        botIngredient.SetActive(true);
        botStack.SetActive(false);
   //     botOffline.SetActive(true);
    }

    public void OnClick_ShowOfflineTab()
    {
        ingredientTab.SetActive(false);
        stackTab.SetActive(false);
 //       offlineTab.SetActive(true);

        botIngredient.SetActive(true);
        botStack.SetActive(true);
    //    botOffline.SetActive(false);
    }

    public void GetIngredientData()
    {
        for(int i = 0; i < listStack.Count; i++)
        {
            int ID = listStack[i].ID;
            int stankNunber = listStack[i].StackNumber;
            Debug.Log("Ingredient(" + ID + ")" + "___Stack " + stankNunber);
        }
    }

    public void ShowComplete(int feet,int coinEarn,bool isMiss,bool isNewHeight, bool isShowPopupUnlockIngredient)
    {
        StartCoroutine(C_ShowComplete(feet, coinEarn, isMiss, isNewHeight, isShowPopupUnlockIngredient));
    }

    private IEnumerator C_ShowComplete(int feet, int coinEarn, bool isMiss, bool isNewHeight, bool isShowPopupUnlockIngredient)
    {
        if (isShowPopupUnlockIngredient)
        {
            UnlockIngredientWhenComplete();
            yield return new WaitForSeconds(2.0f);
        }

        completeUI.ShowComplete(feet, coinEarn, isMiss, isNewHeight);
    }

    public void ShowPopup3Keys()
    {
        popup3keys.SetActive(true);
    }

    public void OnClick_OpenToChestRoom()
    {
        popup3keys.SetActive(false);
        chestUI.SetActive(true);
    }

    public void ShowPopup3Key()
    {
        popup3keys.SetActive(true);
    }

    public void OnClick_OpenWheel()
    {
        wheelScript.gameObject.SetActive(true);
    }

    public void OnClick_CloseWheel()
    {
        wheelScript.gameObject.SetActive(false);
    }

    public void OnClick_OpenDailyQuest()
    {
        dailyQuestOject.SetActive(true);
    }

    public void OnClick_CloseDailyQuest()
    {
        dailyQuestOject.SetActive(false);
    }
}
