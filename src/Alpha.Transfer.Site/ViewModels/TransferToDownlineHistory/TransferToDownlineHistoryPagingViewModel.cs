namespace Alpha.Transfer.Site.ViewModels.TransferToDownlineHistory
{
    using Alpha.Transfer.Models.TransferToDownlineHistory;
    using System.Collections.Generic;

    public class TransferToDownlineHistoryPagingViewModel
    {
        public TransferToDownlineHistoryPagingViewModel(
            IEnumerable<TransferToDownlineHistoryModel> transferToDownlines,
            int totalRecords)
        {
            TransferToDownlines = transferToDownlines;
            TotalRecords = totalRecords;
        }

        public IEnumerable<TransferToDownlineHistoryModel> TransferToDownlines { get; }

        public int TotalRecords { get; }
    }
}