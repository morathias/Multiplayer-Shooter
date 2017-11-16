using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    public GameObject arrow;

    public float movingSpeed;
    float _currentMovingSpeed;

    int _level = 0;
    float _experience = 0;
    float _life = 100f;

    int _fruitCount = 0;
    Text fruitCountTxt;
    int _meatCount = 0;
    Text meatCountTxt;

    [SyncVar(hook = "updateLifeBar")]
    float _currentLife;

    float _angle;

    Vector3 _mousePosition;
    Vector3 _screenPosition;
    Vector3 _spawnPosition;
    Vector3 _translate = Vector3.zero;

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
        _currentMovingSpeed = movingSpeed;

        GameObject[] canvas = GameObject.FindGameObjectsWithTag("UI");

        for (int i = 0; i < canvas.Length; i++)
        {
            if (canvas[i].name == "fruit_count_txt")
                fruitCountTxt = canvas[i].GetComponent<Text>();
            if (canvas[i].name == "meat_count_txt")
                meatCountTxt = canvas[i].GetComponent<Text>();
        }

        _spawnPosition = transform.position;

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
        move();
        crouch();
        
        updateAnimations();

        if (Input.GetMouseButtonDown(0))
            Cmdfire();
	}

    [Command]
    void Cmdfire()
    {
        NetworkServer.Spawn(Instantiate(arrow, transform.position + (transform.forward * 0.5f + new Vector3(0, 0.5f, 0)), transform.rotation));
    }

    void crouch() {
        if (Input.GetKey(KeyCode.LeftControl))
            _currentMovingSpeed = movingSpeed * 0.5f;
        else
            _currentMovingSpeed = movingSpeed;
    }

    void move() {
        _translate = new Vector3(Input.GetAxis("Horizontal"),
                                 0,
                                 Input.GetAxis("Vertical"));

        transform.Translate(_translate * _currentMovingSpeed * Time.deltaTime, Space.World);
    }

    void updateAnimations() {
        if (_translate.magnitude > 0)
            _animations.Play("Armature|running");
        else
            _animations.Play("Armature|iddle");
        _animations.SetFloat("speed", _currentMovingSpeed * 0.5f);
    }

    public override void OnStartLocalPlayer()
    {
        transform.GetChild(1).gameObject.SetActive(true);
    }

    void rotate()
    {
        _mousePosition = Input.mousePosition;
        _mousePosition.z = 13f;

        _screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        _mousePosition.x -= _screenPosition.x;
        _mousePosition.y -= _screenPosition.y;

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

    public void heal(float health) {
        if (!isServer)
            return;

        _currentLife += health;

        if (_currentLife >= _life)
            _currentLife = _life;

        Debug.Log("item healed");
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
