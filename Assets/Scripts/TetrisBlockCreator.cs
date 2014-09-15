using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TetrisBlockCreator : MonoBehaviour {

	public	TetrisGrid	Grid;
	public	GameObject	blockPrefab;
	public	float		createTime	= 3f;
	public	float		fallTime	= 1f;

	private	List<TetrisBlockPlacement> blocks;

	void Start () {
		blocks = new List<TetrisBlockPlacement>();
		StartCoroutine( CreateBlock() );
		StartCoroutine( Fall() );
	}

	public void BlockDied( TetrisBlockPlacement b )
	{
		blocks.Remove( b );
	}
	
	IEnumerator CreateBlock () {
		while ( true )
		{
			GameObject block = Instantiate( blockPrefab ) as GameObject;
			TetrisBlockPlacement p = block.GetComponent<TetrisBlockPlacement>();
			blocks.Add( p );
			p.Init( this, Grid );

			yield return new WaitForSeconds( createTime );
		}
	}

	IEnumerator Fall()
	{
		while ( true )
		{
			foreach ( TetrisBlockPlacement b in blocks )
			{
				b.Fall();
				Grid.highlightX = b.X;
				Grid.highlightY = b.Y;
			}
			yield return new WaitForSeconds( fallTime );
		}
	}
}