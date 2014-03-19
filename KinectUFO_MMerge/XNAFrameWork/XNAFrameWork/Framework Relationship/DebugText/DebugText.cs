//------------------------------//
// DebugText.cs					//
// デバッグテキストの表示			//
//	Display of debug text
// 作成日:2013/09/26				//
// 作成者:Shin Kouno				//
//------------------------------//


//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace XNAFrameWork
{
	public class DebugText
	{
		#region Structure declaration

		// What you are when you print
		private struct DrawStringInfo
		{
			public readonly string text;		// Text to be displayed
			public readonly Vector2 pos;		// Location where you want to display
			public readonly Color color;		// Color to be displayed

			// Constructor
			public DrawStringInfo(string text, Vector2 pos, Color requestColor)
			{
				// To assign a value that we have got their own
				this.text	= text;
				this.pos	= pos;
				this.color	= requestColor;
			}
		}
		#endregion

		#region Field

		private static DebugText debugDraw = null;
		private static SpriteFont debugFont = null;
		private List<DrawStringInfo> DrawList = new List<DrawStringInfo>();

		#endregion

		#region Constructor

		// Private constructor
		// I prevent the instantiation
		private DebugText() { }

		#endregion

		#region 関数

		//----------------------------------------------//
		// 関数名	GetInstance							//
		//	Function GetInstance
		// 機能		debugTextのインスタンスを取得			//
		//	Get the instance of the function debugText
		// 引数		なし									//
		//	No argument
		// 戻り値	debugText							//
		//	Returns debugText
		//----------------------------------------------//
		public static DebugText GetInstance()
		{
			Debug.Assert((null != debugDraw), "Initメソッドが呼ばれていません。");
			return debugDraw;
		}

		//------------------------------------------//
		// 関数名	Init							//
		//	Function Init
		// 機能		デバッグテキストの初期化			//
		//	Initialization of function debug text
		// 引数		使用するフォント					//
		//	Font to use argument
		// 戻り値	なし								//
		//	No return value
		//------------------------------------------//
		public static void Init(SpriteFont font)
		{
			// Create a draw
			debugDraw = new DebugText();
			// Set the font
			debugFont = font;
		}

		//--------------------------------------//
		// 関数名	Printf						//
		//	Function Printf
		// 機能		デバッグテキストの追加		//
		//	Add function debug text
		// 引数		描画する文字、場所			//
		//	Character to be drawn argument, place
		// 戻り値	なし							//
		//	No return value
		//--------------------------------------//
		public void Printf(string text, Vector2 pos)
		{
			// I call the Printf function of upward-compatible
			this.Printf(text,pos,Color.White);
		}

		//----------------------------------------------//
		// 関数名	Printf								//
		//	Function Printf
		// 機能		デバッグテキストの追加(上位互換)		//
		//	Add debug function text (upward-compatible)
		// 引数		描画する文字、場所、色				//
		//	Character to be drawn argument, location, color
		// 戻り値	なし									//
		//	No return value
		//----------------------------------------------//
		public void Printf(string text, Vector2 pos, Color requestColor)
		{
			// Add any of the following characters to draw list
			DrawList.Add(new DrawStringInfo(text, pos, requestColor));
		}

		//--------------------------------------//
		// 関数名	DebugString					//
		//	Function name DebugString
		// 機能		実際にテキストを描画する		//
		//	Draw the text atually function
		// 引数		スプライトバッチ				//
		//	Argument sprite batch
		// 戻り値	なし							//
		//	No return value
		//--------------------------------------//
		public void DebugString(SpriteBatch sprite)
		{
			// Drawing the start of the sprite
			sprite.Begin();

			// I turn the characters are stored
			foreach (DrawStringInfo obj in DrawList)
				sprite.DrawString(debugFont, obj.text, obj.pos, obj.color);

			// Clear draw list
			DrawList.Clear();

			// End of Draw a Sprite
			sprite.End();
		}

		#endregion
	}
}