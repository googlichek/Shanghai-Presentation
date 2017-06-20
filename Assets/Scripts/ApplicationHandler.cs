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
	public List<GameObject> SlideDots;

	public Button SlideButtonLeft;
	public Button SlideButtonRight;
	public Button SlideButtonBack;

	public int MenuButtonStartPosition;
	public int MenuButtonEndPosition;

	public int SlideButtonStartPosition;
	public int SlideButtonEndPosition;

	public int SlideButtonBackStartPosition;
	public int SlideButtonBackEndPosition;

	public int SlideDotStartPosition;
	public int SlideDotEndPosition;

	public GameObject ScrollerContent;

	public Image BannerBackground;
	public Image BannerTitle;

	public Color DotColor = new Color(0.3f, 0.7f, 0.7f);

	private int _currentButtonScreenIndex;
	private int _currentSlideIndex;

	private int _screenWidth = 1920;

	private float _timing = 1.5f;
	private float _delay = 0.5f;

	private bool _slideIsOpened = false;

	private void Start()
	{
		_currentButtonScreenIndex = 0;
		_currentSlideIndex = -1;

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
		ColorizeMenueDots(_currentButtonScreenIndex);

		MoveMenuButtons(MenuButtonEndPosition, Ease.OutBack);
	}

	private void LateUpdate()
	{
		if (!_slideIsOpened)
		{
			return;
		}

		if (_currentSlideIndex != ScrollRectSnap.CurrentImage)
		{
			_currentSlideIndex = ScrollRectSnap.CurrentImage;
			HandleSlideArrows();
			ColorizeSlideDots();
		}
	}

	public void OpenSlide(int index)
	{
		_slideIsOpened = true;

		HideButtons(_currentButtonScreenIndex);
		HideBanner();

		MoveMenuButtons(MenuButtonStartPosition, Ease.InBack);
		MoveSlideButtons(SlideButtonEndPosition, SlideButtonBackEndPosition, Ease.OutBack);
		MoveSlideDots(SlideDotEndPosition, Ease.OutBack, 0.6f);

		ScrollerContent.transform.DOLocalMoveX(-index*_screenWidth, 0f);

		TweenSlide(1, _timing, Ease.OutQuint);
	}

	public void BackToMenu()
	{
		_slideIsOpened = false;

		ShowButtons(_currentButtonScreenIndex);
		ShowBanner();

		MoveMenuButtons(MenuButtonEndPosition, Ease.OutBack, 1f);
		MoveSlideButtons(SlideButtonStartPosition, SlideButtonBackStartPosition, Ease.InBack);
		MoveSlideDots(SlideDotStartPosition, Ease.InBack);

		UntweenSlide(0, _timing, Ease.OutQuint);
	}

	public void LeftArrow()
	{
		if (ScrollRectSnap.CurrentImage == 0)
		{
			return;
		}

		float x = ScrollerContent.transform.localPosition.x;

		ScrollerContent.transform.DOKill();
		ScrollerContent.transform.DOLocalMoveX(x + _screenWidth, _timing).SetEase(Ease.OutBack);
	}

	public void RightArrow()
	{
		if (ScrollRectSnap.CurrentImage == Slides.Count - 1)
		{
			return;
		}

		float x = ScrollerContent.transform.localPosition.x;

		ScrollerContent.transform.DOKill();
		ScrollerContent.transform.DOLocalMoveX(x - _screenWidth, _timing).SetEase(Ease.OutBack);
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
			ColorizeMenueDots(_currentButtonScreenIndex);
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
			ColorizeMenueDots(_currentButtonScreenIndex);
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
		time -= 0.2f;

		foreach (GameObject slide in Slides)
		{
			slide.transform.DOKill();
			slide.GetComponent<Image>().DOFade(amount, time).SetEase(ease);
			slide.transform.DOScale(1.1f, time).SetEase(ease);

			foreach (Transform child in slide.transform)
			{
				child.DOKill();
				child.DOScale(1.1f, time).SetEase(ease);
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

	private void MoveMenuButtons(int position, Ease ease, float delay = 0f)
	{
		int i = 0;
		foreach (GameObject element in MenuElements)
		{
			element.transform.DOKill();
			element.transform.DOLocalMoveY(position, _timing - 1f).SetEase(ease).SetDelay(delay + 0.1f*i);
			i++;
		}
	}

	private void MoveSlideDots(int position, Ease ease, float delay = 0f)
	{
		int i = 0;
		foreach (GameObject element in SlideDots)
		{
			element.transform.DOKill();
			element.transform.DOLocalMoveY(position, _timing - 1f).SetEase(ease).SetDelay(delay + 0.05f * i);
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

	private void ColorizeMenueDots(int index)
	{
		foreach (GameObject element in MenuElements)
		{
			if (MenuElements.IndexOf(element) != index + 1)
			{
				element.GetComponent<Image>().DOColor(DotColor, 1);
			}
			else
			{
				element.GetComponent<Image>().DOColor(Color.white, 1);
			}
		}
	}

	private void ColorizeSlideDots()
	{
		foreach (GameObject element in SlideDots)
		{
			if (SlideDots.IndexOf(element) != _currentSlideIndex)
			{
				element.GetComponent<Image>().DOColor(DotColor, 1);
			}
			else
			{
				element.GetComponent<Image>().DOColor(Color.white, 1);
			}
		}
	}

	private void HandleSlideArrows()
	{
		SlideButtonLeft.DOKill();
		SlideButtonRight.DOKill();

		if (_currentSlideIndex == 0)
		{
			SlideButtonLeft.transform.DOLocalMoveX(-SlideButtonStartPosition, _timing - 0.5f).SetEase(Ease.InBack);
		}
		else if (_currentSlideIndex == Slides.Count - 1)
		{
			SlideButtonRight.transform.DOLocalMoveX(SlideButtonStartPosition, _timing - 0.5f).SetEase(Ease.InBack);
		}
		else
		{
			SlideButtonLeft.transform.DOLocalMoveX(-SlideButtonEndPosition, _timing - 0.5f).SetEase(Ease.OutBack);
			SlideButtonRight.transform.DOLocalMoveX(SlideButtonEndPosition, _timing - 0.5f).SetEase(Ease.OutBack);
		}
		
	}

	private IEnumerator FinishHidingButtons(int index)
	{
		yield return new WaitForSeconds(1f);
		ButtonScreens[index].transform.DOKill(true);
		ButtonScreens[index].transform.DOScale(0, 0);
	}
}