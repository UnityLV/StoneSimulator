using System.Threading.Tasks;
using Network;
using Stone.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PowerButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _counterText;
    private IStoneClickEvents _stoneClickEvents;
    private IStoneClickEventsInvoke _clickEventsInvoke;
    private int _counter;
    private bool _isNeedToIgnoreOneInputClick;

    public static PlayerBehavior PlayerBehavior { get; set; }
    
    [Inject]
    private void Construct(
        IStoneClickEvents stoneClickEvents,
        IStoneClickEventsInvoke clickEventsInvoke
    )
    {
        _stoneClickEvents = stoneClickEvents;
        _clickEventsInvoke = clickEventsInvoke;
    }
    
    private void OnEnable()
    {
        _stoneClickEvents.OnStoneClick += OnOnStoneClick;
        _button.onClick.AddListener(OnButtonClick);
    }

 
    private void OnDisable()
    {
        _stoneClickEvents.OnStoneClick -= OnOnStoneClick;
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnOnStoneClick()
    {
        if (_isNeedToIgnoreOneInputClick)
        {
            _isNeedToIgnoreOneInputClick = false;
            return;
            
        }
        _counter++;
        _counterText.text = _counter.ToString();
    }
    
    private async void OnButtonClick()
    {
        if (_counter <= 0)
        {
            return;
        }
        _isNeedToIgnoreOneInputClick = true;
        int power = GetAndResetCounter();
        
        PlayerBehavior.SetDamage(power);
        
        await Task.Delay(100);//HACK: wait for damage Update;
        
        _clickEventsInvoke.OnStoneClickInvoke();
        
    }

    public int GetAndResetCounter()
    {
        int temp = _counter;
        _counter = 0;
        _counterText.text = _counter.ToString();
        return temp;
    }
    
    
}