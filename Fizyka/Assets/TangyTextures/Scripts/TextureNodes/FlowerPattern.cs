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
		public class FlowerPattern : CTextureNode
		{
		
				public static int TYPE_FLOWER = 0;
				public static int TYPE_GRASS = 1;
		
				public override void Initialize (int windowID, int type, int x, int y)
				{
			
						status_after_click = CNodeManager.STATUS_NONE;
						InitializeWindow (windowID, type, x, y, "FlowerPattern");		
						map = new C2DMap ();
						setupParameters ();
						colors = new C2DRGBMap ();
						displayTypes = false;
						color = LStyle.Colors [1] * 2;
						color.b *= 0.5f;
						displayTypes = false;
						Outputs.Add (new CConnection (this, 0, CConnection.TYPE0));
						Outputs.Add (new CConnection (this, 1, CConnection.TYPE0));
						Outputs.Add (new CConnection (this, 2, CConnection.TYPE0));
						types = new string[] {"FlowerPattern", "Grass"};
						texture = new Texture2D (C2DMap.sizeX, C2DMap.sizeY);						
						if (Type == TYPE_FLOWER) {
								helpMessage = "<size=24>" + LStyle.hexColors [1] + "Flower pattern generator. </color></size>" +
										"\n" +
										"\n" +
										"This node uses random walkers to generate height map with a random plant-like pattern." +
										"\n" +
										"<size=15>" + LStyle.hexColors [2] + "Parameters:</color></size>\n" +
										"  " + LStyle.hexColors [1] + "Amplitude</color>: Controls the amplitude of the overall signal. \n" +
										"  " + LStyle.hexColors [1] + "Thickness</color>: Thickness of the lines drawn. Thicker branches also grow longer. \n" + 
										"  " + LStyle.hexColors [1] + "Levels</color>: The amount of branches. \n" + 
										"  " + LStyle.hexColors [1] + "Angle</color>: Initial bending angle of the first branch. \n" + 
										"  " + LStyle.hexColors [1] + "Anglescale</color>: Amplitude of the angles of sub-branches. Larger value yields more curls. \n" + 
										"  " + LStyle.hexColors [1] + "Size</color>: Length of branches, controls overall size of pattern. \n" + 
										"  " + LStyle.hexColors [1] + "Seed</color>: Initial random seed. Change to get a new pattern with the same properties. \n";
						}
						if (Type == TYPE_GRASS) {
								helpMessage = "<size=24>" + LStyle.hexColors [1] + "Grass pattern generator. </color></size>" +
										"\n" +
										"\n" +
										"This node uses random walkers to generate height map with a pattern of many small random plant-like figures." +
										"\n" +
										"<size=15>" + LStyle.hexColors [2] + "Parameters:</color></size>\n" +
										"  " + LStyle.hexColors [1] + "Amplitude</color>: Controls the amplitude of the overall signal. \n" +
										"  " + LStyle.hexColors [1] + "Thickness</color>: Thickness of the lines drawn. Thicker branches also grow longer. \n" + 
										"  " + LStyle.hexColors [1] + "Amount</color>: The amount (density) of plant-like figures. \n" + 
										"  " + LStyle.hexColors [1] + "Angle</color>: Initial bending angle of the first branch. \n" + 
										"  " + LStyle.hexColors [1] + "Anglescale</color>: Amplitude of the angles of sub-branches. Larger value yields more curls. \n" + 
										"  " + LStyle.hexColors [1] + "Size</color>: Length of branches, controls overall size of pattern. \n" + 
										"  " + LStyle.hexColors [1] + "Seed</color>: Initial random seed. Change to get a new pattern with the same properties. \n";
						}
				}
		
				public override void Calculate ()
				{
						base.Calculate ();
			
						if (!verified)
								return;
			
						if (!changed) 
								return;
			
						//map.Snowflake((int)getValue("levels"), getValue("size"));				
						if (Type == TYPE_FLOWER)
								map.FlowerPattern ((int)getValue ("levels"), getValue ("size"), getValue ("angle"), getValue ("seed"), getValue ("thickness"), getValue ("anglescale"));
						if (Type == TYPE_GRASS)
								map.GrassPattern ((int)getValue ("amount"), getValue ("size"), getValue ("angle"), getValue ("seed"), getValue ("thickness"), getValue ("anglescale"));				
			
						map.ScaleMap (getValue ("amplitude"), 0);
						GenerateHeightTexture ();
						CNodeManager.Progress ();
						updateTexture = true;
			
						changed = false;
				}
		
				public void setupParameters ()
				{
						if (Type == TYPE_FLOWER) {
								parameters ["amplitude"] = new Parameter ("Amplitude:", 1.0f, 0, 1, 0, "amplitude");
								parameters ["thickness"] = new Parameter ("Thickness:", 1.0f, 0, 3, 1, "thickness");
								parameters ["levels"] = new Parameter ("Levels:", 1.0f, 1, 10, 2, "levels");
								parameters ["angle"] = new Parameter ("Angle:", 0.05f, -Mathf.PI, Mathf.PI, 3, "angle");
								parameters ["anglescale"] = new Parameter ("Anglescale:", 1.0f, -4, 4, 4, "anglescale");
								parameters ["size"] = new Parameter ("Size:", 0.05f, 0.01f, 2, 5, "size");
								parameters ["seed"] = new Parameter ("Seed:", 0.0f, 0.0f, 10, 6, "seed");
						}
						if (Type == TYPE_GRASS) {
								parameters ["amplitude"] = new Parameter ("Amplitude:", 1.0f, 0, 1, 0, "amplitude");
								parameters ["thickness"] = new Parameter ("Thickness:", 1.0f, 0, 3, 1, "thickness");
								parameters ["amount"] = new Parameter ("Amount:", 50.0f, 1, 250, 2, "amount");
								parameters ["angle"] = new Parameter ("Angle:", 0.05f, -Mathf.PI, Mathf.PI, 3, "angle");
								parameters ["anglescale"] = new Parameter ("Anglescale:", 1.0f, -4, 4, 4, "anglescale");
								parameters ["size"] = new Parameter ("Size:", 0.05f, 0.01f, 2, 5, "size");
								parameters ["seed"] = new Parameter ("Seed:", 0.0f, 0.0f, 10, 6, "seed");
						}
			
				}
		
		
		}
}