//----------------------------------------------------------//
// Cube.cs													//
// メインメニューのキューブオブジェクトを管理するクラス			//
//	Class that manages the cube object in the main menu
// 制作日:2013/10/21											//
// 制作者:Kouno Shin											//
//----------------------------------------------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation in the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAFrameWork;

namespace XNAFrameWork
{
	#region Cube Class

	class Cube : IObject
	{
		#region Field

		// Model
		Model Model_Cube;

		#region Position

		private Vector3 pos;

		/// Get and set position
		public Vector3 Pos
		{
			get { return this.pos; }
			set { this.pos = value; }
		}

		/// Get and set position X
		public float PosX
		{
			get { return this.pos.X; }
			set { this.pos.X = value; }
		}
		/// Get and set position Y
		public float PosY
		{
			get { return this.pos.Y; }
			set { this.pos.Y = value; }
		}
		/// Get and set position Z
		public float PosZ
		{
			get { return this.pos.Z; }
			set { this.pos.Z = value; }
		}

		#endregion

		#region Scale
		private Vector3 scale;

		// Get and set the scale
		public Vector3 Scale
		{
			get { return this.scale; }
			set { this.scale = value; }
		}

		// Get and set the scale X
		public float scaleX
		{
			get { return this.scale.X; }
			set { this.scale.X = value; }
		}
		// Get and set the scale Y
		public float scaleY
		{
			get { return this.scale.Y; }
			set { this.scale.Y = value; }
		}
		// Get and set the scale Z
		public float scaleZ
		{
			get { return this.scale.Z; }
			set { this.scale.Z = value; }
		}
		#endregion

		#region Rotation
		private Vector3 rotation;

		// Acquisition and rotation settings
		public Vector3 Rotation
		{
			get { return  MyMathHelper.Vector3ToDegree(this.rotation); }
			set { this.rotation = MyMathHelper.Vector3ToRadian(value); }
		}

		// set or get the X-axis rotation
		public float rotationX
		{
			get { return MathHelper.ToDegrees(this.rotation.X);}
			set { this.rotation.X = MathHelper.ToRadians(value); }
		}
		// set or get the Y-axis rotation
		public float rotationY
		{
			get { return MathHelper.ToDegrees(this.rotation.Y);}
			set { this.rotation.Y = MathHelper.ToRadians(value); }
		}
		// set or get the Z-axis rotation
		public float rotationZ
		{
			get { return MathHelper.ToDegrees(this.rotation.Z);}
			set { this.rotation.Z = MathHelper.ToRadians(value); }
		}
		#endregion


		#region Rotation speed

		public float rotateSpeed { get;set;}

		#endregion
		#endregion

		#region Constructor

		public Cube()
		{
			// Reading model
			this.Model_Cube = ModelManager.GetInstance().GetModel(ModelName.MAIN_CUBE);

			// Initialization of each value
			this.pos = new Vector3(0.0f,0.0f,0.0f);
			this.scale = new Vector3(1.0f,1.0f,1.0f);
			this.rotation = new Vector3(0.0f,0.0f,0.0f);
			this.rotateSpeed = 1.5f;
		}

		#endregion
		#region Function

		//--------------------------//
		// 関数名	Draw			//
		//	Function Draw
		// 機能		描画処理			//
		//	Function drawing process
		// 引数		なし				//
		//	No argument
		// 戻り値	なし				//
		//	No return value
		//--------------------------//
		public void Draw()
		{
			// Drawing
			foreach (ModelMesh mesh in this.Model_Cube.Meshes)
			{
				// Specifies the coordinate transformation
				foreach (BasicEffect effect in mesh.Effects)
				{
					// Use the light of default
					effect.EnableDefaultLighting();

					// I set the camera
					effect.View = Game1.camera.view;
					effect.Projection = Game1.camera.projection;

					// Set the world matrix
					effect.World = MyMathHelper.SetWorldMatrix(this.pos, this.scale, this.rotation);
				}
				mesh.Draw();
			}
		}

		#endregion
	}
	#endregion



	// To manage the state of the cube
	namespace Cube_State
	{
		#region CubeState interface

		interface CubeState
		{
			// Putting the function to move the Cube always
			bool RotateCube(ref Cube cube);
		}

		#endregion

		#region Idle state

		class Idle : CubeState
		{
			// It is not nothing
			public bool RotateCube(ref Cube cube)
			{
				return true;
			}
		}

		#endregion

		#region Move to the right state

		class Right : CubeState
		{
			#region Function

			//----------------------------------//
			// 関数名	RotateCube				//
			//	Function name RotateCube
			// 機能		Cubeを右に移動させる		//
			//	I move to the right function Cube
			// 引数		移動させたいキューブ		//
			//	Cube you want to move the argument
			// 戻り値	なし						//
			//	No return value
			//----------------------------------//
			public bool RotateCube(ref Cube cube)
			{
				//Game1.debugText.Printf(cube.rotationY.ToString(),new Vector2(0,120));
				// If you do not yet fully around
				if (90.0 > cube.rotationY)
				{
					// I turn the cube
					cube.rotationY += cube.rotateSpeed;

					// Cube when excessive
					if (cube.rotationY > 90.0)
					{
						// The fixed position of the limit cube
						cube.rotationY = 90.0f;
						// I tell the rotation end
						return true;
					}
				}
				// Rotation still
				return false;
			}


			#endregion
		}
		#endregion

		#region Move to the left state

		class Left : CubeState
		{
			#region Function

			//----------------------------------//
			// 関数名	RotateCube				//
			//	Function name RotateCube
			// 機能		Cubeを左に移動させる		//
			//	I move to the left function Cube
			// 引数		移動させたいキューブ		//
			//	Cube you want to move the argument
			// 戻り値	なし						//
			//	No return value
			//----------------------------------//
			public bool RotateCube(ref Cube cube)
			{
				// If you do not yet fully around
				if (-90.0 < cube.rotationY)
				{
					// I turn the cube
					cube.rotationY -= cube.rotateSpeed;

					// Cube when excessive
					if (cube.rotationY < -90.0)
					{
						// The fixed position of the limit cube
						cube.rotationY = -90.0f;
						// I tell the rotation end
						return true;
					}
				}
				// Rotation still
				return false;
			}


			#endregion
		}

		#endregion

		#region Move on state

		class Up : CubeState
		{
			#region Function

			//----------------------------------//
			// 関数名	RotateCube				//
			//	Function name RotateCube
			// 機能		Cubeを上に移動させる		//
			//	To move up the function Cube
			// 引数		移動させたいキューブ		//
			//	Cube you want to move the argument
			// 戻り値	なし						//
			//	No return value
			//----------------------------------//
			public bool RotateCube(ref Cube cube)
			{
				// If you do not yet fully around
				if (-90.0 < cube.rotationX)
				{
					// I turn the cube
					cube.rotationX -= cube.rotateSpeed;

					// Cube when excessive
					if (cube.rotationX < -90.0)
					{
						// The fixed position of the limit cube
						cube.rotationX = -90.0f;
						// I tell the rotation end
						return true;
					}
				}
				// Rotation still
				return false;
			}


			#endregion
		}

		#endregion

		#region Move the down state

		class Down : CubeState
		{
			#region Function

			//----------------------------------//
			// 関数名	RotateCube				//
			//	Function name RotateCube
			// 機能		Cubeを下に移動させる		//
			//	Move down the function Cube
			// 引数		移動させたいキューブ		//
			//	Cube you want to move the argument
			// 戻り値	なし						//
			//	No return value
			//----------------------------------//
			public bool RotateCube(ref Cube cube)
			{
				// If you do not yet fully around
				if (90.0f > cube.rotationX)
				{
					// I turn the cube
					cube.rotationX += cube.rotateSpeed;

					// Cube when excessive
					if (cube.rotationX > 90.0f)
					{
						// Fixed a cube
						cube.rotationX = 90.0f;
						// I tell the rotation end
						return true;
					}
				}
				// Rotation still
				return false;
			}


			#endregion
		}
		#endregion 
	}
}
