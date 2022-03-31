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
    [SerializeField] GameObject _cursorObj;
    [SerializeField] GameObject _goalPrefab;
    [SerializeField] GoalPublisher _goalPublisher;
    [SerializeField] TextMeshProUGUI _cursorPositionText;
    [SerializeField] TextMeshProUGUI _goalPosRotText;
    [SerializeField] GetCursorPosition _getCursorPosition;
    [SerializeField] GameObject _tb3;
    Vector3 _raycastHitPoint;
    [NonSerialized] public Vector3 _goalPosition;
    [NonSerialized] public Vector3 _goalRotation; 
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
        _raycastHitPoint = _getCursorPosition.GetMousePosition();
        _hitPos = _raycastHitPoint;
            
        _hitPos.x -= _mapReader._originPos.x;
        _hitPos.y -= _mapReader._originPos.y;
        _hitPos.z -= _mapReader._originPos.z;
        _hitPos.x *= -1f;

        _cursorObj.transform.position = _raycastHitPoint;

        _cursorPositionText.text = "Cursor: " + _hitPos.ToString();
        
        GetGoalPosDir();
        
        
    }


    void GetGoalPosDir() {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (_isMouseButtonDown) return;

            _goalPosition = new Vector3(_hitPos.x, 0.2f, _hitPos.z);

            _arrowObj.transform.position = _raycastHitPoint; 

            _isMouseButtonDown = true;
        }
        
        //Goal Direction
        if (_isMouseButtonDown)
        {
            _arrowObj.SetActive(true);

            var direction = _raycastHitPoint - _arrowObj.transform.position;
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

            _goalPosRotText.text = "Goal: {" + 
                                "Pos: " + _goalPosition.ToString() + 
                                ", " + "Rot: " + _goalRotation.ToString() + "}";

            GameObject obj = Instantiate(_goalPrefab, _raycastHitPoint, Quaternion.Euler(_goalRotation));
            // obj.GetComponent<Renderer>().material.color = goal.GetComponent<Renderer>().material.color;
            _isMouseButtonDown = false;
        }
    }

}