using InGameUI;
using TMPro;
using UnityEngine;

namespace MainMenuUI
{
    public class SlaveUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _allEarnClicks;
        [SerializeField] private TMP_Text _allSlaveCount;
        [SerializeField] private SlaveLines _slaveLines;

        public void SetData(SlavesData data)
        {
            _allEarnClicks.text = data.AllClicks.ToString();
            _allSlaveCount.text = data.AllSlaves.ToString();

            _slaveLines.SetData(data.Data);
        }
    }
}