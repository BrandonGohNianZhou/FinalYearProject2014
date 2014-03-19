using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNAFrameWork.StageV2
{
    class Obstacle
    {
        #region ENUMS

        enum OBSTACLE_HOLE
        {
            NONE,
            LEFT,
            CENTER,
            RIGHT,
            END
        };

        enum OBSTACLE_PASS
        {
            NONE,
            JUMP,
            DUCK,
            END
        };

        #endregion

        #region Variables

        OBSTACLE_HOLE Hole;
        OBSTACLE_PASS Pass;

        Model ObstacleModel;

        private int Selector;

        public Vector3 Pos, Scale, Rotation;

        bool Checking;

        #endregion

        #region Functions

        #region Constructor

        public Obstacle()
        {
            Hole = OBSTACLE_HOLE.NONE;
            Pass = OBSTACLE_PASS.NONE;

            Scale = Vector3.One;
            Pos = Vector3.Zero;
            Rotation = Vector3.Zero;

            Checking = true;
        }

        #endregion

        #region CreateObstacle

        public void CreateObstacle(Vector3 Pos)
        {
            this.Pos.Z = Pos.Z;
            Checking = true;

            Random rand = new Random();

            Hole = (OBSTACLE_HOLE)(rand.Next((int)(OBSTACLE_HOLE.END)));
            Pass = (OBSTACLE_PASS)(rand.Next((int)(OBSTACLE_PASS.END)));

            Selector = ((int)(Hole) * 10) + ((int)(Pass));

            switch (Selector)
            {
                case 1: //NONE(0) + JUMP(1) = 1
                    ObstacleModel = ModelManager.GetInstance().GetModel(ModelName.JUMP_NONE);
                    break;

                case 2: //NONE(0) + DUCK(2) = 2
                    ObstacleModel = null;
                    break;

                //Left

                case 10: //LEFT(10) + NONE(0) = 10
                    ObstacleModel = ModelManager.GetInstance().GetModel(ModelName.NONE_LEFT);
                    break;

                case 11: //LEFT(10) + JUMP(1) = 11
                    ObstacleModel = ModelManager.GetInstance().GetModel(ModelName.JUMP_LEFT);
                    break;

                case 12: //LEFT(10) + DUCK(2) = 12
                    ObstacleModel = ModelManager.GetInstance().GetModel(ModelName.DUCK_LEFT);
                    break;

                //Center

                case 20: //CENTER(20) + NONE(0) = 20
                    ObstacleModel = ModelManager.GetInstance().GetModel(ModelName.NONE_CENTER);
                    break;

                case 21: //CENTER(20) + JUMP(1) = 21
                    ObstacleModel = ModelManager.GetInstance().GetModel(ModelName.JUMP_CENTER);
                    break;

                case 22: //CENTER(20) + DUCK(2) = 22
                    ObstacleModel = ModelManager.GetInstance().GetModel(ModelName.DUCK_CENTER);
                    break;

                //Right

                case 30: //RIGHT(30) + NONE(0) = 30
                    ObstacleModel = ModelManager.GetInstance().GetModel(ModelName.NONE_RIGHT);
                    break;

                case 31: //RIGHT(30) + JUMP(1) = 31
                    ObstacleModel = ModelManager.GetInstance().GetModel(ModelName.JUMP_RIGHT);
                    break;

                case 32: //RIGHT(30) + DUCK(2) = 32
                    ObstacleModel = ModelManager.GetInstance().GetModel(ModelName.DUCK_RIGHT);
                    break;

                default:
                    ObstacleModel = null;
                    break;
            }
        }

        #endregion

        #region Draw3D

        public void Draw3D()
        {
            if (ObstacleModel == null) return;

            foreach (ModelMesh mesh in ObstacleModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = Game1.camera.view;
                    effect.Projection = Game1.camera.projection;

                    effect.World = MyMathHelper.SetWorldMatrix(this.Pos, this.Scale, this.Rotation);
                }

                mesh.Draw();
            }
        }

        #endregion

        #region Collision

        const float CHECKING_RANGE = 3.0f;
        public EffectType Hit(Player thePlayer)
        {
            Vector3 PlayerPos = thePlayer.Pos;
            EffectType ReturnEffect = EffectType.NONE;

            float CheckDist = PlayerPos.Z - Pos.Z;

            if (!Checking || CheckDist > CHECKING_RANGE) return ReturnEffect;

            Checking = false;

            ReturnEffect = EffectType.BOO;

            switch (Pass)
            {
                case OBSTACLE_PASS.JUMP:
                    if (PlayerPos.Y >= 9.0f) ReturnEffect = EffectType.PERFECT;
                    else if (PlayerPos.Y >= 3.0f) ReturnEffect = EffectType.GOOD;
                    break;

                case OBSTACLE_PASS.DUCK:
                    if (thePlayer.isDuck) ReturnEffect = EffectType.PERFECT;
                    break;
                    
                case OBSTACLE_PASS.NONE:
                    ReturnEffect = EffectType.GOOD;
                    break;

                default:
                    ReturnEffect = EffectType.NONE;
                    break;
            }

            if (ReturnEffect == EffectType.NONE || ReturnEffect == EffectType.BOO) return ReturnEffect;

            switch (Hole)
            {

            }

            return ReturnEffect;
        }

        #endregion

        #endregion
    }
}
