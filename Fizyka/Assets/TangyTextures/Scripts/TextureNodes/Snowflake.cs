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
		public class Snowflake : CTextureNode
		{
		
		
				public override void Initialize (int windowID, int type, int x, int y)
				{
			
						status_after_click = CNodeManager.STATUS_NONE;
						InitializeWindow (windowID, type, x, y, "Snowflake");		
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
						types = new string[] {"Snowflake"};
						texture = new Texture2D (C2DMap.sizeX, C2DMap.sizeY);						
			
				}
		
				public override void Calculate ()
				{
						base.Calculate ();
			
						if (!verified)
								return;
			
						if (!changed) 
								return;
			
						map.Snowflake ((int)getValue ("levels"), getValue ("size"));				
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
						parameters ["size"] = new Parameter ("Size:", 0.05f, 0.01f, 2, 4, "size");
						/*	parameters ["string1"] = new Parameter ("stringr", 1.0f, 0.1f, 15,5,"string1");
			parameters ["stringcase1"] = new Parameter ("stringr", 0.1f, 0, 0.3f,6,"stringcase1");
			parameters ["stringrule1"] = new Parameter ("stringrr", 0.1f, 0, 0.3f,7,"stringrule1");
			parameters ["stringcase2"] = new Parameter ("stringl", 0.1f, 0, 0.3f,8,"stringcase2");
			parameters ["stringrule2"] = new Parameter ("stringr[l]l", 0.1f, 0, 0.3f,9,"stringrule2");
		*/	
				}
		
		
		
		}
}