using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent (typeof (PlayerController))]
public class PlayerInput : MonoBehaviour {

	private PlayerController player;
	private int rewiredId = 0;
	private Player rewiredPlayer;

	// Use this for initialization
	void Start () {
		this.rewiredPlayer = ReInput.players.GetPlayer(this.rewiredId);
		this.player = GetComponent<PlayerController>();
	}

	// Update is called once per frame
	void Update () {
		Vector2 directionalInput = new Vector2(this.rewiredPlayer.GetAxis("Horizontal"), this.rewiredPlayer.GetAxis("Vertical"));
		this.player.SetDirectionalInput(directionalInput);

		if (this.rewiredPlayer.GetButtonDown("Jump")) {
			this.player.OnJumpInputDown();
		}

		if (this.rewiredPlayer.GetButtonUp("Jump")) {
			this.player.OnJumpInputUp();
		}

		if (this.rewiredPlayer.GetButtonDown("Action")) {
			this.player.OnActionInputDown();
		}
	}
}
