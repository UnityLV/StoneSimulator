using System;
using System.Linq;
using Installers;
using MongoDB.Bson;
using MongoDBCustom;
using PlayerData.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace InGameUI
{
    public class SlaveStatusUI : MonoBehaviour
    {
        private IDBValues _values;

        [SerializeField] private TMP_Text _referrer;

        [SerializeField] private UnityEvent _onSlave;
        [SerializeField] private UnityEvent _onNotSlave;

        [Inject]
        public void Construct(IDBValues values)
        {
            _values = values;
        }

        private void Start()
        {
            SetStatus();
        }

        private async void SetStatus()
        {
            var playersData = await _values.GetPlayersDataById(ValuesFromBootScene.PlayerData[DBKeys.Referrer].AsString);

            BsonDocument referrer = playersData.FirstOrDefault();

            ProcessReferrer(referrer);

        }

        private void ProcessReferrer(BsonDocument referrer)
        {
            if (referrer != null)
            {
                ProcessAsLave(referrer);
            }
            else
            {
                ProcessOnNotSlave();
            }
        }

        private void ProcessOnNotSlave()
        {
            _onNotSlave?.Invoke();
            Debug.Log("You are not slave");
        }

        private void ProcessAsLave(BsonDocument referrer)
        {
            _onSlave?.Invoke();
            _referrer.text = referrer[DBKeys.Name].AsString;
            Debug.Log("You are slave");
        }
    }
}