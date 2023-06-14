using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
using DG.Tweening;
using UnityEngine.UI;
using VacuumShaders.CurvedWorld;
using VacuumShaders.CurvedWorld.Example;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isGenerateMap;

    [Header("Status Game")]
    public bool isStart;
    public bool isComplete;
    public bool isFail;
    public bool isTutorial;
    public int tutorialIndex;
    private bool isShakeCamera;
    private bool isVibration;
    public bool isChesting;
    public bool isCurveWorld;

    [Header("Level Manager")]
    public GameObject levelObject;
    public GameObject[] levelObjects;
    public int levelGame;
    public Text levelText;
    private int levelFixed;
    public float startPositionZ;
    public float finishPositionZ;
    public int stepSkinIndex;
    public int stickmanSkinIndex;
    public int[] levelShowChallengePopup;
    public BEND_TYPE bendType;

    [Header("References")]
    public DragControl dragControl;
    public MeshFilter stepCVBB;
    public Transform pivotFadedWall;
    public Transform currentPosFadedWall;
    public GenerateMap generateMap;
    public Follow followCam;
    public Transform trailObject;
    public TrailRenderer yellowTrail;
    public Transform fakeOffset;
    public Transform stepParent;
    public Transform offsetCamera;
    public Transform loadingObject;
    public Stickman stickManPrefab;
    public List<Stickman> listStickMan = new List<Stickman>();

    public GameObject keyObject;
    public GameObject keyPrefab;

    public GameObject coinPrefab;
    public Transform coinParent;

    public ParticleSystem[] starBurstTrail;
    public Transform trailPlayer;
    public Congra congra;

    public GameObject newWorldPopup;

    [Header("Material")]
    public Material whiteStair;
    public Material yellowStair;
    public Material redStair;
    public Material redStairFade;

    private int numberWin;
    private static bool IsOpenApplication;
    public static int FinishAnalyticIndex;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Instance = this;
        Time.timeScale = 1.0f;

        MMVibrationManager.iOSInitializeHaptics();
    }
    
    private void Start()
    {
        if (IsOpenApplication == false)
        {
            IsOpenApplication = true;
            //FlurryAnalytics.Instance.OpenAppEvent();
        }

        levelGame = PlayerPrefs.GetInt("levelGame");
        levelText.text = "Lv." + (levelGame + 1);
        levelFixed = levelGame;
        if(levelFixed >= levelObjects.Length)
        {
            levelFixed = Random.Range(14, levelObjects.Length);
        }

        if(levelGame == 0)
        {
            isTutorial = true;
        }

        GenerateLevel();
    }

    private void Update()
    {
        if (isComplete) return;

        for(int i = 0; i < listStickMan.Count; i++)
        {
            listStickMan[i].UpdateStep();
        }
 
        SetTrail();
        UpdateFadeWall();

        if(listStickMan.Count > 1)
        UpdateFadeWall_Bot();

        UIManager.Instance.UpdateFillBarStep();
    }

    private void FixedUpdate()
    {

    }

    private void LateUpdate()
    {
        if (isComplete) return;

        CameraFollow();
        TrailFollow();
    }

    private void LevelUp()
    {
        levelGame++;

        //FlurryAnalytics.Instance.Event_LevelAchieved(levelGame);

        levelFixed = levelGame;
        if (levelFixed >= levelObjects.Length)
        {
            levelFixed = Random.Range(14, levelObjects.Length);
        }

        PlayerPrefs.SetInt("levelGame", levelGame);
        ColorManager.Instance.ChangeColor();
    }

    public void Complete(int _number)
    {
        if (isComplete || isFail) return;

        isComplete = true;
        StartCoroutine(C_Complete(_number));

        FinishAnalyticIndex++;
        if(FinishAnalyticIndex >= 2)
        {
            //FlurryAnalytics.Instance.Event_Finish();
        }
    }

    private IEnumerator C_Complete(int _number)
    {
        for(int i = 0; i < listStickMan.Count; i++)
        {
            listStickMan[i].HideUI();
            listStickMan[i].StopMove();
        }

        Transform _target = listStickMan[_number].transform;
        Camera.main.transform.DOLocalRotate(new Vector3(20, 0, 0), 1.0f);
        Camera.main.transform.DOLocalMove(new Vector3(0.0f, 4.0f, -6.0f), 1.0f);

        if (isCurveWorld)
        {
            //if(_number == 0)
            //{
            //    Vector3 tarPos = CurvedWorld_Controller.current.TransformPosition(stickMan[0].transform.position, bendType);
            //  //  offsetCamera.transform.DOMove(tarPos, 1.0f);
            //}
            //else
            //{
            //    Vector3 tarPos = CurvedWorld_Controller.current.TransformPosition(stickManBot_1.transform.position, bendType);
            ////    offsetCamera.transform.DOMove(tarPos, 1.0f);
            //}
        }
        else
        {
            offsetCamera.transform.DOMove(_target.position, 1.0f);
        }

        if (_number != 0)
        {
            isComplete = false;
            string _name = (_number == 1) ? UIManager.Instance.nameEnemy + " wiNs" : UIManager.Instance.nameEnemy2 + " wiNs";
            UIManager.Instance.headerLoseText.text = _name.ToUpper();
            Fail();
            yield break;
        }

        yield return new WaitForSeconds(1.0f);


        LevelUp();
        UIManager.Instance.FireWorkEffect();

        if(levelGame == 5)
        {
            yield return C_ShowNewWorldPopup();
        }
        else if (levelGame == 10)
        {
            yield return C_ShowNewWorldPopup();
        }

        //bool isShowChallengePopup = false;
        //for(int i = 0; i < levelShowChallengePopup.Length; i++)
        //{
        //    if(levelGame == levelShowChallengePopup[i])
        //    {
        //        isShowChallengePopup = true;
        //        break;
        //    }
        //}

        //if (isShowChallengePopup)
        //{
        //    UI.Instance.unlockIngredientCompleteScript.gameObject.SetActive(true);
        //    yield return new WaitForSeconds(2.5f);
        //}


        UpdateKey();

        while (isChesting) yield return null;

        yield return new WaitForSeconds(0.2f);
        UI.Instance.chestUI.SetActive(false);

        UIManager.Instance.Show_Win_UI(3);
    }

    private IEnumerator C_ShowNewWorldPopup()
    {
        newWorldPopup.SetActive(true);
        yield return new WaitForSeconds(2.4f);
        newWorldPopup.SetActive(false);
    }

    public void Fail()
    {
        if (isComplete || isFail) return;

        FinishAnalyticIndex = 0;
        Debug.Log("__ FAIL __");
        isFail = true;
        StartCoroutine(C_Fail());
    }

    private IEnumerator C_Fail()
    {
        yield return new WaitForSeconds(1.0f);
        UIManager.Instance.Show_Lose_UI(3);
    }

    private void GenerateLevel()
    {
        //FlurryAnalytics.Instance.Event_Start();

        int stickManAmount = 5;
        for(int i = 0; i < stickManAmount; i++)
        {
            Stickman stickMan = Instantiate(stickManPrefab);
            stickMan.number = i;
            Vector3 posStickMan = stickMan.transform.position;

            if (i == 0)
            {
                posStickMan.x = 0.0f;
            }
            else if(i == 1)
            {
                posStickMan.x = -3.0f;
            }
            else if (i == 2)
            {
                posStickMan.x = -1.5f;
            }
            else if (i == 3)
            {
                posStickMan.x = 1.5f;
            }
            else if (i == 4)
            {
                posStickMan.x = 3.0f;
            }
            posStickMan.z += i * 2.0f;


            stickMan.transform.position = posStickMan;
            listStickMan.Add(stickMan);
        }

        ActiveStickMan(false);

        if (isGenerateMap)
        {
          //  generateMap.GenerateStairLevel(0);
        }
        else
        {
            //if (levelGame >= 14)
            //{
            //    if (Random.value > 0.5f)
            //        curveWorld[0].SetActive(true);
            //}

            levelObject = Instantiate(levelObjects[levelFixed]);
        }

        for(int i = 0; i < levelObject.transform.childCount; i++)
        {
            if(levelObject.transform.GetChild(i).gameObject.name == "CV_Control")
            {
               // isCurveWorld = true;
            }
        }

    //    GenerateCoin();
        GenerateKey();
    }

    public void ActiveStickMan(bool isActive)
    {
        if (isActive)
        {
            for (int i = 0; i < listStickMan.Count; i++)
            {
                listStickMan[i].gameObject.SetActive(isActive);
            }


            //if(isTutorial == false)
            //StartCoroutine(C_LoadingBot());
            //  stickManBot_2.gameObject.SetActive(isActive);
        }
        else
        {
            for(int i = 0; i < listStickMan.Count; i++)
            {
                listStickMan[i].gameObject.SetActive(isActive);
            }
        }
    }

    private IEnumerator C_LoadingBot()
    {
        listStickMan[0].gameObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        loadingObject.gameObject.SetActive(false);

        for (int i = 1; i < listStickMan.Count; i++)
        {
            listStickMan[i].gameObject.SetActive(true);
        }
    }

    public void PlayGame()
    {
        isStart = true;

        for (int i = 0; i < listStickMan.Count; i++)
        {
            listStickMan[i].StartRun();
        }
    }

    private void CameraFollow()
    {
        if (isComplete || isFail) return;
        if (listStickMan[0].isFinish || listStickMan[0].isDead) return;

        if (isCurveWorld == false)
        {
            Vector3 tarPos = listStickMan[0].transform.position;
            tarPos.x = 0.0f;
            offsetCamera.position = Vector3.Lerp(offsetCamera.position, tarPos, Time.deltaTime * 4.0f);
        }
        else
        {
            followCam.UpdateStep();
            //Vector3 tarPos = CurvedWorld_Controller.current.TransformPosition(stickMan.transform.position, bendType);
            //offsetCamera.position = Vector3.Lerp(offsetCamera.position, tarPos, Time.deltaTime * 4.0f);
            //offsetCamera.rotation = CurvedWorld_Controller.current.TransformRotation(stickMan.transform.position, stickMan.transform.forward, stickMan.transform.right, bendType);
        }

        pivotFadedWall.transform.position = listStickMan[0].transform.position;
    }

    private void SetTrail()
    {
        Vector3 tarpos = trailPlayer.position;

        if (isCurveWorld)
        {
            tarpos = CurvedWorld_Controller.current.TransformPosition(listStickMan[0].transform.position, bendType);
            trailPlayer.transform.rotation = CurvedWorld_Controller.current.TransformRotation(listStickMan[0].transform.position, listStickMan[0].transform.forward, listStickMan[0].transform.right, bendType);

        }
        else
        {
            tarpos = listStickMan[0].transform.position;
        }

        trailPlayer.position = tarpos;
    }

    public void CubeBreakEffect(Transform _position,Material _m)
    {
        Vector3 positionFixed = Vector3.zero;

        if (isCurveWorld)
        {
            positionFixed = CurvedWorld_Controller.current.TransformPosition(_position.position, bendType);
        }
        else
        {
            positionFixed = _position.position;
        }

        GameObject _obj = PoolManager.Instance.GetObject(PoolManager.NameObject.cubeBreak);
        _obj.transform.position = positionFixed;

        ParticleSystemRenderer _p = _obj.GetComponent<ParticleSystemRenderer>();
        _p.material = _m;
        
        StartCoroutine(C_Active(_obj, 1.0f));
    }

    public void PlayerBreakEffect(Transform _position, Material _m)
    {
        Vector3 positionFixed = Vector3.zero;

        if (isCurveWorld)
        {
            positionFixed = CurvedWorld_Controller.current.TransformPosition(_position.position, bendType);
        }
        else
        {
            positionFixed = _position.position;
        }

        GameObject _obj = PoolManager.Instance.GetObject(PoolManager.NameObject.playerBreak);
        _obj.transform.position = positionFixed;

        ParticleSystemRenderer _p = _obj.GetComponent<ParticleSystemRenderer>();
        _p.material = _m;

        StartCoroutine(C_Active(_obj, 1.0f));
    }

    public void StarBurstEffect(Vector3 _position,int _number)
    {
        return;


        GameObject _obj = null;

        if (_number == 0)
        {
            _obj = PoolManager.Instance.GetObject(PoolManager.NameObject.starBurstYellow);
        }
        else if (_number == 1)
        {
            _obj = PoolManager.Instance.GetObject(PoolManager.NameObject.starBurstBlue);
        }
        else if (_number == 2)
        {
            _obj = PoolManager.Instance.GetObject(PoolManager.NameObject.starBurstRed);
        }

        Vector3 positionFixed = Vector3.zero;

        if (isCurveWorld)
        {
            positionFixed = CurvedWorld_Controller.current.TransformPosition(_position, bendType);
        }
        else
        {
            positionFixed = _position;
        }

        _obj.transform.position = positionFixed;



        StartCoroutine(C_Active(_obj, 1.0f));
    }

    private IEnumerator C_Active(GameObject _obj,float _time)
    {
        _obj.SetActive(true);
        yield return new WaitForSeconds(_time);
        _obj.SetActive(false);
    }

    public void Vibration()
    {
        if (isVibration) return;

        StartCoroutine(C_Vibration());
    }

    private IEnumerator C_Vibration()
    {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
        isVibration = true;
        yield return new WaitForSeconds(0.2f);
        isVibration = false;
    }

    public void ShakeCamera()
    {
        if (isShakeCamera) return;

        StartCoroutine(C_ShakeCamera());
    }

    private IEnumerator C_ShakeCamera()
    {
        Camera.main.transform.DOShakePosition(.4f, .2f);
        isShakeCamera = true;
        yield return new WaitForSeconds(0.2f);
        isShakeCamera = false;
    }

    public void GenerateKey()
    {
        UIManager.Instance.playUIScript.Key += 0;
        int _valueKey = PlayerPrefs.GetInt("Key" + levelGame);

        if (keyObject == null) return;

        if(_valueKey == 0)
        {         
            keyObject.SetActive(true);
        }
        else
        {
            keyObject.SetActive(false);
        }
        keyObject.SetActive(false);

        if (levelGame % 2 == 0)
        {
          //  keyObject.SetActive(false);
        }
    }

    public void SetKey()
    {
        UIManager.Instance.playUIScript.Key++;
        int _valueKey = PlayerPrefs.GetInt("Key" + levelGame);
        _valueKey++;
        PlayerPrefs.SetInt("Key" + levelGame,1);
    }

    public void UpdateKey()
    {
        if (UIManager.Instance.playUIScript.Key >= 3)
        {
            UIManager.Instance.playUIScript.Key = 3;
            isChesting = true;
            UIManager.Instance.coinUI.SetActive(true);
            UI.Instance.ShowPopup3Key();
        }
    }

    public void GenerateCoin()
    {
        int n = Random.Range(2, 5);
        int c = 0;

        if (levelObject == null) levelObject = generateMap.levelObject;

        for(int i = 1; i < levelObject.transform.childCount; i++)
        {
            StairGroup _sg = levelObject.transform.GetChild(i).GetComponent<StairGroup>();

            if(_sg != null)
            {
                if(Random.value > 0.5f)
                {
                    GenerateCoinFromGroup(_sg.transform);
                    c++;
                }
            }

            if (c == n) break;
        }       
    }

    public void GenerateCoinFromGroup(Transform parent)
    {
        for(int i = 0; i < parent.childCount; i++)
        {
            GameObject _coin = Instantiate(coinPrefab,coinParent);

            Vector3 stairPos = parent.transform.GetChild(i).transform.position;
            Vector3 pos = _coin.transform.position;
            pos.x = 1.0f;
            pos.y = stairPos.y + 4.2f;
            pos.z = stairPos.z;
            _coin.transform.position = pos;

            GameObject _coin2 = Instantiate(coinPrefab,coinParent);
            Vector3 pos2 = pos;
            pos2.x = -1.0f;
            _coin2.transform.position = pos2;

            if (i > 20)
            {
                break;
            }
        }
    }

    public void TrailFollow()
    {
        if (listStickMan[0].IsMoveToUpStair() || listStickMan[0].listStep.Count == 0 || listStickMan[0].IsMovingSpace())
        {
            yellowTrail.emitting = false;
        }
        else
        {
            yellowTrail.emitting = true;
        }

        Vector3 posFixed = Vector3.zero;

        if (isCurveWorld)
        {
            posFixed = CurvedWorld_Controller.current.TransformPosition(listStickMan[0].TrailPos(), bendType);
        }
        else
        {
            posFixed = listStickMan[0].TrailPos();
        }

        Vector3 p = new Vector3(offsetCamera.transform.position.x, posFixed.y - 0.15f, offsetCamera.transform.position.z - 0.6f);
        posFixed.y += 0.02f;
        trailObject.transform.position = posFixed;
    }

    public void CoinEffect(Vector3 pos)
    {
        Vector3 posFixed = Vector3.zero;

        if (isCurveWorld)
        {
            posFixed = CurvedWorld_Controller.current.TransformPosition(pos, bendType);
        }
        else
        {
            posFixed = pos;
        }    

        GameObject _coin = PoolManager.Instance.GetObject(PoolManager.NameObject.coinEffect);
        _coin.transform.position = offsetCamera.transform.position;
        StartCoroutine(C_ActiveObject(_coin, 1.0f));
    }

    private IEnumerator C_ActiveObject(GameObject _obj,float _time)
    {
        _obj.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _obj.SetActive(false);
    }

    private List<Stair> listStairSpace = new List<Stair>();
    private Stair stair;
    public LayerMask layerWall;

    public void UpdateFadeWall()
    {
        if(stair != null)
        {
        //    stair.ChangeRedStair();
            stair = null;
        }

        if(listStairSpace.Count != 0)
        {
            for(int i = 0; i < listStairSpace.Count; i++)
            {
          //      listStairSpace[i].ChangeRedStair();
            }

            listStairSpace.Clear();
        }

        Vector3 origin = currentPosFadedWall.position;
        Vector3 direction = listStickMan[0].transform.position - origin;
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        float distance = Vector3.Distance(listStickMan[0].transform.position, origin);

        if (Physics.SphereCast(ray, 0.5f, out hit, distance, layerWall))
        {
            if (hit.collider != null)
            {
                if(hit.collider.transform.parent.GetComponent<Wall>() != null)
                {
                    stair = hit.collider.GetComponent<Stair>();
                    stair.ChangeRedStairFade();
                }

                if(hit.collider.transform.parent.GetComponent<StairSpace>() != null)
                {
                    if (hit.collider.transform.position.y < listStickMan[0].transform.position.y) return;

                    for (int i = 0; i < hit.collider.transform.parent.childCount; i++)
                    {
                        listStairSpace.Add(hit.collider.transform.parent.GetChild(i).GetComponent<Stair>());
                        listStairSpace[i].ChangeRedStairFade();
                    }
                }
            }
        }
    }

    private Stair stair2;
    private List<Stair> listStairSpace2 = new List<Stair>();

    public void UpdateFadeWall_Bot()
    {
        if (stair2 != null)
        {
         //   stair2.ChangeRedStair();
            stair2 = null;
        }

        if (listStairSpace2.Count != 0)
        {
            for (int i = 0; i < listStairSpace2.Count; i++)
            {
          //      listStairSpace2[i].ChangeRedStair();
            }

            listStairSpace2.Clear();
        }

        Vector3 origin = currentPosFadedWall.position;
        Vector3 direction = listStickMan[1].transform.position - origin;
        Ray ray = new Ray(origin, direction);
        RaycastHit hit;
        float distance = Vector3.Distance(listStickMan[1].transform.position, origin);

        if (Physics.SphereCast(ray,0.5f, out hit, distance, layerWall))
        {
            if (hit.collider != null)
            {
                if (hit.collider.transform.parent.GetComponent<Wall>() != null)
                {
                    stair2 = hit.collider.GetComponent<Stair>();
                    stair2.ChangeRedStairFade();
                }

                if (hit.collider.transform.parent.GetComponent<StairSpace>() != null)
                {
                    if (hit.collider.transform.position.y < listStickMan[1].transform.position.y) return;

                    for (int i = 0; i < hit.collider.transform.parent.childCount; i++)
                    {
                        listStairSpace2.Add(hit.collider.transform.parent.GetChild(i).GetComponent<Stair>());
                        listStairSpace2[i].ChangeRedStairFade();
                    }
                }
            }
        }
    }
}