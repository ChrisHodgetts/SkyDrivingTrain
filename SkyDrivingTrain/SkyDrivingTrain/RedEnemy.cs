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
		private int speed;
		
		public RedEnemy (int speed)
		{
			texInfo = new TextureInfo("/Application/assets/enemy_R.png");
			sprite = new SpriteUV(texInfo);			
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = new Vector2(10.0f, AppMain.screenHeight * 0.5f);
			//center = node.LocalToWorld(bounds.Center);
			this.speed = speed;
			
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
		
		public void Update()
		{
			
			
		}
		
		public void Cleanup()
		{
			texInfo.Dispose();
		}
		
	}
}

