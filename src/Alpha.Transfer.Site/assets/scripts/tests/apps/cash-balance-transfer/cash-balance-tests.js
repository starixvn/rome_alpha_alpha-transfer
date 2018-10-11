describe("CashBalanceTransfer", function () {
    var jQueryFake = $;
    var topWindow;
    var cashBalance;
    var template = {
        normal: "<body>\n                    <div class=\"container-fluid cash-balance\">\n                        <div id=\"status-message\" class=\"msg-content hidden\">\n                            <div id=\"status-message-content\" class=\"\"></div>\n                        </div>\n                        <div class=\"section\">\n                            <div class=\"row\">\n                                <form id=\"form-transfer\" action=\"~/transfer/cashbalance\" method=\"post\">\n                                    <input type=\"hidden\" name=\"__RequestVerificationToken\" value=\"5nFn1XfKxq2vtqYAHFKl7Vx0Em9gCwylySx-XWSEpg6EOO8_Xj5BcJ0LAzh3H7yBo57-8mZBuhD06ai70i6JfHi0NP1mqlTVDGagVDvliVl4PCB00\" />\n                                    <div class=\"form-group\">\n                                        <label for=\"amount\">Amount</label>\n                                        <input type=\"text\" class=\"textbox hasBorder text-right\" id=\"amount\" maxlength=\"20\" autocomplete=\"off\" autofocus>\n                                        <button type=\"submit\" class=\"btn-submit\">Transfer</button>\n                                    </div>\n                                </form>\n                            </div>\n                            <div class=\"row\">\n                                <div class=\"col-xs-12 note\">\n                                    You can transfer from -194,585.88 to 248,417.24.\n                                </div>\n                            </div>\n                        </div>\n                    </div>\n                    <input type=\"hidden\" id=\"custId\" value=\"1176083\" />\n                    <input type=\"hidden\" id=\"availableAmountFrom\" value=\"-194585.8811\" />\n                    <input type=\"hidden\" id=\"availableAmountTo\" value=\"248417.2422\" />\n                    <input type=\"hidden\" id=\"availableRangeTransfer\" value=\"Amount to transfer must be between -194,585.88 and 248,417.24.\" />\n                    <input type=\"hidden\" id=\"confirmTransfer\" value=\"Are you sure you want to transfer?\" />\n                    <input type=\"hidden\" id=\"transferAmountIs\" value=\"Transfer amount is\" />\n                    <input type=\"hidden\" id=\"invalidValue\" value=\"Invalid value.\" />\n                </body>"
    };
    beforeEach(function () {
        topWindow = {
            OnPopupComplete: jasmine.any(Function)
        };
        cashBalance = new CashBalance(jQueryFake, topWindow);
    });
    describe("_init", function () {
        beforeEach(function () {
            // arrange
            spyOn(cashBalance, "cacheDOMElements");
            spyOn(cashBalance, "registerEvents");
            // act
            cashBalance.init();
        });
        it("_Always_Calls cacheDOMElements", function () {
            expect(cashBalance.cacheDOMElements).toHaveBeenCalled();
        });
        it("_Always_Calls registerEvents", function () {
            expect(cashBalance.registerEvents).toHaveBeenCalled();
        });
    });
    describe("_cacheDOMElements", function () {
        it("_Always_All neccessary elements are cached", function () {
            // arrange
            setFixtures(template.normal);
            // act
            cashBalance.cacheDOMElements();
            // assert
            expect(cashBalance.$msgContainer).toBeDefined();
            expect(cashBalance.$msgContent).toBeDefined();
            expect(cashBalance.$formTransfer).toBeDefined();
            expect(cashBalance.$amount).toBeDefined();
            expect(cashBalance.$custId).toBeDefined();
            expect(cashBalance.$token).toBeDefined();
            expect(cashBalance.availableAmountFrom).toBe(-194585.8811);
            expect(cashBalance.availableAmountTo).toBe(248417.2422);
            expect(cashBalance.availableRangeTransfer).toBe("Amount to transfer must be between -194,585.88 and 248,417.24.");
            expect(cashBalance.confirmTransfer).toBe("Are you sure you want to transfer?");
            expect(cashBalance.transferAmountIs).toBe("Transfer amount is");
        });
    });
    describe("_registerEvents", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            spyOn(cashBalance, "doTransfer");
            spyOn(cashBalance, "amountKeyDown");
            spyOn(cashBalance, "amountKeyUp");
            cashBalance.cacheDOMElements();
            // act
            cashBalance.registerEvents();
        });
        it("_Always_Bind Form_Submit event", function () {
            // act
            cashBalance.$formTransfer.submit();
            // assert
            expect(cashBalance.doTransfer).toHaveBeenCalled();
        });
        it("_Always_Bind Amount_KeyDown event", function () {
            // act
            cashBalance.$amount.keydown();
            // assert
            expect(cashBalance.amountKeyDown).toHaveBeenCalled();
        });
        it("_Always_Bind Amount_KeyUp event", function () {
            // act
            cashBalance.$amount.keyup();
            // assert
            expect(cashBalance.amountKeyUp).toHaveBeenCalled();
        });
    });
    describe("_validateAmount", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
        });
        it("_Valid Amount -> true", function () {
            // act
            var isValid = cashBalance.validateAmount(100);
            // assert
            expect(isValid).toBeTruthy();
        });
        it("_Amount < FromValue -> false", function () {
            // act
            var isValid = cashBalance.validateAmount(-294585.88);
            // assert
            expect(isValid).toBeFalsy();
        });
        it("_Amount > ToValue -> false", function () {
            // act
            var isValid = cashBalance.validateAmount(348417.24);
            // assert
            expect(isValid).toBeFalsy();
        });
        it("_Amount = 0 -> false", function () {
            // act
            var isValid = cashBalance.validateAmount(0);
            // assert
            expect(isValid).toBeFalsy();
        });
    });
    describe("_showMessage", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
        });
        it("_ErrorCode = 0 -> Show Successful Message", function () {
            // act
            cashBalance.showMessage(0, "Transfer successfully.");
            // assert
            expect(cashBalance.$msgContent.hasClass("msg msg-success")).toBeTruthy();
            expect(cashBalance.$msgContainer.hasClass("hidden")).toBeFalsy();
        });
        it("_ErrorCode != 0 -> Show Error Message", function () {
            // act
            cashBalance.showMessage(500, "Internal server error.");
            // assert
            expect(cashBalance.$msgContent.hasClass("msg msg-error")).toBeTruthy();
            expect(cashBalance.$msgContainer.hasClass("hidden")).toBeFalsy();
        });
    });
    describe("_hideMessage", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
        });
        it("_Always_Hide_Message", function () {
            // act
            cashBalance.hideMessage();
            // assert
            expect(cashBalance.$msgContent.val()).toBe("");
            expect(cashBalance.$msgContainer.hasClass("hidden")).toBeTruthy();
        });
    });
    describe("_countDecimals", function () {
        it("_1000 -> 0 Digit", function () {
            // arrange
            var value = 1000;
            // act
            var digit = cashBalance.countDecimals(value);
            // assert
            expect(digit).toBe(0);
        });
        it("_1000.12 -> 2 Digit", function () {
            // arrange
            var value = 1000.12;
            // act
            var digit = cashBalance.countDecimals(value);
            // assert
            expect(digit).toBe(2);
        });
        it("_1000.123456 -> 6 Digit", function () {
            // arrange
            var value = 1000.123456;
            // act
            var digit = cashBalance.countDecimals(value);
            // assert
            expect(digit).toBe(6);
        });
    });
    describe("_amountEqualTo", function () {
        it("_ComparedValue = 0 -> Returns false", function () {
            // arrange
            var amount = 248417.2422;
            var comparedValue = 0;
            // act
            var actual = cashBalance.amountEqualTo(amount, comparedValue);
            // assert
            expect(actual).toBeFalsy();
        });
        it("_ComparedValue > 0 -> Returns false", function () {
            // arrange
            var amount = 248417.2422;
            var comparedValue = 248417.2422;
            // act
            var actual = cashBalance.amountEqualTo(amount, comparedValue);
            // assert
            expect(actual).toBeFalsy();
        });
        it("_ComparedValue > 0 -> Returns true", function () {
            // arrange
            var amount = 248417.25;
            var comparedValue = 248417.2402;
            // act
            var actual = cashBalance.amountEqualTo(amount, comparedValue);
            // assert
            expect(actual).toBeTruthy();
        });
        it("_ComparedValue > 0 -> Returns true", function () {
            // arrange
            var amount = 248417.5;
            var comparedValue = 248417.4999;
            // act
            var actual = cashBalance.amountEqualTo(amount, comparedValue);
            // assert
            expect(actual).toBeTruthy();
        });
        it("_ComparedValue < 0 -> Returns false", function () {
            // arrange
            var amount = -194585.8811;
            var comparedValue = -194585.8811;
            // act
            var actual = cashBalance.amountEqualTo(amount, comparedValue);
            // assert
            expect(actual).toBeFalsy();
        });
        it("_ComparedValue < 0 -> Returns true", function () {
            // arrange
            var amount = -194585.89;
            var comparedValue = -194585.8811;
            // act
            var actual = cashBalance.amountEqualTo(amount, comparedValue);
            // assert
            expect(actual).toBeTruthy();
        });
        it("_ComparedValue < 0 -> Returns true", function () {
            // arrange
            var amount = -194585.6;
            var comparedValue = -194585.5999;
            // act
            var actual = cashBalance.amountEqualTo(amount, comparedValue);
            // assert
            expect(actual).toBeTruthy();
        });
    });
    describe("_isNumeric", function () {
        it("_Valid Negative Number -> Returns true", function () {
            // arrange
            var value = "-123.98";
            // act
            var actual = cashBalance.isNumeric(value);
            // assert
            expect(actual).toBeTruthy();
        });
        it("_Valid Positive Number -> Returns true", function () {
            // arrange
            var value = "123.98";
            // act
            var actual = cashBalance.isNumeric(value);
            // assert
            expect(actual).toBeTruthy();
        });
        it("_Invalid Number -> Returns false", function () {
            // arrange
            var value = "123.98abcd";
            // act
            var actual = cashBalance.isNumeric(value);
            // assert
            expect(actual).toBeFalsy();
        });
    });
    describe("_getValidationObject", function () {
        beforeEach(function () {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
        });
        it("_Amount is Invalid -> Code = 400", function () {
            // arrange
            cashBalance.$amount.val("123.98abcdef");
            // act
            var validationObject = cashBalance.getValidationObject();
            // assert
            expect(validationObject.code).toBe(400);
        });
        it("_Amount < FromValue -> Code = 400", function () {
            // arrange
            cashBalance.$amount.val("-194585.90");
            // act
            var validationObject = cashBalance.getValidationObject();
            // assert
            expect(validationObject.code).toBe(400);
        });
        it("_Amount > ToValue -> Code = 400", function () {
            // arrange
            cashBalance.$amount.val("248417.26");
            // act
            var validationObject = cashBalance.getValidationObject();
            // assert
            expect(validationObject.code).toBe(400);
        });
        it("_Valid Amount -> Code = 0", function () {
            // arrange
            cashBalance.$amount.val("1000.25");
            // act
            var validationObject = cashBalance.getValidationObject();
            // assert
            expect(validationObject.code).toBe(0);
        });
        it("_Amount = FromValue -> Code = 0", function () {
            // arrange
            cashBalance.$amount.val("-194585.89");
            // act
            var validationObject = cashBalance.getValidationObject();
            // assert
            expect(validationObject.code).toBe(0);
        });
        it("_Amount = ToValue -> Code = 0", function () {
            // arrange
            cashBalance.$amount.val("248417.24");
            // act
            var validationObject = cashBalance.getValidationObject();
            // assert
            expect(validationObject.code).toBe(0);
        });
    });
    describe("_afterAddTransferSuccessfully", function () {
        it("_Code = 0 -> Show Successful Message", function () {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
            spyOn(topWindow, "OnPopupComplete");
            var data = { Code: 0, Message: "Transfer successfully." };
            // act
            cashBalance.afterAddTransferSuccessfully(data);
            // assert
            expect(cashBalance.topWindow.OnPopupComplete).toHaveBeenCalled();
        });
        it("_Code = 105 -> Show Error Message & Reload Page After 3 seconds", function () {
            // arrange
            jasmine.clock().install();
            spyOn(location, "reload");
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
            var data = { Code: 105, Message: "" };
            // act
            cashBalance.afterAddTransferSuccessfully(data);
            jasmine.clock().tick(3001);
            // assert
            expect(location.reload).toHaveBeenCalled();
            jasmine.clock().uninstall();
        });
        it("_Code != 0 -> Do NOT Reload Page", function () {
            // arrange
            spyOn(location, "reload");
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
            var data = { Code: 400, Message: "Bad request." };
            // act
            cashBalance.afterAddTransferSuccessfully(data);
            // assert
            expect(location.reload).not.toHaveBeenCalled();
        });
    });
    describe("_doTransfer", function () {
        it("_ValidationObject.Code != 0 -> Show Error Message", function () {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
            cashBalance.$amount.val("250,417.24");
            var e = { preventDefault: function () { } };
            spyOn(cashBalance, "showMessage");
            // act
            cashBalance.doTransfer(e);
            // assert
            expect(cashBalance.showMessage).toHaveBeenCalled();
        });
        it("_ValidationObject.Code = 0 & Confirm Transfer Cancel -> Do Nothing", function () {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
            cashBalance.$amount.val("100,000.00");
            var e = { preventDefault: function () { } };
            spyOn(window, "confirm").and.returnValue(false);
            spyOn(jQueryFake, "unblockUI").and.returnValue(false);
            // act
            cashBalance.doTransfer(e);
            // assert
            expect(window.confirm).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });
        it("_ValidationObject.Code = 0 & Confirm Transfer OK -> Make a Post", function () {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
            cashBalance.$amount.val("100,000.00");
            var e = { preventDefault: function () { } };
            spyOn(window, "confirm").and.returnValue(true);
            spyOn(jQueryFake, "post").and.callThrough();
            // act
            cashBalance.doTransfer(e);
            // assert
            expect(window.confirm).toHaveBeenCalled();
            expect(jQueryFake.post).toHaveBeenCalled();
        });
    });
});
//# sourceMappingURL=cash-balance-tests.js.map