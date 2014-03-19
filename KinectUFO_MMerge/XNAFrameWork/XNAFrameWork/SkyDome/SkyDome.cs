using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNAFrameWork;

// testing
namespace XNAFrameWork
{
	#region SkyDome

    class SkyDome : IObject
	{
		#region Model

		Model Model_SkyDome;

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
			set { this.scale = value; }
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
			get { return this.scale.Z; }
			set { this.scale.Z = value; }
		}
		#endregion

		#region Rotating

        private Vector3 rotation;

        // Acquisition and rotation settings
		public Vector3 Rotation
		{
			get { return  MyMathHelper.Vector3ToDegree(this.rotation); }
			set { this.rotation = MyMathHelper.Vector3ToRadian(value); }
		}

        // set or get the x-axis rotation
		public float rotationX
		{
			get { return MathHelper.ToDegrees(this.rotation.X);}
			set { this.rotation.X = MathHelper.ToRadians(value); }
		}
        // set or get the y-axis rotation
		public float rotationY
		{
			get { return MathHelper.ToDegrees(this.rotation.Y);}
			set { this.rotation.Y = MathHelper.ToRadians(value); }
		}
        // set or get the z-axis rotation
		public float rotationZ
		{
			get { return MathHelper.ToDegrees(this.rotation.Z);}
			set { this.rotation.Z = MathHelper.ToRadians(value); }
		}
		#endregion


		#region Rotation speed

		public float rotateSpeed { get;set;}

		#endregion
		#endregion

		#region Constructor

        public SkyDome()
		{
            // Reading model
            this.Model_SkyDome = ModelManager.GetInstance().GetModel(ModelName.SKYDOME);

            // Initialization of each value
            this.pos = new Vector3(0.0f, -100.0f, 0.0f);
            //this.scale = new Vector3(1.0f, 1.0f, 1.0f);
			this.scale = new Vector3(100.0f,100.0f,100.0f);
			this.rotation = new Vector3(0.0f,0.0f,0.0f);
			this.rotateSpeed = 0.05f;
		}
		
		#endregion
		#region Function

		//--------------------------//
        // Function Draw            // 
        // Function drawing process // 
        // No argument              // 
        // No return value          //
		//--------------------------//
		public void Draw()
		{
            // testing
            //RasterizerState rs = new RasterizerState();
            //rs.CullMode = CullMode.CullClockwiseFace;
            //Game1.graphics.GraphicsDevice.RasterizerState = rs;


			// Drawing
            foreach (ModelMesh mesh in this.Model_SkyDome.Meshes)
			{
				// Specifies the coordinate transformation
				foreach (BasicEffect effect in mesh.Effects)
				{
					// Use the light of default
                    //effect.EnableDefaultLighting();

                    // testing
                    // turn on the lighting subsystem.
                    effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                    effect.DirectionalLight0.Enabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    effect.DirectionalLight0.Direction = new Vector3(0, -1, -1);
                    effect.DirectionalLight0.SpecularColor = new Vector3(0.0f, 0.0f, 0.0f);

					// Set the camera
					effect.View = Game1.camera.view;
					effect.Projection = Game1.camera.projection;

					// Set the world matrix
					effect.World = MyMathHelper.SetWorldMatrix(this.pos, this.scale, this.rotation);
				}
				mesh.Draw();
			}


            // testing
            //RasterizerState rs1 = new RasterizerState();
            //rs1.CullMode = CullMode.CullCounterClockwiseFace;
            //Game1.graphics.GraphicsDevice.RasterizerState = rs1;
		}

		#endregion
	}
	#endregion
}
