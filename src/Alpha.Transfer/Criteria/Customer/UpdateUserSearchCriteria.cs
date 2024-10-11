using Fanex.Data;
using Fanex.Data.Repository;

namespace Alpha.Transfer.Criteria.Customer
{
    internal class UpdateUserSearchCriteria : NonQueryCommand
    {
        public UpdateUserSearchCriteria(int userId, string search, string firstName, string lastName, string nickname)
        {
            UserId = userId;
            Search = search;
            FirstName = firstName;
            LastName = lastName;
            Nickname = nickname;
        }

        public int UserId { get; }

        public string Search { get; }

        public string FirstName { get; }

        public string LastName { get; }

        [SpParam(SqlParamName = "username2")]
        public string Nickname { get; }

        public string System { get; } = "ACCOUNTING";

        public override string GetSettingKey() => "Acc_CustomerUpdUserSearch";

        public override bool IsValid() => UserId > 0 && !string.IsNullOrWhiteSpace(Search);
    }
}
