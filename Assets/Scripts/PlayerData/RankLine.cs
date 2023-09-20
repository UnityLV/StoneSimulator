using TMPro;
using UnityEngine;

namespace PlayerData
{
    public class RankLine : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rank;
        [SerializeField] private TMP_Text _points;

        public void SetViewData(RankLineUIData data)
        {
            _rank.text = data.rank;
            _points.text = data.points;
        }
    }
}