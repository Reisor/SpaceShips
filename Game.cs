/**
 * \file Game.cs
 *
 * \brief Implements the Game class.
 */

using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Audio;

using API.Framework;
using API;

using System.Xml.Linq;
using System.IO;

namespace SpaceShips
{
	public class Game: GameFramework
	{
		public ImageRect rectScreen; /**< Screen information. */
		
		public Random rand = new Random(); /**< Variable to make random numbers. */
		
		public UInt32 appCounter = 0; /** App counter. */

		/**
		 * \fn Root
		 *
		 * \brief Implements the set and get method using the ones of Actor class. */
		public Actor Root{ get; set;}
		
		bool m_pause = false;
		int forwardFrame = 0;
		
		public Texture2D textureUnified, textureMenu, textureIntro;/**< Textures. */
		
		public Dictionary<string, UnifiedTextureInfo> dicTextureInfo; /**< Dictionary with the info of the
		unified textures. */
		public Dictionary<string, UnifiedTextureInfo> dicIntroInfo; /**< Dictionary with the info of the intro unified textures. */

		public PackedIndexedSprite piSprite; /**< Packed index sprites. */
		
		public const int sizeofSprite=1024; /**< Maximum sprite size. */
		
		public AudioManager audioManager;

		public LevelParser levelParser = new LevelParser(); /**< LevelParser class. */
		
		public Int32 GameCounter = 0; /**< Game counter. */
		public Int32 Score = 0; /**< Score counter. */
		public Int32 BestScore = 0; /**< High Score. */
		public Int32 NumShips = 0; /**< Number of lifes. */

		public GameManager gameManager;
		public Opening opening;
		public GameUI gameUI;
		public PlayerInput playerInput;

		public List<TextSprite> textList;

		/**
		 * Enum class for machine states.
		 */
		public enum StepType
		{
			Opening, /**< Opening state. */
			Title, /**< Title state */
			StartMenu, /**< Game modes state */
			SelectPlayer, /**< Select player mode state */
			Gameplay, /**< Gameplay state. */
			Pause,
			GameOver, /**< Gameover state. */
		}
		
		public StepType Step; /**< Step type. */

		/**
		 * \fn Initialize()
		 *
		 * \brief Initialize all public variables, managers and actors.
		 */
		public override void Initialize()
		{
			base.Initialize();
			
			rectScreen = graphics.GetViewport();

			//spriteBuffer=new SpriteBuffer(graphics, sizeofSprite);
			piSprite = new PackedIndexedSprite(graphics);
			
			// Process for unified texture.
			dicTextureInfo = UnifiedTexture.GetDictionaryTextureInfo("/Application/assets/image/game.xml");
			textureUnified=new Texture2D("/Application/assets/image/game.png", false);

			dicIntroInfo = UnifiedTexture.GetDictionaryTextureInfo("/Application/assets/image/intro/intro.xml");
			textureIntro=new Texture2D("/Application/assets/image/intro/intro.png", false);

			textureMenu = new Texture2D("/Application/assets/image/menu/Menu.png", false);
      
			// Initialization of actor tree
			Root = new Actor("root");

			Root.AddChild(new PlayerInput(this, "playerInput"));
			Root.AddChild(new GameManager(this, "gameManager"));
			Root.AddChild(new Opening(this, "opening"));
			Root.AddChild(new Map(this,"Map"));
			Root.AddChild(new PlayerSelect(this, "playerSelect"));
			Root.AddChild(new Player(this, "Player"));
			Root.AddChild(new Actor("bulletManager"));
			Root.AddChild(new Actor("enemyManager"));
			Root.AddChild(new Actor("enemyBulletManager"));
			Root.AddChild(new Actor("effectManager"));
			Root.AddChild(new GameUI(this, "gameUI"));
			Root.AddChild(new EnemyCommander(this, "enemyCommander"));
			Root.AddChild(new CollisionCheck(this, "collisionCheck"));

			audioManager = new AudioManager();

			gameManager = (GameManager)Root.Search("gameManager");
			opening = (Opening)Root.Search("opening");
			gameUI = (GameUI)Root.Search("gameUI");
			playerInput = (PlayerInput)Root.Search("playerInput");

			textList = new List<TextSprite>();

			BestScore = FileManager.ReadFromBinaryFile("/Documents/app.bin");

			Text.Init(graphics);
		}
		
		
		public override void Update()
		{
			BestScore = Math.Max(BestScore, Score);
			Text.Update();

			textList.Clear();

			base.Update();

#if DEBUG
			debugString.WriteLine("counter "+appCounter);
			debugString.WriteLine("Score="+Score);
			debugString.WriteLine("Buttons="+PadData.Buttons);

			// Pause mode by press start button.
			if((this.PadData.ButtonsDown & (GamePadButtons.Start)) != 0 && Step == StepType.Gameplay)
			{
				//m_pause = m_pause ? false : true;
			}
#endif
			if(m_pause==true && (this.PadData.ButtonsDown & (GamePadButtons.Select)) != 0)
			{
				forwardFrame =1;
			}

			if( forwardFrame > 0 || m_pause == false)
			{
				Root.Update();

				++appCounter;

				if( forwardFrame > 0)
					forwardFrame--;
			}
			else
			{
#if DEBUG
				debugString.WriteLine("Pause");
#endif
			}

		}
		
		
		public override void Render()
		{
			graphics.SetClearColor(0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear();

			// Add-transparent.
			graphics.Disable(EnableMode.DepthTest);
			
			graphics.SetTexture(0, this.textureUnified);
			
			//spriteBuffer.Clear();
			piSprite.Clear();
			
			Root.Render();

			piSprite.Update();
			piSprite.Render();

			if (textList.Capacity != 0)
			{
				foreach (var singleText in textList)
				{
					Text.DrawSprite(singleText);
				}
			}

			switch(Step)
			{
			case Game.StepType.Opening:
				if (appCounter < 300)
				{
					opening.gameManagerSprites["logoupv"].Render();
					opening.gameManagerSprites["logodisca"].Render();
					opening.gameManagerSprites["logodsic"].Render();
				}
				else
				{
					opening.gameManagerSprites["fraserm"].Render();
					opening.gameManagerSprites["joalbor"].Render();
					opening.frasermInfo.Render();
					opening.joalborInfo.Render();
				}
				break;
			case Game.StepType.Title:
				gameManager.gameManagerSprites["title"].Render();
				gameManager.gameManagerSprites["pressStart"].Render();
				break;
			case Game.StepType.StartMenu:
				gameManager.gameManagerSprites["title"].Render();
				gameManager.gameManagerSprites["arcade"].Render();
				gameManager.gameManagerSprites["survival"].Render();
				break;
			case Game.StepType.SelectPlayer:
				break;
			case Game.StepType.Gameplay:
				break;
			case Game.StepType.Pause:
				break;
			case Game.StepType.GameOver:
				gameManager.gameManagerSprites["gameover"].Render();
				break;
			default:
				throw new Exception("default in StepType.");			
			}
#if DEBUG
			debugString.WriteLine("Sprite Cnt="+piSprite.GetNumOfSprite());
#endif


			if (textList.Capacity != 0)
			{
				textList.Clear();
			}

			base.Render();
			
			Root.CheckStatus();
		}
		
		public override void Terminate ()
		{
			FileManager.WriteToBinaryFile("/Documents/app.bin", BestScore);

			textureUnified.Dispose();
			textureIntro.Dispose();
			textureMenu.Dispose();

			foreach (var singleText in textList)
			{
				singleText.Dispose();
			}
			
			audioManager.Terminate();

			Root.Terminate();
			
			base.Terminate ();
		}
	}
}
