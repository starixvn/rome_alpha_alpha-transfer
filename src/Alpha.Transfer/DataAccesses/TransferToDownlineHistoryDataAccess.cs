namespace Alpha.Transfer.DataAccesses
{
    using Criteria.TransferToDownlineHistory;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Models.TransferToDownlineHistory;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public interface TransferToDownlineHistoryDataAccess
    {
        Tuple<List<TransferToDownlineHistoryModel>, int> GetTransferToDownlineHistory(GetTransferToDownlineHistoryCriteria criteria);
    }

    public class TransferToDownlineHistoryDataAccessImpl : TransferToDownlineHistoryDataAccess
    {
        public Tuple<List<TransferToDownlineHistoryModel>, int> GetTransferToDownlineHistory(GetTransferToDownlineHistoryCriteria criteria)
        {
            var transferToDownlines = new List<TransferToDownlineHistoryModel>();
            var totalRecords = 0;

            if (criteria.IsValid())
            {
                using (var objDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
                {
                    using (var reader = objDb.ExecuteReader(criteria))
                    {
                        while (reader.Read())
                        {
                            transferToDownlines.Add(BuildTransferToDownlineHistory(reader));
                        }
                    }

                    totalRecords = Converter.ToInt(objDb.GetOutputParams()["TotalRecords"]);
                }
            }

            return Tuple.Create(transferToDownlines, totalRecords);
        }

        private static TransferToDownlineHistoryModel BuildTransferToDownlineHistory(IDataReader reader)
            => new TransferToDownlineHistoryModel(
                actionDate: reader.ToDateTime("TransDate"),
                downlineName: reader.ToString("UserName"),
                transferType: reader.ToByte("TransferType"),
                transferAmount: reader.ToDecimal("TransferAmt"),
                oldBalance: reader.ToDecimal("OldBalance"),
                newBalance: reader.ToDecimal("NewBalance"),
                description: reader.ToString("TransDesc"));
    }
}