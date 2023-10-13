namespace Alpha.Transfer.Site.Requests.AutoTransfer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Transfer.Helpers;
    using Transfer.Models;

    public class UpdateScheduleQuery
    {
        public UpdateScheduleQuery(IUser user)
        {
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            Language = user.LanguageKey;
        }

        public int CustId { get; set; }
        public string CustName { get; set; }
        public string Schedule { get; set; }
        public string UplineCondition { get; set; }

        public int UplineId { get; }
        public int SubAccId { get; }
        public string IP { get; }
        public string Language { get; }

        public List<int> ScheduleDates => TransferConditionHelper.GetTransferrableDatesFromSchedule(Schedule);
        public List<int> TransferrableDates => (UplineCondition ?? string.Empty).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(d => int.Parse(d)).ToList();

        public bool IsValid() => CustId > 0 && UplineId > 0 && ScheduleDates.Any() && TransferrableDates.Any();
    }
}