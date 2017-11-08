using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    public GameObject arrow;

    public float rotateSpeed;
    public float movingSpeed;

    float _life = 100f;
    float _currentLife;
    float _angle;

    Vector3 _mousePosition;
    Vector3 _position;

    Animator _animations;

    void Start() {
        _animations = transform.GetChild(0).GetComponent<Animator>();
    }

	void Update () {
        if (!isLocalPlayer)
            return;

        _currentLife = _life;

        rotate();

        Vector3 translate = new Vector3(Input.GetAxis("Horizontal"),
                                        0,
                                        Input.GetAxis("Vertical"));

        if (translate.magnitude > 0)
            _animations.Play("Armature|running");
        else
            _animations.Play("Armature|iddle");

        transform.Translate(translate * movingSpeed * Time.deltaTime, Space.World);

        if (Input.GetButtonDown("Fire1"))
            Cmdfire();
	}

    [Command]
    void Cmdfire() {
        NetworkServer.Spawn(Instantiate(arrow, transform.position + (transform.forward * 0.5f + new Vector3(0, 0.5f, 0)), transform.rotation));
    }

    public override void OnStartLocalPlayer(){
        //transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void applyDamage(float damageToApply) {
        _currentLife -= damageToApply;
    }

    void rotate()
    {
        _mousePosition = Input.mousePosition;    //agarro la posicion del mouse
        _mousePosition.z = 13f;  //distancia de la camara al prota

        _position = Camera.main.WorldToScreenPoint(transform.position);    //convierto la posicion del prota a screen

        _mousePosition.x -= _position.x;    //saco la direccion
        _mousePosition.y -= _position.y;    //_|

        _angle = Mathf.Atan2(_mousePosition.x, _mousePosition.y) * Mathf.Rad2Deg;    //saco el angulo de ese vector
        transform.rotation = Quaternion.Euler(new Vector3(0, _angle, 0));      //lo roto en eje y
    }
}
