using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    public GameObject arrow;

    public float rotateSpeed;
    public float movingSpeed;

    int _level = 0;
    float _experience = 0;
    float _life = 100f;

    [SyncVar(hook = "updateLifeBar")]
    float _currentLife;

    float _angle;

    Vector3 _mousePosition;
    Vector3 _position;
    Vector3 _spawnPosition;

    Animator _animations;

    Image _lifeBar;

    private void Awake()
    {
        _lifeBar = transform.GetChild(2).GetChild(1).GetComponent<Image>();
    }

    void Start()
    {
        _animations = transform.GetChild(0).GetComponent<Animator>();
        _currentLife = _life;

        _spawnPosition = transform.position;
        
        if (_lifeBar)
            Debug.Log("bar found");

        if (isLocalPlayer)
            Camera.main.GetComponent<CameraFollow>().setTarget(this.transform);
        else
            transform.GetChild(1).gameObject.SetActive(false);
    }

	void Update ()
    {
        if (!isLocalPlayer)
            return;

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
    void Cmdfire()
    {
        NetworkServer.Spawn(Instantiate(arrow, transform.position + (transform.forward * 0.5f + new Vector3(0, 0.5f, 0)), transform.rotation));
    }

    public override void OnStartLocalPlayer()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }

    void rotate()
    {
        _mousePosition = Input.mousePosition;
        _mousePosition.z = 13f;

        _position = Camera.main.WorldToScreenPoint(transform.position);

        _mousePosition.x -= _position.x;
        _mousePosition.y -= _position.y;

        _angle = Mathf.Atan2(_mousePosition.x, _mousePosition.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, _angle, 0));
    }

    public void takeDamage(float damage) {
        if (!isServer)
            return;

        _currentLife -= damage;

        if (_currentLife <= 0)
            RpcRespawn();
    }

    void updateLifeBar(float currentLife) {
        _lifeBar.fillAmount = currentLife / _life;
    }

    [ClientRpc]
    void RpcRespawn() {
        if (isLocalPlayer)
            transform.position = _spawnPosition;

        _currentLife = _life;
    }
}
