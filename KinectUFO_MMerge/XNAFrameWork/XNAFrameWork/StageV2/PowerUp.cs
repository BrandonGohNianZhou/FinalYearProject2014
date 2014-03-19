using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkinnedModel;

namespace XNAFrameWork
{
    class PowerUp
    {
        #region ENUMS

        //Type of power ups available
        enum POWERUP
        {
            POINT_BOOST,
            INVUL,
            SPEED_BOOST,

            NONE
        };

        #endregion

        #region Variables

        //For animation
        AnimationPlayer animationPlayer;
        AnimationClip clip;
        Model Power;

        Matrix world;
        Matrix[] boneTransforms;

        //To identify the type of power up
        POWERUP Type;

        //Pos/Scale/Rotation of Model/Animation
        Vector3 Pos;
        Vector3 Scale;
        Vector3 Rotation;


        #endregion

        #region Functions

        #region Constructor

        public PowerUp()
        {
            Scale = Vector3.One;
            Rotation = Vector3.Zero;
            Pos = Vector3.Zero;

            Power = null;

            this.world = MyMathHelper.SetWorldMatrix(Vector3.Zero, Vector3.One, Vector3.Zero);

        }

        #endregion

        #region CreatePowerUp
        public void CreatePowerUp(Vector3 CreatePos)
        {
            Random rand = new Random();

            Type = (POWERUP)rand.Next((int)POWERUP.NONE);

            switch (Type)
            {
                case POWERUP.INVUL:
                    Power = ModelManager.GetInstance().GetModel(ModelName.INVUL);
                    break;

                case POWERUP.POINT_BOOST:
                    Power = ModelManager.GetInstance().GetModel(ModelName.POINT_BOOST);
                    break;

                case POWERUP.SPEED_BOOST:
                    Power = ModelManager.GetInstance().GetModel(ModelName.SPEED_BOOST);
                    break;

                default:
                    Power = null;
                    return;
            }

            Pos = CreatePos;

            SkinningData skinData = Power.Tag as SkinningData;
            animationPlayer = new AnimationPlayer(skinData);
            clip = skinData.AnimationClips["Take 001"];
            animationPlayer.StartClip(clip);
            animationPlayer.currentKeyframe = 0;
            this.boneTransforms = new Matrix[skinData.BindPose.Count];
        }
        #endregion

        #region Draw3D

        public void Draw3D()
        {
            if (Power == null) return;

            foreach (ModelMesh mesh in this.Power.Meshes)
            {
                //Use SkinnedEffect for skinned models
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    //Sets the translation/rotation/scale of the model
                    effect.View = Game1.camera.view;
                    effect.Projection = Game1.camera.projection;

                    effect.World = MyMathHelper.SetWorldMatrix(this.Pos, this.Scale, this.Rotation);

                    //Apply animation stuff
                    effect.SetBoneTransforms(this.boneTransforms);
                }

                //Draw
                mesh.Draw();
            }

        }
        #endregion

        #region Update

        public void Update(GameTime gameTime, ref Player thePlayer)
        {
            if (Power == null) return;

            Pos = thePlayer.Pos + new Vector3(0, 10, 0);
            Scale = new Vector3(5, 5,  5);

            this.animationPlayer.Update(gameTime.ElapsedGameTime, true, this.world);

            #region UpdateBones [REFER TO AvateerHandler.cs - Update(...) function for more info]

            Matrix[] bones = animationPlayer.GetBoneTransforms();
            SkinningData skinData = this.Power.Tag as SkinningData;

            Matrix[] worldBindPose = new Matrix[skinData.BindPose.Count];

            for (int boneIndex = 0; boneIndex < skinData.BindPose.Count; boneIndex++)
            {
                Matrix boneTransform = skinData.BindPose[boneIndex];
                int parentBoneIndex = skinData.SkeletonHierarchy[boneIndex];

                if (parentBoneIndex > -1)
                {
                    boneTransform *= worldBindPose[parentBoneIndex];
                    boneTransform = bones[boneIndex] * worldBindPose[parentBoneIndex];
                }

                else
                {
                    boneTransform = bones[boneIndex];
                }

                worldBindPose[boneIndex] = boneTransform;

                this.boneTransforms[boneIndex] = skinData.InverseBindPose[boneIndex] * worldBindPose[boneIndex];
            }

            #endregion
        }

        #endregion

        #endregion
    }
}
