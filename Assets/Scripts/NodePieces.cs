using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePieces : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public int value;
	public Point index;

	[HideInInspector]
	public Vector2 position;
	[HideInInspector]
	public RectTransform rect;

	Image img;
	bool isUpdating;

	public void Initialize(int value, Point point, Sprite piece)
	{
		rect = GetComponent<RectTransform>();
		img = GetComponent<Image>();

		this.value = value;
		SetIndex(point);
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

	public void MovePosition(Vector2 moveSpeed)
	{
		rect.anchoredPosition += moveSpeed * Time.deltaTime * 16f;
	}

	public void MovePositionTo(Vector2 moveDestination)
	{
		rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, moveDestination, Time.deltaTime * 16f);
	}

	public bool UpdatePiece()
	{
		if (Vector3.Distance(rect.anchoredPosition, position) > 1)
		{
			MovePositionTo(position);
			isUpdating = true;
			return true;
		}
		else
		{
			rect.anchoredPosition = position;
			isUpdating = false;
			return false;
		}
	}

	void UpdateName()
	{
		transform.name = "Node [" + index.x + index.y + "]";
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (isUpdating) return;
		MovePieces.Instance.MovePiece(this);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		MovePieces.Instance.DropPiece();
	}
}
