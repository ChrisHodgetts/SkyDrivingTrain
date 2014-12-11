using System;

using Sce.PlayStation.Core;


using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace SkyDrivingTrain
{
	public class Projectile
	{
		private SpriteUV sprite;
		private TextureInfo texInfo;
		private int speed;
		private Vector2 position;
		private AppMain.Direction direction;
		private bool live;
		
		//private int moveDirection;
		
		public Projectile(Vector2 position, int speed, AppMain.Direction direction)
		{
			texInfo = new TextureInfo("/Application/assets/missileSheet.png");
			sprite = new SpriteUV(texInfo);	
			sprite.Scale = new Vector2(40.0f, 40.0f);
			sprite.UV.S = new Vector2(0.25f, 1.0f);
			sprite.UV.T = new Vector2(0.0f, 0.0f);
			this.live = false;
			
			this.speed = speed;
			sprite.Position = position;
			this.direction = direction;
			
			//scene.AddChild(sprite);
		}
		
		public bool CollidedWith(SpriteUV sprite)//BlueEnemy enemy, SpriteUV player)
		{
			//
			///define center point of projectiles circle
			//
			float projCenterX = this.sprite.Position.X;
			float projCenterY = this.sprite.Position.Y;
			//
			///define center point of obstacles cirlce
			//
			float obstacleCenterX = sprite.Position.X;
			float obstacleCenterY = sprite.Position.Y;
	 		//
			///define radius of players circle
			//
			//for the 30x30 images
			float projRadius = 15.0f;
			//
			///define radius of obstacles circle
			//
			//32x32 enemies
			float obstacleRadius = 16.0f;
			
			
			float distanceX = projCenterX - obstacleCenterX;
			float distanceY = projCenterY - obstacleCenterY;
			
			float distance = FMath.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
			
			if(distance < (obstacleRadius + projRadius))
			{
				//collision between circle bounds has occured
				return true;
			}
			return false;
		}
		
		public SpriteUV Sprite
		{
			get{ return sprite; }
		}
		
		public int Speed
		{
			get{ return speed; }
			set{ speed = value; }
		}
		
		public bool Live
		{
			get{ return live; }
			set{ live = value; }
		}
		
		public Vector2 Position
		{
			get{ return position; }
			set{ position = value; }
		}
		
		public void Update()
		{
			switch(direction)
			{
				case AppMain.Direction.Up:
					sprite.UV.S = new Vector2(0.25f, 1.0f);
					sprite.UV.T = new Vector2(0.0f, 0.0f);
					sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y + speed);
					break;
				
				case AppMain.Direction.Right:
					sprite.UV.S = new Vector2(0.25f, 1.0f);
					sprite.UV.T = new Vector2(0.25f, 0.0f);
					sprite.Position = new Vector2(sprite.Position.X + speed, sprite.Position.Y);

					break;
				
				case AppMain.Direction.Down:
					sprite.UV.S = new Vector2(0.25f, 1.0f);
					sprite.UV.T = new Vector2(0.50f, 0.0f);
					sprite.Position = new Vector2(sprite.Position.X, sprite.Position.Y - speed);

					break;
				
				case AppMain.Direction.Left:
					sprite.UV.S = new Vector2(0.25f, 1.0f);
					sprite.UV.T = new Vector2(0.75f, 0.0f);
					sprite.Position = new Vector2(sprite.Position.X - speed, sprite.Position.Y);

					break;
			}
			
			
			if(this.position.X > AppMain.screenWidth || this.position.X < 0 || this.position.Y > AppMain.screenHeight || this.position.Y < 0)
			{
				this.Cleanup();
			}
			
		}
		
		public void Cleanup()
		{
			texInfo.Dispose();
			
		}
		
	}
}

