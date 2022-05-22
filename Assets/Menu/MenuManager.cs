using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public void ChangeState()
    {
        if (true == gameObject.activeInHierarchy) //Si l'objet est actif
        {
            gameObject.SetActive(false);	//alors on le désactive
        }
        else  //Sinon (donc il est inactif)
        {
            gameObject.SetActive(true); //alors on l'active
        }
    }
    public void OnQuit()
    {
        Application.Quit();
    }
    public void OnGameStart() 
    {
        GameObject gmObject = GameObject.FindWithTag("GameManager");
        GameManager gm = gmObject.GetComponent<GameManager>();
        gm.StartNewGame();
        gameObject.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
