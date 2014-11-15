using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;

	
namespace SkyDrivingTrain
{
	public class AppMain
	{
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		
		private static SpriteUV playerSprite;
		private static SpriteUV backgroundSprite;
		private static SpriteUV followEnemySprite;
		
		private static TextureInfo playerTex;
		private static TextureInfo backgroundTex;
		private static TextureInfo followEnemyTex;

		
		private static float screenHeight;
		private static float screenWidth;
		private static int playerSpeed;
		
		
		
		public static void Main (string[] args)
		{
			Initialize();
			
			//Game loop
			bool quitGame = false;
			while (!quitGame) 
			{
				Update ();
				
				Director.Instance.Update();
				Director.Instance.Render();
				UISystem.Render();
				
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
			
			//Clean up
			Director.Terminate ();
		}

		public static void Initialize ()
		{
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			//Set the ui scene.
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel  = new Panel();
			panel.Width  = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			uiScene.RootWidget.AddChildLast(panel);
			UISystem.SetScene(uiScene);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			//Set variables
			screenHeight = Director.Instance.GL.Context.GetViewport().Height;
			screenWidth = Director.Instance.GL.Context.GetViewport().Width;
			
			playerSpeed = 5;
			
			//initialise player
			playerTex = new TextureInfo("/Application/assets/spikedShip.png");
			playerSprite = new SpriteUV(playerTex);
			//set the sprites size, equal to it's texture
			playerSprite.Quad.S = playerTex.TextureSizef;
			playerSprite.Position = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f);
			//playerSprite.Scale = new Vector2(0.2f, 0.2f);
			
			//initialize follow enemy(Red)
			followEnemyTex = new TextureInfo("/Application/assets/redEnemy.png");
			followEnemySprite = new SpriteUV(followEnemyTex);
			followEnemySprite.Quad.S = followEnemyTex.TextureSizef;
			followEnemySprite.Position = new Vector2(10.0f, screenHeight * 0.5f);
			
			//initialise background
			backgroundTex = new TextureInfo("/Application/assets/background.png");
			backgroundSprite = new SpriteUV(backgroundTex);
			backgroundSprite.Quad.S = backgroundTex.TextureSizef;
			
			//Painters Algorithm
			gameScene.AddChild(backgroundSprite);
			gameScene.AddChild(followEnemySprite);
			gameScene.AddChild(playerSprite);
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{				
			CheckBoundaries();
			Input();
			chasePlayer(followEnemySprite, playerSprite);
		}
		
		public static void Input()
		{
			var gamePadData = GamePad.GetData(0);
			
			//Controls
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				playerSprite.Position = new Vector2(playerSprite.Position.X - playerSpeed, playerSprite.Position.Y);
			}
		
			if((gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				playerSprite.Position = new Vector2(playerSprite.Position.X + playerSpeed, playerSprite.Position.Y);
			}
			
			if((gamePadData.Buttons & GamePadButtons.Up) !=0)
			{
				playerSprite.Position = new Vector2(playerSprite.Position.X, playerSprite.Position.Y + playerSpeed);
			}
			
			if((gamePadData.Buttons & GamePadButtons.Down) !=0)
			{
				playerSprite.Position = new Vector2(playerSprite.Position.X, playerSprite.Position.Y - playerSpeed);
			}
		}
		
		public static void CheckBoundaries()
		{
			//TO DO: FIX THE UPPER AND RIGHT HAND SIDE BOUNDARIES
			if (playerSprite.Position.X >= screenWidth)
			{
				playerSprite.Position = new Vector2(screenWidth - 40.0f, playerSprite.Position.Y);
			}
			if (playerSprite.Position.X <= 10)
			{
				playerSprite.Position = new Vector2(10.0f, playerSprite.Position.Y);
			}
			if (playerSprite.Position.Y <= 10)
			{
				playerSprite.Position = new Vector2(playerSprite.Position.X, 10.0f);
			}
			if (playerSprite.Position.Y >= screenHeight)
			{
				playerSprite.Position = new Vector2(playerSprite.Position.X, screenHeight - 40.0f);
			}
		}
		
		public static void chasePlayer(SpriteUV chaser, SpriteUV player)
		{
			//ALTERNATIVE CHASE "Algorithm"
			if(player.Position.X < chaser.Position.X)
			{
				chaser.Position = new Vector2(chaser.Position.X - 1.0f, chaser.Position.Y);
			}
			else if(player.Position.X > chaser.Position.X)
			{
				chaser.Position = new Vector2(chaser.Position.X + 1.0f, chaser.Position.Y);
			}
			if(player.Position.Y < chaser.Position.Y)
			{
				chaser.Position = new Vector2(chaser.Position.X, chaser.Position.Y - 1.0f);
			}
			else if(player.Position.Y > chaser.Position.Y)
			{
				chaser.Position = new Vector2(chaser.Position.X, chaser.Position.Y + 1.0f);
			}
			
		}
		
	}
}
