using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FruitSpawner : NetworkBehaviour {
    public GameObject fruit;

    public float radius = 1f;
    float _spawnTime = 4f;
	// Use this for initialization
	void Update () {
        _spawnTime -= Time.deltaTime;

        if (_spawnTime <= 0) {
            CmdDropFruit();
            _spawnTime = 4f;
        }
    }

    [Command]
    void CmdDropFruit() {
        NetworkServer.Spawn(Instantiate(fruit,
                            new Vector3(transform.position.x + Random.Range(-radius, radius), transform.position.y, transform.position.z + Random.Range(-radius, radius)), 
                            Quaternion.identity,
                            transform));
    }

    void OnDrawGizmos() {
        Color gizmosColor = new Color();
        gizmosColor.a = 0.1f;
        Gizmos.color = gizmosColor;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
