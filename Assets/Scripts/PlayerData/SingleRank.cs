using System;

namespace PlayerData
{
    [Serializable]
    public struct SingleRank
    {
        public SingleRank(string rankNameRu, string rankNameEn, int rankPoints)
        {
            this.rankNameRu = rankNameRu;
            this.rankNameEn = rankNameEn;
            this.rankPoints = rankPoints;
        }

        public string rankNameRu;
        public string rankNameEn;
        public int rankPoints;

        public override string ToString()
        {
            return $"rankNameRu {rankNameRu} / rankNameEn {rankNameEn} / rankPoints {rankPoints}";
        }
    }
}