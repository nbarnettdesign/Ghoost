using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {

	public GameObject fade;


	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			fade.gameObject.SetActive (true);


		}
	}
}
