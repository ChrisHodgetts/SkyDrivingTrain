using System;

using Sce.PlayStation.Core;


using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace SkyDrivingTrain
{
	public class Gate
	{
		private SpriteUV sprite;
		private TextureInfo texInfo;
		
		public Gate()
		{
			texInfo = new TextureInfo("/Application/assets/gate.png");
			sprite = new SpriteUV(texInfo);			
			sprite.Quad.S = texInfo.TextureSizef;
			//sprite.Scale = new Vector2(0.25f, 0.25f);
			sprite.Position = new Vector2(100.0f, 100.0f);
			
			//scene.AddChild(sprite);
		}
		
		public SpriteUV Sprite
		{
			get{ return sprite; }
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

