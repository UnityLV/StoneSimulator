using System;
using System.Threading.Tasks;
using Installers;
using MongoDB.Bson;
using MongoDB.Driver;
using NaughtyAttributes;
using PlayerData.Interfaces;
using Ugi.PlayInstallReferrerPlugin;
using UnityEngine;
using Zenject;

namespace MongoDBCustom
{
    public class MongoDBPlayerDataProvider : IDBPlayerDataProvider
    {
        private IClickDataService _clickDataService;
        private IDBCommands _idbCommands;

        [Inject]
        private void Construct(IClickDataService clickDataService)
        {
            _clickDataService = clickDataService;
        }

        public async Task<BsonDocument> GetPlayerDataById()
        {
            _idbCommands = ValuesFromBootScene.IdbCommands;

            BsonDocument playerData = await _idbCommands.GetPlayerDataAsync();

            if (playerData == null)
            {
                playerData = await CreateFirstPlayerData();
                await _idbCommands.InsertPlayerDataAsync(playerData);
                _clickDataService.ResetAll();

                Debug.Log("Player data inserted");
            }
            else
            {
                Debug.Log("Player data found");
            }

            return playerData;
        }

        private async Task<BsonDocument> CreateFirstPlayerData()
        {
            BsonDocument playerData = new BsonDocument {
                { DBKeys.DeviceID, DeviceInfo.GetDeviceId() },
                { DBKeys.Name, String.Empty },
                { DBKeys.Role, DBKeys.PlayerRole },
                { DBKeys.AllClick, 0 },
                { DBKeys.ClickToGiveReferrer, 500 },
                { DBKeys.AllClickToGiveReferrer, 500 },
                { DBKeys.Referrals, new BsonArray { "82a027fca2749eca6c0db80d88330a46369c32f1" } },
                { DBKeys.Referrer, await GetReferrerFromLinkAsync() },
            };

            return playerData;
        }

        private async Task<string> GetReferrerFromLinkAsync()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            PlayInstallReferrer.GetInstallReferrerInfo((installReferrerDetails) =>
            {
                if (installReferrerDetails.Error != null)
                {
                    Debug.LogError("Error occurred!");
                    if (installReferrerDetails.Error.Exception != null)
                    {
                        Debug.LogError("Exception message: " + installReferrerDetails.Error.Exception.Message);
                    }
                    Debug.LogError("Response code: " + installReferrerDetails.Error.ResponseCode.ToString());
                    tcs.SetResult("No Referrer"); 
                    return;
                }

                if (installReferrerDetails.InstallReferrer != null)
                {
                    tcs.SetResult(installReferrerDetails.InstallReferrer);
                }
            });

            return await tcs.Task;
        }
    }
}