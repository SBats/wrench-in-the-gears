using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransporterObject : RaycastController {
	public LayerMask passengerMask;

	protected Vector3 _oldPosition;
	private List<PassengerMovement> _passengerMovement;
	private Dictionary<Transform, Controller2D> _passengerDictionary = new Dictionary<Transform, Controller2D>();

	private struct PassengerMovement {
		public Transform transform;
		public Vector3 velocity;
		public bool standingOnPlatform;
		public bool moveBeforePlatform;

		public PassengerMovement(
			Transform _transform,
			Vector3 _velocity,
			bool _standingOnPlatform,
			bool _moveBeforePlatform
		) {
			transform = _transform;
			velocity = _velocity;
			standingOnPlatform = _standingOnPlatform;
			moveBeforePlatform = _moveBeforePlatform;
		}
	}

	//
	// Lifecycles
	//

	protected override void Awake() {
		base.Awake();
	}

	public void Update() {
		Vector3 velocity = this.transform.position - this._oldPosition;
		this.UpdateRaycastOrigins();
		this._CalculatePassengerMovement(velocity);

		this._MovePassengers(true);
		this._oldPosition = transform.position;
		this._MovePassengers(false);
	}

	//
	// Private
	//

	private void _MovePassengers(bool beforeMovePlatform) {
		foreach (PassengerMovement passenger in this._passengerMovement) {
			Debug.Log(passenger);
			if (!this._passengerDictionary.ContainsKey(passenger.transform)) {
				this._passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
			}
			if (passenger.moveBeforePlatform == beforeMovePlatform) {
				this._passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.standingOnPlatform);
			}
		}
	}

	private void _CalculatePassengerMovement(Vector3 velocity) {
		HashSet<Transform> movedPassenger = new HashSet<Transform>();
		this._passengerMovement = new List<PassengerMovement>();

		float directionX = Mathf.Sign(velocity.x);
		float directionY = Mathf.Sign(velocity.y);

		// Vertically moving platform
		if (velocity.y != 0) {
			float rayLength = Mathf.Abs(velocity.y) + this.skinWidth;

			for (int i = 0; i < this.verticalRayCount; i++) {
				Vector2 rayOrigin = (directionY == -1) ? this.raycastOrigins.bottomLeft : this.raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (this.verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, this.passengerMask);
				Debug.DrawLine(rayOrigin, rayOrigin + Vector2.up * directionY, Color.red);

				if (hit && hit.distance != 0) {
					if (!movedPassenger.Contains(hit.transform)) {
						movedPassenger.Add(hit.transform);
						float pushX = (directionY == 1) ? velocity.x : 0;
						float pushY = velocity.y - (hit.distance - this.skinWidth) * directionY;

						this._passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
					}
				}
			}
		}

		// Horizontally moving platform
		if (velocity.x != 0) {
			float rayLength = Mathf.Abs(velocity.x) + this.skinWidth;

			for (int i = 0; i < this.horizontalRayCount; i++) {
				Vector2 rayOrigin = (directionX == -1) ? this.raycastOrigins.bottomLeft : this.raycastOrigins.bottomRight;
				rayOrigin += Vector2.up * (this.horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, this.passengerMask);
				Debug.DrawLine(rayOrigin, rayOrigin + Vector2.right * directionX, Color.blue);

				if (hit && hit.distance != 0) {
					if (!movedPassenger.Contains(hit.transform)) {
						movedPassenger.Add(hit.transform);
						float pushX = velocity.x - (hit.distance - this.skinWidth) * directionX;
						float pushY = -this.skinWidth;

						this._passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
					}
				}
			}
		}

		// Passenger on top of horizontally or downward moving platform
		if (directionY == -1 || velocity.y == 0 && velocity.x != 0) {
			float rayLength = this.skinWidth * 2;

			for (int i = 0; i < this.verticalRayCount; i++) {
				Vector2 rayOrigin = this.raycastOrigins.topLeft + Vector2.right * (this.verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, this.passengerMask);
				Debug.DrawLine(rayOrigin, rayOrigin + Vector2.up * rayLength, Color.green);

				if (hit && hit.distance != 0) {
					if (!movedPassenger.Contains(hit.transform)) {
						movedPassenger.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;

						this._passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
					}
				}
			}
		}
	}
}
