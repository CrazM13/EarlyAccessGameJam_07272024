using Godot;
using System;

public partial class ToolDigger : ToolBase {

	[Export] private Vector2[] offsets;

	public override void OnUse(Vector2I origin, GameBoard gameBoard) {

		int cost = 1;

		bool success = false;

		if (gameBoard.Dig(origin)) success = true;

		foreach (Vector2 offset in offsets) {
			Vector2I coords = origin + (Vector2I) offset;

			if (gameBoard.Dig(coords)) success = true;

			cost++;
		}

		if (success) gameBoard.GameManager.DeductMoney(cost);
	}

}
