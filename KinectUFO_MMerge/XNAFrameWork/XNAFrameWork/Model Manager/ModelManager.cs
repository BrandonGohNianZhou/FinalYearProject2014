//--------------------------//
// ModelManager.cs			//
//	Class that manages the model
// 制作日:2013/10/03			//
// 制作者:Kouno Shin			//
//--------------------------//

//----------------------//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace XNAFrameWork
{
	#region Enumerated type

	// Identifier of the model
	public enum ModelName : byte
	{
		//----Game play----//
		PLAYER_CHARACTER,			// Player character
		PLAYER_CHARACTER_DEFAULT,	// Default pose character

		//----Stage data----//
		STAGE_START,
		STAGE_END,
		STAGE_0,
		STAGE_1,
		STAGE_2,
		STAGE_3,
		STAGE_4,
		STAGE_5,
		STAGE_6,

        // testing
        SKYDOME,
        TEST,
		
        //Road
        ROAD1,
        ROAD2,

        //Powerups
        INVUL,
        POINT_BOOST,
        SPEED_BOOST,

        //Obstacles
        //Obstacles that need to be jumped to pass
        JUMP_LEFT,
        JUMP_CENTER,
        JUMP_RIGHT,
        JUMP_NONE,

        //Obstacles that need to be ducked to pass
        DUCK_LEFT,
        DUCK_CENTER,
        DUCK_RIGHT,

        //Obstacles that dont need to jump/duck to pass
        NONE_LEFT,
        NONE_CENTER,
        NONE_RIGHT,

		//----Main menu----//
		MAIN_CUBE,
		MaxModelNum		// The maximum number of model
	};

	#endregion

	class ModelManager
	{
		#region Variable

		// Array of model
		// The maximum number of model only ensure an array
		private Model[] model = new Model[(int)ModelName.MaxModelNum];

		// Self-object
		private static ModelManager modelManager = null;

		#endregion

		// Constructor
		private ModelManager()
		{
			// Repeated several minutes maximum of model
			for (int i = 0; i < (int)ModelName.MaxModelNum; i++)
			{
				// Initialization
				this.model[i] = null;
			}
		}

		//--------------------------------------//
		//	Function GetInstance				//
		//	I pass the functional model manager	//
		//	No argument							//
		//	Returns model manager				//
		//--------------------------------------//
		public static ModelManager GetInstance()
		{
			// If null
			if (modelManager == null)
			{
				// Creating
				modelManager = new ModelManager();
			}
			return modelManager;
		}

		//--------------------------------------//
		//	Function name LoadModel				//
		//	I make a load of functional model	//
		//	Argument content manager			//
		//	No return value						//
		//--------------------------------------//
		public void LoadModel(ContentManager contentManager)
		{
			//----Main menu----//
			this.model[(int)ModelName.MAIN_CUBE] = contentManager.Load<Model>(@"モデル\メインメニュー\ui cube");

			// Player
			// Player default pose
            this.model[(int)ModelName.PLAYER_CHARACTER] = contentManager.Load<Model>(@"モデル\Final Animation\With Hit Animation");
            this.model[(int)ModelName.PLAYER_CHARACTER_DEFAULT] = contentManager.Load<Model>(@"モデル\Final Animation\With Hit Animation");

			//	Load Environment models here
            //this.model[(int)ModelName.STAGE_START] = contentManager.Load<Model>(@"モデル\Stages\PartStart");
            //this.model[(int)ModelName.STAGE_END] = contentManager.Load<Model>(@"モデル\Stages\PartEnd");
            //this.model[(int)ModelName.STAGE_0] = contentManager.Load<Model>(@"モデル\Stages\Part1");
            //this.model[(int)ModelName.STAGE_1] = contentManager.Load<Model>(@"モデル\Stages\Part2");
            //this.model[(int)ModelName.STAGE_2] = contentManager.Load<Model>(@"モデル\Stages\Part3");
            //this.model[(int)ModelName.STAGE_3] = contentManager.Load<Model>(@"モデル\Stages\Part4");
            //this.model[(int)ModelName.STAGE_4] = contentManager.Load<Model>(@"モデル\Stages\Part5");
            //this.model[(int)ModelName.STAGE_5] = contentManager.Load<Model>(@"モデル\Stages\Part6");
            //this.model[(int)ModelName.STAGE_6] = contentManager.Load<Model>(@"モデル\Stages\Part7");

            //Load Roads
            this.model[(int)ModelName.ROAD1] = contentManager.Load<Model>(@"Models\Road\Road_1");
            this.model[(int)ModelName.ROAD2] = contentManager.Load<Model>(@"Models\Road\Road_2");

            //Load Power Ups
            this.model[(int)ModelName.INVUL] = contentManager.Load<Model>(@"Models\PowerUp\Invulnerable");
            this.model[(int)ModelName.SPEED_BOOST] = contentManager.Load<Model>(@"Models\PowerUp\Speed");
            this.model[(int)ModelName.POINT_BOOST] = contentManager.Load<Model>(@"Models\PowerUp\PointBooster");

            //Load Obstacles
            this.model[(int)ModelName.JUMP_NONE] = contentManager.Load<Model>(@"Models\Obstacle\JUMP_NONE");
            this.model[(int)ModelName.JUMP_CENTER] = contentManager.Load<Model>(@"Models\Obstacle\JUMP_CENTER");
            this.model[(int)ModelName.JUMP_LEFT] = contentManager.Load<Model>(@"Models\Obstacle\JUMP_LEFT");
            this.model[(int)ModelName.JUMP_RIGHT] = contentManager.Load<Model>(@"Models\Obstacle\JUMP_RIGHT");

            this.model[(int)ModelName.DUCK_LEFT] = contentManager.Load<Model>(@"Models\Obstacle\DUCK_LEFT");
            this.model[(int)ModelName.DUCK_CENTER] = contentManager.Load<Model>(@"Models\Obstacle\DUCK_CENTER");
            this.model[(int)ModelName.DUCK_RIGHT] = contentManager.Load<Model>(@"Models\Obstacle\DUCK_RIGHT");

            this.model[(int)ModelName.NONE_LEFT] = contentManager.Load<Model>(@"Models\Obstacle\NONE_LEFT");
            this.model[(int)ModelName.NONE_CENTER] = contentManager.Load<Model>(@"Models\Obstacle\NONE_CENTER");
            this.model[(int)ModelName.NONE_RIGHT] = contentManager.Load<Model>(@"Models\Obstacle\NONE_RIGHT");

            // skydome
            this.model[(int)ModelName.SKYDOME] = contentManager.Load<Model>(@"モデル\New Skydome");
		}

		//----------------------------------//
		//	Function name GetModel			//
		//	I get the functional model		//
		//	Identification number of arguments model 
		//	Returns model					//
		//----------------------------------//
		public Model GetModel(ModelName name)
		{
			// I examine whether the range
			if (name < 0 || name >= ModelName.MaxModelNum)
			{
				return null;
			}

			// Return null if none
			if (this.model[(int)name] == null)
			{
				return null;
			}
			return this.model[(int)name];
		}
	}
	
}
