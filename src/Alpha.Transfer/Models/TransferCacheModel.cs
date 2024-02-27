namespace Alpha.Transfer.Models
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class TransferCacheModel
    {
        public int AdminId { get; set; }

        public string AdminName { get; set; }

        public List<int> CustIds { get; set; }

        public TransferCacheModel()
        {
            CustIds = new List<int>();
        }

        public bool Equals(TransferCacheModel obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (AdminId != obj.AdminId)
            {
                return false;
            }

            if (CustIds.Count != obj.CustIds.Count)
            {
                return false;
            }

            for (int i = 0; i < CustIds.Count; i++)
            {
                if (CustIds[i] != obj.CustIds[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
