using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
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

	public GameObject ScrollerContent;

	public Image BannerBackground;
	public Image BannerTitle;

	private int _currentButtonScreenIndex;

	private int _screenWidth = 1920;

	private float _timing = 1.5f;
	private float _delay = 0.5f;

	private void Start()
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

		TweenSlide(0, 0, Ease.InQuint);

		ShowButtons(_currentButtonScreenIndex);
		ColorizeDots(_currentButtonScreenIndex);

		MoveMenuButtons(MenuButtonEndPosition, Ease.OutBack);
	}

	public void OpenSlide(int index)
	{
		HideButtons(_currentButtonScreenIndex);
		HideBanner();
		MoveMenuButtons(MenuButtonStartPosition, Ease.InBack);

		MoveSlideButtons(SlideButtonEndPosition, SlideButtonBackEndPosition, Ease.OutBack);

		ScrollerContent.transform.DOLocalMoveX(-index*_screenWidth, 0f);

		TweenSlide(1, _timing, Ease.OutQuint);
	}

	public void BackToMenu()
	{
		ShowButtons(_currentButtonScreenIndex);
		ShowBanner();
		MoveMenuButtons(MenuButtonEndPosition, Ease.OutBack);

		MoveSlideButtons(SlideButtonStartPosition, SlideButtonBackStartPosition, Ease.InBack);

		UntweenSlide(0, _timing, Ease.OutQuint);
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

	private void TweenSlide(float amount, float time, Ease ease)
	{
		foreach (GameObject slide in Slides)
		{
			slide.transform.DOKill();
			slide.GetComponent<Image>().DOFade(amount, time).SetEase(ease);
			slide.transform.DOScale(amount, time).SetEase(ease);

			foreach (Transform child in slide.transform)
			{
				child.DOKill();
				child.DOScale(amount, time).SetEase(ease);
				child.GetComponent<Image>().DOFade(amount, time).SetEase(ease);

				foreach (Transform subChild in child.transform)
				{
					subChild.DOKill();
					subChild.GetComponent<Image>().DOFade(amount, time).SetEase(ease);
				}
			}
		}
	}

	private void UntweenSlide(float amount, float time, Ease ease)
	{
		foreach (GameObject slide in Slides)
		{
			slide.transform.DOKill();
			slide.GetComponent<Image>().DOFade(amount, time).SetEase(ease);
			slide.transform.DOScale(3, time).SetEase(ease);

			foreach (Transform child in slide.transform)
			{
				child.DOKill();
				child.DOScale(3, time).SetEase(ease);
				child.GetComponent<Image>().DOFade(amount, time).SetEase(ease);

				child.DOScale(amount, 0).SetDelay(time + 0.2f);

				foreach (Transform subChild in child.transform)
				{
					subChild.DOKill();
					subChild.GetComponent<Image>().DOFade(amount, time).SetEase(ease);
				}
			}

			slide.transform.DOScale(amount, 0).SetDelay(time + 0.2f);
		}
	}

	private void MoveSlideButtons(int slideArrowPosition, int slideBackPosition, Ease ease)
	{
		SlideButtonLeft.DOKill();
		SlideButtonRight.DOKill();
		SlideButtonBack.DOKill();

		SlideButtonLeft.transform.DOLocalMoveX(-slideArrowPosition, _timing - 0.5f).SetEase(ease);
		SlideButtonRight.transform.DOLocalMoveX(slideArrowPosition, _timing - 0.5f).SetEase(ease);
		SlideButtonBack.transform.DOLocalMoveX(slideBackPosition, _timing - 0.5f).SetEase(ease);
	}

	private void MoveMenuButtons(int position, Ease ease)
	{
		int i = 0;
		foreach (GameObject element in MenuElements)
		{
			element.transform.DOKill();
			element.transform.DOLocalMoveY(position, _timing - 1f).SetEase(ease).SetDelay(0.1f*i);
			i++;
		}
	}

	private void HideButtons(int index)
	{
		ButtonScreens[index].transform.DOKill();
		ButtonScreens[index].transform.DOScale(3, _timing).SetEase(Ease.OutQuint);

		foreach (Transform child in ButtonScreens[index].transform)
		{
			child.DOKill();
			child.GetComponent<Image>().DOFade(0, _timing).SetEase(Ease.OutQuint);

			foreach (Transform subChild in child.transform)
			{
				subChild.DOKill();
				subChild.GetComponent<Image>().DOFade(0, _timing).SetEase(Ease.OutQuint);
			}
		}

		StartCoroutine(FinishHidingButtons(index));
	}

	private void ShowButtons(int index)
	{
		ButtonScreens[index].transform.DOScale(1, _timing).SetEase(Ease.OutQuint).SetDelay(_delay);
		foreach (Transform child in ButtonScreens[index].transform)
		{
			child.DOKill();
			child.GetComponent<Image>().DOFade(1, _timing).SetEase(Ease.OutQuint).SetDelay(_delay);

			foreach (Transform subChild in child.transform)
			{
				subChild.DOKill();
				subChild.GetComponent<Image>().DOFade(1, _timing).SetEase(Ease.OutQuint).SetDelay(_delay);
			}
		}
	}

	private void HideBanner()
	{
		BannerBackground.DOKill();
		BannerTitle.DOKill();

		BannerBackground.DOFade(0, _timing).SetEase(Ease.OutQuint);
		BannerTitle.DOFade(0, _timing).SetEase(Ease.OutQuint);
	}

	private void ShowBanner()
	{
		BannerBackground.DOKill();
		BannerTitle.DOKill();

		BannerBackground.DOFade(1, _timing).SetEase(Ease.OutQuint);
		BannerTitle.DOFade(1, _timing).SetEase(Ease.OutQuint);
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
				element.GetComponent<Image>().DOColor(Color.white*0.7f, 1);
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