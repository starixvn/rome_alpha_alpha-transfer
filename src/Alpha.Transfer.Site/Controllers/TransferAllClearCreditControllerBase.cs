using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Alpha.Auth.Configurations;
using Alpha.Web;
using AlphaPlus.User.Models;
using MediatR;

namespace Alpha.Transfer.Site.Controllers
{
    public class TransferAllClearCreditControllerBase : BaseController
    {
        protected readonly IMediator mediator;
        protected const string InvalidPageViewPath = "~/Views/Shared/InvalidPage.cshtml";

        protected TransferAllClearCreditControllerBase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected ActionResult View<TResponse>(IRequest<TResponse> message)
        {
            return View(mediator.Send(message));
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            var userSession = UserSession.Current;
            if (SubAccDoesNotHavePermission(userSession) || NotOfferTransferAllAndClearCredit(userSession))
            {
                filterContext.Result = new HttpUnauthorizedResult("You are not authorized on this function!");

                var session = filterContext.HttpContext.Session;
                session.Clear();
                session.Abandon();
                if (!session.IsCookieless)
                {
                    FormsAuthentication.SignOut();
                }
            }
        }

        private static bool SubAccDoesNotHavePermission(IUserSession userSession)
        {
            return userSession.IsSubAcc && !userSession.SubPageAccess.Transfer;
        }

        private static bool NotOfferTransferAllAndClearCredit(IUserSession userSession)
        {
            return !AppSettings.OfferClearGivenCreditSites.Any(s => string.Equals(s, "*"))
                && !AppSettings.OfferClearGivenCreditSites.Any(s => string.Equals(s, userSession.Site, StringComparison.OrdinalIgnoreCase));
        }
    }
}