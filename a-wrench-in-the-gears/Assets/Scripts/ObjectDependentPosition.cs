using UnityEngine;

public class ObjectDependentPosition : MonoBehaviour {

	public GameObject refObject;
	public float smoothingTime;
	public bool horizontalDependent;
	public bool verticalDependent;
	public float horizontalOffset;
	public float verticalOffset;
	public float progressiveHorizontalOffset;
	public float progressiveVerticalOffset;

	private Vector3 velocity = Vector3.zero;

	private void Start () {
		this.updatePosition(0);
	}

	private void Update () {
		this.updatePosition(this.smoothingTime);
	}

	private void updatePosition (float smoothing) {
		if (!this.refObject) return;
		Vector3 refPosition = this.refObject.transform.position;
		Vector3 previousPosition = this.transform.position;
		Vector3 newPosition = previousPosition;
		if (this.horizontalDependent) {
			newPosition.x = refPosition.x + refPosition.x * progressiveHorizontalOffset;
			newPosition.x += horizontalOffset;
		}
		if (this.verticalDependent) {
			newPosition.y = refPosition.y + refPosition.x * progressiveVerticalOffset;
			newPosition.y += verticalOffset;
		}

		this.transform.position = Vector3.SmoothDamp(previousPosition, newPosition, ref this.velocity, smoothing);
	}
}
