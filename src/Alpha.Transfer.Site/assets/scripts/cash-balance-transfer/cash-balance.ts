"use strict";

interface Window {
    indexManager: any;
    main: any;
    OnPopupComplete(isReload: boolean): void;
}

class KeyboardValue {
    readonly Delete = 46;
    readonly BackSpace = 8;
    readonly Tab = 9;
    readonly Escape = 27;
    readonly Enter = 13;
    readonly LowercaseN = 110;
    readonly YEN = 190;
    readonly Thron = 231;
    readonly Cent = 189;
    readonly LowercaseM = 46;
    readonly ExclamationMarks = 173;

    readonly CapitalA = 65;
    readonly CapitalC = 67;
    readonly CapitalX = 88;

    readonly End = 35;
    readonly Right = 39;
    readonly Number0 = 48;
    readonly Number9 = 57;
    readonly Numpad0 = 96;
    readonly Numpad9 = 105;
    readonly NegativeKeyCode = 109;

    isEditNumber = function (keyCode: number, isCtrl: boolean) {
        const editNumberKeys =
            [this.Delete, this.BackSpace, this.Tab, this.Escape, this.Enter, this.LowercaseN,
            this.YEN, this.Thron, this.Cent, this.LowercaseM, this.ExclamationMarks];

        return editNumberKeys.indexOf(keyCode) !== -1 ||
            ([this.CapitalA, this.CapitalC, this.CapitalX].indexOf(keyCode) !== -1 && isCtrl) ||
            // allow: home, end, left, right
            (this.End <= keyCode && keyCode <= this.Right);
    };
}

class Result {
    Code: number;
    Message: string;
    Amount: number;
}

class CashBalance {
    $: JQueryStatic;
    topWindow: Window;

    availableAmountFrom: number;
    availableAmountTo: number;
    oldAmount = 0;
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

    keyboardValue = new KeyboardValue();

    constructor($: JQueryStatic, topWindow: Window) {
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

    amountKeyDown = (e: JQueryKeyEventObject) => {
        // allow: backspace, delete, tab, escape, enter, - and .
        const isCtrl = e.ctrlKey === true || e.metaKey === true;
        if (this.keyboardValue.isEditNumber(e.keyCode, isCtrl)) {
            // let it happen, don't do anything
            return;
        }

        // ensure that it is not a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < this.keyboardValue.Number0 || this.keyboardValue.Number9 < e.keyCode))
            && (e.keyCode < this.keyboardValue.Numpad0 || this.keyboardValue.Numpad9 < e.keyCode)) {
            e.preventDefault();
        }
    };

    amountKeyUp = (e: JQueryKeyEventObject) => {
        let amountValue = this.$amount.val();

        if (e.keyCode === this.keyboardValue.NegativeKeyCode) {
            amountValue = `-${amountValue}`;
            this.$amount.val('-');
        }

        if (amountValue.length === 0 || amountValue === "-") {
            return;
        }

        const amount = parseFloat(amountValue.replace(/\,/g, ""));

        if (!isNaN(amount) && amount !== this.oldAmount) {
            this.oldAmount = amount;
            this.$amount.val(this.toString4Digits(amount));
        }
    };

    validateAmount = (amount: number): boolean => {
        return this.availableAmountFrom <= amount && amount <= this.availableAmountTo && amount !== 0;
    };

    showMessage = (code: number, message: string) => {
        const className: string = code === 0 ? "msg msg-success" : "msg msg-error";

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
            const decimals: number = this.countDecimals(amount);
            const decimalDigits = 2;
            const percent = 100;

            if (decimals === 1 || decimals === decimalDigits) {
                return comparedValue > 0 ?
                    Math.floor(amount * percent) === Math.ceil(comparedValue * percent) :
                    Math.floor(amount * percent) === Math.floor(comparedValue * percent);
            }
        }

        return false;
    };

    toString4Digits = (value: number): string => {
        return value.toLocaleString("en-US", { maximumFractionDigits: 4 });
    };

    isNumeric = (value: string) => {
        value = value.replace(/\,/g, "");
        const regExp = /^-?(\d+|\d+\.\d*|\d*\.\d+)$/;
        return value.match(regExp);
    };

    getValidationObject = (): Result => {
        const validationObject = new Result();
        const amountValue: string = this.$amount.val();
        const isNotNumberCode = 400;

        if (amountValue.length !== 0 && !this.isNumeric(amountValue)) {
            validationObject.Code = isNotNumberCode;
            validationObject.Message = this.invalidValue;
            return validationObject;
        }

        let confirmMessage = "";
        let amount = amountValue.length === 0 ? 0 : parseFloat(amountValue.replace(/\,/g, ""));

        if (this.amountEqualTo(amount, this.availableAmountFrom)) {
            amount = this.availableAmountFrom;
            confirmMessage = this.transferAmountIs + " " + this.toString4Digits(amount) + "\n";
        } else if (this.amountEqualTo(amount, this.availableAmountTo)) {
            amount = this.availableAmountTo;
            confirmMessage = this.transferAmountIs + " " + this.toString4Digits(amount) + "\n";
        }

        if (confirmMessage.length === 0 && !this.validateAmount(amount)) {
            validationObject.Code = isNotNumberCode;
            validationObject.Message = this.availableRangeTransfer;
        } else {
            validationObject.Code = 0;

            confirmMessage += this.confirmTransfer;
            validationObject.Message = confirmMessage;

            validationObject.Amount = amount;
        }

        return validationObject;
    };

    afterAddTransferSuccessfully = (data: Result) => {
        if (data.Code === 6001) {
            this.topWindow.indexManager.addCustomCookies('messageForReloadSite', data.Message);
            this.topWindow.indexManager.updateCurrentUrl("adbt=False");
            this.topWindow.location.reload();
        } else {
            this.showMessage(data.Code, data.Message);

            const error = 105;
            const reloadTime = 3000;
            if (data.Code === error) {
                setTimeout(function () { location.reload(); }, reloadTime);
            } else if (data.Code === 0) {
                this.topWindow.OnPopupComplete(true);
            }
        }
    };

    doTransfer = (e: JQueryEventObject) => {
        e.preventDefault();

        this.$.blockUI();

        const validationObject = this.getValidationObject();

        if (validationObject.Code !== 0) {
            this.showMessage(validationObject.Code, validationObject.Message);
            this.$.unblockUI();
            return;
        }

        this.hideMessage();

        if (!confirm(validationObject.Message)) {
            this.$.unblockUI();
            return;
        }

        const postParams = {
            __RequestVerificationToken: this.$token.val(),
            custId: this.$custId.val(),
            amount: validationObject.Amount
        };

        const this$ = this.$;

        this.$.post(this.$formTransfer.attr("action"), postParams, this.afterAddTransferSuccessfully, "json")
            .always(function () { this$.unblockUI(); });
    };
}

$(() => {
    const cashBalance = new CashBalance($, window.top);
    cashBalance.init();
});
