using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Zebra : NetworkBehaviour {
    float _life = 50f;
    public GameObject meat;

    [SyncVar(hook = "updateLifeBar")]
    float _currentLife;

    Image _lifeBar;

    Vector3 targetPosition;

    public float moveSpeed = 1f;
    float _currentSpeed;

    bool _isUnderAttack = false;
    float _underAttackTimer = 2f;

    private void Awake()
    {
        _lifeBar = transform.GetChild(1).GetChild(1).GetComponent<Image>();
    }
	
	void Start () {
        _currentLife = _life;
        _currentSpeed = moveSpeed;
        targetPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
	}
	
	void Update () {
        Vector3 finalPos = Vector3.MoveTowards(transform.position, targetPosition, _currentSpeed * Time.deltaTime);
        transform.LookAt(targetPosition);
        transform.position = finalPos;

        if (_isUnderAttack) {
            _currentSpeed = 6f;
            _underAttackTimer -= Time.deltaTime;

            if (_underAttackTimer <= 0) {
                _underAttackTimer = 2f;
                _isUnderAttack = false;
                _currentSpeed = moveSpeed;
            }
        }

        if (transform.position == targetPosition) {
            targetPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
        }
	}

    [Command]
    void CmdDropMeat()
    {
        NetworkServer.Spawn(Instantiate(meat, transform.position, transform.rotation));
    }

    public void takeDamage(float damage)
    {
        if (!isServer)
            return;

        _currentLife -= damage;

        if (_currentLife <= 0)
        {
            CmdDropMeat();
            ZebraSpawner.removeZebra(this.gameObject);
            Destroy(gameObject);
        }
    }

    void updateLifeBar(float currentLife)
    {
        _lifeBar.fillAmount = currentLife / _life;
    }

    public void underAttack() {
        _isUnderAttack = true;
    }

    private void OnCollisionEnter(Collision collision) {
        targetPosition = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
    }
}
