
using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;

using API.Framework;
using API;

namespace SpaceShips
{
	public class PlayerSelect : GameActor
	{
		private Font font40;
//		private TextSprite textSprite;
		private List<SpriteB> sprites;
		private int cursor = 1;
		private bool analogBlock = true;

		public PlayerSelect (Game gs, string name) : base (gs, name)
		{
			sprites = new List<SpriteB>();

			font40 = new Font("/Application/assets/fonts/Pixel-li.ttf", 40, FontStyle.Regular);
		}

		public override void Update ()
		{
			sprites.Clear();

			if (gs.Step == Game.StepType.SelectPlayer)
			{
				Status = Actor.ActorStatus.Action;

				updateShips();

				if (gs.playerInput.UpDownAxis() < 0.0f && analogBlock)
				{
					++cursor;
					analogBlock = false;
					gs.audioManager.playSound("systemSelect");
				}
				else if (gs.playerInput.UpDownAxis() > 0.0f && analogBlock)
				{
					--cursor;
					analogBlock = false;
					gs.audioManager.playSound("systemSelect");
				}
				if (gs.playerInput.UpDownAxis() == 0.0f)
					analogBlock = true;

				if (cursor == 4)
					cursor = 1;
				else if (cursor == 0)
					cursor = 3;
			}

			base.Update ();
		}

		public override void Render ()
		{
			if (gs.Step == Game.StepType.SelectPlayer)
			{
				renderSelect(cursor);
				renderBullet(cursor);

				foreach (var sprite in sprites)
				{
					gs.piSprite.Add(sprite);
				}
			}
			else
				Status = Actor.ActorStatus.UpdateOnly;

			base.Render ();
		}

		private void updateShips()
		{
			var sprite = new SpriteB(gs.dicTextureInfo["assets/image/player/ship1.png"]);
			sprite.Position = new Vector3(gs.rectScreen.Width/2+100,
			                               gs.rectScreen.Height-((gs.rectScreen.Height/3)/2),
			                              0.3f);
			sprite.Center = new Vector2(0.5f,0.5f);
			sprite.Scale = new Vector2(0.35f, 0.35f);
			sprite.Rotation = (180.0f+90)/180.0f*FMath.PI;
			sprites.Add(sprite);

			sprite = new SpriteB(gs.dicTextureInfo["assets/image/player/ship2.png"]);
			sprite.Position = new Vector3(gs.rectScreen.Width/2+100,
			                               gs.rectScreen.Height/2, 0.3f);
			sprite.Center = new Vector2(0.5f,0.5f);
			sprite.Scale = new Vector2(0.35f, 0.35f);
			sprite.Rotation = (180.0f+90)/180.0f*FMath.PI;
			sprites.Add(sprite);

			sprite = new SpriteB(gs.dicTextureInfo["assets/image/player/ship3.png"]);

			sprite.Position = new Vector3(gs.rectScreen.Width/2+100,
			                               (gs.rectScreen.Height/3)/2, 0.3f);
			sprite.Center = new Vector2(0.5f,0.5f);
			sprite.Scale = new Vector2(0.35f, 0.35f);
			sprite.Rotation = (180.0f+90)/180.0f*FMath.PI;
			sprites.Add(sprite);
		}

		public void renderSelect(int selected)
		{
			spriteB = new SpriteB(gs.dicTextureInfo["assets/image/player/ship"+selected+".png"]);

			spriteB.Position = new Vector3(gs.rectScreen.Width/2,
			                               gs.rectScreen.Height/2, 0.3f);
			spriteB.Center = new Vector2(0.5f,0.5f);
			spriteB.Scale = new Vector2(0.35f, 0.35f);
			spriteB.Rotation = (180.0f+90)/180.0f*FMath.PI;

			if (gs.appCounter % 10 == 0)
				sprites[selected-1].SetColor(1.0f, 1.0f, 1.0f, 0.0f);
			else if (gs.appCounter % 20 == 0)
				sprites[selected-1].SetColor(1.0f, 1.0f, 1.0f, 1.0f);
		}

		public void renderBullet(int selected)
		{
			Actor bulletManager=gs.Root.Search("bulletManager");

			if (gs.appCounter % 10 == 0)
			{
				if (selected == 1)
				{
					bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-5,
				        	                            this.spriteB.Position.Y-30, 0.4f)));
					bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-5,
				    	                                this.spriteB.Position.Y+30, 0.4f)));
					bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-5,
				            	                        this.spriteB.Position.Y, 0.4f)));
				}
				else if (selected == 2)
				{
					bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-20,
				        	                            this.spriteB.Position.Y-10, 0.4f), -8, -2));
					bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-20,
				    	                                this.spriteB.Position.Y+10, 0.4f), -8, 2));
					bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-20,
				            	                        this.spriteB.Position.Y, 0.4f)));
				}
				else
				{
					float incY = 0;
					if (gs.appCounter % 20 == 0)
						incY = 5;
					if (gs.appCounter % 40 == 0)
						incY = -5;
	
					bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-10,
				        	                            this.spriteB.Position.Y-21, 0.4f), -8, incY));
					bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-10,
				    	                                this.spriteB.Position.Y+19, 0.4f), -8, incY));
					bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-20,
				            	                        this.spriteB.Position.Y-1, 0.4f)));
				}
			}
		}

		public int SelectShip()
		{
			return cursor;
		}
	}
}