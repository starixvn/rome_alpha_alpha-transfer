namespace Alpha.Transfer.Site.Tests.Helpers
{
    using System;
    using System.Web.Mvc;
    using Requests;
    using Transfer.Models.CashBalanceTransfer;
    using Xunit;

    [Trait("Category", "Site.Helpers")]
    public class QueryEntityModelBinderTests : QueryEntityModelBinder
    {
        protected override object CreateInstanceOfAssignableReportBaseQuery(Type modelType)
        {
            return "CreateInstanceOfAssignableReportBaseQuery";
        }

        protected override object CallBaseCreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return "CallBaseCreateModel";
        }

        protected override object CallBaseBindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return "CallBaseBindModel";
        }

        [Fact]
        public void CreateModel_ConstructorParamIsNotAssignableFromIUser_CallBaseCreateModel()
        {
            // Arrange
            var userType = typeof(CashBalanceTransfer);

            // Act
            var createdInstance = CreateModel(null, null, userType);

            // Assert
            Assert.Equal("CallBaseCreateModel", createdInstance);
        }

        [Fact]
        public void BindModel_Always_CallBaseBindModel()
        {
            //// Arrange

            //// Act
            var createdInstance = BindModel(null, null);

            //// Assert
            Assert.Equal("CallBaseBindModel", createdInstance);
        }
    }
}
