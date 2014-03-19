//------------------------------------------//
// IObject.cs								//
// 全てのオブジェクトの基本となるクラス		//
//	The underlying class of all objects
// 制作日:2013/10/02							//	
// 制作者:Kouno Shin							//
//------------------------------------------//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace XNAFrameWork
{
	// Object interface
	interface IObject
	{
		#region Position
		// Get and set position
		Vector3 Pos { get;set;}

		// Get and set the X position
		float PosX { get;set;}
		// Get and set the Y position
		float PosY { get;set;}
		// Get and set the Z position
		float PosZ { get;set; }

		#endregion

		#region Scale
		// Get and set the scale
		Vector3 Scale { get;set;}

		// Get and set the X position of the scale
		float scaleX { get;set;}
		// Get and set the Y position of the scale
		float scaleY { get;set;}
		// Get and set the Z position of the scale
		float scaleZ { get;set; }

		#endregion

		#region Rotation
		// Get and set the rotation
		Vector3 Rotation { get;set;}

		// Get and set the X-axis rotation
		float rotationX { get;set;}
		// Get and set the Y-axis rotation
		float rotationY { get;set;}
		// Get and set the Z-axis rotation
		float rotationZ { get;set; }

		#endregion

	}
}
