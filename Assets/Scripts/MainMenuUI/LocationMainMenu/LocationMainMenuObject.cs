using System;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuUI.LocationMainMenu
{
    public class LocationMainMenuObject : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _mainBannerObj;

        [Header("Banner properties"), SerializeField]
        private Image _bannerMainImage;

        [SerializeField]
        private Image _bannerSubImage;

        [Header("Play buttons properties"), SerializeField]
        private GameObject _enablePlayBtn;

        [SerializeField]
        private Localize _enablePlayText;

        [SerializeField]
        private GameObject _disablePlayBtn;

        [SerializeField]
        private Localize _disablePlayText;

        [Header("Progress bar properties"), SerializeField]
        private Image _progressBarMask;

        [SerializeField]
        private Image _progressBarMain;

        [SerializeField]
        private Image _progressBarSub;

        [SerializeField]
        private Image _progressBarBG;

        [Header("Color properties"), SerializeField]
        private ColorLocationUI _progressBarMainColor;

        [SerializeField]
        private ColorLocationUI _progressBarSubColor;

        [SerializeField]
        private ColorLocationUI _progressBarBgColor;

        [SerializeField]
        private ColorLocationUI _bannerMainColor;

        [SerializeField]
        private ColorLocationUI _bannerSubColor;

        [Header("End point properties"), SerializeField]
        private GameObject _enableStartPointObj;

        [SerializeField]
        private TextMeshProUGUI _enableStartPointText;

        [SerializeField]
        private GameObject _disableStartPointObj;

        [SerializeField]
        private TextMeshProUGUI _disableStartPointText;

        [SerializeField]
        private GameObject _endPointObject;

        [SerializeField]
        private GameObject _startPointLight;

        [Header("Location image properties"), SerializeField]
        private Image _locationImage;
        
        
        [Serializable]
        private class ColorLocationUI
        {
            public Color CompleteColor;
            public Color InProgressColor;
            public Color LockedColor;
        }

        private const float CHOSEN_SCALE = 1;
        private const float NOT_CHOSEN_SCALE = 0.75f;

        private LocationMainMenuController _locationMainMenuController;

        public void SetLocationController(LocationMainMenuController locationMainMenuController)
        {
            _locationMainMenuController = locationMainMenuController;
        }

        public void OnLocationClick()
        {
            _locationMainMenuController.ChangeCurrentObject(this);
        } 
        
        public void StartGameClick()
        {
            _locationMainMenuController.OnPlayBTNClick(this);
        }

        public void SetLocationSprite(Sprite sprite)
        {
            _locationImage.sprite = sprite;
        }
        
        private void Start()
        {
           ChangeStateChosen(false);
        }

        public void ChangeStateChosen(bool state)
        {
            StopAllCoroutines();
            Vector2 finVector = state
                ? new Vector2(CHOSEN_SCALE, CHOSEN_SCALE)
                : new Vector2(NOT_CHOSEN_SCALE, NOT_CHOSEN_SCALE);
            StartCoroutine(_mainBannerObj.ScaleWithLerp(_mainBannerObj.localScale, finVector, 25));
        }

        public void SetCompleteState(int lvl)
        {
            _bannerMainImage.color = _bannerMainColor.CompleteColor;
            _bannerSubImage.color = _bannerSubColor.CompleteColor;

            _enablePlayBtn.SetActive(true);
            _enablePlayText.SetTerm("ВОЙТИ");
            _disablePlayBtn.SetActive(false);

            _progressBarMask.fillAmount = 1;
            _progressBarMain.color = _progressBarMainColor.CompleteColor;
            _progressBarSub.color = _progressBarSubColor.CompleteColor;
            _progressBarBG.color = _progressBarBgColor.CompleteColor;
            
            _enableStartPointObj.SetActive(true);
            _enableStartPointText.text = $"Lv.{lvl}";
            _disableStartPointObj.SetActive(false);
            _endPointObject.SetActive(false);
            _startPointLight.SetActive(false);
        }

        public void SetInProgressState(int lvl,float amount)
        {
            _bannerMainImage.color = _bannerMainColor.InProgressColor;
            _bannerSubImage.color = _bannerSubColor.InProgressColor;

            _enablePlayBtn.SetActive(true);
            _enablePlayText.SetTerm("ИГРАТЬ"); 
            _disablePlayBtn.SetActive(false);

            _progressBarMask.fillAmount = amount;
            _progressBarMain.color = _progressBarMainColor.InProgressColor;
            _progressBarSub.color = _progressBarSubColor.InProgressColor;
            _progressBarBG.color = _progressBarBgColor.InProgressColor;
            
            _enableStartPointObj.SetActive(true);
            _enableStartPointText.text = $"Lv.{lvl}";
            _disableStartPointObj.SetActive(false);
            _endPointObject.SetActive(false);
            
            _startPointLight.SetActive(true);
        }

        public void SetLockedState(int lvl, bool isNeedStart)
        {
            _bannerMainImage.color = _bannerMainColor.LockedColor;
            _bannerSubImage.color = _bannerSubColor.LockedColor;

            _enablePlayBtn.SetActive(false);
            _disablePlayText.SetTerm("ИГРАТЬ");
            _disablePlayBtn.SetActive(true);

            _progressBarMask.fillAmount = 0;
            _progressBarMain.color = _progressBarMainColor.LockedColor;
            _progressBarSub.color = _progressBarSubColor.LockedColor;
            _progressBarBG.color = _progressBarBgColor.LockedColor;
            
            _enableStartPointObj.SetActive(false);
            _disableStartPointObj.SetActive(isNeedStart);
            _disableStartPointText.text = $"Lv.{lvl}";
            
            _endPointObject.SetActive(true);
            
            _startPointLight.SetActive(false);
        }
    }
}