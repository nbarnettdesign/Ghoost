using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
	public GameObject spirit;
	public GameObject lastTrigger;

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			spirit.gameObject.SetActive (true);
			lastTrigger.gameObject.SetActive (true);

		}
	}

}
