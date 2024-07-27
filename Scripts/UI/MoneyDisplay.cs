using Godot;
using System;

public partial class MoneyDisplay : Label {

	[Export] private GameManager gameManager;

	public override void _Process(double delta) {
		this.Text = gameManager.GetMoney().ToString("C");
	}

}
