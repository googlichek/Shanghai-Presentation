using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationHandler : MonoBehaviour
{
	public List<Image> SlidePieces = new List<Image>();
	public List<Image> ButtonPieces = new List<Image>();

	public List<GameObject> Slides; 

	void Start()
	{
		List<GameObject> temp = new List<GameObject>(GameObject.FindGameObjectsWithTag("SlideContent"));

		foreach (GameObject piece in temp)
		{
			SlidePieces.Add(piece.GetComponent<Image>());
		}

		temp = new List<GameObject>(GameObject.FindGameObjectsWithTag("ButtonContent"));

		foreach (GameObject piece in temp)
		{
			ButtonPieces.Add(piece.GetComponent<Image>());
		}

		foreach (var image in SlidePieces)
		{
			//image.DOFade(0, 0);
			//image.transform.DOScale(0, 0);
		}
	}
}
