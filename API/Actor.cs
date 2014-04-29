/**
 * \file  Actor.cs
 *
 * \brief Implements the actor class.
 */
using System;
using System.Collections.Generic;


namespace API.Framework
{
	public class Actor
	{
		public string Name { get; set; }

		/**
		 * Enum class for actor status
		 */
		public enum ActorStatus
		{
			Action, //*< Actor will behave normally. */
			UpdateOnly, //*< Actor will only be updated. */
			RenderOnly, //*< Actor will only be render. */
			Rest, //*< Freezed status. */
			NoUse, //*< Actor is updated and render but it is not used. */
			Dead, //*< Actor will be cleaned. */
		}
		
		ActorStatus status;

		/**
		 * \fn Status
		 *
		 * \brief Get and set method for ActorStatus.
		 *
		 * Return the status and set the status and StatusNextFrame.
 		 */
		public ActorStatus Status
		{
			get { return status; }
			set
			{
				status = value;
				StatusNextFrame = value;
			}
		}

		public ActorStatus StatusNextFrame;

		protected Int32 level=0;
		
		protected List<Actor> children = new List<Actor>();

		/**
		 * \fn Children
		 *
		 * \brief Get method of Actors.
		 *
		 * Return the list of Actors.
 		 */
		public List<Actor> Children
		{
			get { return children;}	
		}

		/**
		 * \fn Actor()
		 *
		 * \brief Default constructor.
		 *
		 * Set the name to "no_name" and status to "Action".
 		 */
		public Actor()
		{
			this.Name = "no_name";
			this.Status = ActorStatus.Action;
		}
		
		/**
		 * \fn Actor(string name)
		 *
		 * \brief Constructor that specify the name.
		 *
		 * \param name	Name of the actor.
		 *
		 * Set the status to "Action".
 		 */
		public Actor(string name)
		{
			this.Name = name;
			this.Status = ActorStatus.Action;
		}

		/**
		 * \fn ToString()
		 *
		 * \brief Override the method ToString().
		 *
		 * Return the name of the actor.
		 */
		public override string ToString ()
		{
			return this.Name;
		}
		
		/**
		 * \fn Update()
		 *
		 * \brief Virtual update method.
		 *
		 * Update all actors, only when the status is in action or update only, in the children list.
		 */
		virtual public void Update()
		{ 
			foreach( Actor actorChild in children)
			{
				if(actorChild.Status == ActorStatus.Action || actorChild.Status == ActorStatus.UpdateOnly)
					actorChild.Update();	
			}
		}

		/**
		 * \fn Render()
		 *
		 * \brief Virtual render method.
		 *
		 * Render all the actors in the children list.
		 */
		virtual public void Render() 
		{ 
			foreach( Actor actorChild in children)
			{
				actorChild.Render();	
			}		}

		virtual public void Terminate()
		{
		}

		/**
		 * \fn AddChild(Actor actor)
		 *
		 * \brief Virtual addchild method.
		 *
		 * Add a specific actor to the children list.
		 *
		 * \param actor	New actor.
		 */
		virtual public void AddChild(Actor actor)
		{
			children.Add(actor);
			actor.level = this.level+1;
		}
		
		/**
		 * \fn Search(string name)
		 *
		 * \brief Virtual search method.
		 *
		 * Search for the first actor in children list that have the
		 * same name.
		 *
		 * \param name	Name to search.
		 */
		virtual public Actor Search(string name)
		{
			if( this.Name == name)
				return this;

			Actor retActor;
			
			foreach (Actor actorChild in children)
			{
				if ((retActor = actorChild.GetActor(name)) != null)
					return retActor;
			}
			
			return null;
		}

		/**
		 * \fn GetActor(string name)
		 *
		 * \brief Non virtual search method.
		 *
		 * Search for the first actor in children list that have the
		 * same name.
		 *
		 * \param name	Name to search.
		 */
		public Actor GetActor(string name)
		{
			if( this.Name == name)
				return this;

			Actor retActor;

			foreach (Actor actorChild in children)
			{
				if ((retActor = actorChild.GetActor(name)) != null)
					return retActor;
			}

			return null;
		}

		/**
		 * \fn SearchByPath(string path)
		 *
		 * \brief Search an actor by path.
		 *
		 * Search for an actor in a specific path. The path must be
		 * separated by "/".
		 * P.e: Player/Hero1
		 *
		 * \param path	Path of the actor.
		 */
		public Actor SearchByPath(string path)
		{
			int pos=path.IndexOf('/');
			if(pos!=-1)
			{
				string nameLeft = path.Substring( 0, pos );
				string nameRight = path.Substring( pos+1 );
				
				foreach( Actor actorChild in this.Children)
				{
					if(actorChild.Name == nameLeft)
						return actorChild.SearchByPath(nameRight);
				}
			}
			else
			{
				foreach( Actor actorChild in this.Children)
				{
					if(actorChild.Name == path)
						return actorChild;
				}
			}
			
			return null;
		}
		
		/**
		 * \fn CheckStatus()
		 *
		 * \brief Delete childers if player is dead.
		 *
		 * If the player is dead all the actors in childern list are deleted.
		 */
		virtual public void CheckStatus()
		{
			if (this.status != this.StatusNextFrame)
				this.status = this.StatusNextFrame;
			
			
			// Set dead flags for all the children if the player is dead.
			if( this.Status == ActorStatus.Dead)
			{
				foreach(Actor actorChild in children)
				{
					actorChild.Status = ActorStatus.Dead;
				}
			}
			
			// Visit children with recursive call.
			foreach(Actor actorChild in children)
			{
				actorChild.CheckStatus();
			}
			
			// Delete a child where the dead flag is set from a list.
			children.RemoveAll(CheckDeadActor);
		}

		/**
		 * \fn CheckDeadActor(Actor actor)
		 *
		 * \brief Check is actor is dead
		 *
		 * Return true if the actor is dead, and false it is not.
		 *
		 * \param actor	Player actor.
		 */
		static bool CheckDeadActor(Actor actor)
		{
			// Delete the elements to be proper with this condition.
			return actor.Status == ActorStatus.Dead; 
		}
		
		/**
		 * \fn GetAliveChildren()
		 *
		 * \brief Return the number of childrens.
		 *
		 * Return the length of the children list.
		 */
		public Int32 GetAliveChildren()
		{
			Int32 cnt = 0;

			foreach (Actor actorChild in children)
			{
				if (actorChild.Status != ActorStatus.Dead)
				{
					cnt++;
					cnt += actorChild.GetAliveChildren();
				}
			}

			return cnt;
		}
	}
}
