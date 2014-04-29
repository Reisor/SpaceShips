/**
 * \file  Explosion.cs
 *
 * \brief Implements the explosion class.
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
	public class Explosion : GameActor
	{
		private int maxAnim;
		private int anim = 1;

		static int idNum=0;
		float speed=1.0f;
		int cnt=0;

		SpriteB[] spritesB;
		
		/**
 		 * \fn  Explosion(Game gs, string name, Vector3 position)
 		 *
 		 * \brief Default constructor.
 		 *
 		 * \author  Reisor
 		 * \date  12/03/2014
 		 * 
 		 * \param gs 		Variable that contains the game framework class.
 		 * \param name 		Name of the object.
 		 * \param position	Start position of the object.
 	 	 */
		public Explosion(Game gs, string name, string explosion, string ext,
		                 int anim, Vector3 position) : base (gs, name)
		{
			Name = name + idNum.ToString();

			spritesB = new SpriteB[anim+1];

			for (int i=1; i< anim+1; i++)
			{
				this.spritesB[i] = new SpriteB(gs.dicTextureInfo["assets/image/particle/explosions/"+explosion+i+"."+ext]);
				this.spritesB[i].Center = new Vector2(0.5f, 0.5f);
				this.spritesB[i].Position = position;
				idNum++;
			}

			this.maxAnim = anim;
		}
		
		/**
 		 * \fn  Update()
 		 *
 		 * \brief Overrided update method.
 		 *
 		 * \author  Reisor
 		 * \date  12/03/2014
 		 * 
 	 	 */
		public override void Update()
		{
			if(++cnt >=maxAnim*2)
			{
				this.Status = Actor.ActorStatus.Dead;
			}

			if (cnt % 2 == 0 && anim < maxAnim)
			{
				++anim;
			}

			for (int i = 1; i < maxAnim+1; i++)
			{
				this.spritesB[i].Position.X += speed;
//				this.spritesB[i].SetColor(new Vector4(1.0f, 1.0f, 1.0f, (maxAnim*2-cnt)/maxAnim*2));
			}


//			this.spritesB[anim].Scale.X += 0.02f;
//			this.spritesB[anim].Scale.Y += 0.02f;

			base.Update();
		}

		public override void Render ()
		{
			gs.piSprite.Add(spritesB[anim]);

			base.Render ();
		}
	}
}

