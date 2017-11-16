using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
	void Update () {
        transform.LookAt(Camera.main.transform);
        Vector3 finalRot = transform.rotation.eulerAngles;
        finalRot.x = -finalRot.x;
        finalRot.y = 0;
        finalRot.z = 0;

        transform.rotation = Quaternion.Euler(finalRot);
	}
}
