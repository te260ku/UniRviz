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

int max_waypoint_number = 4;
double tx;
double ty;
double rz;
tf2::Quaternion myQuaternion;
int waypoint_number = 0;
bool waypoint_change =true;

class Nav2Client : public rclcpp::Node
{
public:
  using NavigateToPose = nav2_msgs::action::NavigateToPose;
  using GoalHandleNavigateToPose = rclcpp_action::ClientGoalHandle<NavigateToPose>;
  rclcpp_action::Client<NavigateToPose>::SharedPtr client_ptr_;

  explicit Nav2Client(): Node("nav2_send_goal")
  { 
    //アクション Client の作成
    this->client_ptr_  = rclcpp_action::create_client<NavigateToPose>(this, "navigate_to_pose");
  }


  void waypoints(){
    
    if(waypoint_change == true){
      waypoint_number = waypoint_number +1;
      waypoint_change = false;
    }
    RCLCPP_INFO(get_logger(), "waypoint_number = %i", waypoint_number);

    switch(waypoint_number){
      case 1:
        tx = -6.45;
        ty = -6.06;
        rz = 0;//euler
        break;
      case 2:
        tx = -5.95;
        ty = -5.56;
        rz = 0;//euler
        break;
    }

    //euler -> quaternion
    myQuaternion.setRPY(0,0,rz);
    RCLCPP_INFO(get_logger(), "quaternion %f", myQuaternion.z());

    sendGoal();
  }
  
  
  void sendGoal(void) {
    
    // アクションが提供されているまでに待つ
    while (!this->client_ptr_->wait_for_action_server()) {
      RCLCPP_INFO(get_logger(), "Waiting for action server...");
    }
       //アクション　Goalの作成
       auto goal_msg = NavigateToPose::Goal();
       goal_msg.pose.header.stamp = this->now();
       goal_msg.pose.header.frame_id = "map";

       goal_msg.pose.pose.position.x = tx;
       goal_msg.pose.pose.position.y = ty;
       goal_msg.pose.pose.orientation.x = myQuaternion.x();
       goal_msg.pose.pose.orientation.y = myQuaternion.y();
       goal_msg.pose.pose.orientation.z = myQuaternion.z();
       goal_msg.pose.pose.orientation.w = myQuaternion.w();
       
    
       RCLCPP_INFO(get_logger(), "goal_pos_x = %f", goal_msg.pose.pose.position.x);
       

       //進捗状況を表示するFeedbackコールバックを設定
       auto send_goal_options = rclcpp_action::Client<NavigateToPose>::SendGoalOptions();
       send_goal_options.feedback_callback = std::bind(&Nav2Client::feedbackCallback, this, _1, _2);
       send_goal_options.result_callback = std::bind(&Nav2Client::resultCallback, this, _1);
       //Goal をサーバーに送信
       client_ptr_->async_send_goal(goal_msg, send_goal_options);
    }

  //feedback
  void feedbackCallback(GoalHandleNavigateToPose::SharedPtr,const std::shared_ptr<const NavigateToPose::Feedback> feedback)
  {
    RCLCPP_INFO(get_logger(), "Distance remaininf = %f", feedback->distance_remaining);
  }
  //result
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
    if(max_waypoint_number>waypoint_number){
      waypoint_change = true;
      waypoints();
    }
  }
};

int main(int argc, char **argv)
{
  rclcpp::init(argc, argv);
  auto node = std::make_shared<Nav2Client>();
  node->waypoints();
  rclcpp::spin(node);
  rclcpp::shutdown();
  return 0;
}
