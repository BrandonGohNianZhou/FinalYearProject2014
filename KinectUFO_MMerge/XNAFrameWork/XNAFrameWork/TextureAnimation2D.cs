using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNAFrameWork
{
    class TextureAnimation2D
    {
        private int MaxFrames;
        private Texture2D Texture;
        private float frameTime;

        private int CurrentFrame;
        private float CurrentTime;

        bool Done;
        bool Loaded;

        public TextureAnimation2D()
        {
            Loaded = false;
        }

        public void Load(Texture2D Texture, int MaxFrames, float frameTime)
        {
            this.Texture = Texture;
            this.MaxFrames = MaxFrames;
            this.frameTime = frameTime;

            Loaded = true;
        }

        public void Reset()
        {
            CurrentTime = 0;
            CurrentFrame = 0;

            Done = false;
        }

        public void Update(float deltaTime)
        {
            if (Done || !Loaded) return;

            CurrentTime += deltaTime;

            if (CurrentTime >= frameTime)
            {
                CurrentTime = 0;
                CurrentFrame++;

                if (CurrentFrame > MaxFrames)
                {
                    Done = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 ScreenPos, float Rotation, Vector2 Origin, Vector2 Scale, float Depth)
        {
            if (Done || !Loaded) return;

            int width = Texture.Width / MaxFrames;

            Rectangle Rect = new Rectangle(width * CurrentFrame, 0, width, Texture.Height);
            spriteBatch.Draw(Texture, ScreenPos, Rect, Color.White, Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }
    }
}
