using Godot;
using System;

public partial class GameBoard : TileMap {

	private Vector2I[] AlternateTilePositions = new Vector2I[2];
	private bool isReadyToCrack;
	private bool[] isCracked = new bool[2];

	private Vector2I treasureLocation;
	private FindableObject treasureObject;

	[Export] public GameManager GameManager { get; private set; }
	[Export] private Sprite2D treasureSprite;
	[Export] private FindableObject[] findableObjects;

	public override void _Ready() {
		base._Ready();

		GenerateTilemap();
	}

	private void GenerateTilemap() {
		RandomNumberGenerator random = new RandomNumberGenerator();
		random.Randomize();

		for (int x = 0; x < 5; x++) {
			for (int y = 0; y < 5; y++) {
				SetCell(0, new Vector2I(x, y), random.RandiRange(0, 2), Vector2I.Zero);
			}
		}

		SetAlternateTiles();
		if (findableObjects != null && findableObjects.Length > 0) TrySpawnFindableItem(findableObjects[random.RandiRange(0, findableObjects.Length - 1)]);
	}

	private void SetAlternateTiles() {
		RandomNumberGenerator random = new RandomNumberGenerator();
		random.Randomize();

		for (int i = 0; i < 2; i++) {
			int x = random.RandiRange(0, 5);
			int y = random.RandiRange(0, 5);

			Vector2I target = new Vector2I(x, y);

			SetCell(0, target, GetCellSourceId(0, target), Vector2I.Zero, 1);

			AlternateTilePositions[i] = target;
		}
	}

	public bool Dig(Vector2I position) {
		int state = GetCellSourceId(0, position);

		if (state > 0) {
			SetCell(0, position, state - 1, Vector2I.Zero, GetCellAlternativeTile(0, position));
		} else if (state == 0) {
			EraseCell(0, position);
		} else {
			return false;
		}


		int distanceToCrackA = Mathf.Abs(position.X - AlternateTilePositions[0].X) + Mathf.Abs(position.Y - AlternateTilePositions[0].Y);
		int distanceToCrackB = Mathf.Abs(position.X - AlternateTilePositions[1].X) + Mathf.Abs(position.Y - AlternateTilePositions[1].Y);

		if (!isReadyToCrack) {
			if (distanceToCrackA <= 1) {
				isCracked[0] = true;
				isReadyToCrack = true;
				if (distanceToCrackA != 0) Dig(AlternateTilePositions[0]);
			}

			if (distanceToCrackB <= 1) {
				isCracked[1] = true;
				isReadyToCrack = !isReadyToCrack;
				if (distanceToCrackB != 0) Dig(AlternateTilePositions[1]);
			}
		} else {
			if (distanceToCrackA <= 1 && isCracked[1]) {
				isCracked[0] = true;
				isReadyToCrack = false;
				if (distanceToCrackA != 0) Dig(AlternateTilePositions[0]);

				// Connect Cracks
			}

			if (distanceToCrackB <= 1 && isCracked[0]) {
				isCracked[0] = true;
				isReadyToCrack = false;
				if (distanceToCrackA != 0) Dig(AlternateTilePositions[1]);

				// Connect Cracks
			}
		}

		

		return true;
			
	}

	public bool TrySpawnFindableItem(FindableObject item, int maxChecks = 100) {
		Vector2I tilemapSize = GetUsedRect().Size;
		int itemSize = item.tilePositions.Length / 2; // Assuming two integers per tile position

		// Calculate the maximum origin to ensure the item fits within the tilemap
		int maxOriginX = tilemapSize.X - itemSize;
		int maxOriginY = tilemapSize.Y - itemSize;

		RandomNumberGenerator random = new RandomNumberGenerator();
		random.Randomize();

		int checks = 0;
		int originX, originY;
		do {
			originX = random.RandiRange(0, maxOriginX);
			originY = random.RandiRange(0, maxOriginY);
			checks++;
		} while (!CanSpawnAt(item, new Vector2I(originX, originY), tilemapSize) && checks < maxChecks);

		if (checks >= maxChecks) {
			// Handle failed spawn
			GD.Print("Failed to spawn FindableItem after max checks");
			return false;
		}

		treasureSprite.Position = new Vector2((originX + 0.5f) * 128, (originY + 0.5f) * 128);
		treasureSprite.Texture = item.icon;

		treasureObject = item;
		treasureLocation = new Vector2I(originX, originY);

		return true;
	}

	private bool CanSpawnAt(FindableObject item, Vector2I origin, Vector2I tilemapSize) {
		Vector2I[] adjustedPositions = item.GetAdjustedPositions(origin);

		foreach (Vector2I pos in adjustedPositions) {
			if (pos.X < 0 || pos.X >= tilemapSize.X || pos.Y < 0 || pos.Y >= tilemapSize.Y) {
				return false;
			}
		}

		return true;
	}

	public bool FoundTreasure() {

		if (treasureObject == null) return false;

		Vector2I[] adjustedPositions = treasureObject.GetAdjustedPositions(treasureLocation);

		foreach (Vector2I pos in adjustedPositions) {
			if (GetCellSourceId(0, pos) != -1) {
				return false;
			}
		}

		return true;

	}

	public FindableObject GetFindableObject() { return treasureObject; }

	public void RemoveFindableObject() {
		treasureObject = null;
		treasureSprite.Visible = false;
	}

	public void ResetGameBoard() {

		GenerateTilemap();


		treasureSprite.Visible = true;
	}

}
