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
		public class RGBCombiner : CTextureNode
		{
		
				public static int TYPE_BLEND = 0;
				public static int TYPE_MUL = 1;
				public static int TYPE_SUB = 2;
				public static int TYPE_OVERRIDE = 3;
				public static int TYPE_MIN = 4;
				public static int TYPE_MAX = 5;
		
				public override void Initialize (int windowID, int type, int x, int y)
				{
			
						status_after_click = CNodeManager.STATUS_NONE;
						InitializeWindow (windowID, type, x, y, "RGB Combiner");		
						map = new C2DMap ();
						setupParameters ();
						colors = new C2DRGBMap ();
			
						color = LStyle.Colors [3] * 1.0f;
			
			
						Outputs.Add (new CConnection (this, 0, CConnection.TYPE1));
						Outputs.Add (new CConnection (this, 1, CConnection.TYPE1));
						Outputs.Add (new CConnection (this, 2, CConnection.TYPE1));
						Outputs.Add (new CConnection (this, 3, CConnection.TYPE1));
						Inputs.Add (new CConnection (this, 4, CConnection.TYPE0));
						Inputs.Add (new CConnection (this, 5, CConnection.TYPE1));
						Inputs.Add (new CConnection (this, 6, CConnection.TYPE1));
						types = new string[] {"Blend","Multiply", "Subtract", "Override", "Min", "Max"};
						texture = new Texture2D (C2DMap.sizeX, C2DMap.sizeY);						
			
						helpMessage = "<size=24>" + LStyle.hexColors [1] + "RGB color combiner. </color></size>" +
								"\n" +
								"\n" +
								"This node takes two color maps as input and combines them to a new color map.\n" + 
								"The upper height map input is optional, and only used for the blend and subtract options." +
								"\n" +
								"There are four methods of combining maps: by addition (blend), subraction, multiplication and override.\n " +
								"\n" +
								"<size=15>" + LStyle.hexColors [2] + "Parameters:</color></size>\n" +
								"  " + LStyle.hexColors [1] + "Amplitude</color>: Controls the amplitude of the output signal. \n" +
								"  " + LStyle.hexColors [1] + "Blendval</color>: Weights the two input signals between 0-100% (does not apply for multiply). \n" + 
								"\n" +
								"<size=15>" + LStyle.hexColors [2] + "Blend :</color></size>\n" +
								"  With the blend setting the two colormaps are added together, weighted according to blendval. If a height map is connected, " +
								"this will also define local weighting of the two color maps, so that some areas are dominated by the one color map, and some by the other.\n" +
								"\n" +
								"<size=15>" + LStyle.hexColors [2] + "Multiply :</color></size>\n" +
								"  Multiplies the one color map by the other to create the output color map. \n" +
								"\n" +
								"<size=15>" + LStyle.hexColors [2] + "Subtract :</color></size>\n" +
								"  Subtracts the one color map from the other, with weighting according to blendval. If a height map is connected, " +
								"this will also define local weighting of the two color maps.\n" +
								"\n" +
								"<size=15>" + LStyle.hexColors [2] + "Override :</color></size>\n" +
								"  Override is used for combining two color maps where one (the lower input) should be prioritized over the other (the middle). \n" +
								"If the prioritized value is larger than the other signal, the other signal is overridden. \n";
					
				}
		
				public override void Calculate ()
				{
						//base.Calculate();
			
						if (!verified)
								return;
			
						if (!changed) 
								return;
			
			
						CTextureNode combiner = (CTextureNode)getNode (Inputs, 0);
						CTextureNode m1 = (CTextureNode)getNode (Inputs, 1);
						CTextureNode m2 = (CTextureNode)getNode (Inputs, 2);
						C2DMap combineMap = null;
			
						if (combiner != null) {
								combiner.Calculate ();
								combineMap = combiner.map;
						}
						if (m1 == null || m2 == null) {
								verified = false;
								return;
						}
						m1.Calculate ();						
						m2.Calculate ();						
			
//			map.Combine(m1.map, m2.map, combineMap, getValue("blendval"), Type);
						colors.Combine (m1.colors, m2.colors, combineMap, getValue ("blendval"), Type);
						updateTexture = true;
			
						colors.Scale (getValue ("amplitude"));
						//map.ScaleMap(getValue("amplitude"),0);
						//GenerateHeightTexture();
						CNodeManager.Progress ();
						changed = false;
			
				}
		
				public override void Verify ()
				{
						verified = true;
						int cnt = 0;
						foreach (CConnection c in Inputs) {
								if (c.pointer == null)
										verified = false;
								if (cnt == 0)
										verified = true;
								cnt++;
						}			
						setupAlternativeNames ();
				}
		
				public void setupParameters ()
				{
						parameters ["amplitude"] = new Parameter ("Amplitude:", 1.0f, 0, 2f, 0, "amplitude");
						parameters ["blendval"] = new Parameter ("Blendval:", 0.5f, 0f, 1, 1, "blendval");
			
			
				}
		
		}
}