using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepPool : MonoBehaviour
{
    public static StepPool Instance;

    public Step stepPrefab;
    public List<Step> listStep = new List<Step>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 200; i++)
        {
            Step _step = Instantiate(stepPrefab,transform);
            _step.gameObject.SetActive(false);
            listStep.Add(_step);
        }
    }

    public Step GetStep()
    {
        for(int i = 0; i < listStep.Count; i++)
        {
            if (listStep[i].gameObject.activeSelf == false) return listStep[i];
        }

        Step _step = Instantiate(stepPrefab, transform);
        _step.gameObject.SetActive(false);
        listStep.Add(_step);
        return _step;
    }
}
