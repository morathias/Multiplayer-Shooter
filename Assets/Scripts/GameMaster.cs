using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.UI;

public class GameMaster : NetworkBehaviour {
    public float gameTimeInMinutes = 5f;
    float _currentGameTime = 0;
    float _seconds = 60f;

    Text _winerTxt;
    Text _gameTimeTxt;
	
	void Start () {
        _currentGameTime = gameTimeInMinutes - 1;

        GameObject[] canvas = GameObject.FindGameObjectsWithTag("UI");

        for (int i = 0; i < canvas.Length; i++)
        {
            if (canvas[i].name == "gameTimeTxt")
                _gameTimeTxt = canvas[i].GetComponent<Text>();

            if (canvas[i].name == "winerTxt")
                _winerTxt = canvas[i].GetComponent<Text>();
        }
        _winerTxt.gameObject.SetActive(false);
	}
	
	void Update () {
        if (!isServer)
            return;

        _seconds -= Time.deltaTime;

        if (_seconds <= 0)
        {
            if (_currentGameTime > 0)
                _seconds = 60f;

            _currentGameTime--;
        }

        RpcInGame(_seconds, _currentGameTime);
	}

    [ClientRpc]
    void RpcInGame(float seconds, float minutes) {
        
        _gameTimeTxt.text = _currentGameTime.ToString() + ":" + _seconds.ToString();

        if (minutes <= 0)
        {
            if (seconds <= 0)
            {
                Debug.Log("termino el juego");
                Time.timeScale = 0;

                List<GameObject> players = GameObject.FindGameObjectsWithTag("Player").ToList();

                players = players.OrderByDescending((player) => player.GetComponent<Player>().getScore()).ToList();

                _winerTxt.gameObject.SetActive(true);
                _winerTxt.text = "The best\n score is:\n" + players[0].GetComponent<Player>().getScore();
            }
        }
    }
}
