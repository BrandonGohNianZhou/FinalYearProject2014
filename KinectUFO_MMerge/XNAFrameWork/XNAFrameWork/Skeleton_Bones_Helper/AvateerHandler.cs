//--------------------------------------------------------------------------//
// AvateerHandller.cs														//
// kinectの骨格データに合わせてモデルを動かすクラス							//
//	Class to move the model to fit the data skeleton of kinect
// 参考:http://www.moto-square.com/2012/09/10/kinect3dskeletonsample/		//
// 制作日:2013/10/14														//
// 制作者:Kouno Shin														//
//--------------------------------------------------------------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAFrameWork;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using SkinnedModel;

namespace XNAFrameWork
{
	#region enum declaration

    //// Bone for configuring the model
    //public enum BoneType
    //{
    //    Hip,
    //    Spine,
    //    UpperArmRight,
    //    ForeArmRight,
    //    HandRight,
    //    UpperArmLeft,
    //    ForeArmLeft,
    //    HandLeft,
    //    Head,
    //    ThighLeft,
    //    ShinLeft,
    //    FootLeft,
    //    ThighRight,
    //    ShinRight,
    //    FootRight,
    //    MaxBone,
    //}

    public enum BoneType
    {
        SkateBoard,

        Hip,

        ThighLeft,
        ShinLeft,
        AnkleLeft,
        FootLeft,

        ThighRight,
        ShinRight,
        AnkleRight,
        FootRight,

        Spine,
        ShoulderCenter, // new
        Neck,// new
        Head, 

        UpperArmLeft,
        ForeArmLeft,
        HandLeft,

        UpperArmRight,
        ForeArmRight,
        HandRight,

        MaxBone,
    }
	#endregion

	public class AvateerHandler
	{
		#region field

		// Model (read-only)
		readonly Model model;

		// Number of model bone (read-only)
		readonly Dictionary<int, BoneType> boneIndices;

		// Bone model information
		public Matrix[] boneTransforms;

		// I want to save the world matrix of the model
		public Matrix[] testMatrix;

		#endregion

		#region Constructor

		// (Model, bone number) argument
		public AvateerHandler(Model model, Dictionary<int, BoneType> boneIndices)
		{
			// Save the skinning data model
			var skinningData = model.Tag as SkinningData;

			// Get the model
			this.model = model;
			// Get the bone number information
			this.boneIndices = boneIndices;
			// Get data from skinning bones information
			this.boneTransforms = new Matrix[skinningData.BindPose.Count];
			// Initialize the world matrix of the model
			this.testMatrix = new Matrix[skinningData.BindPose.Count];
		}

		#endregion

		#region function

		//------------------------------------------//
		// 関数名	Update							//
		//	Function Update
		// 機能		モデルのボーン情報を更新する		//
		//	Update the information of bone functional model
		// 引数		スケルトンデータ					//
		//	Argument skeleton data
		// 戻り値	なし								//
		//	No return value
		//------------------------------------------//
        public void Update(Skeleton skeleton, AnimationPlayer animationPlayer, bool JumpAnimate, bool KickAnimate, bool DuckAnimate, bool KnockAnimate)
		{
			// Get information of skin model
            var skinningData = this.model.Tag as SkinningData;

			// To reflect the skeletal data of Kinect for bone information on which to base the model
            var worldBindPose = new Matrix[skinningData.BindPose.Count];
            testMatrix = new Matrix[skinningData.BindPose.Count];

            // animation transformation matrix
            Matrix[] bones;
            if (animationPlayer != null)
            {
                //bones = animationPlayer.GetSkinTransforms();
                bones = animationPlayer.GetBoneTransforms();
            }
            else
            {
                bones = new Matrix[(int)BoneType.MaxBone];
            }

            for (var boneIndex = 0; boneIndex < skinningData.BindPose.Count; boneIndex++)
            {
				// I get the bone information on which to base
                var boneTransform = skinningData.BindPose[boneIndex];
                var parentBoneIndex = skinningData.SkeletonHierarchy[boneIndex];
                if (parentBoneIndex > -1)
                {
					// I synthesize the bone information of parents who completed the reflection
                    boneTransform *= worldBindPose[parentBoneIndex];
                    boneTransform = bones[boneIndex] * worldBindPose[parentBoneIndex];
                }
                else
                {
                    boneTransform = bones[boneIndex];
                }

				// To reflect the coordinates of the joint according to the bone
                switch (this.boneIndices[boneIndex])
                {
                    case BoneType.Hip:
                        {
                            if (JumpAnimate || KickAnimate || DuckAnimate || KnockAnimate)
                            {
                            }
                            else
                            {
                               // TransformRootBone(skeleton, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.Head:
                        {
                            if (JumpAnimate || DuckAnimate || KnockAnimate)
                            {
                            }
                            else
                            {
                                TransformNodeBone(skeleton, JointType.Head, JointType.ShoulderCenter, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.Spine:
                        {
                            if (JumpAnimate || DuckAnimate)
                            {
                            }
                            else
                            {
                                TransformNodeBone(skeleton, JointType.ShoulderCenter, JointType.HipCenter, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.UpperArmRight:
                        {
                            if (JumpAnimate || DuckAnimate)
                            {
                            }
                            else
                            {
                                TransformNodeBone(skeleton, JointType.ShoulderRight, JointType.ElbowRight, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.ForeArmRight:
                        {
                            if (JumpAnimate || DuckAnimate)
                            {
                            }
                            else
                            {
                                TransformNodeBone(skeleton, JointType.ElbowRight, JointType.WristRight, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.HandRight:
                        {
                            if (JumpAnimate || DuckAnimate)
                            {
                            }
                            else
                            {
                                TransformNodeBone(skeleton, JointType.WristRight, JointType.HandRight, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.UpperArmLeft:
                        {
                            if (JumpAnimate || DuckAnimate)
                            {
                            }
                            else
                            {
                                TransformNodeBone(skeleton, JointType.ShoulderLeft, JointType.ElbowLeft, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.ForeArmLeft:
                        {
                            if (JumpAnimate || DuckAnimate)
                            {
                            }
                            else
                            {
                                TransformNodeBone(skeleton, JointType.ElbowLeft, JointType.WristLeft, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.HandLeft:
                        {
                            if (JumpAnimate || DuckAnimate)
                            {
                            }
                            else
                            {
                                TransformNodeBone(skeleton, JointType.WristLeft, JointType.HandLeft, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.ThighRight:
                        {
                            if (JumpAnimate || KickAnimate || KnockAnimate)
                            {
                                //boneTransform = worldBindPose[parentBoneIndex] * bones[boneIndex];
                                //boneTransform = skinningData.BindPose[boneIndex];// *worldBindPose[parentBoneIndex];
                            }
                            else
                            {
                                //TransformNodeBone(skeleton, JointType.HipRight, JointType.KneeRight, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.ShinRight:
                        {
                            if (JumpAnimate || KickAnimate || KnockAnimate)
                            {
                            }
                            else
                            {
                               // TransformNodeBone(skeleton, JointType.KneeRight, JointType.AnkleRight, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.FootRight:
                        {
                            if (JumpAnimate || KickAnimate || KnockAnimate)
                            {
                            }
                            else
                            {
                                //TransformNodeBone(skeleton, JointType.AnkleRight, JointType.FootRight, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.ThighLeft:
                        {
                            if (JumpAnimate || KickAnimate || KnockAnimate)
                            {
                            }
                            else
                            {
                               // TransformNodeBone(skeleton, JointType.HipLeft, JointType.KneeLeft, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.ShinLeft:
                        {
                            if (JumpAnimate || KickAnimate || KnockAnimate)
                            {
                            }
                            else
                            {
                               // TransformNodeBone(skeleton, JointType.KneeLeft, JointType.AnkleLeft, ref boneTransform);
                            }
                        }
                        break;
                    case BoneType.FootLeft:
                        {
                            if (JumpAnimate || KickAnimate || KnockAnimate)
                            {
                            }
                            else
                            {
                                //TransformNodeBone(skeleton, JointType.AnkleLeft, JointType.FootLeft, ref boneTransform);
                            }
                        }
                        break;
                }
                worldBindPose[boneIndex] = boneTransform;
                testMatrix[boneIndex] = boneTransform;

				// To be stored in the field by converting the coordinate system of the bone information
                this.boneTransforms[boneIndex] = skinningData.InverseBindPose[boneIndex] * worldBindPose[boneIndex];
                //testMatrix = boneTransforms;
            }
		}

		//----------------------------------------------------------------------//
		// 関数名	TransformRootBone											//
		//	Function name TransformRootBone
		// 機能		ルートとなる関節に合わせてモデルのボーン情報を生成する			//
		//	Create a bone model information in accordance with the joint to become the root function
		// 引数		スケルトンデータ、ボーンデータ									//
		//	Argument data skeleton, bone data
		// 戻り値	なし															//
		//	No return value
		//----------------------------------------------------------------------//
		private void TransformRootBone(Skeleton skeleton, ref Matrix boneTransform)
		{
			Vector3 s, t;
			Quaternion r;
			if (boneTransform.Decompose(out s, out r, out t))
			{
				// I get the coordinates of the joint on the left and right sides of the origin
				var leftPosition = ConvertJointPosition(skeleton, JointType.HipLeft);
				var rightPosition = ConvertJointPosition(skeleton, JointType.HipRight);

				// I get the rotation axis and angle to rotate the bone from the difference of the coordinates
				var direction = Vector3.Normalize(leftPosition - rightPosition);
				var angle = (float)Math.Acos(Vector3.Dot(Vector3.Right, direction));
				var axis = Vector3.Normalize(Vector3.Cross(Vector3.Right, direction));

				// I want to convert to the amount of movement of the model the amount of movement of the origin
				t = ConvertJointPosition(skeleton, JointType.HipCenter) * 1;

				// Set the reference argument by generating bone information
				boneTransform = Matrix.CreateScale(s) * Matrix.CreateFromQuaternion(r) * Matrix.CreateFromAxisAngle(axis, angle) * Matrix.CreateTranslation(t);
			}
		}

		//------------------------------------------------------------------//
		// 関数名	TransformNodeBone										//
		//	Function name TransformNodeBone
		// 機能		ノードとなる関節に合わせてモデルのボーン情報を生成			//
		//	Create a bone model information in accordance with the joint that it is functional nodes
		// 引数		スケルトンデータ											//
		//	Argument skeleton data
		//			始点の関節												//
		//	Joint of the starting point
		//			終点の関節												//
		//	End point of the point
		//			ボーン情報												//
		//	Bone Information
		// 戻り値	なし														//
		//	No return value
		//------------------------------------------------------------------//
		private void TransformNodeBone( Skeleton skeleton, 
										JointType beginJointType, 
										JointType endJointType, 
										ref Matrix boneTransform)
		{
			Vector3 scale, translation;
			Quaternion rotation;

			// Scale, rotation, broken down into the bone matrix position
			// Decompose the function matrix is Decompose ()
			if (boneTransform.Decompose(out scale, out rotation, out translation))
			{
				// I get the coordinates of joints and end joints of the start point
				var beginPosition = ConvertJointPosition(skeleton, beginJointType);
				var endPosition = ConvertJointPosition(skeleton, endJointType);

				// I get the rotation axis and angle to rotate the bone from the difference of the coordinates
				var direction = Vector3.Normalize(endPosition - beginPosition);
				var angle = (float)Math.Acos(Vector3.Dot(Vector3.Down, direction));
				var axis = Vector3.Normalize(Vector3.Cross(Vector3.Down, direction));

				// I cut off the rotation of the parent bone
				rotation = Quaternion.Identity;

				// Set the reference argument by generating bone information
				boneTransform = Matrix.CreateScale(scale)
									* Matrix.CreateFromQuaternion(rotation)
										* Matrix.CreateFromAxisAngle(axis, angle)
											* Matrix.CreateTranslation(translation);
			}
		}

        ////------------------------------------------------------------//
        //// 関数名	TransformNodeBone									//
		//	Function name TransformNodeBone
        //// 機能		ノードとなる関節に合わせてモデルのボーン情報を生成	//
		//	Create a bone model information in accordance with the joint that is functional nodes
        //// 引数		スケルトンデータ									//
		//	Argugment skeleton data
        ////			始点の関節										//
		//	Joint of the starting point
        ////			終点の関節										//
		//	Joint of the end point
        ////			ボーン情報										//
		//	Bone information
        //// 戻り値	なし													//
		//	No return value
        ////------------------------------------------------------------//
        //private void TransformNodeBone2(Skeleton skeleton,
        //                                JointType beginJointType,
        //                                JointType endJointType,
        //                                ref Matrix boneTransform)
        //{
        //    Vector3 scale, translation;
        //    Quaternion rotation;

        //    // Scale, rotation, broken down into the bone matrix position
		//    // Decompose()	//Decompose the function matrix
        //    if (boneTransform.Decompose(out scale, out rotation, out translation))
        //    {
		//        // I get the coordinates of joints and end joints of the start point
        //        var beginPosition = ConvertJointPosition(skeleton, beginJointType);
        //        var endPosition = ConvertJointPosition(skeleton, endJointType);

		//        // I get the rotation axis and angle to rotate the bone from the difference of the coordinates
        //        var direction = Vector3.Normalize(endPosition - beginPosition);
        //        var angle = (float)Math.Acos(Vector3.Dot(Vector3.Down, direction));
        //        var axis = Vector3.Normalize(Vector3.Cross(Vector3.Down, direction));

		//        // I cut off the rotation of the parent bone
        //       // rotation = Quaternion.Identity;

		//        // Set the reference argument by generating bone information
        //        boneTransform = Matrix.CreateFromQuaternion(rotation)
        //                                * Matrix.CreateRotationX(-angle + 3.14f / 6.0f)
        //                                * Matrix.CreateRotationY(-angle + 3.14f / 6.0f)
        //                                * Matrix.CreateRotationZ( -angle + 3.14f / 6.0f )
        //                                * Matrix.CreateScale(scale)
        //                                //* Matrix.CreateFromAxisAngle(axis, angle)
        //                                    * Matrix.CreateTranslation(translation);
        //    }
        //}

		//------------------------------------------------------//
		// 関数名	ConvertJointPosition						//
		//	Function name ConvertJointPosition
		// 機能		関節の座標をモデルの座標系に変換して取得		//
		//	Obtained by converting the coordinate system of the mode coordinates of the joint functions
		// 引数		スケルトンデータ、関節のタイプ					//
		//	Type argument skeleton data, the joint
		// 戻り値	Vector3										//
		//	Returns Vector3
		//------------------------------------------------------//
		private Vector3 ConvertJointPosition(Skeleton skeleton, JointType jointType)
		{
			// Get the position of joint
			var position = skeleton.Joints[jointType].Position;
			// Convert and return to the coordinate system of the model coordinates acquired
			return new Vector3(-position.X, position.Y, -position.Z);
		}

		//------------------------------//
		// 関数名	Draw				//
		//	Function Draw
		// 機能		モデルの描画			//
		//	Drawing of the functional model
		// 引数		ワールド行列			//
		//	Argument world matrix
		// 戻り値	なし					//
		//	No return value
		//------------------------------//
        public void Draw(Skeleton skeleton, AnimationPlayer animationPlayer)
		{
			// I draw the model for each part
			foreach (ModelMesh mesh in this.model.Meshes)
			{
				foreach (SkinnedEffect effect in mesh.Effects)
				{
                    //if (animationPlayer != null)
                    //{
					//    // Set the bone information to model
                    //    Matrix[] bones = animationPlayer.GetSkinTransforms();
                    //    effect.SetBoneTransforms(bones);//Matrix.Multiply(this.boneTransforms, bones));
                    //}
                    //else
                         effect.SetBoneTransforms(this.boneTransforms);
                }
				// Draw model
				mesh.Draw();
			}
        }

		//--------------------------------------------------//
		// 関数名	GetJointPos								//
		//	Function name GetJointPos
		// 機能		指定された関節のワールド座標を返す			//
		//	I return the world coordinates of the joint that has been specified function
		// 引数		ボーンのインデックス						//
		//	Index of the argument bone
		// 戻り値	Vector3		ポジション					//
		//	Vector3 position return value
		//--------------------------------------------------//
		public Vector3 GetJointPos(BoneType boneType)
		{
			// (Only pos in practice) for storing the one obtained by decomposition
			Vector3 pos,scale;
			Quaternion rotation;

			// If you can get
			if (testMatrix[(int)boneType] != null)
			{
				// The decomposed matrix obtained
				testMatrix[(int)boneType].Decompose(out scale, out rotation, out pos);

				// Value of only initialized if entered if
				if (scale == Vector3.Zero
					&& pos == Vector3.Zero)
				{
					// I will return 0
					pos = Vector3.Zero;
				}
			}
			// If you can not get
			else
			{
				// I will return 0
				pos = Vector3.Zero;
			}


			// Returns the positions acquired
			return pos;
		}


        // testing rotation
        public Quaternion GetJointRotation(BoneType boneType)
        {
            Vector3 pos, scale;
            Quaternion rotation;

            if (testMatrix[(int)boneType] != null)
            {
                testMatrix[(int)boneType].Decompose(out scale, out rotation, out pos);

                if (scale == Vector3.Zero
                    && rotation == Quaternion.Identity)
                {
                    rotation = Quaternion.Identity;
                }
            }
            else
            {
                rotation = Quaternion.Identity;
            }

            return rotation;
        }

        //public Vector3 GetJointPos(BoneType boneType)
        //{
		//    // (Only pos in practice) for storing the one obtained by decomposition
        //    Vector3 pos, scale;
        //    Quaternion rotation;

		//    // If you can get
        //    if (testMatrix[(int)boneType] != null)
        //    {
		//        // The decomposed matrix obtained
        //        testMatrix[(int)boneType].Decompose(out scale, out rotation, out pos);

		//        // Value of only initialized if entered if
        //        if (scale == Vector3.Zero
        //            && pos == Vector3.Zero)
        //        {
		//            // I will return 0
        //            pos = Vector3.Zero;
        //        }
        //    }
		//    // If you can not get
        //    else
        //    {
		//        // I will return 0
        //        pos = Vector3.Zero;
        //    }


		//    // Returns the positions acquired
        //    return pos;
        //}

		#endregion
	}
}
