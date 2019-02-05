using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float speed = 1.5f;
    public int noOfTeleports = 2;
    public int Score = 0;
    public bool isActive = false;

    public GameObject Gm;
    private GameScript maingame;

    public GameObject AiPrefab;
    private AiAgent Ai1;

    public GameObject Enemy;
    public GameObject Enemy2;

    // Use this for initialization
    void Start () {
        maingame = Gm.GetComponent<GameScript>();
        Ai1 = AiPrefab.GetComponent<AiAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive)
        {
            playerMove();
        }
        if (noOfTeleports >0)
        {
            playerTelport();
        }
        
    }

    void playerMove()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position += Vector3.back * speed * Time.deltaTime;
        }
    }

    // One teleport for the enemy one for the agent
    void playerTelport()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            
            if (maingame.WS.distBWagents < 2)
            {
                maingame.teleportRandom(AiPrefab);
                noOfTeleports -= 1;
            }

            else if ( this.transform.position.z < 1.8)
            {
                Enemy.GetComponent<EnemyMain>().reSpawn();
                noOfTeleports -= 1;
            }

            else if (this.transform.position.z > 1.8)
            {
                //Teleport Enemy1
                Enemy2.GetComponent<EnemyMain>().reSpawn();
                noOfTeleports -= 1;
            }

            //maingame.teleportRandom(AiPrefab);
        }
    }
}
