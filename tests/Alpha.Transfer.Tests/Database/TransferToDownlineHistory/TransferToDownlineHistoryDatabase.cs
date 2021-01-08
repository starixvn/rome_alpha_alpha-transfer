namespace Alpha.Transfer.Tests.Database.TransferToDownlineHistory
{
    using Helpers;
    using System.Data;

    public static class TransferToDownlineHistoryDatabase
    {
        public static DataSet TransferToDownlineHistoryTable
            => FileHelper.LoadDataSet("\\Database\\TransferToDownlineHistory\\TransferToDownlineHistoryTable.xml");
    }
}