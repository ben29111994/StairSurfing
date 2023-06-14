using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinUI : MonoBehaviour
{
    public Transform a;
    public Transform b;

    private List<Transform> listCoin = new List<Transform>();

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            listCoin.Add(transform.GetChild(i));
        }
    }

    public void CoinAnimation(int coinEarn)
    {
        StartCoroutine(C_CoinAnimation(coinEarn));
    }

    private IEnumerator C_CoinAnimation(int coinEarn)
    {
        for(int i = 0; i < listCoin.Count;i++)
        {
            Transform _tra = listCoin[i];
            _tra.gameObject.SetActive(true);
            _tra.transform.position = a.position + Vector3.right * Random.Range(-5.0f,5.0f);
            _tra.DOMove(b.position, 0.5f).OnComplete(() => _tra.gameObject.SetActive(false));

            int n = coinEarn / listCoin.Count;
            int m = coinEarn % listCoin.Count;

            if(i < listCoin.Count - 1)
            {
                StartCoroutine(C_EarnCoin(n));
            }
            else
            {
                StartCoroutine(C_EarnCoin(n + m));
            }

            yield return new WaitForSeconds(0.04f);
        }
    }

    private IEnumerator C_EarnCoin(int c)
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.Coin += c;
    }
}
