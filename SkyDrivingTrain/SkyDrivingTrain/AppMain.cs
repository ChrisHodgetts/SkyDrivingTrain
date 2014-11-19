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
		private enum Direction {Up, Down, Left, Right};
		
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		
		private static SpriteUV playerSprite;
		private static SpriteUV backgroundSprite;
		private static SpriteUV followEnemySprite; //R
		private static SpriteUV randomEnemySprite; //G
		private static SpriteUV perimEnemySprite; //B
		
		private static TextureInfo playerTex;
		private static TextureInfo backgroundTex;
		private static TextureInfo followEnemyTex;
		private static TextureInfo randomEnemyTex;
		private static TextureInfo perimEnemyTex;
		
		private static float screenHeight;
		private static float screenWidth;
		private static int playerSpeed;
		private static int greenSpeed;
		private static int blueSpeed;
		private static int redSpeed;
		private static int greenWallCollisionCount;
		
		private static bool bluePerimeterBroken;
		
		private static Direction randomEnemyDirection;
		
		private static Random random;
		
		
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
			random = new Random();
			
			//Setup
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
			greenSpeed = 2;
			redSpeed = 3;
			blueSpeed = 2;
			greenWallCollisionCount = 0;
			bluePerimeterBroken = false;
			
			//initialise player
			playerTex = new TextureInfo("/Application/assets/spikedShip.png");
			playerSprite = new SpriteUV(playerTex);
			//set the sprites size, equal to it's texture
			playerSprite.Quad.S = playerTex.TextureSizef;
			playerSprite.Position = new Vector2(screenWidth * 0.5f, screenHeight * 0.5f);
			//playerSprite.Scale = new Vector2(0.2f, 0.2f);
			
			//initialise follow enemy(Red)
			followEnemyTex = new TextureInfo("/Application/assets/redEnemy.png");
			followEnemySprite = new SpriteUV(followEnemyTex);
			followEnemySprite.Quad.S = followEnemyTex.TextureSizef;
			followEnemySprite.Position = new Vector2(10.0f, screenHeight * 0.5f);
			
			//initialise random enemy(green)
			randomEnemyTex = new TextureInfo("/Application/assets/greenEnemy.png");
			randomEnemySprite = new SpriteUV(randomEnemyTex);
			randomEnemySprite.Quad.S = randomEnemyTex.TextureSizef;
			randomEnemySprite.Position = new Vector2(500.0f, 50.0f);
			randomEnemyDirection = Direction.Right;
			
			//initialise perimeter enemy(blue)
			perimEnemyTex = new TextureInfo("/Application/assets/blueEnemy.png");
			perimEnemySprite = new SpriteUV(perimEnemyTex);
			perimEnemySprite.Quad.S = perimEnemyTex.TextureSizef;
			perimEnemySprite.Position = new Vector2(200.0f, 400.0f);
			
			//initialise background
			backgroundTex = new TextureInfo("/Application/assets/background.png");
			backgroundSprite = new SpriteUV(backgroundTex);
			backgroundSprite.Quad.S = backgroundTex.TextureSizef;
			
			//Renders each sprite to scene, using Painters Algorithm
			gameScene.AddChild(backgroundSprite);
			gameScene.AddChild(followEnemySprite);
			gameScene.AddChild(randomEnemySprite);
			gameScene.AddChild(perimEnemySprite);
			gameScene.AddChild(playerSprite);
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{				
			CheckBoundaries();
			
			Input();
			
			chasePlayer(followEnemySprite, redSpeed, playerSprite);
			
			randomGreenMove();
			
			calculateBlueDistance();
			
			if(bluePerimeterBroken)
			{
				chasePlayer(perimEnemySprite, blueSpeed, playerSprite);
			}
						
			/*switch(playerDirection)
			{
				case Direction.Up:
					playerSprite.Rotate(0.0f);
					break;
				
				case Direction.Right:
					playerSprite.Rotate(90.0f);
					break;
				
				case Direction.Down:
					playerSprite.Rotate(180.0f);
					break;
				
				case Direction.Left:
					playerSprite.Rotate(270.0f);
					break;
			}*/
		}
		
		public static void Input()
		{
			var gamePadData = GamePad.GetData(0);
			
			//Player Controls
			//Move left
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				playerSprite.Position = new Vector2(playerSprite.Position.X - playerSpeed, playerSprite.Position.Y);
				//playerDirection = Direction.Left;
			}
			//Move Right
			if((gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				playerSprite.Position = new Vector2(playerSprite.Position.X + playerSpeed, playerSprite.Position.Y);
				//playerDirection = Direction.Right;
			}
			//Move Up
			if((gamePadData.Buttons & GamePadButtons.Up) != 0)
			{
				playerSprite.Position = new Vector2(playerSprite.Position.X, playerSprite.Position.Y + playerSpeed);
				//playerDirection = Direction.Up;
			}
			//Move Down
			if((gamePadData.Buttons & GamePadButtons.Down) != 0)
			{
				playerSprite.Position = new Vector2(playerSprite.Position.X, playerSprite.Position.Y - playerSpeed);
				//playerDirection = Direction.Down;
			}
		}
		
		public static void CheckBoundaries()
		{
			//TO DO: FIX THE UPPER AND RIGHT HAND SIDE BOUNDARIES
			//Player viewport restrictions/boundaries
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
			
			//Green enemy basic direction reversal upon collision with viewport edge
			if(randomEnemySprite.Position.X >= screenWidth)
			{
				//once collides with right hand side, reverse it's direction.
				randomEnemyDirection = Direction.Left;
				greenWallCollisionCount++;
			}
			
			else if(randomEnemySprite.Position.X <= 10)
			{
				//once collides with left hand side, reverse it's direction.
				randomEnemyDirection = Direction.Right;
				greenWallCollisionCount++;
			}
			else if(randomEnemySprite.Position.Y >= screenHeight)
			{
				//once collides with Top of screen, reverse it's direction.
				randomEnemyDirection = Direction.Down;
				greenWallCollisionCount++;
			}
			else if(randomEnemySprite.Position.Y <= 10)
			{
				//once collides with bottom of screen, reverse it's direction.
				randomEnemyDirection = Direction.Up;
				greenWallCollisionCount++;
			}
		}
		
		public static void chasePlayer(SpriteUV chaser, int chaserSpeed, SpriteUV player)
		{
			//ALTERNATIVE CHASE "Algorithm"
			if(player.Position.X < chaser.Position.X)
			{
				chaser.Position = new Vector2(chaser.Position.X - chaserSpeed, chaser.Position.Y);
			}
			else if(player.Position.X > chaser.Position.X)
			{
				chaser.Position = new Vector2(chaser.Position.X + chaserSpeed, chaser.Position.Y);
			}
			if(player.Position.Y < chaser.Position.Y)
			{
				chaser.Position = new Vector2(chaser.Position.X, chaser.Position.Y - chaserSpeed);
			}
			else if(player.Position.Y > chaser.Position.Y)
			{
				chaser.Position = new Vector2(chaser.Position.X, chaser.Position.Y + chaserSpeed);
			}
		}
		
		public static void randomGreenMove()
		{
			int randMin = 35;
			int randXMax = (int)(screenWidth - 35.0f);
			int randYMax = (int)(screenHeight - 35.0f);
			
			//Basic four directional movement
			if(randomEnemyDirection == Direction.Right)
			{
				randomEnemySprite.Position = new Vector2(randomEnemySprite.Position.X + greenSpeed, randomEnemySprite.Position.Y);
			}
			
			else if(randomEnemyDirection == Direction.Left)
			{
				randomEnemySprite.Position = new Vector2(randomEnemySprite.Position.X - greenSpeed, randomEnemySprite.Position.Y);
			}
			
			else if(randomEnemyDirection == Direction.Up)
			{
				randomEnemySprite.Position = new Vector2(randomEnemySprite.Position.X, randomEnemySprite.Position.Y + greenSpeed);
			}
			
			else if(randomEnemyDirection == Direction.Down)
			{
				randomEnemySprite.Position = new Vector2(randomEnemySprite.Position.X, randomEnemySprite.Position.Y - greenSpeed);
			}
			
			
			//Get the random value to check against
			
			int xRand = random.Next(randMin, randXMax) + 1;
			int yRand = random.Next(randMin, randYMax) + 1;
			
			//changing greens direction
			
			if(greenWallCollisionCount > 1)
			{
				if((randomEnemySprite.Position.X > xRand) && (randomEnemySprite.Position.Y > yRand))
				{
					if(randomEnemyDirection == Direction.Right)
					{
						randomEnemyDirection = Direction.Down;
						greenWallCollisionCount = 0;
					}
					else if(randomEnemyDirection == Direction.Left)
					{
						randomEnemyDirection = Direction.Up;
						greenWallCollisionCount = 0;
					}
					else if(randomEnemyDirection == Direction.Down)
					{
						randomEnemyDirection = Direction.Right;
						greenWallCollisionCount = 0;
					}
					else if(randomEnemyDirection == Direction.Up)
					{
						randomEnemyDirection = Direction.Left;
						greenWallCollisionCount = 0;
					}
				}
			}
		}
		
		public static void calculateBlueDistance()
		{
			//define center point of blues circle
			float blueCenterX = perimEnemySprite.Position.X;
			float blueCenterY = perimEnemySprite.Position.Y;
			
			//define center point of players circle
			float playerCenterX = playerSprite.Position.X;
			float playerCenterY = playerSprite.Position.Y;
			
			//define radius of blues circle
			float blueRadius = 35.0f;
			//define radius of player circle
			float playerRadius = 35.0f;
			
			float distanceX = playerCenterX - blueCenterX;
			float distanceY = playerCenterY - blueCenterY;
			
			if(distanceX < (blueRadius + playerRadius) && distanceY < (blueRadius + playerRadius))
			{
				//collision between circle bounds has occured
				bluePerimeterBroken = true;
			}
			
			
		}
			
		
	}
}
