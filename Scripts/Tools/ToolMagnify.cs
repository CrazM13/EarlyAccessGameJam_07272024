using Godot;
using System;

public partial class ToolMagnify : ToolBase {


	public override void OnUse(Vector2I origin, GameBoard gameBoard) {
		if (gameBoard.FoundTreasure()) {
			FindableObject obj = gameBoard.GetFindableObject();

			FindableObject newObj = obj.Reveal(origin);

			if (newObj == obj) {
				gameBoard.GameManager.AddMoney(obj.value);
				gameBoard.RemoveFindableObject();

				gameBoard.GetTree().CreateTimer(2).Timeout += () => {
					gameBoard.ResetGameBoard();
				};
			}
		}
	}


}
