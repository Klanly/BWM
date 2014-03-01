using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MapNav : MonoBehaviour {

	public float gridWidth = 1.0f;
	public float gridHeight = 1.0f;
	public int gridXNum = 0;
	public int gridZNum = 0;
	public bool showGrids = false;

	public uint[] grids;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Creates a new grid of tile nodes of x by y count
	/// </summary>
	public void Reset()
	{
		grids = new uint[gridZNum * gridXNum];
		for(int i = 0; i < grids.Length; ++i)
			grids[i] = 0;
	}

	// get a grid at index of grids
	public uint this[int _z, int _x]
	{ 
		get 
		{
			if(grids == null) return 0;
			if(_z < 0 && _z > gridZNum) return 0;
			if(_x < 0 && _x > gridXNum) return 0;
			return grids[_z * gridZNum + _x];
		} 

		set
		{
			if(grids == null) return;
			if(_z < 0 && _z > gridZNum) return;
			if(_x < 0 && _x > gridXNum) return;
			grids[_z * gridZNum + _x] = value;
		}
	}
	
	// shortcut to getting the length of grids.
	public int Length 
	{ 
		get 
		{
			if (grids == null) return 0;
			return grids.Length; 
		} 
	}

	void OnDrawGizmos() {
		if(showGrids)
		{
			float y = 0.2f;
			for(int z = 0; z < gridZNum; ++z)
			{
				Gizmos.color = new Color(0, 0, 0, 0.5F);
				Gizmos.DrawLine(new Vector3(0, y, z * gridHeight), new Vector3(gridXNum * gridWidth, y, z * gridHeight));
			}

			for(int x = 0; x < gridXNum; ++x)
			{
				Gizmos.color = new Color(0, 0, 0, 0.5F);
				Gizmos.DrawLine(new Vector3(x * gridWidth, y, 0), new Vector3(x * gridWidth, y, gridZNum * gridHeight));
			}

			for(int z = 0; z < gridZNum; ++z)
			{
				for(int x = 0; x < gridXNum; ++x)
				{
					Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5F);
					Vector3 center = new Vector3(x * gridWidth + gridWidth * 0.5f, y, z * gridHeight + gridHeight * 0.5f);
					Vector3 size = new Vector3(gridWidth, 0, gridHeight);
					Gizmos.DrawCube(center, size);
				}
			}
		}
	}

	
}
