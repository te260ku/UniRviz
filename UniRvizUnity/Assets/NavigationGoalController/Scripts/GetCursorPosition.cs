using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCursorPosition : MonoBehaviour
{
    [SerializeField] Text _cursorPositionText;
    RaycastHit hit;
    Vector3 _cursorPosition;
    Vector3 _cursorPosition3d;

    Vector3 GetMousePosition() {
        _cursorPosition = Input.mousePosition;
        _cursorPosition.z = 10.0f;
        _cursorPosition3d = Camera.main.ScreenToWorldPoint(_cursorPosition);
        if (Physics.Raycast(Camera.main.transform.position, (_cursorPosition3d - Camera.main.transform.position), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(Camera.main.transform.position, (_cursorPosition3d - Camera.main.transform.position) * hit.distance, Color.red);
            _cursorPositionText.text = "Mouse" + "\n" + hit.point.ToString();

            return hit.point;
        }

        return Vector3.zero;
    }
}
