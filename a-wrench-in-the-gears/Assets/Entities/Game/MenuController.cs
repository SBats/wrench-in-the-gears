using UnityEngine;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {
	private LevelController levelController;

	private void Awake() {
		this.levelController = FindObjectOfType<LevelController>();
	}

	public void StartGame() {
		this.levelController.LoadLevel(1);
	}

	public void ReturnToMainMenu() {
		this.levelController.LoadLevel(1);
	}

	public void ExitGame() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
