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
    class InstructionScene
    {
        Feedback testing;

        // screen size
        int w;
        int h;

        // general values
        int page_no = 0;
        const int MAXPAGE = 5;

        Vector2 page_scale;
        float page_angle;
        float page_alpha;

        // animating
        const float SLIDING_SPEED = 100.0f;
        float page_offset;
        int buffer_time;
        bool overshotleft;
        bool sliding;
        bool slidingleft;
        bool slidingright;
        
        // left hand values
        Vector2 Lhand_pos;
        float Lhand_alpha;
        bool Lhand_fade;
        bool Lhand_move;

        // left hand values
        Vector2 Rhand_pos;
        float Rhand_alpha;
        bool Rhand_fade;
        bool Rhand_move;

        public InstructionScene()
        {
            // screen size
            this.w = Game1.graphics.GraphicsDevice.Viewport.Width;
            this.h = Game1.graphics.GraphicsDevice.Viewport.Height;

            // general values
            this.page_no = 0;
            this.page_scale = Vector2.One;
            this.page_angle = 0.0f;
            this.page_alpha = 1.0f;

            // animating
            this.overshotleft = false;
            this.sliding = false;
            this.slidingleft = false;
            this.slidingright = false;

            // page position
            this.page_offset = 0.0f;
            this.buffer_time = -1;

            //testing = new Feedback(0);
            //testing.Reset2((int)EffectType.PERFECT);

            Hand_Reset();
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
         //   testing.Update(gameTime);

            if (this.sliding)
            {
                // animate
                SlideLeft();
                SlideRight();
                OverShotEffect();
                return;
            }

            // buffertime for swiping
            if (gameTime.TotalGameTime.Seconds - this.buffer_time < 1.5f)
            {
                if (gameTime.TotalGameTime.Seconds < this.buffer_time)
                {
                    this.buffer_time = 0;
                }
                return;
            }

            // player swipe left
            if (Game1.person.RH_Left(gameTime))
            {
                page_no += 1;
                if (page_no > MAXPAGE - 1)
                {
                    page_no = MAXPAGE - 1;
                }
                else
                {
                    this.sliding = true;
                    this.slidingleft = true;
                    this.buffer_time = gameTime.TotalGameTime.Seconds;
                }
			}
            // player swipe right
            else if (Game1.person.LH_Right(gameTime))
            {
                page_no -= 1;
                if (page_no < 0)
                {
                    page_no = 0;
                }
                else
                {
                    this.sliding = true;
                    this.slidingright = true;
                    this.buffer_time = gameTime.TotalGameTime.Seconds;
                }
            }

            // hand instrution animation
            Hand_Left();
            Hand_Right();
		}

        // screen sliding effect
        void SlideLeft()
        {
            // if doing other animation~ return
            if (this.slidingright || !this.slidingleft) return;

            // move screen to left
            this.page_offset -= SLIDING_SPEED;

            // reach de end +50 for the overshot effect
            if (this.page_offset <= -(page_no * w) - 50)
            {
                this.page_offset = -(page_no * w) - 50;
                this.slidingleft = false;
                this.overshotleft = true;
            }
        }
        void SlideRight()
        {
            // if doing other animation~ return
            if (this.slidingleft || !this.slidingright) return;

            // move screen to right
            this.page_offset += SLIDING_SPEED;

            // reach de end +50 for the overshot effect
            if (this.page_offset >= -(page_no * w) + 50)
            {
                this.page_offset = -(page_no * w) + 50;
                this.slidingright = false;
                this.overshotleft = false;
            }
        }
        void OverShotEffect()
        {
            if (this.slidingleft || this.slidingright) return;
            if (!this.sliding) return;

            // over shot right
            if (!this.overshotleft)
            {
                if (this.page_offset <= -(page_no * w))
                {
                    // snap to original
                    this.page_offset = -(page_no * w);
                    this.sliding = false;
                    Hand_Reset();
                }
                else
                {
                    // slide back
                    this.page_offset -= SLIDING_SPEED * 0.2f;
                    this.page_offset -= SLIDING_SPEED * 0.2f;
                }
            }

            // over shot left
            if (this.overshotleft)
            {
                // snap to original
                if (this.page_offset >= -(page_no * w))
                {
                    this.page_offset = -(page_no * w);
                    this.sliding = false;
                    Hand_Reset();
                }
                else
                {
                    // slide back
                    this.page_offset += SLIDING_SPEED * 0.2f;
                    this.page_offset += SLIDING_SPEED * 0.2f;
                }
            }
        }

        // hand icon instruction
        void Hand_Left()
        {
            if (this.page_no >= MAXPAGE - 1) return;

            // fade in
            if (!this.Lhand_fade)
            {
                if (this.Lhand_alpha < 1.0f)
                {
                    this.Lhand_alpha += 0.03f;
                }
                else
                {
                    this.Lhand_fade = true;
                    this.Lhand_move = true;
                }
            }

            // slide left
            if (this.Lhand_move)
            {
                if (Lhand_pos.X < (w * 0.85f) - 70.0f)
                {
                    Lhand_pos.X = (w * 0.85f) - 70.0f;
                    this.Lhand_move = false;
                }
                else
                {
                    Lhand_pos.X -= 2.4f;
                }
            }

            // fade out
            if (this.Lhand_fade && !this.Lhand_move)
            {
                if (this.Lhand_alpha > 0.0f)
                {
                    this.Lhand_alpha -= 0.03f;
                }
                else
                {
                    Lhand_pos.X = (w * 0.85f);
                    this.Lhand_fade = false;
                    this.Lhand_move = false;
                }
            }
        }
        void Hand_Right()
        {
            if (this.page_no == 0) return;

            // fade in
            if (!this.Rhand_fade)
            {
                if (this.Rhand_alpha < 1.0f)
                {
                    this.Rhand_alpha += 0.03f;
                }
                else
                {
                    this.Rhand_fade = true;
                    this.Rhand_move = true;
                }
            }

            // slide right
            if (this.Rhand_move)
            {
                if (Rhand_pos.X > (w * 0.15f) + 70.0f)
                {
                    Rhand_pos.X = (w * 0.15f) + 70.0f;
                    this.Rhand_move = false;
                }
                else
                {
                    Rhand_pos.X += 2.4f;
                }
            }

            // fade out
            if (this.Rhand_fade && !this.Rhand_move)
            {
                if (this.Rhand_alpha > 0.0f)
                {
                    this.Rhand_alpha -= 0.03f;
                }
                else
                {
                    Rhand_pos.X = (w * 0.15f);
                    this.Rhand_fade = false;
                    this.Rhand_move = false;
                }
            }
        }
        void Hand_Reset()
        {
            // left hand value
            this.Lhand_pos = new Vector2((w * 0.85f), (h * 0.2f));
            this.Lhand_alpha = 0.0f;
            this.Lhand_fade = false;
            this.Lhand_move = false;

            //  right hand values
            this.Rhand_pos = new Vector2((w * 0.15f), (h * 0.2f));
            this.Rhand_alpha = 0.0f;
            this.Rhand_fade = false;
            this.Rhand_move = false;
        }

		public void Draw2D()
		{
            // half of screen size
            float halfw = w*0.5f;
            float halfh = h*0.5f;

            // first page
		    Game1.spriteBatch.Draw( TextureManager.GetInstance().GetTexture(TextureName.INSTRUCT_0),
                                    new Vector2(halfw + page_offset, halfh), new Rectangle(0, 0, w, h), Color.White * this.page_alpha,
                                    this.page_angle, new Vector2(halfw, halfh), this.page_scale, SpriteEffects.None, 1.0f
								    );
            // second page
		    Game1.spriteBatch.Draw( TextureManager.GetInstance().GetTexture(TextureName.INSTRUCT_1),
                                    new Vector2(halfw + w + page_offset, halfh), new Rectangle(0, 0, w, h), Color.White * this.page_alpha,
                                    this.page_angle, new Vector2(halfw, halfh), this.page_scale, SpriteEffects.None, 1.0f
								    );

            // third page
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.INSTRUCT_2),
                                    new Vector2(halfw + (w * 2) + page_offset, halfh), new Rectangle(0, 0, w, h), Color.White * this.page_alpha,
                                    this.page_angle, new Vector2(halfw, halfh), this.page_scale, SpriteEffects.None, 1.0f
                                    );

            // forth page
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.INSTRUCT_3),
                                    new Vector2(halfw + (w * 3) + page_offset, halfh), new Rectangle(0, 0, w, h), Color.White * this.page_alpha,
                                    this.page_angle, new Vector2(halfw, halfh), this.page_scale, SpriteEffects.None, 1.0f
                                    );

            // forth page
            Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.INSTRUCT_4),
                                    new Vector2(halfw + (w * 4) + page_offset, halfh), new Rectangle(0, 0, w, h), Color.White * this.page_alpha,
                                    this.page_angle, new Vector2(halfw, halfh), this.page_scale, SpriteEffects.None, 1.0f
                                    );

            // testing
           // Game1.debugText.Printf("page no : " + this.page_no, new Vector2(400, 350), new Color(1.0f, 0.0f, 0.0f));
           // Game1.debugText.Printf("off : " + this.page_offset, new Vector2(400, 380), new Color(1.0f, 0.5f, 1.0f));
           // Game1.debugText.Printf("S : " + this.sliding, new Vector2(400, 410), new Color(1.0f, 0.5f, 1.0f));

            // if page sliding
            if (this.sliding) return;

            // hand left
            if (this.page_no < MAXPAGE - 1)
            {
                Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.INSTRUCT_HAND_LEFT),
                                        this.Lhand_pos, new Rectangle(0, 0, w/9, h/5), Color.White * this.Lhand_alpha,
                                        this.page_angle, new Vector2(w/18, h/10), Vector2.One, SpriteEffects.None, 1.0f
                                        );
            }
            // hand right
            if (this.page_no > 0) 
            {
                Game1.spriteBatch.Draw(TextureManager.GetInstance().GetTexture(TextureName.INSTRUCT_HAND_RIGHT),
                                        this.Rhand_pos, new Rectangle(0, 0, w/9, h/5), Color.White * this.Rhand_alpha,
                                        this.page_angle, new Vector2(w/18, h/10), Vector2.One, SpriteEffects.None, 1.0f
                                        );
            }

           // testing.Draw2D();
		}

		public void Draw3D(GameTime gameTime)
		{

		}
    }
}
