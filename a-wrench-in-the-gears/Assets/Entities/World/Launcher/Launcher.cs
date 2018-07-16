using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {
	public GameObject projectileType;
	public float timeBetweenLaunch = 1f;
	public Vector3 angle;

	private float nextLaunch = 0f;

	private void Start() {
	}

	private void Update() {
		if (Time.time >= this.nextLaunch) {
			this.LaunchProjectile();
		}
	}

	private void LaunchProjectile() {
		GameObject projectile = ObjectPooler.SharedInstance.GetPooledObject(this.projectileType.tag);
		if (projectile) {
			projectile.transform.position = transform.position;
			projectile.transform.rotation = Quaternion.Euler(this.angle);
			projectile.SetActive(true);
			this.nextLaunch = Time.time + this.timeBetweenLaunch;
		}
	}
}
