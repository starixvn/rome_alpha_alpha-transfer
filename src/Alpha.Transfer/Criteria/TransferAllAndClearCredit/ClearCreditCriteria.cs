namespace Alpha.Transfer.Criteria.TransferAllAndClearCredit
{
    using Alpha.Transfer.Models.TransferAllAndClearCredit;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System.Collections.Generic;
    using System.Text;

    public class ClearCreditCriteria : CriteriaBase
    {
        public ClearCreditCriteria(
            int custId,
            int userId,
            int subAccId,
            string username,
            string ip,
            string language,
            IEnumerable<TotalAmtModel> totalAmts)
        {
            CustId = custId;
            UserId = userId;
            SubAccId = subAccId;
            Username = username;
            IP = ip;
            Language = language;

            var xmlData = new StringBuilder("<ROOT>");
            foreach (var amt in totalAmts)
            {
                xmlData.Append(amt.ToXML);
            }
            xmlData.Append("</ROOT>");
            MemberAmountXML = xmlData.ToString();
        }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public int SubAccId { get; set; }

        [SpParam(SqlParamName = "DownlineUsername")]
        public string Username { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string MemberAmountXML { get; set; }

        public override string GetSettingKey()
            => "Age_ClearCredit_All_Ins";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username);
    }
}
