using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCharactor : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public Transform enemy2;

    public Vector3 playerPosition_916;
    public Vector3 playerPosition_919;
    public Vector3 playerPosition_34;

    public void Awake()
    {
        float ratio = Camera.main.aspect;

        if (ratio >= 0.74) // 3:4
        {
            player.position = playerPosition_34;
        }
        else if (ratio >= 0.56) // 9:16
        {
            player.position = playerPosition_916;
        }
        else if (ratio >= 0.45) // 9:19
        {
            player.position = playerPosition_919;
        }

        Vector3 temp = player.position;
        temp.x *= -1;
        enemy.position = temp;

        //Vector3 temp2 = player.position;
        //temp2.y = enemy2.position.y;
        //player.position = temp2;

    }
}