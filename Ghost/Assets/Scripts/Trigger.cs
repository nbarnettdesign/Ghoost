using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
	public GameObject spirit;
	public GameObject lastTrigger;
	public GameObject lamp;

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			spirit.gameObject.SetActive (true);
			lastTrigger.gameObject.SetActive (true);
			lamp.GetComponent<Animator> ().Play ("evil");

		}
	}

}
