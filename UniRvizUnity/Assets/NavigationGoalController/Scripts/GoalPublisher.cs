using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using RosMessageTypes.BuiltinInterfaces;

/// <summary>
/// ゴールをパブリッシュするスクリプト
/// </summary>
public class GoalPublisher : MonoBehaviour
{
    [SerializeField] Dropdown _TBSelector;
    [SerializeField] string[] _topicNames;
    ROSConnection _ros;

    void Start()
    {
        _ros = ROSConnection.GetOrCreateInstance();
        foreach (var topicName in _topicNames)
        {
            _ros.RegisterPublisher<PoseStampedMsg>(topicName);
        }

        _ros.RegisterPublisher<PoseWithCovarianceStampedMsg>("/initialpose");
    }

    public void SendInitialPose() {
        PoseWithCovarianceMsg pose = new PoseWithCovarianceMsg();
        HeaderMsg header = new HeaderMsg();

        header.stamp.sec = 0;
        header.stamp.nanosec = 0;
        header.frame_id = "map";

        pose.pose.position.x = 0.0;
        pose.pose.position.y = 0.0;
        pose.pose.position.z = 0.0;
        pose.pose.orientation.x = 0.0;
        pose.pose.orientation.y = 0.0;
        pose.pose.orientation.z = 0.0;
        pose.pose.orientation.w = 0.1;

        PoseWithCovarianceStampedMsg poseStampedMsg = new PoseWithCovarianceStampedMsg(header, pose); 
        _ros.Publish("/initialpose", poseStampedMsg);

        Debug.Log(pose.pose);
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
        var topicName = _topicNames[_TBSelector.value];
        _ros.Publish(topicName, poseStampedMsg);

        Debug.Log(poseStampedMsg.pose);
    }

}
