"use strict";
declare var initAutoComplete: Function;
interface Window {
    bootboxConfirm(message: string, handleElement: object, acceptCallback: Function): void;
}

class TransferAllAndClearCredit {
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
    canTransfer: boolean;
    canClearCredit: boolean;

    constructor($: JQueryStatic, topWindow: Window) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cacheElements();

        const queryUsernameURL = `${this.$usernameAutoSuggestionPath.val()}?${this.$.param({ isDir: 1, issub: 0, ismember: 0 })}`;

        initAutoComplete(this.$username[0], queryUsernameURL);

        const positionTop = 30;
        this.$output.css("top", this.$username.offset().top + positionTop);

        this.$username.keyup(() => {
            if (this.$username.val().trim() === "") {
                this.$username.addClass("text-italic");
            } else {
                this.$username.removeClass("text-italic");
            }
        });
        if (!this.canTransfer && this.canClearCredit) {
            this.$chkClearCredit.click(() => {
                if (this.$chkClearCredit.is(":checked")) {
                    this.$submit.attr("disabled", null);
                    this.$submit.click(this.submitTransferAndClearCredit);
                } else {
                    this.$submit.attr("disabled", "disabled");
                    this.$submit.off("click");
                }
            });
        } else {
            this.$submit.click(this.submitTransferAndClearCredit);
        }
    };

    cacheElements = () => {
        this.$username = this.$("#txt-username");
        this.$usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath");
        this.$output = this.$("#output");
        this.$submit = this.$("#btn-submit");

        this.$transferResultForm = this.$("#transfer-result-form");
        this.$chkClearCredit = this.$("#chk-clear-credit");
        this.$IsClearCredit = this.$("#IsClearCredit");
        this.$usernameTransfer = this.$("#txt-username-transfer");

        this.clearCreditConfirm = this.$("#ClearCreditConfirm").val();
        this.transferAllConfirm = this.$("#TransferAllConfirm").val();

        this.$("#transfer-clear-credit-form").submit((e) => {
            if (this.$username.val().trim() === "") {
                e.preventDefault();
            }
        });
        this.canTransfer = this.$("#CanTransfer").val() === "1";
        this.canClearCredit = this.$("#CanClearCredit").val() === "1";
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

const transferAllAndClearCredit = new TransferAllAndClearCredit($, top);

$(() => {
    transferAllAndClearCredit.init();
});
