namespace Alpha.Transfer.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Criteria.TransferAllAndClearCredit;
    using Fanex.Common;
    using Fanex.Data;
    using Models.TransferAllAndClearCredit;

    public interface TransferAllAndClearCreditDataAccess
    {
        TransferResult GetTransferResult(GetTransferResultCriteria criteria);
    }

    public class TransferAllAndClearCreditDataAccessImpl : TransferAllAndClearCreditDataAccess
    {
        public TransferResult GetTransferResult(GetTransferResultCriteria criteria)
        {
            return StubResult1();

            var transferSteps = new List<TransferStep>();
            var clearCreditSteps = new List<ClearCreditStep>();
            bool isSuccess;
            string errorMessage;

            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            transferSteps.Add(new TransferStep(
                                Converter.ToString2(reader["Step"]),
                                Converter.ToDecimal(reader["Amount"]),
                                reader["Outstanding"] == DBNull.Value ? (decimal?)null : Converter.ToDecimal(reader["ToDecimal"])));
                        }

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                clearCreditSteps.Add(new ClearCreditStep(
                                    Converter.ToString2(reader["Level"]),
                                    Converter.ToDecimal(reader["GivenCredit"]),
                                    Converter.ToDecimal(reader["Amount"])));
                            }
                        }

                        reader.Close();
                    }

                    var output = objectDb.GetOuputParams();
                    isSuccess = Converter.ToBool(output["IsSuccess"]);
                    errorMessage = Converter.ToString2(output["Message"]);
                }
            }

            return new TransferResult(isSuccess, errorMessage, transferSteps, clearCreditSteps);
        }

        public TransferResult StubResult1()
        {
            var transferSteps = new List<TransferStep>
            {
                new TransferStep("Member to Agent", 10000, 5),
                new TransferStep("Agent to CHU01", 660, null),
                new TransferStep("CHU01 to your account", 330, null)
            };

            var clearCreditSteps = new List<ClearCreditStep> { };

            bool isSuccess = true;
            string errorMessage = "Some Errors Occurred";

            return new TransferResult(isSuccess, errorMessage, transferSteps, clearCreditSteps);
        }

        public TransferResult StubResult()
        {
            var transferSteps = new List<TransferStep>
            {
                new TransferStep("Member to Agent", 10000, 8),
                new TransferStep("Agent to CHU01", 660, null),
                new TransferStep("CHU01 to your account", 330, null)
            };

            var clearCreditSteps = new List<ClearCreditStep>
            {
                new ClearCreditStep("CHU01's Members", 10000, 9900),
                new ClearCreditStep("CHU01's Agent", 20000, 1900),
                new ClearCreditStep("CHU01", 40000, 39991)
            };

            bool isSuccess = true;
            string errorMessage = "Some Errors Occurred";

            return new TransferResult(isSuccess, errorMessage, transferSteps, clearCreditSteps);
        }
    }
}
