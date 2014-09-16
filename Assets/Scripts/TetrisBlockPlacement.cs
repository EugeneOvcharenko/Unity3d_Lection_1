﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class TetrisBlockPlacement : MonoBehaviour {

	public	TetrisGrid			Grid;
	public	TetrisBlockCreator	creator;
	public	int					X;
	public	int					Y;


	void Start()
	{
		if ( Grid == null )
		{
			Grid = FindObjectOfType<TetrisGrid>();
		}
	}

	public void Init( TetrisBlockCreator c, TetrisGrid g )
	{
		Grid = g;
		creator = c;
		X = g.GetStartX();
		Y = g.GetStartY();
		Update();
	}

	public void Fall()
	{
		Y--;
		if ( Grid != null )
		{
			if ( Grid.isDead( this ) )
			{
				Destroy( gameObject );
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
	
		if ( Grid != null )
		{
			transform.localPosition = Grid.GetXY( X, Y );
		}
	}
}