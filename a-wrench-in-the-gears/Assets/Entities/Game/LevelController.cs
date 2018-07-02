using Prime31.TransitionKit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	public int nextLevelIndex;
	public int bonusLevelIndex;

	public void LoadNextLevel() {
		this.LoadLevel(this.nextLevelIndex);
	}

	// public void LoadBonusLevel() {
	// 	this.LoadLevel(this.bonusLevelIndex);
	// }

	public void LoadLevel(int sceneIndex) {
		// SceneManager.LoadSceneAsync(sceneIndex);
		// SceneManager.LoadScene(sceneIndex);
		var fader = new FadeTransition() {
			nextScene = sceneIndex,
			fadedDelay = 0.1f,
			fadeToColor = Color.black
		};
		TransitionKit.instance.transitionWithDelegate( fader );
	}
}
