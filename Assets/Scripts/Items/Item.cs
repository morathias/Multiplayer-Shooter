using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Item : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(5, 0, 0);
        Debug.Log("item created");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") {
            collision.gameObject.GetComponent<Player>().heal(20f);
        }

        Destroy(gameObject);
    }
}
