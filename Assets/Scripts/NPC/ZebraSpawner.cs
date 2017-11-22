using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ZebraSpawner : NetworkBehaviour {
    public GameObject zebra;
    public int maxAmount = 3;

    static List<GameObject> _currentZebras = new List<GameObject>();

    const float SPAWN_TIME = 30f;
    float _spawnTimer = 0f;

    public override void OnStartServer()
    {
        for (int i = 0; i < maxAmount; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-8.0f, 8.0f),
                0.0f,
                Random.Range(-8.0f, 8.0f));

            Quaternion spawnRotation = Quaternion.Euler(
                0.0f,
                Random.Range(0, 180),
                0.0f);

            GameObject enemy = Instantiate(zebra, spawnPosition, spawnRotation);
            _currentZebras.Add(enemy);
            NetworkServer.Spawn(enemy);
        }
    }

    void Update() {
        Debug.Log(_currentZebras.Count);
        if (_currentZebras.Count < maxAmount) {
            Debug.Log("spawning");
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer >= SPAWN_TIME)
            {
                CmdSpawnZebra();
                _spawnTimer = 0f;
            }
        }
    }

    [Command]
    void CmdSpawnZebra() {
        Vector3 spawnPosition = new Vector3(
                Random.Range(-8.0f, 8.0f),
                0.0f,
                Random.Range(-8.0f, 8.0f));

        Quaternion spawnRotation = Quaternion.Euler(
            0.0f,
            Random.Range(0, 180),
            0.0f);

        GameObject enemy = Instantiate(zebra, spawnPosition, spawnRotation);
        _currentZebras.Add(enemy);
        NetworkServer.Spawn(enemy);
    }

    public static void removeZebra(GameObject zebraToRemove) {
        _currentZebras.Remove(zebraToRemove);
    }
}
