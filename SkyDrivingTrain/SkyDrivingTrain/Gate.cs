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
		private Vector2 Position;
		
		public Gate (Vector2 position)
		{
			texInfo = new TextureInfo("/Application/assets/gate.png");
			sprite = new SpriteUV(texInfo);			
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Scale = new Vector2(0.2f, 0.2f);
			this.Position = position;
			
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

