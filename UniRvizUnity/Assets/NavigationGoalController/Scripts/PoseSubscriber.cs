using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;

public class PoseSubscriber : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] GameObject _tb3;
    ROSConnection _ros;
    [SerializeField] Vector3 vel;

    void Start()
    {
        _ros = ROSConnection.GetOrCreateInstance();
        _ros.Subscribe<TwistMsg>("/cmd_vel", MoveTurtlesim);
    }

    void Update()
    {
        // if (Input.GetKey(KeyCode.F)) {
        //     float speed = 0.5f;

        // _rb.velocity = _tb3.transform.rotation * new Vector3(0, 0, speed);
        // } else {
        //     _rb.velocity = Vector3.zero;
        // }

        // if (Input.GetKey(KeyCode.R)) {
        //     float speed = 0.5f;
        //     _rb.angularVelocity = new Vector3(0, speed, 0);
        // }
        // else {
        //     _rb.angularVelocity = Vector3.zero;
        // }
    }

    void MoveTurtlesim(TwistMsg Msg)
    {
        Debug.Log(Msg);
        
        var linearVelocity = (float)Msg.linear.x;
        var angularVelocity = -(float)Msg.angular.z;
        _rb.velocity = _tb3.transform.rotation * new Vector3(0, 0, linearVelocity);
        _rb.angularVelocity = new Vector3(0, angularVelocity, 0);
    }
}
