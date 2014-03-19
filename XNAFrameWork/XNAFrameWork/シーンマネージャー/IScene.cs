//--------------------------------------//
// IScene.cs							//
// シーンクラスのインターフェース			//
//	Interface of the scene class
// 作成者:Shin Kouno						//
// 作成日:2013/09/26						//
//--------------------------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;



namespace XNAFrameWork
{
	interface IScene
	{
		// Initialization
		void Initialize();

		// Graphic relationship reading
		void LoadContent(ContentManager content);

		// Graphic destroyed
		void UnLoadContent();

		// Update process
		void Update(GameTime gameTime);

		// Processing of 2D drawing
		void Draw2D(GameTime gameTime);

		// Rendering of 3D
		void Draw3D(GameTime gameTime);
	}
}
