using UnityEngine;
using System.Collections;

public class ParticleAutoDestroy : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		{ if (!particleSystem.IsAlive()) Destroy (gameObject); }
	}
}
