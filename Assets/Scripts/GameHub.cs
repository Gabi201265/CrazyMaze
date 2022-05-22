using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameHub : MonoBehaviour
{
    public MazeGen maze;
    private int[,] mazeMatrice;

    public GameObject prefabJoueur1;
    private GameObject Joueur1;

    public GameObject prefabsChest;
    private GameObject chest;

    public GameObject prefabJoueur2;
    private GameObject Joueur2;

    public GameObject prefabMinos;
    private GameObject minos;

    public GameObject prefabCamera1;
    public GameObject Camera1;

    public GameObject prefabCamera2;
    public GameObject Camera2;
    // Start is called before the first frame update
    void Start()
    {

        this.mazeMatrice = maze.GetMatrice();
        //maze.AffichageMatrice(mazeMatrice);
        spawnCaracters();

        prefabCamera1 = GameObject.Find("cameraPlayer1");
        Camera1 = Instantiate(prefabCamera1, new Vector3(0, 0, 0), Quaternion.identity);
        Camera1.AddComponent<CamFollowSmooth>();
        Camera1.GetComponent<CamFollowSmooth>().setTarget(Joueur1.transform);
        Camera1.AddComponent<AudioListener>();

        prefabCamera2 = GameObject.FindGameObjectWithTag("camplayer2");
        Camera2 = Instantiate(prefabCamera2, new Vector3(0, 0, 0), Quaternion.identity);
        Camera2.AddComponent<CamFollowSmooth>();
        Camera2.GetComponent<CamFollowSmooth>().setTarget(Joueur2.transform);
        //Camera2.AddComponent<AudioListener>();
    }
    //Accesseur du player1
    public GameObject getPlayer1()
    {
        return Joueur1;
    }
    public GameObject getMinos()
    {
        return minos;
    }

    //Accesseur du player1
    public GameObject getPlayer2()
    {
        return Joueur2;
    }

    private void spawnCaracters()
    {
        Vector3 spawnPlayer1 = getAleaSpawn();
        //Vector3 spawnPlayer1 = new Vector3(1, 1, 0);
        Vector3 spawnPlayer2 = getAleaSpawn();
        Vector3 spawnMinos = getAleaSpawn();

        Joueur1 = Instantiate(prefabJoueur1, spawnPlayer1, Quaternion.identity);
        Joueur1.GetComponent<Player>().SetPosition(Joueur1.transform.position);


        Joueur2 = Instantiate(prefabJoueur2, spawnPlayer2, Quaternion.identity);
        Joueur2.GetComponent<Player>().SetPosition(Joueur2.transform.position);


        minos = Instantiate(prefabMinos, spawnMinos, Quaternion.identity);
        chest = Instantiate(prefabsChest, new Vector3(0,0,0), Quaternion.identity);
    }

    public Vector3 getAleaSpawn()
    {
        int i = Random.Range(-(maze.GetTailleVisiteX() - 1) / 2, (maze.GetTailleVisiteX() - 1) / 2);
        int y = Random.Range(-(maze.GetTailleVisiteX() - 1) / 2, (maze.GetTailleVisiteX() - 1) / 2);
        Vector3 vec = new Vector3(i * maze.GetTailleCellule(), 1, y * maze.GetTailleCellule());
        return vec;
    }

    // Update is called once per frame
    void Update()
    {

        //print(Joueur1.GetComponent<Player>().GetPosition());
    }


    public Vector2 getPosInMaze(GameObject o)
    {
        Vector3 posO = o.GetComponent<Transform>().position;
        //print("" + posO.x + posO.y + posO.z);
        Vector2 positionInMaze = new Vector2(Mathf.CeilToInt(posO.x) + maze.groudSize.x / 2 - 1, Mathf.CeilToInt(posO.z) + maze.groudSize.y / 2 - 1);
        return positionInMaze;
    }
}