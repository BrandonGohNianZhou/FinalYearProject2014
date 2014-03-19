//------------------------------------------//
// Animation.cs								//
// アニメーションをするための便利クラス		//
//	Convenience class to the animation
// 制作日:2013/10/04							//
// 制作者:Kouno Shin							//
//------------------------------------------//

#region How to use this class
/*
 *	使いたいクラスのフィールドにAnimation 型でインスタンスを生成
 *	Generate instance in Animation field type of class you want to use
 *	Animation sampleModel;
 *	
 *	コンテンツのロードができる場所でロードを行う
 *	I do load in a place that can load content
 *	sampleModell = new Animation(Content.Load<Model>(@"モデルファイルがある場所"));
 *	
 *	使いたいアニメーションクリップを作成
 *	Create an animation clip you want to use
 *	sampleModel.CreateAnimationClip("アニメーション名");
 *	
 *	更新処理を書く(引数はゲームタイム)
 *	Write the update process (argument game time)
 *	sampleModel.Update(gameTime);
 *	
 *	描画処理を書く
 *	I write a drawing process
 *	sampleModel.Draw();
 * 
*/
#endregion


//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XNAFrameWork;
using SkinnedModel;

namespace XNAFrameWork
{
	class Animation
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
			get { return MyMathHelper.Vector3ToDegree( this.rotation); }
			set { this.rotation = MyMathHelper.Vector3ToRadian(value); }
		}

		// set or get the X-axis rotation
		public float rotationX
		{
			get { return MathHelper.ToDegrees(this.rotation.X);}
			set { this.rotation.X = MathHelper.ToRadians(value); }
		}
		// set or get the Y-axis rotation
		public float rotationY
		{
			get { return MathHelper.ToDegrees(this.rotation.Y);}
			set { this.rotation.Y = MathHelper.ToRadians(value); }
		}
		// set or get the Z-axis rotation
		public float rotationZ
		{
			get { return MathHelper.ToDegrees(this.rotation.Z);}
			set { this.rotation.Z = MathHelper.ToRadians(value); }
		}
		#endregion

		// Stores for model animation
		private Model model;

		// Skinning data storage
		private SkinningData skinningData { get; set; }

		// Animation player
		public AnimationPlayer animationPlayer;

		#endregion

		#region Constructor

		public Animation(Model model)
		{
			// Initialize each value
			this.pos = new Vector3(0.0f, 0.0f, 0.0f);
			this.scale = new Vector3(1.0f, 1.0f, 1.0f);
			this.rotation = MyMathHelper.Vector3ToRadian(new Vector3(0.0f, 0.0f, 0.0f));

			// Stores model that we have got
			this.model = model;

			// (The cast of the tag model) to examine the skinning data
			this.skinningData = model.Tag as SkinningData;

			// If skinning data if empty
			if (skinningData == null)
			{
				// I throw an exception
				throw new InvalidOperationException
				("This model does not contain a SkinningData tag.");
			}

			// Creating animation player
			this.animationPlayer = new AnimationPlayer(skinningData);
		}

		#endregion

		#region Function
		//------------------------------------------//
		// 関数名	CreateAnimationClip				//
		//	Function name CreateAnimationClip
		// 機能		アニメーションクリップの作成		//
		//	Create a feature animation clips
		// 引数		アニメーションの名前				//
		//	Name of the argument animation
		// 戻り値	なし								//
		//	No return value
		//------------------------------------------//
		public void CreateAnimationClip(string animationName)
		{
			// Create a clip of the name of the animation you've got
			AnimationClip clip = skinningData.AnimationClips[animationName];
			// I let the preparation of clip playback in animation player
			this.animationPlayer.StartClip(null);
		}

		//------------------------------------------//
		// 関数名	GetTransform					//
		//	Function name GetTransform
		// 機能		各値が格納された行列の取得		//
		//	Get the matrix function values are stored
		// 引数		なし								//
		//	No argument
		// 戻り値	なし								//
		//	No return value
		//------------------------------------------//
		private Matrix GetTransform()
		{
			// Create a storage matrix, initialization
			Matrix world = Matrix.Identity;

			// Stores scale
			world *= Matrix.CreateScale(this.scale);
			// Stores rotation
			world *= Matrix.CreateRotationX(this.rotationX);
			world *= Matrix.CreateRotationY(this.rotationY);
			world *= Matrix.CreateRotationZ(this.rotationZ);
			// Stores position
			world *= Matrix.CreateTranslation(this.pos);

			// Returns a matrix that is finished store
			return world;

		}

		//------------------------------//
		// 関数名	Update				//
		//	Function Update
		// 機能		更新処理				//
		//	Function update process
		// 引数		ゲームタイム			//
		//	Argument Games Time
		// 戻り値	なし					//
		//	No return value
		//------------------------------//
		public void Update(GameTime gameTime)
		{
			// I call the update function for the animation player
			this.animationPlayer.Update(
				gameTime.ElapsedGameTime,
				true,
				Matrix.Identity);
		}

		//------------------------------//
		// 関数名	Draw				//
		//	Function Draw
		// 機能		描画処理				//
		//	Function drawing process
		// 引数		なし					//
		//	No argument
		// 戻り値	なし					//
		//	No return value
		//------------------------------//
		public void Draw()
		{
			// Initialize the matrix of bone
			Matrix[] bones = animationPlayer.GetSkinTransforms();

			// Draw the skin mesh actually
			foreach (ModelMesh mesh in this.model.Meshes)
			{
				// Effect
				foreach (SkinnedEffect effect in mesh.Effects)
				{
					// Set the light of default
					effect.EnableDefaultLighting();
					// Get the matrix in which each value is stored
					effect.World = this.GetTransform();

					// Set of bone
					effect.SetBoneTransforms(bones);
					// Set of views
					effect.View = Game1.camera.view;
					// Set of projection
					effect.Projection = Game1.camera.projection;
				}

				// Drawing of mesh
				mesh.Draw();
			}
		}

		#endregion
	}
}
