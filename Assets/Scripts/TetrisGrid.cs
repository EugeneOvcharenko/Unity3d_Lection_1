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

	public	TetrisBlockPlacement	block;
	public	TetrisBlockTemplate		grid;

	void Start () {
		trans = transform;

		if ( Application.isPlaying )
		{
			grid = new TetrisBlockTemplate( countX * countY );

			if ( block != null )
			{
				Destroy( block.gameObject );
			}
		}
	}

	public bool CanTakeThisPlace( TetrisBlockPlacement block, bool takeIt )
	{
		/*
		 *	grid
		 * 	28 29 30 31
		 * 	24 25 26 27
		 * 	20 21 22 23
		 * 	16 17 18 19
		 *	12 13 14 15
		 *	8  9  10 11
		 * 	4  5   6  7
		 * 	0  1   2  3
		 * 
		 * 	block
		 * 	3 4 5
		 * 	0 1 2
		 */

		if ( block.Y < 0 )
		{
			Debug.Log( "false 0" );
			return false;
		}

		int idx = block.X + block.Y * countX;
		bool free = true;
		int c = 0;
		for ( int i = 0; i < block.shape.template.Length; i++ )
		{
			if ( c >= ( block.shape.length ) )
			{
				c = 0;
				idx += countX;
			}

			if ( takeIt )
			{
				if ( block.shape.template[ i ] )
				{
					if ( ( ( idx + c ) < grid.template.Length ) )
					{
						if ( grid.template[ idx + c ] == true )
						{
							Debug.LogWarning( "Block " + ( idx + c ) + " is already taken." );
						}

						grid.template[ idx + c ] = true;
					}
				}
			}
			else
			{
				if ( ( ( idx + c ) < grid.template.Length ) )
				{
					if ( grid.template[ idx + c ] && block.shape.template[ i ] )
					{
						Debug.Log( "false 1" );
						free = false;
						return false;
					}

					if ( block.shape.template[ i ] && block.X < 0 )
					{
						Debug.Log( "false 2" );
						free = false;
						return false;
					}

					if ( block.shape.template[ i ] && ( block.X + block.shape.maxX ) > countX )
					{
						Debug.Log( "false 3" );
						free = false;
						return false;
					}
				}
				else
				{
					Debug.Log( "false 4" );
					return false;
				}
			}
			
			c++;
		}

		return free;
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
		return countY - 2;
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