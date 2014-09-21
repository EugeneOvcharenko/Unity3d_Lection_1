using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TetrisBlockTemplate
{
	public	bool[] template;
}

public class TetrisBlockCreator : MonoBehaviour {

	public	TetrisGrid	Grid;
	public	GameObject	blockPrefab;
	public	float		createTime	= 3f;
	public	float		fallTime	= 1f;

	public	GameObject	blockRootPrefab;
	public	GameObject	blockSinglePrefab;

	public	TetrisBlockTemplate[] templates;

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

	void CreateSimpleBlock()
	{
		GameObject block = Instantiate (blockPrefab) as GameObject;
		TetrisBlockPlacement p = block.GetComponent<TetrisBlockPlacement> ();
		blocks.Add (p);
		p.Init (this, Grid, 0);
	}

	Color GetRandomColor()
	{
		return new Color( Random.value, Random.value, Random.value, 1f );
	}

	void CreateComplexBlock()
	{
		GameObject root = Instantiate( blockRootPrefab ) as GameObject;

		int i;
		Vector3 pos = Vector3.zero;
		Color c = GetRandomColor();

		int tr = Random.Range( 0, templates.Length );
		TetrisBlockTemplate t = templates[ tr ];

		int maskLength = t.template.Length / 2;
		
		if ( maskLength < 1 )
		{
			Destroy( root );
			return;
		}

		int max_x = 0;
		for ( i = 0; i < t.template.Length; i++ )
		{
			if ( t.template[ i ] )
			{
				GameObject block = Instantiate( blockSinglePrefab ) as GameObject;
				block.transform.parent = root.transform;
				block.transform.localScale = Vector3.one;

				block.transform.localPosition = pos;
				block.renderer.material.color = c;

				max_x = Mathf.Max( max_x, (int)pos.x + 1 );
			}

			if ( i == maskLength - 1 )
			{
				pos.y += 1;
				pos.x  = 0f;
			}
			else
			{
				pos.x += 1;
			}
		}

		TetrisBlockPlacement p = root.GetComponent<TetrisBlockPlacement> ();
		blocks.Add (p);
		p.Init (this, Grid, max_x);
	}
	
	IEnumerator CreateBlock () {
		while ( true )
		{
			CreateComplexBlock ();

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