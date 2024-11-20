using System.Collections.Generic;
using System.Web.Mvc;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Site.Requests.AutoTransfer;
using AlphaPlus.User.Models;

namespace Alpha.Transfer.Site.ViewModels.AutoTransfer
{
    public class MonitoringViewModel : AutoTransferViewModel
    {
        public MonitoringViewModel(IUserSession userSession, MonitoringQuery message) : base(userSession)
        {
            IsProtectedMode = userSession.IsProtectedMode;
            Status = message.Status;
            Columns = GetColumns();
        }

        public bool IsProtectedMode { get; }

        public string Status { get; }

        private IEnumerable<SelectListItem> GetColumns()
        {
            if (IsShowLevelColumn)
            {
                yield return new SelectListItem { Text = Resources.Level, Value = "8" };
            }

            yield return new SelectListItem { Text = Resources.First_LastName, Value = "3" };
            yield return new SelectListItem { Text = Resources.TransferAmount, Value = "4" };
            yield return new SelectListItem { Text = Resources.Status, Value = "5" };
            yield return new SelectListItem { Text = Resources.Reason, Value = "6" };
            yield return new SelectListItem { Text = Resources.CompletedTime, Value = "7" };
        }
    }
}