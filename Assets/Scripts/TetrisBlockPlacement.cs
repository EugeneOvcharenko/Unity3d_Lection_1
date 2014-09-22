using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TetrisBlockPlacement : MonoBehaviour {

	public	TetrisGrid			Grid;
	public	TetrisBlockCreator	creator;
	public	int					X;
	public	int					Y;
	public	int					W;
	public	bool				falling		= true;

	public	bool				checkLeft	= false;
	public	bool				checkRight	= false;

	public	TetrisBlockTemplate	shape;

	void Start()
	{
		if ( Grid == null )
		{
			Grid = FindObjectOfType<TetrisGrid>();
		}
	}

	public void Init( TetrisBlockCreator c, TetrisGrid g, int set_w )
	{
		Grid = g;
		creator = c;
		X = g.GetStartX();
		Y = g.GetStartY();
		W = set_w;

		if ( g.countX < X + W )
		{
			X -= W;
		}

		Update();
	}

	void CheckLeft ()
	{
		X--;
		X = Grid.CanTakeThisPlace (this, false) ? X : ++X;
		checkLeft = false;
	}

	void CheckRight ()
	{
		X++;
		X = Grid.CanTakeThisPlace (this, false) ? X : --X;
		checkRight = false;
	}

	public void Fall()
	{
		if ( falling && Grid != null )
		{
			if ( checkLeft )
			{
				CheckLeft ();
			}

			if ( checkRight )
			{
				CheckRight ();
			}

			Y--;
			if ( !Grid.CanTakeThisPlace( this, false ) )
			{
				//return place
				Y++;
				Grid.CanTakeThisPlace( this, true );
				falling = false;
			}
		}
	}

	void OnDestroy()
	{
		if ( creator != null )
		{
			creator.BlockDied( this );
		}
	}

	public void Update () {

		if ( falling && Grid != null )
		{
			checkLeft	= checkLeft  || Input.GetKey( KeyCode.LeftArrow );
			checkRight	= checkRight || Input.GetKey( KeyCode.RightArrow );
		
			if ( Input.GetKeyUp( KeyCode.LeftArrow ) )	CheckLeft();
			if ( Input.GetKeyUp( KeyCode.RightArrow ) )	CheckRight();

			transform.localPosition = Grid.GetXY( X, Y );
		}
	}
}