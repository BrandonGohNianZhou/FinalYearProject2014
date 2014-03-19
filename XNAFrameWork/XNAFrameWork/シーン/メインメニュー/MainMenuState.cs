//------------------------------------------//
// MainMenuState							//
// メインメニューの状態を管理するクラス		//
//	Class that manages the state of the main menu
// 制作日:2013/10/21							//
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
using Microsoft.Xna.Framework.Input;

namespace XNAFrameWork
{
	namespace Main_Menu_State
	{
		#region Interface

		// Interface class that represents the state should have
		interface MainMenuState
		{
			// Processing the cube is moving
			bool CubeMovement(ref Cube cube);
			// Process when the decision was pressed
			void DecideScene();
		}

		#endregion

		#region Idol

		class Idle : MainMenuState
		{
			// (Nothing) processing the cube is moving
			public bool CubeMovement(ref Cube cube)
			{
				return true;
			}

			// Process when the decision was pressed
			public void DecideScene()
			{
				// It is not nothing
			}
		}

		#endregion

		#region Game play

		class GamePlay : MainMenuState
		{
			#region Field

			bool duringRotation;		// Whether rotating

			#endregion

			#region Constructor

			public GamePlay()
			{
				// Non-rotating
				this.duringRotation = false;
			}

			#endregion

			#region Function
			//--------------------------------------------------//
			// 関数名	CubeMovement							//
			//	Function name CubeMovement
			// 機能		ゲームプレイからアイドルへの移動処理		//
			//	Move to the idel processing function from game play
			// 引数		回転させたいキューブ						//
			//	Cube you want to arugment rotation
			// 戻り値	回転中		true						//
			//	Returns rotating true
			//			回転終了	false							//
			//	Rotate the end false
			//--------------------------------------------------//
			public bool CubeMovement(ref Cube cube)
			{
				// When the processing back is pressed
				if (Game1.person.Right()
					&& this.duringRotation == false)
				{
					// Transition to rotating
					this.duringRotation = true;
				}

				// In rotating mode
				// Move if it is not over yet
				if (cube.rotationY > 0.0
					&& this.duringRotation == true)
				{
					// I rotate the cube
					cube.rotationY -= cube.rotateSpeed;
					// When the cube overkill if
					if (cube.rotationY <= 0.0)
					{
						// I will fix the cube
						cube.rotationY = 0.0f;
						// Rotate the end
						this.duringRotation = false;
						// I inform the rotation end
						return true;
					}
				}
				// Rotation is not over yet
				return false;
			}


			//--------------------------------------------------//
			// 関数名	DecideScene								//
			//	Function name DecideScene
			// 機能		決定の処理がされたとき各シーンにとぶ		//
			//	I fly to the scene when the decision process of the function is
			// 引数		なし										//
			//	No argument
			// 戻り値	なし										//
			//	No return value
			//--------------------------------------------------//
			public void DecideScene()
			{
				// I fly in game play
				SceneManager.nextScene = SceneManager.SCENE_TYPE.SAMPLE_SCENE1;
			}
			#endregion
		}

		#endregion

		#region Option

		class Option : MainMenuState
		{
			#region Field

			bool duringRotation;		// Whether rotating

			#endregion

			#region Constructor

			public Option()
			{
				// Non-rotating
				this.duringRotation = false;
			}

			#endregion

			#region Function
			//--------------------------------------------------//
			// 関数名	CubeMovement							//
			//	Function name CubeMovement
			// 機能		オプションからアイドルへの移動処理			//
			//	Process of moving from idle to function option
			// 引数		回転させたいキューブ						//
			//	Cube you want to argument rotation
			// 戻り値	回転中		true						//
			//	Returns rotating true
			//			回転終了	false							//
			//	Rotate the end false
			//--------------------------------------------------//
			public bool CubeMovement(ref Cube cube)
			{
				// When the processing back is pressed
				if (Game1.person.Left()
					&& this.duringRotation == false)
				{
					// Transition to rotating
					this.duringRotation = true;
				}

				// In rotating mode
				// Move if it is not over yet
				if (cube.rotationY < 0.0
					&& this.duringRotation == true)
				{
					// I rotate the cube
					cube.rotationY += cube.rotateSpeed;
					// When the cube overkill if
					if (cube.rotationY >= 0.0)
					{
						// I will fix the cube
						cube.rotationY = 0.0f;
						// Rotate the end
						this.duringRotation = false;
						// I inform the rotation end
						return true;
					}
				}
				// Rotation is not over yet
				return false;
			}


			//--------------------------------------------------//
			// 関数名	DecideScene								//
			//	Function name DecideScene
			// 機能		決定の処理がされたとき各シーンにとぶ		//
			//	I fly to the scene when the decision process of the function is
			// 引数		なし										//
			//	No argument
			// 戻り値	なし										//
			//	No return value
			//--------------------------------------------------//
			public void DecideScene()
			{
				// I fly in options
				SceneManager.nextScene = SceneManager.SCENE_TYPE.SAMPLE_SCENE1;
			}
			#endregion
		}

		#endregion

		#region View score

		class Score : MainMenuState
		{
			#region Field

			bool duringRotation;		// Whether rotating

			#endregion 

			#region Constructor

			public Score()
			{
				// Non-rotating
				this.duringRotation = false;
			}

			#endregion

			#region Function
			//--------------------------------------------------//
			// 関数名	CubeMovement							//
			//	Function name CubeMovement
			// 機能		スコアからアイドルへの移動処理				//
			//	Process of moving from idle to function score
			// 引数		回転させたいキューブ						//
			//	Cube you want to argument rotation
			// 戻り値	回転中		true						//
			//	Returns rotating true
			//			回転終了	false							//
			//	Rotate the end false
			//--------------------------------------------------//
			public bool CubeMovement(ref Cube cube)
			{
				// When the processing back is pressed
				if (Game1.person.Up()
					&& this.duringRotation == false)
				{
					// Transition to rotating
					this.duringRotation = true;
				}

				// In rotating mode
				// Move if it is not over yet
				if (cube.rotationX < 0.0
					&& this.duringRotation == true)
				{
					// I rotate the cube
					cube.rotationX += cube.rotateSpeed;
					// When the cube overkill if
					if (cube.rotationX >= 0.0)
					{
						// I will fix the cube
						cube.rotationX = 0.0f;
						// Rotate the end
						this.duringRotation = false;
						// I inform the rotation end
						return true;
					}
				}
				// Rotation is not over yet
				return false;
			}
			

			//--------------------------------------------------//
			// 関数名	DecideScene								//
			//	Function name DecideScene
			// 機能		決定の処理がされたとき各シーンにとぶ		//
			//	I fly to the scene when the decision process of the function is
			// 引数		なし										//
			//	No argument
			// 戻り値	なし										//
			//	No return value
			//--------------------------------------------------//
			public void DecideScene()
			{
				// I fly to score browse
				SceneManager.nextScene = SceneManager.SCENE_TYPE.SAMPLE_SCENE1;
			}
			#endregion
		}

		#endregion

		#region Staff Credit

		// Credit
		class StaffCredit : MainMenuState
		{
			#region Field

			bool duringRotation;		// Whether rotating

			#endregion 

			#region Constructor

			public StaffCredit()
			{
				// Non-rotating
				this.duringRotation = false;
			}

			#endregion

			#region Function
			//--------------------------------------------------//
			// 関数名	CubeMovement							//
			//	Function name CubeMovement
			// 機能		クレジットからアイドルへの移動処理			//
			//	Move to the idle processing from credit function
			// 引数		回転させたいキューブ						//
			//	Cube you want to argument rotation
			// 戻り値	回転中		true						//
			//	Returns rotating true
			//			回転終了	false							//
			//	Rotate the end false
			//--------------------------------------------------//
			public bool CubeMovement(ref Cube cube)
			{
				// When the processing back is pressed
				if (Game1.person.Down()
					&& this.duringRotation == false)
				{
					// Transition to rotating
					this.duringRotation = true;
				}

				// In rotating mode
				// Move if it is not over yet
				if (cube.rotationX > 0.0
					&& this.duringRotation == true)
				{
					// I rotate the cube
					cube.rotationX -= cube.rotateSpeed;
					// When the cube overkill if
					if (cube.rotationX <= 0.0)
					{
						// I will fix the cube
						cube.rotationX = 0.0f;
						// Rotate the end
						this.duringRotation = false;
						// I inform the rotation end
						return true;
					}
				}
				// Rotation is not over yet
				return false;
			}
			

			//--------------------------------------------------//
			// 関数名	DecideScene								//
			//	Function name DecideScene
			// 機能		決定の処理がされたとき各シーンにとぶ		//
			//	I fly to the scene when the decision process of the function is
			// 引数		なし										//
			//	No argument
			// 戻り値	なし										//
			//	No return value
			//--------------------------------------------------//
			public void DecideScene()
			{
				// I fly to the staff credit
				SceneManager.nextScene = SceneManager.SCENE_TYPE.SAMPLE_SCENE1;
			}
			#endregion
		}
		#endregion
		
	}
}
