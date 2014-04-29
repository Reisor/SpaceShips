
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
	public class Opening : GameActor
	{
		private string frasermPhrase, joalborPhrase;
		private int frasermCharPos = 0, joalborCharPos = 0;
		private float _alpha = 0;

		public DebugString frasermInfo, joalborInfo;
		public Dictionary<string, SimpleSprite> gameManagerSprites;

		public Opening(Game gs, string name) : base (gs, name)
		{
			SimpleSprite sprite;

			gameManagerSprites = new Dictionary<string, SimpleSprite>();

			sprite = new SimpleSprite(gs.Graphics, gs.textureIntro, 1538, 0, 1884, 314,
			                          gs.dicIntroInfo["assets/image/intro/logoupv.png"].w,
			                          gs.dicIntroInfo["assets/image/intro/logoupv.png"].h,
			                          90.0f, new Vector2(gs.rectScreen.Width/2-314/2,
			                   gs.rectScreen.Height/2+gs.textureMenu.Width/2));
			sprite.SetColor(1.0f,1.0f,1.0f,0.0f);
			gameManagerSprites.Add("logoupv", sprite);

			sprite = new SimpleSprite(gs.Graphics, gs.textureIntro, 1538, 315, 1674, 370,
			                          gs.dicIntroInfo["assets/image/intro/logodisca.png"].w,
			                          gs.dicIntroInfo["assets/image/intro/logodisca.png"].h,
			                          90.0f, new Vector2(gs.rectScreen.Width-75,204));
			sprite.SetColor(1.0f,1.0f,1.0f,0.0f);
			gameManagerSprites.Add("logodisca", sprite);

			sprite = new SimpleSprite(gs.Graphics, gs.textureIntro, 1538, 371, 1656, 430,
			                          gs.dicIntroInfo["assets/image/intro/logodsic.png"].w,
			                          gs.dicIntroInfo["assets/image/intro/logodsic.png"].h,
			                          90.0f, new Vector2(gs.rectScreen.Width-79,gs.rectScreen.Height-59));
			sprite.SetColor(1.0f,1.0f,1.0f,0.0f);
			gameManagerSprites.Add("logodsic", sprite);

			sprite = new SimpleSprite(gs.Graphics, gs.textureIntro, 769, 0, 1537, 768,
			                          gs.dicIntroInfo["assets/image/intro/fraserm.jpg"].w,
			                          gs.dicIntroInfo["assets/image/intro/fraserm.jpg"].h,
			                          90.0f, new Vector2(gs.rectScreen.Width/2-288, 514));
			sprite.SetColor(1.0f,1.0f,1.0f,0.0f);
			sprite.Scale = new Vector2(0.25f,0.25f);
			gameManagerSprites.Add("fraserm", sprite);

			frasermPhrase = String.Format("Francisco Elias Serrano\nMartinez-Santos\n" +
				"Director, artist and\ngame developer");

			frasermInfo = new DebugString(gs.Graphics);
			frasermInfo.Rotate90 = true;
			frasermInfo.SetColor(new Vector4(1.0f, 1.0f, 1.0f, 0.0f));
			frasermInfo.SetPosition(new Vector3(sprite.Position.X+sprite.Width/2-20,
			                                       sprite.Position.Y-sprite.Height-20,
			                                       0));



			sprite = new SimpleSprite(gs.Graphics, gs.textureIntro, 0, 0, 768, 768,
			                          gs.dicIntroInfo["assets/image/intro/joalbor.jpg"].w,
			                          gs.dicIntroInfo["assets/image/intro/joalbor.jpg"].h,
			                          90.0f, new Vector2(gs.rectScreen.Width/2+96,514));
			sprite.SetColor(1.0f,1.0f,1.0f,0.0f);
			sprite.Scale = new Vector2(0.25f,0.25f);
			gameManagerSprites.Add("joalbor", sprite);

			joalborPhrase = String.Format("Jose Alemany Bordera\nGame developer");

			joalborInfo = new DebugString(gs.Graphics);
			joalborInfo.Rotate90 = true;
			joalborInfo.SetColor(new Vector4(1.0f, 1.0f, 1.0f, 0.0f));
			joalborInfo.SetPosition(new Vector3(sprite.Position.X+sprite.Width/2-20,
			                                       sprite.Position.Y-sprite.Height-20,
			                                       0));
		}

		public override void Update ()
		{
			if(gs.Step == Game.StepType.Opening)
			{
				if ((gs.PadData.ButtonsDown & GamePadButtons.Start) != 0 || gs.appCounter > 1100)
				{
					gs.Step = Game.StepType.Title;
				}
				if (gs.appCounter < 300)
				{
					if (gs.appCounter < 100)
					{
						_alpha += 0.01f;
						gameManagerSprites["logoupv"].SetColor(1.0f,1.0f,1.0f,_alpha);
						gameManagerSprites["logodisca"].SetColor(1.0f,1.0f,1.0f,_alpha);
						gameManagerSprites["logodsic"].SetColor(1.0f,1.0f,1.0f,_alpha);
					}
					else if (gs.appCounter > 201)
					{
						_alpha -= 0.01f;
						gameManagerSprites["logoupv"].SetColor(1.0f,1.0f,1.0f,_alpha);
						gameManagerSprites["logodisca"].SetColor(1.0f,1.0f,1.0f,_alpha);
						gameManagerSprites["logodsic"].SetColor(1.0f,1.0f,1.0f,_alpha);
					}
				}
				else
				{
					if (gs.appCounter < 400)
					{
						_alpha += 0.01f;
						gameManagerSprites["fraserm"].SetColor(1.0f,1.0f,1.0f,_alpha);
						gameManagerSprites["joalbor"].SetColor(1.0f,1.0f,1.0f,_alpha);
						frasermInfo.SetColor(new Vector4(1.0f,1.0f,1.0f,_alpha));
						joalborInfo.SetColor(new Vector4(1.0f,1.0f,1.0f,_alpha));
					}
					else if (gs.appCounter > 400 && gs.appCounter % 5 == 0)
					{
						if (frasermCharPos < frasermPhrase.Length)
						{
							frasermInfo.Write(frasermPhrase[frasermCharPos].ToString());
							frasermCharPos += 1;
						}
						if (joalborCharPos < joalborPhrase.Length)
						{
							joalborInfo.Write(joalborPhrase[joalborCharPos].ToString());
							joalborCharPos += 1;
						}
					}
					else if (gs.appCounter > 1000)
					{
						_alpha -= 0.01f;
						gameManagerSprites["fraserm"].SetColor(1.0f,1.0f,1.0f,_alpha);
						gameManagerSprites["joalbor"].SetColor(1.0f,1.0f,1.0f,_alpha);
						frasermInfo.SetColor(new Vector4(1.0f,1.0f,1.0f,_alpha));
						joalborInfo.SetColor(new Vector4(1.0f,1.0f,1.0f,_alpha));
					}
				}
			}
			
			base.Update ();
		}
	}
}

