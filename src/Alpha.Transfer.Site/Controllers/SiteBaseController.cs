namespace Alpha.Transfer.Site.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Alpha.Transfer.AppResources;
    using Alpha.Transfer.Models;
    using MediatR;
    using Requests;
    using ViewModels;
    using Web;

    public abstract class SiteBaseController : BaseController
    {
        protected readonly IMediator mediator;

        protected const string InvalidPageViewPath = "~/Views/Shared/InvalidPage.cshtml";
        protected const string InvalidExcelFileName = "InvalidExcelFile";

        protected SiteBaseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        protected ActionResult View<TResponse>(IRequest<TResponse> message)
        {
            var query = message as TransferBaseQuery;
            if (query == null || !query.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(mediator.Send(message));
        }

        protected async Task<ActionResult> ViewAsync<TResponse>(IAsyncRequest<TResponse> message, string viewName = null)
        {
            var query = message as TransferBaseQuery;
            if (query == null || !query.IsValid())
            {
                return View(InvalidPageViewPath);
            }

            return View(viewName, await mediator.SendAsync(message));
        }

        protected async Task<ActionResult> JsonAsync<TResponse>(IAsyncRequest<TResponse> message, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            var query = message as TransferBaseQuery;
            if (query == null || !query.IsValid())
            {
                return Json(new Result { Code = (int)HttpStatusCode.BadRequest, Message = Resources.BadRequest });
            }

            return Json(await mediator.SendAsync(message), behavior);
        }

        protected FileContentResult ExcelView(IRequest<ExcelFileViewModel> message)
        {
            var query = message as TransferBaseQuery;
            if (query == null || !query.IsValid())
            {
                return GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, new byte[] { }));
            }

            return GetExcelContent(mediator.Send(message));
        }

        protected async Task<FileContentResult> ExcelViewAsync(IAsyncRequest<ExcelFileViewModel> message)
        {
            var query = message as TransferBaseQuery;
            if (query == null || !query.IsValid())
            {
                return GetExcelContent(new ExcelFileViewModel(InvalidExcelFileName, new byte[] { }));
            }

            return GetExcelContent(await mediator.SendAsync(message));
        }

        private FileContentResult GetExcelContent(ExcelFileViewModel viewModel)
            => File(viewModel.Content, viewModel.ContentType, viewModel.Name);
    }
}