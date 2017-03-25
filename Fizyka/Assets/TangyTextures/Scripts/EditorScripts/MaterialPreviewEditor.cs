using UnityEngine;
using System.Collections;
using UnityEditor;

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
		public class MaterialPreviewEditor : EditorWindow
		{

				static private string materialName;
				static public Material currentMaterial = null;
				static Editor matEditor = null;
				public static EditorWindow window;
				public static float shininess;
				public static float heightmapHeight;
				public static float threshold;
				public static float direction;
				public static float tiling;
				public static Color colorMaterial = new Color (0.5f, 0.5f, 0.5f);
				public static Color colorShininess = new Color (0.5f, 0.5f, 0.5f);
				static Vector2 tilingVec = new Vector2 ();
		
				public static void Create (string mat)
				{
						materialName = mat;
						window = EditorWindow.GetWindow (typeof(MaterialPreviewEditor));
				}
		
				static public void forceRepaint ()
				{
						window.Repaint ();
				}
		
				static public void SetTextures (Texture2D NormalMap, Texture2D HeightMap, Texture2D ColorMap, Material mat)
				{
						if (mat == null)
								return;
						if (ColorMap != null)
								mat.SetTexture ("_MainTex", ColorMap);
						if (HeightMap != null)
								mat.SetTexture ("_ParallaxMap", HeightMap);
						if (NormalMap != null)
								mat.SetTexture ("_BumpMap", NormalMap);
				}
		
				public static void setParameters (float height, float shin, Color colorMat, Color colorShine, float dir, float thresh, float til)
				{
						colorShininess = colorShine;
						colorMaterial = colorMat;
						heightmapHeight = height;
						shininess = shin;
						threshold = thresh;
						direction = dir;
						tiling = til;
				}
		
				public static void SetProperties (Material mat)
				{
						if (mat == null) 
								return;
				
						mat.SetFloat ("_Shininess", shininess);
						mat.SetFloat ("_Parallax", heightmapHeight);
						//mat.SetFloat("_Direction", direction);
						//mat.SetFloat("_Threshold", threshold);
						//mat.SetFloat("_Tiling", tiling);
						mat.SetColor ("_SpecColor", colorShininess);
						mat.SetColor ("_Color", colorMaterial);
			
						mat.SetTextureScale ("_MainTex", tilingVec);			
						mat.SetTextureScale ("_ParallaxMap", tilingVec);			
						mat.SetTextureScale ("_BumpMap", tilingVec);			
						//mat.SetTextureScale("_ShininessMap", tilingVec);
			
				}
		
				private void RenderObjects ()
				{
						if (currentMaterial == null) {
								currentMaterial = (Material)Resources.Load (materialName);
								if (currentMaterial == null)
										return;
								currentMaterial.color = new Color (1, 1, 1);
						}
						//if (matEditor == null) 
						GUI.color = Color.white;
						if (matEditor == null)
								matEditor = Editor.CreateEditor (currentMaterial); 
			
						int d = 0;

						tilingVec.x = tiling;
						tilingVec.y = tiling;
			
						SetProperties (currentMaterial);						
						matEditor.OnPreviewGUI (new Rect (0, d, Screen.width, Screen.height), GUIStyle.none);
	
				}
		
				void OnGUI ()
				{
						RenderObjects ();	
				}
		
		
		}
}