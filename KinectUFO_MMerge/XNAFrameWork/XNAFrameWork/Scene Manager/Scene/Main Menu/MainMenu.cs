//--------------------------------------//
// MainMenu.cs							//
// メインメニューを管理するクラス			//
//	Class that manages the main menu
// 制作日:2013/10/21						//
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
using XNAFrameWork;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Kinect;
using Microsoft.Xna.Framework.Input;

namespace XNAFrameWork
{
	class MainMenu : IScene
	{
		#region field

		Cube mainMenuCube;							// Cube of the main
		Cube_State.CubeState cubeState;				// State of the cube
		Main_Menu_State.MainMenuState menuState;	// State of the main menu

		int skipCount = 0;

		#endregion

		// Initializing process
		public void Initialize()
		{
			// Initialization of the cube
			this.mainMenuCube = new Cube();

			// Initialization of camera
			Game1.camera.Pos = new Vector3(0.0f, 0.0f, 3.0f);
			Game1.camera.Look = new Vector3(0.0f, 0.0f, 0.0f);
			Game1.camera.Angle = 45.0f;

			// Initialization of the state of the cube
			cubeState = new Cube_State.Idle();
			// Initialization of the state of the menu
			menuState = new Main_Menu_State.Idle();
			this.mainMenuCube.Scale = Vector3.One * 0.5f;

		}

		// Graphic relationship reading
		public void LoadContent(ContentManager content)
		{
		}

		// Graphic destroyed
		public void UnLoadContent()
		{
		}

		// Update process
		public void Update(GameTime gametime)
		{
			if (skipCount > 60)
			{
				Game1.camera.PosZ = 10.0f;
				/*
				Game1.debugText.Printf("MainMenuScene", new Vector2(0, 30), Color.White);
				Game1.debugText.Printf(cubeState.ToString(),new Vector2(0,60));
				Game1.debugText.Printf(menuState.ToString(), new Vector2(0, 90));
				 * */

				mainMenuCube.Scale = Vector3.One * 0.5f;

				// If the idle state
				if (menuState is Main_Menu_State.Idle
					&& cubeState is Cube_State.Idle)
				{
					// Right When the input
					if (Game1.person.Left()
						|| MyKeyboard.IsPressed(Keys.Right))
					{
						// To the right of the cube rotation process
						this.cubeState = new Cube_State.Right();
					}
					// Left When the input
					else if (Game1.person.Right()
						|| MyKeyboard.IsPressed(Keys.Left))
					{
						// Left to the rotation of the cube processing
						this.cubeState = new Cube_State.Left();
					}
					// Top When the input
					else if (Game1.person.Down()
						|| MyKeyboard.IsPressed(Keys.Up))
					{
						// Up the rotation of the cube processing
						this.cubeState = new Cube_State.Up();
					}
					// Under When the input
					else if (Game1.person.Up()
						|| MyKeyboard.IsPressed(Keys.Down))
					{
						// Down the rotation of the cube processing
						this.cubeState = new Cube_State.Down();
					}
				}

				// State of the menu if idle
				// I call the rotation process of the cube
				if (this.menuState is Main_Menu_State.Idle
					&& this.cubeState.RotateCube(ref this.mainMenuCube))
				{
					// If process of moving the right if
					if (this.cubeState is Cube_State.Right)
					{
						// To game play
						this.menuState = new Main_Menu_State.GamePlay();
					}
					// If processing of the left if
					if (this.cubeState is Cube_State.Left)
					{
						// To options
						this.menuState = new Main_Menu_State.Option();
					}
					// If the above process if
					if (this.cubeState is Cube_State.Up)
					{
						// Score to browse
						this.menuState = new Main_Menu_State.Score();
					}
					// If moving process under if
					if (this.cubeState is Cube_State.Down)
					{
						// Staff credit
						this.menuState = new Main_Menu_State.StaffCredit();
					}
				}
				// I call the rotation process of each mode
				if (!(this.menuState is Main_Menu_State.Idle)
					&& this.menuState.CubeMovement(ref this.mainMenuCube))
				{
					// To the idle rotation is finished
					this.menuState = new Main_Menu_State.Idle();
					this.cubeState = new Cube_State.Idle();
				}
				// State of non-idle
				// Decision process when you are
				if (!(this.menuState is Main_Menu_State.Idle)
					&& MyKeyboard.IsPressed(Keys.Enter))
				{
					// I fly to each mode
					this.menuState.DecideScene();
				}
				// If you are determined when the idle
				if (this.menuState is Main_Menu_State.Idle
					&& Game1.person.Pushed())
				{
					// To game play
					SceneManager.nextScene = SceneManager.SCENE_TYPE.SAMPLE_SCENE1;
				}
			}
			else
			{
				skipCount++;
			}
		}

		// 2D drawing process
		public void Draw2D(GameTime gametime)
		{
		}

		// 3D drawing process
		public void Draw3D(GameTime gametime)
		{
			// Drawing of a cube
			this.mainMenuCube.Draw();
		}

	}
}
