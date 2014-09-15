using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RenderGL : MonoBehaviour {

	public	TetrisGrid[] Grids;

	void OnPostRender () {
		if ( Grids != null )
		{
			GL.InvalidateState();
			foreach ( TetrisGrid g in Grids )
			{
				if ( g != null )
				{
					g.RenderGL();
				}
			}
		}
	}
}