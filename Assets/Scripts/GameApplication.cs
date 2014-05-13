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

	public static GameObject PlayEffect(string path, Transform transform)
	{
		var prefab = Resources.Load(path);
		if (prefab == null)
			return null;
		var effect = GameObject.Instantiate(prefab) as GameObject;
		if (effect == null)
			return null;
		effect.name = "Effect." + transform.name;
		effect.transform.localPosition = transform.localPosition;
		effect.AddComponent<ParticleParentAutoDestroy>();
		return effect;
	}
}
