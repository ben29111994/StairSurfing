using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using VacuumShaders.CurvedWorld;

public class Stickman : MonoBehaviour
{
    [Header("Status")]
    public TypeStickMan typeStickMan;
    public int number;
    public bool isMoving;
    public bool isHold;
    public bool isFinish;
    public bool isGenerateStair;
    public bool isDead;
    public bool isRespawn;
    public bool isDelayTouchDown;

    [Header("Control")]
    public float moveSpeed;
    public int maximumStep;
    public int myMaxStep;
    public AnimationCurve animCurve;

    [Header("References")]
    public List<Step> listStep = new List<Step>();
    public List<Step> listAllStep = new List<Step>();
    public Animator anim;
    public Rigidbody rigid;
    public Collider collid;
    public Material stickManMaterial;
    public Material stickManMaterBreakMaterial;

    public Text numberStepText;
    public Image numberStep;
    public Transform fakeStep;
    public Transform checkPoint;
    public GameObject model_nonAvatar;
    public GameObject model_Avatar;
    public GameObject nameCharactorText;
    private DragControl dragControl;

    [Header("Layer")]
    public LayerMask layer1;
    public LayerMask layer2;
    public LayerMask layerKey;
    public LayerMask layerCoin;
    public LayerMask layerStickman;
    public LayerMask stepItem;
    public LayerMask layerStair;

    [Header("Materials")]
    public SkinnedMeshRenderer rendDancer;
    public SkinnedMeshRenderer rend;
    public Material[] defaultPlayer;
    public Material skinPlayer;
    public Texture[] skinTexture;
    public Material currentStepMaterial;

    public enum TypeStickMan
    {
        Player,
        Bot
    }

    private void Start()
    {
        typeStickMan = (number == 0) ? TypeStickMan.Player : TypeStickMan.Bot;
        transform.eulerAngles = Vector3.zero;
        StartCoroutine(C_Hide(nameCharactorText.gameObject, 2.0f));
        ChangeSkin(GameManager.Instance.stickmanSkinIndex);
        dragControl = Instantiate(GameManager.Instance.dragControl);
        dragControl.transform.position = transform.position;
        GenerateStair(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeSkin(Random.Range(0, skinTexture.Length));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GenerateStair(4);
        }

        if (GameManager.Instance.isComplete || isDead || GameManager.Instance.isStart == false) return;

        if(typeStickMan == TypeStickMan.Player) dragControl.UpdateSwipe(GetTarget());

        RayCastKey();
        RayCastCoin();
        RayCastStepItem();
    }

    private void BotAI()
    {
        StartCoroutine(C_BotAI());
    }

    private IEnumerator C_BotAI()
    {
        bool localIsLoop = true;

        Vector3 currentPos = Vector3.zero;
        Vector3 targetPos = currentPos;

        while (localIsLoop)
        {
            bool isMove = true;

            while (isMove)
            {
                float maxDistance = Random.Range(10.0f, 20.0f);
                float speed = Random.Range(1.0f, 3.0f);
                RaycastHit hit;
                Vector3 origin = (listStep.Count == 0) ? transform.position : listStep[0].transform.position;
                Ray ray = new Ray(origin, Vector3.forward);
                if (Physics.Raycast(ray, out hit, maxDistance, stepItem))
                {
                    if (hit.collider != null)
                    {
                        isMove = false;                     
                    }
                }
                else
                {
                    float localTimeDelay = Random.Range(0.1f, 1.2f);
                    yield return new WaitForSeconds(localTimeDelay);

                    isMove = true;

                    currentPos = dragControl.transform.position;
                    targetPos = currentPos;
                    targetPos.x = GetAxis_X2();
                    dragControl.transform.position = targetPos;

                    float distance2 = Vector3.Distance(currentPos, targetPos);
                    while (distance2 > 0.1f)
                    {
                        Transform target = GetTarget();
                        Vector3 tarPos = target.position;
                        tarPos.x = Mathf.Lerp(tarPos.x, dragControl.transform.position.x, Time.deltaTime * speed);
                        target.position = tarPos;

                        Vector3 a = target.position;
                        Vector3 b = a;
                        b.x = dragControl.transform.position.x;
                        distance2 = Vector3.Distance(a, b);

                        origin = (listStep.Count == 0) ? transform.position : listStep[0].transform.position;
                        ray = new Ray(origin, Vector3.forward);
                        if (Physics.Raycast(ray, out hit, maxDistance, stepItem))
                        {
                            if (hit.collider != null)
                            {
                                isMove = false;
                                break;
                            }
                        }

                        yield return null;
                    }
                }

                yield return null;
            }

            // pick lane and Move
            currentPos = dragControl.transform.position;
            targetPos = currentPos;
            targetPos.x = GetAxis_X();
            if (targetPos.x == -987) targetPos.x = currentPos.x;
            dragControl.transform.position = targetPos;

            float distance = Vector3.Distance(currentPos, targetPos);
            while (distance > 0.1f)
            {
                Transform target = GetTarget();
                Vector3 tarPos = target.position;
                tarPos.x = Mathf.Lerp(tarPos.x, dragControl.transform.position.x, Time.deltaTime * 4);
                target.position = tarPos;

                Vector3 a = target.position;
                Vector3 b = a;
                b.x = dragControl.transform.position.x;
                distance = Vector3.Distance(a, b);

                yield return null;
            }



            yield return null;
        }
    }

    private float GetAxis_X2()
    {
        return Random.Range(-3.0f, 3.0f);

        int localNumberLane = Random.Range(-2, 3);

        return (float)localNumberLane * 1.5f;
    }

    private float GetAxis_X()
    {
        int localNumberLane = Random.Range(0, GameManager.Instance.listStickMan.Count);

        Vector3 origin = (listStep.Count == 0) ? transform.position : listStep[0].transform.position;
        Ray ray = new Ray(origin, Vector3.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, 40.0f, stepItem))
        {
            if(hit.collider != null)
            {
                Vector3 origin2 = hit.collider.transform.position + Vector3.left * 100.0f;
                Ray ray2 = new Ray(origin2, Vector3.right);
                RaycastHit[] hits = Physics.RaycastAll(ray2, Mathf.Infinity, stepItem);

                foreach (RaycastHit h in hits)
                {
                    if (h.collider.GetComponent<StepItem>().ID == number)
                    {
                        return h.collider.transform.position.x;
                    }
                }
            }
        }

        return -987;
    }

    private Transform GetTarget()
    {
        if(listStep.Count == 0)
        {
            return transform;
        }
        else
        {
            return listStep[0].transform;
        }
    }

    private void RayCastKey()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 1000.0f, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, Mathf.Infinity, layerKey))
        {
            if(hit.collider!= null)
            {
                hit.collider.gameObject.SetActive(false);
                GameManager.Instance.SetKey();
            }
        }
    }

    private void RayCastCoin()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 2.0f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerCoin))
        {
            if (hit.collider != null)
            {
                GameManager.Instance.CoinEffect(hit.collider.gameObject.transform.position);
                hit.collider.gameObject.SetActive(false);
                UIManager.Instance.Coin++;
            }
        }
    }

    private void RayCastStepItem()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 100.0f, Vector3.down);
        RaycastHit hit;

        if (Physics.SphereCast(ray,0.5f, out hit, Mathf.Infinity, stepItem))
        {
            if (hit.collider != null)
            {
                Ray ray2 = new Ray(hit.collider.transform.position + Vector3.up * 100.0f, Vector3.down);
                RaycastHit[] rayCastHit2 = Physics.RaycastAll(ray2, Mathf.Infinity, stepItem);         
                foreach(RaycastHit r in rayCastHit2)
                {
                    r.collider.gameObject.SetActive(false);
                }

                StepItem localStepItem = hit.collider.gameObject.GetComponent<StepItem>();

                if(localStepItem.ID == number)
                {
                    GenerateStair(rayCastHit2.Length);
                }
                else
                {
                    for(int i = 0; i < rayCastHit2.Length; i++)
                    {
                        if(listStep.Count == 0)
                        {
                            Dead();
                            return;
                        }
                        else
                        {
                            listStep[0].DestroyStep();
                        }
                    }
                }
            }
        }
    }

    public void DestructStickman()
    {
        isDead = true;
        
        for(int i = 0; i < listStep.Count; i++)
        {
            GameManager.Instance.PlayerBreakEffect(transform, currentStepMaterial);

            listStep[i].gameObject.SetActive(false);
        }

        GameManager.Instance.PlayerBreakEffect(transform, currentStepMaterial);

        gameObject.SetActive(false);
    }

    public void ChangeSkin(int index)
    {
        if(typeStickMan == TypeStickMan.Bot)
        {
            rend.material = defaultPlayer[number];
            rendDancer.material = defaultPlayer[number];
            currentStepMaterial = defaultPlayer[number];
            return;
        }

        index++;

        if(index == 0)
        {
            rend.material = defaultPlayer[0];
            rendDancer.material = defaultPlayer[0];
            currentStepMaterial = defaultPlayer[0];
        }
        else
        {
            rend.material = skinPlayer;
            rendDancer.material = skinPlayer;
            currentStepMaterial = skinPlayer;
            skinPlayer.SetTexture("_MainTex", skinTexture[index]);
        }
    }

    private void DelayTouchDown()
    {
        if (isDelayTouchDown) return;
        StartCoroutine(C_DelayTouchDown());
    }

    private IEnumerator C_DelayTouchDown()
    {
        isDelayTouchDown = true;
        yield return new WaitForSeconds(0.3f);
        isDelayTouchDown = false;
    }

    private IEnumerator C_Hide(GameObject _obj, float _time)
    {
        _obj.SetActive(true);
        yield return new WaitForSeconds(_time);
        _obj.SetActive(false);
    }

    public void UpdateStep()
    {
        if (GameManager.Instance.isStart == false || isDead || isFinish || isRespawn) return;

        CheckDead_BigWall();
        FixedPositionY();
        TouchHold();
        MoveControl();
        Trail();
    }

    private void LateUpdate()
    {
        UpdateStepStuck();
        FixPosYStep();

        numberStepText.text = listStep.Count.ToString();
    }

    private void Trail()
    {
        ParticleSystem _particle = GameManager.Instance.starBurstTrail[number];

        if (listStep.Count > 0)
        {
            if (_particle.isPlaying == false)
                _particle.Play();

            if (GameManager.Instance.isCurveWorld)
            {
                _particle.transform.position = CurvedWorld_Controller.current.TransformPosition(listStep[0].transform.position, GameManager.Instance.bendType);
            }
            else
            {
                _particle.transform.position = listStep[0].transform.position;
            }
        }
        else
        {
            if (_particle.isStopped == false)
                _particle.Stop();

            if (GameManager.Instance.isCurveWorld)
            {
                _particle.transform.position = CurvedWorld_Controller.current.TransformPosition(transform.position, GameManager.Instance.bendType);
            }
            else
            {
                _particle.transform.position = transform.position;
            }
        }
    }

    public bool IsMoveToUpStair()
    {
        Vector3 point1 = Vector3.zero;
        Vector3 point2 = Vector3.zero;
        Ray ray = new Ray(transform.position + Vector3.up + Vector3.forward * 1.0f, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, Mathf.Infinity, layer1))
        {
            if(hit.collider != null)
            {
                point1 = hit.point;
            }
        }

        Ray ray2 = new Ray(transform.position + Vector3.up - Vector3.forward * 1.0f, Vector3.down);
        RaycastHit hit2;
        if (Physics.Raycast(ray2, out hit2, Mathf.Infinity, layer1))
        {
            if (hit2.collider != null)
            {
                point2 = hit2.point;
            }
        }

        return (point1.y == point2.y) ? false : true;
    }

    private void TouchHold()
    {
        if (isFinish || isRespawn) return;

        if (GameManager.Instance.isTutorial)
        {
            if (UIManager.Instance.timmer < 1)
            {
                return;
            }

            //if (GameManager.Instance.tutorialIndex == 0)
            //{
            //    GameManager.Instance.tutorialIndex = 1;
            //    Time.timeScale = 0.0f;
            //    UIManager.Instance.tutorialText.gameObject.SetActive(true);
            //    UIManager.Instance.tutorialText.text = "Hold To Pile";
            //}
        }


        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (isDelayTouchDown) return;

        //    if (isHold == false)
        //    {               
        //        isHold = true;
        //        isMoving = false;
        //        anim.SetTrigger("Surfing");

        //        DelayTouchDown();
        //    }
        //}

        //if (isHold)
        //{

        //}
    }

    public void GenerateStair(int _amount)
    {
        bool isMoveFirstStep = false;
        int amount = _amount;
        isMoving = false;

        if (isGenerateStair) return;

        Vector3 _stairSize = GameManager.Instance.generateMap.stairSize;

        if (listStep.Count == 0)
        {
            anim.SetTrigger("Surfing");

            Vector3 origin = transform.position + Vector3.up * 1.0f;
            Ray ray = new Ray(origin, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer2))
            {
                if (hit.collider != null)
                {
                    Step _step = StepPool.Instance.GetStep();
                    _step.transform.SetParent(GameManager.Instance.stepParent.transform);
                    _step.transform.localPosition = Vector3.zero;
                    _step.Setup(number);
                    _step.isInit = true;

                    Vector3 _position = Vector3.zero;
                    _position.x = transform.position.x;
                    _position.y = hit.point.y + _stairSize.y * 0.5f;
                    _position.z = transform.position.z;

                    if (transform.position.z > hit.point.z)
                    {
                        _position.z = hit.point.z;
                    }

                    _step.transform.position = _position;
                    _step.gameObject.SetActive(true);
                    listStep.Add(_step);
                    listAllStep.Add(_step);
                }
            }

            amount--;
            isMoveFirstStep = true;


            GameManager.Instance.Vibration();
            if (listStep.Count != 0)
                GameManager.Instance.StarBurstEffect(listStep[listStep.Count - 1].transform.position, number); transform.SetParent(listStep[listStep.Count - 1].transform);
            transform.localPosition = new Vector3(0.0f, 0.2f, 0.0f);

            myMaxStep++;
        }

        while (amount > 0)
        {
            isGenerateStair = true;

            Step _step = StepPool.Instance.GetStep();
            _step.Setup(number);

            if (listStep.Count < 2)
            {
                _step.transform.position = listStep[listStep.Count - 1].transform.position + Vector3.up * _stairSize.y;
            }

            _step.transform.SetParent(listStep[listStep.Count - 1].transform);
            _step.gameObject.SetActive(true);
            GenerateCompleteStep(_step);

            //if (listStep.Count < 2)
            //{
            //    GenerateCompleteStep(_step);
            //}
            //else
            //{
            //    _step.transform.localPosition = Vector3.zero;
            //    _step.transform.DOLocalMove(new Vector3(0.0f, _stairSize.y, 0.0f), 0.06f).SetEase(Ease.Linear).OnComplete(() => GenerateCompleteStep(_step));
            //}
            _step.transform.localPosition = new Vector3(0.0f, _stairSize.y, 0.0f);
            listStep.Add(_step);
            listAllStep.Add(_step);

            GameManager.Instance.Vibration();
            if (listStep.Count != 0)
                GameManager.Instance.StarBurstEffect(listStep[listStep.Count - 1].transform.position, number); transform.SetParent(listStep[listStep.Count - 1].transform);
            transform.localPosition = new Vector3(0.0f, 0.2f, 0.0f);

            myMaxStep++;
            amount--;
        }

        if (PlayerAnim != null) StopCoroutine(PlayerAnim);
        PlayerAnim = C_PlayerAnim();
        StartCoroutine(PlayerAnim);

        if (isMoveFirstStep) MoveStep();
    }

    private IEnumerator PlayerAnim;
    private IEnumerator C_PlayerAnim()
    {
        float t = 0.0f;

        while(t < 1.0f)
        {
            t += Time.deltaTime * 3.0f;

            Vector3 temp = transform.localPosition;
            float f = animCurve.Evaluate(t);
            float f2 = Mathf.Lerp(0.8f, 0.2f, f);
            temp.y = f2;
            transform.localPosition = temp;

            yield return null;
        }
    }

    private void GenerateCompleteStep(Step _step)
    {
        isGenerateStair = false;
        _step.isInit = true;
    }

    public void MoveStep()
    {
        StartCoroutine(C_MoveStep());
    }

    private IEnumerator C_MoveStep()
    {
        if (isFinish || listStep.Count == 0) yield break;

        while (GameManager.Instance.isStart == false)
        {
            yield return null;
        }

        if (gameObject.activeInHierarchy)
            listStep[0].MoveStep();
    }

    private bool isDelayDead;

    private IEnumerator C_DelayDead()
    {
        isDelayDead = true;
        yield return new WaitForSeconds(0.25f);
        isDelayDead = false;
    }

    public void RemoveStepStuck(Step _Step)
    {
        if(listStep.Count > 0)
        {
            listStep.Remove(_Step);

            if(listStep.Count == 0)
            {
                Dead();
            }
        }
    }

    public void UpdateStepStuck()
    {
        for(int i = 0; i < listStep.Count; i++)
        {
            bool isStuck = false;
            listStep[i].CheckStuck(out isStuck);

            if (isStuck)
            {
                i = 0;
            }
        }

        //for(int i = listStep.Count -1; i > 0; i--)
        //{
        //    bool stuck = false;
        //    listStep[i].CheckStuck(out stuck);

        //    if(stuck)
        //    {
        //        break;
        //    }
        //}
    }

    public void DoneStep(Step _step)
    {
        if (listStep.Count > 0)
        {
            listStep.Remove(_step);

            if (listStep.Count == 0)
            {
                SetRunAfterOverStep();
            }
            else
            {
                MoveStep();
            }
        }
    }

    private void FixPosYStep()
    {
        if(listStep.Count > 1)
        {
            for(int i = 1; i < listStep.Count; i++)
            {
                listStep[i].FixPositionYStep();
            }
        }
    }

    private void SetRunAfterOverStep()
    {
        if (GameManager.Instance.isComplete)
        {
            model_nonAvatar.SetActive(false);
            model_Avatar.SetActive(true);
        }

        isMoving = true;
        fakeStep.gameObject.SetActive(false);
        transform.SetParent(null);
        //    transform.position -= Vector3.up * 0.2f;
        anim.SetTrigger("Run");
        DelayTouchDown();
        GameManager.Instance.congra.Active(myMaxStep);
        myMaxStep = 0;

        CheckDead_WhenMoveStepDone();
        CheckDead_BigWall_WhenMoveStepDone();

        if (isDelayDead || gameObject.activeInHierarchy == false) return;
        StartCoroutine(C_DelayDead());
    }

    public void DestroyStep(Step _step)
    {
        DoneStep(_step);

        if (listStep.Count > 0)
        {
            listStep.Remove(_step);

            if (listStep.Count > 0)
            {
                listStep[0].MoveStep();
            }
        }

        if (listStep.Count <= 2)
        {
            for (int i = 0; i < listStep.Count; i++)
            {
                listStep[i].Hide();
                Dead();
            }
            listStep.Clear();
        }
    }

    private void MoveControl()
    {
        if (isMoving == false) return;

        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        CheckDead();
    }

    private void CheckDead()
    {
        if (isDead || isDelayDead) return;

        Ray ray = new Ray(transform.position + Vector3.forward * -0.5f + Vector3.up * 1.0f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer1))
        {
            if (hit.collider.gameObject.GetComponent<Stair>().isHide == true)
            {
                isDead = true;
                rigid.useGravity = true;
                RespawnToCheckPoint();
            }
        }
    }

    private void RespawnToCheckPoint()
    {
        if(typeStickMan == TypeStickMan.Player) GameManager.Instance.Fail();




        return;

        // main code to respawn to check point test version 2.0 on appstore

        listStep.Clear();

        for (int i = 0; i < listAllStep.Count; i++)
        {
            listAllStep[i].Hide();
        }

        listAllStep.Clear();

        StopAllCoroutines();
        StartCoroutine(C_RespawnToCheckPoint());
    }

    private IEnumerator C_RespawnToCheckPoint()
    {
        Debug.Log("Dead");

        isRespawn = true;

        yield return new WaitForSeconds(1.0f);

        listStep.Clear();

        for (int i = 0; i < listAllStep.Count; i++)
        {
            listAllStep[i].Hide();
        }

        listAllStep.Clear();

        isMoving = false;
        isHold = false;
        isFinish = false;
        isGenerateStair = false;
        isDead = false;
        isDelayTouchDown = false;

        transform.SetParent(null);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.useGravity = false;

        Vector3 _position = transform.position;

        if (checkPoint != null)
        {
            _position.z = checkPoint.position.z;
            _position.y = checkPoint.position.y;
        }
        else
        {
            _position.z = -8.0f;
            _position.y = 0.1f;
        }

        transform.position = _position;
        isDelayTouchDown = false;
        gameObject.SetActive(true);
        numberStep.transform.parent.gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        collid.enabled = true;

        StartCoroutine(C_Test());
    }

    private IEnumerator C_Test()
    {
        yield return new WaitForSeconds(2.0f);
        isRespawn = false;
        isMoving = true;
    }

    private void CheckDead_WhenMoveStepDone()
    {
        if (isDead || isDelayDead) return;

        Ray ray = new Ray(transform.position + Vector3.up * 1.0f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer1))
        {
            if (hit.collider.gameObject.GetComponent<Stair>().isHide == true)
            {
                isDead = true;
                rigid.useGravity = true;
                RespawnToCheckPoint();
            }
        }
    }

    private void CheckDead_BigWall_WhenMoveStepDone()
    {
        if (isDead || isDelayDead) return;

        Ray ray = new Ray(transform.position + Vector3.up * 0.2f + Vector3.back * 0.4f, Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, GameManager.Instance.generateMap.stairSize.z * 1.02f, layer1))
        {
            if (hit.collider != null)
            {
                Dead();
            }
        }
    }

    private void CheckDead_BigWall()
    {
        if (isDead || isDelayDead) return;

        Ray ray = new Ray(transform.position + Vector3.up * 0.2f, Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, GameManager.Instance.generateMap.stairSize.z * .5f, layer1))
        {
            if (hit.collider != null)
            {
                Dead();
                return;
            }
        }

        Ray ray2 = new Ray(transform.position + Vector3.up * 1.3f, Vector3.forward);
        RaycastHit hit2;

        if (Physics.Raycast(ray2, out hit2, GameManager.Instance.generateMap.stairSize.z * .5f, layer1))
        {
            if (hit2.collider != null)
            {
                Dead();
                return;
            }
        }

        Ray ray3 = new Ray(transform.position + Vector3.up * 0.8f, Vector3.forward);
        RaycastHit hit3;

        if (Physics.Raycast(ray3, out hit3, GameManager.Instance.generateMap.stairSize.z * .5f, layer1))
        {
            if (hit3.collider != null)
            {
                Dead();
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishTrigger"))
        {
            if (isFinish == false)
            {
                StopAllCoroutines();
                StartCoroutine(C_Finish());
            }
        }
        else if (other.CompareTag("Rock"))
        {
            Dead();
        }
        else if (other.CompareTag("CheckPoint"))
        {
            checkPoint = other.gameObject.transform;

            CheckPoint _checkPoint = other.transform.parent.GetComponent<CheckPoint>();
            _checkPoint.ActiveCheckPoint();
        }
    }

    public void KeepParent1()
    {
        if(listStep.Count > 1)
        {
            for(int i = 1; i < listStep.Count; i++)
            {
                listStep[i].KeepPosition1();
            }
        }
    }

    public void KeepParent2()
    {
        if (listStep.Count > 1)
        {
            for (int i = 1; i < listStep.Count; i++)
            {
                listStep[i].KeepPosition2();
            }
        }
    }

    public void Dead()
    {
        StopAllCoroutines();
        isDead = true;
        GameManager.Instance.ShakeCamera();
        isFinish = true;
        GameManager.Instance.PlayerBreakEffect(transform, stickManMaterBreakMaterial);
        numberStep.transform.parent.gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        collid.enabled = false;
        RespawnToCheckPoint();
    }

    private void FixedPositionY()
    {
        if (isHold) return;

        Vector3 origin = transform.position + Vector3.up * 1.0f;
        Ray ray = new Ray(origin, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer2))
        {
            if (hit.collider.CompareTag("Step"))
            {
                if (hit.collider.gameObject.GetComponent<Step>().number != number) return;
            }

            if (hit.collider != null)
            {
                Vector3 a = transform.position;
                Vector3 b = hit.point;
                float c = Vector3.Distance(a, b);

                if (c > .1f)
                {
                    transform.Translate(Vector3.down * Time.deltaTime * 12.0f);
                    if(transform.position.y < hit.point.y)
                    {
                        Vector3 p = transform.position;
                        p.y = hit.point.y;
                        transform.position = p;
                    }
                }
            }
        }
    }

    public void StartRun()
    {
        if(typeStickMan == TypeStickMan.Bot)
        {
            BotAI();
        }
      //  isMoving = true;
     //   anim.SetTrigger("Run");
    //    anim.speed = 1.35f;
    }

    private bool isLoseAnim;

    public void LoseAnim()
    {
        if (isLoseAnim) return;

        isLoseAnim = true;
        StartCoroutine(C_LoseAnim());
    }

    private IEnumerator C_LoseAnim()
    {
        yield return new WaitForSeconds(0.4f);

        if (isFinish) yield break;
    
        transform.DORotate(Vector3.up * 180, 1.0f);
        anim.SetTrigger("lose");
    }

    private IEnumerator C_Finish()
    {
        if (GameManager.Instance.isComplete)
        {
            anim.SetTrigger("lose");
            yield break;
        }

        GameManager.Instance.Complete(number);
        anim.speed = 1.0f;
        transform.localEulerAngles = Vector3.zero;
        isFinish = true;
        transform.DORotate(Vector3.up * 180, 1.0f);
        anim.SetTrigger("dance" + Random.Range(1, 9));
        //     transform.GetChild(0).DOLocalRotate(Vector3.up * 180.0f, 0.25f);
      //  model_nonAvatar.SetActive(false);
        //model_Avatar.SetActive(true);
    }

    public void HideUI()
    {
        fakeStep.gameObject.SetActive(false);
        numberStep.transform.parent.gameObject.SetActive(false);
    }

    public Vector3 TrailPos()
    {
        if (listStep.Count != 0) return listStep[0].transform.position - Vector3.forward * GameManager.Instance.generateMap.stairSize.z * 0.5f; ;

        return transform.position;
    }

    public bool IsMovingSpace()
    {
        bool _result = false;

        Ray ray = new Ray(transform.position + Vector3.up, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, Mathf.Infinity, layerStair))
        {
            if(hit.collider != null)
            {
                if (hit.collider.gameObject.GetComponent<Stair>().isHide) return true;
            }
        }

        return _result;
    }

    public void StopMove()
    {
        StopAllCoroutines();
        if (listStep.Count == 0) return;
        listStep[0].moveSpeed = 0;
    }
}