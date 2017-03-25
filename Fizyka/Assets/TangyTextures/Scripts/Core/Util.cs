using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;


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

namespace TangyTextures{

/*
* Saving detail prototypes as serializable objects. Bah. Needs to be done because unity is evil.
*/
		[System.Serializable] 
		public class DetailPrototypeSerializable
		{
				public float maxHeight = 2;
				public float minHeight = 1;
				public float minWidth = 1;
				public float maxWidth = 2;
				public float noiseSpread = 1.0f;
				public Color dryColor = new Color ();
				public Color healthyColor = new Color ();
				public Texture2D prototypeTexture = null;

				public DetailPrototype toDetailPrototype ()
				{
						DetailPrototype dt = new DetailPrototype ();
						dt.dryColor = dryColor;
						dt.healthyColor = healthyColor;
						dt.maxWidth = maxWidth;
						dt.minWidth = minWidth;
						dt.maxHeight = maxHeight;
						dt.minHeight = minHeight;
						dt.noiseSpread = noiseSpread;
						dt.prototypeTexture = prototypeTexture;
						dt.usePrototypeMesh = false;
						dt.renderMode = DetailRenderMode.GrassBillboard;
						return dt;
				}


		}	
		/*
		* General utilities
		*
		*/
		public class Util : MonoBehaviour
		{

				private static float[] weights = null;
				static int oct = -1;
				static float lac = -1;
				static float freq = -1;
 //				static System.Random rand = new System.Random (0);
				public static Noise4D noise4D = new Noise4D ();
				public static System.Random rnd = new System.Random();
		
		
				public static float floatTextField (string label, float f)
				{
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Label (label);
						string t = "";
						if (f != 0)
								t += "" + f;
						string text = GUILayout.TextField (t);
						EditorGUILayout.EndHorizontal ();
						return getFloatFromString (text, f);

				}

				public static float floatTextField (string label, float w, float f)
				{
						EditorGUILayout.BeginHorizontal ();
						GUILayout.Label (label);
						string t = "";
						if (f != 0)
								t += "" + f;
						string text = GUILayout.TextField (t, GUILayout.Width (w));
						EditorGUILayout.EndHorizontal ();
						return getFloatFromString (text, f);
				}

				private static void generateSpectralWeights (float lacunarity,
	                                                 int octaves, float h, float frequency)
				{
						if (oct != octaves) {
								weights = new float[octaves]; 
								oct = octaves;
						}

						if (lac != lacunarity || freq != frequency || oct != octaves)
		
								for (int octave = 0; octave < octaves; ++octave) {
										weights [octave] = Mathf.Pow (frequency, h);
										frequency *= lacunarity;
								}
		
				}

				public static float getRidgedMf (Vector3 p, float frequency, int octaves, float lacunarity, float warp, float offset, float gain, float initialOffset)
				{
						float value = 0.0f;
						float weight = 1.0f;
		
						float w = -0.05f;

						generateSpectralWeights (lacunarity, octaves, w, frequency);

						freq = frequency;
						lac = lacunarity;
						oct = octaves;
		
						Vector3 vt = p*frequency;
						for (float octave = 0; octave < octaves; octave++) {
								//float signal = simplex.noise3D (vt);//   Mathf.PerlinNoise(vt.x, vt.z);
								float signal = initialOffset+perlinNoise2dSeamlessRaw(frequency, vt.x, vt.z,0,0,0,0);//   Mathf.PerlinNoise(vt.x, vt.z);
				
								// Make the ridges.
								signal = Mathf.Abs (signal);
								signal = offset - signal;


								signal *= signal;
			
								signal *= weight;
								weight = signal * gain;
								if (weight > 1.0f) {
										weight = 1.0f;
								}
								if (weight < 0.0f) {
										weight = 0.0f;
								}
			
			
								value += (signal * weights [(int)octave]);
								vt = vt * lacunarity;
								frequency*=lacunarity;
						}
						return value;//(value * 1.25) - 1.0;
				}

				public static float getNoise (float x, float y, float z)
				{
						//tmp2.Set (x, y, z);
						//return simplex.noise3D (tmp2);
						//return perlinNoise2dSeamlessRaw(frequency, x, y,0,0,0,0);//   Mathf.PerlinNoise(vt.x, vt.z);
						return noise4D.raw_noise_4d(x,y,z,0);
			
		}
		
		public static float perlinNoiseDeviv (Vector3 p, int i, float sc, float sc2)
				{
						if (sc == 0)
								sc = 1.0f;
						float e = 0.09193f;//*sc;



						float F0 = getNoise (p.x, p.y, p.z);
						float Fx = getNoise (p.x + e, p.y, p.z);
						float Fy = getNoise (p.x, p.y + e, p.z);
						float Fz = getNoise (p.x, p.y, p.z + e);

						N.Set ((Fx - F0) / e, (Fy - F0) / e, (Fz - F0) / e);
		
						float s = 0.8f;
						N.Normalize ();
						N = N * s;
						return F0;
				}

				static Vector3 N = new Vector3 ();
	
				public static float swissTurbulence (Vector3 p, float scalex, float scaley, int octaves, float lacunarity, float warp, float offset, float gain, float powscale, float background)
				{
		
						float sum = 0;
						float freq = 1.0f;
						float amp = 1.0f;
						Vector3 dsum = new Vector3 ();

						dsum.Set (0, 0, 0);
						Vector3 t = new Vector3 ();
						for (int i=0; i < octaves; i++) {
								t.Set (p.x * scalex, p.y * scalex, p.z * scaley);
								t = t + dsum * warp;
								t = t * freq;

								float F = perlinNoiseDeviv (t, i, scalex * freq, scaley*freq);
			
								float n = (offset - powscale * Mathf.Abs (F));

								n = n * n;
								sum += amp * n;
								t.Set (N.x, N.y, N.z);
								t = t * amp * -F;
								dsum = dsum + t;
								freq *= lacunarity;

								amp *= gain * Mathf.Clamp (sum, 0, 1);
						}
		
						return sum;
				}

				public static bool LastFloatValueChanged = false;

				public static float getFloatFromString (string s, float org)
				{
		
						s = Regex.Replace (s, @"[^0-9.]", "");
						float val;
						if (float.TryParse (s, out val)) {
								if (val == org)
										LastFloatValueChanged = false;
								else
										LastFloatValueChanged = true;
								return val;
						} 
						else
						   return 0;
				}

				public static string ColorToHex (Color32 color)
				{
						string hex = "#" + color.r.ToString ("X2") + color.g.ToString ("X2") + color.b.ToString ("X2");
						return hex;
				}
		static public float perlinNoise2dSeamless(float octaves, float scale, float persistence, float s, float t, float cx, float cy, float cz, float cw)
		{
			// Generate the 4d coordinates that wrap around seamlessly
			float r = scale / (2 * Mathf.PI);
			float axy = 2 * Mathf.PI * s;// / scale;        
			float x = r * Mathf.Cos(axy);
			float y = r * Mathf.Sin(axy);
			
			float azw = 2 * Mathf.PI * t;// / scale;        
			float z = r * Mathf.Cos(azw);
			float w = r * Mathf.Sin(azw);
			
			return noise4D.octave_noise_4d(octaves, persistence, scale, cx + x, cy + y, cz + z, cw + w);
		}

		static public float perlinNoise2dSeamless_alt(float octaves, float scale, float persistence, float s, float t, float cx, float cy, float cz, float cw, float pow)
		{
			// Generate the 4d coordinates that wrap around seamlessly
			float r = scale / (2 * Mathf.PI);
			float axy = 2 * Mathf.PI * s;// / scale;        
			float x = r * Mathf.Cos(axy);
			float y = r * Mathf.Sin(axy);
			
			float azw = 2 * Mathf.PI * t;// / scale;        
			float z = r * Mathf.Cos(azw);
			float w = r * Mathf.Sin(azw);
			
			return noise4D.octave_noise_4d_alt (octaves, persistence, scale, cx + x, cy + y, cz + z, cw + w, pow);
		}
		
		static public float perlinNoise2dSeamlessRaw(float scale, float s, float t, float cx, float cy, float cz, float cw)
		{
			// Generate the 4d coordinates that wrap around seamlessly
			float r = scale / (2 * Mathf.PI);
			float axy = 2 * Mathf.PI * s/ scale;        
			float x = r * Mathf.Cos(axy);
			float y = r * Mathf.Sin(axy);
			
			float azw = 2 * Mathf.PI * t/ scale;        
			float z = r * Mathf.Cos(azw);
			float w = r * Mathf.Sin(azw);
			
			return noise4D.raw_noise_4d(cx + x, cy + y, cz + z, cw + w);
		}
		
		
		public static Vector2 Rotate2D(Vector2 v, float t) {
			return new Vector2(v.x*Mathf.Cos(t) - v.y*Mathf.Sin (t), v.x*Mathf.Sin(t) + v.y*Mathf.Cos (t));
		}

		public static void Rotate2DDirect(Vector2 v, float t) {
			v.x = v.x*Mathf.Cos(t) - v.y*Mathf.Sin (t);
			v.y = v.x*Mathf.Sin(t) + v.y*Mathf.Cos (t);
		}
		
		public static void SaveTextureFile(Texture2D incomingTexture, string incomingDataPath, string incomingFilename) { 
			
			FileStream fs = new FileStream(incomingDataPath + incomingFilename + ".png", FileMode.Create);
			BinaryWriter bw = new BinaryWriter(fs);
			bw.Write(incomingTexture.EncodeToPNG());
			
			bw.Close();
			fs.Close();
		}
		}
}
