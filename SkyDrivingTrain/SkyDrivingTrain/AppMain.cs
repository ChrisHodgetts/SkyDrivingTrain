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
		public enum Direction {Up, Down, Left, Right};
		
		private static Sce.PlayStation.HighLevel.GameEngine2D.Scene 	gameScene;
		private static Sce.PlayStation.HighLevel.UI.Scene 				uiScene;
		
		private static SpriteUV testGateSprite;
		private static SpriteUV backgroundSprite;
	
		private static TextureInfo backgroundTex;
		private static TextureInfo testGateTex;
		
		public static float screenHeight;
		public static float screenWidth;
				
		private static Random random;
		
		private static RedEnemy redEnemy;
		private static BlueEnemy blueEnemy;
		private static GreenEnemy greenEnemy;
		
		private static Player player;
		
		//private static Projectile projectile;
		
		private static List<Projectile> projectiles;
				
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
			
			player = new Player(5);
		
			redEnemy = new RedEnemy(3);
			blueEnemy = new BlueEnemy(2);
			greenEnemy = new GreenEnemy(2);
			
			projectiles = new List<Projectile>();
						
			//initialise background
			backgroundTex = new TextureInfo("/Application/assets/background.png");
			backgroundSprite = new SpriteUV(backgroundTex);
			backgroundSprite.Quad.S = backgroundTex.TextureSizef;
			
			//initialise Gate
			testGateTex = new TextureInfo("/Application/assets/placeHolderGate.png");
			testGateSprite = new SpriteUV(testGateTex);
			testGateSprite.Quad.S = testGateTex.TextureSizef;
			testGateSprite.Scale = new Vector2(0.5f, 0.5f);
			float randPosX = (float)random.Next(50, (int)screenWidth);
			float randPosY = (float)random.Next(50, (int)screenHeight);
			testGateSprite.Position = new Vector2(randPosX, randPosY);
			
			
			//Renders each sprite to scene, using Painters Algorithm
			gameScene.AddChild(backgroundSprite);
			gameScene.AddChild(testGateSprite);
			gameScene.AddChild(redEnemy.Sprite);
			gameScene.AddChild(greenEnemy.Sprite);
			gameScene.AddChild(blueEnemy.Sprite);
			gameScene.AddChild(player.Sprite);
			
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{				
			CheckPlayerBoundaries();
			CheckEnemyBoundaries(greenEnemy);
			
			Input();
			
			ChasePlayer(redEnemy.Sprite, redEnemy.Speed, player.Sprite);
			
			//Movement of green enemy
			RandomMoveAlternateAxis(greenEnemy);
			
			//movement of blue enemy
			
			BluePerimCheck(blueEnemy, player.Sprite);
			
			if(blueEnemy.RandomMove)
			{
				//Movement for blue enemies before perimeter has been breached
				RandomMove(blueEnemy);
			}
			
			if(blueEnemy.PerimeterBroken)
			{
				ChasePlayer(blueEnemy.Sprite, blueEnemy.Speed, player.Sprite);
			}
			
			player.Update();
			
			foreach(Projectile p in projectiles)
			{
				if(p.Live)
				{
					p.Update();
				}
				
				if(p.Position.X > screenWidth || p.Position.X < 0 || p.Position.Y > screenHeight || p.Position.Y < 0)
				{
					projectiles.Remove(p);
				}
			}
			
			
			
			
			//player - blue collisions
			
			if(player.CollidedWith(blueEnemy.Sprite))
			{
				gameScene.RemoveChild(player.Sprite, true);
			}
		}
		
		public static void Input()
		{
			var gamePadData = GamePad.GetData(0);
			
			//Player Controls
			//Move left
			if((gamePadData.Buttons & GamePadButtons.Left) != 0)
			{
				player.Sprite.Position = new Vector2(player.Sprite.Position.X - player.Speed, player.Sprite.Position.Y);
				player.Direction = Direction.Left;	
			}			
			//Move Right
			else if((gamePadData.Buttons & GamePadButtons.Right) != 0)
			{
				player.Sprite.Position = new Vector2(player.Sprite.Position.X + player.Speed, player.Sprite.Position.Y);
				player.Direction = Direction.Right;

			}
			//Move Up
			else if((gamePadData.Buttons & GamePadButtons.Up) != 0)
			{
				player.Sprite.Position = new Vector2(player.Sprite.Position.X, player.Sprite.Position.Y + player.Speed);
				player.Direction = Direction.Up;
			}
			//Move Down
			else if((gamePadData.Buttons & GamePadButtons.Down) != 0)
			{
				player.Sprite.Position = new Vector2(player.Sprite.Position.X, player.Sprite.Position.Y - player.Speed);
				player.Direction = Direction.Down;
			}
			
			if((gamePadData.ButtonsUp & GamePadButtons.Start) != 0)
			{
				Projectile p1 = new Projectile(player.Sprite.Position, 10, player.Direction);
				p1.Live = true;
				projectiles.Add(p1);

				gameScene.AddChild(p1.Sprite);			
				
			}
		}
		
		public static void CheckPlayerBoundaries()
		{
			//TO DO: FIX THE UPPER AND RIGHT HAND SIDE BOUNDARIES
			//Player viewport restrictions/boundaries
			if (player.Sprite.Position.X >= screenWidth)
			{
				player.Sprite.Position = new Vector2(screenWidth - 40.0f, player.Sprite.Position.Y);
			}
			else if (player.Sprite.Position.X <= 10)
			{
				player.Sprite.Position = new Vector2(10.0f, player.Sprite.Position.Y);
			}
			else if (player.Sprite.Position.Y <= 10)
			{
				player.Sprite.Position = new Vector2(player.Sprite.Position.X, 10.0f);
			}
			else if (player.Sprite.Position.Y >= screenHeight)
			{
				player.Sprite.Position = new Vector2(player.Sprite.Position.X, screenHeight - 40.0f);
			}
			
		}
		
		
		public static void CheckEnemyBoundaries(GreenEnemy enemy)
		{
			//Green enemy basic direction reversal upon collision with viewport edge
			if(enemy.Sprite.Position.X >= screenWidth)
			{
				//once collides with right hand side, reverse it's direction.
				enemy.Direction = Direction.Left;
				enemy.WallCollisionCount++;
			}
			
			else if(enemy.Sprite.Position.X <= 10)
			{
				//once collides with left hand side, reverse it's direction.
				enemy.Direction  = Direction.Right;
				enemy.WallCollisionCount++;			
			}
			else if(enemy.Sprite.Position.Y >= screenHeight)
			{
				//once collides with Top of screen, reverse it's direction.
				enemy.Direction  = Direction.Down;
				enemy.WallCollisionCount++;			
			}
			else if(enemy.Sprite.Position.Y <= 10)
			{
				//once collides with bottom of screen, reverse it's direction.
				enemy.Direction  = Direction.Up;
				enemy.WallCollisionCount++;			
			}
			
		}
		
		public static void ChasePlayer(SpriteUV chaser, int chaserSpeed, SpriteUV player)
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
		
		public static void RandomMoveAlternateAxis(GreenEnemy enemy)
		{
			int randMin = 35;
			int randXMax = (int)(screenWidth - 35.0f);
			int randYMax = (int)(screenHeight - 35.0f);
			
			//Basic four directional movement
			if(enemy.Direction == Direction.Right)
			{
				enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X + enemy.Speed, enemy.Sprite.Position.Y);
			}
			
			else if(enemy.Direction == Direction.Left)
			{
				enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X - enemy.Speed, enemy.Sprite.Position.Y);
			}
			
			else if(enemy.Direction == Direction.Up)
			{
				enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X, enemy.Sprite.Position.Y + enemy.Speed);
			}
			
			else if(enemy.Direction == Direction.Down)
			{
				enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X, enemy.Sprite.Position.Y - enemy.Speed);
			}
			
			
			//Get the random value to check against
			
			int xRand = random.Next(randMin, randXMax) + 1;
			int yRand = random.Next(randMin, randYMax) + 1;
			
			//changing greens direction
			
			if(enemy.WallCollisionCount > 1)
			{
				if((enemy.Sprite.Position.X > xRand) /*&& (sprite.Position.Y > yRand)*/)
				{
					if(enemy.Direction == Direction.Right)
					{
						enemy.Direction = Direction.Down;
						enemy.WallCollisionCount = 0;
					}
					else if(enemy.Direction == Direction.Left)
					{
						enemy.Direction = Direction.Up;
						enemy.WallCollisionCount = 0;
					}
					else if(enemy.Direction == Direction.Down)
					{
						enemy.Direction = Direction.Right;
						enemy.WallCollisionCount = 0;
					}
					else if(enemy.Direction == Direction.Up)
					{
						enemy.Direction = Direction.Left;
						enemy.WallCollisionCount = 0;
					}
				}
			}
		}
		
		public static void BluePerimCheck(BlueEnemy enemy, SpriteUV player)
		{
			//define center point of blues circle
			float enemyCenterX = enemy.Sprite.Position.X;
			float enemyCenterY = enemy.Sprite.Position.Y;
			
			//define center point of players circle
			float playerCenterX = player.Position.X;
			float playerCenterY = player.Position.Y;
			
			//define radius of blues circle
			float enemyRadius = 35.0f;
			//define radius of player circle
			float playerRadius = 35.0f;
			
			float distanceX = playerCenterX - enemyCenterX;
			float distanceY = playerCenterY - enemyCenterY;
			
			if(distanceX < (enemyRadius + playerRadius) && distanceY < (enemyRadius + playerRadius))
			{
				//collision between circle bounds has occured
				enemy.PerimeterBroken = true;
			}
		}
		
		public static void RandomMove(BlueEnemy enemy)
		{
			int direction = random.Next(1, 5);
			
			switch(direction)
			{
				case 1:
				//move up
				enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X, enemy.Sprite.Position.Y + enemy.Speed);
				break;
				
				case 2:
				//move down
				enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X, enemy.Sprite.Position.Y - enemy.Speed);
				break;
				
				case 3:
				//move left
				enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X - enemy.Speed, enemy.Sprite.Position.Y);
				break;
				
				case 4:
				//move right
				enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X + enemy.Speed, enemy.Sprite.Position.Y);
				break;
			}
			
		}
	}
}
