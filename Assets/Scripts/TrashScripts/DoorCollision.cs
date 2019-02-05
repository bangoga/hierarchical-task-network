using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollision : MonoBehaviour {
    public GameObject[] enemies;
    // Use this for initialization
    void Start () {
     enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    void OnCollisionEnter(Collision col)
    {
        Destroy(col.gameObject);
    }
}
