using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	public float speed = 1f;
	public List<GameObject> stoppers;

	private void OnDisable() {
		gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		Debug.Log(other.gameObject.tag);
		foreach (GameObject stopper in stoppers) {
			Debug.Log(stopper.tag);
			if (other.gameObject.tag == stopper.tag) {
				gameObject.SetActive(false);
			}
		}
	}

	private void Update() {
		transform.Translate(Vector3.up * speed * Time.deltaTime);
	}
}
