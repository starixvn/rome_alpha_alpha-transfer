using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Alpha.Transfer.AppResources;
using Alpha.Transfer.Models;
using Alpha.Transfer.Site.Requests.Reward;
using MediatR;

namespace Alpha.Transfer.Site.Controllers
{
    public class RewardController : SiteBaseController
    {
        public RewardController(IMediator mediator) : base(mediator)
        {
        }

        #region Reward Payment
        public Task<ActionResult> Payment(RewardPaymentQuery message) => ViewAsync(message);

        public Task<ActionResult> GetPayment(GetRewardPaymentDataQuery message) => ViewAsync(message, "PaymentData");

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdatePaymentStatus(UpdateRewardPaymentStatusCommand message)
        {
            if (!message.IsValid())
            {
                return Json(new Result { Code = (int)HttpStatusCode.Forbidden, Message = Resources.NotAuthorized });
            }

            var result = await mediator.SendAsync(message);

            if (result.IsKickout)
            {
                ClearSessionAndSignOut();
            }

            return Json(result, JsonRequestBehavior.DenyGet);
        }

        public Task<FileContentResult> PaymentExcel(RewardPaymentExcelQuery message) => ExcelViewAsync(message);
        #endregion

        #region Reward Detail
        public Task<ActionResult> Detail(RewardDetailQuery message) => ViewAsync(message);

        public Task<ActionResult> GetDetail(GetRewardDetailDataQuery message) => ViewAsync(message, "DetailData");

        public Task<FileContentResult> DetailExcel(RewardDetailExcelQuery message) => ExcelViewAsync(message);
        #endregion

        protected void ClearSessionAndSignOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            FormsAuthentication.SignOut();
        }
    }
}