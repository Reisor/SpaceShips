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
	public class EnemyCommander : GameActor
	{
	    public bool existUfo = false;
		private Random r;
		private int wave;
		private Font font80;
		private Int32 time;
		//private string[] enemyTypes = new string[]{"spectreb","tornadob","valiant","hurricaneu","flanker","firestormg","boss","boss2"};
		private string[] enemyTypes = new string[]{"spectreb","2","3","4","5","6","7","8","9","11"};
	    
		public EnemyCommander(Game gs, string name) : base (gs, name)
		{
			this.Status = Actor.ActorStatus.NoUse;
			this.r = new Random((int) gs.appCounter);
			this.wave = 1;
			this.time = 150;
			this.font80 = new Font("/Application/assets/fonts/Pixel-li.ttf", 80, FontStyle.Regular);
		}
	    	
		public void Initialize()
		{
			this.Status = Actor.ActorStatus.Action;
		}

		public void Clean()
		{
			this.Children.Clear();
		}
		
		public override void Update()
		{
			// Shows the current wave.
			if(gs.Root.Search("enemyManager").GetAliveChildren() == 0 && time < 100)
			{
				gs.Root.Search("enemyBulletManager").Children.Clear();
				Map map = (Map)gs.Root.Search("Map");
				map.speed = 4.0f;
				printNewWave();
			}
			// Make enemies appear.
			else if(gs.Root.Search("enemyManager").GetAliveChildren() == 0 && time == 100)
			{
				Map map = (Map)gs.Root.Search("Map");
				map.speed = 1.0f;
				newWave();
			}
			// Not enemies.
			else if(gs.Root.Search("enemyManager").GetAliveChildren() == 0 && time > 100)
			{
				time = 0;
			}

			time++;
			base.Update();
		}

		private void newWave()
		{
			if (wave % 10 != 0)
			{
				for(int i = 0; i < wave*5; i++)
				{
					//Enemys Level 1
					if(r.NextDouble() > 0.30f)
					{
						gs.Root.Search("enemyManager").AddChild(
								new Enemy(gs, "assets/image/enemy/"+this.enemyTypes[0]+".png",
						    	"enemy", 1,
						        new Vector3((float) r.Next(20, gs.rectScreen.Width) ,(float) r.Next(20, gs.rectScreen.Height-20),0.2f),
						        100, r.Next(90,140)/50.0f, 3));
					}
					//Enemys Level 2
					if(wave > 2 && r.NextDouble() > 0.80f)
					{
						gs.Root.Search("enemyManager").AddChild(
								new Enemy(gs, "assets/image/enemy/"+this.enemyTypes[1]+".png",
						    	"enemy", 2,
						        new Vector3((float) r.Next(20, 2*gs.rectScreen.Width/4) ,(float) r.Next(20, gs.rectScreen.Height-20),0.2f),
						        150, r.Next(60,120)/100.0f, 5));
					}
					//Enemys Level 3
					if(wave > 4 && r.NextDouble() > 0.95f)
					{
						gs.Root.Search("enemyManager").AddChild(
								new Enemy(gs, "assets/image/enemy/"+this.enemyTypes[2]+".png",
						    	"enemy", 3,
						        new Vector3((float) r.Next(20, 2*gs.rectScreen.Width/4) ,(float) r.Next(20, gs.rectScreen.Height-20),0.2f),
						        200, r.Next(60,100)/100.0f, 7));
					}
					//Enemys Level 4
					if(wave > 6 && r.NextDouble() > 0.96f)
					{
						gs.Root.Search("enemyManager").AddChild(
								new Enemy(gs, "assets/image/enemy/"+this.enemyTypes[3]+".png",
						    	"enemy", 4,
						        new Vector3((float) r.Next(20, 2*gs.rectScreen.Width/4) ,(float) r.Next(20, gs.rectScreen.Height-20),0.2f),
						        250, r.Next(60,100)/100.0f, 9));
					}
					//Enemys Level 5
					if(wave > 8 && r.NextDouble() > 0.97f)
					{
						gs.Root.Search("enemyManager").AddChild(
								new Enemy(gs, "assets/image/enemy/"+this.enemyTypes[4]+".png",
						    	"enemy", 5,
						        new Vector3((float) r.Next(20, 2*gs.rectScreen.Width/4) ,(float) r.Next(20, gs.rectScreen.Height-20),0.2f),
						        300, r.Next(60,100)/100.0f, 13));
					}
					//Enemys Level 6
					if(wave%10 == 2 && r.NextDouble() > 0.98f)
					{
						gs.Root.Search("enemyManager").AddChild(
								new Enemy(gs, "assets/image/enemy/"+this.enemyTypes[5]+".png",
						    	"enemy", 6,
						        new Vector3((float) r.Next(20, 2*gs.rectScreen.Width/4) ,(float) r.Next(20, gs.rectScreen.Height-20),0.2f),
						        400, r.Next(60,100)/100.0f, 20));
					}
					//Enemys Level 7
					if(wave%10 == 3 && r.NextDouble() > 0.99f)
					{
						gs.Root.Search("enemyManager").AddChild(
								new Enemy(gs, "assets/image/enemy/"+this.enemyTypes[6]+".png",
						    	"enemy", 7,
						        new Vector3((float) r.Next(20, 2*gs.rectScreen.Width/4) ,(float) r.Next(20, gs.rectScreen.Height-20),0.2f),
						        500, r.Next(60,100)/100.0f, 20));
					}
					//Enemys Level 8
					if(wave%10 == 5 && r.NextDouble() > 0.99f)
					{
						gs.Root.Search("enemyManager").AddChild(
								new Enemy(gs, "assets/image/enemy/"+this.enemyTypes[7]+".png",
						    	"enemy", 8,
						        new Vector3((float) r.Next(20, 2*gs.rectScreen.Width/4) ,(float) r.Next(20, gs.rectScreen.Height-20),0.2f),
						        600, r.Next(60,100)/100.0f, 30));
					}
				}
			}
			else
			{
				gs.Root.Search("enemyManager").AddChild(
							new Enemy(gs, "assets/image/enemy/"+this.enemyTypes[r.Next(8,9)]+".png",
					    	"enemy", 9,
					        new Vector3(50, gs.rectScreen.Height/2,0.2f),
					        10000, r.Next(60,100)/100.0f, 100));
			}

			this.wave++;
		}

		public void resetWave()
		{
			wave = 1;
		}

		private void printNewWave()
		{
			var texture = Text.createTexture("_", font80, 0xffffff00);

			var textureTitle = Text.createTexture("WAVE "+ this.wave, font80, 0xffffff00);

			gs.textList.Add(new TextSprite(texture, gs.rectScreen.Width/2 - 3*texture.Width,
			                            gs.rectScreen.Height - texture.Height/2 - time*gs.rectScreen.Height/100.0f, -90.0f, 1.0f));
			gs.textList.Add(new TextSprite(textureTitle, gs.rectScreen.Width/2 - textureTitle.Width/2,
			                            gs.rectScreen.Height/2-textureTitle.Height/2,-90.0f, 1.0f));
			gs.textList.Add(new TextSprite(texture, gs.rectScreen.Width/2 + texture.Width,
			                            time*gs.rectScreen.Height/100.0f, -90.0f, 1.0f));

		}
	}
}
