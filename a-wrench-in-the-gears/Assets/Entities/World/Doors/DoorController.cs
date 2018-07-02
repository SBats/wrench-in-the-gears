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
		this.updateChildrenStatus(status);
		this.animator.SetBool("open", status);
	}

	private void updateChildrenStatus(bool active) {
		//Assuming parent is the parent game object
		for(int i=0; i< this.transform.childCount; i++) {
			var child = this.transform.GetChild(i).gameObject;
			if(child != null) child.SetActive(active);
		}
	}
}
