using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatIn : MonoBehaviour {

	public GameObject player;
	public float startMoving = 2f;


	void Start () {
		Invoke ("Move", startMoving);
	}
	private void Move(){
		player.GetComponent<PlayerController> ().enabled = true;
	}
}
