//--------------------------------------//
// LineList.cs							//
// LineListを描画するクラス				//
//	Class which draws the LineList
// 制作日:2013/10/17						//
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
	class LineList
	{
		#region Field

		// Vertex buffer
		private VertexBuffer vertexBuffer;

		// Vertex data
		private VertexPositionColor[] vertices;

		// Basic effects
		private BasicEffect basicEffect;

		#endregion

		#region Constructor

		public LineList()
		{
			// I want to create a vertex buffe
			this.vertexBuffer = new VertexBuffer(
										Game1.graphics.GraphicsDevice,
										typeof(VertexPositionColor),
										2,
										BufferUsage.None);

			// I want to create the vertex data
			this.vertices = new VertexPositionColor[2];

			// I want to create a basic effect
			this.basicEffect = new BasicEffect(Game1.graphics.GraphicsDevice);

			// I want to enable the vertex color in effect
			this.basicEffect.VertexColorEnabled = true;
		}

		#endregion

		#region Function

		//--------------------------------------//
		// 関数名	SetLineList					//
		//	Function name SetLineList
		// 機能		ポジション、色のセット		//
		//	Set function position and color
		// 引数		Vector3		始点				//
		//	Vector3 start argument
		//			Vector3		終点				//
		//	Vector3 end point
		//			Color		色				//
		//	Color Color
		//--------------------------------------//
		public void SetLineList(Vector3 startPos, Vector3 EndPos, Color color)
		{
			// Set the value to the vertex
			vertices[0] = new VertexPositionColor(startPos,color);
			vertices[1] = new VertexPositionColor(EndPos,color);

			// I write the vertex data
			this.vertexBuffer.SetData(vertices);
		}

		//------------------------------//
		// 関数名	Draw				//
		//	Function Draw
		// 機能		ラインの描画			//
		//	Draw the line function
		// 引数		なし					//
		//	No argument
		// 戻り値	なし					//
		//	No return value
		//------------------------------//
		public void Draw()
		{
			// Set the vertex buffer to be used for drawing
			Game1.graphics.GraphicsDevice.SetVertexBuffer(this.vertexBuffer);

			// Drawing repeated by the number of paths
			foreach (EffectPass pass in this.basicEffect.CurrentTechnique.Passes)
			{
				// The start of the path
				pass.Apply();

				// Drawing the line
				Game1.graphics.GraphicsDevice.DrawPrimitives(
												PrimitiveType.LineList,
												0,
												1);
			}
		}

		#endregion
	}
}
