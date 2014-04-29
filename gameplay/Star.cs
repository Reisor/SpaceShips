/**
 * \file  Star.cs
 *
 * \brief Implements the star class.
 */

using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;

using API;

namespace SpaceShips
{
	public class Star : GameActor
	{
		float speed;

		/**
 		 * \fn  Star(Game gs, string name, Vector3 position, Vector4 color, float speed)
 		 *
 		 * \brief Default constructor.
 		 *
 		 * \param gs 		Variable that contains the game framework class.
 		 * \param name 		Name of the object.
 		 * \param position	Start position of the object.
 		 * \param color		Color of the star.
 		 * \param speed		Movement speed.
 	 	 */
		public Star(Game gs, string name, Vector3 position, Vector4 color, float speed) : base(gs, name)
		{
			this.spriteB = new SpriteB(gs.dicTextureInfo["assets/image/particle/Star.png"]);
			this.spriteB.Position = position;
			this.spriteB.SetColor(color);
			this.speed = speed;
		}

		/**
 		 * \fn  Update()
 		 *
 		 * \brief Overrided update method.
 		 *
 		 * Change the position of the star incresing the Y position by the value of speed.
 		 * It also compare if the star is out of the screen, in that case it is put in the
 		 * top of the screen again.
 	 	 */
		public override void Update()
		{
			spriteB.Position.Y += speed;
			
			// Return to the top of the screen if it gets out of the screen.
			if (spriteB.Position.Y > gs.rectScreen.Height )
			{
				spriteB.Position.Y = 0.0f;
				spriteB.Position.X = (int)(gs.rectScreen.Width * gs.rand.NextDouble());
				spriteB.Position.Z = 0.2f;
			}
			
			base.Update();
		}
	}
}
