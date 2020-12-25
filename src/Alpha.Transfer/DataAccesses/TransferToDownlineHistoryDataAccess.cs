namespace Alpha.Transfer.DataAccesses
{
    using AppResources;
    using Criteria.TransferToDownlineHistory;
    using Fanex.Common.Extensions;
    using Models.TransferToDownlineHistory;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface TransferToDownlineHistoryDataAccess
    {
        IEnumerable<TransferToDownlineHistoryModel> GetTransferToDownlineHistory(GetTransferToDownlineHistoryCriteria criteria);
    }

    public class TransferToDownlineHistoryDataAccessImpl : TransferToDownlineHistoryDataAccess
    {
        public IEnumerable<TransferToDownlineHistoryModel> GetTransferToDownlineHistory(GetTransferToDownlineHistoryCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            if (criteria.IsValid())
            {
                using (var objDb = TransferObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            yield return BuildTransferToDownlineHistory(reader);
                        }
                    }
                }
            }
        }

        private static TransferToDownlineHistoryModel BuildTransferToDownlineHistory(IDataReader reader)
            => new TransferToDownlineHistoryModel(
                actionDate: reader.ToDateTime("TransDate"),
                downlineName: reader.ToString("UserName"),
                transferType: TransferTypeMapper[reader.ToString("TransferType")],
                transferAmount: reader.ToDecimal("TransferAmt"),
                oldBalance: reader.ToDecimal("OldBalance"),
                newBalance: reader.ToDecimal("NewBalance"),
                description: reader.ToString("TransDesc"));

        private static IDictionary<string, string> TransferTypeMapper => new Dictionary<string, string>
        {
            ["0"] = Resources.Deposit,
            ["1"] = Resources.Withdrawal
        };
    }
}