//--------------------------------------//
// BoardTexture.cs						//
// 板ポリゴンを作成・描画するクラス		//
//	Class to create and draw a polygon plate
// 制作日:2013/10/23						//
// 制作者:Kouno Shin						//
//--------------------------------------//

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
	class BoardTexture
	{
		#region Field

		#region Position

		private Vector3 pos;

		/// Get and set position
		public Vector3 Pos
		{
			get { return this.pos; }
			set { this.pos = value; }
		}

		/// Get and set the X position
		public float PosX
		{
			get { return this.pos.X; }
			set { this.pos.X = value; }
		}
		/// Get and set the Y position
		public float PosY
		{
			get { return this.pos.Y; }
			set { this.pos.Y = value; }
		}
		/// Get and set the Z position
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
			get { return MyMathHelper.Vector3ToDegree(this.rotation); }
			set { this.rotation = MyMathHelper.Vector3ToRadian(value); }
		}

		// set or get the X-axis rotation
		public float rotationX
		{
			get { return MathHelper.ToDegrees(this.rotation.X); }
			set { this.rotation.X = MathHelper.ToRadians(value); }
		}
		// set or get the Y-axis rotation
		public float rotationY
		{
			get { return MathHelper.ToDegrees(this.rotation.Y); }
			set { this.rotation.Y = MathHelper.ToRadians(value); }
		}
		// set or get the Z-axis rotation
		public float rotationZ
		{
			get { return MathHelper.ToDegrees(this.rotation.Z); }
			set { this.rotation.Z = MathHelper.ToRadians(value); }
		}
		#endregion

		public float alpha { get;set;}

		// Vertex buffer
		private VertexBuffer vertexBuffer = null;

		// Basic effects
		private BasicEffect basicEffect = null;

		#endregion

		#region Constructor

		// To get the texture you want to draw
		public BoardTexture()
		{
			// Initialization of each value
			this.pos = Vector3.Zero;
			this.scale = Vector3.One;
			this.rotation = Vector3.Zero;
			this.alpha = 1.0f;

			// Effects creation
			this.basicEffect = Game1.basicEffect;
			// I authorize the use of texture
			this.basicEffect.TextureEnabled = true;

			// Create a vertex buffer
			this.vertexBuffer = new VertexBuffer(
										Game1.graphics.GraphicsDevice,
										typeof(VertexPositionTexture),
										4,
										BufferUsage.None);

			// I want to create the vertex data
			VertexPositionTexture[] vertices = new VertexPositionTexture[4];
			// I want to set the value of each vertex
			vertices[0] = new VertexPositionTexture(new Vector3(-1.0f, 1.0f, 1.0f),
												new Vector2(0.0f, 0.0f));
			vertices[1] = new VertexPositionTexture(new Vector3(1.0f, 1.0f, 1.0f),
												new Vector2(1.0f, 0.0f));
			vertices[2] = new VertexPositionTexture(new Vector3(-1.0f, -1.0f, 1.0f),
												new Vector2(0.0f, 1.0f));
			vertices[3] = new VertexPositionTexture(new Vector3(1.0f, -1.0f, 1.0f),
												new Vector2(1.0f, 1.0f));

			// I write in the vertex buffer vertex data
			this.vertexBuffer.SetData(vertices);
		}

		#endregion

		#region Function

		//----------------------------------//
		// 関数名	LoadTexture				//
		//	Function name LoadTexture
		// 機能		テクスチャのロード		//
		//	Load function texture
		// 引数		なし						//
		//	No argument
		// 戻り値	なし						//
		//	No return value
		//----------------------------------//
		public void LoadTexture(Texture2D tex)
		{
			// I want to set a texture to effect
			this.basicEffect.Texture = tex;
		}

		//------------------------------//
		// 関数名	Draw				//
		//	Function Draw
		// 機能		板ポリゴンの描画		//
		//	Drawing function of plate polygon
		// 引数		なし					//
		//	No argument
		// 戻り値	なし					//
		//	No return value
		//------------------------------//
		public void Draw()
		{
			// Texture, if it is set
			if (this.basicEffect.Texture != null)
			{
				// Set the vertex buffer
				Game1.graphics.GraphicsDevice.SetVertexBuffer(this.vertexBuffer);

				// Drawing repeated by the number of paths
				foreach (EffectPass pass in this.basicEffect.CurrentTechnique.Passes)
				{
					basicEffect.Alpha = this.alpha;
					// View Settings
					basicEffect.View = Game1.camera.view;
					// Set of projection
					basicEffect.Projection = Game1.camera.projection;
					// Setting the World
					basicEffect.World = MyMathHelper.SetWorldMatrix(this.pos, this.scale, this.rotation);

					// The start of the path
					pass.Apply();

					// The polygon drawing
					Game1.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
				}
			}
		}

		
		

		#endregion
	}
}
