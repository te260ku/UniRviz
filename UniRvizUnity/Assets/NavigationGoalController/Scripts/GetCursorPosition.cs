using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetCursorPosition : MonoBehaviour
{
    RaycastHit hit;

    public Vector3 GetMousePosition() {
        var _cursorPosition = Input.mousePosition;
        _cursorPosition.z = 10f;
        var _cursorPosition3d = Camera.main.ScreenToWorldPoint(_cursorPosition);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            var hitPos = hit.point;
            hitPos.y = 0f;
            return hitPos;
        }

        return new Vector3(0f, -100f, 0f);
    }
}
