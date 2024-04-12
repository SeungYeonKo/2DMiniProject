using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowPoint : MonoBehaviour
{
    public GameObject Player;

    void Update()
    {
        this.transform.position = new Vector3(Player.transform.position.x+2.9f, -2, Player.transform.position.z);   
    }
}
