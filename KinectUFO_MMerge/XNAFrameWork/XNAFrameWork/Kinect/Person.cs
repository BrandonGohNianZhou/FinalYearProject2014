//--------------------------------------------------//
// Person.cs										//
// kinectから送られてくる人の情報を管理するクラス		//
//	Class that managese the information of people sent from the kinect
// 制作日:2013/10/09									//
// 制作者:Kouno Shin									//
//--------------------------------------------------//

//----------------------//
//----名前空間の省略----//	Abbreviation of the namespace
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using XNAFrameWork;

namespace XNAFrameWork
{
	/*
	#region Enum declaration

	// Information of joint
	public enum JOINT : byte
	{
		HIP_CENTER = 0,			// おしり中心
		SPINE,					// 背骨
		SHOULDER_CENTER,		// 肩中心
		HEAD = 3,				// 頭
		SHOULDER_LEFT,			// 肩左
		ELBOW_LEFT,				// 肘左
		WRIST_LEFT,				// 手首左
		SHOULDER_RIGHT,			// 肩右
		ELBOW_RIGHT,			// 肘右
		WRIST_RIGHT,			// 手首右
		HAND_RIGHT,				// 手右
		HIP_LEFT,				// おしり左
		KNEE_LEFT,				// 膝左
		ANKLE_LEFT,				// 足首左
		FOOT_LEFT,				// 足左
		HIP_RIGHT,				// おしり右
		KNEE_RIGHT,				// 膝右
		ANKLE_RIGHT,			// 足首右
		FOOT_RIGHT,				// 足右

		MAX_JOINTS,				// 関節の最大数
	}

	#endregion
	 * */
	public class Person
	{
		#region Field

		// Skeleton of the user
		Skeleton userSkeleton;

		// (20 in all this) position of each joint of the user
		SkeletonPoint[] joint = new SkeletonPoint[20];

		// Position of each joint of the user (1-second update)
		SkeletonPoint[] secondJoint = new SkeletonPoint[20];

		// Counter of 1 second update
		//int count;

		#endregion

		#region Constructor

		public Person()
		{
			// Initialize the skeleton of the user
			this.userSkeleton = new Skeleton();

			// Initialize each joint
			for (int i = 0; i < 20; i++)
			{
				this.joint[i] = new SkeletonPoint();
				this.secondJoint[i] = new SkeletonPoint();
			}

			// Initialize the counter
			//this.count = 0;
		}

		#endregion

		#region Function
		//----------------------------------------------------------//
		// 関数名	Update											//
		//	Function Update
		// 機能		kinectから最新のデータをもらうための更新処理		//
		//	Update process to get the latest data from the functional kinect
		// 引数		スケルトンのデータ								//
		//	Data of the skeleton argument
		// 戻り値	なし												//
		//	No return value
		//----------------------------------------------------------//
		public void Update(Skeleton skeleton)
		{
			// I repeat only the number of joint
			for (int i = 0; i < 20; i++)
			{
				// The assignment of the joint 20
				joint[i] = skeleton.Joints[(JointType)i].Position;
			}
		}

		//--------------------------------------------------//
		// 関数名	UpdateSecond							//
		//	Function name UpdateSecond
		// 機能		1秒ごとにスケルトンデータを更新する		//
		//	I want to update the skeleton data every second function
		// 引数		スケルトンデータ							//
		//	Argument skeleton data
		// 戻り値	なし										//
		//	No return value
		//--------------------------------------------------//
		public void UpdateSecond(Skeleton skeleton)
		{
			// And adding the counter
			//count++;

			// When you say the number that is divisible by 60 is counter to (= 1 second) if
			//if (count % 60 == 0)
			//{
				// Update process
				// I repeat only the number of joint
				for (int i = 0; i < 20; i++)
				{
					// Updated data of joint
					this.secondJoint[i] = skeleton.Joints[(JointType)i].Position;
				}
			//}
		}


		//----------------------------------------------------------//
		// 関数名	GetJointPos										//
		//	Function name GetJointPos
		// 機能		指定された関節のポジションを渡す					//
		//	I pass the position of the joint which is function specification
		// 引数		ジョイントの種類									//
		//	Type of argument joint
		// 戻り値	関節のポジション(ローカルの値)をVector3で返す		//
		//	I return in Vector3 (the value of the local) position of joint return value
		//----------------------------------------------------------//
		public Vector3 GetJointPos(JointType jointNumber)
		{
			// Create vector3 to save the joint information
			var jointPos = new Vector3();

			// Get joint information
			jointPos.X = this.joint[(int)jointNumber].X;
			jointPos.Y = this.joint[(int)jointNumber].Y;
			jointPos.Z = this.joint[(int)jointNumber].Z;

			// Returns the joint information acquired
			return jointPos;
		}

		//----------------------------------------------------------//
		// 関数名	GetSecondJointPos								//
		//	Function name GetSecondJointPos
		// 機能		指定された関節のポジションを渡す(1秒更新ver)		//
		//	Pass the position of the joint that has been specified function (1 second update ver)
		// 引数		ジョイントの種類									//
		//	Type of argument joint
		// 戻り値	関節のポジション(ローカルの値)をVector3で返す		//
		//	I return in Vector3( the value of the local )position of joint return value
		//----------------------------------------------------------//
		public Vector3 GetSecondJointPos(JointType jointNumber)
		{
			// Create vector3 to save the joint information
			var jointPos = new Vector3();

			// Get joint information
			jointPos.X = this.secondJoint[(int)jointNumber].X;
			jointPos.Y = this.secondJoint[(int)jointNumber].Y;
			jointPos.Z = this.secondJoint[(int)jointNumber].Z;

			// Returns the joint information acquired
			return jointPos;
		}

        // rise hand above head
        public bool RiseHandAboveHead()
        {
            // if player hand is above their head
            if (GetJointPos(JointType.WristLeft).Y > GetJointPos(JointType.Head).Y
                && GetJointPos(JointType.WristRight).Y > GetJointPos(JointType.Head).Y)
            {
                return true;
            }
             return false;
        }

		//------------------------------------------------------//
		// 関数名	Pushed										//
		//	Function name Pushed
		// 機能		プレイヤーが決定処理をしたかどうか判定			//
		//	Judgment function player whether the decision process
		//			プレイヤーが手を前に大きくだしたら決定			//
		//	Decision once a player has a large soup before hand
		// 引数		なし											//
		//	No argument
		// 戻り値	決定			true							//
		//	Return value determining true
		//			決定していない	false						//
		//	False, which has not been determined
		//------------------------------------------------------//
		public bool Pushed()
		{
			// I remember the position of the right hand of the player of one second before
			float playerRightHand = this.GetSecondJointPos(JointType.HandRight).Z;

			// Position if the current come before than one second before if
			if (playerRightHand - 0.15f > this.GetJointPos(JointType.HandRight).Z)
			{
				// Decision
				return true;
			}
			// Not determined
			return false;
		}

		//----------------------------------------------//
		// 関数名	Right								//
		//	Function Right
		// 機能		プレイヤーが右の処理をしたか判定		//
		//	Judgment function player whether the processing of right
		// 引数		なし									//
		//	No argument
		// 戻り値	決定	: true							//
		//	Return value determination: true
		//			非決定	: false						//
		//	Non-deterministic: false
		//----------------------------------------------//
		public bool Right()
		{
			// I remember the position of the right hand of the player of one second before
			float playerRightHand = this.GetSecondJointPos(JointType.HandRight).X;

			// Position if the current come to the right than one second before if
			if (playerRightHand + 0.3 < this.GetJointPos(JointType.HandRight).X)
			{
				// Decision
				return true;
			}
			// Not determined
			return false;
		}

		//----------------------------------------------//
		// 関数名	Left								//
		//	Function Left
		// 機能		プレイヤーが左の処理をしたか判定		//
		//	Judgement function player whether the processing of the left
		// 引数		なし									//
		//	No argument
		// 戻り値	決定	: true							//
		//	Return value determination: true
		//			非決定	: false						//
		//	Non-deterministic: false
		//----------------------------------------------//
		public bool Left()
		{
			// I remember the position of the right hand of the player of one second before
			float playerRightHand = this.GetSecondJointPos(JointType.HandRight).X;

			// Position if the current coming to the left than one second before if
			if (playerRightHand - 0.3 > this.GetJointPos(JointType.HandRight).X)
			{
				// Decision
				return true;
			}
			// Not determined
			return false;
		}

		//----------------------------------------------//
		// 関数名	Up									//
		//	Function Up
		// 機能		プレイヤーが上の処理をしたか判定		//
		//	Judgment function player whether the processing of the above
		// 引数		なし									//
		//	No argument
		// 戻り値	決定	: true							//
		//	Return value determination: true
		//			非決定	: false						//
		//	Non-deterministic: false
		//----------------------------------------------//
		public bool Up()
		{
			// I remember the position of the right hand of the player of one second before
			float playerRightHand = this.GetSecondJointPos(JointType.HandRight).Y;

			// Position if the current come to the right than one second before if
			if (playerRightHand + 0.5 < this.GetJointPos(JointType.HandRight).Y)
			{
				// Decision
				return true;
			}
			// Not determined
			return false;
		}

		//----------------------------------------------//
		// 関数名	Down								//
		//	Function Down
		// 機能		プレイヤーが下の処理をしたか判定		//
		//	Judgment function player or have been processed under
		// 引数		なし									//
		//	No argument
		// 戻り値	決定	: true							//
		//	Return value determination: true
		//			非決定	: false						//
		//	Non-deterministic: false
		//----------------------------------------------//
		public bool Down()
		{
			// I remember the position of the right hand of the player of one second before
			float playerRightHand = this.GetSecondJointPos(JointType.HandRight).Y;

			// Position if the current come under than one second before if
			if (playerRightHand - 0.5 > this.GetJointPos(JointType.HandRight).Y)
			{
				// Decision
				return true;
			}
			// Not determined
			return false;
		}
		
        private double L_R_UPDATE = 1.0;
        private float L_R_DIFFX = 0.5f;

        private Joint RH_JOINT;
        private double RH_SECONDS = 0;
        // right hand~ left movement
        public bool RH_Left(GameTime gameTime)
        {
			// I remember the position of the right hand of the player of one second before
            //float playerRightHand = this.GetSecondJointPos(JointType.HandRight).X;

			// Position if the current coming to the left than one second before if
            //if (playerRightHand - 0.3 > this.GetJointPos(JointType.HandRight).X)
			//{
                // Decision
            //    return true;
            //}
            // Not determined
            //return false;
			
		//Update the Left Hand Joint every second
            if (gameTime.TotalGameTime.TotalSeconds - RH_SECONDS > L_R_UPDATE)
            {


                RH_SECONDS = gameTime.TotalGameTime.TotalSeconds;

                if (Game1.kinect.skeleton != null)
                {
                    RH_JOINT = Game1.kinect.skeleton.Joints[JointType.HandRight];
                }
            }


            if (RH_JOINT != null && Game1.kinect.skeleton != null)
            {
                //Get the current joint from skeleton
                Joint CurrentJoint = Game1.kinect.skeleton.Joints[JointType.HandRight];
                //Get the difference between last joint and current joint
                float JointDiffX = CurrentJoint.Position.X - RH_JOINT.Position.X;

                //If it is negative make it positive
                if (JointDiffX < 0) JointDiffX *= -1;

                if (JointDiffX > L_R_DIFFX) return true;
            }

            return false;
        }

        private Joint LH_JOINT;
        private double LH_SECONDS = 0;

        // left hand~ right movement
        public bool LH_Right(GameTime gameTime)
        {
			// I remember the position of the right hand of the player of one second before
            //float playerLeftHand = this.GetSecondJointPos(JointType.HandLeft).X;

			// Position if the current come to the right than one second before if
            //if (playerLeftHand + 0.3 < this.GetJointPos(JointType.HandLeft).X)
            //{
                // Decision
            //    return true;
            //}
            // Not determined
            //return false;

            //Update the Left Hand Joint every second
            if (gameTime.TotalGameTime.TotalSeconds - LH_SECONDS > L_R_UPDATE)
            {


                LH_SECONDS = gameTime.TotalGameTime.TotalSeconds;

                if (Game1.kinect.skeleton != null)
                {
                    LH_JOINT = Game1.kinect.skeleton.Joints[JointType.HandLeft];
                }
            }


            //Make sure joint and skeleton are not null
            if (LH_JOINT != null && Game1.kinect.skeleton != null)
            {
                //Get the current joint from skeleton
                Joint CurrentJoint = Game1.kinect.skeleton.Joints[JointType.HandLeft];
                //Get the difference between last joint and current joint
                float JointDiffX = CurrentJoint.Position.X - LH_JOINT.Position.X;

                //If it is negative make it positive
                if (JointDiffX < 0) JointDiffX *= -1;

                if (JointDiffX > L_R_DIFFX) return true;
            }

            return false;
        }
		#endregion
	}
}
