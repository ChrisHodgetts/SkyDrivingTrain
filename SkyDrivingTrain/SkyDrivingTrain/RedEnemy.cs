using System;

using Sce.PlayStation.Core;


using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace SkyDrivingTrain
{
	public class RedEnemy
	{
		private SpriteUV sprite;
		private TextureInfo texInfo;
		private float speed;
		private bool alive;
		
		public RedEnemy (float speed)
		{
			texInfo = new TextureInfo("/Application/assets/enemy_R.png");
			sprite = new SpriteUV(texInfo);			
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = AppMain.chooseRedSpawn();
			//center = node.LocalToWorld(bounds.Center);
			this.speed = speed;
			this.alive = true;
			
			//scene.AddChild(sprite);
		}
		
		public bool CollidedWith(SpriteUV sprite)//BlueEnemy enemy, SpriteUV player)
		{
			//
			///define center point of enemy's circle
			//
			float enemyCenterX = this.sprite.Position.X;
			float enemyCenterY = this.sprite.Position.Y;
			//
			///define center point of obstacles cirlce
			//
			float obstacleCenterX = sprite.Position.X;
			float obstacleCenterY = sprite.Position.Y;
	 		//
			///define radius of enemy's circle
			//
			float enemyRadius = 2.0f;
			//
			///define radius of obstacles circle
			//
			float obstacleRadius = 2.0f;
			
			
			float distanceX = enemyCenterX - obstacleCenterX;
			float distanceY = enemyCenterY - obstacleCenterY;
			
			float distance = FMath.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
			
			if(distance < (obstacleRadius + enemyRadius))
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
		
		public bool Alive
		{
			get{ return alive;}
			set{ alive = value;}
		}
		
		public float Speed
		{
			get{ return speed; }
			set{ speed = value; }
		}
		
		public void Update()
		{
			
			
		}
		
		public bool HasCollidedWith(SpriteUV sprite)
		{
			float xObstacleBound = (sprite.Position.X + (sprite.TextureInfo.Texture.Width * sprite.Scale.X)) * 0.5f;
			float yObstacleBound = (sprite.Position.Y + (sprite.TextureInfo.Texture.Height * sprite.Scale.Y)) * 0.32f;
			
			
				float xBound = (this.Sprite.Position.X + (this.Sprite.TextureInfo.Texture.Width * this.Sprite.Scale.X)) * 0.5f;
				float yBound = (this.Sprite.Position.Y + (this.Sprite.TextureInfo.Texture.Height * this.Sprite.Scale.Y)) * 0.32f;
				
				if(sprite.Position.X > this.Sprite.Position.X && sprite.Position.X < xBound)
				{
					if(sprite.Position.Y > this.Sprite.Position.Y && sprite.Position.Y < yBound)
					{
						return true;
					}
				}
				if(xObstacleBound > this.Sprite.Position.X && xObstacleBound < xBound)
				{
					if(yObstacleBound > this.Sprite.Position.Y && yObstacleBound < yBound)
					{
						return true;
					}
				}
			return false;
		}
		
		
		public void Cleanup()
		{
			texInfo.Dispose();
		}
		
	}
}

