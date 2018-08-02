namespace Alpha.Transfer.Criteria.MultipleTransfer
{
    using System.Collections;
    using Constants;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Models;
    using Params.MultipleTransfer;

    public class GetCustomerListCriteria : CriteriaBase
    {
        public GetCustomerListCriteria(GetCustomerListParam param)
        {
            UserLevel = param.UserLevel;
            CustId = param.CustId;
            Username = param.Username;
            PageIndex = param.PageIndex;
            PageSize = param.PageSize;
            AllYesterdayBalance = param.AllYesterdayBalance;
            AllowTotalBalanceTransfer = param.AllowTotalBalanceTransfer;
            Status = param.Status;
        }

        [SpParam(Ignored = true)]
        public UserLevel UserLevel { get; }

        public int CustId { get; }

        public string Username { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public bool AllYesterdayBalance { get; }

        public bool AllowTotalBalanceTransfer { get; }

        public int Status { get; }

        public override string GetSettingKey()
            => UserLevel.IsAgentLevel() ? "Age_Transfer_SelList_Ag"
            : UserLevel.IsMasterLevel() ? "Age_Transfer_SelList_MAg"
            : "Age_Transfer_SelList_SAg";

        public override bool IsValid()
            => CustId > 0;

        public override Hashtable GetSpParams()
        {
            var result = new Hashtable
            {
                ["pageindex"] = PageIndex,
                ["pagesize"] = PageSize,
                ["username"] = Username,
                ["isyesterdaybalance"] = AllYesterdayBalance,
                ["isallowtotalbalance"] = AllowTotalBalanceTransfer,
                ["totalpages"] = 0
            };

            var custIdName = UserLevel.IsAgentLevel() ? "custid"
                : UserLevel.IsMasterLevel() ? "masterid"
                : "sagid";

            result[custIdName] = CustId;

            switch (Status)
            {
                case StatusConstant.Open:
                    result["closed"] = false;
                    break;
                case StatusConstant.Suspended:
                    result["suspended"] = true;
                    break;
                case StatusConstant.Closed:
                    result["closed"] = true;
                    break;
                case StatusConstant.Disabled:
                    result["enabled"] = false;
                    break;
            }

            return result;
        }
    }
}
