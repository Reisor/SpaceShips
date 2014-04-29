
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
	public class GameUI : GameActor
	{
		private UInt16 cnt=0;
		private int countdown=0;
		private Font font20, font40;
		private TextSprite textSprite;
		private List<SpriteB> ship;

		public GameUI(Game gs, string name) : base (gs, name)
		{
			ship = new List<SpriteB>();

			font20 = new Font("/Application/assets/fonts/Pixel-li.ttf", 20, FontStyle.Regular);
			font40 = new Font("/Application/assets/fonts/Pixel-li.ttf", 40, FontStyle.Regular);
		}

		public override void Update ()
		{
			ship.Clear();

			if (gs.Step == Game.StepType.Gameplay)
			{
				UpdateScore();
				UpdateShips();
			}
			else if (gs.Step == Game.StepType.GameOver)
			{
				UpdateCountdown();
			}

			base.Update ();
		}

		public override void Render ()
		{
			if (gs.Step == Game.StepType.Gameplay)
			{
				foreach (var sprite in ship)
				{
					gs.piSprite.Add(sprite);
				}
			}

			base.Render ();
		}

		private void UpdateScore ()
		{
			var texture = Text.createTexture("HIGH", font20, 0xffffff00);
			textSprite = new TextSprite(texture, 1.0f, gs.rectScreen.Height/2-texture.Width/2, 0.5f, 0.5f,
			                            -90.0f, 1.0f, 1.0f);
			gs.textList.Add(textSprite);

			texture = Text.createTexture(string.Format("{0,8}", gs.BestScore),
			                                 font20, 0xffffff00);
			textSprite = new TextSprite(texture, 1.0f+texture.Height, textSprite.PositionY+texture.Width/2,
			                            0.5f, 0.5f, -90.0f, 1.0f, 1.0f);
			gs.textList.Add(textSprite);

			texture = Text.createTexture("PLAYER 1", font20, 0xffffffff);
			textSprite = new TextSprite(texture, 1.0f, gs.rectScreen.Height-5, 0.5f, 0.5f,
			                            -90.0f, 1.0f, 1.0f);
			gs.textList.Add(textSprite);

			texture = Text.createTexture(string.Format("{0,8}", gs.Score),
			                                 font20, 0xffffffff);
			textSprite = new TextSprite(texture, 1.0f+texture.Height, textSprite.PositionY,
			                            0.5f, 0.5f, -90.0f, 1.0f, 1.0f);
			gs.textList.Add(textSprite);

			texture.Dispose();
//			textSprite.Dispose();
		}

		private void UpdateShips()
		{
			Player player = (Player)gs.Root.Search("Player");

			for (int i = 0; i < gs.NumShips; i++)
			{
				var sprite = new SpriteB(gs.dicTextureInfo["assets/image/player/ship"+player.ship+".png"]);

				sprite.Position = new Vector3(60.0f,gs.rectScreen.Height-(sprite.Width*0.15f) -
				                              (sprite.Width*0.15f)*i, 0.3f);
				sprite.Center = new Vector2(0.5f,0.5f);
				sprite.Scale = new Vector2(0.15f,0.15f);
				sprite.Rotation = player.spriteB.Rotation;

				ship.Add(sprite);
			}
		}

		private void UpdateCountdown()
		{
			if (++cnt % 60 == 0)
				countdown += 1;

			var texture = Text.createTexture((9-countdown).ToString() , font40, 0xffff0000);
			var textSprite = new TextSprite(texture, gs.rectScreen.Width/2+40-texture.Height/2
			                            , gs.rectScreen.Height/2-texture.Height/2);
			textSprite.Degree = -90;
			gs.textList.Add(textSprite);

			texture.Dispose();
			//textSprite.Dispose();
		}

		public void ResetCountdown()
		{
			cnt = 0;
			countdown=0;
		}

		public override void Terminate ()
		{
			font20.Dispose();
			font40.Dispose();
			textSprite.Dispose();

			base.Terminate ();
		}
	}
}