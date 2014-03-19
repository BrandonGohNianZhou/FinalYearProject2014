using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace XNAFrameWork
{
    public class Shadows //: Microsoft.Xna.Framework.Game
    {
        public Shadows()
        {
            //Game1.graphics.PreferredDepthStencilFormat = SelectStencilMode();
        }

        //private static DepthFormat SelectStencilMode()
        //{
        //    // Check stencil formats
        //    GraphicsAdapter adapter = GraphicsAdapter.DefaultAdapter;
        //    SurfaceFormat format = adapter.CurrentDisplayMode.Format;

        //    if (adapter.CheckDepthStencilMatch(DeviceType.Hardware, format, 
        //        format, DepthFormat.Depth24Stencil8))
        //        return DepthFormat.Depth24Stencil8;

        //    else if (adapter.CheckDepthStencilMatch(DeviceType.Hardware, format,
        //        format, DepthFormat.Depth24Stencil8Single))
        //        return DepthFormat.Depth24Stencil8Single;

        //    else if (adapter.CheckDepthStencilMatch(DeviceType.Hardware, format,
        //        format, DepthFormat.Depth24Stencil4))
        //        return DepthFormat.Depth24Stencil4;

        //    else if (adapter.CheckDepthStencilMatch(DeviceType.Hardware, format,
        //        format, DepthFormat.Depth15Stencil1))
        //        return DepthFormat.Depth15Stencil1;

        //    else
        //        throw new InvalidOperationException(
        //            "Could Not Find Stencil Buffer for Default Adapter");
        //}


        Matrix View;
        Matrix Projection;
        Quad wall;
        Plane wallPlane;
        public void Initialize()
        {
            // Create the View and Projection matrices
            View = Matrix.CreateLookAt(new Vector3(1, -2, 10), 
                Vector3.Zero, Vector3.Up);

            Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4, 4.0f / 3.0f, 1, 500);

            // Create a new Textured Quad to represent a wall
            wall = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, 7, 7);

            // Create a Plane using three points on the Quad
            wallPlane = new Plane(wall.UpperLeft, wall.UpperRight, wall.LowerLeft);

            LoadContent();
        }

        // LoadContent will be called once per game and is the place to load all of your content.
        //Model model;
        //Matrix modelWorld;
        Texture2D texture;

        VertexDeclaration wallVertexDecl;
        BasicEffect quadEffect;
        Vector3 shadowLightDir;
        Matrix shadow;

        public void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            quadEffect = new BasicEffect(Game1.graphics.GraphicsDevice);
            quadEffect.EnableDefaultLighting();
            quadEffect.View = View;
            quadEffect.Projection = Projection;
            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture;
            shadowLightDir = quadEffect.DirectionalLight0.Direction;

            // Use the wall plane to create a shadow matrix, 
            // and make the shadow slightly higher than the wall.  
            // The shadow is based on the strongest light
            shadow = Matrix.CreateShadow(shadowLightDir, wallPlane) *
                Matrix.CreateTranslation(wall.Normal / 100);
            wallVertexDecl = new VertexDeclaration(VertexPositionNormalTexture.VertexDeclaration.GetVertexElements());
        }
            
        // update
        public void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
        }

        // draw
        public void Draw()
        {
            // Draw floor, and draw model
            DrawQuad();

            //foreach (ModelMesh mesh in model.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();

            //        effect.View = View;
            //        effect.Projection = Projection;
            //        effect.World = modelWorld;
            //    }
            //    mesh.Draw();
            //}

            // Draw shadow, using the stencil buffer to 
            // prevent drawing overlapping polygons

            // Clear stencil buffer to zero.
            //Game1.graphics.GraphicsDevice.RenderState.StencilEnable = true;
            
            // Draw on screen if 0 is the stencil buffer value           
            //Game1.graphics.GraphicsDevice.RenderState.ReferenceStencil = 0;
            //Game1.graphics.GraphicsDevice.RenderState.StencilFunction = CompareFunction.Equal;
            
            // Increment the stencil buffer if we draw
            //Game1.graphics.GraphicsDevice.RenderState.StencilPass = StencilOperation.Increment;
            
            // Setup alpha blending to make the shadow semi-transparent
            //Game1.graphics.GraphicsDevice.RenderState.AlphaBlendEnable = true;
            //Game1.graphics.GraphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            //Game1.graphics.GraphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;
            
            // Draw the shadow without lighting
            //foreach (ModelMesh mesh in model.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.AmbientLightColor = Vector3.Zero;
            //        effect.Alpha = 0.5f;
            //        effect.DirectionalLight0.Enabled = false;
            //        effect.DirectionalLight1.Enabled = false;
            //        effect.DirectionalLight2.Enabled = false;
            //        effect.View = View;
            //        effect.Projection = Projection;
            //        effect.World = modelWorld * shadow;
            //    }
            //    mesh.Draw();
            //}
            // Return render states to normal            

            // turn stencilling off
            //Game1.graphics.GraphicsDevice.RenderState.StencilEnable = false;

            // turn alpha blending off
            //Game1.graphics.GraphicsDevice.RenderState.AlphaBlendEnable = false;
        }

        // draw
        public void DrawShadow(Model model, Matrix modelWorld)
        {
            // Draw floor, and draw model
            //DrawQuad();

            //foreach (ModelMesh mesh in model.Meshes)
            //{
            //    foreach (BasicEffect effect in mesh.Effects)
            //    {
            //        effect.EnableDefaultLighting();

            //        effect.View = View;
            //        effect.Projection = Projection;
            //        effect.World = modelWorld;
            //    }
            //    mesh.Draw();
            //}

            // Draw shadow, using the stencil buffer to 
            // prevent drawing overlapping polygons

            // Clear stencil buffer to zero
            DepthStencilState newdepth = new DepthStencilState();
            newdepth.StencilEnable = true;
            newdepth.ReferenceStencil = 0;
            newdepth.StencilFunction = CompareFunction.Equal;
            newdepth.StencilPass = StencilOperation.Increment;
            Game1.graphics.GraphicsDevice.DepthStencilState = newdepth;

            //Game1.graphics.GraphicsDevice.Clear(ClearOptions.Stencil, Color.Black, 0, 0);
            //Game1.graphics.GraphicsDevice.DepthStencilState.StencilEnable = true;

            //// Draw on screen if 0 is the stencil buffer value
            //Game1.graphics.GraphicsDevice.DepthStencilState.ReferenceStencil = 0;
            //Game1.graphics.GraphicsDevice.DepthStencilState.StencilFunction = CompareFunction.Equal;

            //// Increment the stencil buffer if we draw
            //Game1.graphics.GraphicsDevice.DepthStencilState.StencilPass = StencilOperation.Increment;

            // Setup alpha blending to make the shadow semi-transparent
            BlendState newblend = new BlendState();
            newblend = BlendState.AlphaBlend;
            //newblend.AlphaSourceBlend = Blend.SourceAlpha;
            //newblend.AlphaDestinationBlend = Blend.InverseSourceAlpha;
            Game1.graphics.GraphicsDevice.BlendState = newblend;

            //Game1.graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            //Game1.graphics.GraphicsDevice.BlendState.AlphaSourceBlend = Blend.SourceAlpha;
            //Game1.graphics.GraphicsDevice.BlendState.AlphaDestinationBlend = Blend.InverseSourceAlpha;

            // Draw the shadow without lighting
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.AmbientLightColor = Vector3.Zero;
                    effect.Alpha = 0.5f;
                    effect.DirectionalLight0.Enabled = false;
                    effect.DirectionalLight1.Enabled = false;
                    effect.DirectionalLight2.Enabled = false;
                    effect.View = View;
                    effect.Projection = Projection;
                    effect.World = modelWorld * shadow;
                }
                mesh.Draw();
            }
            // Return render states to normal            

            // turn stencilling off
            DepthStencilState newdepth2 = new DepthStencilState();
            newdepth2.StencilEnable = false;
            Game1.graphics.GraphicsDevice.DepthStencilState = newdepth2;
            //Game1.graphics.GraphicsDevice.DepthStencilState.StencilEnable = false;

            // turn alpha blending off
            BlendState newblend2 = new BlendState();
            newblend2 = BlendState.Opaque;
            Game1.graphics.GraphicsDevice.BlendState = newblend2;
            //Game1.graphics.GraphicsDevice.BlendState = BlendState.Opaque;
        }

        private void DrawQuad()
        {
            //Game1.graphics.GraphicsDevice.VertexDeclaration = wallVertexDecl;

            //quadEffect.Begin();
            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                //pass.Begin();
                pass.Apply();

                Game1.graphics.GraphicsDevice.DrawUserIndexedPrimitives
                    <VertexPositionNormalTexture>(
                    PrimitiveType.TriangleList, wall.Vertices, 0, 4, 
                    wall.Indexes, 0, 2);

                //pass.End();
            }
            //quadEffect.End();
        }
    }
}
