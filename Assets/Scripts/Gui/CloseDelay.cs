using UnityEngine;
using System.Collections;

public class CloseDelay : MonoBehaviour {

	public float delay = 1.0f;

	void Start() {
		StartCoroutine(WaitAndClose(delay));
	}

	IEnumerator WaitAndClose(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		Destroy(gameObject);
	}
	
}
