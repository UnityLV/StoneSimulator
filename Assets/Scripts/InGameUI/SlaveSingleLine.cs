using System.Collections;
using PlayerData.Interfaces;
using TMPro;
using UnityEngine;

namespace InGameUI
{
    public class SlaveSingleLine : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _clickCount;

        public void SetData(SingleSlaveData data)
        {
            _name.text = data.Name;
            _clickCount.text = data.Clicks.ToString();
        }
    }
}