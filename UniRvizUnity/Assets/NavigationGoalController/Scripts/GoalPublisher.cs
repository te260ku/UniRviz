using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;

public class GoalPublisher : MonoBehaviour
{
    [SerializeField] private Dropdown TBSelector;
    [SerializeField] string[] _topicNames;
    ROSConnection _ros;

    void Start()
    {
        _ros = ROSConnection.GetOrCreateInstance();
        foreach (var topicName in _topicNames)
        {
            _ros.RegisterPublisher<PoseStampedMsg>(topicName);
        }
    }

    public void SendGoal(Vector3 pos, Vector3 rot) {
        var goalPosition = pos;
        var goalRotation = rot;

        PoseMsg pose = new PoseMsg();
        HeaderMsg header = new HeaderMsg();

        // header
        header.stamp.sec = 0;
        header.stamp.nanosec = 0;
        header.frame_id = "map";

        // pose
        pose.position.x = goalPosition.z;
        pose.position.y = goalPosition.x;
        pose.position.z = 0;
        pose.orientation.x = 0;
        pose.orientation.y = 0;
        pose.orientation.z = goalRotation.y;
        pose.orientation.w = 0;

        PoseStampedMsg poseStampedMsg = new PoseStampedMsg(header, pose); 
        var topicName = _topicNames[TBSelector.value];
        _ros.Publish(topicName, poseStampedMsg);

        Debug.Log(poseStampedMsg.pose);
    }

}
