namespace Alpha.Transfer.Tests.Models.TransferAllAndClearCredit
{
    using System.Collections.Generic;
    using System.Linq;
    using Transfer.Models.TransferAllAndClearCredit;
    using Xunit;

    [Trait("Category", "TransferAllAndClearCredit")]
    public class TransferResultTests
    {
        [Fact]
        public void Conditions_NotHaveTransferAllResultConditionsAndNotHaveClearCreditResultConditions_ReturnsCorrectValue()
        {
            // Arrange
            var transferAllResult = new TransferAllResult();
            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 1, string.Empty, Enumerable.Empty<string>());

            var model = new TransferResult(transferAllResult, clearCreditResult, false);
            var expectedValue = new List<string>();

            // Act
            var actualValue = model.Conditions;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Conditions_NotHaveTransferAllResultConditionsAndHaveClearCreditResultConditions_ReturnsCorrectValue()
        {
            // Arrange
            var transferAllResult = new TransferAllResult();
            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 1, string.Empty, new List<string> { "Clear Credit Conditions" });

            var model = new TransferResult(transferAllResult, clearCreditResult, false);
            var expectedValue = new List<string> { "Clear Credit Conditions" };

            // Act
            var actualValue = model.Conditions;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Conditions_HaveTransferAllResultConditionsAndNotHaveClearCreditResultConditions_ReturnsCorrectValue()
        {
            // Arrange
            var transferAllResult = new TransferAllResult(Enumerable.Empty<TransferStep>(), 1, string.Empty, new List<string> { "Transfer All Conditions" });
            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 1, string.Empty, Enumerable.Empty<string>());

            var model = new TransferResult(transferAllResult, clearCreditResult, false);
            var expectedValue = new List<string> { "Transfer All Conditions" };

            // Act
            var actualValue = model.Conditions;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void Conditions_HaveTransferAllResultConditionsAndHaveClearCreditResultConditions_ReturnsCorrectValue()
        {
            // Arrange
            var transferAllResult = new TransferAllResult(Enumerable.Empty<TransferStep>(), 1, string.Empty, new List<string> { "Transfer All Conditions" });
            var clearCreditResult = new ClearCreditResult(Enumerable.Empty<ClearCreditStep>(), 1, string.Empty, new List<string> { "Clear Credit Conditions" });

            var model = new TransferResult(transferAllResult, clearCreditResult, false);
            var expectedValue = new List<string> { "Transfer All Conditions", "Clear Credit Conditions" };

            // Act
            var actualValue = model.Conditions;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }
    }
}
