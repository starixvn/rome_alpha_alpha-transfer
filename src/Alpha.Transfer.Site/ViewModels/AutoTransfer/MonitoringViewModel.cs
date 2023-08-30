using System.Collections.Generic;
using System.Web.Mvc;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Configurations;
using Alpha.Transfer.Site.Requests.AutoTransfer;
using Alpha.Web.Helpers;
using AlphaPlus.User.Models;

namespace Alpha.Transfer.Site.ViewModels.AutoTransfer
{
    public class MonitoringViewModel
    {
        public MonitoringViewModel(IUserSession userSession, MonitoringQuery message)
        {
            ViewerId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            IsProtectedMode = userSession.IsProtectedMode;
            Status = message.Status;
        }

        public bool IsProtectedMode { get; set; }

        public int ViewerId { get; set; }

        public int SubAccId { get; set; }

        public string Status { get; set; }

        public IEnumerable<SelectListItem> Columns
            => new[] {
                new SelectListItem { Text = Resources.First_LastName, Value = "3" },
                new SelectListItem { Text = Resources.TransferAmount, Value = "4" },
                new SelectListItem { Text = Resources.Status, Value = "5" },
                new SelectListItem { Text = Resources.Reason, Value = "6" },
                new SelectListItem { Text = Resources.CompletedTime, Value = "7" },
                new SelectListItem { Text = Resources.Cancel, Value = "8" },
            };

        public string UserGuideLink => AppSettings.IsMBCSite
            ? MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=184099")
            : MicroSites.MessageUrl("~/StaticMessage/Guide?id=184099");
    }
}