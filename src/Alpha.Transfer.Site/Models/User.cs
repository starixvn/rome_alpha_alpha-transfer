namespace Alpha.Transfer.Site.Models
{
    using System;
    using Sunplus.Agent.Model.Entities;
    using Transfer.Models;

    public class User : IUser
    {
        private readonly IUserSession userSession;

        public User(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int Id => userSession.CustId;

        public string Name => userSession.UserName;

        public string Language => userSession.Language;

        public byte LevelId => userSession.RoleId;

        public string LanguageKey => userSession.LangKey;

        public int SubAccId => userSession.SubAccId;

        public string SubAccName => userSession.SubUserName;

        public string IP => userSession.UserLoginIp;

        public bool AllowTotalBalanceTransfer => userSession.IsAllowedTotalBalanceTransfer;

        public int UserId => userSession.UserId;

        public bool IsInternal => userSession.IsInternal;

        public DateTime LimitedDateToShowInfo => userSession.GetProperty<DateTime>("CustomerDateLimit");

        public string Site => userSession.Site;
    }
}