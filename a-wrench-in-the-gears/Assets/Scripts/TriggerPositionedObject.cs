using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public class TriggerPositionedObject : MonoBehaviour {

	public float speed;
	public Vector3 startPosition;
	public Vector3 endPosition;
	public bool active = false;
	public LeverController lever;

	private Vector3 startGlobalPosition;
	private Vector3 endGlobalPosition;
	private UnityAction<bool> statusUpdateActions;

	//
	// Lifecycles
	//

	private void Awake() {
		this.startGlobalPosition = startPosition + transform.position;
		this.endGlobalPosition = endPosition + transform.position;
	}

	private void Start() {
		if (this.lever) {
			this.statusUpdateActions += this.UpdateStatus;
			this.lever.subscribeToStateChange(UpdateStatus);
		}
	}

	private void Update() {
		this.Move();
	}

	//
	// Public
	//

	public void UpdateStatus(bool status) {
		this.active = status;
	}

	//
	// Private
	//

	private void Move() {
		Vector3 targetPosition = this.active ? this.startGlobalPosition : this.endGlobalPosition;

		if (transform.position != targetPosition) {
			// transform.position = targetPosition;
			transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
		}
	}

	private void OnDrawGizmos() {
		if (this.startPosition != null) this.DrawPoint(this.startPosition, .3f, Color.red);
		if (this.endPosition != null) this.DrawPoint(this.endPosition, .3f, Color.yellow);
	}

	private void DrawPoint(Vector3 point, float size, Color color) {
		Gizmos.color = color;
		Vector3 globalPosition = Application.isPlaying ? point : (point + transform.position);
		Gizmos.DrawLine(globalPosition - Vector3.up * size, globalPosition + Vector3.up * size);
		Gizmos.DrawLine(globalPosition - Vector3.left * size, globalPosition + Vector3.left * size);
	}
}
