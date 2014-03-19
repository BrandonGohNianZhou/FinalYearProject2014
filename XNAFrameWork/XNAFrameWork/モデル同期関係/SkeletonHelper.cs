using System;
using Microsoft.Kinect;

namespace XNAFrameWork
{
	/// <summary>
	/// A class that provides helper on skeleton data of Kinect.
	/// </summary>
	public static class SkeletonHelper
	{
		/// <summary>
		/// I will reverse to the left or right of the Kinect skeleton data.
		/// </summary>
		public static void MirrorSkeleton()
		{
			// Left and right switches the information of joint pair
			SwapJoints(JointType.ShoulderLeft, JointType.ShoulderRight);
			SwapJoints(JointType.ElbowLeft, JointType.ElbowRight);
			SwapJoints(JointType.WristLeft, JointType.WristRight);
			SwapJoints(JointType.HandLeft, JointType.HandRight);
			SwapJoints(JointType.HipLeft, JointType.HipRight);
			SwapJoints(JointType.KneeLeft, JointType.KneeRight);
			SwapJoints(JointType.AnkleLeft, JointType.AnkleRight);
			SwapJoints(JointType.FootLeft, JointType.FootRight);

			// I invert the X coordinate of all joints
			var jointTypes = (JointType[])Enum.GetValues(typeof(JointType));
			foreach (var jointType in jointTypes)
			{
				var joint = Game1.kinect.skeleton.Joints[jointType];
				var position = joint.Position;

				position.X = -position.X;

				joint.Position = position;
				Game1.kinect.skeleton.Joints[jointType] = joint;
			}
		}

		/// <summary>
		/// I alternated the information of the joint.
		/// </summary>
		private static void SwapJoints(JointType jointTypeL, JointType jointTypeR)
		{
			var jointL = Game1.kinect.skeleton.Joints[jointTypeL];
			var jointR = Game1.kinect.skeleton.Joints[jointTypeR];

			var tmpPosition = jointL.Position;
			jointL.Position = jointR.Position;
			jointR.Position = tmpPosition;

			var tmpState = jointL.TrackingState;
			jointL.TrackingState = jointR.TrackingState;
			jointR.TrackingState = tmpState;

			Game1.kinect.skeleton.Joints[jointTypeL] = jointL;
			Game1.kinect.skeleton.Joints[jointTypeR] = jointR;
		}
	}
}
