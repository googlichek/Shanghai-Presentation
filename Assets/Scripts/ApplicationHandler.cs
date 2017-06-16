using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationHandler : MonoBehaviour
{
	public List<GameObject> Slides;
	public List<GameObject> ButtonScreens;
	public List<Image> BannerPieces;

	public List<GameObject> MenuElements;

	public Button SlideButtonLeft;
	public Button SlideButtonRight;
	public Button SlideButtonBack;

	public int MenuButtonStartPosition;
	public int MenuButtonEndPosition;

	void Start()
	{
		foreach (GameObject screen in ButtonScreens)
		{
			screen.transform.DOKill();
			screen.transform.DOScale(0, 0);

			foreach (Transform child in screen.transform)
			{
				child.DOKill();
				child.GetComponent<Image>().DOFade(0, 0);

				foreach (Transform subChild in child.transform)
				{
					subChild.DOKill();
					subChild.GetComponent<Image>().DOFade(0, 0);
				}
			}
		}

		foreach (GameObject slide in Slides)
		{
			slide.transform.DOKill();
			slide.GetComponent<Image>().DOFade(0, 0);
			slide.transform.DOScale(0, 0);

			foreach (Transform child in slide.transform)
			{
				child.DOKill();
				child.DOScale(0, 0);
				child.GetComponent<Image>().DOFade(0, 0);

				foreach (Transform subChild in child.transform)
				{
					subChild.DOKill();
					subChild.GetComponent<Image>().DOFade(0, 0);
				}
			}

			ButtonScreens[0].transform.DOKill();
			ButtonScreens[0].transform.DOScale(1, 2).SetEase(Ease.OutQuint).SetDelay(1);
			foreach (Transform child in ButtonScreens[0].transform)
			{
				child.DOKill();
				child.GetComponent<Image>().DOFade(1, 2).SetEase(Ease.OutQuint).SetDelay(1);

				foreach (Transform subChild in child.transform)
				{
					subChild.DOKill();
					subChild.GetComponent<Image>().DOFade(1, 2).SetEase(Ease.OutQuint).SetDelay(1);
				}
			}

			int i = 0;
			foreach (GameObject element in MenuElements)
			{
				element.transform.DOKill();
				element.transform.DOLocalMoveY(MenuButtonEndPosition, 1.5f).SetEase(Ease.OutBack).SetDelay(1.5f + 0.2f * i);

				if (i == 1)
				{
					element.GetComponent<Image>().DOColor(Color.white * 0.7f, 2f);
				}

				i++;
			}
		}
	}

	public void OpenSlide(int index)
	{
		
	}
}
