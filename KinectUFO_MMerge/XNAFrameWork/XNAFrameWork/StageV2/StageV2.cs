using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNAFrameWork.StageV2
{
    class StageV2
    {
        #region Variables

        #region Road Variables

        //How many road models are there
        const int numTypeOfRoads = 2;
        //Road Models
        Model []Road;

        //Length of Model of the Road
        const float ROAD_LENGTH = 93.8f;
        //Number of road to have on the screen
        const int NUM_OF_ROAD = 15;

        //Selector to tell which road to display
        int[] roadType;

        Vector3 Pos;
        Vector3 Scale;
        Vector3 Rotation;

        #endregion

        Model Static_Building;
        Model Falling_Building;


        const int MAX_POWERS = 1;
        public PowerUp[] Powers;

        const int MAX_OBS = 1;
        public Obstacle[] Obstacles;

        Feedback Obs_Response;

        #endregion

        public StageV2(float playerStartZ)
        {
            Road = new Model[numTypeOfRoads];
            roadType = new int[NUM_OF_ROAD];

            for (int i = 0; i < NUM_OF_ROAD; i++)
            {
                Random rand = new Random();

                roadType[i] = rand.Next(numTypeOfRoads);
            }

            Pos = new Vector3(0, 0, playerStartZ);
            Scale = Vector3.One;
            Rotation = Vector3.Zero;

            Powers = new PowerUp[MAX_POWERS];

            for (int i = 0; i < MAX_POWERS; i++)
            {
                Powers[i] = new PowerUp();
            }

            Obstacles = new Obstacle[MAX_OBS];

            for (int i = 0; i < MAX_OBS; i++)
            {
                Obstacles[i] = new Obstacle();
            }

            Obs_Response = new Feedback(EffectType.NONE);
        }

        public void Draw3D()
        {
            DrawRoad();

            if (Powers == null) return;

            //Draw power ups
            for (int i = 0; i < MAX_POWERS; i++)
            {
                Powers[i].Draw3D();
            }

            //Draw obstacles
            for (int i = 0; i < MAX_OBS; i++)
            {
                Obstacles[i].Draw3D();
            }
        }

        Vector3 playerPos;
        public void Draw2D()
        {
            //Draw feedback
            Obs_Response.Draw2D();

            //Testing
            DebugText.GetInstance().Printf("Player X:" + playerPos.X.ToString(), new Vector2(100, 100));
            DebugText.GetInstance().Printf("Player Y:" + playerPos.Y.ToString(), new Vector2(100, 150));
        }

        public void Update(GameTime gameTime, ref Player thePlayer)
        {
            for (int i = 0; i < MAX_POWERS; i++)
            {
                Powers[i].Update(gameTime, ref thePlayer);
            }

            //Check obstacles
            for (int i = 0; i < MAX_OBS; i++)
            {
                //Check if player hit the obstacles
                EffectType Check = Obstacles[i].Hit(thePlayer);

                //If player fail an obstacle
                if (Check == EffectType.BOO)
                {
                    //Knockback the player
                    thePlayer.KnockBack(gameTime);
                }

                //If player collides with obstacle
                //Create feedback with returned effect
                if(Check != EffectType.NONE) Obs_Response.Reset(Check);

            }

            //Make sure the tracking skeleton is not null, then update the player movement
            if (Game1.kinect.skeleton != null) thePlayer.MovingPlayer(gameTime);

            //Check to see if there is a need to extend the front road
            UpdateRoad(thePlayer);
            //Update the feedback animation
            Obs_Response.Update(gameTime);

            //Testing
            playerPos = thePlayer.Pos;
        }

        #region Road Functions

		#region UpdateRoad
		//	=	=	=	=	=	Brandon	=	=	=	=	=
		//	This function updates everytime the player moves
		//	from one road to the next road
		//	however in the event that the player bounces back due to collision
		//	the previous road will not be loaded as it has been reused in front
		//	=	=	=	=	=	=	=	=	=	=	=	=
		void UpdateRoad(Player thePlayer)
        {
			//	distZ stores the distance between player and the roadType[0]
            float distZ = Pos.Z - thePlayer.PosZ;

			//	as long as distZ is smaller than the indicated length of the ROAD_LENGTH
			if (distZ < ROAD_LENGTH)
			{
				//	No need to update road
				return;
			}

			//	Move all of the roadType[] data forward except for roadType[NUM_OF_ROAD-1]
			//	which is the last roadType available
            for (int i = 0; i < NUM_OF_ROAD - 1; i++)
            {
                roadType[i] = roadType[i + 1];
            }

            //Make a new random road at the last road of the array
            Random rand = new Random();

			//	Give the last roadType a new data
            roadType[NUM_OF_ROAD-1] = rand.Next(numTypeOfRoads);

            //Set the first road to current player position
            Pos.Z = thePlayer.PosZ;
        }
		#endregion

		void LoadRoad()
        {
            for (int i = 0; i < numTypeOfRoads; i++)
            {
                switch (i)
                {
                    case 0:
                        Road[0] = ModelManager.GetInstance().GetModel(ModelName.ROAD1);
                        break;

                    case 1:
                        Road[1] = ModelManager.GetInstance().GetModel(ModelName.ROAD2);
                        break;

                    default:
                        Road[i] = ModelManager.GetInstance().GetModel(ModelName.ROAD1);
                        break;
                }
            }
        }

        void DrawRoad()
        {
            //If model not loaded
            if (Road[0] == null)
            {
                //Load model
                LoadRoad();
                return;
            }

            for (int i = 0; i < NUM_OF_ROAD; i++)
            {
                //Draw road
                foreach (ModelMesh mesh in Road[roadType[i]].Meshes)
                {
                    //Update/Apply position/scale/rotation to model
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.View = Game1.camera.view;
                        effect.Projection = Game1.camera.projection;

                        effect.World = MyMathHelper.SetWorldMatrix(this.Pos - new Vector3(0, 0, ROAD_LENGTH*(i-1)), this.Scale, this.Rotation);
                    }

                    mesh.Draw();
                }
            }
        }

        #endregion
    }
}
