/**
 * \file Player.cs
 *
 * \brief Implements the player class.
 */

using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using API.Framework;
using API;

namespace SpaceShips
{
	public class Player : GameActor
	{
		UInt32 cnt=0;
		Int16 shootTime=0;
		int speed = 3;

		/**
		 * Enum class for player status
		 */
		public enum PlayerStatus
		{
			Normal, /**< Play status */
			Explosion, /**< Hit status */
			Invincible, /**< Respawn status */
			GameOver, /**< End status */
		};

		/**
		 * \brief Player status.
		 */
		public PlayerStatus playerStatus;

		public int ship;

		/**
		 * \fn Player(Game gs, string name)
		 *
		 * \brief Default constructor.
		 *
		 * Set the status to NoUse and load the default ship image.
		 *
		 * \param gs 		Variable that contains the game framework class.
 		 * \param name 		Name of the object.
 		 */
		public Player(Game gs, string name) : base(gs, name)
		{
			this.Status = Actor.ActorStatus.NoUse;
//			this.spriteB = new SpriteB(gs.dicTextureInfo["assets/image/player/"+ship+".png"]);
		}

		/**
		 * \fn Initialize()
		 *
		 * \brief Player initialize.
		 *
		 * Put player tu normal status and at the middle right screen. Also put the counter to 0.
		 */
		public void Initialize(int ship)
		{
			this.spriteB = new SpriteB(gs.dicTextureInfo["assets/image/player/ship"+ship+".png"]);

			this.ship = ship;

			this.playerStatus = PlayerStatus.Normal;
			this.Status = Actor.ActorStatus.Action;

			this.spriteB.Center.X = 0.5f;
			this.spriteB.Center.Y = 0.5f;
			this.spriteB.Rotation = (-90)/180.0f*FMath.PI;

			this.spriteB.Scale = new Vector2(0.35f, 0.35f);

			this.collision.X = 6;
			this.collision.Y = 6;

			spriteB.Position.X=gs.rectScreen.Width*3/4;
			spriteB.Position.Y=gs.rectScreen.Height*1/2;
			spriteB.Position.Z=0.1f;
			cnt=0;
		}

		/**
		 * \fn Update()
		 *
		 * \brief Override update method.
		 *
		 * Handle the input and change the state if it is hit.
		 * If the game is in debug mode it will print the position of the player.
		 */
		public override void Update ()
		{
#if DEBUG
			gs.debugString.WriteLine(string.Format("Position=({0},{1})\n", spriteB.Position.X, spriteB.Position.Y));
#endif

			var gamePadData = GamePad.GetData(0);

			if(this.playerStatus == PlayerStatus.Normal || this.playerStatus == PlayerStatus.Invincible)
			{
				if(this.playerStatus == PlayerStatus.Invincible && ++cnt > 120)
				{
					this.playerStatus= PlayerStatus.Normal;
					this.spriteB.SetColor(Vector4.One);
					cnt=0;
				}

				if (gs.playerInput.LeftRightAxis() != 0.0f)
				{
					spriteB.Position.X += gs.playerInput.LeftRightAxis() * speed;

					if (spriteB.Position.X < spriteB.Width/2.0f)
						spriteB.Position.X=spriteB.Width/2.0f;
					else if (spriteB.Position.X> gs.rectScreen.Width - spriteB.Width/2.0f)
						spriteB.Position.X=gs.rectScreen.Width - spriteB.Width/2.0f;
				}

				if (gs.playerInput.UpDownAxis() != 0.0f)
				{
					spriteB.Position.Y += gs.playerInput.UpDownAxis() * speed;

					if (spriteB.Position.Y < spriteB.Height/2.0f)
						spriteB.Position.Y =spriteB.Height/2.0f;
					else if (spriteB.Position.Y > gs.rectScreen.Height - spriteB.Height/2.0f)
						spriteB.Position.Y=gs.rectScreen.Height - spriteB.Height/2.0f;
				}

				// Shoot bullets.
				if (gs.playerInput.AttackButton() && (this.playerStatus == PlayerStatus.Normal ||
				   this.playerStatus == PlayerStatus.Invincible))
				{
					++shootTime;
					Actor bulletManager=gs.Root.Search("bulletManager");

					if (shootTime < 5)
					{
						gs.audioManager.playSound("laser");
						if (ship == 1)
						{
							bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-5,
						        	                            this.spriteB.Position.Y-30, 0.4f)));
							bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-5,
						    	                                this.spriteB.Position.Y+30, 0.4f)));
							bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-5,
						            	                        this.spriteB.Position.Y, 0.4f)));
						}
						else if (ship == 2)
						{
							bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-20,
						        	                            this.spriteB.Position.Y-10, 0.4f), -8, -2));
							bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-20,
						    	                                this.spriteB.Position.Y+10, 0.4f), -8, 2));
							bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-20,
						            	                        this.spriteB.Position.Y, 0.4f)));
						}
						else
						{
							float incY = 0;
							if (gs.playerInput.UpDownAxis() < 0)
								incY = 5*gs.playerInput.UpDownAxis();
							else if (gs.playerInput.UpDownAxis() > 0)
								incY = 5*gs.playerInput.UpDownAxis();

							bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-10,
						        	                            this.spriteB.Position.Y-21, 0.4f), -8, incY));
							bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-10,
						    	                                this.spriteB.Position.Y+19, 0.4f), -8, incY));
							bulletManager.AddChild(	new Bullet(gs, "Bullet",new Vector3(this.spriteB.Position.X-20,
						            	                        this.spriteB.Position.Y-1, 0.4f)));
						}
					}
				}
				else
				{
					shootTime=0;
				}
			}
			else if(this.playerStatus == PlayerStatus.Explosion)
			{
				this.Status = Actor.ActorStatus.UpdateOnly;
				
				if( ++cnt > 60)
				{
					if( gs.NumShips > 0)
					{
						this.spriteB.Position.X=gs.rectScreen.Width*3/4;
						this.spriteB.Position.Y=gs.rectScreen.Height*1/2;
						this.Status = Actor.ActorStatus.Action;
						this.playerStatus = PlayerStatus.Invincible;
						cnt=0;
					}
					else
					{
						this.playerStatus = PlayerStatus.GameOver;
						this.Status = Actor.ActorStatus.UpdateOnly;
					}
				}
			}
			
			if(this.playerStatus == PlayerStatus.Invincible)
			{
				// Flash in translucence.
				if( gs.appCounter % 2 == 0)
					this.spriteB.SetColor(1.0f, 1.0f, 1.0f, 0.75f);
				else
					this.spriteB.SetColor(1.0f, 1.0f, 1.0f, 0.2f);
			}
			
			base.Update();
		}
	}
}