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
    // type of feedback
    public enum EffectType
    {
        GOOD,
        PERFECT,
        BOO,
        NONE
    };

    class Feedback
    {

        // screen size
        int w;
        int h;

        EffectType Type;

        TextureAnimation2D Perfect, Good, Boo;

        public Feedback(EffectType inType)
        {
            // screen size
            this.w = Game1.graphics.GraphicsDevice.Viewport.Width;
            this.h = Game1.graphics.GraphicsDevice.Viewport.Height;

            Type = inType;

            Perfect = Good = Boo = null;
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
            if(Perfect != null) Perfect.Update(gameTime.ElapsedGameTime.Milliseconds);
            if(Good != null) Good.Update(gameTime.ElapsedGameTime.Milliseconds);
            if(Boo != null) Boo.Update(gameTime.ElapsedGameTime.Milliseconds);
		}

        public void Reset(EffectType intype)
        {
            this.Type = intype;

            if (Perfect != null) Perfect.Reset();
            if (Good != null) Good.Reset();
            if (Boo != null) Boo.Reset();
        }

        private void LoadTextures()
        {
            Perfect = new TextureAnimation2D();
            Good = new TextureAnimation2D();
            Boo = new TextureAnimation2D();

            Perfect.Load(TextureManager.GetInstance().GetTexture(TextureName.PERFECT), 4, 200);
            Good.Load(TextureManager.GetInstance().GetTexture(TextureName.GOOD), 4, 200);
            Boo.Load(TextureManager.GetInstance().GetTexture(TextureName.BOO), 5, 200);

        }

		public void Draw2D()
		{
            if (Perfect == null || Good == null || Boo == null) LoadTextures();

            switch (Type) 
            {
                // perfect
                case EffectType.PERFECT:
                    if(Perfect != null) Perfect.Draw(Game1.spriteBatch, new Vector2(w/2.0f, h/4.0f), 0, new Vector2(256, 256), new Vector2(1, 1), 1.0f);
                    break;

                // good
                case EffectType.GOOD:
                    if(Good != null) Good.Draw(Game1.spriteBatch, new Vector2(w/2.0f, h/4.0f), 0, new Vector2(256, 256), new Vector2(1, 1), 1.0f);
                    break;

                // boo
                case EffectType.BOO:
                    if(Boo != null) Boo.Draw(Game1.spriteBatch, new Vector2(w / 2.0f, h / 4.0f), 0, new Vector2(256, 256), new Vector2(1, 1), 1.0f);
                    break;

                default:
                    break;
            }
		}

		public void Draw3D(GameTime gameTime)
		{

		}
    }
}
