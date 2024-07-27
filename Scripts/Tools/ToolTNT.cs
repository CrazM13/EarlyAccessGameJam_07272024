using Godot;
using System;

public partial class ToolTNT : ToolBase {
	public override void OnUse(Vector2I origin, GameBoard gameBoard) {
		gameBoard.ResetGameBoard();
	}
}
