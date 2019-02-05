using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {
    public GameObject[] enemies;
    public GameObject Door1;
    public GameObject Door2;
    public GameObject Door3;
    public GameObject Door4;
    public GameObject Obs1;
    public GameObject Obs2;

    public GameObject[] items;
    private WorldState[] possibleStates; // keeps track of all possible states and selects the best one 
    public GameObject AiAgentPrefab;
    public GameObject PlayerAgentPrefab;
    private Player P1;
    private AiAgent Ai1;
    public WorldState WS;
    public Planner HTN;

    public GameObject E1;
    public GameObject E2;


    // SpawnPoints
    private GameObject SP1;
    private GameObject SP2;
    private GameObject SP3;
    private GameObject SP4;
    private GameObject SP5;
    private GameObject SP6;
    private GameObject SP7;
    private GameObject SP8;
    private GameObject SP9;
    private GameObject SP10;

    public Text AiScore;
    public Text P1Score;

    public Text winner;
    public Text loser;

    public List<GameObject> AllPickedObjects;

    public int rndno = 0;
    // Use this for initialization
    void Start () {

        winner.gameObject.SetActive(false);
        loser.gameObject.SetActive(false);

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        items = GameObject.FindGameObjectsWithTag("Pickable");
        Ai1 = AiAgentPrefab.GetComponent<AiAgent>();
        P1 = PlayerAgentPrefab.GetComponent<Player>();

        SP1 = GameObject.Find("SP1");
        SP2 = GameObject.Find("SP2");
        SP3 = GameObject.Find("SP3");
        SP4 = GameObject.Find("SP4");
        SP5 = GameObject.Find("SP5");
        SP6 = GameObject.Find("SP6");
        SP7 = GameObject.Find("SP7");
        SP8 = GameObject.Find("SP8");
        SP9 = GameObject.Find("SP9");
        SP10 = GameObject.Find("SP10");

        disableSpawnPoint(); // For testing spawn points are visable, they are invisable in runtime 


        InvokeRepeating("delayedRandomNumberGenerator", 0, 5f); // new number every 2 seconds
        spawnAtRandom();

    }
	
	// Update is called once per frame
	void Update () {
        WS.AiScore = Ai1.score;
        WS.PlayerScore = P1.Score;
        updateScoresText();
        enemyCollision();
        Planner();
        pickItem();
        isGameEnding();

    }

    // Handles Collision Detection and Response
    void enemyCollision()
    {

        foreach (GameObject e in enemies) {
            if (e.transform.name == "Enemy1")
            {
                // if colliding with door
                if (e.GetComponent<BoxCollider>().bounds.Intersects(Door2.GetComponent<BoxCollider>().bounds))
                {
                    // On hit change enemy direction
                    e.GetComponent<EnemyMain>().reSpawn();
                }
                if (e.GetComponent<BoxCollider>().bounds.Intersects(Door3.GetComponent<BoxCollider>().bounds))
                {
                    // On hit change enemy direction
                    e.GetComponent<EnemyMain>().reSpawn();
                }

                // if colliding with obs

                if (e.GetComponent<BoxCollider>().bounds.Intersects(Obs1.GetComponent<BoxCollider>().bounds))
                {
                    // React 1: Pass through 
                    // If the random number generated rn is greater than 8 --> rare occasions, passes through instead 
                    if (Vector3.Distance(e.transform.position, Obs2.transform.position) < 1)
                    {
                        if (rndno < 8)
                        {
                            e.GetComponent<EnemyMain>().inBetween = true;
                            e.GetComponent<EnemyMain>().FieldOfView.SetActive(false);
                        }
                        if(rndno == 5)
                        {
                            e.GetComponent<EnemyMain>().reSpawn();
                        }
                    }




                    if (e.GetComponent<EnemyMain>().direction == EnemyMain.MovementState.Left)
                    {
                        e.GetComponent<EnemyMain>().ChangeDirection("Right");
                    }
                    else
                    {
                        e.GetComponent<EnemyMain>().ChangeDirection("Left");
                    }


                    if (Vector3.Distance(e.transform.position, Obs2.transform.position) > 1)
                    {
                        e.GetComponent<EnemyMain>().inBetween = false;
                        e.GetComponent<EnemyMain>().FieldOfView.SetActive(true);
                    }

                }
            }

            if (e.transform.name == "Enemy3")
            {
                // if colliding with door
                if (e.GetComponent<BoxCollider>().bounds.Intersects(Door4.GetComponent<BoxCollider>().bounds))
                {
                    //print("Hitting door");
                    print("Hitting door");
                    e.GetComponent<EnemyMain>().reSpawn();
                }
                if (e.GetComponent<BoxCollider>().bounds.Intersects(Door1.GetComponent<BoxCollider>().bounds))
                {
                    print("Hitting door");
                    e.GetComponent<EnemyMain>().reSpawn();
                }


                // if colliding with obs
                // React 1: Pass through 
                // If the random number generated rn is greater than 8 --> rare occasions, passes through instead 
                if (Vector3.Distance(e.transform.position, Obs2.transform.position) < 1)
                {
                    if (rndno <8)
                    {
                        e.GetComponent<EnemyMain>().inBetween = true;
                        e.GetComponent<EnemyMain>().FieldOfView.SetActive(false);
                    }
                    if(rndno == 5)
                    {
                        e.GetComponent<EnemyMain>().reSpawn();
                    }

                }

                if (e.GetComponent<BoxCollider>().bounds.Intersects(Obs2.GetComponent<BoxCollider>().bounds) && e.GetComponent<EnemyMain>().inBetween == false)
                {
                    // react 2: Change direction                    
                    if (e.GetComponent<EnemyMain>().direction == EnemyMain.MovementState.Left)
                    {
                        e.GetComponent<EnemyMain>().ChangeDirection("Right");
                    }
                    else
                    {
                        e.GetComponent<EnemyMain>().ChangeDirection("Left");
                    }
                    
                }

                if (Vector3.Distance(e.transform.position, Obs2.transform.position) > 1)
                {
                        e.GetComponent<EnemyMain>().inBetween = false ;
                        e.GetComponent<EnemyMain>().FieldOfView.SetActive(true);
                }


            }
        }
    }

    void obtainItem()
    {
        foreach (GameObject e in enemies) {
            foreach (GameObject item in items)
            {
                if (e.GetComponent<BoxCollider>().bounds.Intersects(item.GetComponent<BoxCollider>().bounds))
                {
                    // e gets the item and item gets destroyed 
                }
            }
        }
    }
    // Picking of items


    // if p doesn't exist in all picked objects, then object has been picked 
    void pickItem()
    {
        foreach(GameObject P in items)
        {
            if (P.GetComponent<BoxCollider>().bounds.Intersects(Ai1.GetComponent<BoxCollider>().bounds) && !AllPickedObjects.Contains(P))
            {
                AllPickedObjects.Add(P);
                P.GetComponent<SpriteRenderer>().enabled = false;
                Ai1.score += 1;
            }
        }

        foreach (GameObject P in items)
        {
            if (P.GetComponent<BoxCollider>().bounds.Intersects(P1.GetComponent<BoxCollider>().bounds) && !AllPickedObjects.Contains(P))
            {
                AllPickedObjects.Add(P);
                P.GetComponent<SpriteRenderer>().enabled = false;
                P1.Score += 1;
            }
        }
    }

    void Planner()
    {
        WorldState pred_WS = new WorldState();
        pred_WS = stateClone(pred_WS,WS);


        // Get the primitive actions first for Method 1 
        if (HTN.transverse() == "Idle2" || HTN.transverse() == "Idle1" || HTN.transverse() == "Idle3")
        {
            if(WS.Enemy3_InComing && WS.Enemy3_inRange) // precondition
            {
                Ai1.Tasks.Push(AiActions.StayIdle);
                WS.Enemy3_InComing = Ai1.CheckRange(E2);    // Effect 
                WS.Enemy3_inRange = Ai1.comingTowards(E2);
                
            }

            if (WS.Enemy1_InComing && WS.Enemy1_inRange) // precondition
            {
                Ai1.Tasks.Push(AiActions.StayIdle);
                WS.Enemy1_InComing = Ai1.CheckRange(E1);    // Effect 
                WS.Enemy1_inRange = Ai1.comingTowards(E1);

            }

            HTN.decompose.Add(HTN.transverse());
        }

        if (HTN.transverse() == "MoveToEnclave1" || HTN.transverse() == "MoveToEnclave2")
        {
            //on the lower side 
            if (!WS.Enemy3_inRange && Ai1.transform.position.z<1.8) 
            {
                Ai1.Tasks.Push(AiActions.MoveForward);
                WS.Enemy3_inRange = Ai1.CheckRange(E2);
            }
            // On the upper side 
            if (!WS.Enemy1_inRange && Ai1.transform.position.z>1.8 )
            {
                Ai1.Tasks.Push(AiActions.MoveForward);
                WS.Enemy1_inRange = Ai1.CheckRange(E1);
            }

            HTN.decompose.Add(HTN.transverse());
        }

        // Using teleport more than once, use it once only 
        if (HTN.transverse() == "Teleport")
        {
            if (!Ai1.firstTelePort || !Ai1.secondTelePort)
            {
                if (WS.PlayerScore == 4)
                {
                    WS.whatToTeleport = "Enemy";
                    Ai1.Tasks.Push(AiActions.Teleport);
                }

                if (WS.distBWagents < 1)
                {
                    WS.whatToTeleport = "Player";
                    Ai1.Tasks.Push(AiActions.Teleport);
                }
            }


            HTN.decompose.Add(HTN.transverse());
        }

        // Empty decomposed
        if (HTN.transverse() == "Win")
        {
            HTN.emptyDecomposition(); //Restart my planning
        }

        HTN.decompose.Add(HTN.transverse());
    }


    // used at the start to spawn the Agents in a random enclave 
    private void spawnAtRandom()
    {

        System.Random rnd = new System.Random();
        int a = rnd.Next(0, 10);
        int b = rnd.Next(0, 10);

       // print(SP9.transform.position);
        print(b);
        if (a == 1){Ai1.transform.position = SP1.transform.position;}
        if (a == 2) { Ai1.transform.position = SP2.transform.position; }
        if (a == 3) { Ai1.transform.position = SP3.transform.position; }
        if (a == 5) { Ai1.transform.position = SP5.transform.position; }
        if (a == 6) { Ai1.transform.position = SP6.transform.position; }
        if (a == 7) { Ai1.transform.position = SP7.transform.position; }
        if (a == 10) { Ai1.transform.position = SP10.transform.position; }


        if (b == 1) { P1.transform.position = SP1.transform.position; }
        if (b == 2) { P1.transform.position = SP2.transform.position; }
        if (b == 3) { P1.transform.position = SP3.transform.position; }
        if (b == 4) { P1.transform.position = SP4.transform.position; }
        if (b == 5) { P1.transform.position = SP5.transform.position; }
        if (b == 6) { P1.transform.position = SP6.transform.position; }
        if (b == 7) { P1.transform.position = SP7.transform.position; }
        if (b == 8) { P1.transform.position = SP8.transform.position; }
        if (b == 9) { P1.transform.position = SP9.transform.position; }
        if (b == 10) { P1.transform.position = SP10.transform.position; }

    }


    // Shared function used between AiAgent and Player Agent
    public void teleportRandom(GameObject G)
    {
        System.Random rnd = new System.Random();
        float t = rnd.Next(0, 10);

        if (t == 1) { G.transform.position = SP1.transform.position; }
        if (t == 2) { G.transform.position = SP2.transform.position; }
        if (t == 3) { G.transform.position = SP3.transform.position; }
        if (t == 5) { G.transform.position = SP5.transform.position; }
        if (t == 6) { G.transform.position = SP6.transform.position; }
        if (t == 7) { G.transform.position = SP7.transform.position; }
        if (t == 10) { G.transform.position = SP10.transform.position; }
    }

    // Not Used 
    private WorldState stateClone(WorldState A, WorldState B)
    {
        A.Enemy1_InComing = B.Enemy1_InComing;
        A.Enemy1_inRange = B.Enemy1_inRange;
        A.Enemy3_InComing = B.Enemy3_InComing;
        A.Enemy3_inRange = B.Enemy3_inRange;


        A.ItemsLeft = B.ItemsLeft;
        A.Agent1_Predictive_Position = B.Agent1_Predictive_Position;
        A.Agent2_Predictive_Position = B.Agent2_Predictive_Position;

        A.AiScore = B.AiScore;
        A.PlayerScore = B.PlayerScore;

        A.Ai1State = B.Ai1State;
        A.ItemsList = B.ItemsList;

        A.Enemy1_Predictive_Position = B.Enemy1_Predictive_Position;
        A.Enemy3_Predictive_Position = B.Enemy3_Predictive_Position;

        return A;
    }



    // helper functions 
    private void delayedRandomNumberGenerator()
    {
        System.Random rnd = new System.Random();
        int x = rnd.Next(0, 10);

        rndno = x;
    }

    private void disableSpawnPoint()
    {
        SP1.GetComponent<SpriteRenderer>().enabled = false;
        SP2.GetComponent<SpriteRenderer>().enabled = false;
        SP3.GetComponent<SpriteRenderer>().enabled = false;
        SP4.GetComponent<SpriteRenderer>().enabled = false;
        SP5.GetComponent<SpriteRenderer>().enabled = false;
        SP6.GetComponent<SpriteRenderer>().enabled = false;
        SP7.GetComponent<SpriteRenderer>().enabled = false;
        SP8.GetComponent<SpriteRenderer>().enabled = false;
        SP9.GetComponent<SpriteRenderer>().enabled = false;
        SP10.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void updateScoresText()
    {
        AiScore.text = "AiScore: " + Ai1.score.ToString();
        P1Score.text = "PlayerScore: " + P1.Score.ToString();
    }


    // Checks for the game end condition, if one of them is caught or all the items collected -- > game ended --> make decision 
    private void isGameEnding()
    {
        if (Ai1.score + P1.Score == 10)
        {
            winState();
        }

        //
        foreach (GameObject e in enemies)
        {
            if (e.GetComponent<BoxCollider>().bounds.SqrDistance(P1.transform.position)<2)
            {
                P1.gameObject.SetActive(false);
                Ai1.gameObject.SetActive(false);
                loseState("Player 1 caught");
            }

            if (e.GetComponent<EnemyMain>().FieldOfView.GetComponent<BoxCollider>().bounds.Intersects(Ai1.GetComponent<BoxCollider>().bounds))
            {
                P1.gameObject.SetActive(false);
                Ai1.gameObject.SetActive(false);
                loseState("Ai Agent caught");
            }
        }
    }


    // Winning happens only if all the objects have been collected AND one of the players or the Ai has 10 score -- draw possible
    private void winState()
    {
        if(Ai1.score > P1.Score)
        {
            winner.text = "Agent 1 Wins";
        }

        if (Ai1.score < P1.Score)
        {
            winner.text = "Player 1 Wins";
        }

        if (Ai1.score > P1.Score)
        {
            winner.text = "Agent 1 Wins";
        }

        if (Ai1.score == P1.Score)
        {
            winner.text = "Draw";
        }

        winner.gameObject.SetActive(true);

    }

    private void loseState(string s)
    {
       loser.text = s;
       loser.gameObject.SetActive(true);
    }
}
