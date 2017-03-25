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
		public class BumpMap : CTextureNode
		{

				public override void Initialize (int windowID, int type, int x, int y)
				{
			
						status_after_click = CNodeManager.STATUS_NONE;
						InitializeWindow (windowID, type, x, y, "NormalMap");		
						map = new C2DMap ();
						colors = new C2DRGBMap ();
						color = LStyle.Colors [2] * 1.0f;
			
						Outputs.Add (new CConnection (this, 0, CConnection.TYPE2));
						Inputs.Add (new CConnection (this, 1, CConnection.TYPE0));
						types = new string[] {"Normal map"};

						//parameters ["threshold"] = new Parameter ("Threshold:", 0f, -1, 1f,0, "threshold");
						parameters ["contrast"] = new Parameter ("Contrast:", 5f, -15, 15f, 1, "contrast");
			
						texture = new Texture2D (C2DMap.sizeX, C2DMap.sizeY);
			
						helpMessage = "<size=24>" + LStyle.hexColors [1] + "Normal map generator. </color></size>" +
								"\n" +
								"\n" +
								"This node takes a height map as input and generates a RGB normal map." +
								"\n" +
								"<size=15>" + LStyle.hexColors [2] + "Parameters:</color></size>\n" +
								"  " + LStyle.hexColors [1] + "Contrast</color>: Controls the amplitude of the normal map. \n";
			
															
				}
		
				public override void Calculate ()
				{
			
						if (!verified)
								return;
				
						if (!changed)
								return;
						CTextureNode m1 = (CTextureNode)getNode (Inputs, 0);
						if (m1 == null) {
								verified = false;
								return;
						}
						m1.Calculate ();
						updateTexture = true;
						//		float scale = getValue("scale");
						map.CopyFrom (m1.map);
			
				}
		
				public override void GenerateTexture ()
				{
						foreach (CConnection ct in Inputs) {
								if (ct.pointer != null)
								if (ct.pointer.parent != null)
										((CTextureNode)ct.pointer.parent).GenerateTexture ();
						}
						CTextureNode m1 = (CTextureNode)getNode (Inputs, 0);
						CNodeManager.Progress ();
						if (m1 == null)
								return;
						if (!updateTexture) 
								return;
						map.CopyFrom (m1.map);
						map.Normalize (1);
						texture = NormalMap.CreateDOT3 (m1.map, getValue ("contrast") * 1.5f, 0);
						CNodeManager.Progress ();
						updateTexture = false;
				}
		
		
		
		}

}
