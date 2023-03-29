namespace Alpha.Transfer.Models
{
    using System;

    public interface IUser
    {
        int Id { get; }

        string Name { get; }

        string Language { get; }

        byte LevelId { get; }

        string LanguageKey { get; }

        int SubAccId { get; }

        string SubAccName { get; }

        string IP { get; }

        bool AllowTotalBalanceTransfer { get; }

        int UserId { get; }

        bool IsInternal { get; }

        DateTime LimitedDateToShowInfo { get; }

        string Site { get; }
    }
}