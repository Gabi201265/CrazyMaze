using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MazeGen : MonoBehaviour
{
    //matrice 0= vide || 1= mur
    private int[,] matrice;
    private int[,] matriceVisite;
    private int currentI;
    private int currentJ;
    private Stack<(int, int)> pile = new Stack<(int, int)>();


    private int tailleVisiteX;
    private int tailleVisiteY;
    [SerializeField]
    private GameObject wallPrefabs=null;
    public GameObject cardinalPrefab;

    public Vector3 groudSize; // valeur possible : 5n -1  
    private int sizeCellule;


    
    public void AffichageMatrice(int[,] mat)
    {

        string s = "";
        int row = mat.GetLength(0);
        int col = mat.GetLength(1);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                s += mat[i, j];
            }
            s += "\n";

        }
        print(s);
    }
    //Accesseur de l'attribut matrice ! 
    public int [,] GetMatrice()
    {
        return this.matrice;
    }
    //Accesseur de l'attribut TailleVisiteX ! 
    public int GetTailleVisiteX()
    {
        return tailleVisiteX;
    }
    //Accesseur de l'attribut TailleVisiteY ! 
    public int GetTailleVisiteY()
    {
        return tailleVisiteY;
    }

    //Accesseur de l'attribut tailleCellule ! 
    public int GetTailleCellule()
    {
        return sizeCellule;
    }
    // Start is called before the first frame update
    public void Awake()
    {
        //setup de valeur importante
        sizeCellule = 5;
        groudSize = transform.localScale;

        //Initialisation de la matrice rempli
        matrice = new int[(int)groudSize.x, (int)groudSize.y];
        int tailleY = matrice.GetLength(0);//Ligne
        int tailleX = matrice.GetLength(1);//colonne
        for (int i = 0; i < tailleX; i++)
        {
            for (int j = 0; j < tailleY; j++)
            {

                matrice[i, j] = 0;
            }
        }
        //initialisation les bordures
        for (int i = 0; i < tailleX; i++)
        {
            for (int j = 0; j < tailleY; j++)
            {
                if (i == 0 || i == tailleX-1 || j == 0 || j == tailleY-1 )
                {
                    matrice[i, j] = 1;

                }
                if (i % sizeCellule == 0 || j % sizeCellule == 0)
                {
                    matrice[i, j] = 1;
                }
            }

        }

        

        //matrice des visites
        matriceVisite = new int[(int)groudSize.x / sizeCellule, (int)groudSize.y / sizeCellule];
        //Initialisation de la matrice booléenne
        tailleVisiteY = matriceVisite.GetLength(0);//ligne
        tailleVisiteX = matriceVisite.GetLength(1);//colonne
        for (int i = 0; i < tailleVisiteX; i++)
        {
            for (int j = 0; j < tailleVisiteY; j++)
            {
                matriceVisite[i, j] = 0;
            }
        }
        //print("--------------------------------------------------------------------------------");
        //AffichageMatrice(matriceVisite);

        //On suit la page wikipedia https://fr.wikipedia.org/wiki/Modélisation_mathématique_d%27un_labyrinthe
        //On choisit arbitrairement une cellule, on stocke la position en cours et on la marque comme visitée (vrai).
        currentI = Random.Range(0, tailleVisiteX);
        //print(currentI);
        currentJ = Random.Range(0, tailleVisiteY);
        //print(currentJ);
        matriceVisite[currentI, currentJ] = 1;
        pile.Push((currentI, currentJ));
        GenLab();
        GenererMaze();
    }
    private void GenererMaze()
    {
        //AffichageMatrice(matrice);
        for (int i = 0; i < groudSize.x; i++)
        {
            for (int j = 0; j < groudSize.y; j++)
            {
                if (matrice[i, j] == 1)
                {
                    Vector3 posWall = new Vector3(-groudSize.x / 2+0.5f + i , 1.5f, -groudSize.y / 2 + j+0.5f);
                    GameObject wall =Instantiate(wallPrefabs, posWall, Quaternion.identity) as GameObject;
                    wall.transform.localScale = new Vector3( 1,3,1);
                }
                
            }
        }
        Vector3 pos = new Vector3(-groudSize.x / 2 + 0.5f + groudSize.x / 2, 1.5f, -groudSize.y / 2 + groudSize.y + 0.5f);
        GameObject est = Instantiate(cardinalPrefab, pos, Quaternion.identity) as GameObject;
        est.GetComponent<TextMesh>().text = "est";
        est.GetComponent<TextMesh>().color = Color.red;
        est.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 0);

        pos = new Vector3(-groudSize.x / 2 + 0.5f + groudSize.x / 2, 1.5f, -groudSize.y / 2 + 0.5f);
        GameObject ouest = Instantiate(cardinalPrefab, pos, Quaternion.identity) as GameObject;
        ouest.GetComponent<TextMesh>().text = "ouest";
        ouest.GetComponent<TextMesh>().color = Color.red;
        ouest.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 180);
        
        pos = new Vector3(-groudSize.x / 2 + 0.5f, 1.5f, 0.5f);
        GameObject nord = Instantiate(cardinalPrefab, pos, Quaternion.identity) as GameObject;
        nord.GetComponent<TextMesh>().text = "nord";
        nord.GetComponent<TextMesh>().color = Color.red;
        nord.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, 90);

        pos = new Vector3(-groudSize.x / 2 + 0.5f + groudSize.x, 1.5f, -groudSize.y / 2 + groudSize.y / 2 + 0.5f);
        GameObject sud = Instantiate(cardinalPrefab, pos, Quaternion.identity) as GameObject;
        sud.GetComponent<TextMesh>().text = "sud";
        sud.GetComponent<TextMesh>().color = Color.red;
        sud.GetComponent<Transform>().rotation *= Quaternion.Euler(90, 0, -90);


    }
    void GenLab()
    {
        
        while (pile.Count != 0)
        {
            int interI = currentI * sizeCellule + 1;
            //print("InterI : " + interI);
            int interJ = currentJ * sizeCellule + 1;
            //print("InterJ : " + interJ);
            //print("InterI,InterJ : " + matrice[interI, interJ]);
            //Puis on regarde quelles sont les cellules voisines possibles et non visitées.
            int celluleHaut;
            int celluleDroite;
            int celluleBas;
            int celluleGauche;
            try
            {
                celluleHaut = matriceVisite[currentI - 1, currentJ];
            }
            catch (System.IndexOutOfRangeException e)
            {
                celluleHaut = 1;
            }
            try
            {
                celluleBas = matriceVisite[currentI + 1, currentJ];
            }
            catch (System.IndexOutOfRangeException e)
            {
                celluleBas = 1;
            }
            try
            {
                celluleGauche = matriceVisite[currentI, currentJ - 1];
            }
            catch (System.IndexOutOfRangeException e)
            {
                celluleGauche = 1;
            }
            try
            {
                celluleDroite = matriceVisite[currentI, currentJ + 1];
            }
            catch (System.IndexOutOfRangeException e)
            {
                celluleDroite = 1;
            }
            ////print("cell droite:"  +celluleDroite);
            ////print("cell gauche:" + celluleGauche);
            ////print("cell haut:" + celluleHaut);
            ////print("cell bas:" + celluleBas);

            string[] choixPossibles = new string[4];
            int compteur = 0;

            if (celluleHaut == 0)
            {
                choixPossibles[compteur] = "haut";
                compteur += 1;
            }
            if (celluleBas == 0)
            {
                choixPossibles[compteur] = "bas";
                compteur += 1;
            }
            if (celluleGauche == 0)
            {
                choixPossibles[compteur] = "gauche";
                compteur += 1;
            }
            if (celluleDroite == 0)
            {
                choixPossibles[compteur] = "droite";
                compteur += 1;
            }

            //S'il y a au moins une possibilité, on en choisit une au hasard, on ouvre le mur et on recommence avec la nouvelle cellule
            if (compteur != 0)
            {
                int choix = Random.Range(0, compteur);
                string result = choixPossibles[choix];
                //print("resultat aleatoire : " + result);
                if (result == "droite")
                {
                    currentJ += 1;
                    matriceVisite[currentI, currentJ] = 1;


                    //Il faut ouvrir le mur ! 
                    //De base on es en haurt à gauche de chaque case !!!!
                    for (int i = 0; i < sizeCellule - 1; i++)
                    {
                        //interY += 1;
                        matrice[interI + i, interJ + sizeCellule - 1] = 0;
                    }
                }
                else if (result == "gauche")
                {
                    currentJ -= 1;
                    matriceVisite[currentI, currentJ] = 1;

                    //Il faut ouvrir le mur ! 
                    for (int i = 0; i < sizeCellule - 1; i++)
                    {
                        //interY -= 1;
                        matrice[interI + i, interJ - 1] = 0;
                    }

                }
                else if (result == "haut")
                {
                    currentI -= 1;
                    matriceVisite[currentI, currentJ] = 1;

                    //Il faut ouvrir le mur ! 
                    for (int i = 0; i < sizeCellule - 1; i++)
                    {
                        //interX -= 1;
                        matrice[interI - 1, interJ + i] = 0;

                    }
                }
                else if (result == "bas")
                {
                    currentI += 1;
                    matriceVisite[currentI, currentJ] = 1;

                    //Il faut ouvrir le mur ! 
                    for (int i = 0; i < sizeCellule - 1; i++)
                    {
                        //interX += 1;
                        matrice[interI + sizeCellule - 1, interJ + i] = 0;
                    }
                }
                pile.Push((currentI, currentJ));
                //AffichageMatrice(matrice);
                //print("-----------------------------------------------------------");
                //AffichageMatrice(matriceVisite);
            }
            //S'il n'y en pas, on revient à la case précédente et on recommence.
            else
            {
                (currentI, currentJ) = pile.Pop();
            }
            GenLab();
        }
        
    }
}
