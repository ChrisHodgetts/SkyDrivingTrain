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
		
		//private static SpriteUV testGateSprite;
		private static SpriteUV backgroundSprite;
		
		private static Gate testGate;
	
		private static TextureInfo backgroundTex;
		
		public static float screenHeight;
		public static float screenWidth;
				
		private static Random random;
		
		private static RedEnemy redEnemy;
		private static BlueEnemy blueEnemy;
		private static GreenEnemy greenEnemy;
		
		private static List<RedEnemy> redEnemies;
		
		private static Player player;
				
		private static List<Projectile> projectiles;
		private static List<Gate> gates;
		
		private static BgmPlayer bgmp;
		private static SoundPlayer gameOverSP;
		private static SoundPlayer explosionSP;
		private static SoundPlayer speedUpSP;
		private static SoundPlayer missileSP;
		
		private static int redTimeCount;
		private static int scoreTimeCount;
		private static int playerSpeedTimeCount;
		
		private static float redSpeed;
		private static float playerSpeed;
		
		private static bool spawnNewGate;
		private static bool playerAlive;
		private static bool fireEnabled;
		
		private static  Sce.PlayStation.HighLevel.UI.Label score;
		private static Sce.PlayStation.HighLevel.UI.Label instructions;
		private static Sce.PlayStation.HighLevel.UI.Label instructions2;
		
		private static int scoreCount;
		private static int blueMoveFrameCount;
		
		private static  Sce.PlayStation.HighLevel.UI.Label gameOverScreen;

		private static bool playerCanShoot;
		private static bool scoreFromGate;
		private static bool gameOver;
				
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
			
			gameOver = false;
			
			//Setup
			Director.Initialize ();
			UISystem.Initialize(Director.Instance.GL.Context);
			
			//Set the ui scene.
			uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			Panel panel  = new Panel();
			panel.Width  = Director.Instance.GL.Context.GetViewport().Width;
			panel.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			scoreCount = 0;
			blueMoveFrameCount = 0;
			
			score = new Sce.PlayStation.HighLevel.UI.Label();
			score.X = 10;
			score.Y = 10;
			score.Width = 300;
			score.TextColor = (UIColor)Colors.Orange;
			score.Text = "Score: " + scoreCount;
			
			gameOverScreen = new Sce.PlayStation.HighLevel.UI.Label();
			gameOverScreen.X = 400;
			gameOverScreen.Y = 250;
			gameOverScreen.Width = 200;
			gameOverScreen.TextColor = (UIColor)Colors.Magenta;
			gameOverScreen.Text = "GAME OVER";
			
			instructions = new Sce.PlayStation.HighLevel.UI.Label();
			instructions.X = 525;
			instructions.Y = 10;
			instructions.Width = 500;
			instructions.TextColor = (UIColor)Colors.Magenta;
			instructions.Text = "Missiles disabled, score higher to activate!";
			
			instructions2 = new Sce.PlayStation.HighLevel.UI.Label();
			instructions2.X = 600;
			instructions2.Y = 10;
			instructions2.Width = 500;
			instructions2.TextColor = (UIColor)Colors.Magenta;
			instructions2.Text = "Missiles enabled, press start to fire.";
			
			
			uiScene.RootWidget.AddChildLast(panel);
			uiScene.RootWidget.AddChildLast(score);
			uiScene.RootWidget.AddChildLast(instructions);
			uiScene.RootWidget.RemoveChild(instructions2);
			UISystem.SetScene(uiScene);
			
			//Set game scene
			gameScene = new Sce.PlayStation.HighLevel.GameEngine2D.Scene();
			gameScene.Camera.SetViewFromViewport();
			
			//Set variables
			screenHeight = Director.Instance.GL.Context.GetViewport().Height;
			screenWidth = Director.Instance.GL.Context.GetViewport().Width;
			
			redSpeed = 3.0f;
			playerSpeed = 5.0f;
			player = new Player(playerSpeed);
			
			redEnemy = new RedEnemy(redSpeed);
			blueEnemy = new BlueEnemy(2);
			greenEnemy = new GreenEnemy(2);
			
			redEnemies = new List<RedEnemy>();
			redEnemies.Add(redEnemy);
			redTimeCount = 0;
			
			playerSpeedTimeCount = 0;
			
			projectiles = new List<Projectile>();
			
			playerAlive = true;
			fireEnabled = false;
			scoreFromGate = false;
			
			gates = new List<Gate>();

			//initialise background
			backgroundTex = new TextureInfo("/Application/assets/background.png");
			backgroundSprite = new SpriteUV(backgroundTex);
			backgroundSprite.Quad.S = backgroundTex.TextureSizef;
			
			//initialise Gate
			float randGatePosX = (float)random.Next(50, (int)(screenWidth * 0.8f));
			float randGatePosY = (float)random.Next(50, (int)(screenHeight * 0.8f));
			testGate = new Gate();
			testGate.Sprite.Position = new Vector2(randGatePosX, randGatePosY);
			
			//initialise background music
			Bgm bgm = new Bgm("/Application/assets/gameMusic.mp3");
			bgmp = bgm.CreatePlayer();
			bgmp.Loop = true;
			bgmp.Play();
			
			//intialise missile sound
			Sound missileSound;
			missileSound = new Sound("/Application/assets/missile_shot.wav");
			missileSP = missileSound.CreatePlayer();
			
			//initalise explosion sound
			Sound explosionSound;
			explosionSound = new Sound("/Application/assets/explosion.wav");
			explosionSP = explosionSound.CreatePlayer();
			
			//initialise game over sound
			Sound gameOverSound;
			gameOverSound = new Sound ("/Application/assets/gameOver.wav");
			gameOverSP = gameOverSound.CreatePlayer();
			
			//initialise speed up sound
			Sound speedUpSound;
			speedUpSound = new Sound ("/Application/assets/speedUp.wav");
			speedUpSP = speedUpSound.CreatePlayer();
			
			
			gates.Add(testGate);
			
			spawnNewGate = false;
			playerCanShoot = false;
			
			//Renders each sprite to scene, using Painters Algorithm
			gameScene.AddChild(backgroundSprite);
			gameScene.AddChild(testGate.Sprite);
			gameScene.AddChild(greenEnemy.Sprite);

			gameScene.AddChild(redEnemy.Sprite);
			gameScene.AddChild(blueEnemy.Sprite);
			gameScene.AddChild(player.Sprite);
			
			//Run the scene.
			Director.Instance.RunWithScene(gameScene, true);
		}
		
		public static void Update()
		{				
			if(!gameOver)
			{
				CheckPlayerBoundaries();
				CheckEnemyBoundaries(greenEnemy);
				Input();
				
				foreach(RedEnemy r in redEnemies)
				{
					ChasePlayer(r.Sprite, r.Speed, player.Sprite);
				}
				
				redTimeCount++;
				scoreTimeCount++;
				playerSpeedTimeCount++;
				
	
				if(redTimeCount >= 600)
				{
					redSpeed += 0.1f;
					redTimeCount = 0;
					RedEnemy red = new RedEnemy(redSpeed);
					red.Sprite.Position = chooseRedSpawn();
		
					gameScene.AddChild(red.Sprite);
					redEnemies.Add(red);
				}
				
				if(scoreTimeCount >= 60)
				{
					scoreTimeCount -= 60;
					scoreCount += 1;
				}
				
				score.Text = "Score: " + scoreCount;
				
				if(playerSpeedTimeCount >= 90)
				{
					playerSpeedTimeCount -= 90;
					playerSpeed = 5.0f;
					player.Speed = playerSpeed;
				}
				
				if(fireEnabled == true)
				{
					uiScene.RootWidget.RemoveChild(instructions);
					uiScene.RootWidget.AddChildLast(instructions2);
				}
				
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
						missileSP.Volume = 0.5f;
						missileSP.Play();
						//projectile->blue enemy collisions
						if(p.CollidedWith(blueEnemy.Sprite))
						{
							gameScene.RemoveChild(p.Sprite, true);
							gameScene.RemoveChild(blueEnemy.Sprite, true);
							
							explosionSP.Volume = 0.5f;
							explosionSP.Play();
						}
						
						//projectile->green enemy collisions
						if(p.CollidedWith(greenEnemy.Sprite))
						{
							gameScene.RemoveChild(p.Sprite, true);
							gameScene.RemoveChild(greenEnemy.Sprite, true);
							explosionSP.Volume = 0.5f;
							explosionSP.Play();
						}
						//projectile->red enemy collisions
						foreach(RedEnemy red in redEnemies)
						{
							if(p.CollidedWith(red.Sprite))
							{
								gameScene.RemoveChild(p.Sprite, true);
								gameScene.RemoveChild(red.Sprite, true);
								explosionSP.Volume = 0.5f;
								explosionSP.Play();
							}
						}
					}
					
					if(p.Position.X > screenWidth || p.Position.X < 0 || p.Position.Y > screenHeight || p.Position.Y < 0)
					{
						p.Live = false;
						projectiles.Remove(p);
					}
				}
				
				if(scoreCount >= 30)
				{
							playerCanShoot = true;
							fireEnabled = true;
				}
				
				//player - enemy collisions
				foreach(RedEnemy r in redEnemies)
				{
					
					if(player.CollidedWith(r.Sprite))
					{
						playerAlive = false;
						gameScene.RemoveChild(player.Sprite, true);
						gameOver = true;
						
					}
				}
				
				if(player.CollidedWith(greenEnemy.Sprite))
				{
					playerAlive = false;
					gameScene.RemoveChild(player.Sprite, true);
					gameOver = true;
				}
					
				
				if(player.CollidedWith(blueEnemy.Sprite))
				{
					playerAlive = false;
					gameScene.RemoveChild(player.Sprite, true);
					gameOver = true;				
				}
				
				//player - gate collisions
				foreach(Gate g in gates)
				{
					Rectangle gRect = new Rectangle(g.Sprite.Position.X, g.Sprite.Position.Y, 80.0f, 10.0f);
					Rectangle playerRect = new Rectangle(player.Sprite.Position.X, player.Sprite.Position.Y, 30.0f, 30.0f); 
					
					if(Overlaps(gRect, playerRect) && gameScene.Children.Contains(g.Sprite))
					{
						gameScene.RemoveChild(g.Sprite, true);
						scoreFromGate = true;
						
						speedUpSP.Volume = 0.5f;
						speedUpSP.Play();
						playerSpeed *= 1.5f;
						player.Speed = playerSpeed;
						
						score.Text = "Score: " + scoreCount;
						spawnNewGate = true;
					}
				}
				
				//only increase score once per gate collision
				if(scoreFromGate)
				{
					scoreFromGate = false;
					scoreCount += 5;
				}
				
				if(spawnNewGate)
				{
					spawnNewGate = false;
					
					float randGatePosX = (float)random.Next(50, (int)(screenWidth * 0.8f));
					float randGatePosY = (float)random.Next(50, (int)(screenHeight * 0.8f));
					Gate newGate = new Gate();
					newGate.Sprite.Position = new Vector2(randGatePosX, randGatePosY);
							
					gates.Add(newGate);
					gameScene.AddChild(newGate.Sprite);
				}
			}
			else
			{		
				uiScene.RootWidget.AddChildLast(gameOverScreen);
				bgmp.Stop();
				gameOverSP.Volume = 0.5f;
				gameOverSP.Play();
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
			
			if(((gamePadData.ButtonsUp & GamePadButtons.Start) != 0) && playerCanShoot)
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
		
		public static Vector2 chooseRedSpawn()
		{
			//define four spawn locations
			//N
			Vector2 loc1 = new Vector2(screenWidth / 2.0f, screenHeight - 10.0f);
			//E
			Vector2 loc2 = new Vector2(screenWidth - 10.0f, screenHeight / 2.0f);
			//S
			Vector2 loc3 = new Vector2(screenWidth / 2.0f, 10.0f);
			//W
			Vector2 loc4 = new Vector2(10.0f, screenHeight / 2.0f);
			
			Random random = new Random();
			int choice = random.Next(1, 5);//picks 1, 2, 3, or 4
			                           
			switch(choice)
			{
				case 1:
					return loc1;
					
				case 2:
					return loc2;
					
				case 3:
					return loc3;
					
				case 4:
					return loc4;
			}
			
			return loc1;
		}
		
		public static void ChasePlayer(SpriteUV chaser, float chaserSpeed, SpriteUV player)
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
			//check boundaries
			if (enemy.Sprite.Position.X >= screenWidth)
			{
				enemy.Sprite.Position = new Vector2(screenWidth - 40.0f, enemy.Sprite.Position.Y);
			}
			else if (enemy.Sprite.Position.X <= 10)
			{
				enemy.Sprite.Position = new Vector2(10.0f, enemy.Sprite.Position.Y);
			}
			else if (enemy.Sprite.Position.Y <= 10)
			{
				enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X, 10.0f);
			}
			else if (enemy.Sprite.Position.Y >= screenHeight)
			{
				enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X, screenHeight - 40.0f);
			}
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
			float enemyRadius = 16.0f;
			//define radius of player circle
			float playerRadius = 15.0f;
			
			float distanceX = playerCenterX - enemyCenterX;
			float distanceY = playerCenterY - enemyCenterY;
			
			float distance = FMath.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
				
			if(distanceX < (enemyRadius + playerRadius) && distanceY < (enemyRadius + playerRadius))
			{
				//collision between circle bounds has occured
				enemy.PerimeterBroken = true;
			}
		}
		
		public static void RandomMove(BlueEnemy enemy)
		{
			blueMoveFrameCount++;
			int direction = random.Next(1, 5);
			
			if(blueMoveFrameCount >= 30)
			{
				blueMoveFrameCount -= 30;
				switch(direction)
				{
					case 1:
					//move up
					for(int i = 0; i < 5; i++)
					{
						enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X, enemy.Sprite.Position.Y + enemy.Speed * 2);
						if(enemy.Sprite.Position.Y >= screenWidth)
						{	
							direction = 2;
							break;
						}
					}
					break;
					
					case 2:
					//move down
					for(int i = 0; i < 5; i++)
					{
						enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X, enemy.Sprite.Position.Y - enemy.Speed * 2);
						if(enemy.Sprite.Position.Y <= 0)
						{
							direction = 1;
							break;
						}
					}
					break;
					
					case 3:
					//move left
					for(int i = 0; i < 5; i++)
					{
						enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X - enemy.Speed * 2, enemy.Sprite.Position.Y);
						if(enemy.Sprite.Position.X <= 0)
						{
							direction = 4;
							break;
						}
					}
					break;
					
					case 4:
					//move right
					for(int i = 0; i < 5; i++)
					{
						enemy.Sprite.Position = new Vector2(enemy.Sprite.Position.X + enemy.Speed * 2, enemy.Sprite.Position.Y);
						if(enemy.Sprite.Position.X >= screenWidth)
						{
							direction = 3;
							break;
						}
					}
					break;
				}
			}
			
		}
		
		private static bool Overlaps(Rectangle rect1, Rectangle rect2)
		{
			//first rectangle is too far to the left to overlap
			if(rect1.X + rect1.Width < rect2.X)
			{
				return false;
			}
			//first rectangle is too far to the right to overlap
			if(rect1.X > rect2.X + rect2.Width)
			{
				return false;
			}
			//first rectangle is too high to overlap
			if(rect1.Y + rect1.Height < rect2.Y)
			{
				return false;
			}
			//first rectangle is too low to overlap
			if(rect1.Y > rect2.Y + rect2.Height)
			{
				return false;
			}
			//overlap must have occurred
			else
			{
				return true;
			}
		}
	}
}
