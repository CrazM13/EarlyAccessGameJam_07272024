using Godot;
using System;

public abstract partial class ToolBase : Resource {

	public abstract void OnUse(Vector2I origin, GameBoard gameBoard);

}
