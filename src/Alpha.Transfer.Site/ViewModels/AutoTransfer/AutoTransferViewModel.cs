using System.Collections.Generic;
using System.Web.Mvc;
using Alpha.Transfer.Configurations;
using Alpha.Transfer.Models;
using Alpha.Web.Helpers;
using AlphaPlus.User.Models;

namespace Alpha.Transfer.Site.ViewModels.AutoTransfer
{
    public class AutoTransferViewModel
    {
        protected AutoTransferViewModel(IUserSession userSession)
        {
            ViewerLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            IsShowLevelColumn = ViewerLevel.IsMasterOrSuperLevel();
            ViewerId = userSession.CustId;
            SubAccId = userSession.SubAccId;
        }

        public int ViewerId { get; }

        public int SubAccId { get; }

        public UserLevel ViewerLevel { get; }

        public bool IsShowLevelColumn { get; }

        public IEnumerable<SelectListItem> Columns { get; protected set; }

        public string UserGuideLink
        {
            get
            {
                if (AppSettings.SupportedNewHandbook)
                {
                    return MicroSites.HowToUseUrl($"~/Default/GetBySlug?slug=auto-transfer");
                }

                return AppSettings.IsMBCSite
                    ? MicroSites.HowToUseUrl("~/Index.aspx?linktomessagesite=184099")
                    : MicroSites.MessageUrl("~/StaticMessage/Guide?id=184099");
            }
        }
    }
}