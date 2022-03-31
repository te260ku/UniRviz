import threading
import rclpy
import os
from rclpy.node import Node

from geometry_msgs.msg import PoseStamped


class GoalPublisher(Node):

    def __init__(self):
        super().__init__('goal_publisher')
        self.publisher_ = self.create_publisher(PoseStamped, 'goal_pose', 10)
        timer_period = 0.5  # seconds
        self.timer = self.create_timer(timer_period, self.timer_callback)

    def timer_callback(self):
        goal = PoseStamped()

        x, y = (float(x) for x in input("Gaol: << ").split())

        z = 0.0

        goal.pose.position.x = x 
        goal.pose.position.y = y
        goal.pose.position.z = z
        self.publisher_.publish(goal)
        print("Publish: x: %f, y: %f, z: %f" % (x, y, z))


def main(args=None):
    rclpy.init(args=args)

    goal_publisher = GoalPublisher()

    

    
    rclpy.spin(goal_publisher)

    goal_publisher.destroy_node()
    rclpy.shutdown()


if __name__ == '__main__':
    main()