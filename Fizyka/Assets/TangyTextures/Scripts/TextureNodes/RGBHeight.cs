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
		public class RGBHeight : CTextureNode
		{
		
				public override void Initialize (int windowID, int type, int x, int y)
				{
			
						status_after_click = CNodeManager.STATUS_NONE;
						InitializeWindow (windowID, type, x, y, "RGB Height");		
						map = new C2DMap ();
						colors = new C2DRGBMap ();
			
						color = LStyle.Colors [3] * 1.0f;
			
						Outputs.Add (new CConnection (this, 0, CConnection.TYPE1));
						Inputs.Add (new CConnection (this, 1, CConnection.TYPE0));
						types = new string[] {"RGB Height"};
			
						//parameters ["scale"] = new Parameter ("Scale:", 10.0f, 0, 100f);
						parameters ["color1"] = new Parameter ("ColorColor1", 0.5f, 0.5f, 0.5f, 0, "color1");
						parameters ["color2"] = new Parameter ("ColorColor2", 0.5f, 0.5f, 0.5f, 1, "color2");
						parameters ["scale"] = new Parameter ("Threshold", 0.5f, 0.0f, 1.0f, 2, "scale");
						parameters ["power"] = new Parameter ("Power", 1.0f, 0.0f, 10.0f, 3, "power");
			
						texture = new Texture2D (C2DMap.sizeX, C2DMap.sizeY);						
						helpMessage = "<size=24>" + LStyle.hexColors [1] + "RGB color map generator from heightmap. </color></size>" +
								"\n" +
								"\n" +
								"This node takes a heighmap as input and produces a color map that interpolates between two colors given the values from the input map." +
								"\n" +
								"<size=15>" + LStyle.hexColors [2] + "Parameters:</color></size>\n" +
								"  " + LStyle.hexColors [1] + "Color 1/2</color>: The base colors for the lowest / highest regions in the map \n" +
								"  " + LStyle.hexColors [1] + "Threshold</color>: Scales the height threshold for the transition between color 1 and color 2,  \n" + 
								"  " + LStyle.hexColors [1] + "Power</color>: Defines the sharpness of the transition between the colors. \n";
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
			
						float scale = getValue ("scale");
						float power = getValue ("power");
						Color c1 = getColor ("color1");
						Color c2 = getColor ("color2");
		
						Color c = new Color (1, 1, 1);
						for (int i=0; i<C2DMap.sizeX; i++) {
								for (int j=0; j<C2DMap.sizeY; j++) {
										float val = Mathf.SmoothStep (0, 1.0f, m1.map [i, j] - scale + 0.75f);
										val = Mathf.Clamp (Mathf.Pow (val, power), 0, 1);
										c = c1 * val + c2 * (1 - val);
										colors.colors [i, j] = c;
								}
						}
						updateTexture = true;
						CNodeManager.Progress ();
			
						changed = false;
				}
		
		
		}
	
}
