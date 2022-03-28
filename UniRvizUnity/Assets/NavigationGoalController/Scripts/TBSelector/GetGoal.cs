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

    public Vector3 non_clickPosition;
    public Vector3 _goalPosition;
    public Vector3 _goalRotation; 
    Quaternion lookRotation;
    Vector3 eulerRotation;
    public GameObject goal;
    public bool goal_status = false;
    bool _isMouseButtonDown;
    [SerializeField] GameObject _arrowObj;
    MeshRenderer _meshRenderer;
    
    void Start()
    {
        non_clickPosition.x = 0;
        non_clickPosition.y = -2;
        non_clickPosition.z = 0;
        _goalPosRotText.text = "";

        _meshRenderer = _arrowObj.GetComponent<MeshRenderer>();
    }


    void Update()
    {
        GetMousePosition();
        _goalPosRotText.text = "Goal" + "\n" + "Pos=" + _goalPosition.ToString() + "\n" + "Rot=" + _goalRotation.ToString();
        bool isValidPosition = 0 < hit.point.x && hit.point.x < 10 && 0 < hit.point.z && hit.point.z < 10;
        if (isValidPosition) {
            GetGoalPosDir();
            Decide_goalPosition();
            
        }
    }

    void GetMousePosition(){
        _cursorPosition = Input.mousePosition;
        _cursorPosition.z = 10.0f;
        _cursorPosition3d = Camera.main.ScreenToWorldPoint(_cursorPosition);
        if (Physics.Raycast(Camera.main.transform.position, (_cursorPosition3d - Camera.main.transform.position), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(Camera.main.transform.position, (_cursorPosition3d - Camera.main.transform.position) * hit.distance, Color.red);
        }

    }

    void GetGoalPosDir(){
        if (Input.GetMouseButtonDown(0))
        {
            _goalPosition = new Vector3(hit.point.x, 0.2f, hit.point.z);

            // transform.position = _goalPosition;

            _arrowObj.transform.position = _goalPosition; 

            _isMouseButtonDown = true;
        }
        
        //Goal Direction
        if (_isMouseButtonDown)
        {
            _meshRenderer.enabled = true;

            var direction = hit.point - transform.position;
            direction.y = 0;
            lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            // transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);

            _arrowObj.transform.rotation = Quaternion.Lerp(_arrowObj.transform.rotation, lookRotation, 0.1f);

            _goalRotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f).eulerAngles;
        }
        else
        {
            // transform.position = non_clickPosition;
            _meshRenderer.enabled = false;
        }
    }

    
    void Decide_goalPosition(){
        if (Input.GetMouseButtonUp(0))
        {
            GameObject obj = Instantiate(goal, _goalPosition, Quaternion.Lerp(transform.rotation, lookRotation, 0.1f));
            obj.GetComponent<Renderer> ().material.color = goal.GetComponent<Renderer> ().material.color;
            goal_status = true;

            _isMouseButtonDown = false;
        }
    }

}