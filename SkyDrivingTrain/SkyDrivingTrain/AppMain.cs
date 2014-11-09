using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;


namespace SkyDrivingTrain
{
	public class AppMain
	{
		/****CHRIS****
		Do you prefer variables grouped by type, or by their associations?
		I've currently gone with grouping them by variable type
		*/
		private static GraphicsContext graphics;
		private static Texture2D backgroundTexture;
		private static Texture2D playerCharTexture;
		private static Texture2D followEnemyTexture;//Follow = red enemy
		private static Texture2D randomEnemyTexture;//Random = green enemy
		private static Texture2D perimeterEnemyTexture;//Perimter = blue enemy
		
		private static Sprite background;
		private static Sprite playerChar;
		private static Sprite followEnemy;
		private static Sprite randomEnemy;
		private static Sprite perimterEnemy;
			
		private static BgmPlayer bgmP;
		private static int speed;
		private static int screenWidth;
		private static int screenHeight;
		
		
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
			//Initialise Variables
			speed = 5;
			screenWidth = 910;
			screenHeight = 504;
			
			// Set up the graphics system
			graphics = new GraphicsContext();
			
			
			
			//Load in audio
			Bgm bgm = new Bgm("/Application/assets/gameMusic.mp3");
			bgmP = bgm.CreatePlayer();
			bgmP.Loop = true;
			bgmP.Play ();
			
			//Load in player character
			playerCharTexture = new Texture2D("/Application/assets/spikedShip.png", false);
			playerChar = new Sprite(graphics, playerCharTexture);
			playerChar.Position.X = 5;
			playerChar.Position.Y = 5;
			
			//Load in red 'follow' enemy for testing
			followEnemyTexture = new Texture2D("/Application/assets/redEnemy.png", false);
			followEnemy = new Sprite(graphics, followEnemyTexture);
			followEnemy.Position.X = 280;
			followEnemy.Position.Y = 77;
			
			//Load in green 'random' enemy for testing
			randomEnemyTexture = new Texture2D("/Application/assets/greenEnemy.png", false);
			randomEnemy = new Sprite(graphics, randomEnemyTexture);
			randomEnemy.Position.X = 380;
			randomEnemy.Position.Y = 177;
			
			//Load in blue 'random' enemy for testing
			perimeterEnemyTexture = new Texture2D("/Application/assets/blueEnemy.png", false);
			perimterEnemy = new Sprite(graphics, perimeterEnemyTexture);
			perimterEnemy.Position.X = 180;
			perimterEnemy.Position.Y = 177;
			
			
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
			
			//Boundaries
			if (playerChar.Position.X >= screenWidth)
				playerChar.Position.X = screenWidth;
			
			if (playerChar.Position.X <= 10)
				playerChar.Position.X = 10;
			
			if (playerChar.Position.Y <= 10)
				playerChar.Position.Y = 10;
			
			if (playerChar.Position.Y >= screenHeight)
				playerChar.Position.Y = screenHeight;
			
			//Controls
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
				playerChar.Position.X = playerChar.Position.X -speed;
		
			if((gamePadData.Buttons & GamePadButtons.Right) != 0)
				playerChar.Position.X = playerChar.Position.X +speed;
			
			if((gamePadData.Buttons & GamePadButtons.Up) !=0)
				playerChar.Position.Y = playerChar.Position.Y -speed;
			
			if((gamePadData.Buttons & GamePadButtons.Down) !=0)
				playerChar.Position.Y = playerChar.Position.Y +speed;
			
			
			chasePlayer(followEnemy, playerChar);	
		}

		public static void Render()
		{
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear();
			
			//All sprites must be rendered here
			background.Render();
			playerChar.Render();
			followEnemy.Render();
			randomEnemy.Render();
			perimterEnemy.Render();

			// Present the screen
			graphics.SwapBuffers();
		}
		
		public static void chasePlayer(Sprite chaser, Sprite player)
		{
			//ALTERNATIVE CHASE "Algorithm"
			if(player.Position.X < chaser.Position.X)
			{
				followEnemy.Position.X -= 1;
			}
			else if(player.Position.X > chaser.Position.X)
			{
				followEnemy.Position.X += 1;
			}
			if(player.Position.Y < chaser.Position.Y)
			{
				followEnemy.Position.Y -= 1;
			}
			else if(player.Position.Y > chaser.Position.Y)
			{
				followEnemy.Position.Y += 1;
			}
		}
	}
}
