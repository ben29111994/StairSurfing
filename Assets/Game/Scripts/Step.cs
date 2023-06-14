using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Step : MonoBehaviour
{
    public Vector3 startPosition;
   
    public float ratio;
    public LayerMask layerRock;
    public LayerMask layer2;
    public LayerMask layer;
    public float moveSpeed;
    public bool isMove;
    public bool isDone;
    public bool isMoveToTarget;
    public int number;
    public bool isStuck;
    public bool isInit;
    public bool isAutoHide;
    public bool isTest;
    public bool isFallDown;

    [Header("References")]
    public Rigidbody rigid;
    public Renderer rend;
    public BoxCollider boxCol;

    public MeshFilter meshFilter;
    public Material[] stepMaterials;
    public Vector3 targetStairPosition;
    private float targetY;

    
    public Material[] skinMaterials;
    public Mesh[] skinMeshes;

    public void ChangeSkin(int index)
    {
        index++;
        rend.material = skinMaterials[index];
        Mesh mesh = skinMeshes[index];
        meshFilter.sharedMesh = mesh;
        if (GameManager.Instance.stepCVBB.sharedMesh != mesh)
        {
            GameManager.Instance.stepCVBB.gameObject.SetActive(true);
            GameManager.Instance.stepCVBB.sharedMesh = mesh;
        }
    }

    public void Setup(int _number)
    {
        isMoveToTarget = false;
        isDone = false;
        isStuck = false;
        isFallDown = false;
        isInit = false;
        isMove = false;
        isAutoHide = false;
        targetStairPosition = Vector3.zero;
        number = _number;
        rend.material = stepMaterials[number];

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        rigid.useGravity = false;

        if(number == 0)
        {
            ChangeSkin(GameManager.Instance.stepSkinIndex);
        }
        else
        {
            meshFilter.mesh = skinMeshes[0];
        }
    }

    private IEnumerator C_AutoHide()
    {
        yield break;
        isAutoHide = true;
        yield return new WaitForSeconds(5.0f);
        Hide();
    }

    public void MoveStep()
    {
        if (gameObject.activeSelf == false) return;

        StartCoroutine(C_MoveStep());
    }

    private IEnumerator C_MoveStep()
    {
        while (isInit == false)
        {
            yield return null;
        }

        isMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rock"))
        {
            if (isDone == false)
            {
                GameManager.Instance.ShakeCamera();
                DestroyStep();
            }

            Hide();
        }
    }
    
    public void Hide()
    {
        StopAllCoroutines();

        isMove = false;

        if (gameObject.activeInHierarchy == false) return;

        if(transform.childCount !=0)
        transform.GetChild(0).SetParent(GameManager.Instance.stepParent.transform);
        GameManager.Instance.CubeBreakEffect(transform, rend.material);

        gameObject.SetActive(false);
    }

    private void HideEffect()
    {
        return;

        StopAllCoroutines();

        if(gameObject.activeSelf)
        StartCoroutine(C_HideEffect());
    }

    private IEnumerator C_HideEffect()
    {
        yield return new WaitForSeconds(1.0f);
        GameManager.Instance.CubeBreakEffect(transform, rend.material);
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (isDone && isFallDown == false)
        {
           // if (isMoveToTarget) return;

            FallDownWhenDone();
        }

        Ray ray3 = new Ray(transform.position, Vector3.forward);
        RaycastHit hit3;

        if (Physics.Raycast(ray3, out hit3, GameManager.Instance.generateMap.stairSize.z, layer2))
        {
           // Debug.Log("ASD");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
          //  ChangeSkin(Random.Range(0, skinMeshes.Length));
        }

        if (isMove == false && isDone)
        {
       //     HideUpdate();
            return;    
        }


        if (transform.position.z > GameManager.Instance.finishPositionZ)
        {
            if (GameManager.Instance.isComplete)
            {
                GameManager.Instance.listStickMan[number].LoseAnim();
            }

            return;
        }
        

        CheckGround();
    }

    private void LateUpdate()
    {
      // KeepPosition();
    }

    private Vector3 TargetPosition()
    {
        Vector3 targetPosition = Vector3.one * 1000;

        if (number == 0)
        {
            targetPosition.x = 1.0f;
        }
        else if (number == 1)
        {
            targetPosition.x = -1.0f;
        }
        else if (number == 2)
        {
            targetPosition.x = 1.5f;
        }

        Vector3 _stairSize = GameManager.Instance.generateMap.stairSize;
        Ray ray = new Ray(transform.position, Vector3.forward);
        RaycastHit hit;
        float distance = GameManager.Instance.generateMap.stairSize.z;

        if (Physics.Raycast(ray, out hit, distance, layer))
        {
            if (hit.collider != null)
            {
                targetPosition.z = hit.collider.gameObject.transform.position.z - (_stairSize.z);
                float _y = hit.collider.gameObject.transform.position.y - (_stairSize.y);
                targetPosition.y = (_y > transform.position.y) ? transform.position.y : _y;

                return targetPosition;
            }
        }

        Vector3 _origin = transform.position + Vector3.forward * _stairSize.z;
        Ray _ray = new Ray(_origin, Vector3.down);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, layer))
        {
            if (_hit.collider != null)
            {
                targetPosition.z = _hit.collider.gameObject.transform.position.z;
                targetPosition.y = _hit.point.y + _stairSize.y * 0.5f;
            }
        }

        return targetPosition;
    }

    private void MoveFoward()
    {
        if (isMove == false) return;

        ratio = 1.0f + (float)m_StepStickman().listStep.Count * 0.01f;
  
        //if (number == 0)
        //{
        //    transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * ratio);
        //}
        //else
        //{
        //    transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * ratio * 0.9f);
        //}

        transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed * ratio);
    }

    private Stickman m_StepStickman()
    {
        for(int i = 0; i < GameManager.Instance.listStickMan.Count; i++)
        {
            if(number == GameManager.Instance.listStickMan[i].number)
            {
                return GameManager.Instance.listStickMan[i];
            }
        }

        return null;
    }

    private void MoveToTarget()
    {
        if (isMoveToTarget) return;

        if (targetStairPosition == Vector3.zero)
        {
            targetStairPosition = TargetPosition();
        }
        else
        {
            return;
        }

        StartCoroutine(C_MoveToTarget());

        isMoveToTarget = true;
    }

    private IEnumerator C_MoveToTarget()
    {
        Vector3 fPos = transform.position;
        Vector3 tPos = targetStairPosition;

        float t = 0;
        while(t < 1)
        {
            t += Time.deltaTime * 14.0f;

            Vector3 temp = transform.position;
            temp.z = Mathf.Lerp(fPos.z, tPos.z, t);
            transform.position = temp;

            if(tPos.y < fPos.y)
            {
                GameManager.Instance.listStickMan[number].KeepParent1();

                temp.y = Mathf.Lerp(fPos.y, tPos.y, t);
                transform.position = temp;

                GameManager.Instance.listStickMan[number].KeepParent2();
            }

            if (t < 1)
            {
                yield return null;
            }
        }

        DoneMoveStep();
    }

    public void DestroyStep()
    {
        GameManager.Instance.listStickMan[number].DestroyStep(this);
        Hide();
    }

    private void CheckStopMoveStairGroup()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, 1.0f, layer))
        {
            if(hit.collider != null)
            {
                StairGroup _stairGroup = hit.collider.transform.parent.GetComponent<StairGroup>();

                if(_stairGroup != null)
                {
                    _stairGroup.StopMove();
                }
            }
        }
    }

    private void DoneMoveStep()
    {
        GameManager.Instance.Vibration();
        CheckStopMoveStairGroup();

        isMove = false;

        GameManager.Instance.listStickMan[number].DoneStep(this);

        Transform myParent = transform.parent;
        Transform myChild = (transform.childCount != 0) ? transform.GetChild(0) : null;
        transform.SetParent(GameManager.Instance.stepParent.transform);
        if (myChild != null)
        {
            myChild.SetParent(myParent);
        }

        CheckGrounded();
        if(gameObject.activeSelf) StartCoroutine(C_AutoHide());

        HideEffect();
    }

    public void RemoveStepStuck()
    {
        isDone = true;
        isMove = false;

        GameManager.Instance.listStickMan[number].RemoveStepStuck(this);
        Transform myParent = transform.parent;
        Transform myChild = (transform.childCount != 0) ? transform.GetChild(0) : null;
        transform.SetParent(GameManager.Instance.stepParent.transform);
        if (myChild != null)
        {
            myChild.SetParent(myParent);
        }

        CheckGrounded();
        if (gameObject.activeSelf) StartCoroutine(C_AutoHide());

        HideEffect();
    }

    public void CheckGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit, Mathf.Infinity, layer))
        {
            if(hit.collider != null)
            {
                Stair _stair = hit.collider.gameObject.GetComponent<Stair>();

                if (_stair.isHide)
                {
                    rigid.AddForce(Vector3.down * 150.0f);
                    rigid.useGravity = true;
                    isFallDown = true;
                }
            }
        }
    }

    public void HideUpdate()
    {
        if (isDone == false || GameManager.Instance.isComplete) return;

        float myZ = transform.position.z;   

        float playerZ = MinPlayerPosZ();


        if (playerZ - myZ > 20.0f)
        {
            transform.SetParent(StepPool.Instance.transform);
            Hide();
        }
    }

    private float MinPlayerPosZ()
    {
        List<Stickman> _lsm = GameManager.Instance.listStickMan;

        float _mz = 9999.0f;

        for(int i = 0; i < _lsm.Count; i++)
        {
            if(_lsm[i].transform.position.z < _mz) _mz = _lsm[i].transform.position.z;
        }

        return _mz;
    }

    public bool IsFallDown()
    {
        return (transform.position.y == targetY) ? false : true;
    }

    public void CheckGround()
    {
        if (isDone == false)
        {
            if (GameManager.Instance.listStickMan[number].listStep.Count == 0) return;

            if (GameManager.Instance.listStickMan[number].listStep[0] != this)
            {
                return;
            }

            if (GameManager.Instance.listStickMan[number].isDead) return;          
        }

        if (isMoveToTarget || isStuck) return;

        Ray ray2 = new Ray(transform.position + Vector3.forward * GameManager.Instance.generateMap.stairSize.z * 0.51f, Vector3.down);
        RaycastHit hit2;

        if (Physics.Raycast(ray2, out hit2, GameManager.Instance.generateMap.stairSize.y * 1.501f, layer2))
        {
            if (hit2.collider.CompareTag("Step"))
            {
                // MoveFoward();
            }
            else if (hit2.collider.CompareTag("Stair"))
            {
                Stair _stair = hit2.collider.gameObject.GetComponent<Stair>();

                if (_stair.isHide)
                {
                    MoveToTarget();
                    return;
                }
            }
        }

        MoveFoward();
        FallDown();

        Ray ray3 = new Ray(transform.position, Vector3.forward);
        RaycastHit hit3;

        if (Physics.Raycast(ray3, out hit3, GameManager.Instance.generateMap.stairSize.z * 0.5f, layer2))
        {
            Vector3 temp = transform.position;
            temp.z = hit3.point.z - GameManager.Instance.generateMap.stairSize.z * 0.5f;
            transform.position = temp;

            isTest = true;
            isMoveToTarget = true;
            DoneMoveStep();
            return;
        }
    }
   
    public void CheckStuck(out bool _isStuck)
    {
        Ray ray3 = new Ray(transform.position, Vector3.forward);
        RaycastHit hit3;

        if (Physics.Raycast(ray3, out hit3, GameManager.Instance.generateMap.stairSize.z * 0.501f, layer2))
        {
            isStuck = true;
            isMoveToTarget = true;
            RemoveStepStuck();

            Vector3 temp = transform.position;
            temp.z = hit3.point.z - GameManager.Instance.generateMap.stairSize.z * 0.5f;
            transform.position = temp;
        }

        _isStuck = isStuck;
    }

    public void FixPositionYStep()
    {
        if (isInit == false) return;

        Vector3 sizeStep = GameManager.Instance.generateMap.stairSize;

        Ray ray = new Ray(transform.position + Vector3.forward * sizeStep.z * 0.51f, Vector3.down);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, sizeStep.y * 0.5f))
        {
            if(hit.collider != null) return;
        }
        
        ray = new Ray(transform.position - Vector3.forward * sizeStep.z * 0.51f, Vector3.down);
        if (Physics.Raycast(ray, out hit, sizeStep.y * 0.5f))
        {
            if (hit.collider != null) return;
        }

        GameManager.Instance.listStickMan[number].KeepParent1();

        Vector3 myPos = transform.localPosition;
        float targetY = GameManager.Instance.generateMap.stairSize.y;

        if(myPos.y > targetY)
        {
            myPos.y -= Time.deltaTime * 10.0f;
        }

        if (myPos.y < targetY) myPos.y = targetY;

        transform.localPosition = myPos;

        GameManager.Instance.listStickMan[number].KeepParent2();
    }
    public bool a;
    private void FallDown()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position - Vector3.forward * GameManager.Instance.generateMap.stairSize.z * 0.51f, Vector3.down);
        if (Physics.Raycast(ray, out hit, GameManager.Instance.generateMap.stairSize.y * 0.501f, layer2))
        {          
            if (hit.collider != null)
            {
                return;
            }
        }

        RaycastHit hit2;
        ray = new Ray(transform.position + Vector3.forward * GameManager.Instance.generateMap.stairSize.z * 0.51f, Vector3.down);
        if (Physics.Raycast(ray, out hit2, GameManager.Instance.generateMap.stairSize.y * 0.501f, layer2))
        {
            if (hit2.collider != null)
            {
                return;
            }
        }

        RaycastHit hit3;
        ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit3, GameManager.Instance.generateMap.stairSize.y * 0.501f, layer2))
        {
            if (hit3.collider != null)
            {
                return;
            }
        }

        RaycastHit hit5;
        ray = new Ray(transform.position + Vector3.up, Vector3.down);
        if (Physics.SphereCast(ray,1.0f, out hit5, GameManager.Instance.generateMap.stairSize.y * 2.5f, layerRock))
        {
            if (hit5.collider != null)
            {
                return;
            }
        }

        RaycastHit hit4;
        ray = new Ray(transform.position, Vector3.down);
        if(Physics.Raycast(ray, out hit4, Mathf.Infinity, layer))
        {
            targetY = hit4.point.y + GameManager.Instance.generateMap.stairSize.y * 0.5f;

            GameManager.Instance.listStickMan[number].KeepParent1();

            transform.Translate(Vector3.down * Time.deltaTime * moveSpeed * 1.85f);

            GameManager.Instance.listStickMan[number].KeepParent2();


            if (transform.position.y < targetY)
            {
                Vector3 temp = transform.position;
                temp.y = targetY;
                transform.position = temp;
            }
        }

 
    }

    private void FallDownWhenDone()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, GameManager.Instance.generateMap.stairSize.y * 0.501f, layer2))
        {
            if (hit.collider == null)
            {
            
            }
        }
        else
        {
            targetY = hit.point.y + GameManager.Instance.generateMap.stairSize.y * 0.5f;
            transform.Translate(Vector3.down * Time.deltaTime * moveSpeed * 1.15f);

            if (transform.position.y < targetY)
            {
                Vector3 temp = transform.position;
                temp.y = targetY;
                transform.position = temp;
            }
        }
    }

    private Transform keepParent;

    public void KeepPosition1()
    {
        if (GameManager.Instance.listStickMan[number].listStep.Count != 0)
        {
            if (GameManager.Instance.listStickMan[number].listStep[0] == this)
            {
                return;
            }
        }

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit,GameManager.Instance.generateMap.stairSize.y * 0.501f, layer))
        {
            if(hit.collider != null)
            {
                keepParent = transform.parent;
                transform.SetParent(null);
            }
        }
    }

    public void KeepPosition2()
    {
        if (keepParent == null) return;
        transform.SetParent(keepParent);
        keepParent = null;
    }
}