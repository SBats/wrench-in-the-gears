using UnityEngine;
using UnityEngine.Events;

public class DoorController : MonoBehaviour {

	public bool open = false;
	public LeverController lever;

	private UnityAction<bool> statusUpdateActions;
	private Animator animator;

	private void Awake() {
		this.animator = GetComponent<Animator>();
	}

	private void Start() {
		this.UpdateStatus(this.open);
		if (this.lever) {
			this.statusUpdateActions += this.UpdateStatus;
			this.lever.subscribeToStateChange(UpdateStatus);
		}
	}

	public void UpdateStatus(bool status) {
		this.open = status;
		this.animator.SetBool("open", status);
	}
}
