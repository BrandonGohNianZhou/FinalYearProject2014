//------------------------------//
// Score.cs						//
// スコアを管理するクラス			//
//	Class that manages the score			
// 制作日:2013/11/11				//
// 制作者:Kouno Shin				//
//------------------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAFrameWork;
using Microsoft.Xna.Framework.Input;

namespace XNAFrameWork
{
	class Score
	{
		#region Field

		// Score
        public static int score = 0;

		// Basic sum of point
		public int basicPoint;

        // screen size
        int w;
        int h;

        // size of the digit number
        int num_sizex;
        int num_sizey;
        float num_scale;

        // score position
        float posx;
        float posy;

        // bar position
        float b_posx;
        float b_posy;

        // combo
        public int combo;

        // accuracy of evading
        static public int acc_level;

		#endregion

		#region Constructor

		public Score()
		{
			// Initialization of basic added value
			this.basicPoint = 0;

            // screen size
            this.w = Game1.graphics.GraphicsDevice.Viewport.Width;
            this.h = Game1.graphics.GraphicsDevice.Viewport.Height;

            // size of the number
            this.num_sizex = (int)(w * 0.0325f);
            this.num_sizey = (int)(h * 0.063f);
            this.num_scale = 0.8f;

            // score position
            this.posx = w * 0.07f;
            this.posy = h * 0.082f;

            // bar position
            this.b_posx = w * 0.035f;
            this.b_posy = h * 0.035f;

            // combo
            this.combo = 1;

            // accuracy of evading
            acc_level = 1;
		}

		#endregion

		#region Function

		public void Update(Vector3 playerVel)
        {
            // update score
            score += -(int)playerVel.Z;
        }

        public void AddScore()
        {
            // addition score base on combo and accuracy of evasion
            this.basicPoint = (this.combo * (acc_level * 200));
            score += this.basicPoint;
            this.combo += 1;
            acc_level = 1;
        }

        public void Draw2D()
        {
            DrawMeter();
            DrawScore();
        }

        void DrawMeter()
        {
            // score back ground && speed meter
            if (Player.extraspeed <= -1.5f)
            {
                Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.SPEED_METER_4),
                                        new Vector2(b_posx, b_posy), new Rectangle(0, 0, 500, 120), Color.White * 0.95f,
                                        0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1.0f
                                        );
            }
            else if (Player.extraspeed <= -1.125f)
            {
                Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.SPEED_METER_3),
                                        new Vector2(b_posx, b_posy), new Rectangle(0, 0, 500, 120), Color.White * 0.95f,
                                        0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1.0f
                                        );
            }
            else if (Player.extraspeed <= -0.75f)
            {
                Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.SPEED_METER_2),
                                        new Vector2(b_posx, b_posy), new Rectangle(0, 0, 500, 120), Color.White * 0.95f,
                                        0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1.0f
                                        );
            }
            else if (Player.extraspeed < 0.0f)
            {
                Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.SPEED_METER_1),
                                        new Vector2(b_posx, b_posy), new Rectangle(0, 0, 500, 120), Color.White * 0.95f,
                                        0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1.0f
                                        );
            }
            else
            {
                Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.SPEED_METER_0),
                                        new Vector2(b_posx, b_posy), new Rectangle(0, 0, 500, 120), Color.White * 0.95f,
                                        0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 1.0f
                                        );
            }
        }

        void DrawScore()
        {
            // 1st digit (10000)
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(CalScore(10000)),
                                    new Vector2(posx, posy), new Rectangle(0, 0, num_sizex, num_sizey), Color.White,
                                    0.0f, Vector2.Zero, num_scale, SpriteEffects.None, 1.0f
                                    );

            // 2nd digit (1000)
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(CalScore(1000)),
                                    new Vector2(posx + (num_sizex * 0.45f * 1.5f), posy), new Rectangle(0, 0, num_sizex, num_sizey), Color.White,
                                    0.0f, Vector2.Zero, num_scale, SpriteEffects.None, 1.0f
                                    );

            // 3rd digit (100)
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(CalScore(100)),
                                    new Vector2(posx + (num_sizex * 0.45f * 2 * 1.5f), posy), new Rectangle(0, 0, num_sizex, num_sizey), Color.White,
                                    0.0f, Vector2.Zero, num_scale, SpriteEffects.None, 1.0f
                                    );

            // 4th digit (10)
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(CalScore(10)),
                                    new Vector2(posx + (num_sizex * 0.45f * 3 * 1.5f), posy), new Rectangle(0, 0, num_sizex, num_sizey), Color.White,
                                    0.0f, Vector2.Zero, num_scale, SpriteEffects.None, 1.0f
                                    );

            // 5th digit (1)
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(CalScore(1)),
                                    new Vector2(posx + (num_sizex * 0.45f * 4 * 1.5f), posy), new Rectangle(0, 0, num_sizex, num_sizey), Color.White,
                                    0.0f, Vector2.Zero, num_scale, SpriteEffects.None, 1.0f
                                    );
        }

        public TextureName CalScore(int digit)
        {
            int tempscore = score % (digit*10);
            tempscore /= digit;

            TextureName tempname = TextureName.SCORE_0;

            switch (tempscore)
            {
                case 0:
                    tempname = TextureName.SCORE_0;
                    break;
                case 1:
                    tempname = TextureName.SCORE_1;
                    break;
                case 2:
                    tempname = TextureName.SCORE_2;
                    break;
                case 3:
                    tempname = TextureName.SCORE_3;
                    break;
                case 4:
                    tempname = TextureName.SCORE_4;
                    break;
                case 5:
                    tempname = TextureName.SCORE_5;
                    break;
                case 6:
                    tempname = TextureName.SCORE_6;
                    break;
                case 7:
                    tempname = TextureName.SCORE_7;
                    break;
                case 8:
                    tempname = TextureName.SCORE_8;
                    break;
                case 9:
                    tempname = TextureName.SCORE_9;
                    break;
                default:
                    break;
            }

            return tempname;
        }

		//------------------------------//
		// 関数名	AddPoint			//
		//	Function name AddPoint
		// 機能		スコアの追加			//
		//	Add function score
		// 引数		現在のコンボ数		//
		//	Combo number of current argument
		// 戻り値	なし					//
		//	No return value
		//------------------------------//
		public void AddPoint(int combo)
		{
			// Combo magnification
			float odds = 1.0f;

			// No bonus 1 combo
			if (combo != 1)
			{
				// Put a bonus depending on the combo
				odds += combo * 0.1f;
			}

			// In addition to the score points obtained by adding the bonus
			score += (int)(odds * basicPoint);
		}

		#endregion
	}
}
