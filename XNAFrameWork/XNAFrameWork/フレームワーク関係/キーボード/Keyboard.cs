//----------------------------------//
// Keyboard.cs						//
// キーボードを使うためのクラス		//
//	Class for using the keyboard
// 作成日:2013/09/26					//
// 作成者:Shin Kouno					//
//----------------------------------//


//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;



namespace XNAFrameWork
{
	class MyKeyboard
	{
		// Variable that contains the state of the keyboard
		static private KeyboardState keyState;
		// Variable that contains the key information that was pressed last time
		static private KeyboardState oldKeyState;

		// Constructor
		public MyKeyboard()
		{
			// Initialize keyState
			keyState = Keyboard.GetState();
		}

		//--------------------------------------//
		// 関数名	Update						//
		//	Function Update
		// 機能		キーボード情報の更新			//
		//	Update function keyboard information
		// 引数		なし							//
		//	No argument
		// 戻り値	なし							//
		//	No return value
		//--------------------------------------//
		static public void Update()
		{
			// I remember the key that was pressed last time
			oldKeyState = keyState;
			// Get the state of the keyboard
			keyState = Keyboard.GetState();
		}

		//--------------------------------------------------//
		// 関数名	IsPress									//
		//	Function name IsPress
		// 機能		そのキーが押されているかどうか調べる		//
		//	I find out if the key is whether the function is pressed
		// 引数		キーコード								//
		//	Argument key code
		// 戻り値	押されている	: true						//
		//	Is pressed return value: true
		//			押されていない	: fale					//
		//	It is not pressed: false
		//--------------------------------------------------//
		static public bool IsPress(Keys key)
		{
			// Returns the false true, if not pressed if pressed
			return keyState.IsKeyDown(key);
		}

		//--------------------------------------------------//
		// 関数名	IsPressed								//
		//	Function name IsPressed
		// 機能		そのキーが押されたかどうか(トリガー)		//
		//	The key is whether or not pressed function(trigger)
		// 引数		キーコード								//
		//	Argument key code
		// 戻り値	押された		: true						//
		//	Pressed return value: true
		//			押されていない	: false					//
		//	It is not pressed: false
		//--------------------------------------------------//
		static public bool IsPressed(Keys key)
		{
			// If key is pressed
			if (keyState.IsKeyDown(key))
			{
				// Is this different from a key that was pressed before
				if (oldKeyState != keyState)
				{
					// I return true
					return true;
				}
			}
			// I return false trigger processing is not pass
			return false;
		}

		//----------------------------------------------//
		// 関数名	IsRelease							//
		//	Function name IsRelease
		// 機能		そのキーが離されているかどうか			//
		//	The key is whether the isolated function
		// 引数		キーコード							//
		//	Argument key code
		// 戻り値	押されていない	: true				//
		//	Not pressed return value: true
		//			押されている	: false					//
		//	It is pressed: false
		//----------------------------------------------//
		static public bool IsRelease(Keys key)
		{
			// False true, if the pressed if it is released
			return keyState.IsKeyUp(key);
		}

		//------------------------------------------//
		// 関数名	IsReleased						//
		//	Function name IsReleased
		// 機能		そのキーが離されたかどうか		//
		//	The key is whether the function is released
		// 引数		キーコード						//
		//	Argument key code
		// 戻り値	離された		: true				//
		//	It was released return value: true
		//			離されていない	: false			//
		//	It is not released: false
		//------------------------------------------//
		static public bool IsReleased(Keys key)
		{
			// The key is pressed by mistake?
			if (keyState.IsKeyUp(key))
			{
				// The button was down last time
				if (oldKeyState.IsKeyDown(key))
				{
					// I return true
					return true;
				}
			}
			// Released process if no pass
			return false;
		}
	}
}
