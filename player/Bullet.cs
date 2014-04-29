/**
 * \file Bullet.cs
 *
 * \brief Implements the Bullet class.
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
	public class Bullet : GameActor
	{
		static int idNum=0;
		private float speedX, speedY;

		/**
		 * \fn Bullet(Game gs, string name, Vector3 position)
		 *
		 * \brief Default constructor.
		 *
		 * Load the bullet texture and set it at the given position.
		 *
		 * \param gs		Variable that contains the game framework class.
		 * \param position	Starting position.
		 */
		public Bullet(Game gs, string name, Vector3 position,
		              float speedX=-8, float speedY=0) : base (gs, name)
		{
			Name = name + idNum.ToString();
			spriteB = new SpriteB(gs.dicTextureInfo["assets/image/player/mybullet.png"]);
			
			spriteB.Center.X = 0.5f;
			spriteB.Center.Y = 0.5f;
			this.spriteB.Rotation = ((360*180.0f/360.0f)+90)/180.0f*FMath.PI;

			this.speedX = speedX;
			this.speedY = speedY;
			
			idNum++;
			
			spriteB.Position = position;
		}

		/**
		 * \fn Update()
		 *
		 * \brief Overrided update method
		 *
		 * Add the speed to the X axis of the bullet and check if it is out of the screen.
		 */
		public override void Update()
		{
			spriteB.Position.X += speedX;
			spriteB.Position.Y += speedY;
			
			if (spriteB.Position.X < 0 || spriteB.Position.Y < 0 ||
			    spriteB.Position.Y > gs.rectScreen.Height)
			{
				this.Status = Actor.ActorStatus.Dead;
			}
			
			base.Update();
		}
	}
}

