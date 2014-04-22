using UnityEngine;
using System.Collections;
using System;

public delegate void ValueChanged<T>(T last, T cur);

public enum GameState
{
	None,
}

public static class GameApplication
{
	private static GameState gameState;
	public static GameState GameState
	{
		get { return gameState; }
		set
		{
			if (gameState == value)
				return;
			var last = gameState;
			gameState = value;
			if (GameStateChanged != null)
				GameStateChanged(last, gameState);
		}
	}
	public static event ValueChanged<GameState> GameStateChanged;
}
