namespace Alpha.Transfer.Criteria.TransferAllAndClearCredit
{
    using Alpha.Transfer.Models.TransferAllAndClearCredit;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Text;

    public class GetCalculateResultCriteria : CriteriaBase
    {
        public GetCalculateResultCriteria(
            int custId,
            string username,
            string language,
            string allowedTotalBalanceTransferSuperIds,
            bool isAllDayBalanceTransfer,
            IEnumerable<NetPositionModel> netPositions,
            int downlineCustId)
        {
            CustId = custId;
            Username = username;
            Language = language;
            AllowedTotalBalanceTransferSuperIds = allowedTotalBalanceTransferSuperIds;
            IsAllDayBalanceTransfer = isAllDayBalanceTransfer;
            DownlineCustId = downlineCustId;

            var xmlData = new StringBuilder("<ROOT>");
            foreach (var item in netPositions)
            {
                xmlData.Append(item.ToXML);
            }
            xmlData.Append("</ROOT>");
            DownlineNetPosition = xmlData.ToString();
        }

        public string DownlineNetPosition { get; set; }

        public int DownlineCustId { get; set; }

        public int CustId { get; set; }

        [SpParam(SqlParamName = "DownlineUsername")]
        public string Username { get; set; }

        public string Language { get; set; }

        [SpParam(SqlParamName = "SuperIDsTransferTotal")]
        public string AllowedTotalBalanceTransferSuperIds { get; set; }

        [SpParam(SqlParamName = "IsAllDayTransfer")]
        public bool IsAllDayBalanceTransfer { get; set; }

        public override string GetSettingKey()
            => "Age_Transfer_Credit_All_Sel";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username) && DownlineCustId > 0;
    }
}
