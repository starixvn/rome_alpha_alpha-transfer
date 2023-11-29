namespace Alpha.Transfer.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Criteria.TransferAllAndClearCredit;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
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
            var conditions = new List<string>();

            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                if (criteria.IsValid())
                {
                    using (var reader = objectDb.ExecuteReader(criteria))
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                conditions.Add(Converter.ToString2(reader["Content"]));
                            }

                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    var transferStep = new TransferStep(
                                        Converter.To<byte>(reader["RowId"]),
                                        Converter.ToDecimal(reader["TransferableAmount"]),
                                        reader["MemberOutStanding"] == DBNull.Value ? (decimal?)null : Converter.ToDecimal(reader["MemberOutStanding"]));
                                    transferSteps.Add(transferStep);
                                }
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
            }

            return new TransferResult(transferSteps, clearCreditSteps, conditions);
        }

        public TransferAllResult TransferAll(TransferAllCriteria criteria)
        {
            var result = DoAction(criteria, MapTransferStep);

            return new TransferAllResult((IEnumerable<TransferStep>)result[0], (int)result[1], (string)result[2], (IEnumerable<string>)result[3]);
        }

        public ClearCreditResult ClearCredit(ClearCreditCriteria criteria)
        {
            var result = DoAction(criteria, MapClearCreditStep);

            return new ClearCreditResult((IEnumerable<ClearCreditStep>)result[0], (int)result[1], (string)result[2], (IEnumerable<string>)result[3]);
        }

        public List<object> DoAction<T>(ICriteria criteria, Func<IDataReader, T> createData)
        {
            var steps = new List<T>();
            int errCode;
            string errMsg;
            var conditions = new List<string>();

            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            conditions.Add(Converter.ToString2(reader["Content"]));
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                steps.Add(createData(reader));
                            }
                        }

                        reader.Close();
                    }

                    var output = objectDb.GetOutputParams();
                    errCode = Converter.ToInt(output["ErrCode"]);
                    errMsg = Converter.ToString2(output["ErrMsg"]);
                }
            }

            return new List<object> { steps, errCode, errMsg, conditions };
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