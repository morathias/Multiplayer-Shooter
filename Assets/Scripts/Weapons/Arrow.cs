using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Arrow : NetworkBehaviour {

    float _lifeTime = 3f;

    float _damage = 10f;

    bool _collided = false;
    Rigidbody _rigidBody;
    Collider _collider;

    void Start() {
        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

	void Update () {
        _lifeTime -= Time.deltaTime;

        if (_lifeTime <= 0)
            Destroy(gameObject);

        if(!_collided)
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

        if (collision.transform.tag == "Zebra")
        {
            Zebra zebra = collision.gameObject.GetComponent<Zebra>();
            zebra.takeDamage(_damage);
            zebra.underAttack();
        }

        _collided = true;
        transform.SetParent(collision.gameObject.transform, true);
        Destroy(_rigidBody);
        _collider.enabled = false;

        //Destroy(gameObject);
    }
}
