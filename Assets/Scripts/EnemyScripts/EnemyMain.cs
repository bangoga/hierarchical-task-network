using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : MonoBehaviour {
    public GameObject FieldOfView;
    public enum MovementState {Left,Right,DeSpawn}
    public MovementState direction;
    public bool inBetween;

    public GameObject thisEnemy; // Enemy + FieldOfView
	// Use this for initialization
	void Start () {
        thisEnemy = this.transform.parent.gameObject;
        direction = MovementState.Left; // Initial Direction of all is left
        print(thisEnemy.transform.name);

    }
	
	// Update is called once per frame
	void Update () {
        // Always checking for collision
        EnemyMovement(direction);
        fieldOfViewDirection(direction);
    }

    // Regularized Movement of Enemy
    public void EnemyMovement(MovementState currentDir)
    {
        switch (currentDir)
        {
            // direction left
            case MovementState.Left:
                thisEnemy.transform.localPosition = new Vector3(thisEnemy.transform.localPosition.x - 2 * Time.deltaTime, thisEnemy.transform.localPosition.y, thisEnemy.transform.localPosition.z);
                break;
            // direction right
            case MovementState.Right:
                thisEnemy.transform.localPosition = new Vector3(thisEnemy.transform.localPosition.x + 2 * Time.deltaTime, thisEnemy.transform.localPosition.y, thisEnemy.transform.localPosition.z);
                break;
            case MovementState.DeSpawn:
                break;

        }
    }

    // move field of view 
    public void fieldOfViewDirection(MovementState currentDir)
    {
        switch (currentDir)
        {
            // direction left
            case MovementState.Left:
                FieldOfView.transform.localPosition = new Vector3(-1.1f, this.transform.position.y, this.transform.localPosition.z);
                break;
            // direction right
            case MovementState.Right:
                FieldOfView.transform.localPosition = new Vector3(1.1f, this.transform.position.y, this.transform.localPosition.z);
                break;
            case MovementState.DeSpawn:
                break;

        }
    }
    // Change of Direction - Happens on Collision
    public void ChangeDirection(string dir)
    {
        print("hel");
        if (dir == "Left")
        {
            direction = MovementState.Left;
            
        }
        if (dir == "Right")
        {
            direction = MovementState.Right;
        }
    }

    // Hardcode the respawn, and respawn on both sides 
    public void reSpawn()
    {
        if (direction == MovementState.Left)
        {
            thisEnemy.transform.localPosition = new Vector3(32.5f, thisEnemy.transform.localPosition.y, 0);
        }

        if (direction == MovementState.Right)
        {
            thisEnemy.transform.localPosition = new Vector3(-3.5f, thisEnemy.transform.localPosition.y, 0);
        }
    }

    // GameScript --> ChekcFieldOfVision --> 
    public void checkFieldOfVision()
    {

    }

    public Vector3 getPosition()
    {
        return this.transform.position;
    }


}
