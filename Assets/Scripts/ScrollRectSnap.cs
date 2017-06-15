using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour
{
	public RectTransform Panel;
	public Image[] ImageContainer;
	public RectTransform Center;

	private float[] _distanceToTheCenters;
	private bool _dragging = false;
	private int _distanceBetweenImages;
	private int _currentImage;

	private void Update()
	{
		int bottomBoundry = 0;
		if (Panel.localPosition.x > bottomBoundry)
		{
			Panel.localPosition = new Vector3(bottomBoundry, Panel.localPosition.y);
		}

		int topBoundry = -(ImageContainer.Length - 1)*1920;
		if (Panel.localPosition.x < topBoundry)
		{
			Panel.localPosition = new Vector3(topBoundry, Panel.localPosition.y);
		}

		Prepare();

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
				_currentImage = i;
			}
		}

		if (!_dragging)
		{
			LerpToImage(_currentImage * -_distanceBetweenImages);
		}
	}

	public void StartDrag()
	{
		_dragging = true;
	}

	public void EndDrag()
	{
		_dragging = false;
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
}
