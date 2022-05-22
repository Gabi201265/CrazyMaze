using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class IAMinotaure : MonoBehaviour
{

    float time2;
    float TimerInterval2 = 15;
    float tick2;

    public MazeGen maze;
    private int[,] DIST;
    private int[,] matrice;

    private int[] position;
    private GameHub hub;

    //private List<Player> players;
    private GameObject FirstPlayer;
    private GameObject SecondPlayer;
    private Vector2 FirstPlayerPos;
    private Vector2 SecondPlayerPos;
    private int tailleX;
    private int tailleY;

    int compteur;
    // Start is called before the first frame update

    public AudioClip criMino;
    AudioSource cri;
    float intensiteCri = 0.1f;

    //Musique d'ambiance pour accélere le pitch
    AudioSource musiqueAmbiance;
    void Awake()
    {
        tick2 = TimerInterval2; 
    }
    public void Start()
    {
        cri = GetComponent<AudioSource>();
        GameObject gameHub = GameObject.FindGameObjectWithTag("GameHub");

        hub = gameHub.GetComponent<GameHub>();
        musiqueAmbiance = hub.GetComponent<AudioSource>();

        //maze = new MazeGen();
        //maze.Start();
        maze = hub.maze;
        this.matrice = maze.GetMatrice();



        //maze.AffichageMatrice(this.matrice);

        DIST = (int[,])matrice.Clone();
        
        tailleX = DIST.GetLength(1);//Renvoie le nombre de colonne
        tailleY = DIST.GetLength(0);//Renvoie le nombre de ligne

        //Poisition de départ du minautore.
        position = new int[2];
        position[0] = (int)hub.getPosInMaze(gameObject).x;
        position[1] = (int)hub.getPosInMaze(gameObject).y;
        //AffichageMatrice(DIST, "");
    }

    //Initialisation de la carte des distances
    int[,] PreInitDistPlayer()
    {
        //AffichageMatrice(matrice,"");
        for (int x = 0; x < tailleX; x++)
        {
            for (int y = 0; y < tailleY; y++)
            {
                if (matrice[x, y] == 1)
                {
                    DIST[x, y] = 999;
                }
                else
                {
                    DIST[x, y] = 998;
                }
            }
        }
        return DIST;
    }
    int[,] InitDistPlayer(int xp, int yp)
    {
        //xp += (int)(maze.groudSize.x / 2);
        //yp += (int)(maze.groudSize.y / 2);
        for (int x = 0; x < tailleX; x++)
        {
            for (int y = 0; y < tailleY; y++)
            {
                if (DIST[x, y] == 0)
                {
                    continue;
                }
                else if (DIST[x, y] == 999)
                {
                    continue;
                }
                else if (x == xp && y == yp)
                {
                    DIST[y, x] = 0;
                }
                else
                {
                    DIST[x, y] = 998;
                }
            }
        }
        return DIST;
    }

    int[,] UpdateDistPlayer()
    {
        //print("coucou");
        for (int x = 1; x < tailleX - 1; x++)
        {
            for (int y = 1; y < tailleY - 1; y++)
            {
                //print("coucou1");
                if (DIST[x, y] == 999)
                {
                    //print("coucou2");
                    continue;
                }
                else
                {
                    int valeurMin = Minimum(new int[4] { DIST[x + 1, y], DIST[x, y + 1], DIST[x - 1, y], DIST[x, y - 1] });
                    //print("valeurMin : " + valeurMin);
                    if (DIST[x, y] > valeurMin)
                    {
                        DIST[x, y] = valeurMin + 1;
                        //print("coucou3");
                    }
                    //print("coucou4");
                }
            }
        }
        return DIST;
    }

    int[,] CalculDistPlayer(int xp, int yp)
    {
        /*InitDistPlayer(xp, yp);
        int[,] Previous = (int[,])DIST.Clone();
        // Parcours de calcules des distances
        while (Comparer(DIST, Previous) == false)
        {
            Previous = (int[,])DIST.Clone();
            UpdateDistPlayer();
        }
        UpdateDistPlayer();
        return DIST;*/
        InitDistPlayer(xp, yp);
        // Parcours de calcules des distances
        for (int i = 0; i < 100; i++)
        {
            UpdateDistPlayer();
        }
        return DIST;
    }

    string[] MinautorePossibleMove()
    {
        string[] choixPossibles = new string[4];
        compteur = 0;
        //print("PositionXMinautore : " + position[1] + "  /  PositionYMinautore : " + position[0]);
        int caseHaut = DIST[position[0] - 1, position[1]];
        //print("caseHaut " + caseHaut);
        int caseBas = DIST[position[0] + 1, position[1]];
        //print("caseBas " + caseBas);
        int caseGauche = DIST[position[0], position[1] - 1];
        //print("caseGauche " + caseGauche);
        int caseDroite = DIST[position[0], position[1] + 1];
        //print("caseDroite " + caseDroite);
        int valeurMin = Minimum(new int[4] { caseHaut, caseBas, caseDroite, caseGauche });
        print("ValeurMin : " + valeurMin);
        if (caseGauche == valeurMin)
        {
            choixPossibles[compteur] = "gauche";
            compteur += 1;
        }
        if (caseDroite == valeurMin)
        {
            choixPossibles[compteur] = "droite";
            compteur += 1;
        }
        if (caseBas == valeurMin)
        {
            choixPossibles[compteur] = "bas";
            compteur += 1;
        }
        if (caseHaut == valeurMin)
        {
            choixPossibles[compteur] = "haut";
            compteur += 1;
        }
        //AffichageMatrice(DIST, "");

        /*print("choixPossibles :");
        
        for (int i = 0; i < choixPossibles.Length; i++)
        {
            print("choixPossibles" + "[" + i + "] = " + choixPossibles[i]);
        }*/

        return choixPossibles;
    }
    private Vector3 IA()
    {

        DIST = PreInitDistPlayer();
        DIST = CalculDistPlayer((int)FirstPlayerPos.y, (int)FirstPlayerPos.x);
        DIST = CalculDistPlayer((int)SecondPlayerPos.y, (int)SecondPlayerPos.x);
        //deplacement Minautore
        string[] choixPossibles = MinautorePossibleMove();
        int choix = Random.Range(0, compteur);
        string result = choixPossibles[choix];
        Vector3 vec = new Vector3(0,0,0);
        //print(result);
        if (result == "haut")
        {
            vec = new Vector3(-1, 0, 0);
            
            //transform.Translate(-1, 0, 0);
        }
        else if (result == "bas")
        {
            vec = new Vector3(1, 0, 0);
            
            //transform.Translate(1, 0, 0);

        }
        else if (result == "droite")
        {
            vec = new Vector3(0, 0, 1);
            
            //transform.Translate(0, 0, 1);
        }
        else if (result == "gauche")
        {
            vec = new Vector3(0, 0, -1);
            
            //transform.Translate(0, 0, -1);
        }
        position[0] = (int)hub.getPosInMaze(gameObject).x;
        position[1] = (int)hub.getPosInMaze(gameObject).y;
        //print(result);
        //AffichageMatrice(DIST);

        //print("PositionXMinautore : " + position[1] + "  /  PositionYMinautore : " + position[1]);


       /* print("Vie j1 : " + FirstPlayer.GetComponent<Player>().GetLives());
        print("Vie j2 : " + SecondPlayer.GetComponent<Player>().GetLives());*/
        if (position[1] == FirstPlayerPos.y && FirstPlayerPos.x == position[0])
        {
            print("collision vec le Player1");
            FirstPlayer.GetComponent<Player>().SetLives(FirstPlayer.GetComponent<Player>().GetLives()-1);
            Vector3 respawn = hub.getAleaSpawn();
            FirstPlayer.GetComponent<Player>().SetPosition(respawn);
        }
        if (position[1] == SecondPlayerPos.y && SecondPlayerPos.x == position[0])
        {
            print("collision vec le Player2");
            SecondPlayer.GetComponent<Player>().SetLives(SecondPlayer.GetComponent<Player>().GetLives() - 1);
            Vector3 respawn = hub.getAleaSpawn();
            SecondPlayer.GetComponent<Player>().SetPosition(respawn);
        }
        return vec;

    }
    IEnumerator waiter(int sec)
    {
        yield return new WaitForSeconds(sec);
    }


    // Update is called once per frame
    void Update()
    {
        FirstPlayer = hub.getPlayer1();
        FirstPlayerPos = hub.getPosInMaze(FirstPlayer);

        SecondPlayer = hub.getPlayer2();
        SecondPlayerPos = hub.getPosInMaze(SecondPlayer);

        //print("XPLAYER :" + players.position.x + " / YPLAYER : " + players.position.y);
        //AffichageMatrice(matrice);
        /*print(tick);
        print(tick2);*/
        GameObject gm = GameObject.FindGameObjectWithTag("GameManager");

        Vector3 vec = IA();
        transform.Translate(vec * Time.deltaTime* gm.GetComponent<GameManager>().Level);
        
        time2 += Time.deltaTime;
        print(time2);
        //Gestion du cri du mino
        if (time2 >= tick2)
        {

            time2 = 0;
            //AffichageMatrice(DIST,"light");
            if (DIST[position[0], position[1]] < 20)//SI le minautore approche on modifie le pitch
            {
                musiqueAmbiance.pitch *= 1.1f;
            }
            else
            {
                musiqueAmbiance.pitch = 1.0f;
            }
            if (DIST[position[0], position[1]] > 40)
            {
                intensiteCri = 0.13f;
            }
            else if (DIST[position[0], position[1]] > 30)
            {
                intensiteCri = 0.14f;
            }
            else if (DIST[position[0], position[1]] > 20)
            {
                intensiteCri = 0.15f;
            }
            else if (DIST[position[0], position[1]] > 10)
            {
                intensiteCri = 0.17f;
            }
            else if (DIST[position[0], position[1]] > 5)
            {
                intensiteCri = 0.18f;
            }
            else if (DIST[position[0], position[1]] <= 5)
            {
                intensiteCri = 0.3f;
            }
            cri.PlayOneShot(criMino, intensiteCri);
        }
    }


    //Fonctions utilitaires à déplacer dans un fichier utilitaire
    int Minimum(int[] tab)
    {
        int mimimum = 999;
        for (int i = 0; i < tab.Length; i++)
        {
            //print("Tab[" + i + "] = " + tab[i]);
            if (mimimum > tab[i])
            {
                mimimum = tab[i];
            }
        }
        return mimimum;
    }


    private void AffichageMatrice(int[,] mat, string mode)
    {

        string s = "";
        int row = mat.GetLength(0);
        //print(row);
        int col = mat.GetLength(1);
        //print(col);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (i == 0 && j == 0)
                {
                    s += "00";
                }
                if (i == position[0] && j == position[1])
                {
                    s += " X ";
                }

                //else if(i == maze.groudSize.x/2 && j ==maze.groudSize.y/2 )

                else if (i == (int)FirstPlayerPos.x && j == (int)FirstPlayerPos.y)

                {
                    s += " P ";
                }
                else if (i == (int)SecondPlayerPos.x && j == (int)SecondPlayerPos.y)

                {
                    s += " P ";
                }
                else
                {
                    if (mode == "light")
                    {
                        if (mat[i, j] == 999)
                        {
                            s += "I";
                        }
                        else
                        {
                            s += 0;
                        }
                    }
                    else
                    {
                        s += mat[i, j];
                    }
                }
            }
            s += "\n";
        }
        print(s);

    }


}
