using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour {

    [SerializeField]
    Transform Destination1;

    [SerializeField]
    Transform Destination2;

    [SerializeField]
    Transform Destination3;

    [SerializeField]
    Transform Destination4;

    [SerializeField]
    Transform Destination5;

    [SerializeField]
    Transform Destination6;

    [SerializeField]
    Transform Destination7;

    [SerializeField]
    Transform Destination8;

    [SerializeField]
    Transform Destination9;

    [SerializeField]
    Transform Destination10;


    public Transform PreviousDestination;
    Stack<Transform> ItemDestinations = new Stack<Transform>();
    public Stack<AiActions> Tasks = new Stack<AiActions>();

    Transform CurrentDis;

    NavMeshAgent Nv;

    public Enclave currentEnclave = Enclave.Lost;
    public Enclave previousEnclave = Enclave.Lost;

    public GameObject Enemy;
    public GameObject Enemy2;

    public GameObject E1_FOV;
    public GameObject E2_FOV;

    public GameObject WorldStatesPrefab;
    private WorldState WS;
    public string goal = "Win";
   public enum direction
    {
        Left,
        Right
    }

    public direction currentDirection;
    public bool firstTelePort = false;
    public bool secondTelePort = false;
    private bool hasbeenreset = false;
    private bool teleportused = false;
    public int score;

    public GameObject mainScript;
    public GameObject Player;

	// Use this for initialization
	void Start () {

        WS=WorldStatesPrefab.GetComponent<WorldState>();
        Nv = this.GetComponent<NavMeshAgent>();
        E1_FOV = Enemy.GetComponent<EnemyMain>().FieldOfView;
        E2_FOV = Enemy2.GetComponent<EnemyMain>().FieldOfView;
        //Destination4.GetComponent<SpriteRenderer>().enabled = false;
        //ItemDestinations.Push(Destination5);
        //ItemDestinations.Push(Destination4);
        //ItemDestinations.Push(Destination3);
        //ItemDestinations.Push(Destination2);
        //ItemDestinations.Push(Destination1);

        movetoneareastEnclave();
        MoveToNext();
        currentDirection = direction.Left;

    }
	
	// Update is called once per frame
	void Update () {
        // NextDestination();
        // movetoneareastEnclave(); undo for testing 
        PlanRunner();

        WS.Enemy1_inRange = CheckRange(Enemy2);
        WS.Enemy3_inRange = CheckRange(Enemy);
        
        WS.Enemy3_InComing=comingTowards(Enemy);
        WS.Enemy1_InComing=comingTowards(Enemy2);

        updateEnclavePosition();
        //printPlans();

        SafeAgainSensor();
        InComingSensor();

        if(this.transform.position.z > 1.8)
        {
            currentDirection = direction.Right;
        }

        if (this.transform.position.z < 1.8)
        {
            currentDirection = direction.Left;
        }
        distanceSensor();
    }



    // Operator1: Simple act of moving to the next position
    private void MoveToNext()
    {
        if (ItemDestinations != null && ItemDestinations.Count > 0)
        {
            CurrentDis = ItemDestinations.Pop();
            Vector3 t_vector = CurrentDis.transform.position;
            Nv.SetDestination(t_vector);
            //print(CurrentDis);
        }
    }

    // Primitive Task Move to the Point once reached the old one
    public void NextDestination()
    {
        if (CurrentDis != null)
        {
            float dist = Vector3.Distance(CurrentDis.position, transform.position);

            if (dist < 0.5)
            {
                MoveToNext();
            }
        }
    }

    // Check the range of the moving enemy  // Change worldstate instead 
    // Takes in Just the enemy
    public bool CheckRange(GameObject E)
    {

        float dist = E.GetComponent<EnemyMain>().FieldOfView.GetComponent<BoxCollider>().bounds.SqrDistance(this.transform.position);

        if(dist <3)
        {
            return true;
        }

        return false;
    }

    // Check towardsDirection, meaning the gap is going to get closwer 
    public bool comingTowards(GameObject E)
    {
        if ((E.GetComponent<EnemyMain>().direction == EnemyMain.MovementState.Right) && this.currentDirection == direction.Left)
        {
            return true;
        }

        if ((E.GetComponent<EnemyMain>().direction == EnemyMain.MovementState.Left) && this.currentDirection == direction.Right)
        {
            return true;
        }
        return false;
    }



    // stop movement
    public void remainIdle()
    {
        if (currentEnclave != Enclave.Lost)
        {
            Nv.isStopped = true;
        }   
    }

    // resume movement
    public void resumeMovement()
    {
        hasbeenreset = false; // Just for convience to make sure a reset is done only once to the pathways 
        teleportused = false; // same thing 
        Nv.isStopped = false;
    }


    // Teleport and set enemies state
    public void TeleportThenSet()
    {
        if (!teleportused)
        {
            teleportused = true;
            if (WS.whatToTeleport == "Player")
            {
                mainScript.GetComponent<GameScript>().teleportRandom(Player);
            }

            if(WS.whatToTeleport == "Enemy" && this.transform.position.z<1.8)
            {
                Enemy.GetComponent<EnemyMain>().reSpawn();
            }

            if (WS.whatToTeleport == "Enemy" && this.transform.position.z > 1.8)
            {
                //Teleport Enemy1
                Enemy2.GetComponent<EnemyMain>().reSpawn();
            }

            // To mark the teleports have been used 
            if (!firstTelePort)
            {
                firstTelePort = true;
            }
            else secondTelePort = true;
        }
    }

    // Helper function to indicate current enclave position  4.45?
    public void updateEnclavePosition()
    {
        if (currentEnclave != Enclave.Lost)
        {
            previousEnclave = currentEnclave;
        }

        if (this.transform.position.z< -1.3 && this.transform.position.x >6.9 && this.transform.position.x<7.9)
        {
            currentEnclave = Enclave.Enclave5;
        }

        else if (this.transform.position.z < -1.3 && this.transform.position.x > 4.25 && this.transform.position.x < 5.7)
        {
            currentEnclave = Enclave.Enclave4;
        }

        else if (this.transform.position.z < -1.3 && this.transform.position.x > 1.5 && this.transform.position.x < 3)
        {
            currentEnclave = Enclave.Enclave3;
        }

        else if (this.transform.position.z < -1.3 && this.transform.position.x > -1.25 && this.transform.position.x < 0.25)
        {
            currentEnclave = Enclave.Enclave2;
        }

        else if (this.transform.position.z < -1.3 && this.transform.position.x > -4 && this.transform.position.x < -2.25)
        {
            currentEnclave = Enclave.Enclave1;
        }

        // [ Upper Side ] 

        else if (this.transform.position.z > 4.6 && this.transform.position.x > -3.7 && this.transform.position.x < -2.5)
        {
            currentEnclave = Enclave.Enclave6;
        }

        else if (this.transform.position.z > 4.6 && this.transform.position.x > -1.1 && this.transform.position.x < 0.16)
        {
            currentEnclave = Enclave.Enclave7;
        }

        else if (this.transform.position.z > 4.6 && this.transform.position.x > 1.48 && this.transform.position.x < 2.65)
        {
            currentEnclave = Enclave.Enclave8;
        }

        else if (this.transform.position.z > 4.6 && this.transform.position.x > 4.20 && this.transform.position.x < 5.35) //BLAZE IT UWUW OWO
        {
            currentEnclave = Enclave.Enclave9;
        }

        else if (this.transform.position.z > 4.6 && this.transform.position.x > 6.8 && this.transform.position.x < 8)
        {
            currentEnclave = Enclave.Enclave10;
        }
        else { currentEnclave = Enclave.Lost; } // meaning its in the middle section 
        
    }


    // Recalculate 
    public void Recalculate()
    {
        //If right, then closest number on the right thats not taken, 1 to 5 if all taken go to 6 to 10 
        Nv.ResetPath();
        if (currentEnclave == Enclave.Enclave6 || currentEnclave == Enclave.Enclave7 || currentEnclave == Enclave.Enclave8 || currentEnclave == Enclave.Enclave9 || currentEnclave == Enclave.Enclave10)
        {
            for (int i = 0; i <= WS.ItemsList.Count; i++)
            {
                if(WS.ItemsList[i].transform.name == "Pickable6" || WS.ItemsList[i].transform.name == "Pickable7" || WS.ItemsList[i].transform.name == "Pickable8" || WS.ItemsList[i].transform.name == "Pickable9" || WS.ItemsList[i].transform.name == "Pickable10")
                {
                    ItemDestinations.Push(WS.ItemsList[i].transform);
                }
            }
        }
    }

    // Be able to move to the nearest enclave if that is mpossible 
    public Enclave movetoneareastEnclave()
    {
        // get the previous distination 
        //if (ItemDestinations.Count > 0) { PreviousDestination = ItemDestinations.Peek(); }
         
        if(currentEnclave == Enclave.Enclave1){ ItemDestinations.Push(Destination6); }
        if (currentEnclave == Enclave.Enclave2) { ItemDestinations.Push(Destination1); }
        if (currentEnclave == Enclave.Enclave3) { ItemDestinations.Push(Destination2); }
        if (currentEnclave == Enclave.Enclave4) { ItemDestinations.Push(Destination3); }
        if (currentEnclave == Enclave.Enclave5) { ItemDestinations.Push(Destination4); }
        if (currentEnclave == Enclave.Enclave6) { ItemDestinations.Push(Destination7); }
        if (currentEnclave == Enclave.Enclave7) { ItemDestinations.Push(Destination8); }
        if (currentEnclave == Enclave.Enclave8) { ItemDestinations.Push(Destination9); }
        if (currentEnclave == Enclave.Enclave9) { ItemDestinations.Push(Destination10); }
        if (currentEnclave == Enclave.Enclave10) { ItemDestinations.Push(Destination5); }
        

        MoveToNext();

        return Enclave.Lost;
    }

    // Change Planrunners plans to compondmovements
    void PlanRunner()
    {
        AiActions S = AiActions.StepBack; // do nothing
        if (Tasks.Count > 0) {
            S = Tasks.Peek();
            Tasks.Pop();
            print(S);
        }

        // Make the change to remain idle to go back to previous
        switch (S)
        {
            case AiActions.MoveForward:
                resumeMovement();
                moveForward();
                break;
            case AiActions.Recalculate:
                break;
            case AiActions.StayIdle:
                doStayIdle();
                remainIdle();
                break;
            case AiActions.StepBack:
                break;
            case AiActions.Teleport:
                TeleportThenSet();
                break;
        }
    }

    void printPlans()
    {
        if (Tasks.Count > 0)
        {
            AiActions S = Tasks.Peek();
            Tasks.Pop();
            print(S);
        }

    }


    // Stay idle does two things, it moves back to the previous position and stays idle there or it stays idle there
    void doStayIdle()
    {
        if (!hasbeenreset)
        {
            Nv.ResetPath();
            hasbeenreset = true;
        }
        

        if (previousEnclave == Enclave.Enclave1)
        {
            Nv.SetDestination(Destination1.transform.position);
        }


        if (previousEnclave == Enclave.Enclave2)
        {
            Nv.SetDestination(Destination2.transform.position);
        }

        if (previousEnclave == Enclave.Enclave3)
        {
            Nv.SetDestination(Destination3.transform.position);
        }


        if (previousEnclave == Enclave.Enclave4)
        {
            Nv.SetDestination(Destination4.transform.position);
        }


        if (previousEnclave == Enclave.Enclave5)
        {
            Nv.SetDestination(Destination5.transform.position);
        }

        if (previousEnclave == Enclave.Enclave6)
        {
            Nv.SetDestination(Destination6.transform.position);
        }

        if (previousEnclave == Enclave.Enclave7)
        {
            Nv.SetDestination(Destination7.transform.position);
        }

        if (previousEnclave == Enclave.Enclave8)
        {
            Nv.SetDestination(Destination8.transform.position);
        }

        if (previousEnclave == Enclave.Enclave9)
        {
            Nv.SetDestination(Destination9.transform.position);
        }

        if (previousEnclave == Enclave.Enclave10)
        {
            Nv.SetDestination(Destination10.transform.position);
        }

        //remainIdle();
    }

    // Moveforward is a simple moveforward
    void moveForward()
    {
        movetoneareastEnclave();
    }

    // Sensor that tells the agent when its safe to come out of the closet
    // Assumption here is it goes back to the enclave 
    void SafeAgainSensor()
    {
        // If im going left
        if (WS.Enemy3_inRange)
        {
            if (currentEnclave == Enclave.Enclave1) { if (E1_FOV.GetComponent<BoxCollider>().bounds.min.x > -2.39 || E1_FOV.GetComponent<BoxCollider>().bounds.max.x < -3.9) { WS.Enemy3_inRange = false; } }
            if (currentEnclave == Enclave.Enclave2) { if (E1_FOV.GetComponent<BoxCollider>().bounds.min.x > 0.3 || E1_FOV.GetComponent<BoxCollider>().bounds.max.x < -1.15) { WS.Enemy3_inRange = false; } }
            if (currentEnclave == Enclave.Enclave3) { if (E1_FOV.GetComponent<BoxCollider>().bounds.min.x > 2.8 || E1_FOV.GetComponent<BoxCollider>().bounds.max.x < 1.45) { WS.Enemy3_inRange = false; } }
            if (currentEnclave == Enclave.Enclave4) { if (E1_FOV.GetComponent<BoxCollider>().bounds.min.x > 5.51 || E1_FOV.GetComponent<BoxCollider>().bounds.max.x < 4.2) { WS.Enemy3_inRange = false; } }
            if (currentEnclave == Enclave.Enclave5) { if (E1_FOV.GetComponent<BoxCollider>().bounds.min.x > 7.4 || E1_FOV.GetComponent<BoxCollider>().bounds.max.x < 6.60) { WS.Enemy3_inRange = false; } }
            // Change these to enemy 1s
        }


        if (WS.Enemy1_inRange)
        {
            if (currentEnclave == Enclave.Enclave6) { if (E2_FOV.GetComponent<BoxCollider>().bounds.min.x > -2.39 || E2_FOV.GetComponent<BoxCollider>().bounds.max.x < -3.9) { WS.Enemy1_inRange = false; } }
            if (currentEnclave == Enclave.Enclave7) { if (E2_FOV.GetComponent<BoxCollider>().bounds.min.x > 0.3 || E2_FOV.GetComponent<BoxCollider>().bounds.max.x < -1.15) { WS.Enemy1_inRange = false; } }
            if (currentEnclave == Enclave.Enclave8) { if (E2_FOV.GetComponent<BoxCollider>().bounds.min.x > 2.8 || E2_FOV.GetComponent<BoxCollider>().bounds.max.x < 1.45) { WS.Enemy1_inRange = false; } }
            if (currentEnclave == Enclave.Enclave9) { if (E2_FOV.GetComponent<BoxCollider>().bounds.min.x > 5.51 || E2_FOV.GetComponent<BoxCollider>().bounds.max.x < 4.2) { WS.Enemy1_inRange = false; } }
            if (currentEnclave == Enclave.Enclave10) { if (E2_FOV.GetComponent<BoxCollider>().bounds.min.x > 8 || E2_FOV.GetComponent<BoxCollider>().bounds.max.x < 6.87) { WS.Enemy1_inRange = false; } }
        }

        // Enemy1
    }

    // Detects if my enemy is coming towards my way. 
    void InComingSensor()
    {
        if(currentDirection == direction.Left && Enemy.GetComponent<EnemyMain>().direction == EnemyMain.MovementState.Right && this.transform.position.z<1.8)
        {
            WS.Enemy3_InComing = true;
        }

        if (currentDirection == direction.Right && Enemy.GetComponent<EnemyMain>().direction == EnemyMain.MovementState.Left && this.transform.position.z < 1.8)
        {
            WS.Enemy3_InComing = false;
        }

        if (currentDirection == direction.Right && Enemy2.GetComponent<EnemyMain>().direction == EnemyMain.MovementState.Left && this.transform.position.z > 1.8)
        {
            WS.Enemy1_InComing = true;
        }

        if (currentDirection == direction.Left && Enemy2.GetComponent<EnemyMain>().direction == EnemyMain.MovementState.Right && this.transform.position.z > 1.8)
        {
            WS.Enemy1_InComing = false;
        }
    }

    void distanceSensor()
    {
        WS.distBWagents = this.GetComponent<BoxCollider>().bounds.SqrDistance(Player.transform.position);
    }
}

