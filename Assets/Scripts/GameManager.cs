using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    public enum State
    {
        Menu,
        Play,
        Win,
        GameOver
    };
    public int Level;
    public State GameState;
    public GameObject prefabGround;
    public GameObject Ground;
    public GameObject prefabGameHub;
    public GameObject GameHub;
    public GameObject Menu;
    public GameObject PrefabMapCinematique;
    private GameObject MapCinematique;
    public GameObject cam1;
    public GameObject cam2;
    public GameObject camCinematique;
    public Text level;
    public GameObject levelPanel;
    public GameObject GameOverPanel;
    public GameObject VictoryPanel;

    public AudioClip GameOver;
    public AudioSource JouerSon;
    public AudioClip Level1;
    public AudioClip Level2;
    public AudioClip Level3;
    public AudioClip Victory;
    // Start is called before the first frame update
    void Start()
    {

        Level = 1;
        GameState = State.Menu;
        Menu = GameObject.FindGameObjectWithTag("Menu");
        Menu.SetActive(false);
        MapCinematique = Instantiate(PrefabMapCinematique, new Vector3(0, 0, 0), Quaternion.identity);
        levelPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        VictoryPanel.SetActive(false);

        JouerSon = GetComponent<AudioSource>();
    }
    public void StartNewGame()
    {
        GameState = State.Play;
        camCinematique.SetActive(false);
        MapCinematique.SetActive(false);
        if (Level > 1)
        {
            DestroyMap();
        }
         
        if (Ground != null)
        {
            Destroy(Ground);
        }
        Ground = Instantiate(prefabGround, new Vector3(0, 0, 0),Quaternion.identity);
        Ground.transform.Rotate(90, 0, 0, 0);
        GameObject Gh = GameObject.FindGameObjectWithTag("GameHub");
        if (Gh!= null)
        {
            Destroy(Gh);
        }
        GameHub=Instantiate(prefabGameHub, new Vector3(0, 0, 0), Quaternion.identity);
        GameHub.GetComponent<GameHub>().maze = Ground.GetComponent<MazeGen>();
        level.text = "Level " + Level;

        if (Level == 1)
        {
            JouerSon.PlayOneShot(Level1, 1);
        }
        else if (Level == 2)
        {
            JouerSon.PlayOneShot(Level2, 1);
        }
        else if (Level == 3)
        {
            JouerSon.PlayOneShot(Level3, 1);
        }

        levelPanel.SetActive(true);
    }

    // Update is called once per frame
    private void DestroyMap()
    {
        GameObject[] walls=  GameObject.FindGameObjectsWithTag("Mur");
        GameObject[] Affichage = GameObject.FindGameObjectsWithTag("AffichageVie");
        GameObject cameraPlayer2 = GameObject.Find("cameraPlayer2(Clone)");
        GameObject cameraPlayer1 = GameObject.Find("cameraPlayer1(Clone)");
        Destroy(cameraPlayer2);
        Destroy(cameraPlayer1);

        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }
        foreach (GameObject aff in Affichage)
        {
            Destroy(aff);
        }
        Destroy( GameHub.GetComponent<GameHub>().getPlayer1());
        Destroy(GameHub.GetComponent<GameHub>().getPlayer2());
        Destroy(GameHub.GetComponent<GameHub>().getMinos());

    }
    void Update()
    {
        
        if (GameState == State.Win && Level == 3)
        {
            //Quand on a passé les 3 levels on print le victory :
            levelPanel.SetActive(false);
            JouerSon.PlayOneShot(Victory, 1);
            Level = 1;
            VictoryPanel.SetActive(true);
            GameState = State.Menu;
        }
        else if (GameState == State.Win)
        {
            Level++;
            StartNewGame();
        }
       
        else if (GameState == State.GameOver)
        {
            levelPanel.SetActive(false);
            JouerSon.PlayOneShot(GameOver, 1);
            GameOverPanel.SetActive(true);
            GameState = State.Menu;
            //StartNewGame();
        }
        //print(GameState);
    }
}
