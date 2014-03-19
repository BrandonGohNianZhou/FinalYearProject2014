// Sample scene

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
		private Stage stage;
		#endregion

		#region Countdown for

		// Countdown for sprite
		SpriteObject[] countdownNum = new SpriteObject[3];
		// Go!! Sprite
		SpriteObject go;
		// Count down for count
		int countdounCount = 180;

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
		// 関数名	Initialize		//
		//	Function Initialize
		// 機能		初期化処理		//
		//	Initialization function
		// 引数		なし				//
		//	No argument 
		// 戻り値	なし				//
		//	No return value
		//--------------------------//
		public void Initialize()
		{
			// Creating a player
			player = new Player(
						new Vector3(0.0f, 0.0f, 1000.0f),   // 1000
						new Vector3(2.0f, 2.0f, 2.0f),
						new Vector3(0.0f, 180.0f, 0.0f));

			// Creating a stage
			stage = new Stage(player.Pos);

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
		// 関数名	LoadContent					//
		//	Function LoadContent
		// 機能		グラフィック関係読み込み		//
		//	Function relationship graphic read
		// 引数		コンテンツマネージャー		//
		//	Argument content manager
		// 戻り値	なし							//
		//	No return value
		//--------------------------------------//
		public void LoadContent(ContentManager content)
		{
			// Read the texture of shadow
			shadow.LoadTexture(TextureManager.GetInstance().GetTexture(TextureName.SHADOW));
		}

		//------------------------------//
		// 関数名	UnLoadContent		//
		//	Function name UnLoadContent
		// 機能		グラフィック破棄		//
		//	Function graphics abandoned
		// 引数		なし					//
		//	No argument
		// 戻り値	なし					//
		//	No return value
		//------------------------------//
		public void UnLoadContent()
		{

		}

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
		public void Update(GameTime gameTime)
		{
            // testing
            if (Game1.paused)
            {
                PauseAnimate();
                Game1.pausetime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                return;
            } 
            if (MyKeyboard.IsPress(Keys.P))
            {
                Game1.paused = !Game1.paused;
            }
            if (MyKeyboard.IsPress(Keys.O))
            {
                Game1.paused = false;
            }

			// Start count reaches 0
            if (countdounCount < 0)
            {
                if (LifePoint.LP > 0)
                {
                    if (player.PosZ > -14000)
                    {
						// Control processing of player
                        //player.ControllPlayer();

						// Control processing of the player using the skeleton data
                        player.ControllPlayerWithSkeleton(gameTime);

						// Process of moving the player
                        player.MovingPlayer();

                    }
                    else
                    {
                        // win
                        clearFlag = true;
                        player.KickAnimate = false;
                        resultMenu.animate_start = true;
                        resultMenu.Victory = true;
                        Stage.damegeCount = 0;
                    }
                }
                else
                {
                    // lose
                    clearFlag = true;
                    player.KickAnimate = false;
                    resultMenu.animate_start = true;
                    resultMenu.Victory = false;
                    Stage.damegeCount = 0;
                }

            }


			// Update of player
            player.Update(gameTime);
            // testing
            if (!player.InitStartPos())
            {
                Instruction.Update(gameTime);
                return;
            }

            // limit player position to the road side
            player.LimitPlayerPosition();

			// Camera work
			this.CameraWork();

			// Countdown
			this.UpdateCountDown();

			// Update of shadow
			this.UpdateShadow();

            // skydome
            this.SkydomeMove();
            
            // progress bar
            this.progressbar.Update(player.PosZ);

            //Game1.debugText.Printf("CX:" + Game1.person.GetJointPos(JointType.HipCenter).X, new Vector2(200, 225));
            //Game1.debugText.Printf("min x:" + player.minx + " max x:" + player.maxx, new Vector2(200, 250));
            //Game1.debugText.Printf("Model x:" + player.PosX + " y:" + player.PosY + " z:" + player.PosZ, new Vector2(200, 300));
            //Game1.debugText.Printf("Skele Start x:" + player.startpos.X + " y:" + player.startpos.Y + " z:" + player.startpos.Z, new Vector2(200, 300));

			// If the clear
			if (clearFlag)
			{
				// Update of the result menu
				resultMenu.Update();
			}
			
            if (!clearFlag && countdounCount < 0)
			{
				// Update of the stage
                if (stage.Update(gameTime, player.Pos, player.Invincible, player.Vel, player.isDuck))
                {
                    // if player hit obstacle
                    player.KnockBack(gameTime);
                }
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
		// 関数名	Draw2D			//
		//	Function name Draw2D
		// 機能		2D描画処理		//	
		//	Function 2D drawing process
		// 引数		なし				//
		//	No argument
		// 戻り値	なし				//
		//	No return value
		//--------------------------//
		public void Draw2D(GameTime gameTime)
		{
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
            this.stage.Draw2D(clearFlag);

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
		// 関数名	Draw3D			//
		//	Function name Draw3D
		// 機能		3D描画処理		//
		//	Function 3D drawing process
		// 引数		なし				//
		//	No argument
		// 戻り値	なし				//
		//	No return value
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
		// 関数名	CameraWork			//
		//	Function name CameraWork
		// 機能		カメラワーク処理		//
		//	Function camera work processing
		// 引数		なし					//
		//	No argument
		// 戻り値	なし					//
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
			
		}

		//----------------------------------------------------------//
		// 関数名	VanishCountDown									//
		//	Function name VanishCountDown
		// 機能		カウントダウンの文字がだんだんと消えていく			//
		//	Function of character countdown fades away gradually
		// 引数		カウントダウンのスプライトオブジェクト				//
		//	Sprite object of argument countdown
		// 戻り値	なし												//
		//	No return value
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
		// 関数名	VanishGo							//
		//	Function name VanishGo
		// 機能		Goの文字がだんだんと消えていく		//
		//	Character of the function Go fades gradually
		// 引数		なし									//
		//	No argument
		// 戻り値	なし									//
		//	No return value
		//----------------------------------------------//
		private void VanishGo()
		{
			// Small amount of growth
			go.scale += Vector2.One * 0.1f;
			// Transparent little by little
			go.alpha -= 1.0f / 60.0f;
		}

		//--------------------------------------//
		// 関数名	UpdateCountDown				//
		//	Function name UpdateCountDown
		// 機能		カウントダウンの更新処理		//
		//	Update process of the function countdown
		// 引数		なし							//
		//	No argument
		// 戻り値	なし							//
		//	No return value
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
		// 関数名	DrawCountDown			//
		//	Function name DrawCountDown
		// 機能		カウントダウンの描画		//
		//	Draw function countdown
		// 引数		なし						//
		//	No argument
		// 戻り値	なし						//
		//	No return value
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
		// 関数名	InitCountDown					//
		//	Function name InitCountDown
		// 機能		カウントダウン変数の初期化		//
		//	Initialization function countdown variable
		// 戻り値	なし								//
		//	No return value
		// 引数		なし								//
		//	No argument
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
		// 関数名	InitShadow		//
		//	Function name InitShadow
		// 機能		影の初期化		//
		//	Initialization function of shadow
		// 引数		なし				//
		//	No argument
		// 戻り値	なし				//
		//	No return value
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
		// 関数名	UpdateShadow		//
		//	Function name UpdateShadow
		// 機能		影の更新				//
		//	Update function shadow
		// 引数		なし					//
		//	No argument
		// 戻り値	なし					//
		//	No return value
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
