using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Models.Reward;
using Alpha.Transfer.Site.Requests.Reward;
using Alpha.Transfer.Site.ViewModels.Components;
using Alpha.Transfer.Utils;

namespace Alpha.Transfer.Site.ViewModels.Reward
{
    public class RewardFilterViewModel
    {
        public RewardFilterViewModel(
            RewardBaseQuery message,
            IEnumerable<RewardProgram> programs,
            string excelAction = null,
            bool showColumns = false)
        {
            DateRangePicker = new DateRangePickerJQueryViewModel(
                message.FromDate,
                message.ToDate,
                message.MinDate,
                SystemTime.Today.AddDays(1));
            Programs = GetPrograms(programs, message.ProgramId);
            PaymentStatus = GetPaymentStatus(message.PaymentStatus);
            Username = message.Username;
            ExcelAction = excelAction;
            ShowColumns = showColumns;
        }

        public DateRangePickerJQueryViewModel DateRangePicker { get; }

        public IEnumerable<SelectListItem> Programs { get; }

        public IEnumerable<SelectListItem> PaymentStatus { get; }

        public string Username { get; }

        public string ExcelAction { get; }

        public bool ShowColumns { get; }

        private static IEnumerable<SelectListItem> GetPrograms(IEnumerable<RewardProgram> programs, int? selectedProgramIds)
            => new[]
            {
                new SelectListItem { Text = Resources.All, Value = "0", Selected = selectedProgramIds == null }
            }.Concat(programs.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.ResourceId.ToString(),
                Selected = p.ResourceId == selectedProgramIds
            }));

        private static IEnumerable<SelectListItem> GetPaymentStatus(bool? selectedPaymentStatus)
            => new[]
            {
                new SelectListItem { Text = Resources.All, Value = "0", Selected = selectedPaymentStatus == null },
                new SelectListItem { Text = Resources.Paid, Value = true.ToString(), Selected = selectedPaymentStatus == true },
                new SelectListItem { Text = Resources.Unpaid, Value = false.ToString(), Selected = selectedPaymentStatus == false },
            };
    }
}