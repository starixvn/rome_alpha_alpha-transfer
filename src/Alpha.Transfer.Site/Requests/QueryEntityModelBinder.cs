namespace Alpha.Transfer.Site.Requests
{
    using System;
    using System.Web.Mvc;
    using Transfer.Models;

    public class QueryEntityModelBinder : DefaultModelBinder
    {
        private static readonly Type[] IUserType = { typeof(IUser) };

        protected override object CreateModel(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            Type modelType)
        {
            if (modelType != null && modelType.GetConstructor(IUserType) != null)
            {
                return CreateInstanceOfAssignableReportBaseQuery(modelType);
            }

            return CallBaseCreateModel(controllerContext, bindingContext, modelType);
        }

        protected virtual object CreateInstanceOfAssignableReportBaseQuery(Type modelType)
            => Activator.CreateInstance(modelType, DependencyResolver.Current.GetService<IUser>());

        protected virtual object CallBaseCreateModel(
           ControllerContext controllerContext,
           ModelBindingContext bindingContext,
           Type modelType) => base.CreateModel(controllerContext, bindingContext, modelType);

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var boundModel = CallBaseBindModel(controllerContext, bindingContext);

            return boundModel;
        }

        protected virtual object CallBaseBindModel(
           ControllerContext controllerContext,
           ModelBindingContext bindingContext)
            => base.BindModel(controllerContext, bindingContext);
    }
}