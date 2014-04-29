/**
 * \file LevelParser.cs
 *
 * \brief Implements the level parser.
 */

using System;

using Sce.PlayStation.Core;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SpaceShips
{
	public class LevelParser
	{
		public int mapWidth, mapHeight, tileWidth, tileHeight;

		/**
		 * \fn LevelParser
		 *
		 * \brief Default constructor.
		 *
		 * Empty constructor.
		 */
		public LevelParser ()
		{}

		/**
		 * \fn LoadLevel (Game gs, string xmlFilename)
		 *
		 * \brief Load the level and add all the objects to the game.
		 *
		 * Search only for all the objectgroups in the xml.
		 *
		 * \param gs			Variable that contains the game framework class.
		 * \param xmlFilename	Name of the level xml.
		 */
		public void LoadLevel (Game gs, string xmlFilename)
		{
			try
			{
				XDocument doc = XDocument.Load(xmlFilename);

				XElement root = doc.Element("map");

				mapWidth = int.Parse(root.Attribute("width").Value);
				mapHeight = int.Parse(root.Attribute("height").Value);
				tileWidth = int.Parse(root.Attribute("tilewidth").Value);
				tileHeight = int.Parse(root.Attribute("tileheight").Value);

				foreach( var objectGroup in root.Descendants("objectgroup"))
				{
					int level = 1, life = 1, puntuation =1;
					float speed = 0.15f;
					Vector3 position;
					string file = "", name = "";
	
					foreach( var element in objectGroup.Descendants("object"))
					{
						name = element.Attribute("name").Value;
						position.X = float.Parse(element.Attribute("x").Value);
						position.Y = float.Parse(element.Attribute("y").Value);
						position.Z = 0.2f;

						foreach( var propierties in element.Descendants("properties"))
						{
							foreach( var propierty in element.Descendants("property"))
							{
								if (propierty.Attribute("name").Value == "File")
									file = propierty.Attribute("value").Value;
								else if (propierty.Attribute("name").Value == "Level")
									level = int.Parse(propierty.Attribute("value").Value);
								else if (propierty.Attribute("name").Value == "Life")
									life = int.Parse(propierty.Attribute("value").Value);
								else if (propierty.Attribute("name").Value == "Puntuation")
									puntuation = int.Parse(propierty.Attribute("value").Value);
								else if (propierty.Attribute("name").Value == "Speed")
									speed = float.Parse(propierty.Attribute("value").Value);

							}
							gs.Root.Search("enemyManager").AddChild(new Enemy(gs, file, name, level, position,
							                                                  puntuation, speed, life));
						}
					}
				}
			}
			catch (Exception e)
			{
			    Console.Error.WriteLine(e.Message);
			    Environment.Exit(1);
			}
		}
	}
}

