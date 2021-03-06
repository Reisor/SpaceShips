
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Input;

namespace API
{
	public static class Text
	{
	    private static GraphicsContext graphics;
	
	    private static ShaderProgram textureShaderProgram;
	    private static ShaderProgram colorShaderProgram;

	    private static Matrix4 projectionMatrix;
	    private static Matrix4 viewMatrix;
	
	    private static VertexBuffer rectVertices;
	    private static VertexBuffer circleVertices;
	
	    private static Dictionary<string, TextSprite> spriteDict;
	    private static int spriteNamelessCount;

	    private static Font defaultFont;
	    private static Font currentFont;
	
	    public static bool Init(GraphicsContext graphicsContext, Stopwatch swInit = null)
	    {
	        graphics = graphicsContext;
	
	        textureShaderProgram = createSimpleTextureShader();
	        colorShaderProgram = createSimpleColorShader();
	
	        projectionMatrix = Matrix4.Ortho(0, Width,
	                                         0, Height,
	                                         0.0f, 32768.0f);
	
	        viewMatrix = Matrix4.LookAt(new Vector3(0, Height, 0),
	                                    new Vector3(0, Height, 1),
	                                    new Vector3(0, -1, 0));
	
	        rectVertices =  new VertexBuffer(4, VertexFormat.Float3);
	        rectVertices.SetVertices(0, new float[]{0, 0, 0,
	                                                1, 0, 0,
	                                                1, 1, 0,
	                                                0, 1, 0});
	
	        circleVertices =  new VertexBuffer(36, VertexFormat.Float3);
	        float[] circleVertex = new float[3 * 36];
	        for (int i = 0; i < 36; i++) {
	            float radian = ((i * 10) / 180.0f * FMath.PI);
	            circleVertex[3 * i + 0] = FMath.Cos(radian);
	            circleVertex[3 * i + 1] = FMath.Sin(radian);
	            circleVertex[3 * i + 2] = 0.0f;
	        }
	        circleVertices.SetVertices(0, circleVertex);
	
	        defaultFont = new Font(FontAlias.System, 24, FontStyle.Regular);
	        SetDefaultFont();
	
	        spriteDict = new Dictionary<string, TextSprite>();
	        spriteNamelessCount = 0;
	
	        return true;
	    }
	
	
	    /// Terminate
	    public static void Term()
	    {
	        ClearSprite();
	
	        defaultFont.Dispose();
	        circleVertices.Dispose();
	        rectVertices.Dispose();
	        colorShaderProgram.Dispose();
	        textureShaderProgram.Dispose();
	        graphics = null;
	    }
	
	    public static int Width
	    {
	        get {return graphics.GetFrameBuffer().Width;}
	    }
	
	    public static int Height
	    {
	        get {return graphics.GetFrameBuffer().Height;}
	    }
	
	    /// Touch coordinates -> screen coordinates conversion : X
	    public static int TouchPixelX(TouchData touchData)
	    {
	        return (int)((touchData.X + 0.5f) * Width);
	    }
	
	    /// Touch coordinates -> screen coordinates conversion : Y
	    public static int TouchPixelY(TouchData touchData)
	    {
	        return (int)((touchData.Y + 0.5f) * Height);
	    }
	
	    public static void AddSprite(TextSprite sprite)
	    {
	        AddSprite("[nameless]:" + spriteNamelessCount, sprite);
	        spriteNamelessCount++;
	    }
	
	    public static void AddSprite(string key, TextSprite sprite)
	    {
	        if (spriteDict.ContainsKey(key) == false) {
	            spriteDict.Add(key, sprite);
	        }
	    }
	
	    /// Register a sprite that draws text
	    public static void AddSprite(string key, string text, uint argb, Font font, int positionX, int positionY)
	    {
	        if (spriteDict.ContainsKey(text) == false) {
	            AddSprite(key, new TextSprite(text, argb, font, positionX, positionY));
	        }
	    }
	
	    /// Register a sprite that draws text
	    public static void AddSprite(string text, uint argb, int positionX, int positionY)
	    {
	        AddSprite(text, text, argb, currentFont, positionX, positionY);
	    }
	
	    public static void RemoveSprite(string key)
	    {
	        if (spriteDict.ContainsKey(key)) {
	            spriteDict[key].Dispose();
	            spriteDict.Remove(key);
	        }
	    }
	
	    public static void ClearSprite()
	    {
	        foreach (string key in spriteDict.Keys) {
	            spriteDict[key].Dispose();
	        }
	
	        spriteDict.Clear();
	        spriteNamelessCount = 0;
	    }
	
	    // if use Text's method, please call this method.
	    public static void Update()
	    {
	        int keyNo = 0;
	
	        string[] removeKey = new string[spriteDict.Count];
	
	        foreach (var key in spriteDict.Keys) {
	            if(false == spriteDict[key].CheckLifeTimer())
	            {
	                removeKey[keyNo] = key;
	                keyNo++;
	            }
	        }
	
	        for(int i = 0; i < keyNo; i++)
	        {
	            if(removeKey[i] != null)
	            {
	                RemoveSprite(removeKey[i]);
	            }
	        }
	    }
	
	    public static void DrawSprite(string key)
	    {
	        if (spriteDict.ContainsKey(key)) {
	            DrawSprite(spriteDict[key]);
	        }
	    }
	
	    public static void DrawSprite(TextSprite sprite)
	    {
	        var modelMatrix = sprite.CreateModelMatrix();
	        var worldViewProj = projectionMatrix * viewMatrix * modelMatrix;
	
	        textureShaderProgram.SetUniformValue(0, ref worldViewProj);
	
	        graphics.SetShaderProgram(textureShaderProgram);
	        graphics.SetVertexBuffer(0, sprite.Vertices);
	        graphics.SetTexture(0, sprite.Texture);
	
	        graphics.Enable(EnableMode.Blend);
	        graphics.SetBlendFunc(BlendFuncMode.Add, BlendFuncFactor.SrcAlpha, BlendFuncFactor.OneMinusSrcAlpha);
	
	        graphics.DrawArrays(DrawMode.TriangleFan, 0, 4);
	
	        sprite.SetLifeTimer();
	    }
	
	    public static Font CurrentFont
	    {
	        get {return currentFont;}
	    }
	
	    public static Font SetDefaultFont()
	    {
	        return SetFont(defaultFont);
	    }
	
	    public static Font SetFont(Font font)
	    {
	        Font previousFont = currentFont;
	        currentFont = font;
	        return previousFont;
	    }
	
	    public static void DrawText(string text, uint argb, Font font, int positionX, int positionY)
	    {
	        AddSprite(text, text, argb, font, positionX, positionY);
	        DrawSprite(text);
	    }
	
	    public static void DrawText(string text, uint argb, int positionX, int positionY)
	    {
	        AddSprite(text, text, argb, currentFont, positionX, positionY);
	        DrawSprite(text);
	    }

	    public static void FillVertices(VertexBuffer vertices, uint argb, float positionX, float positionY, float scaleX, float scaleY)
	    {
	        var transMatrix = Matrix4.Translation(new Vector3(positionX, positionY, 0.0f));
	        var scaleMatrix = Matrix4.Scale(new Vector3(scaleX, scaleY, 1.0f));
	        var modelMatrix = transMatrix * scaleMatrix;
	
	        var worldViewProj = projectionMatrix * viewMatrix * modelMatrix;
	
	        colorShaderProgram.SetUniformValue(0, ref worldViewProj);
	
	        Vector4 color = new Vector4((float)((argb >> 16) & 0xff) / 0xff,
	                                    (float)((argb >> 8) & 0xff) / 0xff,
	                                    (float)((argb >> 0) & 0xff) / 0xff,
	                                    (float)((argb >> 24) & 0xff) / 0xff);
	        colorShaderProgram.SetUniformValue(colorShaderProgram.FindUniform("MaterialColor"), ref color);
	
	        graphics.SetShaderProgram(colorShaderProgram);
	        graphics.SetVertexBuffer(0, vertices);
	
	        graphics.DrawArrays(DrawMode.TriangleFan, 0, vertices.VertexCount);
	    }

		public static Texture2D createTexture(string text, Font font, uint argb)
		{
			int width = font.GetTextWidth(text, 0, text.Length);
			int height = font.Metrics.Height;
	
			var image = new Image(ImageMode.Rgba,
			new ImageSize(width, height),
			new ImageColor(0, 0, 0, 0));
	
			image.DrawText(text,
			new ImageColor((int)((argb >> 16) & 0xff),
			               (int)((argb >> 8) & 0xff),
			               (int)((argb >> 0) & 0xff),
			               (int)((argb >> 24) & 0xff)),
			font, new ImagePosition(0, 0));
	
			var texture = new Texture2D(width, height, false, PixelFormat.Rgba);
			texture.SetPixels(0, image.ToBuffer());
			image.Dispose();

			return texture;
		}
	
	    private static ShaderProgram createSimpleTextureShader()
	    {
	        string ResourceName = "API.shaders.Texture.cgx";
	            
	        Assembly resourceAssembly = Assembly.GetExecutingAssembly();
	        if (resourceAssembly.GetManifestResourceInfo(ResourceName) == null)
	        {
	            throw new FileNotFoundException("File not found.", ResourceName);
	        }
	
	        Stream fileStreamVertex = resourceAssembly.GetManifestResourceStream(ResourceName);
	        Byte[] dataBufferVertex = new Byte[fileStreamVertex.Length];
	        fileStreamVertex.Read(dataBufferVertex, 0, dataBufferVertex.Length);
	            
	        var shaderProgram = new ShaderProgram(dataBufferVertex);
	        // var shaderProgram = new ShaderProgram("/Application/shaders/Texture.cgx");
	
	        shaderProgram.SetAttributeBinding(0, "a_Position");
	        shaderProgram.SetAttributeBinding(1, "a_TexCoord");
	
	        shaderProgram.SetUniformBinding(0, "WorldViewProj");
	
	        return shaderProgram;
	    }
	
	    private static ShaderProgram createSimpleColorShader()
	    {
	        string ResourceName = "API.shaders.Simple.cgx";
	            
	        Assembly resourceAssembly = Assembly.GetExecutingAssembly();
	        if (resourceAssembly.GetManifestResourceInfo(ResourceName) == null)
	        {
	            throw new FileNotFoundException("File not found.", ResourceName);
	        }
	
	        Stream fileStreamVertex = resourceAssembly.GetManifestResourceStream(ResourceName);
	        Byte[] dataBufferVertex = new Byte[fileStreamVertex.Length];
	        fileStreamVertex.Read(dataBufferVertex, 0, dataBufferVertex.Length);
	            
	        var shaderProgram = new ShaderProgram(dataBufferVertex);
	        // var shaderProgram = new ShaderProgram("/Application/shaders/Simple.cgx");
	
	        shaderProgram.SetAttributeBinding(0, "a_Position");
	        //shaderProgram.SetAttributeBinding(1, "a_Color0");
	
	        shaderProgram.SetUniformBinding(0, "WorldViewProj");
	
	        return shaderProgram;
	    }
	}
	
}