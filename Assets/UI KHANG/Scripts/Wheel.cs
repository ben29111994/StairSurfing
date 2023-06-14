using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public Transform wheelSlotParent;
    public WheelSlot wheelSlotPrefab;

    public List<WheelSlot> listWheelSlot = new List<WheelSlot>();

    private void Start()
    {
        GenerateWheelSlot();
    }

    private void OnEnable()
    {
        // update ingredient
    }

    public void GenerateWheelSlot()
    {
        for(int i = 0; i < 8; i++)
        {
            WheelSlot _wheelSlot = Instantiate(wheelSlotPrefab, wheelSlotParent);
            _wheelSlot.transform.eulerAngles = new Vector3(0.0f, 0.0f, i * 45);
            listWheelSlot.Add(_wheelSlot);
        }
    }
}
