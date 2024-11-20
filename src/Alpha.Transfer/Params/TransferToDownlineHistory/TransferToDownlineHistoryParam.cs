namespace Alpha.Transfer.Params.TransferToDownlineHistory
{
    using System;
    using Utils;

    public class TransferToDownlineHistoryParam
    {
        public TransferToDownlineHistoryParam(
            int custId,
            string userName,
            DateTime fromDate,
            DateTime toDate,
            byte levelType)
        {
            CustId = custId;
            UserName = userName;
            FromDate = fromDate == DateTime.MinValue ? SystemTime.Today : fromDate;
            ToDate = toDate == DateTime.MinValue ? SystemTime.Today : toDate;
            LevelType = levelType;
        }

        public int CustId { get; }

        public string UserName { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public byte LevelType { get; }
    }
}