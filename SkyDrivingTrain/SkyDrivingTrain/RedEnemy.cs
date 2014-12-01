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
		private Bounds2 bounds;
		private Vector2 center;
		public RedEnemy (int speed)
		{
			texInfo = new TextureInfo("/Application/assets/enemy_R.png");
			sprite = new SpriteUV(texInfo);			
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = new Vector2(10.0f, AppMain.screenHeight * 0.5f);
			bounds = new Bounds2();
			sprite.GetlContentLocalBounds(ref bounds);
			//center = node.LocalToWorld(bounds.Center);
			this.speed = speed;
			
			//scene.AddChild(sprite);
		}
		
		public SpriteUV Sprite
		{
			get{ return sprite; }
		}
		
		public Bounds2 Bounds
		{
			get { return bounds; }
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

