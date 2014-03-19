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
    class LifePoint
    {
        // screen size
        int w;
        int h;

        // life icon
        public static int LP = 5;
        Vector2 life_pos;

        // init
        public LifePoint()
        {
            // screen size
            this.w = Game1.graphics.GraphicsDevice.Viewport.Width;
            this.h = Game1.graphics.GraphicsDevice.Viewport.Height;

            // life icon
            LP = 5;
            life_pos = new Vector2((w * 0.7f), h * 0.07f);
        }

        // update
        public void Update(float posZ)
        {
        }

        // draw life icon
        public void Draw2D() 
        {
            for (int i = 0; i < LP; ++i)
            {
                this.life_pos.X = (w * 0.9f) - (w * 0.05f) * i;
                Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.P_ICON), this.life_pos, new Rectangle(0, 0, (int)(w * 0.04f), (int)(h * 0.075f)),
                                    Color.White * 1.0f, 0.0f, Vector2.Zero, new Vector2(1.2f, 1.2f), SpriteEffects.None, 1.0f);
            }
        }
    }
}
