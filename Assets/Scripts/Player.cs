using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    [Header("Mvt Setup")]
    [Tooltip("unit: m/s")]
    float m_TranslationSpeed;
    [Tooltip("unit: °/s")]
    float m_RotationSpeed;


    public Vector3 positionInMaze;

    private int lives;
    private int MaxLives = 3;
    public static Player instance;
    Rigidbody m_Rigidbody;

    public GameObject prefabLives;
    private GameObject Lives;

    private Animator animator;

    bool check;

    private void Awake()
    {
        instance = this;
        m_Rigidbody = GetComponent<Rigidbody>();
        m_RotationSpeed = 120;
        m_TranslationSpeed = 20;
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lives = MaxLives;
        Lives = Instantiate(prefabLives, new Vector3(0, 0, 0), Quaternion.identity);
        check = false;
    }
    void Update()
    {
        
    }
    private void OnAnimatorMove()
    {
        
    }
    private void FixedUpdate()
    {
        float vInput = 0;
        float hInput = 0;
        //Par TAG
        if (this.gameObject.CompareTag("player1"))
        {
            //Dynamique

             vInput = Input.GetAxis("P2_Vertical"); // entre -1 et 1
             hInput = Input.GetAxisRaw("P2_Horizontal"); // entre -1 et 1

            // MODE VELOCITY
            Vector3 targetVelocity = vInput * m_TranslationSpeed * Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            Vector3 velocityChange = targetVelocity - m_Rigidbody.velocity;
            m_Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

            Vector3 targetAngularVelocity = hInput * m_RotationSpeed * transform.up;
            Vector3 angularVelocityChange = targetAngularVelocity - m_Rigidbody.angularVelocity;
            m_Rigidbody.AddTorque(angularVelocityChange, ForceMode.VelocityChange);

            Quaternion qRotUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
            Quaternion qOrientSlightlyUpright = Quaternion.Slerp(transform.rotation, qRotUpright * transform.rotation, Time.fixedDeltaTime * 4);
            m_Rigidbody.MoveRotation(qOrientSlightlyUpright);

        }
        else if (this.gameObject.CompareTag("player2"))
        {
            //Dynamique

             vInput = Input.GetAxis("P1_Vertical"); // entre -1 et 1
             hInput = Input.GetAxisRaw("P1_Horizontal"); // entre -1 et 1

            // MODE VELOCITY
            Vector3 targetVelocity = vInput * m_TranslationSpeed * Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            Vector3 velocityChange = targetVelocity - m_Rigidbody.velocity;
            m_Rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

            Vector3 targetAngularVelocity = hInput * m_RotationSpeed * transform.up;
            Vector3 angularVelocityChange = targetAngularVelocity - m_Rigidbody.angularVelocity;
            m_Rigidbody.AddTorque(angularVelocityChange, ForceMode.VelocityChange);

            Quaternion qRotUpright = Quaternion.FromToRotation(transform.up, Vector3.up);
            Quaternion qOrientSlightlyUpright = Quaternion.Slerp(transform.rotation, qRotUpright * transform.rotation, Time.fixedDeltaTime * 4);
            m_Rigidbody.MoveRotation(qOrientSlightlyUpright);
        }
        
        //Pour executer la boucle une seule fois
        if (this.lives==0 && check ==false)
        {
            GameObject gm = GameObject.FindGameObjectWithTag("GameManager");
            gm.GetComponent<GameManager>().GameState = GameManager.State.GameOver;
            check = true;
        }
    }
    public void SetPosition(Vector3 vec)
    {
        positionInMaze = vec;
        this.transform.position = positionInMaze;
    }
    public Vector3 GetPosition()
    {
        return positionInMaze;
    }
    public int GetLives()
    {
        return lives;
    }
    public void SetLives(int newLives)
    {
        lives=newLives;
    }
    public void setPosInMaze(float halfGround)
    {
        Vector3 posPlayer = gameObject.GetComponent<Transform>().position;
        positionInMaze= new Vector3((int)posPlayer.x + halfGround,0, (int)posPlayer.z + halfGround);
    }
   
}
