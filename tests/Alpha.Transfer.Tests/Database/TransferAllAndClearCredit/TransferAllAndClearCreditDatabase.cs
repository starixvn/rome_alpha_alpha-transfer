namespace Alpha.Transfer.Tests.Database.TransferAllAndClearCredit
{
    using System.Data;
    using Helpers;

    public static class TransferAllAndClearCreditDatabase
    {
        private static DataSet calculation_NoCredit = FileHelper.LoadDataSet("\\Database\\TransferAllAndClearCredit\\Calculation_NoCredit.xml");
        private static DataSet calculation_TransferCredit = FileHelper.LoadDataSet("\\Database\\TransferAllAndClearCredit\\Calculation_TransferCredit.xml");
        private static DataSet transferAll = FileHelper.LoadDataSet("\\Database\\TransferAllAndClearCredit\\TransferAll.xml");
        private static DataSet clearCredit = FileHelper.LoadDataSet("\\Database\\TransferAllAndClearCredit\\ClearCredit.xml");

        public static DataSet Calculation_NoCredit => calculation_NoCredit;
        public static DataSet Calculation_TransferCredit => calculation_TransferCredit;
        public static DataSet TransferAll => transferAll;
        public static DataSet ClearCredit => clearCredit;
    }
}
