using UnityEngine.EventSystems;
using UnityEngine;
using Rewired;

public class SelectOnInput : MonoBehaviour {

	public EventSystem eventSystem;
	public GameObject selectedObject;

	private bool buttonSelected;

	private int rewiredId = 0;
	private Player rewiredPlayer;

	private void Awake() {
		this.rewiredPlayer = ReInput.players.GetPlayer(this.rewiredId);
	}

	private void Update () {
		float directionalInput = this.rewiredPlayer.GetAxis("Vertical");

		if (directionalInput != 0 && !buttonSelected) {
			this.eventSystem.SetSelectedGameObject(selectedObject);
			this.buttonSelected = true;
		}
	}

	private void OnDisable() {
		this.buttonSelected = false;
	}
}
