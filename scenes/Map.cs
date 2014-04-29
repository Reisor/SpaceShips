/**
 * \file Map.cs
 *
 * \brief Implements the map class.
 */
using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;

using API;

namespace SpaceShips
{
	public class Map : GameActor
	{
		public float speed;
		private int numOfmapTileX=0;
		private string backgroundPath = "assets/image/background/";

		private List<SpriteB> listSpriteB = new List<SpriteB>();

		/**
		 * \fn Map(Game gs, string name)
		 *
		 * \brief Default constructor.
		 *
		 * \param gs 		Variable that contains the game framework class.
 		 * \param name 		Name of the object.
 		 */
		public Map(Game gs, string name) : base(gs, name)
		{
			this.changeBackground("space.png");

			this.speed = 1.0f;
		}

		public void changeBackground(string name)
		{
			string mapTexture = backgroundPath + name;

			float tileMapWidth=gs.dicTextureInfo[mapTexture].w;
			float tileMapHeight=gs.dicTextureInfo[mapTexture].h;

			numOfmapTileX = (int)(gs.rectScreen.Width/tileMapWidth)+2;

			this.fillScreen(mapTexture, tileMapWidth, tileMapHeight);
		}

		public void fillScreen(string mapTexture, float tileMapWidth, float tileMapHeight)
		{
			for(float y=-tileMapHeight; y < gs.rectScreen.Height; y+=tileMapHeight)
			{
				for(float x=0; x < gs.rectScreen.Width+tileMapWidth*2; x=x+tileMapWidth)
				{
					SpriteB spriteTileMap = new SpriteB(gs.dicTextureInfo[mapTexture]);

					spriteTileMap.Position.X=x;
					spriteTileMap.Position.Y=y;

					spriteTileMap.Position.Z=0.9f;

					listSpriteB.Add(spriteTileMap);
				}
			}
		}

		/**
		 * \fn Update()
		 *
		 * \brief Override update method.
		 */
		public override void Update()
		{
			foreach(var sprite in listSpriteB)
			{
				sprite.Position.X += speed;
			
				// Return onto the screen if it gets out of the screen. 
				if (sprite.Position.X > gs.rectScreen.Width )
				{
					sprite.Position.X = sprite.Position.X-sprite.Width*numOfmapTileX;
				}
			}
			
			base.Update();
		}

		/**
		 * \fn Update()
		 *
		 * \brief Override render method.
		 */
		public override void Render()
		{
			foreach(var sprite in listSpriteB)
			{
				gs.piSprite.Add(sprite);
			}
			
			base.Render();
		}
	}
}
