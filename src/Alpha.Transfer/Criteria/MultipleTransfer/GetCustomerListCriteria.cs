namespace Alpha.Transfer.Criteria.MultipleTransfer
{
    using System.Collections.Generic;
    using Constants;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Params.MultipleTransfer;

    public class GetAgentTransferListCriteria : GetCustomerListCriteria
    {
        public GetAgentTransferListCriteria(GetCustomerListParam param)
            : base(param)
        {
            CustId = param.CustId;
        }

        public int CustId { get; }

        public override string GetSettingKey() => "Age_Transfer_SelList_Ag";

        public override bool IsValid() => CustId > 0;
    }

    public class GetMasterTransferListCriteria : GetCustomerListCriteria
    {
        public GetMasterTransferListCriteria(GetCustomerListParam param)
            : base(param)
        {
            MasterId = param.CustId;
        }

        public int MasterId { get; }

        public override string GetSettingKey() => "Age_Transfer_SelList_MAg";

        public override bool IsValid() => MasterId > 0;
    }

    public class GetSuperTransferListCriteria : GetCustomerListCriteria
    {
        public GetSuperTransferListCriteria(GetCustomerListParam param)
            : base(param)
        {
            SuperId = param.CustId;
        }

        [SpParam(SqlParamName = "sagid")]
        public int SuperId { get; }

        public override string GetSettingKey() => "Age_Transfer_SelList_SAg";

        public override bool IsValid() => SuperId > 0;
    }

    public abstract class GetCustomerListCriteria : CriteriaBase
    {
        private readonly IDictionary<int, string> CustStatusMapping = new Dictionary<int, string>
        {
            { StatusConstant.All, "All" },
            { StatusConstant.Open, "Open" },
            { StatusConstant.Suspended, "Suspended" },
            { StatusConstant.Closed, "Closed" },
            { StatusConstant.Disabled, "Disabled" },
        };

        protected GetCustomerListCriteria(GetCustomerListParam param)
        {
            Username = param.Username;
            PageIndex = param.PageIndex;
            PageSize = param.PageSize;
            AllYesterdayBalance = param.AllYesterdayBalance;
            AllowTotalBalanceTransfer = param.AllowTotalBalanceTransfer;
            CustStatus = CustStatusMapping.ContainsKey(param.Status) ? CustStatusMapping[param.Status] : "All";
            TotalPages = 0;
        }

        public string Username { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public int TotalPages { get; set; }

        [SpParam(SqlParamName = "IsYesterdayBalance")]
        public bool AllYesterdayBalance { get; }

        [SpParam(SqlParamName = "IsAllowTotalBalance")]
        public bool AllowTotalBalanceTransfer { get; }

        public string CustStatus { get; }
    }
}