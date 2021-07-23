namespace Alpha.Transfer.Site.Controllers
{
    using System.Web.Mvc;
    using MediatR;
    using Requests.TransferToDownlineHistory;
    using Sunplus.Agent.Model.Entities;

    public class TransferToDownlineHistoryController : SiteBaseController
    {
        public TransferToDownlineHistoryController(IMediator mediator, IUserSession userSession)
            : base(mediator, userSession)
        {
        }

        public ActionResult Index(TransferToDownlineHistoryQuery message) => View(message);

        public ActionResult Excel(TransferToDownlineHistoryExcelQuery message) => ExcelView(message);

        public JsonResult GetTransferToDownlineHistoryData(TransferToDownlineHistoryPagingQuery message)
            => Json(mediator.Send(message), JsonRequestBehavior.DenyGet);
    }
}