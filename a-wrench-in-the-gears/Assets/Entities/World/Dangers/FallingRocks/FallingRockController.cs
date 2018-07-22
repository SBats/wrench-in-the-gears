using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class FallingRockController : MonoBehaviour {
	public GameObject rock;
	public GameObject hitbox;
	public GameObject trigger;
	public float fallDuration;
	public AnimationCurve animationCurve;
	public bool reversed = false;
	public bool active = true;

	private Vector3 top;
	private Vector3 bottom;
	private Vector3 start;
	private Vector3 end;
	private BoxCollider2D hitBoxCollider;
	private BoxCollider2D movementContainer;
	private TriggerController triggerController;
	public bool triggerStatus;
	private bool moving = false;

	private void Awake() {
		this.movementContainer = GetComponent<BoxCollider2D>();
		this.hitBoxCollider = this.hitbox.GetComponent<BoxCollider2D>();
		this.triggerController = this.trigger.GetComponent<TriggerController>();
	}

	private void Start() {
		this.top = transform.position + Vector3.up * this.movementContainer.size.y;
		this.bottom = transform.position;
		if (this.reversed) {
			this.start = this.bottom;
			this.end = this.top;
		} else {
			this.start = this.top;
			this.end = this.bottom;
		}
		this.triggerController.subscribeToStateChange(onTriggerChange);
	}

	private void OnEnable() {
		this.ToggleHitBox(this.triggerStatus);
	}

	private void FixedUpdate() {
		if (this.active && !this.moving) {
			if (this.triggerStatus && this.rock.transform.position == this.start) {
				this.ToggleHitBox(this.end == this.bottom);
				StartCoroutine(AnimatePosition(this.rock.transform.position, this.end, fallDuration));
			}
			if (!this.triggerStatus && this.rock.transform.position == this.end) {
				this.ToggleHitBox(this.start == this.bottom);
				StartCoroutine(AnimatePosition(this.rock.transform.position, this.start, fallDuration));
			}
		}
	}

	public void onTriggerChange(bool status) {
		this.triggerStatus = status;
	}

	public void ToggleHitBox(bool status) {
		this.hitBoxCollider.enabled = status;
	}

	IEnumerator AnimatePosition(Vector3 origin, Vector3 target, float duration) {
		float elapsedTime = 0f;

		this.moving = true;
		while (elapsedTime <= duration) {
			elapsedTime += Time.deltaTime;

			float animationProgression = Mathf.Clamp01(elapsedTime / duration);
			float curvedProgression = this.animationCurve.Evaluate(animationProgression);
			this.rock.transform.position = Vector3.Lerp(origin, target, curvedProgression);

			yield return null;
		}
		this.moving = false;
	}
}
