//------------------------------------------------------//
// HandPointer.cs										//
// プレイヤーの手の位置にポインタを表示するクラス			//
//	A class that displays the pointer at the position of the player's hand
// 制作日:2013/11/13										//
// 制作者:Kouno Shin										//
//------------------------------------------------------//

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
using Microsoft.Kinect;

namespace XNAFrameWork
{
	class HandPointer
	{
		#region Field

		// The current position of the hand pointer
		public Vector2 pos;

		// Offset of the hand pointer
		public Vector2 offset;

		// Data of the hand pointer
		private SpriteObject handPointer;

		#endregion

		#region Constructor

		public HandPointer()
		{
			// Instance of the data
			this.handPointer = new SpriteObject();

			// Initializing the value
			this.pos = Vector2.Zero;
			this.offset = new Vector2(16, 16) * handPointer.scale;
		}

		#endregion

		#region 関数


		//--------------------------//
		// 関数名	Update			//
		//	Function Update
		// 機能		更新処理			//
		//	Function update process
		// 引数		なし				//
		//	No argument
		// 戻り値	なし				//
		//	No return value
		//--------------------------//
		public void Update()
		{
			// Get limit drawing point of the screen
			Vector2 limit = new Vector2(Game1.graphics.GraphicsDevice.Viewport.Width,
										Game1.graphics.GraphicsDevice.Viewport.Height);

			// I synchronize the position of the hand pointer and the position of the hand of the player
			this.handPointer.pos = new Vector2(Game1.person.GetJointPos(JointType.HandRight).X * 500.0f + limit.X * 0.5f,
											   Game1.person.GetJointPos(JointType.HandRight).Y * -500.0f + limit.Y * 0.5f);

			// Set so as not to go off off-screen
			this.LimitControll(limit);

			// Save the coordinates of the position in 2D
			this.pos = this.handPointer.pos;

			// Update of the offset
			this.offset = new Vector2(16, 16) * handPointer.scale;
		}

		//----------------------------------------------------------//
		// 関数名	LimitControll									//
		//	Function name LimitControll
		// 機能		ハンドポインターが画面外に出ていかないように		//
		//	Function pointer hand so as not to go off off-screen
		//			コントロールする									//
		//	I want to control
		// 引数		リミットのVector2								//
		//	Vector2 limit of argument
		// 戻り値	なし												//
		//	No return value
		//----------------------------------------------------------//
		private void LimitControll(Vector2 limit)
		{
			// When the pointer hand gone off the screen
			// I fit in the screen
			// Side
			if (this.handPointer.pos.X > limit.X)
			{
				this.handPointer.pos.X = limit.X;
			}
			else if (this.handPointer.pos.X < 0.0)
			{
				this.handPointer.pos.X = 0.0f;
			}
			// Length
			if (this.handPointer.pos.Y > limit.Y)
			{
				this.handPointer.pos.Y = limit.Y;
			}
			else if (this.handPointer.pos.Y < 0.0)
			{
				this.handPointer.pos.Y = 0.0f;
			}
		}

		

		//----------------------------------//
		// 関数名	Draw					//
		//	Function Draw
		// 機能		ハンドポインタの描画		//
		//	Drawing of hand function pointer
		// 引数		なし						//
		//	No argument
		// 戻り値	なし						//
		//	No return value
		//----------------------------------//
		public void Draw()
		{
			// Drawing of hand pointer
            //Game1.spriteBatch.Draw(
            //                    TextureManager.GetInstance().GetTexture(TextureName.HAND_POINTER),
            //                    this.handPointer.pos,
            //                    new Rectangle(0,0,32,32),
            //                    Color.White * this.handPointer.alpha,
            //                    this.handPointer.angle,
            //                    new Vector2(16,16),
            //                    this.handPointer.scale,
            //                    SpriteEffects.None,
            //                    0.0f);

            // new hand pointer
            Game1.spriteBatch.Draw(
                                TextureManager.GetInstance().GetTexture(TextureName.HAND_POINTER),
                                this.handPointer.pos,
                                new Rectangle(0, 0, 110, 110),
                                Color.White * this.handPointer.alpha,
                                this.handPointer.angle,
                                new Vector2(55, 55),
                                this.handPointer.scale,
                                SpriteEffects.None,
                                0.0f);
		}

		
		#endregion
	}
}
