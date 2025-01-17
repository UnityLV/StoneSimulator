﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGameUI
{
    public class RatingSingleLine : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;

        [SerializeField] private TMP_Text _ratingNumber;
        [SerializeField] private TMP_Text _pointsAmount;

         public RatingPlayerData Data { get; private set; }
        
        public void SetData(RatingPlayerData playerData)
        {
            Data = playerData;
            if (_image != null)
            {
                _image.sprite = playerData.Sprite;
            }
            _name.text = playerData.Name;
            _ratingNumber.text = playerData.RatingNumber.ToString();
            _pointsAmount.text = playerData.PointsAmount.ToString();
        }
    }
}