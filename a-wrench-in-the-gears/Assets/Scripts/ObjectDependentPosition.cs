using UnityEngine;

public class ObjectDependentPosition : MonoBehaviour {

	public GameObject refObject;
	public bool reverseX;
	public bool reverseY;
	public bool horizontalDependent;
	public bool verticalDependent;
	public float horizontalOffset;
	public float verticalOffset;
	public float progressiveHorizontalOffset;
	public float progressiveVerticalOffset;

	private void Start () {
		this.updatePosition(false);
	}

	private void LateUpdate () {
		this.updatePosition();
	}

	private void updatePosition (bool smooth = true) {
		if (!this.refObject) return;
		Vector3 refPosition = this.refObject.transform.position;
		Vector3 previousPosition = this.transform.position;
		Vector3 newPosition = previousPosition;
		float reverseXFactor = this.reverseX ? -1 : 1;
		float reverseYFactor = this.reverseY ? -1 : 1;
		if (this.horizontalDependent) {
			newPosition.x = refPosition.x + refPosition.x * progressiveHorizontalOffset * reverseXFactor;
			newPosition.x += horizontalOffset * reverseXFactor;
		}
		if (this.verticalDependent) {
			newPosition.y = refPosition.y + refPosition.x * progressiveVerticalOffset * reverseYFactor;
			newPosition.y += verticalOffset * reverseYFactor;
		}

		if (smooth) {
			transform.Translate(newPosition - previousPosition);
		} else {
			transform.position = newPosition;
		}
	}
}
