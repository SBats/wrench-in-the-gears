using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	public string nextLevelName;
	public string bonusLevelName;

	public void LoadNextLevel() {
		this.LoadLevel(this.nextLevelName);
	}

	public void LoadBonusLevel() {
		this.LoadLevel(this.bonusLevelName);
	}

	public void LoadLevel(string sceneName) {
		// SceneManager.LoadSceneAsync(sceneName);
		SceneManager.LoadScene(sceneName);
	}
}
