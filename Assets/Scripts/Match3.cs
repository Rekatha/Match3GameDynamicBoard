using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3 : MonoBehaviour
{
	public ArrayLayout boardLayout;

	[Header("UI Elements")]
	public Sprite[] pieces;
	public RectTransform gameBoard;

	[Header("Prefabs")]
	public GameObject nodePiece;

	int width = 9;
	int height = 14;
	Node[,] board;
	List<NodePieces> updatePiece;

	System.Random random;
    // Start is called before the first frame update
    void Start()
    {
		StartGame();
	}

	// Update is called once per frame
	void Update()
	{
		List<NodePieces> finishedUpdating = new List<NodePieces>();

		for (int i = 0; i < updatePiece.Count; i++)
		{
			NodePieces piece = updatePiece[i];
			if(!piece.UpdatePiece())
			{
				finishedUpdating.Add(piece);
			}
		}

		for (int i = 0; i < finishedUpdating.Count; i++)
		{
			NodePieces pieces = finishedUpdating[i];
			updatePiece.Remove(pieces);
		}
	}

	void StartGame()
	{
		string seed = GetRandomSeed();
		random = new System.Random(seed.GetHashCode());
		updatePiece = new List<NodePieces>();

		InitializeBoard();
		VerifyBoard();
		InstantiateBoard();
	}

	void InitializeBoard()
	{
		board = new Node[width, height];
		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				board[x, y] = new Node((boardLayout.rows[y].row[x]) ? -1 : FillPiece(), new Point(x, y));
			}
		}
	}

	void VerifyBoard()
	{
		List<int> valuesToBeRemoved;
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Point point = new Point(x, y);
				int value = GetValueAtPoint(point);
				if (value <= 0) continue;

				valuesToBeRemoved = new List<int>();
				while (IsConnected(point, true).Count > 0)
				{
					value = GetValueAtPoint(point);
					if (!valuesToBeRemoved.Contains(value))
					{
						valuesToBeRemoved.Add(value);
					}

					SetValueAtPoint(point, NewValue(ref valuesToBeRemoved));
				}
			}
		}
	}

	void InstantiateBoard()
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				int value = board[x, y].value;

				if (value <= 0) continue;

				GameObject tile = Instantiate(nodePiece, gameBoard);
				NodePieces node = tile.GetComponent<NodePieces>();
				RectTransform tilePosition = tile.GetComponent<RectTransform>();
				tilePosition.anchoredPosition = new Vector2((64 * x), - (64 * y));
				node.Initialize(value, new Point(x, y), pieces[value - 1]); // pieces start with 0
			}
		}
	}

	public void ResetPiece(NodePieces piece)
	{
		piece.ResetPosition();
		updatePiece.Add(piece);
	}

	List<Point> IsConnected(Point currentPoint, bool main)
	{
		List<Point> connectedPoints = new List<Point>();
		int currentValue = GetValueAtPoint(currentPoint);

		Point[] directions =
		{
			Point.Up,
			Point.Right,
			Point.Down,
			Point.Left
		};

		foreach(Point direction in directions) // Check same 2 more shapes in the directions
		{
			List<Point> line = new List<Point>();

			int sameAmount = 0;
			for (int i = 1; i < 3; i++)
			{
				Point nextPoint = Point.Add(currentPoint, Point.Multiply(direction, i));

				if(GetValueAtPoint(nextPoint) == currentValue )
				{
					line.Add(nextPoint);
					sameAmount++;
				}
			}

			if(sameAmount > 1) // If there are more than 1 same shapes it is a match
			{
				AddPoints(ref connectedPoints, line); // Add these points to the overarching connected list
			}
		}

		for (int i = 0; i < 2; i++) // Checking wether if in the middle of two same shapes
		{
			List<Point> line = new List<Point>();

			int sameAmount = 0;
			Point[] nextPoints =
			{
				Point.Add(currentPoint, directions[i]),
				Point.Add(currentPoint, directions[i+2])
			};

			foreach (Point nextPoint in nextPoints) // Check both sides if the are same value	
			{
				if (GetValueAtPoint(nextPoint) == currentValue)
				{
					line.Add(nextPoint);
					sameAmount++;
				}
			}

			if (sameAmount > 1)
			{
				AddPoints(ref connectedPoints, line);
			}
		}

		for (int i = 0; i < 4; i++) // 2x2
		{
			List<Point> square = new List<Point>();

			int sameAmount = 0;
			int next = i + 1;
			if (next >= 4) next -= 4; // back to zero

			Point[] nextPoints =
			{
				Point.Add(currentPoint, directions[i]),
				Point.Add(currentPoint, directions[next]),
				Point.Add(currentPoint, Point.Add(directions[i], directions[next]))
			};

			foreach (Point nexPoint in nextPoints)
			{
				if (GetValueAtPoint(nexPoint) == currentValue)
				{
					square.Add(nexPoint);
					sameAmount++;
				}
			}

			if (sameAmount > 2)
			{
				AddPoints(ref connectedPoints, square);
			}
		}

		if (main) // Check for other matches along current match
		{
			for (int i = 0; i < connectedPoints.Count; i++)
			{
				AddPoints(ref connectedPoints, IsConnected(connectedPoints[i], false));
			}
		}

		//if (connectedPoints.Count > 0)
		//{
		//	connectedPoints.Add(currentPoint);
		//}

		return connectedPoints;
	}

	void AddPoints(ref List<Point> points, List<Point> adderPoints)
	{
		foreach (Point adderPoint in adderPoints)
		{
			bool isAdding = true;

			for (int i = 0; i < points.Count; i++)
			{
				if(points[i].IsEquals(adderPoint))
				{
					isAdding = false;
					break;
				}
			}

			if (isAdding)
			{
				points.Add(adderPoint);
			}	
		}
	}

	int FillPiece()
	{
		int val = 0;
		val = (random.Next(0, 100) / (100 / pieces.Length)) + 1;
		return val;
	}

	int GetValueAtPoint (Point point)
	{
		if (point.x < 0 || point.x >= width || point.y < 0 || point.y >= height) return -1;
		return board[point.x, point.y].value;
	}

	void SetValueAtPoint (Point point, int value)
	{
		board[point.x, point.y].value = value;
	}

	int NewValue(ref List<int> valuesToBeRemoved)
	{
		List<int> availableValues = new List<int>();

		for (int i = 0; i < pieces.Length; i++)
		{
			availableValues.Add(i + 1);  // pieces start from 0 so we need to add by 1
		}

		foreach (int valueToBeRemoved in valuesToBeRemoved)
		{
			availableValues.Remove(valueToBeRemoved);
		}

		if (availableValues.Count <= 0) return 0;

		return availableValues[random.Next(0, availableValues.Count)];
	}

	string GetRandomSeed()
	{
		string seed = "";
		string acceptableCharacters = "ABCDEFHGIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
		for (int i = 0; i < 20; i++)
		{
			seed += acceptableCharacters[Random.Range(0, acceptableCharacters.Length)];
		}
		return seed;
	}

	public Vector2 GetPositionFromPoint(Point point)
	{
		return new Vector2((64 * point.x), -(64 * point.y));
	}
}

[System.Serializable]
public class Node
{
	public int value; // 0 = blank, 1 = cube, 2 = sphere, 3 = cylinder, 4 = pyramid, 5 = diamond, -1 = hole
	public Point index;

	public Node(int value, Point index)
	{
		this.value = value;
		this.index = index;
	}
}
