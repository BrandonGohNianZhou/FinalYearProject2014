//--------------------------------------//
// TextureManager.cs					//
//	Class that manages the texture data
// 制作日:2013/10/23						//
// 制作者:Kouno Shin						//
//--------------------------------------//

//----------------------//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace XNAFrameWork
{
	#region Enumerated type

	public enum TextureName : byte
	{
		// Number texture
		NUMBER_1,			// 1
		NUMBER_2,			// 2
		NUMBER_3,			// 3
		COUNTDOWN,			// Countdown for texture
		GO,					// GO!!Character of

		// Shadow texture
		SHADOW,				// Shadow

		// Hand pointer
		HAND_POINTER,

		// Result menu
		REPLAY,			    // Replay
		EXIT,				// Game over
		BLACK,				// Fade for black image
        WHITE,              // white blank

		// Title texture
		TITLE,

        // victory
        WIN_TEXT,
        WIN_SCREEN,

        // game over
        GAMEOVER_SCREEN,
        GAMEOVER_TEXT,
        YOUR_SCORE,

        // final score numbers
        FINAL_SCORE_0,
        FINAL_SCORE_1,
        FINAL_SCORE_2,
        FINAL_SCORE_3,
        FINAL_SCORE_4,
        FINAL_SCORE_5,
        FINAL_SCORE_6,
        FINAL_SCORE_7,
        FINAL_SCORE_8,
        FINAL_SCORE_9,

        // feedback
        PERFECT,
        GOOD,
        BOO,

        // progress bar
        P_BAR,
        P_ICON,

        // pause
        PAUSE_TEXT,

        // instuction
        INSTRUCT_0,
        INSTRUCT_1,
        INSTRUCT_2,
        INSTRUCT_3,
        INSTRUCT_4,
        INSTRUCT_HAND_LEFT,
        INSTRUCT_HAND_RIGHT,

        // speed meter
        SPEED_METER_0,
        SPEED_METER_1,
        SPEED_METER_2,
        SPEED_METER_3,
        SPEED_METER_4,

        // score numbers
        SCORE_0,
        SCORE_1,
        SCORE_2,
        SCORE_3,
        SCORE_4,
        SCORE_5,
        SCORE_6,
        SCORE_7,
        SCORE_8,
        SCORE_9,

		MAX_TEX_NUM,		// The maximum number of texture
	}

	#endregion

	class TextureManager
	{
		#region Field

		// Array of texture
		// The maximum number of texture only ensure an array
		private Texture2D[] texture = new Texture2D[(int)TextureName.MAX_TEX_NUM];

		// Self-object
		private static TextureManager textureManager = null;

		#endregion

		#region Constructor

		public TextureManager()
		{
			// The maximum number of times repeated texture
			for (int i = 0; i < (int)TextureName.MAX_TEX_NUM; i++)
			{
				// Initialization
				this.texture[i] = null;
			}
		}

		#endregion

		#region Function

		//------------------------------------------//
		//	Function GetInstance					//
		//	Get the texture manager function		//
		//	No argument								//
		//	Returns the texture manager				//
		//------------------------------------------//
		public static TextureManager GetInstance()
		{
			// If null
			if (textureManager == null)
			{
				// Create a texture manager
				textureManager = new TextureManager();
			}
			// I return the texture manager
			return textureManager;
		}

		//--------------------------------------//
		//	Function name LoadTexture			//
		//	Load function texture				//
		//	Argument content manager			//
		//	No return value						//
		//--------------------------------------//
		public void LoadTexture(ContentManager contentManager)
		{
			// Number
			this.texture[(int)TextureName.NUMBER_1] = contentManager.Load<Texture2D>(@"テクスチャ\ナンバー\count1");
			this.texture[(int)TextureName.NUMBER_2] = contentManager.Load<Texture2D>(@"テクスチャ\ナンバー\count2");
			this.texture[(int)TextureName.NUMBER_3] = contentManager.Load<Texture2D>(@"テクスチャ\ナンバー\count3");

			// Countdown for GO!!
            //this.texture[(int)TextureName.GO] = contentManager.Load<Texture2D>(@"テクスチャ\ナンバー\go");
            this.texture[(int)TextureName.GO] = contentManager.Load<Texture2D>(@"テクスチャ\Go");
			// Shadow texture
			this.texture[(int)TextureName.SHADOW] = contentManager.Load<Texture2D>(@"テクスチャ\影\shadow");

			// Hand pointer texture
            //this.texture[(int)TextureName.HAND_POINTER] = contentManager.Load<Texture2D>(@"テクスチャ\HandPointer");
            this.texture[(int)TextureName.HAND_POINTER] = contentManager.Load<Texture2D>(@"テクスチャ\New Pointer");

			// Replay texture
            //this.texture[(int)TextureName.REPLAY] = contentManager.Load<Texture2D>(@"テクスチャ\リザルトメニュー\playagain");
            this.texture[(int)TextureName.REPLAY] = contentManager.Load<Texture2D>(@"テクスチャ\Menu\Play Again");
			// Exit texture
			this.texture[(int)TextureName.EXIT] = contentManager.Load<Texture2D>(@"テクスチャ\リザルトメニュー\exit");
			// Fade for black image
            this.texture[(int)TextureName.BLACK] = contentManager.Load<Texture2D>(@"テクスチャ\Black");
            this.texture[(int)TextureName.WHITE] = contentManager.Load<Texture2D>(@"テクスチャ\White");
			// Title image
			this.texture[(int)TextureName.TITLE] = contentManager.Load<Texture2D>(@"テクスチャ\タイトル\Title");

            // victory
            this.texture[(int)TextureName.WIN_SCREEN] = contentManager.Load<Texture2D>(@"テクスチャ\Menu\Victory Screen");
            this.texture[(int)TextureName.WIN_TEXT] = contentManager.Load<Texture2D>(@"テクスチャ\Menu\Gratz");

            // game over
            this.texture[(int)TextureName.GAMEOVER_SCREEN] = contentManager.Load<Texture2D>(@"テクスチャ\Menu\Game Over Screen");
            this.texture[(int)TextureName.GAMEOVER_TEXT] = contentManager.Load<Texture2D>(@"テクスチャ\Menu\Game Over Words");
            this.texture[(int)TextureName.YOUR_SCORE] = contentManager.Load<Texture2D>(@"テクスチャ\Menu\Your Score");

            // final score number
            this.texture[(int)TextureName.FINAL_SCORE_0] = contentManager.Load<Texture2D>(@"テクスチャ\Red Numbers\Red 0");
            this.texture[(int)TextureName.FINAL_SCORE_1] = contentManager.Load<Texture2D>(@"テクスチャ\Red Numbers\Red 1");
            this.texture[(int)TextureName.FINAL_SCORE_2] = contentManager.Load<Texture2D>(@"テクスチャ\Red Numbers\Red 2");
            this.texture[(int)TextureName.FINAL_SCORE_3] = contentManager.Load<Texture2D>(@"テクスチャ\Red Numbers\Red 3");
            this.texture[(int)TextureName.FINAL_SCORE_4] = contentManager.Load<Texture2D>(@"テクスチャ\Red Numbers\Red 4");
            this.texture[(int)TextureName.FINAL_SCORE_5] = contentManager.Load<Texture2D>(@"テクスチャ\Red Numbers\Red 5");
            this.texture[(int)TextureName.FINAL_SCORE_6] = contentManager.Load<Texture2D>(@"テクスチャ\Red Numbers\Red 6");
            this.texture[(int)TextureName.FINAL_SCORE_7] = contentManager.Load<Texture2D>(@"テクスチャ\Red Numbers\Red 7");
            this.texture[(int)TextureName.FINAL_SCORE_8] = contentManager.Load<Texture2D>(@"テクスチャ\Red Numbers\Red 8");
            this.texture[(int)TextureName.FINAL_SCORE_9] = contentManager.Load<Texture2D>(@"テクスチャ\Red Numbers\Red 9");

            // feedback
            //this.texture[(int)TextureName.PERFECT] = contentManager.Load<Texture2D>(@"テクスチャ\Feedback\PERFECT");
            //this.texture[(int)TextureName.GOOD] = contentManager.Load<Texture2D>(@"テクスチャ\Feedback\GOOD");
            //this.texture[(int)TextureName.BOO] = contentManager.Load<Texture2D>(@"テクスチャ\Feedback\BOO");

            this.texture[(int)TextureName.PERFECT] = contentManager.Load<Texture2D>(@"Textures\Feedback\PERFECT");
            this.texture[(int)TextureName.GOOD] = contentManager.Load<Texture2D>(@"Textures\Feedback\GREAT");
            this.texture[(int)TextureName.BOO] = contentManager.Load<Texture2D>(@"Textures\Feedback\BOO");


            // progress bar
            this.texture[(int)TextureName.P_BAR] = contentManager.Load<Texture2D>(@"テクスチャ\Progress Bar\ProgressBar");
            this.texture[(int)TextureName.P_ICON] = contentManager.Load<Texture2D>(@"テクスチャ\Progress Bar\HP");

            // pause
            this.texture[(int)TextureName.PAUSE_TEXT] = contentManager.Load<Texture2D>(@"テクスチャ\Menu\Resume");

            // instruction
            this.texture[(int)TextureName.INSTRUCT_0] = contentManager.Load<Texture2D>(@"テクスチャ\Instructions\Page 1");
            this.texture[(int)TextureName.INSTRUCT_1] = contentManager.Load<Texture2D>(@"テクスチャ\Instructions\Page 2");
            this.texture[(int)TextureName.INSTRUCT_2] = contentManager.Load<Texture2D>(@"テクスチャ\Instructions\Page 3");
            this.texture[(int)TextureName.INSTRUCT_3] = contentManager.Load<Texture2D>(@"テクスチャ\Instructions\Page 4");
            this.texture[(int)TextureName.INSTRUCT_4] = contentManager.Load<Texture2D>(@"テクスチャ\Instructions\Page 5");
            this.texture[(int)TextureName.INSTRUCT_HAND_LEFT] = contentManager.Load<Texture2D>(@"テクスチャ\Instructions\Hand to left");
            this.texture[(int)TextureName.INSTRUCT_HAND_RIGHT] = contentManager.Load<Texture2D>(@"テクスチャ\Instructions\Hand to right");

            // speed meter and score bar
            this.texture[(int)TextureName.SPEED_METER_0] = contentManager.Load<Texture2D>(@"テクスチャ\Score Bar\0 Speed");
            this.texture[(int)TextureName.SPEED_METER_1] = contentManager.Load<Texture2D>(@"テクスチャ\Score Bar\1 Speed");
            this.texture[(int)TextureName.SPEED_METER_2] = contentManager.Load<Texture2D>(@"テクスチャ\Score Bar\2 Speed");
            this.texture[(int)TextureName.SPEED_METER_3] = contentManager.Load<Texture2D>(@"テクスチャ\Score Bar\3 Speed");
            this.texture[(int)TextureName.SPEED_METER_4] = contentManager.Load<Texture2D>(@"テクスチャ\Score Bar\4 Speed");

            // score number
            this.texture[(int)TextureName.SCORE_0] = contentManager.Load<Texture2D>(@"テクスチャ\Numbers\0");
            this.texture[(int)TextureName.SCORE_1] = contentManager.Load<Texture2D>(@"テクスチャ\Numbers\1");
            this.texture[(int)TextureName.SCORE_2] = contentManager.Load<Texture2D>(@"テクスチャ\Numbers\2");
            this.texture[(int)TextureName.SCORE_3] = contentManager.Load<Texture2D>(@"テクスチャ\Numbers\3");
            this.texture[(int)TextureName.SCORE_4] = contentManager.Load<Texture2D>(@"テクスチャ\Numbers\4");
            this.texture[(int)TextureName.SCORE_5] = contentManager.Load<Texture2D>(@"テクスチャ\Numbers\5");
            this.texture[(int)TextureName.SCORE_6] = contentManager.Load<Texture2D>(@"テクスチャ\Numbers\6");
            this.texture[(int)TextureName.SCORE_7] = contentManager.Load<Texture2D>(@"テクスチャ\Numbers\7");
            this.texture[(int)TextureName.SCORE_8] = contentManager.Load<Texture2D>(@"テクスチャ\Numbers\8");
            this.texture[(int)TextureName.SCORE_9] = contentManager.Load<Texture2D>(@"テクスチャ\Numbers\9");
		}

		//------------------------------------------//
		//	Function name GetTexture				//
		//	Get the texture function				//
		//	Identification number of arguments texture
		//	Returns the texture data				//
		//------------------------------------------//
		public Texture2D GetTexture(TextureName name)
		{
			// I examine whether the range
			if (name < 0 || name >= TextureName.MAX_TEX_NUM)
			{
				return null;
			}

			// Return null if none
			if (this.texture[(int)name] == null)
			{
				return null;
			}
			return this.texture[(int)name];
		}

		#endregion
	}
}
