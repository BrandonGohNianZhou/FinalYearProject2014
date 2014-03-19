// Sample scene

//----------------------//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAFrameWork;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Kinect;

namespace XNAFrameWork
{
	class SampleScene1 : IScene
	{
		#region Constant

		// Set the end of the stage
		//	const float STAGE_SIDE_LEFT = -50.0f;		// Left end
		//	const float STAGE_SIDE_RIGHT = 50.0f;		// Right end

		#endregion

		#region Field

		#region Object
		// Player
		private Player player;

		// Stage
		//private Stage stage;
		private StageV2.StageV2 stage;
		#endregion

		#region Countdown for

		// Countdown for sprite
		SpriteObject[] countdownNum = new SpriteObject[3];
		// Go!! Sprite
		SpriteObject go;
		// Count down for count
		int countdounCount = 180;
		int obstacleLastCreatedTime = 50;

		#endregion

		#region Shadow

		// Plate of poly shadow
		BoardTexture shadow;

		#endregion

		#region Result menu

		ResultMenu resultMenu;

		bool clearFlag = false;		// Whether or not cleared

		#endregion

        #region testing
        
        // skydome
        SkyDome Skydome;
        //Cube cube1;
        InstructionScene Instruction;
        // progress bar
        ProgressBar progressbar;

        // pause shakin animation
        float pause_angle;
        bool pause_left;

        #endregion

		#endregion

		#region Function

		//--------------------------//
		//	Function Initialize		//
		//	Initialization function	//
		//	No argument 			//
		//	No return value			//
		//--------------------------//
		float playerStartZ = 1000.0f;
		
		public void Initialize()
		{
			// Creating a player
			player = new Player(
						new Vector3(0.0f, 0.0f, playerStartZ),   // 1000
						new Vector3(2.0f, 2.0f, 2.0f),
						new Vector3(0.0f, 180.0f, 0.0f));

			// Creating a stage
			//stage = new Stage(player.Pos);
			stage = new StageV2.StageV2(player.PosZ);

			// The angle of view of the camera
			Game1.camera.Angle = 45.0f;

			// Initialization of the countdown
			this.InitCountDown();

			// Initialization of the wall
			this.InitShadow();

			// Instantiation of the result menu
			resultMenu = new ResultMenu();

            // testing
            Skydome = new SkyDome();
            Instruction = new InstructionScene();
            progressbar = new ProgressBar();
            pause_angle = 0.0f;
            pause_left = false;
		}

		//--------------------------------------//
		//	Function LoadContent				//
		//	Function relationship graphic read	//
		//	Argument content manager			//
		//	No return value						//
		//--------------------------------------//
		public void LoadContent(ContentManager content)
		{
			// Read the texture of shadow
			shadow.LoadTexture(TextureManager.GetInstance().GetTexture(TextureName.SHADOW));
		}

		//------------------------------//
		//	Function name UnLoadContent	//
		//	Function graphics abandoned	//
		//	No argument					//
		//	No return value				//
		//------------------------------//
		public void UnLoadContent()
		{

		}

		//--------------------------//
		//	Function Update			//
		//	Function update process	//
		//	No argument				//
		//	No return value			//
		//--------------------------//
		public void Update(GameTime gameTime)
		{
            // プレイヤーの更新
            player.Update(gameTime);

            if (!player.InitStartPos(gameTime))
            {
                Instruction.Update(gameTime);
                //return;
            }

            else if (Game1.paused)
            {
                //PauseAnimate();
                //Game1.pausetime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                //return;
            }

			// Start count reaches 0
            if (countdounCount < 0)
			{
				#region IfPlayerIsAlive
				if (LifePoint.LP > 0)
				{
					#region Winning Condition
					//	Winning Condition for old version
					//if (player.PosZ > -14000)
					//{
					//    // Control processing of player
					//    //player.ControllPlayer();

					//    // Control processing of the player using the skeleton data
					//    player.ControllPlayerWithSkeleton(gameTime);

					//    // Process of moving the player
					//    //player.MovingPlayer(gameTime);

					//}
					//else
					//{
					//    // win
					//    clearFlag = true;
					//    player.KickAnimate = false;
					//    resultMenu.animate_start = true;
					//    resultMenu.Victory = true;
					//    //Stage.damegeCount = 0;
					//}
					#endregion
				}
				#endregion
				#region IfPlayerIsDead
				else
                {
                    // lose
                    clearFlag = true;
                    player.KickAnimate = false;
                    resultMenu.animate_start = true;
                    resultMenu.Victory = false;
                    //Stage.damegeCount = 0;
				}
				#endregion

			}

			// Camera work
			this.CameraWork();
			
			player.ControllPlayerWithSkeleton(gameTime);

            // limit player position to the road side
            player.LimitPlayerPosition();
			
			// Countdown
			this.UpdateCountDown();

			//	Countdown to next obstacle
			

			// Update of shadow
			this.UpdateShadow();

            // skydome
            this.SkydomeMove();
            
            // progress bar
            this.progressbar.Update(player.PosZ);
			
			//check player touch power up/ collision with obstacles
			this.stage.Update(gameTime, ref player);
			
			// If the clear
			if (clearFlag)
			{
				// Update of the result menu
				resultMenu.Update();
			}
			
            if (!clearFlag && countdounCount < 0)
			{
				// Update of the stage
                //if (stage.Update(gameTime, player.Pos, player.Invincible, player.Vel, player.isDuck))
                //{
                //    // if player hit obstacle
                //    player.KnockBack(gameTime);
                //}
			}
		}

        void PauseAnimate()
        {
            // rotate left
            if (pause_left)
            {
                pause_angle += 0.02f;
                if (pause_angle > 0.25f)
                {
                    pause_left = false;
                }
            }
            // rotate right
            else
            {
                pause_angle -= 0.02f;
                if (pause_angle < -0.25f)
                {
                    pause_left = true;
                }
            }
        }

        void DrawPause()
        {
            // draw pause
            if (Game1.paused && Game1.pausetime > 0.5f)
            {
                //Game1.debugText.Printf("Please move to the center of the screen", new Vector2(Game1.graphics.GraphicsDevice.Viewport.Width*0.3f, Game1.graphics.GraphicsDevice.Viewport.Height*0.75f), new Color(1.0f, 0.0f, 0.0f));

                Game1.spriteBatch.Draw(
                                    TextureManager.GetInstance().GetTexture(TextureName.BLACK),
                                    new Vector2(Game1.graphics.GraphicsDevice.Viewport.Width * 0.5f,
                                                Game1.graphics.GraphicsDevice.Viewport.Height * 0.5f),
                                    new Rectangle(0, 0, 16, 16),
                                    Color.White * 0.5f,
                                    0.0f,
                                    new Vector2(8, 8),
                                    100.0f,
                                    SpriteEffects.None,
                                    1.0f);

                Game1.spriteBatch.Draw(
                                    TextureManager.GetInstance().GetTexture(TextureName.PAUSE_TEXT),
                                    new Vector2(Game1.graphics.GraphicsDevice.Viewport.Width * 0.5f,
                                                Game1.graphics.GraphicsDevice.Viewport.Height * 0.5f),
                                    new Rectangle(0, 0, 900, 200),
                                    Color.White,
                                    pause_angle,
                                    new Vector2(450, 100),
                                    1.0f,
                                    SpriteEffects.None,
                                    1.0f);
            }
        }

		//--------------------------//
		//	Function name Draw2D	//
		//	Function 2D drawing process
		//	No argument				//
		//	No return value			//
		//--------------------------//
		public void Draw2D(GameTime gameTime)
		{
            DebugText.GetInstance().Printf("T-Pose Time: " + player.T_Pose_Time.ToString(), new Vector2(400, 650));
            if (!player.tookstartpos)
            {
                Instruction.Draw2D();
                return;
            }

            // draw life
            player.Draw2D();
            //Game1.debugText.Printf("life:" + LifePoint.LP, new Vector2(650, 0));

            // draw score

			// Drawing of countdown
			this.DrawCountDown();

            // draw feedback
            //this.stage.Draw2D(clearFlag);
			stage.Draw2D();

            // draw progress bar
            this.progressbar.Draw2D();

            //Game1.debugText.Printf("" + Game1.graphics.GraphicsDevice.Viewport.Width, new Vector2(800, 450));
            //Game1.debugText.Printf("" + Game1.graphics.GraphicsDevice.Viewport.Height, new Vector2(800, 480));

			// If the clear
			if (clearFlag == true)
			{
				// Drawing of the result menu
                this.resultMenu.Draw();
                
                //Game1.debugText.Printf("YOUR    SCORE", new Vector2(Game1.graphics.GraphicsDevice.Viewport.Width * 0.45f, Game1.graphics.GraphicsDevice.Viewport.Height * 0.05f));
                //Game1.debugText.Printf(stage.score.score.ToString(), new Vector2(Game1.graphics.GraphicsDevice.Viewport.Width * 0.46f, Game1.graphics.GraphicsDevice.Viewport.Height * 0.075f));
			}

            // draw pause
            DrawPause();
		}

		//--------------------------//
		//	Function name Draw3D	//
		//	Function 3D drawing process
		//	No argument				//
		//	No return value			//
		//--------------------------//
		public void Draw3D(GameTime gameTime)
        {
            if (!player.tookstartpos)
            {
                return;
            }

            // skydome
            Skydome.Draw();

			// Drawing of shadow
            //shadow.Draw();

			// Drawing of the stage
            stage.Draw3D();


			// Drawing of player
            player.Draw3D();
        }

        // skydome rotation
        private void SkydomeMove()
        {
            Skydome.rotationY += Skydome.rotateSpeed;
            Skydome.PosZ = player.Pos.Z;
        }

		//------------------------------//
		//	Function name CameraWork
		//	Function camera work processing
		//	No argument
		//	No return value
		//------------------------------//
		private void CameraWork()
		{
			// To determine the position of the camera
			Game1.camera.PosX = player.PosX;
			Game1.camera.PosY = player.PosY + 10.0f;
			Game1.camera.PosZ = player.PosZ + 40.0f;//16.0f;


			// Fixation point of the camera
			Game1.camera.Look = new Vector3(0.0f, player.PosY, player.PosZ - 200.0f);
			if (MyKeyboard.IsPress(Keys.W))
			{
                Game1.camera.PosY = 15.0f;
                Game1.camera.PosZ = player.PosZ - 20.0f;
				Game1.camera.Look = new Vector3(0.0f, player.PosY, player.PosZ);
			}

            // testing
            if (MyKeyboard.IsPress(Keys.S))
            {
                Game1.camera.PosY = player.PosY;
                Game1.camera.PosZ = player.PosZ;
                Game1.camera.Look = new Vector3(0.0f, player.PosY, player.PosZ);
            }

            if (MyKeyboard.IsPress(Keys.A))
            {
                Game1.camera.PosX = -20.0f;
                Game1.camera.PosZ = player.PosZ;
                Game1.camera.Look = new Vector3(0.0f, player.PosY, player.PosZ);
            }

            if (MyKeyboard.IsPress(Keys.D))
            {
                Game1.camera.PosX = 20.0f;
                Game1.camera.PosZ = player.PosZ;
                Game1.camera.Look = new Vector3(0.0f, player.PosY, player.PosZ);
            }
			
			//HH Testing----------------------------------------------------------------------------
            if (MyKeyboard.IsPress(Keys.C))
            {
                stage.Powers[0].CreatePowerUp(player.Pos + new Vector3(0, 10, -50));
            }

            if (MyKeyboard.IsPress(Keys.X))
            {
                stage.Powers[0] = new PowerUp();
            }

            if (MyKeyboard.IsPress(Keys.V))
            {
                stage.Obstacles[0].CreateObstacle(player.Pos + new Vector3(0, 0, -50));
            }

            const float MOVE_STRENGTH = 1;

            if (MyKeyboard.IsPress(Keys.I))
            {
                //stageTest.Obstacles[0].Pos = Vector3.Zero;
                player.PosZ -= MOVE_STRENGTH;
            }
            if (MyKeyboard.IsPress(Keys.K))
            {
                player.PosZ += MOVE_STRENGTH;
            }

            if (MyKeyboard.IsPress(Keys.J))
            {
                player.PosX -= MOVE_STRENGTH;
            }
            if (MyKeyboard.IsPress(Keys.L))
            {
                player.PosX += MOVE_STRENGTH;
            }
            //HH Testing----------------------------------------------------------------------------

		}

		//----------------------------------------------------------//
		//	Function name VanishCountDown							//
		//	Function of character countdown fades away gradually	//
		//	Sprite object of argument countdown						//
		//	No return value											//
		//----------------------------------------------------------//
		private void VanishCountDown(SpriteObject num)
		{
			// Set the alpha
			num.alpha -= 1.0f / 60.0f;
			// Set the scale
			num.scale -= new Vector2(2.0f / 60.0f, 2.0f / 60.0f);
			// Set the angle
			num.angle += 0.1f;
		}

		//----------------------------------------------//
		//	Function name VanishGo						//
		//	Character of the function Go fades gradually//
		//	No argument									//
		//	No return value								//
		//----------------------------------------------//
		private void VanishGo()
		{
			// Small amount of growth
			go.scale += Vector2.One * 0.1f;
			// Transparent little by little
			go.alpha -= 1.0f / 60.0f;
		}

		//--------------------------------------//
		//	Function name UpdateObstacleLastCreatedTime
		//	Update process of creating another obstacle
		//	No argument
		//	No return value
		//--------------------------------------//
		void UpdateObstacleLastCreatedTime()
		{
			obstacleLastCreatedTime--;
			if (obstacleLastCreatedTime < 1)
			{
				stage.Obstacles[0].CreateObstacle(player.Pos + new Vector3(0, 0, -50));
				obstacleLastCreatedTime = 50;
			}
		}

		//--------------------------------------//
		//	Function name UpdateCountDown		//
		//	Update process of the function countdown
		//	No argument							//
		//	No return value						//
		//--------------------------------------//
		private void UpdateCountDown()
		{
            // testing
            if (!player.tookstartpos) return;

			// I will reduce the count
			countdounCount--;
			// Drawing of countdown
			if (countdounCount < 1 && countdounCount > -60)
			{
				this.VanishGo();
                 player.KickAnimate = true;
			}
			// 1
			else if (countdounCount < 60 && countdounCount > 0)
			{
				this.VanishCountDown(countdownNum[0]);
			}
			// 2
			else if (countdounCount > 61 && countdounCount < 120)
			{
				this.VanishCountDown(countdownNum[1]);
			}
			// 3
			else if (countdounCount > 121)
			{
				this.VanishCountDown(countdownNum[2]);
			}
		}

		//----------------------------------//
		//	Function name DrawCountDown		//
		//	Draw function countdown			//
		//	No argument						//
		//	No return value					//
		//----------------------------------//
		private void DrawCountDown()
		{
			// Drawing of countdown
			// Go!!
			if (countdounCount < 1 && countdounCount > -60)
			{
				Game1.spriteBatch.Draw(
									TextureManager.GetInstance().GetTexture(TextureName.GO),
									go.pos,
                                    new Rectangle(0, 0, 237, 130),
									Color.White * go.alpha,
									go.angle,
									new Vector2(118, 65),
									go.scale,
									SpriteEffects.None,
									1.0f
									);
			}
			// 1
			else if (countdounCount < 60 && countdounCount > 0)
			{
				Game1.spriteBatch.Draw(
									TextureManager.GetInstance().GetTexture(TextureName.FINAL_SCORE_1),
									countdownNum[0].pos,
                                    new Rectangle(0, 0, 70, 70),
									Color.White * countdownNum[0].alpha,
                                    countdownNum[0].angle,
                                    new Vector2(35, 35),
									countdownNum[0].scale,
									SpriteEffects.None,
									1.0f
									);
			}
			// 2
			else if (countdounCount > 61 && countdounCount < 120)
			{
				Game1.spriteBatch.Draw(
									TextureManager.GetInstance().GetTexture(TextureName.FINAL_SCORE_2),
                                    countdownNum[1].pos,
                                    new Rectangle(0, 0, 70, 70),
									Color.White * countdownNum[1].alpha,
                                    countdownNum[1].angle,
                                    new Vector2(35, 35),
									countdownNum[1].scale,
									SpriteEffects.None,
									1.0f
									);
			}
			// 3
			else if (countdounCount > 121)
			{
				Game1.spriteBatch.Draw(
									TextureManager.GetInstance().GetTexture(TextureName.FINAL_SCORE_3),
                                    countdownNum[2].pos,
                                    new Rectangle(0, 0, 70, 70),
									Color.White * countdownNum[2].alpha,
                                    countdownNum[2].angle,
                                    new Vector2(35, 35),
									countdownNum[2].scale,
									SpriteEffects.None,
									1.0f
									);
			}
		}

		//------------------------------------------//
		//	Function name InitCountDown				//
		//	Initialization function countdown variable
		//	No return value							//
		//	No argument								//
		//------------------------------------------//
		private void InitCountDown()
		{
			// Initialization of the countdown for sprite
			for (int i = 0; i < countdownNum.Length; i++)
			{
				// Initialization
				countdownNum[i] = new SpriteObject();
				// Position is the middle of the screen
				countdownNum[i].pos = new Vector2(Game1.graphics.GraphicsDevice.Viewport.Width * 0.5f,
												  Game1.graphics.GraphicsDevice.Viewport.Height * 0.5f);
				// Scale
				countdownNum[i].scale = Vector2.One * 3.0f;
			}
			// Initialization of the characters in the Go
			go = new SpriteObject();
			// The middle of the screen
			go.pos = new Vector2(Game1.graphics.GraphicsDevice.Viewport.Width * 0.5f,
								 Game1.graphics.GraphicsDevice.Viewport.Height * 0.5f);
			// スケール
			go.scale = Vector2.One * 3.0f;
		}

		//--------------------------//
		//	Function name InitShadow//
		//	Initialization function of shadow
		//	No argument				//
		//	No return value			//
		//--------------------------//
		private void InitShadow()
		{
			// Instantiation of shadow
			shadow = new BoardTexture();
			// Initialization of shadow
			shadow.Pos = new Vector3(player.PosX, -1.0f, player.PosZ);
			shadow.Scale = Vector3.One * 2.0f;
			shadow.rotationX = -90.0f;
			shadow.alpha = 0.5f;
		}

		//------------------------------//
		//	Function name UpdateShadow	//
		//	Update function shadow		//
		//	No argument					//
		//	No return value				//
		//------------------------------//
		private void UpdateShadow()
		{
			// Sync players and the position of the shadow
			shadow.Pos = new Vector3(player.PosX, -1.9f, player.PosZ);
			// The fine-tuning by the height of the player the scale of the shadow
			shadow.Scale = (2 + player.PosY * 0.2f) * Vector3.One;
		}

		#endregion
	}
}
