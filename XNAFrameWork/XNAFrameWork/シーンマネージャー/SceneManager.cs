//------------------------------//
// SceneManager.cs				//
// シーンを管理するクラス			//
//	Class that manages the scene
// 作成日:2013/09/26				//
// 作成者:Shin Kouno				//
//------------------------------//


//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAFrameWork;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace XNAFrameWork
{
	class SceneManager
	{
		// Kind of scene
		public enum SCENE_TYPE
        {
			TITLE_SCENE,		// Title scene
			SAMPLE_SCENE1,		// Sample scene
			MAIN_MENU_SCENE,	// Main menu scene
			MAX_SCENE,			// The maximum number of scene
			NONE,				// Nothing contained scene
		};


		// The current scene
		private IScene currentScene;

		// The next scene
		public static SCENE_TYPE nextScene = SCENE_TYPE.SAMPLE_SCENE1;

		// Content Manager
		ContentManager content;

		//------------------------------//
		// Constructor					//
		// I for initializing the scene	//
		//------------------------------//
		public SceneManager(ContentManager content)
		{
			// Branches by entering the next scene
			switch (nextScene)
			{
				// Sample scene
				case SCENE_TYPE.SAMPLE_SCENE1:
					currentScene = new SampleScene1();
					break;

				// Main menu scene
				case SCENE_TYPE.MAIN_MENU_SCENE:
					currentScene = new MainMenu();
					break;

				// Title scene
				case SCENE_TYPE.TITLE_SCENE:
					currentScene = new TitleScene();
					break;

				// To the sample if it is not specified
				default:
					currentScene = new SampleScene1();
					break;
			}
			nextScene = SCENE_TYPE.NONE;		// Nothing on

			// Save content manager
			this.content = content;
		}

		//----------------------------------------------//
		// 関数名	Initialize							//
		//	Function Initialize
		// 機能		現在のシーンの初期化処理を呼び出す		//
		//	Call the initialization process of the scene of the current function
		// 引数		なし									//
		//	No argument
		// 戻り値	なし									//
		//	No return value
		//----------------------------------------------//
		public void Initialize()
		{
			// Initialization process of the current scene
			currentScene.Initialize();
		}

		//--------------------------------------------------------------//
		// 関数名	LoadContent											//
		//	Function LoadContent
		// 機能		現在のシーンのグラフィック読み込み処理を呼び出す		//
		//	I call the process of reading the graphic scene of the current function
		// 引数		コンテンツ管理マネージャー							//
		//	Argument content management Manager
		// 戻り値	なし													//
		//	No return value
		//--------------------------------------------------------------//
		public void LoadContent(ContentManager content)
		{
			// Graphic reading process of the current scene
			currentScene.LoadContent(content);
		}

		//------------------------------------------------------//
		// 関数名	UnLoadContent								//
		//	Function name UnLoadContent
		// 機能		現在のシーンのグラフィック破棄を呼び出す		//
		//	I call the graphic scene of the destruction of the current function
		// 引数		なし											//
		//	No argument
		// 戻り値	なし											//
		//	No return value
		//------------------------------------------------------//
		public void UnLoadContent()
		{
			// Graphic destruction of the current scene
			currentScene.UnLoadContent();
		}

		//----------------------------------------------//
		// 関数名	Update								//
		//	Function Update
		// 機能		現在のシーンの更新処理を呼び出す		//
		//	I call the process of updating the current scene of the function
		// 引数		なし									//
		//	No argument
		// 戻り値	なし									//
		//	No return value
		//----------------------------------------------//
		public void Update(GameTime gameTime)
		{
			// Update process of the current scene
			currentScene.Update(gameTime);

			// Create a scene if there is a scene that is reserved
			if (nextScene != SCENE_TYPE.NONE)
			{
				// Create a scene
				CreateScene();
			}
		}

		//----------------------------------------------//
		// 関数名	Draw2D								//
		//	Function name Draw2D
		// 機能		現在のシーンの2D描画処理を呼び出す		//
		//	I call the 2D drawing of the scene processing function of current
		// 引数		なし									//
		//	No argument 
		// 戻り値	なし									//
		//	No return value
		//----------------------------------------------//
		public void Draw2D(GameTime gameTime)
		{
			// 2D drawing process of the current scene
			currentScene.Draw2D(gameTime);
		}

		//----------------------------------------------//
		// 関数名	Draw3D								//
		//	Function name Draw3D
		// 機能		現在のシーンの3D描画処理を呼び出す		//
		//	I call the 3D rendering of the scene of the current function
		// 引数		なし									//
		//	No argument
		// 戻り値	なし									//
		//	No return value
		//----------------------------------------------//
		public void Draw3D(GameTime gameTime)
		{
			// 3D rendering of the current scene
			currentScene.Draw3D(gameTime);
		}


		//--------------------------------------//
		// 関数名	CreateScene					//
		//	Function name CreateScene
		// 機能		予約されたシーンを作成		//
		//	Create a scene that has been reserved function
		// 引数		なし							//
		//	No argument
		// 戻り値	なし							//
		//	No return value
		//--------------------------------------//
		public void CreateScene()
		{
			// Delete the current scene
			currentScene = null;

			// Create a scene that is reserved
			switch (nextScene)
            {
				// Sample scene
				case SCENE_TYPE.SAMPLE_SCENE1:
					currentScene = new SampleScene1();
					break;

				// Sample Scene 2
				case SCENE_TYPE.MAIN_MENU_SCENE:
					currentScene = new MainMenu();
					break;

				case SCENE_TYPE.TITLE_SCENE:
					currentScene = new TitleScene();
					break;
			}

			// I to the scene without reservation the following
			nextScene = SCENE_TYPE.NONE;

			// I call the initialization processing of the scene that contains new
			currentScene.Initialize();
			// I call the processing load of the scene that contains new
			currentScene.LoadContent(this.content);
		}
	}
}
