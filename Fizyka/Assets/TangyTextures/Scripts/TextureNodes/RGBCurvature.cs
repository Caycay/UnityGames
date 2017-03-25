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
		public class RGBCurvature : CTextureNode
		{
		
				public override void Initialize (int windowID, int type, int x, int y)
				{
			
			
						status_after_click = CNodeManager.STATUS_NONE;
						InitializeWindow (windowID, type, x, y, "RGB Curvature");		
						map = new C2DMap ();
						colors = new C2DRGBMap ();
			
						color = LStyle.Colors [3] * 1.0f;
			
						Outputs.Add (new CConnection (this, 0, CConnection.TYPE1));
						Inputs.Add (new CConnection (this, 1, CConnection.TYPE0));
						types = new string[] {"RGB Curvature"};
			
						//parameters ["scale"] = new Parameter ("Scale:", 10.0f, 0, 100f);
						parameters ["color1"] = new Parameter ("ColorColor", 0.5f, 0.5f, 0.5f, 0, "color1");
						parameters ["curvtype"] = new Parameter ("Type", 1.0f, 1.0f, 6.0f, 1, "curvtype");
						parameters ["scale"] = new Parameter ("Scale", 0.0f, 0.0f, 20.0f, 2, "scale");
			
						texture = new Texture2D (C2DMap.sizeX, C2DMap.sizeY);						
						helpMessage = "<size=24>" + LStyle.hexColors [1] + "RGB color map curvature. </color></size>" +
								"\n" +
								"\n" +
								"This node is a filter that takes a heigth map as input and assigns a color to regions depending on their curvature properties." +
								"\n" +
								"<size=15>" + LStyle.hexColors [2] + "Parameters:</color></size>\n" +
								"  " + LStyle.hexColors [1] + "Amplitude</color>: Controls the amplitude of the output signal. \n" +
								"  " + LStyle.hexColors [1] + "Scale</color>: Amplifies the signal, creating steeper (and narrower) curves. \n" + 
								"  " + LStyle.hexColors [1] + "Type</color>: Type of curvature identification: \n" + 
								"      " + LStyle.hexColors [1] + "1</color>: Hills. \n" + 
								"      " + LStyle.hexColors [1] + "2</color>: Lakes. \n" +
								"      " + LStyle.hexColors [1] + "3</color>: Hills + Lakes. \n" + 
								"      " + LStyle.hexColors [1] + "4</color>: Upward slopes. \n" + 
								"      " + LStyle.hexColors [1] + "5</color>: Downward slopes. \n" + 
								"      " + LStyle.hexColors [1] + "6</color>: All slopes. \n";
			
				}
		
				Vector3[,] normals = null;
		
				protected override void ExtraOnGUI ()
				{
						setValue ("curvtype", (int)getValue ("curvtype"));
				}
		
				public override void Calculate ()
				{
						base.Calculate ();
						if (!verified)
								return;
			
						CTextureNode m1 = (CTextureNode)getNode (Inputs, 0);
			
						if (m1 == null) {
								verified = false;
								return;
						}
			
						m1.Calculate ();
			
						if (!changed)
								return;
			
						float curvtype = getValue ("curvtype");
						float scale = getValue ("scale");
						Color c1 = getColor ("color1");
						if (normals == null)
								normals = new Vector3[C2DMap.sizeX, C2DMap.sizeY];
						if (normals.GetLength (0) != C2DMap.sizeX)
								normals = new Vector3[C2DMap.sizeX, C2DMap.sizeY];
			
						m1.map.calculateNormals (100, normals);
						for (int i=0; i<C2DMap.sizeX; i++) {
								for (int j=0; j<C2DMap.sizeY; j++) {
										colors.colors [i, j] = c1 * Mathf.Pow (m1.map.getCurvature (i, j, scale, (int)curvtype, normals), 1);					
								}
						}
						colors.Smooth (1);
						updateTexture = true;
						CNodeManager.Progress ();
			
						changed = false;
				}
		
		
		}
	
}
