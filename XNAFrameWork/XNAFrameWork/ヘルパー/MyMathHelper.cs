//------------------------------------------//
// MyMathHelper.cs							//
// MathHelperで足りないものを補うクラス		//
//	Class to make up for those missing in MathHelper
// 制作日:2013/10/03							//
// 制作者:Kouno Shin							//
//------------------------------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNAFrameWork;
using Microsoft.Xna.Framework.Graphics;

namespace XNAFrameWork
{
	static public class MyMathHelper
	{
		// Random number
		public static Random rand = new Random();



		//----------------------------------------------------------//
		// 関数名	Vector3ToRadian									//
		//	Function name Vector3ToRadian
		// 機能		Vector3型で渡されたすべての項目をradianで返す		//
		//	I return in radian all the items passed in the Vector3 type function
		// 引数		Vector3のdegree角度								//
		//	Degree angle of argument Vector3
		// 戻り値	変換したVector3									//
		//	Vector3 you return value conversion
		//----------------------------------------------------------//
		public static Vector3 Vector3ToRadian(Vector3 vec)
		{
			return new Vector3(
							MathHelper.ToRadians(vec.X),
							MathHelper.ToRadians(vec.Y),
							MathHelper.ToRadians(vec.Z));
		}

		//--------------------------------------------------------------//
		// 関数名	Vector3ToDegree										//
		//	Function name Vector3ToDegree
		// 機能		Vector3型で渡されたすべての項目をdegreeで返す			//
		//	I return in degree all the items passed in the Vector3 type function
		// 引数		Vector3のradion角度									//
		//	Radian angle of argument Vector3
		// 戻り値	変換したVector3										//
		//	Vector3 you return value conversion
		//--------------------------------------------------------------//
		public static Vector3 Vector3ToDegree(Vector3 vec)
		{
			return new Vector3(
							MathHelper.ToDegrees(vec.X),
							MathHelper.ToDegrees(vec.Y),
							MathHelper.ToDegrees(vec.Z));
		}



		#region Extracted randomly from a selected number of

		#region Two argument
		public static float selectRandom2(float num1, float num2)
		{
			int randomNum = MyMathHelper.rand.Next(0, 2);

			switch (randomNum)
			{
				case 0: return num1;
				case 1: return num2;
				default: return 0.0f;
			}
		}
		#endregion

		#region Three arguments
		public static float selectRandom3(float num1, float num2, float num3)
		{
			int randomNum = MyMathHelper.rand.Next(0, 3);

			switch (randomNum)
			{
				case 0: return num1;
				case 1: return num2;
				case 2: return num3;
				default: return 0.0f;
			}
		}
		#endregion

		#region Four arguments
		public static float selectRandom4(float num1, float num2, float num3, float num4)
		{
			int randomNum = MyMathHelper.rand.Next(0, 4);

			switch (randomNum)
			{
				case 0: return num1;
				case 1: return num2;
				case 2: return num3;
				case 3: return num4;
				default: return 0.0f;
			}
		}
		#endregion

		#region Five arguments
		public static float selectRandom5(float num1, float num2, float num3, float num4, float num5)
		{
			int randomNum = MyMathHelper.rand.Next(0, 5);

			switch (randomNum)
			{
				case 0: return num1;
				case 1: return num2;
				case 2: return num3;
				case 3: return num4;
				case 4: return num5;
				default: return 0.0f;
			}
		}
		#endregion

		#endregion

		//----------------------------------------------//
		// 関数名	SetWorldMatrix						//
		//	Function name SetWorldMatrix
		// 機能		ワールドに渡すための行列を作成			//
		//	Create a matrix for which is passed to a function World
		// 引数		ポジション、スケール、回転			//
		//	Argument position, scale, rotation
		// 戻り値	作成した行列							//
		//	Matrix return value creation
		//----------------------------------------------//
		public static Matrix SetWorldMatrix(Vector3 pos, Vector3 scale, Vector3 rotate)
		{
			// Synthesis matrix
			Matrix mat = Matrix.Identity;

			// I will by multiplying each value
			mat *= Matrix.CreateScale(scale);
			mat *= Matrix.CreateRotationX(rotate.X);
			mat *= Matrix.CreateRotationY(rotate.Y);
			mat *= Matrix.CreateRotationZ(rotate.Z);
			mat *= Matrix.CreateTranslation(pos);

			// Returns a matrix that you created
			return mat;
		}

		//------------------------------------------------------//
		// 関数名	WorldToScreen								//
		//	Function name WorldToScreen
		// 機能		ワールド座標からスクリーン座標への変換			//
		//	Conversion to the screen coordinates from the world coordinate function
		// 引数		変換したいVector3							//
		//	Vector3 you want to convert argument
		// 戻り値	変換し終わったVector2						//
		//	Vector2 that you have finished return value conversion
		//------------------------------------------------------//
		public static Vector2 WorldToScreen(Vector3 pos)
		{
			// Declare viewport, initialization
			Viewport viewport = Game1.graphics.GraphicsDevice.Viewport;

			// Stored in Vector3 temporarily
			Vector3 a = viewport.Project(pos,
										 Game1.camera.projection,
										 Game1.camera.view,
										 Matrix.Identity);
			// I returned in Vector2
			return new Vector2(a.X,a.Y);
		}
	}

}
