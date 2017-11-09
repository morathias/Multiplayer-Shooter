using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    Transform _player;

    public float smoothness;
    public float height;
    public float distance;

    Vector3 _finalPosition;

    void Start () {
		
	}
	
	void Update () {
        if (!_player)
            return;

        transform.LookAt(_player);

        _finalPosition = new Vector3(_player.position.x, height + _player.position.y, _player.position.z - distance);
        transform.position = Vector3.Lerp(transform.position, _finalPosition, Time.deltaTime * smoothness);
    }

    public void setTarget(Transform player) {
        _player = player;
    }
}
