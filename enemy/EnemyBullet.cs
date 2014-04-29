/**
 * \file EnemyBullet.cs
 *
 * \brief Implements the EnemyBullet class.
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
	public class EnemyBullet : GameActor
	{
		static int idNum=0;

		int bulletLevel;
		
		float speed=6;
		
		Vector2 trans;
		Vector3 direction;
		
		static float depth=0.0f;

		/**
		 * \fn EnemyBullet(Game gs, string name, int level, Vector3 position, float speed, float direction)
		 *
		 * \brief Default constructor
		 *
		 * Load the specific level bullet, and set the level, position, speed and direction to the bullet.
		 *
		 * \param gs		Variable that contains the game framework class.
 		 * \param name 		Name of the object.
 		 * \param level		Level of the bullet.
 		 * \param position	Starting position.
 		 * \param speed		Movement speed.
 		 * \param direction	Position of the player.
 		 */
		public EnemyBullet(Game gs, string name, int level, Vector3 position, float speed, Vector2 direction) : base (gs, name)
		{
			Name = name + idNum.ToString();
			idNum++;

			if (level == 1) spriteB = new SpriteB(gs.dicTextureInfo["assets/image/enemy/bullet_red.png"]);
			else if (level == 2)
			{
				this.spriteB = new SpriteB(gs.dicTextureInfo["assets/image/enemy/bullet_green.png"]);
				this.spriteB.Scale = new Vector2(0.5f, 0.5f);
			}
			this.spriteB.Position = position;
			this.spriteB.Center = new Vector2(0.5f, 0.5f);
			this.spriteB.Rotation = -direction.Angle(new Vector2(0.0f, -1.0f));

			this.bulletLevel = level;
			this.direction = new Vector3(direction.X*speed, direction.Y*speed, 0.0f);
		}

		/**
		 * \fn EnemyBullet(Game gs, string name, int level, Vector3 position, float speed, float direction)
		 *
		 * \brief Constructor for boss bullets.
		 *
		 * Load the specific level bullet, and set the level, position, speed and direction to the bullet.
		 *
		 * \param gs		Variable that contains the game framework class.
 		 * \param name 		Name of the object.
 		 * \param level		Level of the bullet.
 		 * \param position	Starting position.
 		 * \param speed		Movement speed.
 		 * \param direction	Direction of the bullet
 		 */
		public EnemyBullet(Game gs, string name, int level, Vector3 position, float speed, float direction) :
			base (gs, name)
		{
			Name = name + idNum.ToString();
			if (level == 1)
				spriteB = new SpriteB(gs.dicTextureInfo["assets/image/enemy/bullet_test.png"]);
			else if (level == 2)
				spriteB = new SpriteB(gs.dicTextureInfo["assets/image/enemy/bullet_green.png"]);

			this.bulletLevel = level;
			
			this.spriteB.Center.X = 0.5f;
			this.spriteB.Center.Y = 0.5f;

			this.spriteB.Scale=new Vector2(1.0f, 1.0f);
			
			this.speed = speed;

			//this.spriteB.Rotation = direction;//(direction+90)/180*FMath.PI;
			trans.Y= FMath.Cos(direction/180*FMath.PI);
			trans.X= FMath.Sin(direction/180*FMath.PI);
		
			this.spriteB.Position = position;
			this.spriteB.Position.Z += depth;
			
			depth+=0.0001f;
			if(depth>0.1f)
				depth=0.0f;
			
			++idNum;
		}

		/**
		 * \fn Update()
		 *
		 * \brief Overrided update method.
		 *
		 * Check the level of the bullet to update the position, it has differents method for normal and boss
		 * bullets. \n
		 * Check if it is out of the screen.
		 */
		public override void Update()
		{

			spriteB.Position += direction;
			
			// Kill if it gets out of screen.
			if (spriteB.Position.X < 0 -spriteB.Width ||
			    spriteB.Position.Y < 0 -spriteB.Height||
			    gs.rectScreen.Width + spriteB.Width < spriteB.Position.X ||
				gs.rectScreen.Height + spriteB.Height < spriteB.Position.Y  )
			{
				this.Status = Actor.ActorStatus.Dead;
			}
			
			base.Update();
		}
	}
}


