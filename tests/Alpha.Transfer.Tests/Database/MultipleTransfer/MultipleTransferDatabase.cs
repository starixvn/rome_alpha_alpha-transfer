namespace Alpha.Transfer.Tests.Database.TransferAllAndClearCredit
{
    using System.Data;
    using Helpers;

    public static class MultipleTransferDatabase
    {
        private static DataSet customerList = FileHelper.LoadDataSet("\\Database\\MultipleTransfer\\CustomerList.xml");

        public static DataSet CustomerList => customerList;
    }
}
