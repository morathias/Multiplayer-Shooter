using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorUtil : MonoBehaviour {

    void Update()
    {
        Cursor.visible = false;

        transform.position = Input.mousePosition;

        Vector2 mouseDirection = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

        Vector2 rotation = screenCenter - mouseDirection;

        float angle = Mathf.Atan2(rotation.x, -rotation.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
