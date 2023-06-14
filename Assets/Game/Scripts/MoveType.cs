using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveType : MonoBehaviour
{
    public Move moveType;

    public enum Move
    {
        Idle,
        MoveHorizontal,
        MoveUpDown,
        MoveDown,
        MoveUp,
        MoveDown2,
        MoveUp2,
        MoveUp5
    }

    private void Start()
    {
        switch (moveType)
        {
            case Move.Idle:
                break;
            case Move.MoveHorizontal:
                StartCoroutine(C_MoveHorizontal());
                break;
            case Move.MoveUpDown:
                StartCoroutine(C_MoveUpDown());
                break;
            case Move.MoveDown:
                StartCoroutine(C_MoveDown());
                break;
            case Move.MoveUp:
                StartCoroutine(C_MoveUp());
                break;
            case Move.MoveDown2:
             //   StartCoroutine(C_MoveUp());
                break;
            case Move.MoveUp2:
                StartCoroutine(C_MoveUp_2());
                break;
            case Move.MoveUp5:
                StartCoroutine(C_MoveUp_5());
                break;
        }
    }

    private IEnumerator C_MoveHorizontal()  
    {
        int h_direction = (Random.value > 0.5) ? -1 : 1;
        float moveSpeed = 2.0f;

        bool isLoop = true;

        while (isLoop)
        {
            transform.Translate(Vector3.right * h_direction * moveSpeed * Time.deltaTime,Space.World);

            if(transform.position.x > 5.0f)
            {
                Vector3 pos = transform.position;
                pos.x = 5.0f;
                transform.position = pos;
                h_direction *= -1;
            }


            if (transform.position.x < -5.0f)
            {
                Vector3 pos = transform.position;
                pos.x = -5.0f;
                transform.position = pos;
                h_direction *= -1;
            }

            yield return null;
        }
    }

    private IEnumerator C_MoveUpDown()
    {
        Vector3 pos1 = transform.position;
        Vector3 pos2 = pos1;
        pos2.y += 2.0f;

        transform.position = pos2;

        bool isLoop = true;

        while (isLoop)
        {
            transform.DOMove(pos1, 2.0f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);

            yield return new WaitForSeconds(7.0f);
        }
    }

    private Transform Top()
    {
        return transform;
    }

    private Transform Bot()
    {
        return transform.GetChild(transform.childCount - 1);
    }

    private IEnumerator C_MoveDown()
    {
        bool isLoop = true;

        while (isLoop)
        {
            if (GameManager.Instance != null && GameManager.Instance.listStickMan[0] != null)
            {
                float distance = transform.position.z - GameManager.Instance.listStickMan[0].transform.position.z;

                if (GameManager.Instance.isStart && distance < 30.0f)
                {
                    transform.Translate(Vector3.down * Time.deltaTime * 3.2f);

                    Ray ray = new Ray(Bot().position, Vector3.down);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, GameManager.Instance.generateMap.stairSize.y * 0.5f))
                    {
                        isLoop = false;
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator C_MoveUp()
    {
        bool isLoop = true;

        while (isLoop)
        {
            if (GameManager.Instance != null && GameManager.Instance.listStickMan[0] != null)
            {
                float distance = transform.position.z - GameManager.Instance.listStickMan[0].transform.position.z;

                if (GameManager.Instance.isStart && distance < 30.0f)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * 3.2f);

                    Ray ray = new Ray(Top().position, Vector3.down);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, GameManager.Instance.generateMap.stairSize.y * 0.5f))
                    {
                        isLoop = false;
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator C_MoveUp_2()
    {
        bool isLoop = true;
        float targetY = transform.position.y + 2.0f;

        while (isLoop)
        {
            if (GameManager.Instance != null && GameManager.Instance.listStickMan[0] != null)
            {
                float distance = transform.position.z - GameManager.Instance.listStickMan[0].transform.position.z;

                if (GameManager.Instance.isStart && distance < 30.0f)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * 3.2f);

                    if (transform.position.y > targetY) isLoop = false;
                }
            }
      
            yield return null;
        }
    }

    private IEnumerator C_MoveUp_5()
    {
        bool isLoop = true;
        float targetY = transform.position.y + 5.0f;

        while (isLoop)
        {
            if (GameManager.Instance != null && GameManager.Instance.listStickMan[0] != null)
            {
                float distance = transform.position.z - GameManager.Instance.listStickMan[0].transform.position.z;

                if (GameManager.Instance.isStart && distance < 30.0f)
                {
                    transform.Translate(Vector3.up * Time.deltaTime * 6.4f);

                    if (transform.position.y > targetY) isLoop = false;
                }
            }

            yield return null;
        }
    }
}
