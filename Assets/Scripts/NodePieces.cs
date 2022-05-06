using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodePieces : MonoBehaviour
{
	public int value;
	public Point index;

	[HideInInspector]
	public Vector2 position;
	[HideInInspector]
	public RectTransform rect;

	Image img;

	public void Initialize(int value, Point point, Sprite piece)
	{
		rect = GetComponent<RectTransform>();
		img = GetComponent<Image>();

		this.value = value;
		this.index = point;
		img.sprite = piece;
	}

	public void SetIndex(Point point)
	{
		this.index = point;
		ResetPosition();
		UpdateName();
	}

	public void ResetPosition()
	{
		position = new Vector2((64 * index.x), -(64 * index.y));
	}

	void UpdateName()
	{
		transform.name = "Node [" + index.x + index.y + "]";
	}
}
