using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollowPoint : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(Player.transform.position.x+2.9f, -2, Player.transform.position.z);   
    }
}
