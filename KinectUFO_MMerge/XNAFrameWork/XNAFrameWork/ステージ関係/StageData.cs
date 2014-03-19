//--------------------------------------//
// StageData.cs							//
// ステージデータの管理をするクラス		//
//	Class that the management of stage data
// 制作日:2013/10/22						//
// 制作者:Kouno Shin						//
//--------------------------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;
using XNAFrameWork;
using Microsoft.Xna.Framework.Input;

namespace XNAFrameWork
{
	class StageData
	{
		#region Field

		#region Position

		private Vector3 pos;

		/// Get and set position
		public Vector3 Pos
		{
			get { return this.pos; }
			set { this.pos = value; }
		}

		/// Get and set the X position
		public float PosX
		{
			get { return this.pos.X; }
			set { this.pos.X = value; }
		}
		/// Get and set the Y position
		public float PosY
		{
			get { return this.pos.Y; }
			set { this.pos.Y = value; }
		}
		/// Get and set the Z position
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
			set { this.scale = value; }
		}

		// Get and set the scale X
		public float scaleX
		{
			get { return this.scale.X; }
			set { this.scale.X = value; }
		}
		// Get and set the scale Y
		public float scaleY
		{
			get { return this.scale.Y; }
			set { this.scale.Y = value; }
		}
		// Get and set the scale Z
		public float scaleZ
		{
			get { return this.scale.Z; }
			set { this.scale.Z = value; }
		}
		#endregion

		#region Rotation
		private Vector3 rotation;

		// Acquisition and rotation settings
		public Vector3 Rotation
		{
			get { return MyMathHelper.Vector3ToDegree(this.rotation); }
			set { this.rotation = MyMathHelper.Vector3ToRadian(value); }
		}

		// set or get the X-axis rotation
		public float rotationX
		{
			get { return MathHelper.ToDegrees(this.rotation.X); }
			set { this.rotation.X = MathHelper.ToRadians(value); }
		}
		// set or get the Y-axis rotation
		public float rotationY
		{
			get { return MathHelper.ToDegrees(this.rotation.Y); }
			set { this.rotation.Y = MathHelper.ToRadians(value); }
		}
		// set or get the Z-axis rotation
		public float rotationZ
		{
			get { return MathHelper.ToDegrees(this.rotation.Z); }
			set { this.rotation.Z = MathHelper.ToRadians(value); }
		}
		#endregion

        // active 
        public bool active = false;

		// Model data
		Model Model_Stage;

		// Hit determination data
		IStageCollision collision;

		// World matrix
		private Matrix world = Matrix.Identity;

		// Animation player
		public AnimationPlayer animationPlayer;

		// Whether to animation
		public bool isPlayAnimation { get;set;}

		#endregion

		#region コンストラクタ

		public StageData(int stageNumber)
		{
			// Reading of the data model
			this.LoadStage(stageNumber);
            
            this.active = false;

			// Initialization of each value
			this.pos = Vector3.Zero;
			this.scale = Vector3.One;
			this.rotation = Vector3.Zero;

			// Is not the first animation
			this.isPlayAnimation = false;

			// Get skinning data model
			SkinningData skinningData = Model_Stage.Tag as SkinningData;
			// Creating animation player
            animationPlayer = new AnimationPlayer(skinningData);
			// Set of animation
            AnimationClip clip = skinningData.AnimationClips["Take 001"];
            animationPlayer.StartClip(clip);

            shadow = new Shadows();
            shadow.Initialize();
		}

		#endregion

        // testing
        Shadows shadow;

		#region Function

		//--------------------------------------------------//
		// 関数名	LoadStage								//
		//	Function name LoadStage
		// 機能		渡されたナンバーのステージの読み込み		//
		//	Read the number of the stage passed function
		// 引数		なし										//
		//	No argument
		// 戻り値	なし										//
		//	No return value
		//--------------------------------------------------//
		private void LoadStage(int stageNumber)
		{
			// The branch by number passed
			switch (stageNumber)
			{
			case 0:
				this.Model_Stage = ModelManager.GetInstance().GetModel(ModelName.STAGE_0);
				this.collision = new StageCollisionPart0();
				break;

			case 1:
				this.Model_Stage = ModelManager.GetInstance().GetModel(ModelName.STAGE_1);
				this.collision = new StageCollisionPart1();
				break;

			case 2:
				this.Model_Stage = ModelManager.GetInstance().GetModel(ModelName.STAGE_2);
				this.collision = new StageCollisionPart2();
				break;

			case 3:
				this.Model_Stage = ModelManager.GetInstance().GetModel(ModelName.STAGE_3);
				this.collision = new StageCollisionPart3();
				break;

			case 4:
				this.Model_Stage = ModelManager.GetInstance().GetModel(ModelName.STAGE_4);
				this.collision = new StageCollisionPart4();
				break;

			case 5:
				this.Model_Stage = ModelManager.GetInstance().GetModel(ModelName.STAGE_5);
				this.collision = new StageCollisionPart5();
				break;

			case 6:
				this.Model_Stage = ModelManager.GetInstance().GetModel(ModelName.STAGE_6);
				this.collision = new StageCollisionPart6();
				break;

			case 7:
				this.Model_Stage = ModelManager.GetInstance().GetModel(ModelName.STAGE_START);
				this.collision = new StageCollisionNone();
				break;

			case 8:
				this.Model_Stage = ModelManager.GetInstance().GetModel(ModelName.STAGE_END);
				this.collision = new StageCollisionNone();
				break;

			}
		}

		//--------------------------//
		// 関数名	Update			//
		//	Function Update
		// 機能		更新処理			//
		//	Function update process
		// 引数		ゲームタイム		//
		//	Argument Games Time
		// 戻り値	なし				//
		//	No return value
		//--------------------------//
		public void Update(GameTime gameTime)
		{
			// Update world matrix
			this.world = MyMathHelper.SetWorldMatrix(pos,scale,rotation);

			// Update animation player
            //if (animationPlayer != null)
			    this.animationPlayer.Update(gameTime.ElapsedGameTime, true, this.world);

		}

		//----------------------------------------------//
		// 関数名	Hit									//
		//	Function name Hit
		// 機能		壁に当たっているかどうかを調べる		//
		//	To check to see if they hit the wall function
		// 引数		プレイヤーのポジション				//
		//	Position of argument Player
		// 戻り値	当たっている	: true					//
		//	Is hitting return value: true
		//			当たっていない	: false				//
		//	You do not hit: false
		//----------------------------------------------//
        public bool Hit(Vector3 playerPos, bool playerDuck)
		{
			// Update of collision data
			this.collision.Update(this.pos);
			// I return the result of the judgment per
            return this.collision.Hit(playerPos, playerDuck);
		}

		//----------------------------------//
		// 関数名	OnlyOneFrameUpdate		//
		//	Function name OnlyOneFrameUpdate
		// 機能		1フレーム目だけ更新		//
		//	Update only the first frame function
		// 引数		なし						//
		//	No argument
		// 戻り値	なし						//
		//	No return value
		//----------------------------------//
		public void OnlyOneFrameUpdate()
		{
			// Update world matrix
			this.world = MyMathHelper.SetWorldMatrix(pos, scale, rotation);
			// Display only one frame
           // if (animationPlayer != null)
			    this.animationPlayer.Update(new TimeSpan(0),true,this.world);
		}

		//--------------------------//
		// 関数名	Draw			//
		//	Function Draw
		// 機能		描画処理			//
		//	Function drawing process
		// 引数		なし				//
		//	No argument
		// 戻り値	なし				//
		//	No return value
		//--------------------------//
		public void Draw()
		{
			//collision.Draw();

            //Matrix[] bones;
			//// Acquisition of bone information
            //if (animationPlayer != null)
            Matrix[] bones = animationPlayer.GetSkinTransforms();
            //else
            //    bones = new Matrix[15];

//            Game1.graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
//            Game1.graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

			// Drawing
			foreach (ModelMesh mesh in this.Model_Stage.Meshes)
			{
				// Specifies the coordinate transformation
				    foreach (SkinnedEffect effect in mesh.Effects)
				    {
						// The ON the valid flag of fog
                        effect.FogEnabled = false;
						// Set the color of the fog
                        effect.FogColor = Vector3.One * 0.8f;
						// Set the start distance of fog
                        effect.FogStart = 500.0f;
						// Set the end distance of fog
                        effect.FogEnd = 5000.0f;

						//// Use the light of default
                        ////effect.EnableDefaultLighting();

                        // testing
                        // turn on the lighting subsystem.
                        effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                        //effect.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f);
                        effect.DirectionalLight0.Enabled = true;
                        effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, -1);
                        effect.DirectionalLight0.SpecularColor = new Vector3(0.0f, 0.0f, 0.0f);

						// Set bones information
                        //if (animationPlayer != null)
		    			    effect.SetBoneTransforms(bones);

							/*	Note!!
							 *  No need to do here to you, since you have a world transformation already in the Update
							*/

							// Camera settings can be changed
					    effect.View = Game1.camera.view;
					    effect.Projection = Game1.camera.projection;

					
				    }
					// Draw model
				mesh.Draw();
			}

            //shadow.Draw();
            //shadow.DrawShadow(this.Model_Stage, world);
		}

        public bool CheckSafePass()
        {
            return this.collision.CheckSafePass();
        }
		#endregion
	}
}
