using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetGoal : MonoBehaviour
{
    public Text _goalPosRotText;
    //mouse
    Vector3 _cursorPosition;
    Vector3 _cursorPosition3d;
    RaycastHit hit;
    public Vector3 _goalPosition;
    public Vector3 _goalRotation; 
    Quaternion lookRotation;
    public GameObject _goalPrefab;
    public bool goal_status = false;
    bool _isMouseButtonDown;
    [SerializeField] GameObject _arrowObj;
    [SerializeField] Text _cursorPositionText;
    MeshRenderer _meshRenderer;
    
    void Start()
    {
        _goalPosRotText.text = "";
        _arrowObj.SetActive(false);
    }


    void Update()
    {
        GetMousePosition();
        _goalPosRotText.text = "Goal" + "\n" + "Pos=" + _goalPosition.ToString() + "\n" + "Rot=" + _goalRotation.ToString();
        bool isValidPosition = 0 < hit.point.x && hit.point.x < 10 && 0 < hit.point.z && hit.point.z < 10;
        if (isValidPosition) {
            GetGoalPosDir();
        }
    }

    void GetMousePosition(){
        _cursorPosition = Input.mousePosition;
        _cursorPosition.z = 10.0f;
        _cursorPosition3d = Camera.main.ScreenToWorldPoint(_cursorPosition);
        if (Physics.Raycast(Camera.main.transform.position, (_cursorPosition3d - Camera.main.transform.position), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(Camera.main.transform.position, (_cursorPosition3d - Camera.main.transform.position) * hit.distance, Color.red);
            _cursorPositionText.text = "Mouse"+"\n" + hit.point.ToString();
        }

    }

    void GetGoalPosDir(){
        if (Input.GetMouseButtonDown(0))
        {
            _goalPosition = new Vector3(hit.point.x, 0.2f, hit.point.z);

            _arrowObj.transform.position = _goalPosition; 

            _isMouseButtonDown = true;
        }
        
        //Goal Direction
        if (_isMouseButtonDown)
        {
            _arrowObj.SetActive(true);

            var direction = hit.point - _arrowObj.transform.position;
            direction.y = 0;
            lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            

            _arrowObj.transform.rotation = Quaternion.Lerp(_arrowObj.transform.rotation, lookRotation, 0.1f);

            _goalRotation = Quaternion.Lerp(_arrowObj.transform.rotation, lookRotation, 0.1f).eulerAngles;
        }
        else
        {
            _arrowObj.SetActive(false);
        }


        if (Input.GetMouseButtonUp(0))
        {
            GameObject obj = Instantiate(_goalPrefab, _goalPosition, Quaternion.Euler(_goalRotation));
            // obj.GetComponent<Renderer> ().material.color = goal.GetComponent<Renderer>().material.color;
            goal_status = true;

            _isMouseButtonDown = false;
        }
    }

}