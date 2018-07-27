"use strict";

class CashBalance {
    $: JQueryStatic;
    topWindow: any;

    availableAmountFrom: number;
    availableAmountTo: number;
    oldAmount: number = 0;
    availableRangeTransfer: string;
    confirmTransfer: string;
    transferAmountIs: string;
    invalidValue: string;

    $msgContainer: JQuery;
    $msgContent: JQuery;
    $formTransfer: JQuery;
    $amount: JQuery;
    $custId: JQuery;
    $userName: JQuery;
    $token: JQuery;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cacheDOMElements();
        this.registerEvents();
    };

    cacheDOMElements = () => {
        this.$msgContainer = this.$("#status-message");
        this.$msgContent = this.$("#status-message-content");
        this.$formTransfer = this.$("#form-transfer");
        this.$amount = this.$("#amount");
        this.$custId = this.$("#custId");
        this.$token = this.$("input[name=\"__RequestVerificationToken\"]", this.$formTransfer);

        this.availableAmountFrom = parseFloat(this.$("#availableAmountFrom").val());
        this.availableAmountTo = parseFloat(this.$("#availableAmountTo").val());
        this.availableRangeTransfer = this.$("#availableRangeTransfer").val();
        this.confirmTransfer = this.$("#confirmTransfer").val();
        this.transferAmountIs = this.$("#transferAmountIs").val();
        this.invalidValue = this.$("#invalidValue").val();

        this.$amount.focus();
    };

    registerEvents = () => {
        this.$formTransfer.submit(this.doTransfer);
        this.$amount.keydown(this.amountKeyDown)
            .keyup(this.amountKeyUp);
    };

    amountKeyDown = (e: any) => {
        // allow: backspace, delete, tab, escape, enter, - and .
        if (this.$.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190, 231, 189, 109, 173]) !== -1 ||
            // allow: Ctrl/cmd+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
            // allow: Ctrl/cmd+C
            (e.keyCode === 67 && (e.ctrlKey === true || e.metaKey === true)) ||
            // allow: Ctrl/cmd+X
            (e.keyCode === 88 && (e.ctrlKey === true || e.metaKey === true)) ||
            // allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }

        // ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    };

    amountKeyUp = (e: any) => {
        let amountValue: string = this.$amount.val();

        if (amountValue.length === 0 || amountValue === "-") {
            return;
        }

        let amount: number = parseFloat(amountValue.replace(/\,/g, ""));

        if (!isNaN(amount) && amount !== this.oldAmount) {
            this.oldAmount = amount;
            this.$amount.val(this.toString4Digits(amount));
        }
    };

    validateAmount = (amount: number): boolean => {
        return this.availableAmountFrom <= amount && amount <= this.availableAmountTo && amount !== 0;
    };

    showMessage = (code: number, message: string) => {
        var className: string = code === 0 ? "msg msg-success" : "msg msg-error";

        this.$msgContent.removeClass().html(message).addClass(className);
        this.$msgContainer.removeClass("hidden");
    };

    hideMessage = () => {
        this.$msgContainer.addClass("hidden");
        this.$msgContent.removeClass().html("");
    };

    countDecimals = (value: number): number => {
        return Math.floor(value) === value ?
            0 :
            value.toString().split(".")[1].length || 0;
    };

    amountEqualTo = (amount: number, comparedValue: number): boolean => {
        if (comparedValue !== 0 && amount !== comparedValue) {
            let decimals: number = this.countDecimals(amount);

            if (decimals === 1 || decimals === 2) {
                return comparedValue > 0 ?
                    Math.floor(amount * 100) === Math.ceil(comparedValue * 100) :
                    Math.floor(amount * 100) === Math.floor(comparedValue * 100);
            }
        }

        return false;
    };

    toString4Digits = (value: number): string => {
        return value.toLocaleString("en-US", { maximumFractionDigits: 4 });
    };

    isNumeric = (value: string) => {
        value = value.replace(/\,/g, "");
        var regExp = /^-?(\d+|\d+\.\d*|\d*\.\d+)$/;
        return value.match(regExp);
    };

    getValidationObject = (): any => {
        let validationObject: any = {};
        let amountValue: string = this.$amount.val();

        if (amountValue.length !== 0 && !this.isNumeric(amountValue)) {
            validationObject.code = 400;
            validationObject.message = this.invalidValue;
            return validationObject;
        }

        let confirmMessage: string = "";
        let amount: number = amountValue.length === 0 ? 0 : parseFloat(amountValue.replace(/\,/g, ""));

        if (this.amountEqualTo(amount, this.availableAmountFrom)) {
            amount = this.availableAmountFrom;
            confirmMessage = this.transferAmountIs + " " + this.toString4Digits(amount) + "\n";
        } else if (this.amountEqualTo(amount, this.availableAmountTo)) {
            amount = this.availableAmountTo;
            confirmMessage = this.transferAmountIs + " " + this.toString4Digits(amount) + "\n";
        }

        if (confirmMessage.length === 0 && !this.validateAmount(amount)) {
            validationObject.code = 400;
            validationObject.message = this.availableRangeTransfer;
        } else {
            validationObject.code = 0;

            confirmMessage += this.confirmTransfer;
            validationObject.message = confirmMessage;

            validationObject.amount = amount;
        }

        return validationObject;
    };

    afterAddTransferSuccessfully = (data: any) => {
        this.showMessage(data.Code, data.Message);

        if (data.Code === 105) {
            setTimeout(function () {
                location.reload(true);
            }, 3000);
        } else if (data.Code === 0) {
            this.topWindow.OnPopupComplete(true);
        }
    };

    doTransfer = (e: any) => {
        e.preventDefault();

        this.$.blockUI();

        let validationObject: any = this.getValidationObject();

        if (validationObject.code !== 0) {
            this.showMessage(validationObject.code, validationObject.message);
            this.$.unblockUI();
            return;
        }

        this.hideMessage();

        if (!confirm(validationObject.message)) {
            this.$.unblockUI();
            return;
        }

        let postParams: any = {
            __RequestVerificationToken: this.$token.val(),
            custId: this.$custId.val(),
            amount: validationObject.amount
        };

        var self: any = this;

        this.$.post(this.$formTransfer.attr("action"), postParams, this.afterAddTransferSuccessfully, "json")
            .always(function () {
                self.$.unblockUI();
            });
    };
}

$(() => {
    let cashBalance: any = new CashBalance($, window.top);
    cashBalance.init();
});