using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class PlayerInput : MonoBehaviour {

	private PlayerController player;

	// Use this for initialization
	void Start () {
		player = GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update () {
		Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		this.player.SetDirectionalInput(directionalInput);

		if (Input.GetButtonDown("Jump")) {
			this.player.OnJumpInputDown();
		}

		if (Input.GetButtonUp("Jump")) {
			this.player.OnJumpInputUp();
		}

		if (Input.GetButtonDown("Action")) {
			this.player.OnActionInputDown();
		}
	}
}
