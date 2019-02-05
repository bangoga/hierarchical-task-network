using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldState : MonoBehaviour {
    public bool Enemy1_inRange;
    public bool Enemy3_inRange;

    public bool Enemy1_InComing;
    public bool Enemy3_InComing;

    public int ItemsLeft;
    public Enclave Agent1_Predictive_Position;
    public Enclave Agent2_Predictive_Position;

    public float Enemy1_Predictive_Position;
    public float Enemy3_Predictive_Position;

    public float distBWagents = 10000; // the distance currently between the two agents 

    public string whatToTeleport;
    public int AiScore;
    public int PlayerScore;

    public AiState Ai1State;
    public GameObject[] items;
    public List<GameObject> ItemsList = new List<GameObject>();
    private void Start()
    {
        items = GameObject.FindGameObjectsWithTag("Pickable");
        ItemsList.AddRange(items);
    }
}
