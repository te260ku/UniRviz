using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class GetGoal : MonoBehaviour
{
    [SerializeField] GameObject _arrowObj;
    [SerializeField] GameObject _goalPrefab;
    [SerializeField] GoalPublisher _goalPublisher;
    [SerializeField] TextMeshProUGUI _cursorPositionText;
    [SerializeField] TextMeshProUGUI _goalPosRotText;
    [SerializeField] GetCursorPosition _getCursorPosition;
    [SerializeField] GameObject _tb3;
    Vector3 _cursorPosition;
    Vector3 _cursorPosition3d;
    RaycastHit hit;
    [NonSerialized] public Vector3 _goalPosition;
    [NonSerialized] public Vector3 _goalRotation; 
    [NonSerialized] public bool goal_status = false;
    Quaternion _lookRotation;
    bool _isMouseButtonDown;
    Vector3 _hitPos;
    [SerializeField] MapReader _mapReader;
    
    void Start()
    {
        _arrowObj.SetActive(false);

        _cursorPositionText.text = "Cursor: ";
        _goalPosRotText.text = "Goal: ";
    }


    void Update()
    {
        GetMousePosition();
        
        bool isValidPosition = 0 < hit.point.x && hit.point.x < 10 && 0 < hit.point.z && hit.point.z < 10;
        isValidPosition = true;
        if (isValidPosition) {
            GetGoalPosDir();
            _goalPosRotText.text = "Goal: {" + 
                                    "Pos: " + _goalPosition.ToString() + 
                                    ", " + "Rot: " + _goalRotation.ToString() + "}";
        }

        if (Input.GetKeyDown(KeyCode.I)) {
            _goalPublisher.SendInitialPose(_tb3.transform.position, _tb3.transform.rotation.eulerAngles);
        }
    }

    

    void GetMousePosition() {
        _cursorPosition = Input.mousePosition;
        _cursorPosition.z = 10.0f;
        _cursorPosition3d = Camera.main.ScreenToWorldPoint(_cursorPosition);

        

        if (Physics.Raycast(Camera.main.transform.position, (_cursorPosition3d - Camera.main.transform.position), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(Camera.main.transform.position, (_cursorPosition3d - Camera.main.transform.position) * hit.distance, Color.red);
            

            _hitPos = hit.point;
            
            _hitPos.x -= _mapReader._originPos.x;
            _hitPos.y -= _mapReader._originPos.y;
            _hitPos.z -= _mapReader._originPos.z;
            _hitPos.x *= -1f;

            // _cursorPositionText.text = "Cursor" + "\n" + hit.point.ToString();
            _cursorPositionText.text = "Cursor: " + _hitPos.ToString();
        }

        
        

    }

    void GetGoalPosDir() {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (_isMouseButtonDown) return;

            _goalPosition = new Vector3(_hitPos.x, 0.2f, _hitPos.z);

            _arrowObj.transform.position = hit.point; 

            _isMouseButtonDown = true;
        }
        
        //Goal Direction
        if (_isMouseButtonDown)
        {
            _arrowObj.SetActive(true);

            var direction = hit.point - _arrowObj.transform.position;
            direction.y = 0;
            _lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            

            _arrowObj.transform.rotation = Quaternion.Lerp(_arrowObj.transform.rotation, _lookRotation, 0.1f);

            _goalRotation = Quaternion.Lerp(_arrowObj.transform.rotation, _lookRotation, 0.1f).eulerAngles;
        }
        else
        {
            _arrowObj.SetActive(false);
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (!_isMouseButtonDown) return;
            _goalPublisher.SendGoal(_goalPosition, _goalRotation);

            GameObject obj = Instantiate(_goalPrefab, hit.point, Quaternion.Euler(_goalRotation));
            // obj.GetComponent<Renderer>().material.color = goal.GetComponent<Renderer>().material.color;
            goal_status = true;
            _isMouseButtonDown = false;
        }
    }

}