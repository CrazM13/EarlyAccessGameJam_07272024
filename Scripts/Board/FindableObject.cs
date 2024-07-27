using Godot;
using System;

public partial class FindableObject : Resource {
	[Export] public Texture2D icon;

	[Export] public Vector2[] tilePositions;

	[Export] public int value;
	[Export] public FindableObject[] revealOptions;

	public Vector2I[] GetAdjustedPositions(Vector2I origin) {
		var adjustedPositions = new Vector2I[tilePositions.Length];

		for (int i = 0; i < tilePositions.Length; i ++) {
			adjustedPositions[i] = (Vector2I)tilePositions[i] + origin;
		}

		return adjustedPositions;
	}

	public FindableObject Reveal(Vector2I position) {
		if (revealOptions == null || revealOptions.Length <= 0) return this; 
		return revealOptions[(position.X + position.Y) % revealOptions.Length];
	}
}
