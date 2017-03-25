using UnityEngine;
using System.Collections;


/*
 * 
 * © 2014 LemonSpawn. All rights reserved. Source code in this project ("TangyTextures") are not supported under any LemonSpawn standard support program or service. 
 * The scripts are provided AS IS without warranty of any kind. LemonSpawn disclaims all implied warranties including, without limitation, 
 * any implied warranties of merchantability or of fitness for a particular purpose. 
 * 
 * Basically, do what you want with this code, but don't blame us if something blows up. 
 * 
 * 
*/

namespace TangyTextures
{
		public class NormalMap
		{

				public static Vector2 size = new Vector2 ();

				public static float Sgn (float val)
				{
						float retSign = 0f;
						if (val > 0)
								retSign = 1f;
						if (val < 0)
								retSign = -1f;
						return retSign;
				}

				public static float GetPixelGray (int x, int y, C2DMap map)
				{
						if (x < 0) 
								x += (int)size.x;
						if (x >= size.x) 
								x -= (int)size.x;
						if (y < 0) 
								y += (int)size.y;
						if (y >= size.y) 
								y -= (int)size.y;

						return map.map [x, y] * 255;
				}

				public static Texture2D CreateDOT3 (C2DMap pixmap, float contrast, float treshold)
				{
	
						Texture2D retTexture = new Texture2D (C2DMap.sizeX, C2DMap.sizeY, TextureFormat.ARGB32, false);
						Color[] retColor = new Color[C2DMap.sizeX * C2DMap.sizeY];
						size.x = C2DMap.sizeX;
						size.y = C2DMap.sizeY;
						for (int x = 0; x<size.x; x++) {
								for (int y = 0; y<size.y; y++) {
			
										float tl = -1.0f;
										float tm = -1.0f;
										float tr = -1.0f;
										float ml = -1.0f;
										float mm = -1.0f;
										float mr = -1.0f;
										float bl = -1.0f;
										float bm = -1.0f;
										float br = -1.0f;
			
			
			
										tl = GetPixelGray (x - 1, y - 1, pixmap);
				
										tm = GetPixelGray (x, y - 1, pixmap);
			
										tr = GetPixelGray (x + 1, y - 1, pixmap);
			
										ml = GetPixelGray (x - 1, y, pixmap);
										mm = GetPixelGray (x, y, pixmap);
			
										mr = GetPixelGray (x + 1, y, pixmap);
			
										bl = GetPixelGray (x - 1, y + 1, pixmap);
			
										bm = GetPixelGray (x, y + 1, pixmap);
			
										br = GetPixelGray (x + 1, y + 1, pixmap);
			
			
										if (tl == -1.0f)
												tl = mm;
										if (tm == -1.0f)
												tm = mm;
										if (tr == -1.0f)
												tr = mm;
										if (ml == -1.0f)
												ml = mm;
										if (mr == -1.0f)
												mr = mm;
										if (bl == -1.0f)
												bl = mm;
										if (bm == -1.0f)
												bm = mm;
										if (br == -1.0f)
												br = mm;
			
										float vx = 0.0f;
										float vy = 0.0f;
										float vz = 1.0f;
			
										float isq2 = 1.0f / Mathf.Sqrt (2.0f);
										float sum = 1.0f + isq2 + isq2;
						
										float al = (tl * isq2 + ml + bl * isq2) / sum;
										float ar = (tr * isq2 + mr + br * isq2) / sum;
										float at = (tl * isq2 + tm + tr * isq2) / sum;
										float ab = (bl * isq2 + bm + br * isq2) / sum;			

										vx = (al - ar) / 255.0f * contrast;
										vy = (at - ab) / 255.0f * contrast;


										float r = vx * 128.5f + 128.5f;
										float g = vy * 128.5f + 128.5f;
										float b = vz * 255.0f;
			
										if (r < 0)
												r = 0f;
										if (r > 255)
												r = 255f;
										if (g < 0)
												g = 0f;
										if (g > 255)
												g = 255f;
										if (b < 0)
												b = 0f;
										if (b > 255)
												b = 255f;
			
										Color rgb = new Color (r / 255f, g / 255f, b / 255f, 0.5f);
			
										retColor [x + (C2DMap.sizeX * y)] = rgb;
								}
						}
						retTexture.SetPixels (retColor);
						retTexture.wrapMode = TextureWrapMode.Repeat;
						retTexture.Apply ();
			
						return retTexture;
				}

		}
}
