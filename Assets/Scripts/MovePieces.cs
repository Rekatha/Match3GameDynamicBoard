using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{
	public static MovePieces Instance;
	Match3 game;

	NodePieces movedPieced;
	Point newIndex;
	Vector2 mouseStart;

	private void Awake()
	{
		Instance = this;
	}
	// Start is called before the first frame update
	void Start()
    {
		game = GetComponent<Match3>();
    }

    // Update is called once per frame
    void Update()
    {
        if(movedPieced != null)
		{
			Vector2 direction = (Vector2)Input.mousePosition - mouseStart;
			Vector2 normalizedDirections = direction.normalized;
			Vector2 absoluteDirections = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
			Point destination = Point.Zero;

			if(direction.magnitude > 32) // If mouse is 32 pixels move away from starting point
			{
				if(absoluteDirections.x > absoluteDirections.y)
				{
					destination = new Point((normalizedDirections.x > 0) ? 1 : -1, 0);
				}
				else if (absoluteDirections.y > absoluteDirections.x)
				{
					destination = new Point(0, (normalizedDirections.y > 0) ? 1 : -1);
				}
			}

			newIndex = Point.Clone(movedPieced.index);
			newIndex.Add(destination);

			Vector2 position = game.GetPositionFromPoint(movedPieced.index);

			if(!newIndex.IsEquals(movedPieced.index)) // Is added by destination
			{
				position += Point.Multiply(destination, 16).ToVector();
			}

			movedPieced.MovePositionTo(position);
			
		}
    }

	public void MovePiece(NodePieces piece)
	{
		if (movedPieced != null) return;

		movedPieced = piece;
		mouseStart = Input.mousePosition;
	}

	public void DropPiece()
	{
		if (movedPieced == null) return;
		game.ResetPiece(movedPieced);
		movedPieced = null;
	}
}
