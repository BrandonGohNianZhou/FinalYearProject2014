//----------------------//
// SpriteObject.cs		//
// スプライト用データ		//
//	Sprite data
// 制作日:2013/10/23		//
// 制作者:Kouno Shin		//
//----------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAFrameWork
{
	// Class that contains the value to be used well
	class SpriteObject
	{
		#region Field

		public Vector2 pos;		// Position
		public Vector2 scale;	// Scale
		public float angle;		// Angle
		public float alpha;		// Alpha

		#endregion

		#region Constructor

		public SpriteObject()
		{
			// Initialize each value
			this.pos = Vector2.Zero;
			this.scale = Vector2.One;
			this.angle = 0.0f;
			this.alpha = 1;
		}

		#endregion
	}
}
