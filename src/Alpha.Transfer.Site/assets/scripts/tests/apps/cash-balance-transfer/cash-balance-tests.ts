describe("CashBalanceTransfer", () => {
    const jQueryFake: any = $;
    let topWindow: any;
    let cashBalance: CashBalance;

    let template: any = {
        normal: `<body>
                    <div class="container-fluid cash-balance">
                        <div id="status-message" class="msg-content hidden">
                            <div id="status-message-content" class=""></div>
                        </div>
                        <div class="section">
                            <div class="row">
                                <form id="form-transfer" action="/transfer/cashbalance" method="post">
                                    <input type="hidden" name="__RequestVerificationToken" value="5nFn1XfKxq2vtqYAHFKl7Vx0Em9gCwylySx-XWSEpg6EOO8_Xj5BcJ0LAzh3H7yBo57-8mZBuhD06ai70i6JfHi0NP1mqlTVDGagVDvliVl4PCB00" />
                                    <div class="form-group">
                                        <label for="amount">Amount</label>
                                        <input type="text" class="textbox hasBorder text-right" id="amount" maxlength="20" autocomplete="off" autofocus>
                                        <button type="submit" class="btn-submit">Transfer</button>
                                    </div>
                                </form>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 note">
                                    You can transfer from -194,585.88 to 248,417.24.
                                </div>
                            </div>
                        </div>
                    </div>
                    <input type="hidden" id="custId" value="1176083" />
                    <input type="hidden" id="availableAmountFrom" value="-194585.8811" />
                    <input type="hidden" id="availableAmountTo" value="248417.2422" />
                    <input type="hidden" id="availableRangeTransfer" value="Amount to transfer must be between -194,585.88 and 248,417.24." />
                    <input type="hidden" id="confirmTransfer" value="Are you sure you want to transfer?" />
                    <input type="hidden" id="transferAmountIs" value="Transfer amount is" />
                    <input type="hidden" id="invalidValue" value="Invalid value." />
                </body>`
    };

    beforeEach(() => {
        topWindow = {
            OnPopupComplete: jasmine.any(Function)
        };

        cashBalance = new CashBalance(jQueryFake, topWindow);
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            spyOn(cashBalance, "cacheDOMElements");
            spyOn(cashBalance, "registerEvents");

            // act
            cashBalance.init();
        });

        it("_Always_Calls cacheDOMElements", () => {
            expect(cashBalance.cacheDOMElements).toHaveBeenCalled();
        });

        it("_Always_Calls registerEvents", () => {
            expect(cashBalance.registerEvents).toHaveBeenCalled();
        });
    });

    describe("_cacheDOMElements", () => {
        it("_Always_All neccessary elements are cached", () => {
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

    describe("_registerEvents", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(cashBalance, "doTransfer");
            spyOn(cashBalance, "amountKeyDown");
            spyOn(cashBalance, "amountKeyUp");
            cashBalance.cacheDOMElements();

            // act
            cashBalance.registerEvents();
        });

        it("_Always_Bind Form_Submit event", () => {
            // act
            cashBalance.$formTransfer.submit();

            // assert
            expect(cashBalance.doTransfer).toHaveBeenCalled();
        });

        it("_Always_Bind Amount_KeyDown event", () => {
            // act
            cashBalance.$amount.keydown();

            // assert
            expect(cashBalance.amountKeyDown).toHaveBeenCalled();
        });

        it("_Always_Bind Amount_KeyUp event", () => {
            // act
            cashBalance.$amount.keyup();

            // assert
            expect(cashBalance.amountKeyUp).toHaveBeenCalled();
        });
    });

    describe("_validateAmount", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
        });

        it("_Valid Amount -> true", () => {
            // act
            let isValid: boolean = cashBalance.validateAmount(100);

            // assert
            expect(isValid).toBeTruthy();
        });

        it("_Amount < FromValue -> false", () => {
            // act
            let isValid: boolean = cashBalance.validateAmount(-294585.88);

            // assert
            expect(isValid).toBeFalsy();
        });

        it("_Amount > ToValue -> false", () => {
            // act
            let isValid: boolean = cashBalance.validateAmount(348417.24);

            // assert
            expect(isValid).toBeFalsy();
        });

        it("_Amount = 0 -> false", () => {
            // act
            let isValid: boolean = cashBalance.validateAmount(0);

            // assert
            expect(isValid).toBeFalsy();
        });
    });

    describe("_showMessage", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
        });

        it("_ErrorCode = 0 -> Show Successful Message", () => {
            // act
            cashBalance.showMessage(0, "Transfer successfully.");

            // assert
            expect(cashBalance.$msgContent.hasClass("msg msg-success")).toBeTruthy();
            expect(cashBalance.$msgContainer.hasClass("hidden")).toBeFalsy();
        });

        it("_ErrorCode != 0 -> Show Error Message", () => {
            // act
            cashBalance.showMessage(500, "Internal server error.");

            // assert
            expect(cashBalance.$msgContent.hasClass("msg msg-error")).toBeTruthy();
            expect(cashBalance.$msgContainer.hasClass("hidden")).toBeFalsy();
        });
    });

    describe("_hideMessage", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
        });

        it("_Always_Hide_Message", () => {
            // act
            cashBalance.hideMessage();

            // assert
            expect(cashBalance.$msgContent.val()).toBe("");
            expect(cashBalance.$msgContainer.hasClass("hidden")).toBeTruthy();
        });
    });

    describe("_countDecimals", () => {
        it("_1000 -> 0 Digit", () => {
            // arrange
            let value: number = 1000;

            // act
            let digit: number = cashBalance.countDecimals(value);

            // assert
            expect(digit).toBe(0);
        });

        it("_1000.12 -> 2 Digit", () => {
            // arrange
            let value: number = 1000.12;

            // act
            let digit: number = cashBalance.countDecimals(value);

            // assert
            expect(digit).toBe(2);
        });

        it("_1000.123456 -> 6 Digit", () => {
            // arrange
            let value: number = 1000.123456;

            // act
            let digit: number = cashBalance.countDecimals(value);

            // assert
            expect(digit).toBe(6);
        });
    });

    describe("_amountEqualTo", () => {
        it("_ComparedValue = 0 -> Returns false", () => {
            // arrange
            let amount: number = 248417.2422;
            let comparedValue: number = 0;

            // act
            let actual: boolean = cashBalance.amountEqualTo(amount, comparedValue);

            // assert
            expect(actual).toBeFalsy();
        });

        it("_ComparedValue > 0 -> Returns false", () => {
            // arrange
            let amount: number = 248417.2422;
            let comparedValue: number = 248417.2422;

            // act
            let actual: boolean = cashBalance.amountEqualTo(amount, comparedValue);

            // assert
            expect(actual).toBeFalsy();
        });

        it("_ComparedValue > 0 -> Returns true", () => {
            // arrange
            let amount: number = 248417.25;
            let comparedValue: number = 248417.2402;

            // act
            let actual: boolean = cashBalance.amountEqualTo(amount, comparedValue);

            // assert
            expect(actual).toBeTruthy();
        });

        it("_ComparedValue > 0 -> Returns true", () => {
            // arrange
            let amount: number = 248417.5;
            let comparedValue: number = 248417.4999;

            // act
            let actual: boolean = cashBalance.amountEqualTo(amount, comparedValue);

            // assert
            expect(actual).toBeTruthy();
        });

        it("_ComparedValue < 0 -> Returns false", () => {
            // arrange
            let amount: number = -194585.8811;
            let comparedValue: number = -194585.8811;

            // act
            let actual: boolean = cashBalance.amountEqualTo(amount, comparedValue);

            // assert
            expect(actual).toBeFalsy();
        });

        it("_ComparedValue < 0 -> Returns true", () => {
            // arrange
            let amount: number = -194585.89;
            let comparedValue: number = -194585.8811;

            // act
            let actual: boolean = cashBalance.amountEqualTo(amount, comparedValue);

            // assert
            expect(actual).toBeTruthy();
        });

        it("_ComparedValue < 0 -> Returns true", () => {
            // arrange
            let amount: number = -194585.6;
            let comparedValue: number = -194585.5999;

            // act
            let actual: boolean = cashBalance.amountEqualTo(amount, comparedValue);

            // assert
            expect(actual).toBeTruthy();
        });
    });

    describe("_isNumeric", () => {
        it("_Valid Negative Number -> Returns true", () => {
            // arrange
            let value: string = "-123.98";

            // act
            let actual = cashBalance.isNumeric(value);

            // assert
            expect(actual).toBeTruthy();
        });

        it("_Valid Positive Number -> Returns true", () => {
            // arrange
            let value: string = "123.98";

            // act
            let actual = cashBalance.isNumeric(value);

            // assert
            expect(actual).toBeTruthy();
        });

        it("_Invalid Number -> Returns false", () => {
            // arrange
            let value: string = "123.98abcd";

            // act
            let actual = cashBalance.isNumeric(value);

            // assert
            expect(actual).toBeFalsy();
        });
    });

    describe("_getValidationObject", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
        });

        it("_Amount is Invalid -> Code = 400", () => {
            // arrange
            cashBalance.$amount.val("123.98abcdef");

            // act
            let validationObject: any = cashBalance.getValidationObject();

            // assert
            expect(validationObject.Code).toBe(400);
        });

        it("_Amount < FromValue -> Code = 400", () => {
            // arrange
            cashBalance.$amount.val("-194585.90");

            // act
            let validationObject: any = cashBalance.getValidationObject();

            // assert
            expect(validationObject.Code).toBe(400);
        });

        it("_Amount > ToValue -> Code = 400", () => {
            // arrange
            cashBalance.$amount.val("248417.26");

            // act
            let validationObject: any = cashBalance.getValidationObject();

            // assert
            expect(validationObject.Code).toBe(400);
        });

        it("_Valid Amount -> Code = 0", () => {
            // arrange
            cashBalance.$amount.val("1000.25");

            // act
            let validationObject: any = cashBalance.getValidationObject();

            // assert
            expect(validationObject.Code).toBe(0);
        });

        it("_Amount = FromValue -> Code = 0", () => {
            // arrange
            cashBalance.$amount.val("-194585.89");

            // act
            let validationObject: any = cashBalance.getValidationObject();

            // assert
            expect(validationObject.Code).toBe(0);
        });

        it("_Amount = ToValue -> Code = 0", () => {
            // arrange
            cashBalance.$amount.val("248417.24");

            // act
            let validationObject: any = cashBalance.getValidationObject();

            // assert
            expect(validationObject.Code).toBe(0);
        });
    });

    describe("_afterAddTransferSuccessfully", () => {
        it("_Code = 0 -> Show Successful Message", () => {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
            spyOn(topWindow, "OnPopupComplete");

            let data: any = { Code: 0, Message: "Transfer successfully." };

            // act
            cashBalance.afterAddTransferSuccessfully(data);

            // assert
            expect(cashBalance.topWindow.OnPopupComplete).toHaveBeenCalled();
        });

        it("_Code = 105 -> Show Error Message & Reload Page After 3 seconds", () => {
            // arrange
            jasmine.clock().install();
            spyOn(location, "reload");

            setFixtures(template.normal);
            cashBalance.cacheDOMElements();

            let data: any = { Code: 105, Message: "" };

            // act
            cashBalance.afterAddTransferSuccessfully(data);
            jasmine.clock().tick(3001);

            // assert
            expect(location.reload).toHaveBeenCalled();

            jasmine.clock().uninstall();
        });

        it("_Code != 0 -> Do NOT Reload Page", () => {
            // arrange
            spyOn(location, "reload");

            setFixtures(template.normal);
            cashBalance.cacheDOMElements();

            let data: any = { Code: 400, Message: "Bad request." };

            // act
            cashBalance.afterAddTransferSuccessfully(data);

            // assert
            expect(location.reload).not.toHaveBeenCalled();
        });
    });

    describe("_doTransfer", () => {
        it("_ValidationObject.Code != 0 -> Show Error Message", () => {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
            cashBalance.$amount.val("250,417.24");
            let e: any = { preventDefault: function () { } };

            spyOn(cashBalance, "showMessage");

            // act
            cashBalance.doTransfer(e);

            // assert
            expect(cashBalance.showMessage).toHaveBeenCalled();
        });

        it("_ValidationObject.Code = 0 & Confirm Transfer Cancel -> Do Nothing", () => {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
            cashBalance.$amount.val("100,000.00");
            let e: any = { preventDefault: function () { } };

            spyOn(window, "confirm").and.returnValue(false);
            spyOn(jQueryFake, "unblockUI").and.returnValue(false);

            // act
            cashBalance.doTransfer(e);

            // assert
            expect(window.confirm).toHaveBeenCalled();
            expect(jQueryFake.unblockUI).toHaveBeenCalled();
        });

        it("_ValidationObject.Code = 0 & Confirm Transfer OK -> Make a Post", () => {
            // arrange
            setFixtures(template.normal);
            cashBalance.cacheDOMElements();
            cashBalance.$amount.val("100,000.00");
            let e: any = { preventDefault: function () { } };

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
