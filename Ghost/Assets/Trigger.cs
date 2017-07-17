using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {
	public GameObject text1;
	public GameObject text2;
	public GameObject text3;
	public GameObject text4;
	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			text1.gameObject.SetActive (true);
			text2.gameObject.SetActive (false);
			text3.gameObject.SetActive (false);
			text4.gameObject.SetActive (false);
		}
	}

}
