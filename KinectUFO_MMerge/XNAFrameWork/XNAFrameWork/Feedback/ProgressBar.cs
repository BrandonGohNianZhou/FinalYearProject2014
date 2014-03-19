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
    class ProgressBar
    {
        // screen size
        int w;
        int h;

        // max distance of the game
        public const float MAX_DISTANCE = 15000;

        // distance traveled
        public float Dis_Travel = 0;
        public static float percentage = 0.0f;

        // progress bar varible
        Vector2 bar_pos;
        float bar_length = 0;

        // icon varible
        Vector2 icon_pos;
        float icon_angle;
        bool icon_rotate_left = false;

        // init
        public ProgressBar()
        {
            // screen size
            this.w = Game1.graphics.GraphicsDevice.Viewport.Width;
            this.h = Game1.graphics.GraphicsDevice.Viewport.Height;

            // bar
            this.Dis_Travel = 0;
            this.bar_length = (int)(w * 0.601f);
            this.bar_pos = new Vector2(w * 0.2f, h * 0.86f);

            // icon
            this.icon_pos = new Vector2(w * 0.2f, h * 0.9f);
            this.icon_angle = 0;
            this.icon_rotate_left = false;
        }

        // update
        public void Update(float posZ)
        {
            // distance traveled
            this.Dis_Travel = -posZ + 1000;

            // percentage of gameplay
            percentage = (this.Dis_Travel / MAX_DISTANCE);

            // moving icon
            this.icon_pos.X = (w * 0.2f) + (w * 0.02f) + percentage * (this.bar_length - (w * 0.04f));

            // shake icon
            ShakeIcon();
        }

        // draw
        public void Draw2D() 
        {
            //Game1.debugText.Printf("ip:" + (this.icon_pos.X-150.0f), new Vector2(250, 350));
            //Game1.debugText.Printf("dt:" + Dis_Travel + "/" + MAX_DISTANCE, new Vector2(250, 400));
            //Game1.debugText.Printf("per:" + ((Dis_Travel / MAX_DISTANCE)*100), new Vector2(250, 440));

            // bar
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.P_BAR), this.bar_pos, new Rectangle(0, 0, (int)(w * 0.301f), (int)(h * 0.043f)),
                                Color.White * 1.0f, 0.0f, Vector2.Zero, new Vector2(2.0f, 2.0f), SpriteEffects.None, 1.0f);
                
            // icon
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.P_ICON), this.icon_pos, new Rectangle(0, 0, (int)(w * 0.04f), (int)(h * 0.075f)),
                                Color.White * 1.0f, this.icon_angle, new Vector2(32, 32), new Vector2(1.2f, 1.2f), SpriteEffects.None, 1.0f);
        }

        // shake face icon
        public void ShakeIcon()
        {
            // rotate to the right
            if (this.icon_rotate_left)
            {
                this.icon_angle += 0.02f;
            }
            else
            {
                this.icon_angle -= 0.02f;
            }
            
            // change direction of rotation
            if (this.icon_angle > 0.5f) 
            {
                this.icon_rotate_left = !this.icon_rotate_left;
            } 
            else if (this.icon_angle < -0.5f) 
            {
                this.icon_rotate_left = !this.icon_rotate_left;
            }
        }
    }
}
