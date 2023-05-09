using System;
using System.Collections.Generic;
using Debugging;
using GameState.Interfaces;
using LocationGameObjects.Interfaces;
using TMPro;
using UnityEngine;
using Zenject;

public class DebugUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _currentHPText;

    [SerializeField]
    private DebugObjUI _debugObjPrefab;

    [SerializeField]
    private Transform _parent;

    private List<DebugObjUI> _debugObjList = new List<DebugObjUI>();
    
    #region Dependency

    private IGetLocationGameObjectService _getLocationGameObjectService;
    private ICurrentLocationIDService _currentLocationIDService;
    private IGetLocationCountService _getLocationCountService;
    private IHealthService _healthService;
    private IHealthDebugging _healthDebugging;
    private IChageLocationDebugging _chageLocationDebugging;

    [Inject]
    private void Construct(
        IGetLocationGameObjectService getLocationGameObjectService,
        ICurrentLocationIDService currentLocationIDService,
        IGetLocationCountService getLocationCountService,
        IHealthService healthService,
        IHealthDebugging healthDebugging,
        IChageLocationDebugging chageLocationDebugging)
    {
        _getLocationGameObjectService = getLocationGameObjectService;
        _currentLocationIDService = currentLocationIDService;
        _getLocationCountService = getLocationCountService;
        _healthService = healthService;
        _healthDebugging = healthDebugging;
        _chageLocationDebugging = chageLocationDebugging;
    }

    #endregion

    private void Start()
    {
        for (int i = 0; i < _getLocationCountService.GetLocationsCount(); i++)
        {
            for (int j = 0; j <_getLocationCountService.GetStoneCount(i); j++)
            {
                var debugObj =  Instantiate(_debugObjPrefab, _parent);
                debugObj.LocationId = i;
                debugObj.StoneId = j;
                debugObj.OnEditEndAction =(x)=>
                {
                    _healthDebugging.SetHealth(debugObj.LocationId,debugObj.StoneId,x);
                };
                debugObj.OnBtnClickAction =()=>
                {
                    _chageLocationDebugging.ChangeLocation(debugObj.LocationId,debugObj.StoneId);
                };
                _debugObjList.Add(debugObj);
                
            }
        }
    }

    private void FixedUpdate()
    {
        _currentHPText.text = _healthService.GetCurrentStoneHealth().ToString();
        if (_debugObjList.Count==0) return;
        foreach (var debugObjUI in _debugObjList)
        {
            debugObjUI.SetCurrentState(_currentLocationIDService.GetCurrentLocationId()==debugObjUI.LocationId&&
                                       _currentLocationIDService.GetCurrentStoneId() == debugObjUI.StoneId);
            debugObjUI.SetLocationText(_getLocationGameObjectService.GetLocationsGameObject(debugObjUI.LocationId)[0].name);
            debugObjUI.SetStoneText(_getLocationGameObjectService.GetStoneGameObject(debugObjUI.LocationId,debugObjUI.StoneId).name);
            debugObjUI.SetHpText(_healthDebugging.GetHealth(debugObjUI.LocationId, debugObjUI.StoneId).ToString());
        }
    }

    public void SetCurrentHealth(string value)
    {        
        string input = value;
        try
        {
            int result = Int32.Parse(input);
            _healthDebugging.SetCurrentHealth(result);
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse '{input}'");
        }
    }
    
    public void ChangeState()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}