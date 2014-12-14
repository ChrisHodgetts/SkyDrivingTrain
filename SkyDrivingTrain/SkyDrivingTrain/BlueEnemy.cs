using System;

using Sce.PlayStation.Core;


using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace SkyDrivingTrain
{
	public class BlueEnemy
	{
		private SpriteUV sprite;
		private TextureInfo texInfo;
		private int speed;
		
		private bool perimeterBroken;
		private bool randomMove;
		private bool follow;
		private bool alive;
		
		
		public BlueEnemy(int speed)
		{
			texInfo = new TextureInfo("/Application/assets/enemy_B.png");
			sprite = new SpriteUV(texInfo);			
			sprite.Quad.S = texInfo.TextureSizef;
			sprite.Position = new Vector2(200.0f, 400.0f);

			this.speed = speed;
			this.randomMove = true;
			this.follow = false;
			this.perimeterBroken = false;
			this.alive = true;

			
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
		
		public bool RandomMove
		{
			get{ return randomMove; }
			set{ randomMove = value; }	
		}
		
		public bool Follow
		{
			get{ return follow; }
			set{ follow = value; }	
		}
		
		public bool Alive
		{
			get{ return alive;}
			set{ alive = value;}
		}
		
		public bool PerimeterBroken
		{
			get{ return perimeterBroken; }
			set{ perimeterBroken = value; }	
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

