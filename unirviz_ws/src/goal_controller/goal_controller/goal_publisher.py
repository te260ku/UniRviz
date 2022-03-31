import rclpy
from rclpy.node import Node
from geometry_msgs.msg import PoseStamped
from tf2_msgs.msg import TFMessage


class GoalPublisher(Node):

    def __init__(self):
        super().__init__('goal_publisher')
        self.publisher_ = self.create_publisher(PoseStamped, 'goal_pose', 10)
        timer_period = 5.0  # seconds
        self.timer = self.create_timer(timer_period, self.timer_callback)

        self.subscription = self.create_subscription(
            TFMessage,
            '/tf',
            self.listener_callback,
            10)
        self.subscription 

        self.goal = PoseStamped()

    def timer_callback(self):


        x = self.goal.pose.position.x
        y = self.goal.pose.position.y
        z = self.goal.pose.position.z
        self.publisher_.publish(self.goal)
        print("Publish: x: %f, y: %f, z: %f" % (x, y, z))


    def listener_callback(self, msg):
        # frame_index = int(input("Index: << "))
        frame_index = 0

        frame_id = msg.transforms[frame_index].header.frame_id
        child_frame_id = msg.transforms[frame_index].child_frame_id
        
        positions = [
            msg.transforms[frame_index].transform.translation.x, 
            msg.transforms[frame_index].transform.translation.y, 
            msg.transforms[frame_index].transform.translation.z
        ]
        rotations = [
            msg.transforms[frame_index].transform.rotation.x, 
            msg.transforms[frame_index].transform.rotation.y, 
            msg.transforms[frame_index].transform.rotation.z, 
            msg.transforms[frame_index].transform.rotation.w
        ]

        self.goal.pose.position.x = positions[0] 
        self.goal.pose.position.y = positions[1]
        self.goal.pose.position.z = positions[2]

        # print("%f, %f, %f" % (positions[0], positions[1], positions[2]))
            


def main(args=None):
    rclpy.init(args=args)

    goal_publisher = GoalPublisher()

    

    
    rclpy.spin(goal_publisher)

    goal_publisher.destroy_node()
    rclpy.shutdown()


if __name__ == '__main__':
    main()