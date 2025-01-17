﻿using System.Linq;
using Installers;
using MongoDB.Bson;
using MongoDBCustom;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Zenject;

namespace InGameUI
{
    public static class StringExtension
    {
        public static void LogValue(this string value, string name = "")
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Debug.Log(name + " is empty");
                return;
            }

            Debug.Log(name + " = " + value);
        }
    }

    public class SlaveStatus : MonoBehaviour
    {
        private IDBCommands _values;

        [SerializeField] private TMP_Text _referrer;

        [SerializeField] private UnityEvent _onSlave;

        [FormerlySerializedAs("_onNotSlave")]
        [SerializeField] private UnityEvent _onRedeemedSlave;

        [SerializeField] private UnityEvent _onNeverBeSlave;

        [Inject]
        public void Construct(IDBCommands values)
        {
            _values = values;
        }

        private void Start()
        {
            SetStatus();
        }

        public void SetAsNotSlave()
        {
            _onRedeemedSlave?.Invoke();
        }

        private async void SetStatus()
        {
            var playersData =
                await _values.GetPlayersDataById(ValuesFromBootScene.PlayerData[DBKeys.Referrer].AsString);

            BsonDocument referrer = playersData.FirstOrDefault();

            if (referrer != null && IsReferrerNameExist(referrer))
            {
                ProcessReferrer(referrer);
            }
            else
            {
                ProcessAsNeverSlave();
            }
        }

        private void ProcessAsNeverSlave()
        {
            Debug.Log("You are never been slave");
            _onNeverBeSlave?.Invoke();
        }

        private bool IsReferrerNameExist(BsonDocument referrer)
        {
            return string.IsNullOrWhiteSpace(referrer[DBKeys.Name].AsString) == false;
        }

        private void ProcessReferrer(BsonDocument referrer)
        {
            bool isYouSlave = referrer[DBKeys.Referrals].AsBsonArray.Contains(DeviceInfo.GetDeviceId());

            _referrer.text = referrer[DBKeys.Name].AsString;
            _referrer.text.LogValue("Referrer.text");
            if (isYouSlave)
            {
                ProcessAsLave();
            }
            else
            {
                ProcessOnNotSlave();
            }
        }

        private void ProcessOnNotSlave()
        {
            _onRedeemedSlave?.Invoke();
            Debug.Log("You are not slave");
        }

        private void ProcessAsLave()
        {
            _onSlave?.Invoke();
            Debug.Log("You are slave");
        }
    }
}