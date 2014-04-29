/**
 * \file CollisionCheck.cs
 *
 * \brief Implements the CollisionCheck class.
 */

using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;
using API.Framework;

namespace SpaceShips
{
	public class CollisionCheck : GameActor
	{
		/**
		 * \fn CollisionCheck(Game gs, string name)
		 *
		 * \brief Default constructor
		 *
		 * Initialize the GameActor.
		 */
		public CollisionCheck(Game gs, string name) : base (gs, name){}

		/**
		 * \fn Update()
		 *
		 * \brief Overrided update method.
		 *
		 * Check if the player bullet has hit an enemy. If it is so then it check if the enemy is still alive then
		 * reduce the life and set it status to "hit" and the bullet status to "dead", and if it is dead, set the
		 * status of the enemy and the player to "dead", add a explosion effect, increase the score and play a
		 * sound. \n
		 * Check if a enemy or a enemy bullet has hit the player and set it status to "explosion", add an explosion
		 * effect and play a sound.
		 */
		public override void Update()
		{
			Player player =(Player)gs.Root.Search("Player");
			Actor bulletManager=gs.Root.Search("bulletManager");
			Actor enemyManager =gs.Root.Search("enemyManager");      
			Actor bulletEnemyManager=gs.Root.Search("enemyBulletManager");
			Actor effectManager =gs.Root.Search("effectManager");
			
			// Collision detection for bullets and enemies.
			foreach( Bullet bullet in  bulletManager.Children)
			{
				if(bullet.Status == Actor.ActorStatus.Action)
				{
					foreach( Enemy enemy in  enemyManager.Children)
					{
						if(enemy.Status ==  Actor.ActorStatus.Action &&  
						   Math.Abs(bullet.spriteB.Position.X -enemy.spriteB.Position.X ) < enemy.collision.X &&
							Math.Abs(bullet.spriteB.Position.Y -enemy.spriteB.Position.Y ) < enemy.collision.Y
								)
						{
							--enemy.life;
							
							if (enemy.life <= 0)
							{
							bullet.Status = Actor.ActorStatus.Dead;
							enemy.Status = Actor.ActorStatus.Dead;              
							effectManager.AddChild(new Explosion(gs, "explosion", "explosion1-", "gif", 16,
							    new Vector3(enemy.spriteB.Position.X, enemy.spriteB.Position.Y, 0.3f)));
							
							gs.Score += enemy.puntuation;
							gs.audioManager.playSound("explosion2");
							}
							else
							{
								enemy.enemyStatus = Enemy.EnemyStatus.Hit;
								bullet.Status = Actor.ActorStatus.Dead;
							}
						}
					}
				}
			}
			
			if(player.playerStatus== Player.PlayerStatus.Normal)
			{
				// Collision detection of player and enemy
				foreach( Enemy enemy in  enemyManager.Children)
				{
					if(enemy.Status ==  Actor.ActorStatus.Action &&  
					   Math.Abs(player.spriteB.Position.X -enemy.spriteB.Position.X ) < enemy.collision.X &&
						Math.Abs(player.spriteB.Position.Y -enemy.spriteB.Position.Y ) < enemy.collision.Y
							)
					{
						
						effectManager.AddChild(new Explosion(gs, "explosion", "explosion1-", "gif", 16,
						    new Vector3(player.spriteB.Position.X, player.spriteB.Position.Y, 0.3f)));
						
						player.playerStatus = Player.PlayerStatus.Explosion;	
						
						gs.audioManager.playSound("explosion");
						
						--gs.NumShips;

						--enemy.life;

						if (enemy.life <= 0)
						{
							enemy.Status = Actor.ActorStatus.Dead;
						}
						else
						{
							enemy.enemyStatus = Enemy.EnemyStatus.Hit;
						}
					}
				}
				
				
				// Collision detection for players and enemys bullet
				foreach( EnemyBullet enemyBullet in  bulletEnemyManager.Children)
				{
					if(enemyBullet.Status ==  Actor.ActorStatus.Action &&  
					   Math.Abs(player.spriteB.Position.X -enemyBullet.spriteB.Position.X ) < player.collision.X && 
						Math.Abs(player.spriteB.Position.Y -enemyBullet.spriteB.Position.Y ) < player.collision.Y
							)
					{
						enemyBullet.Status = Actor.ActorStatus.Dead;
						effectManager.AddChild(new Explosion(gs, "explosion", "explosion1-", "gif", 16,
						    new Vector3(player.spriteB.Position.X, player.spriteB.Position.Y, 0.3f)));
						
						player.playerStatus = Player.PlayerStatus.Explosion;	
						gs.audioManager.playSound("explosion");
						gs.NumShips--;
					}
				}
			}
			
			base.Update();
		}
	}
}