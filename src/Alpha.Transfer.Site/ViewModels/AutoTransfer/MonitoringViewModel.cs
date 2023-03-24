using System.Collections.Generic;
using System.Web.Mvc;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Extensions;
using Sunplus.Agent.Model.Entities;

namespace Alpha.Transfer.Site.ViewModels.AutoTransfer
{
    public class MonitoringViewModel
    {
        public MonitoringViewModel(IUserSession userSession)
        {
            ViewerId = userSession.CustId;
            SubAccId = userSession.SubAccId;
        }

        public bool IsProtectedMode { get; set; }

        public int ViewerId { get; set; }

        public int SubAccId { get; set; }

        public IEnumerable<SelectListItem> Columns
            => new[] {
                Resources.No,
                Resources.Username,
                Resources.FirstName,
                Resources.TransferAmount,
                Resources.Status,
                Resources.Reason,
                Resources.CompletedTime,
                Resources.Cancel,
            }.GetSelectListItems();
    }
}