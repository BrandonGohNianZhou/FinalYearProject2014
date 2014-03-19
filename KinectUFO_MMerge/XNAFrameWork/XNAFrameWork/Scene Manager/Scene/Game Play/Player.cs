//--------------------------//
//Player.cs					//
//Player Class				//
// 制作日:2013/10/02			//
// 制作者:Shin Kouno			//
//--------------------------//

//----------------------//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkinnedModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAFrameWork;
using Microsoft.Xna.Framework.Input;
using Microsoft.Kinect;

namespace XNAFrameWork
{
	class Player : IObject
	{
		#region Field

		// Model
		// Model of Yes pose
		Model Skeleton_Player;
		// Mode without a pause
		Model Model_Player;

		// Position of the last of the skeleton
		//Vector3 oldSkeletonPos = new Vector3(0,0,0);

		// Keep class to move the model to fit the data skeleton of Kinect
		static public AvateerHandler avateerHandler;

		#region Position

		private Vector3 pos;

		/// Get and set position
		public Vector3 Pos
		{
			get { return this.pos; }
			set { this.pos = value; }
		}

		/// Get and set position X
		public float PosX
		{
			get { return this.pos.X; }
			set { this.pos.X = value; }
		}
		/// Get and set position Y
		public float PosY
		{
			get { return this.pos.Y; }
			set { this.pos.Y = value; }
		}
		/// Get and set position Z
		public float PosZ
		{
			get { return this.pos.Z; }
			set { this.pos.Z = value; }
		}

		#endregion

		#region Scale
		private Vector3 scale;

		// Get and set the scale
		public Vector3 Scale
		{
			get { return this.scale; }
			set { this.scale = value;}
		}

		// Get and set the scale x
		public float scaleX
		{
			get { return this.scale.X; }
			set { this.scale.X = value; }
		}
		// Get and set the scale y
		public float scaleY 
		{
			get { return this.scale.Y; }
			set { this.scale.Y = value; }
		}
		// Get and set the scale z
		public float scaleZ
		{
			get { return this.scale.Z;}
			set { this.scale.Z = value;}
		}
		#endregion

		#region Rotation
		private Vector3 rotation;

		// Acquisition and rotation settings
		public Vector3 Rotation
		{
			get { return MyMathHelper.Vector3ToDegree(this.rotation); }
			set { this.rotation = MyMathHelper.Vector3ToRadian(value);}
		}

		// set or get the x-axis rotation
		public float rotationX
		{
			get { return MathHelper.ToDegrees(this.rotation.X);}
			set { this.rotation.X = MathHelper.ToRadians(value);}
		}
		// set or get the y-axis rotation
		public float rotationY
		{
			get { return MathHelper.ToDegrees(this.rotation.Y); }
			set { this.rotation.Y = MathHelper.ToRadians(value);}
		}
		// set or get the z-axis rotation
		public float rotationZ
		{
			get { return MathHelper.ToDegrees(this.rotation.Z); }
			set { this.rotation.Z = MathHelper.ToRadians(value);}
		}
		#endregion

		#region Offset
		private Vector3 offset;

		// Set the offset
		public Vector3 Offset
		{
			get { return this.offset; }
		}

		// Set the offset x
		public float offsetX
		{
			get { return this.offset.X; }
		}
		// Set the offset y
		public float offsetY
		{
			get { return this.offset.Y; }
		}
		// Set the offset z
		public float offsetZ
		{
			get { return this.offset.Z; }
		}
		#endregion

		#region Speed

		private Vector3 vel;

		/// Get and set the speed
		public Vector3 Vel
		{
			get { return this.vel; }
			set { this.vel = value; }
		}

		/// Get and set the speed of the X
		public float VelX
		{
			get { return this.vel.X; }
			set { this.vel.X = value; }
		}
		/// Get and set the speed of the Y
		public float VelY
		{
			get { return this.vel.Y; }
			set { this.vel.Y = value; }
		}
		/// Get and set the speed of the Z
		public float VelZ
		{
			get { return this.vel.Z; }
			set { this.vel.Z = value; }
		}

		#endregion

		#region Jump

		float	jumpPower = 0.0f;		// The power of the jump
		bool	jumpFlag = false;		// ( Jumping, false: the groud true ) or are jumping

		#endregion

		#endregion

		#region Constructor

		public Player()
		{
			this.pos = new Vector3(0.0f,0.0f,0.0f);
			this.scale = new Vector3(1.0f,1.0f,1.0f);
			this.rotation = MyMathHelper.Vector3ToRadian(new Vector3(0.0f,180.0f,0.0f));

			// Load model of player
			Model_Player = ModelManager.GetInstance().GetModel(ModelName.PLAYER_CHARACTER);
			Skeleton_Player = ModelManager.GetInstance().GetModel(ModelName.PLAYER_CHARACTER);

            // Whether or not to jump
            this.isJump = false;

			// 初期化
			this.Initialize();
		}

		// 引数つきコンストラクタ
		public Player(Vector3 pos, Vector3 scale, Vector3 rotation)
		{
			// もらってきた各値を代入
			this.pos = pos;
			this.scale = scale;
			this.rotation = MyMathHelper.Vector3ToRadian(rotation);
			this.offset = scale * 0.5f;

            this.vel = new Vector3(0.0f, 0.0f, 0.0f);
            this.vel.Z = MIN_SPEED;

			// プレイヤーのモデルをロード
			Model_Player = ModelManager.GetInstance().GetModel(ModelName.PLAYER_CHARACTER_DEFAULT);
			Skeleton_Player = ModelManager.GetInstance().GetModel(ModelName.PLAYER_CHARACTER);

			// ジャンプしているかどうか
			this.isJump = false;
            this.jumpCD = false;
            this.CDtime = 0.0f;

			// Initialization
			this.Initialize();

            // testing
            this.wpos = Vector3.Zero;
            this.wscale = Vector3.One;
            this.wrotation = Vector3.Zero;

            Life = new LifePoint();
            this.Invincible = false;
            this.InvinTime = 0.0f;

            //SkinningData skinningData = Skeleton_Player.Tag as SkinningData;
            //animationPlayer = new AnimationPlayer(skinningData);
            //clip = skinningData.AnimationClips["Take 001"];
            //animationPlayer.StartClip(clip);
		}
		#endregion

        #region testing

        // life
        LifePoint Life;
        public bool Invincible = false;
        float InvinTime = 0.0f;

        // total distance travel
        public float TotalDis = 0;

        // speed
        //public static float extraspeed = 0.0f;
        const float MIN_SPEED = -100.0f;

        // new code
        // limit to road side is -20, 20
        const float STAGE_SIDE_LEFT = -20.0f;		// Left Edge
        const float STAGE_SIDE_RIGHT = 20.0f;		// Right Edge

        // play position calibration
        public bool tookstartpos = false;
        public Vector3 startpos;
        public float minx;
        public float maxx;

        // kicking flags
        bool StopKick = false;
        int timeStart;

        // jump flags
        const float JUMP_VALUE = 0.1f;
        bool isJump;
        bool jumpFlag2 = false;
        bool jumpCD;
        float CDtime;

        // duck flags
        const float DUCK_VALUE = -1.0f;
        public bool isDuck;

        // animation
        public AnimationPlayer animationPlayer;
        AnimationClip clip;
        bool DuckAnimate = false;
        bool JumpAnimate = false;
        bool KnockAnimate = false;
        public bool KickAnimate = false;

        // world matrix
        private Matrix world = Matrix.Identity;
        Vector3 wpos;
        Vector3 wscale;
        Vector3 wrotation;

        #endregion

		#region Function

		//--------------------------//
		//	Function initialize		//
		//	Initialization function	//
		//	No argument				//
		//	No return value			//
		//--------------------------//
		private void Initialize()
		{
			// I want to initialize the class to handle the skeleton data
			var boneIndices = new Dictionary<int, BoneType>
            {
                // Number of bones model has because it is different for each model,
                // I want to map the number and types of bone
                //{ 00, BoneType.Hip },
                //{ 01, BoneType.Spine },
                //{ 02, BoneType.UpperArmRight },
                //{ 03, BoneType.ForeArmRight },
                //{ 04, BoneType.HandRight },
                //{ 05, BoneType.UpperArmLeft },
                //{ 06, BoneType.ForeArmLeft },
                //{ 07, BoneType.HandLeft },
                //{ 08, BoneType.Head },
                //{ 09, BoneType.ThighLeft },
                //{ 10, BoneType.ShinLeft },
                //{ 11, BoneType.FootLeft },
                //{ 12, BoneType.ThighRight },
                //{ 13, BoneType.ShinRight },
                //{ 14, BoneType.FootRight }
                
                { 00, BoneType.SkateBoard },

                { 01, BoneType.Hip },
                
                { 02, BoneType.ThighLeft },
                { 03, BoneType.ShinLeft },
                { 04, BoneType.AnkleLeft },
                { 05, BoneType.FootLeft },
            
                { 06, BoneType.ThighRight },
                { 07, BoneType.ShinRight },
                { 08, BoneType.AnkleRight },
                { 09, BoneType.FootRight },

                { 10, BoneType.Spine },
                { 11, BoneType.ShoulderCenter },
                { 12, BoneType.Neck },
                { 13, BoneType.Head },
            
                { 14, BoneType.UpperArmLeft },
                { 15, BoneType.ForeArmLeft },
                { 16, BoneType.HandLeft },
            
                { 17, BoneType.UpperArmRight },
                { 18, BoneType.ForeArmRight },
                { 19, BoneType.HandRight },

            };
			// Initializes the handler Avatar
			avateerHandler = new AvateerHandler(Skeleton_Player, boneIndices);


            // frame specific animation
            // kicking 1 to 71 = 2.3seconds
            // jumping 71 to 156 = 2.5seconds
            // ducking 156 to 190 = 1.2seconds
            // knockback 160 to 185 = 0.5seconds

            //Boosting - 1 to 50
            //Jumping - 50 to 110
            //Ducking - 110 to 160

            SkinningData skinningData = Skeleton_Player.Tag as SkinningData;
            animationPlayer = new AnimationPlayer(skinningData);
            clip = skinningData.AnimationClips["Take 001"];
            animationPlayer.StartClip(clip);
            this.animationPlayer.Update(new GameTime().ElapsedGameTime, true, this.world);
		}

		//--------------------------------------//
		//	Function name ControllPlayer		//
		//	Control function of Player			//
		//	No argument							//
		//	No return value						//
		//--------------------------------------//
		public void ControllPlayer()
		{
			if (MyKeyboard.IsPress(Keys.Right))
			{
				this.pos.X += 3.0f;
			}
			else if (MyKeyboard.IsPress(Keys.Left))
			{
				this.pos.X -= 3.0f;
			}

			if (MyKeyboard.IsPress(Keys.Up))
			{
				this.pos.Z -= 13.0f;
			}
			else if (MyKeyboard.IsPress(Keys.Down))
			{
				this.pos.Z += 3.0f;
			}
			this.Jump();
		}

		//--------------------------------------------------------------//
		//Function name ControllPlayerWithSkeleton						//
		//Move a character it in concert with the functional skeleton data
		//No argument													//
		//No return value												//
		//--------------------------------------------------------------//
        public void ControllPlayerWithSkeleton(GameTime gameTime)
		{
			// I synchronize the movement of the skeleton and player
			//this.pos.X = Game1.person.GetJointPos(JointType.HipCenter).X * 30.0f;

            // testing
            if (!isJump && !isDuck) 
            {
                ControlByTilting(gameTime);
            }

            //// moving the player left and right after knowing the maximium space the player has
            //if (Game1.person.GetJointPos(JointType.HipCenter).X > 0) 
            //{
            //    // to the right
            //    this.pos.X = Game1.person.GetJointPos(JointType.HipCenter).X * (STAGE_SIDE_RIGHT / maxx);
            //} 
            //else
            //{
            //    // to the left
            //    this.pos.X = Game1.person.GetJointPos(JointType.HipCenter).X * (STAGE_SIDE_LEFT / minx);
            //}

            // moving player up and down
            if (Game1.person.GetJointPos(JointType.HipCenter).Y < startpos.Y && this.jumpFlag == false)
            {
                // player moving down~ offset model up
                this.pos.Y = startpos.Y - Game1.person.GetJointPos(JointType.HipCenter).Y;
            }
            else if (Game1.person.GetJointPos(JointType.HipCenter).Y > startpos.Y && this.jumpFlag == false)
            {
                // player moving up~ offset model down
                this.pos.Y = Game1.person.GetJointPos(JointType.HipCenter).Y - startpos.Y;
            }

			this.JudgJump();
            this.JudgDuck();
			this.SkeletonJump();
		}

        public void ControlByTilting(GameTime gameTime)
        {
            if (Game1.kinect.skeleton == null) return;

            int speed = 100;
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //// moving the player left and right after knowing the maximium space the player has
            //this.pos.X += (Game1.person.GetJointPos(JointType.ShoulderCenter).X - Game1.person.GetJointPos(JointType.HipCenter).X) * (float)gameTime.ElapsedGameTime.TotalSeconds * speed;

            //Check Left Or Right Hip is infront

            bool Left = false;
            //If Left Hip is nearer to kinect than right hip
            if (Game1.kinect.skeleton.Joints[JointType.HipLeft].Position.Z < Game1.kinect.skeleton.Joints[JointType.HipRight].Position.Z)
            {
                Left = true;
            }

            float ShoulderX, HipX;

            //If left hip infront
            if (Left)
            {
                //Take shoulder Left
                ShoulderX = Game1.kinect.skeleton.Joints[JointType.ShoulderLeft].Position.X;
                //Take Hip Left
                HipX = Game1.kinect.skeleton.Joints[JointType.HipLeft].Position.X;
            }

            //Right hip infront
            else
            {
                //Take shoulder right
                ShoulderX = Game1.kinect.skeleton.Joints[JointType.ShoulderRight].Position.X;
                //Take hip right
                HipX = Game1.kinect.skeleton.Joints[JointType.HipRight].Position.X;
            }

            //Compare difference between shoulder X & hip X and move player left/right
            this.pos.X += (ShoulderX - HipX) * deltaTime * speed;


            //Keyboard controls for testing
            int run = 1;
            if (MyKeyboard.IsPressed(Keys.LeftShift)) run = 5;

            if (MyKeyboard.IsPress(Keys.Right)) this.pos.X += deltaTime * speed / 2 * (run);
            else if (MyKeyboard.IsPress(Keys.Left)) this.pos.X += deltaTime * speed * -1 / 2 * (run);
        }

		//----------------------------------------------//
		//	Function name MovingPlayer					//
		//	I move to force the function Player			//
		//	No argument									//
		//	No return value								//
		//----------------------------------------------//
		public void MovingPlayer(GameTime gameTime)
		{
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			this.pos.X += this.vel.X * deltaTime;
            this.pos.Y += this.vel.Y * deltaTime;
            this.pos.Z += this.vel.Z * deltaTime;

            this.TotalDis += Math.Abs(this.vel.Z * deltaTime);
		}

		//------------------------------------------//
		//	Function name Jump						//
		//	I to jump the function Player			//
		//	No argument								//
		//	No return value							//
		//------------------------------------------//
		public void Jump()
		{
			// Jump key when pressed while you are on the ground
			if (MyKeyboard.IsPressed(Keys.J) && this.jumpFlag == false)
			{
				// I will give your jumping
				this.jumpPower = 1.0f;
				// And on the flag of jumping
				this.jumpFlag = true;
			}
			// If jumping
			if (this.jumpFlag == true)
			{
				// I reduce the jumping ability
				jumpPower -= 0.05f;
			}
			// I float your jumping minute player
			this.pos.Y += jumpPower;
			// Once a player has down to earth by a jump in
			if (this.pos.Y <= 0.0f && this.jumpFlag == true)
			{
				// To landing
				jumpPower = 0.0f;
				// I want to be able to jump again
				jumpFlag = false;
				// Fixed to the ground position of the player
				this.pos.Y = 0.0f;

                JumpAnimate = false;
			}
		}

		//------------------------------------------//
		//	Function name SkeletonJump				//
		//	Jump with a skeleton function			//
		//	No argument								//
		//	No return value							//
		//------------------------------------------//
		public void SkeletonJump()
		{
			// Operation of the jump If you are having
			if (this.isJump == true && this.jumpFlag == false)
			{
				// I will give your jumping
				this.jumpPower = 1.0f;

				// The ON the flag of jumping
				this.jumpFlag = true;
			}
			// If jumping
			if (this.jumpFlag == true)
			{
				// 0.05f to reduce the jumping ability
                jumpPower -= 0.05f;

				// I float your jumping minute player
                this.pos.Y += jumpPower;
			}

			// Once a player has down to earth by a jump in
			if (this.pos.Y <= 0.0f && this.jumpFlag == true)
			{
				// To landing
				jumpPower = 0.0f;
				// I want to be able to jump again
				this.isJump = false;
				// Landing
				this.jumpFlag = false;
				// Fixed to the ground position of the player
				//this.pos.Y = 0.0f;

                // jump cool down
                this.jumpCD = true;
			}
		}

		//------------------------------------------------------//
		//	Function name JudgJump								//
		//	Player judgment function of whether the jump		//
		//	No argument											//
		//	No return value										//
		//------------------------------------------------------//
		private void JudgJump()
		{
            if (this.isDuck || this.jumpCD || this.DuckAnimate) return;

			// If it is not jumping if
			if (this.isJump == false && this.jumpFlag == false)
			{
                float SpineDiffY = Game1.person.GetJointPos(JointType.Spine).Y - Game1.person.GetSecondJointPos(JointType.Spine).Y;
                float JUMP_DIFF = 0.1f;
				
				// If so apart predetermined number or more than the current values ​​of one second before
                if (SpineDiffY >= JUMP_DIFF || MyKeyboard.IsPress(Keys.Space))
                {
                    if (!jumpFlag2)
                    {
						// I will jump
                        this.isJump = true;
                        this.jumpFlag2 = true;
                        this.JumpAnimate = true;

                        // start jump animation ~ jumping 71 to 156 = 2.5seconds
                        animationPlayer.StartClip(clip);
                        animationPlayer.currentTimeValue = TimeSpan.FromSeconds(85 / 30);
                    }
                    else
                    {
                        this.isJump = false;
                        this.JumpAnimate = false;
                    }
                }
                else
                {
                    if (this.jumpFlag2) this.jumpFlag2 = false;
                    this.JumpAnimate = false;
                }
			}
		}

        // check player ducking
        private void JudgDuck()
        {
            // if is jumping 
            if (this.isJump || this.jumpFlag || this.jumpCD || this.JumpAnimate)
            {
                this.isDuck = false;
                return;
            }
			
            float SpineDiffY = Game1.person.GetSecondJointPos(JointType.Spine).Y - Game1.person.GetJointPos(JointType.Spine).Y;
            float DUCK_DIFF = 0.1f;

			// If so apart predetermined number or more than the current values ​​of one second before
            if ((SpineDiffY) >= DUCK_DIFF || MyKeyboard.IsPress(Keys.Down))
            {
                if (!isDuck)
                {
					// I will jump
                    this.isDuck = true;
                    this.KickAnimate = false;
                    this.DuckAnimate = true;

                    // start duck animation ~ ducking 156 to 190 = 1.2seconds
                    animationPlayer.StartClip(clip);
                    animationPlayer.currentTimeValue = TimeSpan.FromSeconds(140 / 30);
                }
            }
			
			else if (isDuck || MyKeyboard.IsRelease(Keys.Down))
			{
				this.isDuck = false;
				this.KickAnimate = true;
			}
        }

		//------------------------------//
		//	Function Update				//
		//	Update function of Player	//
		//	No argument					//
		//	No return value				//
		//------------------------------//
        public void Update(GameTime gameTime)
        {
            if (Game1.person.GetJointPos(JointType.Spine).Y == 0.0f)
            {
                Game1.paused = true;
            }

            else
            {
                Game1.paused = false;
                Game1.pausetime = 0;
            }

            //extraspeed = -10.0f;

            //// limit max speed
            //if (extraspeed < -10.0f)
            //{
            //    extraspeed = -10.0f;
            //}

            //// maintain minimium speed limit
            //if (vel.Z > MIN_SPEED + extraspeed)  // -2.5
            //{
            //    vel.Z -= 1.0f;
            //}

            // jump cool down
            if (this.jumpCD) 
            {
                if (this.CDtime > 60.0f)
                {
                    this.CDtime = 0.0f;
                    this.jumpCD = false;
                }
                else
                {
                    this.CDtime += 1.0f;
                }
            }

            // if animation clip not null
            if (animationPlayer != null)
            {
                // update animation
                AnimationUpdate(gameTime);
            }

            // check invincible
            if (this.Invincible)
            {
                // check for resetted time
                if (gameTime.TotalGameTime.Seconds < this.InvinTime) 
                {
                    this.InvinTime = 0.0f;
                }

                // invunerable for few seconds
                if (gameTime.TotalGameTime.Seconds - this.InvinTime > 2.0f)
                {
                    this.Invincible = false;
                }
            }

			//  When the skeleton is successfully acquired
            if (Game1.kinect.skeleton != null)
            {
                // Update of Avatar
                avateerHandler.Update(Game1.kinect.skeleton, this.animationPlayer, JumpAnimate, KickAnimate, DuckAnimate, KnockAnimate);
            }
        }

		public int T_Pose_Time = 0;
		private int T_Pose_Start = 2000;
        // checking for T-Pose
        public bool InitStartPos(GameTime gameTime)
        {
            if (MyKeyboard.IsPress(Keys.Enter))
            {
                this.tookstartpos = true;
            }
			
            // if completed takeing initial position
            if (this.tookstartpos) return true;

            // if player hand in "T" position
            if (avateerHandler.GetJointRotation(BoneType.UpperArmLeft).Z > 0.5f && avateerHandler.GetJointRotation(BoneType.UpperArmRight).Z < -0.5f &&
                avateerHandler.GetJointRotation(BoneType.ForeArmLeft).X > -0.3 && avateerHandler.GetJointRotation(BoneType.ForeArmRight).X > -0.3)
            {
                // completed takeing initial position
                this.startpos = Game1.person.GetJointPos(JointType.HipCenter);
                this.minx = Game1.person.GetJointPos(JointType.ElbowLeft).X;
                this.maxx = Game1.person.GetJointPos(JointType.ElbowRight).X;

                // init life
                LifePoint.LP = 5;
                this.TotalDis = 0;

                T_Pose_Time += gameTime.ElapsedGameTime.Milliseconds;
            }

            else T_Pose_Time = 0;

            if (T_Pose_Time >= T_Pose_Start)
            {
                T_Pose_Time = 0;
                this.tookstartpos = true;
                return true;
            }

            return false;
        }

        // player knockback
        public void KnockBack(GameTime gameTime)
        {
            this.Invincible = true;
            this.InvinTime = gameTime.TotalGameTime.Seconds;
            this.KnockAnimate = true;
            this.KickAnimate = false;
            vel.Z *= -1.0f;

            // knock back animation ~ knockback 160 to 185 = 0.5seconds
            animationPlayer.StartClip(clip);
            animationPlayer.currentTimeValue = TimeSpan.FromSeconds(162.0f / 30.0f);
            this.animationPlayer.Update(gameTime.ElapsedGameTime, true, this.world);
        }

        // animation handler
        public void AnimationUpdate(GameTime gameTime)
        {
            this.world = MyMathHelper.SetWorldMatrix(wpos, wscale, wrotation);

            // kicking animation ~ kicking 1 to 71 = 2.3seconds
            if (!this.JumpAnimate && !this.DuckAnimate && this.KickAnimate && !this.KnockAnimate)
            {
                // checking for time null
                if (gameTime.TotalGameTime.Seconds < this.timeStart)
                {
                    this.timeStart = 0;
                }
                if (!this.StopKick)
                {
                    // reach the end of kicking animation
                    if (animationPlayer.currentTimeValue >= TimeSpan.FromSeconds(46 / 30))
                    {
                        this.StopKick = true;
                        this.timeStart = gameTime.TotalGameTime.Seconds;
                    }
                    // update animation
                    this.animationPlayer.Update(gameTime.ElapsedGameTime, true, this.world);
                }
                else
                {
                    if (gameTime.TotalGameTime.Seconds - this.timeStart > 1)
                    {
                        this.StopKick = false;
                        animationPlayer.StartClip(clip);
                        this.animationPlayer.Update(gameTime.ElapsedGameTime, true, this.world);
                    }
                }
            }

            // jumping animation ~ jumping 71 to 156 = 2.5seconds
            if (this.JumpAnimate && !this.KnockAnimate)
            {
                // reach the end of jumping animation
                if (animationPlayer.currentTimeValue >= TimeSpan.FromSeconds(106 / 30))
                {
                    this.JumpAnimate = false;
                }
                // update animation
                this.animationPlayer.Update(gameTime.ElapsedGameTime, true, this.world);
            }

            // ducking animation ~ ducking 156 to 190 = 1.2seconds
            if (this.DuckAnimate && !this.KnockAnimate)
            {
                // reach halfway through duck animation (still ducking)
                if (animationPlayer.currentTimeValue <= TimeSpan.FromSeconds(123.0f / 30.0f))
                {
                    this.animationPlayer.Update(gameTime.ElapsedGameTime, true, this.world);
                }

                // not ducking anymore, return back to standing position
                else if (!isDuck && animationPlayer.currentTimeValue <= TimeSpan.FromSeconds(159.0f / 30.0f))
                {
                    this.animationPlayer.Update(gameTime.ElapsedGameTime, true, this.world);
                }
                
                // Animated to standing position, stop animating
                else if(!isDuck) this.DuckAnimate = false;
            }

            // knockback animation ~ knockback 160 to 185 = 0.5seconds
            if (this.KnockAnimate)
            {
                // reach the end of knockback animation
                if (animationPlayer.currentTimeValue >= TimeSpan.FromSeconds(183.0f / 30.0f))
                {
                    this.KnockAnimate = false;
                    this.KickAnimate = true;
                    this.StopKick = true;
                    this.timeStart = 0;
                }
                // update animation
                this.animationPlayer.Update(gameTime.ElapsedGameTime, true, this.world);
            }
        }

        public void LimitPlayerPosition()
        {
            // player reach the right road side
            if (this.pos.X > STAGE_SIDE_RIGHT)
            {
                this.pos.X = STAGE_SIDE_RIGHT - 0.1f;
            }
            // player reach the left road side
            else if (this.pos.X < STAGE_SIDE_LEFT)
            {
                this.pos.X = STAGE_SIDE_LEFT + 0.1f;
            }
        }

        public void Draw2D()
        {
            //Game1.debugText.Printf("speed : " + vel.Z, new Vector2(400, 350), new Color(1.0f, 0.0f, 0.0f));
            Life.Draw2D();

            if (isJump)
                DebugText.GetInstance().Printf("Jumping", new Vector2(400, 350));
            else
                DebugText.GetInstance().Printf("Not Jumping", new Vector2(400, 350));

            DebugText.GetInstance().Printf("Current: " + Game1.person.GetJointPos(JointType.Spine).Y.ToString(), new Vector2(400, 500));
            DebugText.GetInstance().Printf("Calibrated: " + Game1.person.GetSecondJointPos(JointType.Spine).Y.ToString(), new Vector2(400, 550));
            //DebugText.GetInstance().Printf("T-Pose Time: " + T_Pose_Time.ToString(), new Vector2(400, 650));
        }

		//------------------------------//
		//	Function Draw				//
		//	Drawing function of Player	//
		//	No argument					//
		//	No return value				//
		//------------------------------//
		public void Draw3D()
		{
			// Position and the current position of the last things being equal if
			//if (oldSkeletonPos == Game1.person.GetJointPos(JointType.Head)
			//	|| oldSkeletonPos == Vector3.Zero)
			//{
            //   if (Game1.paused)
            //   {
			//		// Draw a model of the default pose
            //        this.DrawDefaultModel();
            //			 } else {
                    // testing
            //        this.DrawSkeletonModel();
            //   }

            //    Game1.paused = true;
			//}
			// If so unlike last time (if you track)
			//else
			//{
				// Draw a skeleton model that is synchronized with the bone
            //    this.DrawSkeletonModel();

                // testing
            //    Game1.paused = false;
            //   Game1.pausetime = 0.0f;
                //Game1.pausetime = 0.0f;
			
			
            if (Game1.paused || Game1.kinect.skeleton == null) this.DrawDefaultModel();
            else this.DrawSkeletonModel();
			
			// I keep a position
			//oldSkeletonPos = Game1.person.GetSecondJointPos(JointType.Head);
            //oldSkeletonPos = pos;
		}

		//----------------------------------------------------------//
		//	Function name DrawSkeletonModel							//
		//	Drawing of the model when the function skeleton has been obtained
		//	No argument												//
		//	No return value											//
		//----------------------------------------------------------//
		private void DrawSkeletonModel()
		{
            // testing
            // original pos.Y + 1.7f
            //Vector3 upPos = new Vector3(this.pos.X,this.pos.Y + 2.5f,this.pos.Z - 5.0f);
            Vector3 upPos = new Vector3(this.pos.X, this.pos.Y - 0.2f, this.pos.Z);

			// モデルにエフェクトを設定する
			foreach (var mesh in Skeleton_Player.Meshes)
			{
				foreach (SkinnedEffect effect in mesh.Effects)
				{
					// I want to enable default lighting
					//effect.EnableDefaultLighting();

                    // testing
                    // turn on the lighting subsystem.
                    effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    effect.DirectionalLight0.Direction = new Vector3(0, -1, -1);
                    effect.DirectionalLight0.SpecularColor = new Vector3(1.0f, 0.0f, 0.0f);

					if (this.Invincible)
					{
                        effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 0.0f, 0.0f);
					}
					else
					{
                        effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
					}
					// The application of the World
					effect.World = MyMathHelper.SetWorldMatrix(upPos, this.scale, this.rotation);
					// Application of the camera
					effect.View = Game1.camera.view;
					effect.Projection = Game1.camera.projection;
				}
				// Drawing
                //mesh.Draw();
            }
            avateerHandler.Draw(Game1.kinect.skeleton, animationPlayer);
		}

		//----------------------------------------------------------//
		//	Function name DrawDefaultModel							//
		//	Drawing of the model when the function skeleton is not required
		//	No argument												//
		//	No return value											//
		//----------------------------------------------------------//
		private void DrawDefaultModel()
		{
			// I want to set the effect to model
			foreach (var mesh in Model_Player.Meshes)
			{
				foreach (SkinnedEffect effect in mesh.Effects)
				{
					// I want to enable default lighting
					effect.EnableDefaultLighting();

                    //if (Stage.damegeCount > 0)
                    //{
                    //    effect.SpecularColor = new Vector3(1, 0, 0);
                    //}
                    //else
                    //{
                    //    effect.SpecularColor = new Vector3(0,0,0);
                    //}
					// The application of the World
					effect.World = MyMathHelper.SetWorldMatrix(this.pos, this.scale, this.rotation);
					// Application of the camera
					effect.View = Game1.camera.view;
					effect.Projection = Game1.camera.projection;

				}
				// Drawing
				mesh.Draw();
			}
		}

		#endregion
	}	
}

		