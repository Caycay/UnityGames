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
		public class PatternGenerator : CTextureNode
		{
		
				public static int TYPE_GRID = 0;
				public static int TYPE_REGULARDOTS = 1;
				public static int TYPE_BRICKS = 2;
		
				public override void Initialize (int windowID, int type, int x, int y)
				{
			
						status_after_click = CNodeManager.STATUS_NONE;
						InitializeWindow (windowID, type, x, y, "Pattern Generator");		
						map = new C2DMap ();
						setupParameters ();
						colors = new C2DRGBMap ();
						color = LStyle.Colors [1] * 2;
						color.b *= 0.5f;
						displayTypes = false;
						Outputs.Add (new CConnection (this, 0, CConnection.TYPE0));
						Outputs.Add (new CConnection (this, 1, CConnection.TYPE0));
						Outputs.Add (new CConnection (this, 2, CConnection.TYPE0));
						types = new string[] {"Grid", "Circles", "Bricks"};
						texture = new Texture2D (C2DMap.sizeX, C2DMap.sizeY);
			
						if (Type == TYPE_GRID) {						
								helpMessage = "<size=24>" + LStyle.hexColors [1] + "Pattern generator: Regular grid. </color></size>" +
										"\n" +
										"\n" +
										"This node generates a height map with regular grids. " +
										"\n" +
										"<size=15>" + LStyle.hexColors [2] + "Parameters:</color></size>\n" +
										"  " + LStyle.hexColors [1] + "Amplitude</color>: Controls the amplitude of the output signal. \n" +
										"  " + LStyle.hexColors [1] + "Thickness</color>: Defines line thickness. \n" + 
										"  " + LStyle.hexColors [1] + "Spacing X/Y</color>: Spacing width in x and y directions. \n" + 
										"  " + LStyle.hexColors [1] + "Rotation</color>: Rotates the grid by the rotation angle. Be careful, breaks seamlessness except for manually tweaked parameters.  \n";
						}
						if (Type == TYPE_REGULARDOTS) {						
								helpMessage = "<size=24>" + LStyle.hexColors [1] + "Pattern generator: Regular circles/dots. </color></size>" +
										"\n" +
										"\n" +
										"This node generates a height map with a regular grid of circles/dots " +
										"\n" +
										"<size=15>" + LStyle.hexColors [2] + "Parameters:</color></size>\n" +
										"  " + LStyle.hexColors [1] + "Amplitude</color>: Controls the amplitude of the output signal. \n" +
										"  " + LStyle.hexColors [1] + "Size</color>: Size of the circles. Increase to yield larger circles. \n" + 
										"  " + LStyle.hexColors [1] + "Filling</color>: Filling threshold of circles. Smaller values fill the inner parts of the circles, larger values creates rings. \n" + 
										"  " + LStyle.hexColors [1] + "Spacing X/Y</color>: Spacing width in x and y directions. \n";
						}
						if (Type == TYPE_BRICKS) {						
								helpMessage = "<size=24>" + LStyle.hexColors [1] + "Pattern generator: Bricks. </color></size>" +
										"\n" +
										"\n" +
										"This node generates a height map of seamless bricks with two types of patterns." +
										"\n" +
										"<size=15>" + LStyle.hexColors [2] + "Parameters:</color></size>\n" +
										"  " + LStyle.hexColors [1] + "Amplitude</color>: Controls the amplitude of the output signal. \n" +
										"  " + LStyle.hexColors [1] + "Thickness</color>: Defines line thickness. \n" + 
										"  " + LStyle.hexColors [1] + "Spacing X/Y</color>: Spacing width in x and y directions. \n" + 
										"  " + LStyle.hexColors [1] + "Type</color>: Brick pattern type. 1 = 'Running bond', 2 = 'Herring bone'. Google them! \n";
						}
			
				}
		
				public override void Calculate ()
				{
						base.Calculate ();
			
						if (!verified)
								return;
			
						if (!changed) 
								return;
				
						if (Type == TYPE_GRID) 
								map.RenderGrid (getValue ("amplitude"), getValue ("thickness"), getValue ("spacingx"), getValue ("spacingy"), getValue ("rotation"));
				
						if (Type == TYPE_REGULARDOTS) 
								map.RenderDots (getValue ("amplitude"), getValue ("size"), getValue ("spacingx"), getValue ("spacingy"), getValue ("filling"));

						if (Type == TYPE_BRICKS) 
								map.RenderBricks (getValue ("amplitude"), getValue ("thickness"), getValue ("spacingx"), getValue ("spacingy"), (int)getValue ("bricktype"));
			
						//map.Normalize(getValue("amplitude"));
						map.ScaleMap (getValue ("amplitude"), 0);
						GenerateHeightTexture ();
						CNodeManager.Progress ();
						updateTexture = true;
			
						changed = false;
				}
		
				public void setupParameters ()
				{
						if (Type == TYPE_GRID) {
								parameters ["amplitude"] = new Parameter ("Amplitude:", 1.0f, 0, 1, 0, "amplitude");
								parameters ["thickness"] = new Parameter ("Thickness:", 1.0f, 0.1f, 15, 1, "thickness");
								parameters ["spacingx"] = new Parameter ("Spacing X:", 0.1f, 0, 0.3f, 2, "spacingx");
								parameters ["spacingy"] = new Parameter ("Spacing Y:", 0.1f, 0, 0.3f, 3, "spacingy");
								parameters ["rotation"] = new Parameter ("Rotation:", 0.0f, 0.0f, 360f, 4, "rotation");
						}

						if (Type == TYPE_REGULARDOTS) {
								parameters ["amplitude"] = new Parameter ("Amplitude:", 1.0f, 0, 1, 0, "amplitude");
								parameters ["size"] = new Parameter ("Size:", 0.1f, 0.01f, 0.5f, 1, "size");
								parameters ["spacingx"] = new Parameter ("Spacing X:", 0.1f, 0, 0.3f, 2, "spacingx");
								parameters ["spacingy"] = new Parameter ("Spacing Y:", 0.1f, 0, 0.3f, 3, "spacingy");
								parameters ["filling"] = new Parameter ("Filling:", 0.0f, 0, 1f, 4, "filling");
						}
						if (Type == TYPE_BRICKS) {
								parameters ["amplitude"] = new Parameter ("Amplitude:", 1.0f, 0, 1, 0, "amplitude");
								parameters ["thickness"] = new Parameter ("Thickness:", 1.0f, 0.1f, 15, 1, "thickness");
								parameters ["spacingx"] = new Parameter ("Spacing X:", 0.1f, 0, 0.6f, 2, "spacingx");
								parameters ["spacingy"] = new Parameter ("Spacing Y:", 0.1f, 0, 0.6f, 3, "spacingy");
								parameters ["bricktype"] = new Parameter ("Type:", 1f, 1, 2, 4, "bricktype");
						}
			
			
				}
		
				protected override void ExtraOnGUI ()
				{
						base.ExtraOnGUI ();
						if (Type == TYPE_BRICKS) {
								setValue ("bricktype", (int)getValue ("bricktype"));
						}
				}
		
		}
}