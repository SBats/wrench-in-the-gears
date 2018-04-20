using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public CheckpointController start;
	public CheckpointController end;
	public List<CheckpointController> checkpoints;
	public PlayerController player;

	// Use this for initialization
	void Awake () {
		this.InitCheckpoints();
		this.InitPlayer();
	}

	private void InitPlayer() {
		this.player.UpdateRespawnPoint(this.start);
		this.player.Respawn();
		this.start.Disable();
		this.player.gameObject.SetActive(true);
	}

	private void InitCheckpoints() {
		this.checkpoints.ForEach(checkpoint => checkpoint.Enable());
	}
}
