/**
 * \file AppMain.cs
 *
 * \brief Implements the AppMain class.
 */

using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;

using API.Framework;

namespace SpaceShips
{
	public class AppMain
	{
		/**
		 * \fn Main(string[] args)
		 *
		 * \brief Call the run method of the class Game.
		 */
	    public static void Main(string[] args)
	    {
			using( Game game = new Game())
			{
				game.Run(args);
			}
	    }
	}
}
