//--------------------------//
// ModelManager.cs			//
// モデルを管理するクラス		//
//	Class that manages the model
// 制作日:2013/10/03			//
// 制作者:Kouno Shin			//
//--------------------------//

//----------------------//
//----名前空間の省略-----//
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
		// 関数名	GetInstance					//
		//	Function GetInstance
		// 機能		モデルマネージャーを渡す		//
		//	I pass the functional model manager
		// 引数		なし							//
		//	No argument
		// 戻り値	モデルマネージャー			//
		//	Returns model manager
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
		// 関数名	LoadModel					//
		//	Function name LoadModel
		// 機能		モデルのロードをする			//
		//	I make a load of functional model
		// 引数		コンテンツマネージャー		//
		//	Argument content manager
		// 戻り値	なし							//
		//	No return value
		//--------------------------------------//
		public void LoadModel(ContentManager contentManager)
		{
			//----Main menu----//
			this.model[(int)ModelName.MAIN_CUBE] = contentManager.Load<Model>(@"モデル\メインメニュー\ui cube");

			// Player
			// Player default pose
            this.model[(int)ModelName.PLAYER_CHARACTER] = contentManager.Load<Model>(@"モデル\Final Animation\With Hit Animation");
            this.model[(int)ModelName.PLAYER_CHARACTER_DEFAULT] = contentManager.Load<Model>(@"モデル\Final Animation\With Hit Animation");

            this.model[(int)ModelName.STAGE_START] = contentManager.Load<Model>(@"モデル\Stages\PartStart");
            this.model[(int)ModelName.STAGE_END] = contentManager.Load<Model>(@"モデル\Stages\PartEnd");
            this.model[(int)ModelName.STAGE_0] = contentManager.Load<Model>(@"モデル\Stages\Part1");
            this.model[(int)ModelName.STAGE_1] = contentManager.Load<Model>(@"モデル\Stages\Part2");
            this.model[(int)ModelName.STAGE_2] = contentManager.Load<Model>(@"モデル\Stages\Part3");
            this.model[(int)ModelName.STAGE_3] = contentManager.Load<Model>(@"モデル\Stages\Part4");
            this.model[(int)ModelName.STAGE_4] = contentManager.Load<Model>(@"モデル\Stages\Part5");
            this.model[(int)ModelName.STAGE_5] = contentManager.Load<Model>(@"モデル\Stages\Part6");
            this.model[(int)ModelName.STAGE_6] = contentManager.Load<Model>(@"モデル\Stages\Part7");

            // skydome
            this.model[(int)ModelName.SKYDOME] = contentManager.Load<Model>(@"モデル\New Skydome");
		}

		//----------------------------------//
		// 関数名	GetModel				//
		//	Function name GetModel
		// 機能		モデルを取得する			//
		//	I get the functional model
		// 引数		モデルの識別ナンバー		//
		//	Identification number of arguments model 
		// 戻り値	モデル					//
		//	Returns model
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
