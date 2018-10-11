namespace Alpha.Transfer.Tests.Database.TransferAllAndClearCredit
{
    using System.Data;
    using Helpers;

    public static class TransferAllAndClearCreditDatabase
    {
        private static DataSet calculation_NoCredit = FileHelper.LoadDataSet("\\Database\\TransferAllAndClearCredit\\Calculation_NoCredit.xml");
        private static DataSet calculation_TransferCredit = FileHelper.LoadDataSet("\\Database\\TransferAllAndClearCredit\\Calculation_TransferCredit.xml");
        private static DataSet calculation_NoTransferAndNoCredit = FileHelper.LoadDataSet("\\Database\\TransferAllAndClearCredit\\Calculation_NoTransferAndNoCredit.xml");
        private static DataSet transferAll = FileHelper.LoadDataSet("\\Database\\TransferAllAndClearCredit\\TransferAll.xml");
        private static DataSet transferAllHasErrorAndNoData = FileHelper.LoadDataSet("\\Database\\TransferAllAndClearCredit\\TransferAll_HasErrorAndNoData.xml");
        private static DataSet clearCredit = FileHelper.LoadDataSet("\\Database\\TransferAllAndClearCredit\\ClearCredit.xml");
        private static DataSet clearCreditNoData = FileHelper.LoadDataSet("\\Database\\TransferAllAndClearCredit\\ClearCredit_NoData.xml");

        public static DataSet Calculation_NoCredit => calculation_NoCredit;
        public static DataSet Calculation_TransferCredit => calculation_TransferCredit;
        public static DataSet Calculation_NoTransferAndNoCredit => calculation_NoTransferAndNoCredit;
        public static DataSet TransferAll => transferAll;
        public static DataSet TransferAllHasErrorAndNoData => transferAllHasErrorAndNoData;
        public static DataSet ClearCredit => clearCredit;
        public static DataSet ClearCreditNoData => clearCreditNoData;
    }
}
