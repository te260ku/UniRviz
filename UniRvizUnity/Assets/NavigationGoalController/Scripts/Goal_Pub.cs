using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;


public class Goal_Pub : MonoBehaviour
{
    ROSConnection ros;

    //PoseStampedMsg
    //header
    HeaderMsg header = new HeaderMsg();
    PoseMsg pose = new PoseMsg();

    //TB Selector
    [SerializeField] private Dropdown TBSelector;
    public GetGoal getGoal;
    Vector3 goalPosition;
    Vector3 goalRotation;
    public GetGoal getgoal;

    

    
    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.instance;
        ros.RegisterPublisher<PoseStampedMsg>("TB1/pose");
        ros.RegisterPublisher<PoseStampedMsg>("TB2/pose");
    }

    
    void Update()
    {
        Move_a_TB();
        
    }

    void Move_a_TB(){
        //receive Goal Position
        goalPosition = getGoal.goalPosition;
        goalRotation = getGoal.goalRotation;

        //header
        header.stamp.sec = 0;
        header.stamp.nanosec = 0;
        header.frame_id = "map";

        //pose
        pose.position.x = goalPosition.z;
        pose.position.y = goalPosition.x;
        pose.position.z = 0;
        pose.orientation.x = 0;
        pose.orientation.y = 0;
        pose.orientation.z = goalRotation.y;
        pose.orientation.w = 0;

        //選択するTBによってtopic名を変更する
        if(getgoal.goal_status ==true){

            PoseStampedMsg Pose = new PoseStampedMsg(
                    header,
                    pose
                    ); 
            //Move_a_TB();
            //Move_Multiple_TB();

            switch(TBSelector.value){
               
                case 0: //赤TB
                    ros.Send("TB1/pose", Pose);
                    getgoal.goal_status = false;
                    print(Pose.pose);
                    break;
                
                case 1: //青TB
                    ros.Send("TB2/pose", Pose);
                    getgoal.goal_status = false;
                    print(Pose);
                    break;
            }
        }
    }

    


}
