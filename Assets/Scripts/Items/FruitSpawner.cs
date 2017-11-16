using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FruitSpawner : NetworkBehaviour {
    public GameObject[] fruits;
	// Use this for initialization
	void Start () {
        if (isServer)
        {
            Instantiate(fruits[0], transform.position + new Vector3(1, 0, 1), Quaternion.identity);
            Instantiate(fruits[0], transform.position + new Vector3(-1, 0, 1), Quaternion.identity);
        }
    }
}
