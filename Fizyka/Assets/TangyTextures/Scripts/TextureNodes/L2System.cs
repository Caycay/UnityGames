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
		public class L2System : CTextureNode
		{
		
				public static int TYPE_GRID = 0;
				public static int TYPE_REGULARDOTS = 1;
				public static int TYPE_BRICKS = 2;
		
				public override void Initialize (int windowID, int type, int x, int y)
				{
			
						status_after_click = CNodeManager.STATUS_NONE;
						InitializeWindow (windowID, type, x, y, "L2System");		
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
						types = new string[] {"L2 System"};
						texture = new Texture2D (C2DMap.sizeX, C2DMap.sizeY);						
			
				}
		
				public override void Calculate ()
				{
						base.Calculate ();
			
						if (!verified)
								return;
			
						if (!changed) 
								return;
			
						
						//map.Normalize(getValue("amplitude"));
						map.L2System (getString ("string1"), getString ("stringcase1"), getString ("stringrule1"), getString ("stringcase2"), getString ("stringrule2"), 
				(int)getValue ("levels"), getValue ("amplitude"), getValue ("thickness"), getValue ("angle"), getValue ("size"));
						//map.ScaleMap(getValue("amplitude"),0);
						GenerateHeightTexture ();
						CNodeManager.Progress ();
						updateTexture = true;
			
						changed = false;
				}
		
				public void setupParameters ()
				{
						parameters ["amplitude"] = new Parameter ("Amplitude:", 1.0f, 0, 1, 0, "amplitude");
						parameters ["thickness"] = new Parameter ("Thickness:", 2.0f, 0, 20, 1, "thickness");
						parameters ["levels"] = new Parameter ("Levels:", 1.0f, 1, 10, 2, "levels");
						parameters ["angle"] = new Parameter ("Angle:", 0.05f, -Mathf.PI, Mathf.PI, 3, "angle");
						parameters ["size"] = new Parameter ("Size:", 0.05f, 0.001f, 0.2f, 4, "size");
						parameters ["string1"] = new Parameter ("stringr", 1.0f, 0.1f, 15, 5, "string1");
						parameters ["stringcase1"] = new Parameter ("stringr", 0.1f, 0, 0.3f, 6, "stringcase1");
						parameters ["stringrule1"] = new Parameter ("stringrr", 0.1f, 0, 0.3f, 7, "stringrule1");
						parameters ["stringcase2"] = new Parameter ("stringl", 0.1f, 0, 0.3f, 8, "stringcase2");
						parameters ["stringrule2"] = new Parameter ("stringr[l]l", 0.1f, 0, 0.3f, 9, "stringrule2");
			
				}
		
		
		
		}
}