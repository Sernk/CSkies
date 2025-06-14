using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace CSkies.Utilities.Base.BaseMod.Base
{
    public class BaseWorldGenTex
    {
		public static Dictionary<Color, int> colorToLiquid = null;
		public static Dictionary<Color, int> colorToSlope = null;	

		//NOTE: all textures MUST be the same size or horrible things happen!
		public static TexGen GetTexGenerator(Texture2D tileTex, Dictionary<Color, int> colorToTile, Texture2D wallTex = null, Dictionary<Color, int> colorToWall = null, Texture2D liquidTex = null, Texture2D slopeTex = null)
		{
			if(colorToLiquid == null)
			{
				colorToLiquid = new Dictionary<Color, int>();
				colorToLiquid[new Color(0, 0, 255)] = 0;
				colorToLiquid[new Color(255, 0, 0)] = 1;
				colorToLiquid[new Color(255, 255, 0)] = 2;

				colorToSlope = new Dictionary<Color, int>();
				colorToSlope[new Color(255, 0, 0)] = 1; // |\ //
				colorToSlope[new Color(0, 255, 0)] = 2; // /| //
				colorToSlope[new Color(0, 0, 255)] = 3; // |/ //				
				colorToSlope[new Color(255, 255, 0)] = 4; // \| //
				colorToSlope[new Color(255, 255, 255)] = -1; // HALFBRICK //
				colorToSlope[new Color(0, 0, 0)] = -2; // FULLBLOCK //			
			}
			Color[] tileData = new Color[tileTex.Width * tileTex.Height];
			tileTex.GetData(0, tileTex.Bounds, tileData, 0, tileTex.Width * tileTex.Height);
			Color[] wallData = (wallTex != null ? new Color[wallTex.Width * wallTex.Height] : null);
			if(wallData != null) wallTex.GetData(0, wallTex.Bounds, wallData, 0, wallTex.Width * wallTex.Height);			
			Color[] liquidData = (liquidTex != null ? new Color[liquidTex.Width * liquidTex.Height] : null);
			if(liquidData != null) liquidTex.GetData(0, liquidTex.Bounds, liquidData, 0, liquidTex.Width * liquidTex.Height);				
			Color[] slopeData = (slopeTex != null ? new Color[slopeTex.Width * slopeTex.Height] : null);
			if(slopeData != null) slopeTex.GetData(0, slopeTex.Bounds, slopeData, 0, slopeTex.Width * slopeTex.Height);
			
			int x = 0, y = 0;
			TexGen gen = new TexGen(tileTex.Width, tileTex.Height);
			for(int m = 0; m < tileData.Length; m++)
			{
				Color tileColor = tileData[m], wallColor = (wallTex == null ? Color.Black : wallData[m]), liquidColor = (liquidTex == null ? Color.Black : liquidData[m]), slopeColor = (slopeTex == null ? Color.Black : slopeData[m]);
				int tileID = (colorToTile.ContainsKey(tileColor) ? colorToTile[tileColor] : -1); //if no key assume no action
				int wallID = (colorToWall != null && colorToWall.ContainsKey(wallColor) ? colorToWall[wallColor] : -1);
				int liquidID = (colorToLiquid != null && colorToLiquid.ContainsKey(liquidColor) ? colorToLiquid[liquidColor] : -1);
				int slopeID = (colorToSlope != null && colorToSlope.ContainsKey(slopeColor) ? colorToSlope[slopeColor] : -1);
				gen.tileGen[x, y] = new TileInfo(tileID, 0, wallID, liquidID, liquidID == -1 ? 0 : 255, slopeID);
				x++;
				if(x >= tileTex.Width){ x = 0; y++; }
				if(y >= tileTex.Height) break; //you've somehow reached the end of the texture! (this shouldn't happen!)
			}
			return gen;
		}
	}
	
	public class TexGen
	{
		public int width, height;
		public TileInfo[,] tileGen;
		public int torchStyle = 0, platformStyle = 0;
		
		public TexGen(int w, int h)
		{
			width = w; height = h;
			tileGen = new TileInfo[width, height];
		}

		//where x, y is the top-left hand corner of the gen
		public void Generate(int x, int y, bool silent, bool sync)
		{
			for(int x1 = 0; x1 < width; x1++)
			{
				for(int y1 = 0; y1 < height; y1++)
				{
					int x2 = x + x1, y2 = y + y1;
					TileInfo info = tileGen[x1, y1];
					if(info.tileID == -1 && info.wallID == -1 && info.liquidType == -1 && info.wire == -1) continue;
					if(info.tileID != -1 || info.wallID > -1 || info.wire > -1) BaseWorldGen.GenerateTile(x2, y2, info.tileID, info.wallID, (info.tileStyle != 0 ? info.tileStyle : info.tileID == TileID.Torches ? torchStyle : info.tileID == TileID.Platforms ? platformStyle : 0), info.tileID > -1, info.liquidAmt == 0, info.slope, false, sync);
					if(info.liquidType > -1)
					{
						BaseWorldGen.GenerateLiquid(x2, y2, info.liquidType, false, info.liquidAmt, sync);
					}
				}				
			}
		}
	}
	
	public class TileInfo
	{
		public int tileID = -1, tileStyle = 0, wallID = -1;
		public int liquidType = -1, liquidAmt = 0; //liquidType can be 0 (water), 1 (lava), 2 (honey)
		public int slope = -2, wire = -1;
		
		public TileInfo(int id, int style, int wid = -1, int lType = -1, int lAmt = 0, int sl = -2, int w = -1)
		{
			tileID = id; tileStyle = style; wallID = wid; liquidType = lType; liquidAmt = lAmt; slope = sl; wire = w;
		}
	}
}