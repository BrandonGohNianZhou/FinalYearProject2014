//--------------------------//
// FPSCounter.cs			//
// FPSを計測するクラス		//
//	Class to measure the FPS
// 制作日:2013/10/02			//
// 制作者:Kouno Shin			//
//--------------------------//

//----------------------//
//----名前空間の省略-----//
//	Abbreviation of the name space
//----------------------//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNAFrameWork
{
	class FPSCounter
	{
		float	fps;			// FPS actual
		float	interval;		// (In most cases one second) update rate of FPS
		float	updateTimer;	// Timer to achieve it until the update
		int		frameCount;		// The current number of frames


		// Constructor
		public FPSCounter()
		{
			this.fps			= 0.0f;
			this.interval		= 1.0f;		// Update rate is 1 second
			this.updateTimer	= 0.0f;
			this.frameCount		= 0;
		}

		// Drawing functions of FPS
		public void Draw(float delta)
		{
			// Increase the number of frames
			frameCount++;

			// And adds the time that has passed since the previous frame timer
			updateTimer += delta;

			// When the timer for more than one (1) second
			if (updateTimer > interval)
			{
				// I calculate the difference here if you calculate the FPS, speed was hanging
				fps = frameCount / updateTimer;

				// I want to reset the counter and timer
				frameCount = 0;
				updateTimer -= interval;
			}

			// Drawing
			//Game1.debugText.Printf((string.Format("FPS: {0:F2}",fps)),new Vector2(0,0),Color.White);
		}
	}
}
