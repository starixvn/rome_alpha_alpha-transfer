namespace Alpha.Transfer.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Criteria.TransferAllAndClearCredit;
    using Fanex.Common;
    using Models.TransferAllAndClearCredit;

    public interface TransferAllAndClearCreditDataAccess
    {
        TransferResult GetCalculateResult(GetCalculateResultCriteria criteria);

        TransferAllResult TransferAll(TransferAllCriteria criteria);

        ClearCreditResult ClearCredit(ClearCreditCriteria criteria);
    }

    public class TransferAllAndClearCreditDataAccessImpl : TransferAllAndClearCreditDataAccess
    {
        public TransferResult GetCalculateResult(GetCalculateResultCriteria criteria)
        {
            var transferSteps = new List<TransferStep>();
            var clearCreditSteps = new List<ClearCreditStep>();

            using (var objectDb = TransferObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var transferStep = new TransferStep(
                                Converter.To<byte>(reader["RowId"]),
                                Converter.ToDecimal(reader["TransferableAmount"]),
                                reader["MemberOutStanding"] == DBNull.Value ? (decimal?)null : Converter.ToDecimal(reader["MemberOutStanding"]));
                            transferSteps.Add(transferStep);
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                var clearCreditStep = new ClearCreditStep(
                                    Converter.To<byte>(reader["RowId"]),
                                    Converter.ToDecimal(reader["GivenCredit"]),
                                    Converter.ToDecimal(reader["ClearableCredit"]));
                                clearCreditSteps.Add(clearCreditStep);
                            }
                        }

                        reader.Close();
                    }
                }
            }

            return new TransferResult(transferSteps, clearCreditSteps);
        }

        public TransferAllResult TransferAll(TransferAllCriteria criteria)
        {
            var steps = new List<TransferStep>();
            int errCode;
            string errMsg;

            using (var objectDb = TransferObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            steps.Add(MapTransferStep(reader));
                        }

                        reader.Close();
                    }

                    var output = objectDb.GetOuputParams();
                    errCode = Converter.ToInt(output["ErrCode"]);
                    errMsg = Converter.ToString2(output["ErrMsg"]);
                }
            }

            return new TransferAllResult(steps, errCode, errMsg);
        }

        public ClearCreditResult ClearCredit(ClearCreditCriteria criteria)
        {
            var steps = new List<ClearCreditStep>();
            int errCode;
            string errMsg;

            using (var objectDb = TransferObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            steps.Add(MapClearCreditStep(reader));
                        }

                        reader.Close();
                    }

                    var output = objectDb.GetOuputParams();
                    errCode = Converter.ToInt(output["ErrCode"]);
                    errMsg = Converter.ToString2(output["ErrMsg"]);
                }
            }

            return new ClearCreditResult(steps, errCode, errMsg);
        }

        private TransferStep MapTransferStep(IDataReader reader)
            => new TransferStep(
                Converter.To<byte>(reader["RowId"]),
                Converter.ToDecimal(reader["TransferredAmount"]),
                reader["MemberOutStanding"] == DBNull.Value ? (decimal?)null : Converter.ToDecimal(reader["MemberOutStanding"]));

        private ClearCreditStep MapClearCreditStep(IDataReader reader)
           => new ClearCreditStep(
               Converter.To<byte>(reader["RowId"]),
               Converter.ToDecimal(reader["GivenCredit"]),
               Converter.ToDecimal(reader["ClearedCredit"]));
    }
}
