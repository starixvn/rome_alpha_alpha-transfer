namespace Alpha.Transfer.DataAccesses
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public interface TransferDataAccess
    {
        bool IsAllDayBalanceTransfer(ICriteria criteria);

        Task<bool> IsAllDayBalanceTransferAsync(ICriteria criteria);
    }

    public class TransferDataAccessImpl : TransferDataAccess
    {
        public bool IsAllDayBalanceTransfer(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    return reader.Read() && Convert.ToBoolean(reader["IsAllDayBalTransfer"]);
                }
            }
        }

        public async Task<bool> IsAllDayBalanceTransferAsync(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = await db.ExecuteReaderAsync(criteria))
                {
                    return reader.Read() && Convert.ToBoolean(reader["IsAllDayBalTransfer"]);
                }
            }
        }
    }
}