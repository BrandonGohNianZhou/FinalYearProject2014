using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAFrameWork;
using Microsoft.Xna.Framework.Content;


namespace XNAFrameWork
{
	class TitleScene : IScene
	{
		// Title sprite
		SpriteObject title;

		public TitleScene()
		{
			title = new SpriteObject();
			title.pos = new Vector2(0,0);
			title.scale = Vector2.One;
			title.angle = 0.0f;
			title.alpha = 1.0f;
		}

		public void Initialize()
		{
		}

		public void LoadContent(ContentManager content)
		{
		}

		public void UnLoadContent()
		{
		}
		public void Update(GameTime gameTime)
		{
			if (Game1.person.Pushed())
			{
                //SceneManager.nextScene = SceneManager.SCENE_TYPE.MAIN_MENU_SCENE;
                SceneManager.nextScene = SceneManager.SCENE_TYPE.SAMPLE_SCENE1;
			}
		}

		public void Draw2D(GameTime gameTime)
		{
			Game1.spriteBatch.Draw(
									TextureManager.GetInstance().GetTexture(TextureName.TITLE),
									this.title.pos,
									new Rectangle(0, 0, 1600, 900),
									Color.White * title.alpha,
									this.title.angle,
									new Vector2(0, 0),
									this.title.scale,
									SpriteEffects.None,
									1.0f
									);
		}

		public void Draw3D(GameTime gameTime)
		{

		}
	}
}
