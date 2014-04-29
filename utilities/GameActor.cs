/**
 * \file GameActor.cs
 *
 * \brief Implements the GameActor class.
 */

using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using API.Framework;
using API;

namespace SpaceShips
{
	public class GameActor : Actor
	{
		protected Game gs;
		
		public SpriteB spriteB; /**< SpriteB of the actor. */
		
		public Vector2 collision; /**< Vector of the collision area. */
		
		public List<ActionBase> actionList=new List<ActionBase>(); /**<  List of actions. */

		/**
		 * \fn GameActor(Game gs, string name)
		 *
		 * \brief Default constructor.
		 *
		 * \param gs		Variable that contains the game framework class.
 		 * \param name 		Name of the object.
		 */
		public GameActor(Game gs, string name) : base(name)
		{	
			this.gs = gs;
		}

		/**
		 * \fn GameActor(Game gs, string name)
		 *
		 * \brief Constructor for unified textures.
		 *
		 * \param gs					Variable that contains the game framework class.
 		 * \param name 					Name of the object.
 		 * \param UnifiedTextureInfo	Dictionary witht he information of the unfied textures.
		 */
		public GameActor(Game gs, string name, UnifiedTextureInfo textureInfo) : this(gs,name) 
		{	
			spriteB = new SpriteB(textureInfo);
		}

		/**
		 * \fn AddAction( ActionBase action)
		 *
		 * \brief Add the given action.
		 */
		public void AddAction( ActionBase action)
		{
			actionList.Add(action);	
		}

		/**
		 * \fn Update()
		 *
		 * \brief Overrided update method.
		 *
		 * For each action in the action list it update the action. \n
		 * Check if any action is done and remove it.
		 */
		public override void Update ()
		{
			foreach( ActionBase action in actionList)
			{
				action.Update();
			}

			actionList.RemoveAll(action=>action.Done==true);
			
			base.Update ();
		}

		/**
		 * \fn Render()
		 *
		 * \brief Overrided render method.
		 *
		 * Check if it status is "Action" and it spriteB is not null, in that case add the SpriteB to the
		 * root piSprite.
		 */
		public override void Render ()
		{
			if(spriteB!=null && this.Status == ActorStatus.Action)
			{
				gs.piSprite.Add(this.spriteB);
			}
			
			base.Render ();
		}
	}
}
