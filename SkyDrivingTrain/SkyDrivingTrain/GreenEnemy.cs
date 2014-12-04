using System;

using Sce.PlayStation.Core;


using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace SkyDrivingTrain
{
	public class GreenEnemy
	{
		private SpriteUV sprite;
		private TextureInfo texInfo;
		private int speed;
		private int wallCollisionCount;
		private AppMain.Direction direction;
		

		public GreenEnemy (int speed)
		{
			texInfo = new TextureInfo("/Application/assets/enemy_G.png");
			sprite = new SpriteUV(texInfo);			
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = new Vector2(500.0f, 50.0f);
			//center = node.LocalToWorld(bounds.Center);
			this.speed = speed;
			this.direction = AppMain.Direction.Right;
			this.wallCollisionCount = 0;
			
			//scene.AddChild(sprite);
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
		
		public AppMain.Direction Direction
		{
			get{ return direction; }
			set{ direction = value; }	
		}
		
		public int WallCollisionCount
		{
			get{ return wallCollisionCount; }
			set{ wallCollisionCount = value; }
		}
		
		public void Update()
		{
			
			
		}
		
		public void Cleanup()
		{
			texInfo.Dispose();
		}
		
	}
}

