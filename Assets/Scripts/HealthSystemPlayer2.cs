using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystemPlayer2 : MonoBehaviour
{

    [SerializeField] Sprite fullHeart, emptyHeart;
    [SerializeField] Image life1, life2, life3;

    private GameHub hub;
    private GameObject SecondPlayer;
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameHub = GameObject.FindGameObjectWithTag("GameHub");
        hub = gameHub.GetComponent<GameHub>();

        SecondPlayer = GameObject.FindGameObjectWithTag("player2");
    }
    // Update is called once per frame
    void Update()
    {
        //print("Vie j1 : " + FirstPlayer.GetComponent<Player>().GetLives());
        //print("Vie j2 : " + SecondPlayer.GetComponent<Player>().GetLives());
        //print("Player Vie" + Player.instance.GetLives());
        switch (SecondPlayer.GetComponent<Player>().GetLives())
        {
            case 3:
                life3.sprite = fullHeart;
                life2.sprite = fullHeart;
                life1.sprite = fullHeart;
                break;
            case 2:
                life3.sprite = emptyHeart;
                life2.sprite = fullHeart;
                life1.sprite = fullHeart;
                break;
            case 1:
                life3.sprite = emptyHeart;
                life2.sprite = emptyHeart;
                life1.sprite = fullHeart;
                break;
            case 0:
                life3.sprite = emptyHeart;
                life2.sprite = emptyHeart;
                life1.sprite = emptyHeart;
                break;

        }

    }
}
