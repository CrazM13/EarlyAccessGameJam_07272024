using Godot;
using System;

public partial class GameManager : Node {

	[Export] private ToolBase startingTool;

	[Export] private SceneTransition sceneManager;
	[Export] private GameBoard gameBoard;

	private int money;

	private ToolBase tool;

	public override void _Ready() {
		SetTool(startingTool);
		if (money == 0) { money = 25; }
	}

	public int GetMoney() => money;

	public void SetMoney(int money) => this.money = money;

	public void AddMoney(int money) => this.money += money;

	public void DeductMoney(int money) {
		this.money -= money;

		if (this.money <= 0) {
			sceneManager.LoadScene("res://Scenes/GameOver.tscn");
		}
	}

	public void SetTool(ToolBase tool) => this.tool = tool;
	public ToolBase GetTool() => this.tool;

	private bool wasMousePressed = false;
	public override void _Process(double delta) {
		base._Process(delta);

		bool isMousePressed = Input.IsMouseButtonPressed(MouseButton.Left);

		if (isMousePressed && !wasMousePressed) {
			Vector2I mousePosition = (Vector2I)(gameBoard.ToLocal(GetViewport().GetMousePosition()) / 128);

			tool.OnUse(mousePosition, gameBoard);
		}

		wasMousePressed = isMousePressed;
	}

}
