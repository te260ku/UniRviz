#include <memory>
#include <chrono>
#include "rclcpp/rclcpp.hpp"
#include "rclcpp_action/rclcpp_action.hpp"
#include "geometry_msgs/msg/pose_stamped.hpp"
#include "nav2_msgs/action/navigate_to_pose.hpp"
#include "rclcpp/time.hpp"

#include <tf2/LinearMath/Quaternion.h>
#include <tf2/LinearMath/Matrix3x3.h>

using namespace std::chrono_literals;
using std::placeholders::_1;
using std::placeholders::_2;

double tx;
double ty;
double tz;

class Nav2Client : public rclcpp::Node
{
public:
  using NavigateToPose = nav2_msgs::action::NavigateToPose;
  using GoalHandleNavigateToPose = rclcpp_action::ClientGoalHandle<NavigateToPose>;
  rclcpp_action::Client<NavigateToPose>::SharedPtr client_ptr_;

  explicit Nav2Client(): Node("nav2_send_goal")
  { 
    rclcpp::QoS qos(10);
    qos.keep_last(10);
    qos.best_effort();
    qos.durability_volatile();

    // サブスクライバーを作成
    mPoseSub = create_subscription<geometry_msgs::msg::PoseStamped>(
                "TB1/pose", qos,
                std::bind(&Nav2Client::poseCallback, this, _1) );
  
    // アクションクライアントを作成
    this->client_ptr_  = rclcpp_action::create_client<NavigateToPose>(this, "navigate_to_pose");
  }
  
  void poseCallback(const geometry_msgs::msg::PoseStamped::SharedPtr msg) {
    // Camera position in map frame
    tx = msg->pose.position.x;
    ty = msg->pose.position.y;
    tz = msg->pose.position.z;

    // Orientation quaternion
    tf2::Quaternion q(
                msg->pose.orientation.x,
                msg->pose.orientation.y,
                msg->pose.orientation.z,
                msg->pose.orientation.w);

    // 3x3 Rotation matrix from quaternion
    tf2::Matrix3x3 m(q);

    // Roll Pitch and Yaw from rotation matrix
    double roll, pitch, yaw;
    m.getRPY(roll, pitch, yaw);

    // Output the measure
    RCLCPP_INFO(get_logger(), "Received frame : X: %.2f Y: %.2f Z: %.2f",
             tx, ty, tz);
    
    if (tx >0) {
      sendGoal();
    }
}
  rclcpp::Subscription<geometry_msgs::msg::PoseStamped>::SharedPtr mPoseSub;
  
  void sendGoal(void) {
    // アクションが提供されているまでに待つ
    while (!this->client_ptr_->wait_for_action_server()) {
      RCLCPP_INFO(get_logger(), "Waiting for action server...");
    }
    // アクション　Goalの作成
    auto goal_msg = NavigateToPose::Goal();
    goal_msg.pose.header.stamp = this->now();
    goal_msg.pose.header.frame_id = "map";

    goal_msg.pose.pose.position.x = tx;
    goal_msg.pose.pose.position.y = ty;
    goal_msg.pose.pose.orientation.x = 0.0;
    goal_msg.pose.pose.orientation.y = 0.0;
    goal_msg.pose.pose.orientation.w = 1.0;
    goal_msg.pose.pose.orientation.z = 0;

    RCLCPP_INFO(get_logger(), "goal_pos_x = %f", goal_msg.pose.pose.position.x);

    // 進捗状況を表示するFeedbackコールバックを設定
    auto send_goal_options = rclcpp_action::Client<NavigateToPose>::SendGoalOptions();
    send_goal_options.feedback_callback = std::bind(&Nav2Client::feedbackCallback, this, _1, _2);
    send_goal_options.result_callback = std::bind(&Nav2Client::resultCallback, this, _1);
    // Goalをサーバーに送信
    client_ptr_->async_send_goal(goal_msg, send_goal_options);
  }

  // feedback
  void feedbackCallback(GoalHandleNavigateToPose::SharedPtr,const std::shared_ptr<const NavigateToPose::Feedback> feedback)
  {
    RCLCPP_INFO(get_logger(), "Distance remaininf = %f", feedback->distance_remaining);
  }

  // result
  void resultCallback(const GoalHandleNavigateToPose::WrappedResult & result)
  {
    switch (result.code) {
      case rclcpp_action::ResultCode::SUCCEEDED:
        RCLCPP_INFO(get_logger(), "Success!!!");
        break;
      case rclcpp_action::ResultCode::ABORTED:
        RCLCPP_ERROR(get_logger(), "Goal was aborted");
        return;
      case rclcpp_action::ResultCode::CANCELED:
        RCLCPP_ERROR(get_logger(), "Goal was canceled");
        return;
      default:
        RCLCPP_ERROR(get_logger(), "Unknown result code");
        return;
    }
  }
};

int main(int argc, char **argv)
{
  rclcpp::init(argc, argv);
  auto node = std::make_shared<Nav2Client>();
  rclcpp::spin(node);

  rclcpp::shutdown();
  return 0;
}
