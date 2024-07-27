using Godot;
using System;

public partial class SceneTransition : AnimationPlayer {

	[Export] private string animationName = "Fade";

	private string pathToLoad = "";
	private float delay = 0;

	public override void _Ready() {
		this.PlayBackwards(animationName);
		this.AnimationFinished += OnAnimationFinish;
	}

	public override void _Process(double delta) {
		if (delay > 0) {
			delay -= (float) delta;
			if (delay <= 0) {
				LoadScene(pathToLoad);
			}
		}
	}

	private void OnAnimationFinish(StringName _) {
		if (!string.IsNullOrEmpty(pathToLoad)) {
			GetTree().ChangeSceneToFile(pathToLoad);
			pathToLoad = "";
		}
	}

	public void LoadScene(string path, float delay) {
		this.pathToLoad = path;
		this.delay = delay;
	}

	public void LoadScene(string path) {
		this.Play(animationName);
		pathToLoad = path;
	}

	public void ReloadScene() {
		this.Play(animationName);
		pathToLoad = GetTree().CurrentScene.SceneFilePath;
	}

	public void ReloadScene(float delay) {
		LoadScene(GetTree().CurrentScene.SceneFilePath, delay);
	}

}
