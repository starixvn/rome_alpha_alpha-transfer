"use strict";
declare var initAutoComplete: Function;
interface Window {
    bootboxConfirm(message: string, handleElement: object, acceptCallback: Function, cancelCallback: Function): void;
}

class TransferResult {
    $: JQueryStatic;
    $username: JQuery;
    $usernameAutoSuggestionPath: JQuery;
    $output: JQuery;
    $submit: JQuery;
    $transferResultForm: JQuery;
    $chkClearCredit: JQuery;
    $IsClearCredit: JQuery;
    $usernameTransfer: JQuery;

    topWindow: Window;
    clearCreditConfirm: string;
    transferAllConfirm: string;

    constructor($: JQueryStatic, topWindow: Window) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cacheElements();

        const queryUsernameURL = `${this.$usernameAutoSuggestionPath.val()}?${this.$.param({ isDir: 1, ismember: 0, issub: 0 })}`;

        initAutoComplete(this.$username[0], queryUsernameURL);

        const positionTop = 30;
        this.$output.css("top", this.$username.offset().top + positionTop);
        this.$submit.click(this.submitTransferAndClearCredit);
        this.$("#transfer-clear-credit-form").submit((e) => {
            if (this.$username.val().trim() === "") {
                e.preventDefault();
            }
        });

        this.$username.keyup(() => {
            if (this.$username.val().trim() === "") {
                this.$username.addClass("text-italic");
            } else {
                this.$username.removeClass("text-italic");
            }
        });
    };

    cacheElements = () => {
        this.$username = this.$("#txt-username");
        this.$usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath");
        this.$output = this.$("#output");
        this.$submit = this.$("#btn-submit");

        this.$transferResultForm = this.$("#transfer-result-form");
        this.$IsClearCredit = this.$("#IsClearCredit");
        this.$chkClearCredit = this.$("#chk-clear-credit");
        this.$usernameTransfer = this.$("#txt-username-transfer");

        this.clearCreditConfirm = this.$("#ClearCreditConfirm").val();
        this.transferAllConfirm = this.$("#TransferAllConfirm").val();
    };
    submitTransferAndClearCredit = () => {
        const isClearCredit = this.$chkClearCredit.is(":checked");
        const message = isClearCredit ? this.clearCreditConfirm : this.transferAllConfirm;
        this.topWindow.bootboxConfirm(message, null, () => {
            this.$.blockUI();
            this.$IsClearCredit.val(isClearCredit.toString());
            this.$transferResultForm.submit();
        });
    };
}

const transferResult = new TransferResult($, top);

$(() => {
    transferResult.init();
});
