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
		private static Texture2D playerCharTexture;
		private static Sprite background;
		private static Sprite playerChar;
		
		
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
			
			//Load in player character
			playerCharTexture = new Texture2D("Application/assets/spikedShip.png", false);
			playerChar = new Sprite(graphics, playerCharTexture);
			playerChar.Position.X = 480;
			playerChar.Position.Y = 277;
			
			
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
			
			
			//Controls
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
				playerChar.Position.X = playerChar.Position.X -5;
		
			if((gamePadData.Buttons & GamePadButtons.Right) != 0)
				playerChar.Position.X = playerChar.Position.X +5;
			
			if((gamePadData.Buttons & GamePadButtons.Up) !=0)
				playerChar.Position.Y = playerChar.Position.Y -5;
			
			if((gamePadData.Buttons & GamePadButtons.Down) !=0)
				playerChar.Position.Y = playerChar.Position.Y +5;
			
		}

		public static void Render()
		{
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear();
			
			//All sprites must be rendered here
			background.Render();
			playerChar.Render ();

			// Present the screen
			graphics.SwapBuffers();
		}
	}
}
