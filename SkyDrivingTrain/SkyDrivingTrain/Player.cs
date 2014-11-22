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
		private int speed;
		
		private AppMain.Direction direction;
		
		public Player(int speed)
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
		
		public int Speed
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
		
		public void Cleanup()
		{
			texInfo.Dispose();
		}
		
	}
}

