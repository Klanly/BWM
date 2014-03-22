using UnityEngine;
using System.Collections;

public class ParticleParentAutoDestroy : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(GetComponentsInChildren<ParticleSystem>().Length == 0)
			Destroy(gameObject);
	}
}
