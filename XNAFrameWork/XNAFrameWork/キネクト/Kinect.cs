//------------------------------//
// Kinect.cs					//
// kinectを管理するクラス			//	Class that manages the kinect
// 制作日:2013/10/09				//
// 制作者:Kouno Shin				//
//------------------------------//

//----------------------//
//----名前空間の省略-----//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Kinect;
using XNAFrameWork;

namespace XNAFrameWork
{
	public class Kinect
	{
		#region Field

		// variable to store the access to the kinect
		private KinectSensor kinect;

		// Skeletal point coordinates indicating the location of the user
		public SkeletonPoint skeletonPoint_UserPosition{get;set;}

		// Skeleton information about the user
		public Skeleton skeleton { get;set;}

		// 2D texture to reflect the image of the Kinect
		Texture2D	tex_kinectCamera;


		#endregion

		#region Constructor

		public Kinect()
		{
			if (KinectSensor.KinectSensors.Count != 0)
			{
				// I will ensure access to kinect that is connected to the 0-th
				this.kinect = KinectSensor.KinectSensors[0];
				// Set for obtaining a camera image
				this.kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

				// I want to register (methods) event handler to be executed when the camera image is sent from the kinect
				this.kinect.ColorFrameReady +=
						new EventHandler<ColorImageFrameReadyEventArgs>(UpdateKinectCamera);
				// Settings for acquiring skeleton information
				//this.kinect.SkeletonStream.Enable();
				this.kinect.SkeletonFrameReady +=
						new EventHandler<SkeletonFrameReadyEventArgs>(UpdateSkeletonFrame);

				// I will start the operation of the kinect
				this.kinect.Start();
			}
		}

		#endregion

		#region Function

		//----------------------------------//
		// 関数名	GetInstance				//
		//	Function GetInstance
		// 機能		kinectの取得				//
		//	Get the function kinect
		// 引数		なし						//
		//	No argument
		// 戻り値	kinectオブジェクト		//
		//	Returns kinect object
		//----------------------------------//
		public KinectSensor GetInstance()
		{
			return this.kinect;
		}

		//------------------------------------------------------------------//
		// 関数名	ConversionLocalKinectPosToScreenPos						//
		//	Function ConversionLocalKinectPosToScreenPos
		// 機能		kinectが取得したローカル座標をカメラ上の点へ変換する		//
		//	I want to convert to a point on the camera a local coordinate function kinect has acquired
		// 引数		変換したい点												//
		//	That you want to convert argument
		// 戻り値	変換し終わった点											//
		//	That you have finished return value conversion
		//------------------------------------------------------------------//
		public ColorImagePoint ConversionLocalKinectPosToScreenPos(SkeletonPoint point)
		{
			// Returns the points finished converted
			return this.kinect.CoordinateMapper.MapSkeletonPointToColorPoint(
																point,
																this.kinect.ColorStream.Format);
		}

		//----------------------------------------------//
		// 関数名	UpdateKinectCamera					//
		//	Function name UpdateKinectCamera
		// 機能		RGBカメラのフレーム更新イベント		//
		//	Frame update event of RGB camera function
		// 引数		イベントで自動的に渡される引数			//
		//	Arguments passed automatically argument Events
		// 戻り値	なし									//
		//	No return value
		//----------------------------------------------//
		private void UpdateKinectCamera(object sender, ColorImageFrameReadyEventArgs e)
		{
			// I try to challenge the data acquisition of the RGB camera
			try
			{
				// I get the frame data of RGB camera
				using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
				{
					// Color frame is not empty
					if (colorFrame != null)
					{
						// I want to get the pixel data of the RGB camera
						byte[] colorPixel = new byte[colorFrame.PixelDataLength];
						colorFrame.CopyPixelDataTo(colorPixel);
					}
				}
			}
			catch (Exception ex)
			{
				// View the contents of the exception
				Game1.debugText.Printf(ex.Message, new Vector2(10, 10), Color.White);
			}

			// I get the ColorImageFrame
			ColorImageFrame colorImageFrame = e.OpenColorImageFrame();

			// If you can not get the camera image
			if (colorImageFrame == null)
			{
				// Interruption
				return;
			}

			// Providing a variable to store the byte array in the camera image
			byte[] kinectColorArray = new byte[colorImageFrame.PixelDataLength];

			// I save all the data byte of the camera image
			colorImageFrame.CopyPixelDataTo(kinectColorArray);

			// For display on XNA on, providing a byte sequence of the same length another
			byte[] xnaColorArray = new byte[colorImageFrame.PixelDataLength];

			// I change the order of RGBA
			for (int i = 0; i + 3 < kinectColorArray.Length; i += 4)
			{
				xnaColorArray[i] = kinectColorArray[i + 2];			// R
				xnaColorArray[i + 1] = kinectColorArray[i + 1];		// G
				xnaColorArray[i + 2] = kinectColorArray[i];			// B
				xnaColorArray[i + 3] = 255;							// A
			}

			// I initialize the texture
			tex_kinectCamera = new Texture2D(
									Game1.graphics.GraphicsDevice,			// Graphics device
									colorImageFrame.Width,					// Width of the texture to be displayed
									colorImageFrame.Height);				// The height of the texture to be displayed

			// I want to set the image data to the texture
			tex_kinectCamera.SetData<byte>(xnaColorArray);

			// data received from the kinect to discard it when it is no longer used
			colorImageFrame.Dispose();
		}


		//----------------------------------------------//
		// 関数名	UpdateSkeletonFrame					//
		//	Function name UpdateSkeletonFrame
		// 機能		骨格情報の更新イベント				//
		//	Update event function of skeletal information
		// 引数		イベントで自動的に渡される引数			//
		//	Arguments passed automatically arguments Events
		// 戻り値	なし									//
		//	No return value
		//----------------------------------------------//
		public void UpdateSkeletonFrame(object sender, SkeletonFrameReadyEventArgs e)
		{

			// I get the SkeletonFrame
			SkeletonFrame skeletonFrame = e.OpenSkeletonFrame();

			// Processing is interrupted when the skeleton information is not successfully acquired
			if (skeletonFrame == null)
			{
				return;
			}

			// Providing a variable to store an array of skeleton information
			Skeleton[] skeletonArray = new Skeleton[skeletonFrame.SkeletonArrayLength];

			// I save all the data of the skeleton information
			skeletonFrame.CopySkeletonDataTo(skeletonArray);

			// Do iterate through the skeleton information stored 
			// The assumption that only one person has shifted here
			foreach (Skeleton skeleton in skeletonArray)
			{
				// Once a user has been able to track skeleton (joint)
				if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
				{
					// I keep the coordinates of the user
					this.skeletonPoint_UserPosition = skeleton.Position;

					// Save the skeleton information about the user
					this.skeleton = skeleton;
				}
			}

			// and discards the data received from the kinect
			skeletonFrame.Dispose();
		}

		
		//----------------------------------------------//
		// 関数名	Dispose								//
		//	Function Dispose
		// 機能		Kinectが保持していたメモリの解放		//
		//	Release the memory function Kinect might hold
		// 引数		なし									//
		//	No argument
		// 戻り値	なし									//
		//	No return value
		//----------------------------------------------//
		public void Dispose()
		{
			// Connection to the kinect if it has been established
			if (this.kinect != null)
			{
				// discarded to stop the kinect
				kinect.Dispose();
			}
		}


		//--------------------------------------//
		// 関数名	DrawCamera					//
		//	Function name DrawCamera
		// 機能		kinectのカメラ映像の描画		//
		//	Draw the camera image of function kinect
		// 引数		なし							//
		//	No argument
		// 戻り値	なし							//
		//	No return value
		//--------------------------------------//
		public void DrawCamera()
		{
			Game1.spriteBatch.Draw(this.tex_kinectCamera, Vector2.Zero, Color.White);
		}

		#endregion

	}
}
