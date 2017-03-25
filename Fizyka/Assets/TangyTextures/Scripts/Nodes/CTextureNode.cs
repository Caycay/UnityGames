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
		public class CTextureNode : CNode
		{

				public Texture2D texture = null;
				public C2DMap map = null;
				public C2DRGBMap colors = null;
				public bool updateTexture = true;
		
				public override void resetMaps ()
				{
						map = new C2DMap ();
						colors = new C2DRGBMap ();
						texture = new Texture2D (C2DMap.sizeX, C2DMap.sizeY);
						changed = true;
				}
	
				public override void Draw (int ID)
				{
						base.Draw (ID);
			
						buildGUI (types [Type]);
						renderPresets ();
		
					
						GUI.DragWindow ();
						GUI.color = Color.white;
						int d = 12;
						int sz = (int)(window.width / 2 - 2 * d);
						if (texture != null)  
								GUI.DrawTexture (new Rect (rightSize.x + d + 3, 7 + d, sz, sz), texture);
/*			for (int i=0;i<2;i++)
				for (int j=0;j<2;j++)
						GUI.DrawTexture (new Rect (rightSize.x + d+3 + i*window.width/4f,  j*window.width/4, window.width/4, window.width/4), texture);
*/			
				
						//size.y += window.width;
						window.height = Mathf.Max (size.y, sz + 2 * d);
			
						ExtraOnGUI ();				
				
				}
		
				protected virtual void ExtraOnGUI ()
				{
		
				}
		
				protected void GenerateHeightTexture ()
				{
						Color c = new Color (1, 1, 1);
						for (int i=0; i<C2DMap.sizeX; i++) 
								for (int j=0; j<C2DMap.sizeX; j++) {
										float h = map [i, j];
										c.r = h;//*color.r;
										c.g = h;//*color.g;
										c.b = h;//*color.b;
										c.a = h;
										colors.colors [i, j] = c;
								} 
				}
		
				public virtual void GenerateTexture ()
				{
						foreach (CConnection ct in Inputs) {
								if (ct.pointer != null)
								if (ct.pointer.parent != null)
										((CTextureNode)ct.pointer.parent).GenerateTexture ();
						}
						CNodeManager.Progress ();
						if (!updateTexture)
								return;
						colors.toTexture (texture);
						texture.wrapMode = TextureWrapMode.Repeat;
						texture.Apply ();
						updateTexture = false;
				}
	
				// Use this for initialization
		}

}