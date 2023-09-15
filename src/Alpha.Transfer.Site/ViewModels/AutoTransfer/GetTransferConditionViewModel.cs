using Alpha.Transfer.AppResources;
using Alpha.Transfer.Models;

namespace Alpha.Transfer.Site.ViewModels.AutoTransfer
{
    public class GetTransferConditionViewModel
    {
        public string SearchAccountIds { get; set; }

        public string Usernames { get; set; }

        public string TransferCondition { get; set; }

        public byte ViewerLevel { get; set; }

        public string AutoTransfer_InfoAffectTime
        {
            get
            {
                if (ViewerLevel == UserLevel.Super.Value)
                {
                    return Resources.AutoTransfer_InfoAffectTimeSuper;
                }

                return string.Format(Resources.AutoTransfer_InfoAffectTime, TransferCondition);
            }
        }
    }
}