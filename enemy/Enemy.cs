/**
 * \file Enemy.cs
 *
 * \brief Implements the Enemy class.
 */
using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;

using API.Framework;
using API;

namespace SpaceShips
{
	public class Enemy : GameActor
	{    
		static int idNum = 0;
		Random r;

		Int32 cnt = 0;

		int enemyLevel;
		float speed;

		Vector2 trans;
		Vector3 positionI;

		public int puntuation; /**< Puntuation score. */
		public int life; /**< Life value. */

		/**
		 * Enum class for enemy status
		 */
		public enum EnemyStatus
		{
			Normal, /**< Play status */
			Hit, /**< Hit status */
			Damage, /**< Damage status */
			Invincible, /**< Boss begining status */
		};

		/**
		 * Enum class for enemy movement
		 */
		public enum EnemyMovement
		{
			Nothing, /**< No movement. */
			Positioning, /**< Positioning movement. */
			Horizontal, /**< Horizontal movement. */
			Vertical, /**< Vertical movement. */
			Bounce, /**< Bounce movement. */
			Exit, /**< Retreat movement. */
			Stop, /**< Stop movement. */
		};

		public EnemyStatus enemyStatus; /**< Status of the enemy. */
		public EnemyMovement enemyMovement; /**< Movement of the enemy. */

		/*public Enemy(Game gs, string name, int level, Vector3 position,
		             int punt, float speed=0.15f, int life=1) : base (gs, name)
		{
			Name = name + idNum.ToString();
			
			if (level == 10){
				this.spriteB = new SpriteB(gs.dicTextureInfo["assets/image/boss.png"]);
				this.life = life;
			}
			
			this.spriteB.Center.X = 0.5f;
			this.spriteB.Center.Y = 0.5f;
			this.spriteB.Rotation = ((1*180.0f/360.0f)+90)/180.0f*FMath.PI;
			idNum++;
			
			this.spriteB.Position = position;
			this.collision.X = spriteB.Width/2;
			this.collision.Y = spriteB.Height/2;
			this.pos=position.Y;
			
			this.speed = speed;
			
			this.puntuation = punt;
		}*/

		/**
		 * \fn Enemy(Game gs, string name, int level, Vector3 position, int punt, float speed=0.15f, int life=1)
		 *
		 * Default constructor
		 *
		 * \param gs		Variable that contains the game framework class.
		 * \param file		File name of the texture.
 		 * \param name 		Name of the object.
 		 * \paraam level	Level of the enemy.
 		 * \param position	Starting position.
 		 * \param punt		Puntuation value.
 		 * \param speed		Movement speed.
 		 * \param life		Life amount
 		 */
		public Enemy(Game gs, string file, string name, int level, Vector3 position,
			int punt, float speed = 0.15f, int life = 1) : base(gs, name)
		{
			Name = name + idNum.ToString();
			idNum++;

			this.r = new Random(idNum);

			this.enemyStatus = EnemyStatus.Normal;
			this.enemyMovement = EnemyMovement.Positioning;

			this.life = life;
			this.enemyLevel = level;

			this.spriteB = new SpriteB(gs.dicTextureInfo[file]);
			this.spriteB.Center = new Vector2(0.5f, 0.5f);
			this.spriteB.Rotation = 90/180.0f*FMath.PI;
			this.spriteB.Position = new Vector3(-position.X, position.Y, position.Z);

			this.trans = new Vector2(1f, (position.Y > gs.rectScreen.Height/2) ? -1f : 1f);
			this.positionI = position;

			this.collision = new Vector2(spriteB.Height/6, spriteB.Width/2);

			this.speed = speed;
			
			this.puntuation = punt;
		}

		/**
		 * \fn Update()
		 *
		 * \brief Overrided Update method
		 *
		 * Handle the movement, shooting and kill the actor when is out of the screen.
		 */
		public override void Update()
		{
			UpdateStatus();

			UpdateMovement();

			// Dead if it gets out of screen.
			if((spriteB.Position.X > gs.rectScreen.Width + spriteB.Width ||
				spriteB.Position.Y > gs.rectScreen.Height + gs.rectScreen.Height/2 ||
				spriteB.Position.Y < 0 - gs.rectScreen.Height/2) && enemyMovement != EnemyMovement.Positioning)
			{
				this.Status = Actor.ActorStatus.Dead;
			}

			// Shoot bullets.
			Shoot();

			++cnt;
			
			base.Update();
		}


		private void UpdateStatus()
		{
			switch(enemyStatus)
			{
				case EnemyStatus.Hit:
					enemyStatus = EnemyStatus.Damage;
					cnt = 0;
					break;

				case EnemyStatus.Damage:
					if(++cnt > 20)
					{
						enemyStatus = EnemyStatus.Normal;
						spriteB.SetColor(Vector4.One);
						cnt = 0;
					}
					else // Flash in translucence.
					{
						spriteB.SetColor(1.0f, 1.0f, 1.0f, 0.25f + (gs.appCounter%2)/2.0f);
					}
					break;

				default:
					break;
			}
		}

		private void UpdateMovement()
		{
			switch(enemyMovement)
			{
				case EnemyMovement.Nothing:
					defaultMovement();
					break;

				case EnemyMovement.Positioning:
					if(Vector3.DistanceSquared(positionI, spriteB.Position) > 2*speed)
					{
						if((spriteB.Position.X >= positionI.X + speed) || (spriteB.Position.X <= positionI.X - speed))
							spriteB.Position.X += speed*(spriteB.Position.X > positionI.X ? -1 : 1);
						if((spriteB.Position.Y >= positionI.Y + speed) || (spriteB.Position.Y <= positionI.Y - speed))
							spriteB.Position.Y += speed*(spriteB.Position.Y > positionI.Y ? -1 : 1);
					}
					else enemyMovement = EnemyMovement.Nothing;
					break;

				case EnemyMovement.Horizontal:
					UpdatePositionX();
					if(cnt > 500) enemyMovement = EnemyMovement.Vertical;
					break;

				case EnemyMovement.Vertical:
					UpdatePositionY();
					if(cnt > 500 && enemyLevel < 9) enemyMovement = EnemyMovement.Horizontal;
					break;

				case EnemyMovement.Bounce:
					UpdatePositionX();
					UpdatePositionY();
					break;

				case EnemyMovement.Exit:
					spriteB.Position.X += speed;
					break;

				case EnemyMovement.Stop:
					break;

				default:
					break;
			}

			if(enemyLevel <= 2 && cnt > enemyLevel*1000)
			{
				cnt = 0;
				speed = 2*speed;
				enemyMovement = EnemyMovement.Exit;
			}
		}

		public void defaultMovement()
		{
			switch(enemyLevel)
			{
				case 1:
				case 2:
					enemyMovement = EnemyMovement.Bounce;
					break;
				case 3:
					enemyMovement = EnemyMovement.Vertical;
					break;
				case 4:
				case 5:
					enemyMovement = EnemyMovement.Horizontal;
					break;
				case 6:
					enemyMovement = EnemyMovement.Nothing;
					break;
				case 9:
					enemyMovement = EnemyMovement.Vertical;
					break;
				default:
					enemyMovement = EnemyMovement.Stop;
					break;
			}
		}

		private void UpdatePositionX()
		{
			spriteB.Position.X += trans.X*speed;
			if(spriteB.Position.X < spriteB.Width/2)
			{
				trans.X = +1.0f;
				//spriteB.Rotation = (FMath.PI-spriteB.Rotation)*2 + spriteB.Rotation;
			}
			if((5*gs.rectScreen.Width/8 - spriteB.Width/2) < spriteB.Position.X)
			{
				trans.X = -1.0f;
				//spriteB.Rotation = (FMath.PI-spriteB.Rotation)*2 + spriteB.Rotation;
			}
		}

		private void UpdatePositionY()
		{
			spriteB.Position.Y += trans.Y*speed;
			if(spriteB.Position.Y < spriteB.Height/2)
			{
				trans.Y = +1.0f;
				//spriteB.Rotation = (FMath.PI/2.0f-spriteB.Rotation)*2 + spriteB.Rotation;
			}
			if((gs.rectScreen.Height - spriteB.Height/2) < spriteB.Position.Y)
			{
				trans.Y = -1.0f;
				//spriteB.Rotation = (FMath.PI/2.0f-spriteB.Rotation)*2 + spriteB.Rotation;
			}
		}

		private void Shoot()
		{
			if (enemyLevel < 9)
			{
				if (cnt % 30 == 0)
				{
					if(enemyLevel >= 5 && r.NextDouble() > 0.7f) circularShot((enemyLevel%2 == 0) ? 2 : 1);
					if(enemyLevel >= 3 && r.NextDouble() > 0.9f) directShot((enemyLevel%2 == 0) ? 2 : 1);
					if(enemyLevel >= 1 && r.NextDouble() > 0.9f) horizontalShot((enemyLevel%2 == 0) ? 2 : 1);
				}
			}
			else
			{
				if (cnt % 10 == 0)
				{
					circularShot((enemyLevel%2 == 0) ? 2 : 1);
					if(enemyLevel >= 3 && r.NextDouble() > 0.5f) directShot((enemyLevel%2 == 0) ? 2 : 1, 10.0f);
					//if(enemyLevel >= 1 && r.NextDouble() > 0.9f) horizontalShot((enemyLevel%2 == 0) ? 2 : 1);
				}
			}
		}

		private void horizontalShot(int lev)
		{
			gs.Root.Search("enemyBulletManager").AddChild(
					new EnemyBullet(gs, "enemyBullet", lev, this.spriteB.Position, 4.0f,
					new Vector2(1, 0).Normalize()));
		}

		private void directShot(int lev, float speed = 4.0f)
		{
			GameActor player = (GameActor) gs.Root.Search("Player");

			gs.Root.Search("enemyBulletManager").AddChild(
					new EnemyBullet(gs, "enemyBullet", lev, this.spriteB.Position, speed,
					new Vector2(
							player.spriteB.Position.X - this.spriteB.Position.X,
					        player.spriteB.Position.Y - this.spriteB.Position.Y)
							.Normalize()));
		}

		private void circularShot(int lev)
		{
			for(int i = 0; i < 90; i+=15)
			{
				gs.Root.Search("enemyBulletManager").AddChild(
						new EnemyBullet(gs, "enemyBullet", lev, this.spriteB.Position, 4.0f,
						new Vector2(
								(float) Math.Cos(i * 2 * Math.PI / 360.0),
					        	(float) Math.Sin(i * 2 * Math.PI / 360.0))
								.Normalize()));
				gs.Root.Search("enemyBulletManager").AddChild(
						new EnemyBullet(gs, "enemyBullet", lev, this.spriteB.Position, 4.0f,
						new Vector2(
								(float) Math.Cos(i * 2 * Math.PI / 360.0),
					        	(float) - Math.Sin(i * 2 * Math.PI / 360.0))
								.Normalize()));
			}
		}
	}
}