﻿using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCustom
{
    public static class DBKeys
    {
        public const string DataBase = "admin";
        public const string Collection = "users";
        public const string DeviceID = "deviceId";
        public const string AllClick = "allClicks";
        public const string ClickToGiveReferrer= "clicksToGiveReferrer";
        public const string AllClickToGiveReferrer= "allClicksToGiveReferrer";
        public const string Name = "name";
        public const string Referrals = "referrals";
        public const string Referrer = "referrer";
        public const string Role = "role";
        public const string PlayerRole = "player";
        public const string ChatRole = "chat";

    }
}