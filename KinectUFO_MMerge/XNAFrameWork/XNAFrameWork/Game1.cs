//------------------------------------------//
// Game1.cs									//
//	Class from which the game scene of all	//
// 制作日:2013/09/26							//
// 制作者:Kouno Shin							//
//------------------------------------------//

// Kinect whether used
#define	KINECT
// Whether full screen
//#define	FULL
//----------------------//
//	Abbreviation of the name space
//----------------------//
#region Using Statements
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
using XNAFrameWork;
using SkinnedModel;
using Microsoft.Kinect;
#endregion

//----------------------------------//
//	Main (entry point)				//
//----------------------------------//
#region Main Method (entry point)

static class Program
{
	static void Main(string[] args)
	{
		using (Game1 game = new Game1())
		{
			game.Run();
		}
	}
}
#endregion

//------------------//
//	Game Class		//
//------------------//
#region Game1 Class
public class Game1 : Microsoft.Xna.Framework.Game
{
	//------------------//
	//	Field			//
	//------------------//

	// Whether or not to end the game(end: true, continued: false)
	static public bool gameEndFlag = false;

	// Graphics Manager
	static public GraphicsDeviceManager graphics;

	// Sprite batch
	static public SpriteBatch spriteBatch;

	// Debug text
	static public DebugText debugText = null;

	// Create a template BasicEffect
	static public BasicEffect basicEffect = null;

	// Scene Manager
	private SceneManager sceneManager;

	// Camera to be used in all scenes
	static public Camera camera;

	// FPS controller
	private FPSCounter fps = new FPSCounter();

	// Kinect object
	static public Kinect kinect;

	// Skeleton data
	static public Person person;

    // testing
    static public bool paused = false;
    static public float pausetime = 0.0f;

	/// <summary>Holds the class to move the model to fit the data skeleton of Kinect.</summary>
	//	static public AvateerHandler avateerHandler;

	//--------------------------------------------------//
	// Constructor										//
	// Do initialization here before the game starts	//
	//--------------------------------------------------//
	public Game1()
	{
		// Initialize the graphics manager
		graphics = new GraphicsDeviceManager(this);
        graphics.PreferMultiSampling = true;
        graphics.IsFullScreen = true;
        graphics.PreferredBackBufferWidth = 1600;
        graphics.PreferredBackBufferHeight = 900;

        //graphics.IsFullScreen = false;
        //graphics.PreferredBackBufferWidth = 800;
        //graphics.PreferredBackBufferHeight = 600;
       
		// Initialization of the scene manager
		sceneManager = new SceneManager(this.Content);

		// Root directory of the content
		Content.RootDirectory = "Content";

		// Initialization of the skeleton data
		person = new Person();

		// Initialization of kinect
		kinect = new Kinect();

        graphics.ApplyChanges();

#if FULL
		graphics.ToggleFullScreen();
#endif
	}

	//----------------------------------------------//
	//	Function Initialize							//
	//	Initialization of objects to use in the game function
	//	No argument									//
	//	No return value								//
	//----------------------------------------------//
	protected override void Initialize()
	{
		// Initialization of the camera to be used in all scenes
		camera = new Camera((float)GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height);

		// Load the model()
		// Do not keep up with the constructor is not called before initialize the scene
		ModelManager.GetInstance().LoadModel(this.Content);

		// Load the texture
		TextureManager.GetInstance().LoadTexture(this.Content);

		// Read the font used to debug text
		DebugText.Init(this.Content.Load<SpriteFont>(@"フォント\SpriteFont1"));

		// Create a debug text actually
		debugText = DebugText.GetInstance();

		// Display the mouse cursor on the screen
		this.IsMouseVisible = true;

		// Title of the window
		base.Window.Title = "XNA Game Studio";

		// Initialization of BasicEffect
		basicEffect = new BasicEffect(graphics.GraphicsDevice);

		// I call the initilization of the current scene from the scene manager
		sceneManager.Initialize();
		
		// I get the Kinect
		/*
		kinectSensor = KinectSensor.KinectSensors[0];
		if (kinectSensor == null || kinectSensor.Status != KinectStatus.Connected)
		{
			throw new Exception("Kinect is not connected。");
		}
		 * */

		// I want to enable the skeleton data of Kinect
		
#if KINECT
        if (kinect.GetInstance() != null)
        {
            kinect.GetInstance().SkeletonStream.Enable(new TransformSmoothParameters()
            {
                Smoothing = 0.5f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.5f,
                MaxDeviationRadius = 0.5f
            });
        }

#endif
		// Initialization of class the underlying
		base.Initialize();
		
	}

	//----------------------------------------------//
	//	Function LoadContent						//
	//	I do to read the content of all functions	//
	//	No argument									//
	//	No return value								//
	//----------------------------------------------//
	protected override void LoadContent()
	{
		// Create a sprite batch
		spriteBatch = new SpriteBatch(GraphicsDevice);

		// I call the graphic reading of the current scene from the scene manager
		sceneManager.LoadContent(this.Content);

		/*
		// I read the model
		var model = Content.Load<Model>(@"モデル\Droid");
		//								@"model
		//var model = Content.Load<Model>(@"モデル\characterRig");


		// I want to set the effect to model
		foreach (var mesh in model.Meshes)
		{
			foreach (SkinnedEffect effect in mesh.Effects)
			{
				// I want to enable default lighting
				effect.EnableDefaultLighting();

				// I set the white texture if there is not texture
				if (effect.Texture == null)
				{
					effect.Texture = new Texture2D(GraphicsDevice, 1, 1);
					effect.Texture.SetData<Color>(new[] { Color.White });
				}
			}
		}


		// I initialize the class to handle the skeleton data
		var boneIndices = new Dictionary<int, BoneType>
            {
                // Number of bones model has it is different for each model
                // I to map the number and types of bone
                { 00, BoneType.Hip },
                { 01, BoneType.Spine },
                { 02, BoneType.UpperArmRight },
                { 03, BoneType.ForeArmRight },
                { 04, BoneType.HandRight },
                { 05, BoneType.UpperArmLeft },
                { 06, BoneType.ForeArmLeft },
                { 07, BoneType.HandLeft },
                { 08, BoneType.Head },
                { 09, BoneType.ThighLeft },
                { 10, BoneType.ShinLeft },
                { 11, BoneType.FootLeft },
                { 12, BoneType.ThighRight },
                { 13, BoneType.ShinRight },
                { 14, BoneType.FootRight }
            };
		avateerHandler = new AvateerHandler(model, boneIndices);
		*/
		// I want to start the Kinect
		//kinectSensor.Start();
	}

	//--------------------------------------------------//
	//	Function name UnLoadContent						//
	//	I do destruction of imported graphics function	//
	//	No argument										//
	//	No return value									//
	//--------------------------------------------------//
	protected override void UnloadContent()
	{
		// I call the graphic destruction of the current scene from the scene manager
		sceneManager.UnLoadContent();

#if KINECT
		if (kinect.GetInstance() != null)
		{
			// I want to stop the Kinect
			kinect.GetInstance().Stop();

			// Destruction of memory kinect had possession
			kinect.GetInstance().Dispose();
		}
#endif

		
	}

	//----------------------------------//
	//	Function Update					//
	//	Function update process			//
	//	Rate of progression of argument Games
	//	No return value					//
	//----------------------------------//
	protected override void Update(GameTime gameTime)
	{
		// I call the update process from the current scene
		sceneManager.Update(gameTime);

		// Update the keyboard
		MyKeyboard.Update();

		// Kinect if successfully acquired the skeleton data
		if (kinect.skeleton != null)
		{
			// Update the skeleton data
			person.Update(kinect.skeleton);
			
			if(kinect.t_Skeleton != null) person.UpdateSecond(kinect.t_Skeleton);
		}

		// Escape key has been pressed
		if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
			|| MyKeyboard.IsPressed(Keys.Escape)
			|| gameEndFlag == true)
		{
			// I want to end the game
			this.Exit();
		}
		
		// Update processing of the class from which the original	
		base.Update(gameTime);
	}

	//--------------------------//
	//	Function Draw			//
	//	Function drawing process//
	//	No argument				//
	//	No return value			//
	//--------------------------//
	protected override void Draw(GameTime gameTime)
	{
		// I fill the screen
		//graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
        graphics.GraphicsDevice.Clear(Color.Orange);
		//graphics.GraphicsDevice.Clear(new Color(255,255,255) * 0.1f);


		// I wanto to enable the depth buffer
		var depthStencilState = new DepthStencilState();
        depthStencilState.DepthBufferEnable = true;
        graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
		graphics.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

		// I call the 3D rendering of the current scene
		sceneManager.Draw3D(gameTime);

		// I draw the model
//		avateerHandler.Draw();

		
		// Drawing preparation of sprite
		spriteBatch.Begin();

		// I call the 2D drawing of the current scene
		sceneManager.Draw2D(gameTime);

		// Draw a textrue that is generated from the origin at the upper left
		//spriteBatch.Draw(this.tex_kinectImage,Vector2.Zero,Color.White);
		//	kinect.DrawCamera();
		kinect.DrawDebug();


		// End of the sprite( collective drawing )
		spriteBatch.End();

		// Draw the FPS
		fps.Draw((float)gameTime.ElapsedGameTime.TotalSeconds);

		// Drawing of the character
		debugText.DebugString(spriteBatch);

		/*
		if (KinectSensor.KinectSensors.Count != 0)
		{
			// I want to conver to the coordinates of the camera image on the user's information
			ColorImagePoint userPosition =
				kinect.ConversionLocalKinectPosToScreenPos(kinect.skeletonPoint_HeadPosition);

			debugText.Printf("HEAD", new Vector2(userPosition.X, userPosition.Y), Color.White);
			debugText.Printf(kinect.skeletonPoint_HeadPosition.X.ToString(), new Vector2(100, 100), Color.White);
			debugText.Printf(kinect.skeletonPoint_HeadPosition.X.ToString(), new Vector2(100, 150), Color.White);

		}*/


		// Drawing process of the class from which the original
		base.Draw(gameTime);

	}
}
#endregion

