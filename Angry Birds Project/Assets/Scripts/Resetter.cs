﻿using UnityEngine;
using System.Collections;

public class Resetter : MonoBehaviour {

	public Rigidbody2D projectile;
	public float resetSpeed = 0.025f;
	private float resetSpeedSqr;
	private SpringJoint2D spring;

	// Use this for initialization
	void Start () {
		resetSpeedSqr = resetSpeed * resetSpeed;
		spring = projectile.GetComponent <SpringJoint2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)){
			Reset();
		}

		if (spring == null && projectile.velocity.sqrMagnitude < resetSpeedSqr) {
			Reset();
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.rigidbody2D == projectile) {
			Reset ();
		}
	}

	void Reset () {
		Application.LoadLevel (Application.loadedLevel);
	}
}
