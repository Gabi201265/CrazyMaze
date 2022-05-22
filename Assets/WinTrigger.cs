using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="player1" || collision.gameObject.tag == "player2")
        {
           GameObject gm =GameObject.FindGameObjectWithTag("GameManager");
           gm.GetComponent<GameManager>().GameState = GameManager.State.Win;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
