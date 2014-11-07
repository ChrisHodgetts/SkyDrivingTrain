using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;


namespace SkyDrivingTrain
{
	public class AppMain
	{
		private static GraphicsContext graphics;
		private static Texture2D backgroundTexture;
		private static Sprite background;
		
		
		
		public static void Main (string[] args)
		{
			Initialize();

			while (true) 
			{
				SystemEvents.CheckEvents();
				Update ();
				Render ();
			}
		}

		public static void Initialize()
		{
			// Set up the graphics system
			graphics = new GraphicsContext();
			
			//NOTE: Screen size for PSVITA = 960x544
			backgroundTexture = new Texture2D("/Application/assets/background.png", false);
			background = new Sprite(graphics, backgroundTexture);
			background.Position.X = 0;
			background.Position.Y = 0;

		}

		public static void Update()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);
		}

		public static void Render()
		{
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear();
			
			background.Render();

			// Present the screen
			graphics.SwapBuffers();
		}
	}
}
