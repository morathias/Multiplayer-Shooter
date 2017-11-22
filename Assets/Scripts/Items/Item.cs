using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Item : NetworkBehaviour {
    Rigidbody _rigidBody;

    public int amount;
    public float healAmount;
	
	void Start () {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.AddTorque(transform.position);
        Debug.Log("item created");
	}
	
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") {

            Player player = collision.gameObject.GetComponent<Player>();
            player.heal(healAmount);
            player.gotFood(amount);

            Destroy(gameObject);
        }
    }
}
