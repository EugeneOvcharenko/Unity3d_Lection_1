using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TetrisGrid : MonoBehaviour {

	public	Material	lineMat;
	public	int			countX		= 8;
	public	int			countY		= 16;
	public	int			blockSize	= 64;
	public	Transform	trans;
	public	Color		lineColor;

	public	int			highlightX	= 0;
	public	int			highlightY	= 0;

	public	TetrisBlockPlacement block;

	void Start () {
		trans = transform;

		if ( Application.isPlaying )
		{
			if ( block != null )
			{
				Destroy( block.gameObject );
			}
		}
	}
	
	public void RenderGL () {

		float x = trans.localPosition.x;
		float y = trans.localPosition.y;
		float w = blockSize * countX;
		float h = blockSize * countY;
		int i;

		if ( block != null && !Application.isPlaying )
		{
			block.X = highlightX;
			block.Y = highlightY;
			block.Update();
		}

		GL.PushMatrix();
		lineMat.SetPass( 0 );

		GL.Begin( GL.LINES );
		GL.Color( lineColor );

		for ( i = 0; i < countY + 1; i++ )
		{
			GL.Vertex3( x - w / 2.0f, y + i * blockSize - h / 2.0f - blockSize / 2.0f, 0 );
			GL.Vertex3( x + w / 2.0f, y + i * blockSize - h / 2.0f - blockSize / 2.0f, 0 );

			if ( i == highlightY )
			{
				GL.Color( Color.red );
				GL.Vertex3( x - w / 2.0f, y + i * blockSize - h / 2.0f, 0 );
				GL.Vertex3( x + w / 2.0f, y + i * blockSize - h / 2.0f, 0 );
				GL.Color( lineColor );
			}
		}

		for ( i = 0; i < countX + 1; i++ )
		{
			GL.Vertex3( x + i * blockSize - w / 2.0f, y - h / 2.0f - blockSize / 2.0f, 0 );
			GL.Vertex3( x + i * blockSize - w / 2.0f, y + h / 2.0f - blockSize / 2.0f, 0 );

			if ( i == highlightX )
			{
				GL.Color( Color.blue );
				GL.Vertex3( x + i * blockSize - w / 2.0f + blockSize / 2.0f, y - h / 2.0f - blockSize / 2.0f, 0 );
				GL.Vertex3( x + i * blockSize - w / 2.0f + blockSize / 2.0f, y + h / 2.0f - blockSize / 2.0f, 0 );
				GL.Color( lineColor );
			}
		}

		GL.End();
		GL.PopMatrix();
	}

	public Vector3 GetXY( int X, int Y )
	{
		float x = trans.localPosition.x;
		float y = trans.localPosition.y;
		float w = blockSize * countX;
		float h = blockSize * countY;

		return new Vector3(
			x + X * blockSize - w / 2.0f + blockSize / 2.0f,
			y + Y * blockSize - h / 2.0f,
			0 );
	}

	public int GetStartX()
	{
		return Random.Range( 0, countX );
	}

	public int GetStartY()
	{
		return countY - 1;
	}

	public bool isDead( TetrisBlockPlacement block )
	{
		if ( block != null )
		{
			return block.Y < 0;
		}
		return false;
	}
}