using System;
using System.Collections;
using System.Collections.Generic;
using MongoDBCustom;
using PlayerData.Interfaces;
using UnityEngine;
using Zenject;

public class SettingPage : MonoBehaviour
{
    [SerializeField]
    private GameObject _russian;

    [SerializeField]
    private GameObject _english;

    private string _currentLanguage;

    #region Dependency

    private INicknameDataService _nicknameDataService;
    private IDBValues _dbValues;
    
    [Inject]
    private void Construct(INicknameDataService nicknameDataService, IDBValues dbValues)
    {
        _nicknameDataService = nicknameDataService;
        _dbValues = dbValues;
    }

    #endregion
 
    
    private void Start()
    {
        _currentLanguage = PlayerPrefs.GetString("CurrentLanguage", I2.Loc.LocalizationManager.CurrentLanguageCode);
        I2.Loc.LocalizationManager.CurrentLanguageCode = _currentLanguage;
        UpdateUISettings();
    }

    private void UpdateUISettings()
    {
        _russian.SetActive(_currentLanguage is ("ru" or "Russian"));
        _english.SetActive(_currentLanguage is ("en" or "English"));
    }

    public void SetLanguageBtn(string language)
    {
        I2.Loc.LocalizationManager.CurrentLanguageCode = language;
        _currentLanguage = I2.Loc.LocalizationManager.CurrentLanguageCode;
        UpdateUISettings();
        PlayerPrefs.SetString("CurrentLanguage", I2.Loc.LocalizationManager.CurrentLanguageCode);
    }

    public void SetNickname(string nickname)
    {
        _nicknameDataService.SetNickname(nickname);
        
        _dbValues.UpdatePlayerName(nickname);
    }
}