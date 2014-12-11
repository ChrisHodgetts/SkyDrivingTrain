using System;

using Sce.PlayStation.Core;


using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace SkyDrivingTrain
{
	public class Player
	{
		private SpriteUV sprite;
		private TextureInfo texInfo;
		private float speed;
		
		private Bounds2 bounds;
		private Vector2 center;
		private AppMain.Direction direction;
		
		
		public Player(float speed)
		{
			texInfo = new TextureInfo("/Application/assets/spikedShipSheet.png");
			sprite = new SpriteUV(texInfo);	
			sprite.Scale = new Vector2(40.0f, 40.0f);
			//sprite UV uses a trans/rotat/scale
			//S = scale down so don't use entire spritesheet
			sprite.UV.S = new Vector2(0.25f, 1.0f);
			//T = translate (starting point of image on spritesheet)
			sprite.UV.T = new Vector2(0.0f, 0.0f);
			
			
			//sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = new Vector2(AppMain.screenWidth * 0.5f, AppMain.screenHeight * 0.5f);

			this.speed = speed;
			
			this.direction = AppMain.Direction.Up;
			
			//bounds = new Bounds2();
			//sprite.GetlContentLocalBounds(ref bounds);
			
			//bounds.Scale(new Vector2(0.001f, 0.001f), sprite.Position);
			
			
			//center = node.LocalToWorld(bounds.Center);
			

			
			//scene.AddChild(sprite);
		}
		
		public SpriteUV Sprite
		{
			get{ return sprite; }
		}
		
		public AppMain.Direction Direction
		{
			get{ return direction; }
			set{ direction = value; }	
		}
		
		public Bounds2 Bounds
		{
			get { return bounds; }
		}
		public float Speed
		{
			get{ return speed; }
			set{ speed = value; }
		}
		
		public void Update()
		{			
			switch(direction)
			{
				case AppMain.Direction.Up:
					sprite.UV.S = new Vector2(0.25f, 1.0f);
					sprite.UV.T = new Vector2(0.0f, 0.0f);
					break;
				
				case AppMain.Direction.Right:
					sprite.UV.S = new Vector2(0.25f, 1.0f);
					sprite.UV.T = new Vector2(0.25f, 0.0f);
					break;
				
				case AppMain.Direction.Down:
					sprite.UV.S = new Vector2(0.25f, 1.0f);
					sprite.UV.T = new Vector2(0.50f, 0.0f);
					break;
				
				case AppMain.Direction.Left:
					sprite.UV.S = new Vector2(0.25f, 1.0f);
					sprite.UV.T = new Vector2(0.75f, 0.0f);
					break;

			}
			
			
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
		
		public bool CollidedWith(SpriteUV sprite)//BlueEnemy enemy, SpriteUV player)
		{
			//
			///define center point of players circle
			//
			float playerCenterX = this.sprite.Position.X;
			float playerCenterY = this.sprite.Position.Y;
			//
			///define center point of obstacles cirlce
			//
			float obstacleCenterX = sprite.Position.X;
			float obstacleCenterY = sprite.Position.Y;
	 		//
			///define radius of players circle
			//
			//for the 32x32 images
			float playerRadius = 15.0f;
			//
			///define radius of obstacles circle
			//
			float obstacleRadius = 16.0f;
			
			
			float distanceX = playerCenterX - obstacleCenterX;
			float distanceY = playerCenterY - obstacleCenterY;
			
			float distance = FMath.Sqrt((distanceX * distanceX) + (distanceY * distanceY));
			
			if(distance < (obstacleRadius + playerRadius))
			{
				//collision between circle bounds has occured
				return true;
			}
			return false;
		}
		
		public void Cleanup()
		{
			texInfo.Dispose();
		}
		
	}
}

