namespace Alpha.Transfer.Criteria.Customer
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetUsernameSuggestionCriteria : CriteriaBase
    {
        public GetUsernameSuggestionCriteria(
            int actorId,
            string query,
            bool isInternal,
            bool directDownline,
            bool isAccInfo)
        {
            ActorId = actorId;
            Query = query;
            IsInternal = isInternal;
            IsAccInfo = isAccInfo;
            DirectDownline = directDownline;
        }

        public int UserId => ActorId;

        [SpParam(SqlParamName = "CustId")]
        public int ActorId { get; }

        [SpParam(SqlParamName = "username")]
        public string Query { get; }

        [SpParam(SqlParamName = "isInternalAgent")]
        public bool IsInternal { get; }

        public bool DirectDownline { get; }

        public bool IsAccInfo { get; }

        public override string GetSettingKey() => "Age_CustomerSelUserSearch";

        public override bool IsValid() => ActorId > 0 && !string.IsNullOrWhiteSpace(Query);
    }
}
