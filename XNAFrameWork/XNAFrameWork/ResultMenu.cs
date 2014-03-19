//--------------------------------------//
// ResultMenu.cs						//
// リザルトメニューを管理するクラス		//
//	Class that manages the result menu
// 制作日:2013/11/13						//
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
using Microsoft.Xna.Framework;
using XNAFrameWork;
using Microsoft.Xna.Framework.Graphics;

namespace XNAFrameWork
{
	// State management interface of the result menu
	interface ResultCommandState
	{
		// Processing at the time of the chosen
		void Pushed();
	}

	#region Replay command

	class Replay : ResultCommandState
	{
		//----------------------------------------------//
		// 関数名	Pushed								//
		//	Function name Pushed
		// 機能		リプレイが押されたときの処理			//
		//	Process when the replay function is pressed
		// 引数		なし									//
		//	No argument
		// 戻り値	なし									//
		//	No return value
		//----------------------------------------------//
		public void Pushed()
		{
            Score.score = 0;
			SceneManager.nextScene = SceneManager.SCENE_TYPE.SAMPLE_SCENE1;
		}
	}
	#endregion

	#region Shutdown command

	class Exit : ResultCommandState
	{
		//--------------------------------------------------//
		// 関数名	Pushed									//
		//	Function name Pushed
		// 機能		終了コマンドが押されたときの処理			//
		//	Process when the function end command is pressed
		// 引数		なし										//
		//	No argument
		// 戻り値	なし										//
		//	No return value
		//--------------------------------------------------//
		public void Pushed()
        {
            //Score.score = 0;
            //SceneManager.nextScene = SceneManager.SCENE_TYPE.SAMPLE_SCENE1;
			//Game1.gameEndFlag = true;
		}
	}

	#endregion

	#region If no command is selected
	class NoneCommand : ResultCommandState
	{
		//------------------------------------------//
		// 関数名	Pushed							//
		//	Function name Pushed
		// 機能		決定処理がされたときの処理		//
		//	Process when the function determining process is
		// 引数		なし								//
		//	No argument
		// 戻り値	なし								//
		//	No return value
		//------------------------------------------//
		public void Pushed()
		{
			// It is not nothing
		}
	}

	#endregion

	// Result menu class
	class ResultMenu
	{
        // screen size
        int w;
        int h;

        // complete game
        public bool Victory;

        // background 
        float bg_alpha;
        float dark_alpha;

        // animation
        public bool animate_start;
        bool drawall;
        bool showtext;

        // score
        float yourscore_posx;
        float yourscore_scale;
        float score_posx;
        float score_posy;
        int score_sizex;
        int score_sizey;
        float score_scale;

        // gameover text
        float gameover_text_y;
        float gameover_text_alpha;

        // victory text
        float victory_text_y;
        float victory_text_alpha;

		#region Field

		ResultCommandState currentCommand;		// The selected command now

		HandPointer handPointer;				// Hand pointer

		// Each command
		SpriteObject replay;		// Replay
		SpriteObject exit;			// End


		// Object rotation counter
		float counter;

		#endregion

		#region Constructor

		public ResultMenu()
		{
            // screen size
            this.w = Game1.graphics.GraphicsDevice.Viewport.Width;
            this.h = Game1.graphics.GraphicsDevice.Viewport.Height;
            
            // complete game
            this.Victory = false;

            // background
            this.bg_alpha = 0.0f;
            this.dark_alpha = 0.0f;

            // animation
            this.animate_start = false;
            this.drawall = false;
            this.showtext = false;

            // score
            this.yourscore_posx = w * 0.25f;
            this.yourscore_scale = 1.0f;
            this.score_posx = w * 0.5f;
            this.score_posy = h * 0.5f;
            this.score_sizex = 70;
            this.score_sizey = 70;
            this.score_scale = 2.0f;
            
            // gameover text
            this.gameover_text_y = h * 0.5f;
            this.gameover_text_alpha = 0.5f;

            // victory text
            this.victory_text_y = h * 0.75f;
            this.victory_text_alpha = 0.5f;

			// Configured without a command that is selected
			this.currentCommand = new NoneCommand();

			// Initialization of the hand pointer
			this.handPointer = new HandPointer();

			// Instantiation of each command
			this.replay = new SpriteObject();
			this.exit	= new SpriteObject();

			// Adjustment of the position of each command
            //this.replay.pos = new Vector2(w * 0.5f, h * 0.32f);
			this.replay.pos = new Vector2(w * 0.5f, h * 0.75f);
            this.exit.pos = new Vector2(w * 0.5f, h * 0.625f);

			// Initialize the counter
			this.counter = 0.0f;
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
            if (Victory)
            {
                VictoryAnimate();
            }
            else
            {
                GameOverAnimate();
            }

			// Update of the hand pointer
			this.handPointer.Update();

			// Check if they hit any command currently
			this.ChangeCommand();

			// Run the effect of the command that is currently selected
			// Nothing
			if (currentCommand is NoneCommand)
			{
				this.NonEffect(ref replay);
				this.NonEffect(ref exit);
			}
			// Replay
			else if (currentCommand is Replay)
			{
				this.SelectEffect(ref replay);
				this.NonEffect(ref exit);
			}
			// End
			else if (currentCommand is Exit)
			{
				this.SelectEffect(ref exit);
				this.NonEffect(ref replay);
			}

			// Determination process if you are
			if (Game1.person.Pushed())
			{
				// I fly to the process of determining the command of the currently selected
				this.currentCommand.Pushed();
			}
		}

		//------------------------------------------------------------------//
		// 関数名	ChangeCommand											//
		//	Function name ChangeCommand
		// 機能		あたり判定により選ばれているコマンドをチェンジする			//
		//	I want to change the command that is selecte by the judgment per function
		// 引数		なし														//
		//	No argument
		// 戻り値	なし														//
		//	No return value
		//------------------------------------------------------------------//
		private void ChangeCommand()
		{
			// Get the offset of each command
			// Replay
			var replayOffset = new Vector2(256.0f * this.replay.scale.X,
										   32.0f * this.replay.scale.Y);
			// End
			var exitOffset = new Vector2(256.0f * this.exit.scale.X,
										 32.0f * this.exit.scale.Y);

			// When you are hit with replay
			if(Collision.BoundingBox2D(this.replay.pos,replayOffset,
									this.handPointer.pos,this.handPointer.offset))
			{
				// To replay the commands selected
				this.currentCommand = new Replay();
			}
			// When you are hit with the end
			else if (Collision.BoundingBox2D(this.exit.pos, exitOffset,
									this.handPointer.pos, this.handPointer.offset))
			{
				// To exit the command that is selected
				this.currentCommand = new Exit();
			}
			// When nothing hit
			else
			{
				// Nothing is selected
				this.currentCommand = new NoneCommand();
			}
			
		}
		
		//----------------------------------------------------------------------//
		// 関数名	SelectEffect												//
		//	Function name SelectEffect
		// 機能		そのスプライトオブジェクトが選ばれているときのエフェクト		//
		//	Effect when the sprite object is selected function
		// 引数		スプライトオブジェクト										//
		//	Argument Sprite object
		// 戻り値	なし															//
		//	No return value
		//----------------------------------------------------------------------//
		public void SelectEffect(ref SpriteObject sprite)
		{
			// And adding the counter
			this.counter += 0.1f;

			// I shake the sprite object
			sprite.angle = (float)Math.Sin(this.counter) * 0.3f;

			// I expand the sprite object
            //sprite.scale = new Vector2(2.4f, 2.4f);
            sprite.scale = new Vector2(1.4f, 1.4f);
		}

		//--------------------------------------//
		// 関数名	NonEffect					//
		//	Function name NonEffect
		// 機能		エフェクトをかけない			//
		//	Do not apply effect function
		// 引数		スプライトオブジェクト		//
		//	Argument Sprite object
		// 戻り値	なし							//
		//	No return value
		//--------------------------------------//
		public void NonEffect(ref SpriteObject sprite)
		{
			// The return on the basis of the angle
			sprite.angle = 0.0f;

			// I returned to the original scale
            //sprite.scale = new Vector2(2.0f, 2.0f);
            sprite.scale = new Vector2(1.0f, 1.0f);
		}

		//----------------------------------//
		// 関数名	Draw					//
		//	Function Draw
		// 機能		各コマンドの描画処理		//
		//	Drawing process of the function each command
		// 引数		なし						//
		//	No argument
		// 戻り値	なし						//
		//	No return value
		//----------------------------------//
		public void Draw()
		{
            if (Victory)
            {
                DrawVictory();
            }
            else
            {
                DrawGameOver();
            }

			//// Drawing of the end command
            //Game1.spriteBatch.Draw(
            //                    TextureManager.GetInstance().GetTexture(TextureName.EXIT),
            //                    this.exit.pos,
            //                    new Rectangle(0, 0, (int)(w * 0.64f), (int)(h * 0.133f)),
            //                    Color.White * this.exit.alpha,
            //                    this.exit.angle,
            //                    new Vector2(256, 32),
            //                    this.exit.scale,
            //                    SpriteEffects.None,
            //                    1.0f);					// Set back from the hand pointer
			// Drawing of hand pointer
			this.handPointer.Draw();
		}

        // game over
        void DrawGameOver()
        {
            if (Victory) return;
            // gameover back ground
            Game1.spriteBatch.Draw(
                                TextureManager.GetInstance().GetTexture(TextureName.GAMEOVER_SCREEN),
                                Vector2.Zero, new Rectangle(0, 0, w, h), Color.White * bg_alpha,
                                0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

            // fade dark
            Game1.spriteBatch.Draw(
                                TextureManager.GetInstance().GetTexture(TextureName.BLACK),
                                new Vector2(w * 0.5f, h * 0.5f), new Rectangle(0, 0, 16, 16), Color.White * dark_alpha,
                                0.0f, new Vector2(8, 8), 100.0f, SpriteEffects.None, 1.0f);

            // show gameover text
            if (showtext)
            {
                // gameover text
                Game1.spriteBatch.Draw(
                                    TextureManager.GetInstance().GetTexture(TextureName.GAMEOVER_TEXT),
                                    new Vector2(w * 0.5f, gameover_text_y), new Rectangle(0, 0, 797, 182), Color.White * gameover_text_alpha,
                                    0.0f, new Vector2(399, 91), 1.0f, SpriteEffects.None, 1.0f);
            }

            // if finish gameover animation
            if (drawall)
            {
                // draw digit
                DrawScore();

                // play again button
                Game1.spriteBatch.Draw(
                                    TextureManager.GetInstance().GetTexture(TextureName.REPLAY),
                                    this.replay.pos, new Rectangle(0, 0, 407, 100), Color.White * this.replay.alpha,
                                    this.replay.angle, new Vector2(203, 50), this.replay.scale, SpriteEffects.None, 1.0f);
            }
        }

        void DrawVictory()
        {
            if (!Victory) return;
            // victory back ground
            Game1.spriteBatch.Draw(
                                TextureManager.GetInstance().GetTexture(TextureName.WIN_SCREEN),
                                Vector2.Zero, new Rectangle(0, 0, w, h), Color.White * bg_alpha,
                                0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);

            // show victory text
            if (showtext)
            {
                // victory text
                Game1.spriteBatch.Draw(
                                    TextureManager.GetInstance().GetTexture(TextureName.WIN_TEXT),
                                    new Vector2(w * 0.25f, victory_text_y), new Rectangle(0, 0, 552, 207), Color.White * victory_text_alpha,
                                    0.0f, new Vector2(276, 103), 1.0f, SpriteEffects.None, 1.0f);
            }

            // if finish victory animation
            if (drawall)
            {
                // draw digit
                DrawScore();

                // play again button
                Game1.spriteBatch.Draw(
                                    TextureManager.GetInstance().GetTexture(TextureName.REPLAY),
                                    this.replay.pos, new Rectangle(0, 0, 407, 100), Color.White * this.replay.alpha,
                                    this.replay.angle, new Vector2(203, 50), this.replay.scale, SpriteEffects.None, 1.0f);
            }
        }

        // draw final score
        void DrawScore()
        {
            // your score
            Game1.spriteBatch.Draw(
                                TextureManager.GetInstance().GetTexture(TextureName.YOUR_SCORE),
                                new Vector2(yourscore_posx, score_posy), new Rectangle(0, 0, 482, 103), Color.White * this.replay.alpha,
                                0.0f, new Vector2(241, 51), yourscore_scale, SpriteEffects.None, 1.0f);

            // 1st digit (10000)
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(CalScore(10000)),
                                    new Vector2(score_posx, score_posy), new Rectangle(0, 0, score_sizex, score_sizey), Color.White,
                                    0.0f, new Vector2(score_sizex * 0.5f, score_sizey * 0.5f), score_scale, SpriteEffects.None, 1.0f
                                    );

            // 2nd digit (1000)
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(CalScore(1000)),
                                    new Vector2(score_posx + (score_sizex * 0.75f * 1.5f), score_posy), new Rectangle(0, 0, score_sizex, score_sizey), Color.White,
                                    0.0f, new Vector2(score_sizex * 0.5f, score_sizey * 0.5f), score_scale, SpriteEffects.None, 1.0f
                                    );

            // 3rd digit (100)
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(CalScore(100)),
                                    new Vector2(score_posx + (score_sizex * 0.75f * 2 * 1.5f), score_posy), new Rectangle(0, 0, score_sizex, score_sizey), Color.White,
                                    0.0f, new Vector2(score_sizex * 0.5f, score_sizey * 0.5f), score_scale, SpriteEffects.None, 1.0f
                                    );

            // 4th digit (10)
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(CalScore(10)),
                                    new Vector2(score_posx + (score_sizex * 0.75f * 3 * 1.5f), score_posy), new Rectangle(0, 0, score_sizex, score_sizey), Color.White,
                                    0.0f, new Vector2(score_sizex * 0.5f, score_sizey * 0.5f), score_scale, SpriteEffects.None, 1.0f
                                    );

            // 5th digit (1)
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(CalScore(1)),
                                    new Vector2(score_posx + (score_sizex * 0.75f * 4 * 1.5f), score_posy), new Rectangle(0, 0, score_sizex, score_sizey), Color.White,
                                    0.0f, new Vector2(score_sizex * 0.5f, score_sizey * 0.5f), score_scale, SpriteEffects.None, 1.0f
                                    );
        }

        TextureName CalScore(int digit)
        {
            int tempscore = Score.score % (digit * 10);
            tempscore /= digit;

            TextureName tempname = TextureName.FINAL_SCORE_0;

            switch (tempscore)
            {
                case 0:
                    tempname = TextureName.FINAL_SCORE_0;
                    break;
                case 1:
                    tempname = TextureName.FINAL_SCORE_1;
                    break;
                case 2:
                    tempname = TextureName.FINAL_SCORE_2;
                    break;
                case 3:
                    tempname = TextureName.FINAL_SCORE_3;
                    break;
                case 4:
                    tempname = TextureName.FINAL_SCORE_4;
                    break;
                case 5:
                    tempname = TextureName.FINAL_SCORE_5;
                    break;
                case 6:
                    tempname = TextureName.FINAL_SCORE_6;
                    break;
                case 7:
                    tempname = TextureName.FINAL_SCORE_7;
                    break;
                case 8:
                    tempname = TextureName.FINAL_SCORE_8;
                    break;
                case 9:
                    tempname = TextureName.FINAL_SCORE_9;
                    break;
                default:
                    break;
            }

            return tempname;
        }

        // gameover screen animation
        void GameOverAnimate()
        {
            if (!animate_start) return;

            // fade in back ground
            if (bg_alpha < 1.5f) 
            {
                bg_alpha += 0.01f;
            }

            // fade in dark screen
            if (bg_alpha > 1.4f)
            {
                if (dark_alpha < 0.5f)
                {
                    dark_alpha += 0.01f;
                }
                else
                {
                    // show gameover text
                    showtext = true;
                }
            }

            // gameover text animation
            if (showtext)
            {
                // fade in
                if (gameover_text_alpha < 1.0f)
                {
                    gameover_text_alpha += 0.01f;
                }

                // move up
                if (gameover_text_y > h * 0.15f)
                {
                    gameover_text_y -= h * 0.01f;
                }
                else
                {
                    // finish moving up
                    drawall = true;
                    this.yourscore_posx = w * 0.25f;
                    this.yourscore_scale = 1.0f;
                    this.score_posx = w * 0.5f;
                    this.score_posy = h * 0.5f;
                    this.score_scale = 2.0f;
                    this.replay.pos.X = w * 0.5f;
                    this.replay.pos.Y = h * 0.75f;
                }
            }
        }

        // victory screen animation
        void VictoryAnimate()
        {
            if (!animate_start) return;

            // fade in back ground
            if (bg_alpha < 1.5f)
            {
                bg_alpha += 0.01f;
                if (bg_alpha > 1.1f)
                {
                    // show congrat text
                    showtext = true;
                }
            }

            // congrat text animation
            if (showtext)
            {
                // move up
                if (victory_text_y > h * 0.45f)
                {
                    victory_text_y -= h * 0.01f;
                }

                // fade in
                if (victory_text_alpha < 1.0f)
                {
                    victory_text_alpha += 0.01f;
                }
                else
                {
                    // finish moving up
                    drawall = true;
                    this.yourscore_posx = w * 0.25f;
                    this.yourscore_scale = 0.75f;
                    this.score_posx = w * 0.5f;
                    this.score_posy = h * 0.65f;
                    this.score_scale = 1.5f;
                    this.replay.pos.X = w * 0.5f;
                    this.replay.pos.Y = h * 0.8f;
                }
            }
        }

		#endregion
	}
}
