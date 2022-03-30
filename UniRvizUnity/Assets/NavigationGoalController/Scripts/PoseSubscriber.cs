using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class PoseSubscriber : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    ROSConnection _ros;

    void Start()
    {
        _ros = ROSConnection.GetOrCreateInstance();
        _ros.Subscribe<TwistMsg>("/cmd_vel", MoveTurtlesim);
    }

    void MoveTurtlesim(TwistMsg Msg)
    {
        print(Msg);
        _rb.velocity = transform.forward * (float)Msg.linear.x;
        _rb.angularVelocity = new Vector3(0, (float)Msg.angular.z, 0);
    }
}
