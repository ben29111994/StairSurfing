using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerP", menuName = "PlayerPData", order = 2)]
public class PlayerTableObject : ScriptableObject
{
    public string nameIngredient
    {
        get
        {
            string[] splitName = iconIngredient.name.Split('_');
            string name = splitName[1].ToUpper();
            return name;
        }
    }

    public Sprite iconIngredient;
    public int priceUpgrade;
}
