using Godot;
using System;

public partial class ToolTNT : ToolBase {
	public override void OnUse(Vector2I origin, GameBoard gameBoard) {
		if (!IsMouseOverUI(gameBoard)) gameBoard.ResetGameBoard();
	}

	private bool IsMouseOverUI(GameBoard gameBoard) {
		CanvasLayer _uiRoot = gameBoard.GetParent().GetNode<CanvasLayer>("CanvasLayer");

		var globalMousePos = gameBoard.GetViewport().GetMousePosition();

		foreach (Control child in _uiRoot.GetChildren()) {
			if (child.Visible && child.GetGlobalRect().HasPoint(globalMousePos)) {
				return true;
			}
		}

		return false;
	}
}
