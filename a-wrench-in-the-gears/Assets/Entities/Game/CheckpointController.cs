using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour {

	public bool active = true;

	public void Enable() {
		this.active = true;
	}

	public void Disable() {
		this.active = false;
	}
}
