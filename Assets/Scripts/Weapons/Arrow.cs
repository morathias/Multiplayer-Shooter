using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Arrow : NetworkBehaviour {

    float _lifeTime = 3f;

	void Update () {
        _lifeTime -= Time.deltaTime;

        if (_lifeTime <= 0)
            Destroy(gameObject);

        transform.Translate(transform.forward  * 10f * Time.deltaTime, Space.World);
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
            Destroy(gameObject);
    }
}
