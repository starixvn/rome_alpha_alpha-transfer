namespace Alpha.Transfer.Models
{
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
    }
}
