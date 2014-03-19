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
        GOOD = 0,
        PERFECT = 1,
        BOO = 2,

        MAXEFFECT = 3,		// total effect
    };

    class Feedback
    {
        // screen size
        int w;
        int h;

        public bool animate1 = false;
        public bool animate2 = false;
        public Vector2 pos;
        public Vector2 scale;
        public float alpha = 1.0f;
        public float angle = 0.0f;
        public int Type;

        public Feedback(int inType)
        {
            // screen size
            this.w = Game1.graphics.GraphicsDevice.Viewport.Width;
            this.h = Game1.graphics.GraphicsDevice.Viewport.Height;

            animate1 = false;
            animate2 = false;
			pos = new Vector2(400,150);
            scale = Vector2.Zero;
			angle = 0.0f;
			alpha = 1.0f;
            Type = inType;
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
            // play spinning and growing out animation
            this.PlayAnime();
            this.PlayAnime2();
		}

        public void Reset(int intype)
        {
            this.pos.X = w * 0.5f;
            this.pos.Y = h * 0.29f;
            this.animate1 = true;
            this.scale = Vector2.Zero;
            this.angle = 0.0f;
            this.alpha = 1.0f;
            this.Type = intype;
        }
        public void Reset2(int intype)
        {
            Random r = new Random();
            int val = r.Next(-2, 2);

            this.pos.X = w * 0.5f;
            this.pos.Y = h * 0.29f;
            this.animate2 = true;
            this.scale.X = 6.0f;
            this.scale.Y = 6.0f;
            this.angle = val * 0.1f;
            this.alpha = 1.0f;
            this.Type = intype;
        }

        public void PlayAnime()
        {
            if (!this.animate1) return;
            // grow out
            if (this.scale.X < 1.0f)
            {
                this.scale += new Vector2(0.015f, 0.015f);
            }

            // spin out
            if (this.angle < 3.14f * 10)
            {
                this.angle += 0.5f;
            }
            else
            {
                this.angle = 3.14f * 10;

                // fade away after spinning complete
                if (alpha > 0)
                {
                    this.alpha -= 0.5f * 0.015f;
                }
                else
                {
                    this.animate1 = false;
                }
            }
        }
        public void PlayAnime2()
        {
            if (!this.animate2) return;

            // grow in
            if (this.scale.X > 2.0f)
            {
                this.scale -= new Vector2(1.0f, 1.0f);
            }
            else
            {
                // fly off
                if (this.alpha < 0.7f) {
                    this.pos += new Vector2(20.0f, -10.0f);
                    this.scale -= new Vector2(0.01f, 0.01f);
                    this.angle += 0.04f;
                }

                // grow in
                this.scale -= new Vector2(0.006f, 0.006f);

                // fade out
                if (this.alpha > 0.0f)
                {
                    this.alpha -= 0.02f;
                }
                else
                {
                    //Reset2((int)EffectType.BOO);
                    this.animate2 = false;
                }
            }
        }

		public void Draw2D()
		{
            if (!this.animate1 && !this.animate2) return;
            
            switch (Type) 
            {
                // perfect
                case (int)EffectType.PERFECT:
                    Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.PERFECT), this.pos, new Rectangle(0, 0, (int)(w * 0.64f), (int)(h * 0.43f)),
                                        Color.White * alpha, this.angle, new Vector2(256, 256), this.scale, SpriteEffects.None, 1.0f);
                    break;

                // good
                case (int)EffectType.GOOD:
                    Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.GOOD), this.pos, new Rectangle(0, 0, (int)(w * 0.64f), (int)(h * 0.43f)), 
                                        Color.White * alpha, this.angle,new Vector2(256, 256), this.scale, SpriteEffects.None, 1.0f);
                    break;

                // boo
                case (int)EffectType.BOO:
                    Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.BOO), this.pos, new Rectangle(0, 0, (int)(w * 0.64f), (int)(h * 0.43f)),
                                            Color.White * alpha, this.angle, new Vector2(256, 256), this.scale, SpriteEffects.None, 1.0f);
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
