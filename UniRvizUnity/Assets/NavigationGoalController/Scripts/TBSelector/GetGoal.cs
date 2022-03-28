using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetGoal : MonoBehaviour
{
    public Text text2;
    //mouse
    Vector3 cursorPosition;
    Vector3 cursorPosition3d;
    RaycastHit hit;

    //goal
    public Vector3 non_clickPosition;
    public Vector3 goalPosition;
    public Vector3 goalRotation; 
    Quaternion lookRotation;
    Vector3 eulerRotation;
    public GameObject goal;
    public bool goal_status = false;
    

    // Start is called before the first frame update
    void Start()
    {
        non_clickPosition.x = 0;
        non_clickPosition.y = -2;
        non_clickPosition.z = 0;
        text2.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePosition();
        Text();
        if(hit.point.x >0 && hit.point.x<10){
            if(hit.point.z>0 && hit.point.z<10){
                GetGoalPosDir();
                DecideGoalPosition();
            }
        }
    }

    void GetMousePosition(){
        // Mouse Position
        cursorPosition = Input.mousePosition; // ��ʏ�̃J�[�\���̈ʒu
        cursorPosition.z = 10.0f; // z���W�ɓK���Ȓl������
        cursorPosition3d = Camera.main.ScreenToWorldPoint(cursorPosition); // 3D�̍��W�ɂȂ���
        if (Physics.Raycast(Camera.main.transform.position, (cursorPosition3d - Camera.main.transform.position), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(Camera.main.transform.position, (cursorPosition3d - Camera.main.transform.position) * hit.distance, Color.red);
        }

    }

    void GetGoalPosDir(){
        //Goal Position
        if (Input.GetMouseButtonDown(0))
        {
            //arrow
            goalPosition.x = hit.point.x;
            goalPosition.y = 0.2f;
            goalPosition.z = hit.point.z;
            transform.position = goalPosition;
        }
        
        //Goal Direction
        if (Input.GetMouseButton(0))
        {
            
            var direction = hit.point - transform.position;
            direction.y = 0;
            lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
            eulerRotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f).eulerAngles;
            goalRotation = eulerRotation;
        }
        else
        {
            transform.position = non_clickPosition;
        }
    }

    void Text(){
        //text
        text2.text = "Goal" + "\n" + "Pos=" + goalPosition.ToString() + "\n" + "Rot=" + goalRotation.ToString();
    }

    void DecideGoalPosition(){
        if (Input.GetMouseButtonUp(0))
        {
            //goal prehub
            GameObject obj =Instantiate(goal, goalPosition, Quaternion.Lerp(transform.rotation, lookRotation, 0.1f));
            obj.GetComponent<Renderer> ().material.color = goal.GetComponent<Renderer> ().material.color;
            goal_status = true;
        }

    }

}