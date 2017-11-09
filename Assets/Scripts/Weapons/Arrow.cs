using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Arrow : NetworkBehaviour {

    float _lifeTime = 3f;

    float _damage = 10f;

	void Update () {
        _lifeTime -= Time.deltaTime;

        if (_lifeTime <= 0)
            Destroy(gameObject);

        transform.Translate(transform.forward  * 10f * Time.deltaTime, Space.World);
	}

    public float getDamage()
    {
        return _damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
            collision.gameObject.GetComponent<Player>().takeDamage(_damage);

        Destroy(gameObject);
    }
}
