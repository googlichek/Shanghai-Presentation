using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
	public static int CurrentImage;

	public RectTransform Panel;
	public Image[] ImageContainer;
	public RectTransform Center;

	private int _distanceBetweenImages;

	private float[] _distanceToTheCenters;
	private float _dragStartPosition;
	private float _dragEndPosition;

	private bool _dragging = false;
	private bool _swiping = false;

	private void Update()
	{
		int bottomBoundry = 0;
		if (Panel.localPosition.x > bottomBoundry)
		{
			Panel.localPosition = new Vector3(bottomBoundry, Panel.localPosition.y);
		}

		int topBoundry = -(ImageContainer.Length - 1) * 1920;
		if (Panel.localPosition.x < topBoundry)
		{
			Panel.localPosition = new Vector3(topBoundry, Panel.localPosition.y);
		}

		Prepare();

		if (!_swiping)
		{
			for (int i = 0; i < ImageContainer.Length; i++)
			{
				_distanceToTheCenters[i] =
					Mathf.Abs(Center.transform.position.x - ImageContainer[i].transform.position.x);
			}

			float minDistance = Mathf.Min(_distanceToTheCenters);

			for (int i = 0; i < ImageContainer.Length; i++)
			{
				if (Math.Abs(minDistance - _distanceToTheCenters[i]) < 1)
				{
					CurrentImage = i;
				}
			}

			if (!_dragging)
			{
				LerpToImage(CurrentImage * -_distanceBetweenImages);
			}
		}
		else
		{
			DetectAndDoSwipe();
		}
	}

	public void StartDrag()
	{
		_dragStartPosition = Input.mousePosition.x;
		_dragging = true;
		_swiping = true;
	}

	public void EndDrag()
	{
		_dragEndPosition = Input.mousePosition.x;
		_dragging = false;
	}

	private void DetectAndDoSwipe()
	{
		if (_dragEndPosition - _dragStartPosition < 0 && CurrentImage <= ImageContainer.Length - 1)
		{
			_dragStartPosition = 0;
			_dragEndPosition = 0;

			StartCoroutine(DisableSwipeMode());
		}
		else if (_dragEndPosition - _dragStartPosition > 0 && CurrentImage >= 0)
		{
			_dragStartPosition = 0;
			_dragEndPosition = 0;

			StartCoroutine(DisableSwipeMode());
		}
	}

	private void Prepare()
	{
		int containerLenght = Panel.transform.childCount;
		ImageContainer = new Image[containerLenght];

		for (int i = 0; i < containerLenght; i++)
		{
			ImageContainer[i] = Panel.transform.GetChild(i).GetComponent<Image>();
		}

		_distanceToTheCenters = new float[containerLenght];

		if (ImageContainer[0] && ImageContainer[1])
		{
			_distanceBetweenImages =
				Mathf.RoundToInt(
					Mathf.Abs(
						ImageContainer[1].GetComponent<RectTransform>().anchoredPosition.x -
						ImageContainer[0].GetComponent<RectTransform>().anchoredPosition.x));
		}
	}

	private void LerpToImage(int position)
	{
		float newX = Mathf.Lerp(Panel.anchoredPosition.x, position, Time.deltaTime*5f);
		Vector2 newPosition = new Vector2(newX, Panel.anchoredPosition.y);

		Panel.anchoredPosition = newPosition;
	}

	private IEnumerator DisableSwipeMode()
	{
		yield return new WaitForSeconds(1);
		_swiping = false;
	}
}
