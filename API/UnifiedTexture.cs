/**
 * \file UnifiedTexture.cs
 *
 * \brief Texture parser.
 */
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace API
{

	public class UnifiedTextureInfo
	{
		public float u0;/**< u0 coordinate. 0.0f~1.0f */
		public float v0;/**< v0 coordinate. 0.0f~1.0f */
		public float u1;/**< u1 coordinate. 0.0f~1.0f */
		public float v1;/**< v1 coordinate. 0.0f~1.0f */
		
		public float w;/**< Texture width. */
		
		public float h;/**< Texture height. */
		
		/**
		 * \fn UnifiedTextureInfo(float u0, float v0, float u1, float v1, float w, float h)
		 * 
		 * \brief Default constructor.
		 * 
		 * \param u0	u0 coordinate.
		 * \param v0	v0 coordinate.
		 * \param u1	u1 coordinate.
		 * \param v1	v1 coordinate.
		 * \param w		Width.
		 * \param h		Height.
		 */
		public UnifiedTextureInfo(float u0, float v0, float u1, float v1, float w, float h)
		{
			this.u0=u0;
			this.v0=v0;
			this.u1=u1;
			this.v1=v1;
			this.w=w;
			this.h=h;
		}		
		
		/**
		 * \fn UnifiedTextureInfo GetTileTextureInfo(Int32 indexX, Int32 indexY, Int32 numOfX, Int32 numOfY)
		 * 
		 * \brief Return the information of the tile.
		 * 
		 * Return the info of the texture tile specified in the arguments.
		 * 
		 * \param indexX	Index X of the tile.
		 * \param indexY	Index Y of the tile.
		 * \param numOfX	Number X of the tile.
		 * \param numOfY	Number Y of the tile.
		 */
		public UnifiedTextureInfo GetTileTextureInfo(Int32 indexX, Int32 indexY, Int32 numOfX, Int32 numOfY)
		{
			
			if(indexX < 0 || indexY < 0)
			{
				throw new Exception("Index of argument is invalid.  Index must be greater than or equal to 0 . -  GetTileTextureInfo()");
			}
			
			if(numOfX <= 0 || numOfY <=0 )
			{
				throw new Exception("numOfXY of argument is invalid. numOfXY must be greater than 0. -  GetTileTextureInfo()");
			}
			
			if( numOfX <= indexX ||  numOfY <= indexY)
			{
				throw new Exception("Index of argument  is out of numOfXY.  -  GetTileTextureInfo()");
			}

			
			float tileW,tileH;
			tileW=this.w/numOfX;
			tileH=this.h/numOfY;
			
			float tileU0, tileV0, tileU1, tileV1;
				
			float tileUW = (this.u1-this.u0)/numOfX;
			float tileVH = (this.v1-this.v0)/numOfY;
			
			tileU0 = this.u0 + tileUW*indexX;
			tileV0 = this.v0 + tileVH*indexY;

			tileU1 = tileU0 + tileUW;
			tileV1 = tileV0 + tileVH;
			
			return new UnifiedTextureInfo(tileU0, tileV0, tileU1, tileV1, tileW, tileH);
		}
	}
	
	public class UnifiedTexture
	{
		/**
		 * \fn Dictionary<string, UnifiedTextureInfo>　GetDictionaryTextureInfo(string xmlFilename)
		 * 
		 * \brief Return a dictonary with the texture info.
		 * 
		 * Receive a xml and return a dictonary with that information.
		 * 
		 * \param xmlFilename	Name of the xml.
		 */
		static public Dictionary<string, UnifiedTextureInfo>　GetDictionaryTextureInfo(string xmlFilename)
		{
			Dictionary<string, UnifiedTextureInfo> dicTextureInfo = new Dictionary<string, UnifiedTextureInfo>();
			
			try
			{
				XDocument doc = XDocument.Load(xmlFilename);
				
				
				XElement root= doc.Element("root");
				
				// Get the size of the whole texture.
				var elementInfo = root.Element("info");
				float textureWidth=float.Parse(elementInfo.Attribute("w").Value);
				float textureHeight=float.Parse(elementInfo.Attribute("h").Value);
#if DEBUG
				Console.WriteLine(textureWidth+","+textureHeight);
#endif		
				float x,y,w,h;
				
				foreach( var element in root.Descendants("texture"))
				{
#if DEBUG
					Console.WriteLine("texture "+element.Attribute("filename").Value);
#endif
					x=float.Parse(element.Attribute("x").Value);
					y=float.Parse(element.Attribute("y").Value);
					w=float.Parse(element.Attribute("w").Value);
					h=float.Parse(element.Attribute("h").Value);

					// Convert to UV.
					dicTextureInfo.Add(element.Attribute("filename").Value, 
					    new UnifiedTextureInfo(
							x/textureWidth,	y/textureHeight,
							(x+w)/textureWidth,	(y+h)/textureHeight,
							w,	h
						)
					 );
				}
			}
			catch (Exception e)
			{
			    Console.Error.WriteLine(e.Message);
			    Environment.Exit(1);
			}
			
			return dicTextureInfo;
		}
	}

}

