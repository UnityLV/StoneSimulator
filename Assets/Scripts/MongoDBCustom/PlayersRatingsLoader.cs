using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstAuth;
using InGameUI;
using MongoDB.Bson;
using MongoDB.Driver;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace MongoDBCustom
{
    public class PlayersRatingsLoader : MonoBehaviour
    {
        [SerializeField] private RatingListUI ratingListUI;
        [SerializeField] private RatingListUI ratingListUI2;

        [SerializeField] private Sprite _image1;
        [SerializeField] private Sprite _image2;
        [SerializeField] private Sprite _image3;
        [SerializeField] private Sprite _imageDefault;

        private IDBCommands _idbCommands;
        private IDBAllClickSaver _idbAllClickSaver;

        [Inject]
        private void Construct(IDBCommands idbCommands, IDBAllClickSaver idbAllClickSaver)
        {
            _idbCommands = idbCommands;
            _idbAllClickSaver = idbAllClickSaver;
        }

        private void Start()
        {
            LoadRating();
        }

        public async void LoadFromButton()
        {
            await _idbAllClickSaver.Save();
            LoadRating();
        }

        private async void LoadRating()
        {
            var playersRatings = await _idbCommands.PlayersRating();
            var ratingPlayerDataList = new List<RatingPlayerData>();

            for (int i = 0; i < playersRatings.Count; i++)
            {
                Sprite sprite = i switch {
                    0 => _image1,
                    1 => _image2,
                    2 => _image3,
                    _ => _imageDefault
                };

                var ratingPlayerData = new RatingPlayerData {
                    Sprite = sprite,
                    Name = playersRatings[i][DBKeys.Name].AsString,
                    RatingNumber = (i + 1),
                    PointsAmount = playersRatings[i][DBKeys.AllClick].AsInt32
                };
                ratingPlayerDataList.Add(ratingPlayerData);
            }
            ratingPlayerDataList = SkipEmptyNames(ratingPlayerDataList);

            ratingListUI.SetData(ratingPlayerDataList);
            ratingListUI2.SetData(ratingPlayerDataList);
        }

        private List<RatingPlayerData> SkipEmptyNames(List<RatingPlayerData> list)
        {
            return list.Where(d => string.IsNullOrWhiteSpace(d.Name) == false).ToList();
        }
    }
}