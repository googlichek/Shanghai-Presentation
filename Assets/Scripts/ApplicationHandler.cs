using System.Collections;
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

	public int SlideButtonStartPosition;
	public int SlideButtonEndPosition;

	public int SlideButtonBackStartPosition;
	public int SlideButtonBackEndPosition;

	private int _currentButtonScreenIndex;

	void Start()
	{
		_currentButtonScreenIndex = 0;

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

			ShowButtons(_currentButtonScreenIndex);
			ColorizeDots(_currentButtonScreenIndex);

			MoveMenuButtons(MenuButtonEndPosition, Ease.OutBack);
		}
	}

	public void OpenSlide(int index)
	{
		HideButtons(_currentButtonScreenIndex);
		MoveMenuButtons(MenuButtonStartPosition, Ease.InBack);

		MoveSlideButtons(SlideButtonEndPosition, SlideButtonBackEndPosition, Ease.OutBack);
	}

	public void BackToMenu()
	{
		ShowButtons(_currentButtonScreenIndex);
		MoveMenuButtons(MenuButtonEndPosition, Ease.OutBack);

		MoveSlideButtons(SlideButtonStartPosition, SlideButtonBackStartPosition, Ease.InBack);
	}

	public void SwitchButtonScreen(bool direction)
	{
		if (direction)
		{
			if (_currentButtonScreenIndex == ButtonScreens.Count - 1)
			{
				return;
			}

			HideButtons(_currentButtonScreenIndex);

			_currentButtonScreenIndex++;

			ShowButtons(_currentButtonScreenIndex);
			ColorizeDots(_currentButtonScreenIndex);
		}
		else
		{
			if (_currentButtonScreenIndex == 0)
			{
				return;
			}

			HideButtons(_currentButtonScreenIndex);

			_currentButtonScreenIndex--;

			ShowButtons(_currentButtonScreenIndex);
			ColorizeDots(_currentButtonScreenIndex);
		}
	}

	private void MoveSlideButtons(int slideArrowPosition, int slideBackPosition, Ease ease)
	{
		SlideButtonLeft.DOKill();
		SlideButtonRight.DOKill();
		SlideButtonBack.DOKill();

		SlideButtonLeft.transform.DOLocalMoveX(-slideArrowPosition, 1.5f).SetEase(ease).SetDelay(0.5f);
		SlideButtonRight.transform.DOLocalMoveX(slideArrowPosition, 1.5f).SetEase(ease).SetDelay(0.5f);
		SlideButtonBack.transform.DOLocalMoveX(slideBackPosition, 1.5f).SetEase(ease).SetDelay(0.5f);
	}

	private void MoveMenuButtons(int position, Ease ease)
	{
		int i = 0;
		foreach (GameObject element in MenuElements)
		{
			element.transform.DOKill();
			element.transform.DOLocalMoveY(position, 1.5f).SetEase(ease).SetDelay(1f + 0.2f*i);
			i++;
		}
	}

	private void HideButtons(int index)
	{
		ButtonScreens[index].transform.DOKill();
		ButtonScreens[index].transform.DOScale(3, 1.5f).SetEase(Ease.OutQuint);

		foreach (Transform child in ButtonScreens[index].transform)
		{
			child.DOKill();
			child.GetComponent<Image>().DOFade(0, 1.5f).SetEase(Ease.OutQuint);

			foreach (Transform subChild in child.transform)
			{
				subChild.DOKill();
				subChild.GetComponent<Image>().DOFade(0, 1.5f).SetEase(Ease.OutQuint);
			}
		}

		StartCoroutine(FinishHidingButtons(index));
	}

	private void ShowButtons(int index)
	{
		ButtonScreens[index].transform.DOScale(1, 1.5f).SetEase(Ease.OutQuint).SetDelay(0.5f);
		foreach (Transform child in ButtonScreens[index].transform)
		{
			child.DOKill();
			child.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.OutQuint).SetDelay(0.5f);

			foreach (Transform subChild in child.transform)
			{
				subChild.DOKill();
				subChild.GetComponent<Image>().DOFade(1, 1.5f).SetEase(Ease.OutQuint).SetDelay(0.5f);
			}
		}
	}

	private void ColorizeDots(int index)
	{
		foreach (GameObject element in MenuElements)
		{
			if (MenuElements.IndexOf(element) != index + 1)
			{
				element.GetComponent<Image>().DOColor(Color.white, 1);
			}
			else
			{
				element.GetComponent<Image>().DOColor(Color.white * 0.7f, 1);
			}
		}
	}

	private IEnumerator FinishHidingButtons(int index)
	{
		yield return new WaitForSeconds(1f);
		ButtonScreens[index].transform.DOKill(true);
		ButtonScreens[index].transform.DOScale(0, 0);
	} 
}
