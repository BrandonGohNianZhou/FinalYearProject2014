//------------------------------//
// Camera.cs					//
// カメラを管理するクラス			//
//	Class that manages the camera
// 作成日:2013/09/27				//
// 作成者:Shin Kouno				//
//------------------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAFrameWork
{
	public class Camera
	{
		#region Field

		#region Transformation matrix

		// View transformation matrix
		public Matrix view { set; get; }


		// Projective transformation matrix
		public Matrix projection { set;get; }

		#endregion

		#region ポジション

		// Camera position
		private Vector3 pos;
		public Vector3 Pos
		{
			set
			{
				pos = value;
				// I call the update process
				Update();
			}
			get
			{
				return pos;
			}
		}
		public float PosX
		{
			get
			{
				return pos.X;
			}
			set
			{
				pos.X = value;
				// I call the update process
				Update();
			}
		}

		public float PosY
		{
			get
			{
				return pos.Y;
			}
			set
			{
				pos.Y = value;
				// I call the update process
				Update();
			}
		}

		public float PosZ
		{
			get
			{
				return pos.Z;
			}
			set
			{
				pos.Z = value;
				// I call the update process
				Update();
			}
		}

		#endregion

		#region Point of gaze

		private Vector3 look;
		public Vector3 Look
		{
			set
			{
				look = value;
				// I set the view transform matrix
				LookAt(pos, look, Vector3.Up);
			}
			get
			{
				return look;
			}
		}

		#endregion

		#region 方向

		// Direction
		public Vector3 Direction
		{
			get
			{
				// To find the distance by subtracting the position from the fixation point
				Vector3 direction = look - pos;
				// The normalized distance
				direction.Normalize();
				return direction;
			}
		}

		#endregion

		#region Yaw, pitch, and roll
		// Lateral location
		private float yaw;
		public float Yaw
		{
			set
			{
				yaw = value;
				// Update
				Update();
			}
			get
			{
				return yaw;
			}
		}

		// Height position
		private float pitch;
		public float Pitch
		{
			set
			{
				pitch = value;
				// Update
				Update();
			}
			get
			{
				return pitch;
			}

		}

		// Tilt position
		private float roll;
		public float Roll
		{
			set
			{
				roll = value;
				// Update
				Update();
			}
			get
			{
				return roll;
			}
		}

		#endregion

		#region Aspect ratio

		private float aspect;
		public float Aspect
		{
			get
			{
				return aspect;
			}
			set
			{
				this.aspect = value;
				this.UpdatePerspective();
			}
		}

		#endregion

		#region Angle of view

		private float angle;
		public float Angle
		{
			get
			{
				return this.angle;
			}
			set
			{
				this.angle = value;
				this.UpdatePerspective();
			}
		}

		#endregion

		#region Visible distance

		private float nearClip;
		public float NearClip
		{
			get
			{
				return this.nearClip;
			}
			set
			{
				this.nearClip = value;
				this.UpdatePerspective();
			}
		}
		private float farClip;
		public float FarClip
		{
			get
			{
				return this.farClip;
			}
			set
			{
				this.farClip = value;
				this.UpdatePerspective();
			}
		}

		#endregion

		#endregion


		// Constructor
		public Camera(float aspectRatio,float near = 0.1f,float far = 10000.0f)
		{
			// Initialization of the view transform matrix
			yaw		= 0.0f;
			pitch	= 0.0f;
			roll	= 0.0f;
			pos		= new Vector3(0.0f, 0.0f, 10.0f);
			angle = 45.0f;
			Update();

			// Initialization of the values ​​that we have got
			this.aspect = aspectRatio;
			this.nearClip = near;
			this.farClip = far;

			// Projective transformation matrix
			PerspectiveFieldOfView(
				MathHelper.ToRadians(this.angle),	// View angle
				aspectRatio,						// Aspect ratio
				near,								// Maximum point-blank range
				far);								// Visible distance
		}

		// Update
		private void Update()
		{
			Vector3 vec = new Vector3(0, 0, -1);
			Matrix trans = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
			Vector3 look = Vector3.Transform(vec, trans) + pos;
			LookAt(pos, look, Vector3.Up);

			
		}

		// Update of projective transformation matrix
		private void UpdatePerspective()
		{
			// Projective transformation matrix
			PerspectiveFieldOfView(
				MathHelper.ToRadians(this.angle),	// View angle
				this.aspect,						// Aspect ratio
				this.nearClip,								// Maximum point-blank range
				this.farClip);								// Visible distance
		}

		// Setting the view transform matrix
		public void LookAt(Vector3 pos,Vector3 look,Vector3 up)
		{
			this.pos = pos;
			this.look = look;
			view = Matrix.CreateLookAt(pos, look, up);
		}

		// Set of projective transformation matrix
		public void PerspectiveFieldOfView(float fieldOfView, float aspectRatio, float near, float far)
		{
			projection = Matrix.CreatePerspectiveFieldOfView(
									fieldOfView,
									aspectRatio,
									near,
									far);
		}
	}
}