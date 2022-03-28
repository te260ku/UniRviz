using UnityEngine;
using System.Collections;
using System.Collections.Generic;using System;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class testScript : MonoBehaviour
{

    

    ROSConnection ros;

    //Twist
    Vector3Msg linear = new Vector3Msg(0f, 0f, 0f);
    Vector3Msg angular = new Vector3Msg(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        // start the ROS connection
        ros = ROSConnection.instance;
        ros.RegisterPublisher<TwistMsg>("/cmd_veel");

        
    }

    // Update is called once per frame
    void Update()
    {
        
        linear.x = 3.0f;
        angular.z = 0.0f;

        //Send untiy_odom to turtlebot_control
        TwistMsg Twist = new TwistMsg(
               linear,
               angular
            );

        // Finally send the message to server_endpoint.py running in ROS
        ros.Send("cmd_veel", Twist);
    }
}