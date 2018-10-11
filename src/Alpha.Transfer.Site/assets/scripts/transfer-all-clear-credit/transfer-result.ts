"use strict";
declare var initAutoComplete: any;

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

    topWindow: any;
    clearCreditConfirm: string;
    transferAllConfirm: string;

    constructor($: JQueryStatic,
        topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cacheElements();

        var queryUsernameURL = this.$usernameAutoSuggestionPath.val() + "?" +
            this.$.param({
                isDir: 1,
                issub: 0,
                ismember: 0
            });

        initAutoComplete(this.$username[0], queryUsernameURL);
        this.$output.css("top", this.$username.offset().top + 30);
        this.$submit.click(this.submitTransferAndClearCredit);
        this.$("#transfer-clear-credit-form").submit((e) => {
            if (this.$username.val().trim() == "") {
                e.preventDefault();
            }
        });

        this.$username.keyup(() => {
            if (this.$username.val().trim() == "") {
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
        this.$chkClearCredit = this.$("#chk-clear-credit");
        this.$IsClearCredit = this.$("#IsClearCredit");
        this.$usernameTransfer = this.$("#txt-username-transfer");

        this.clearCreditConfirm = this.$("#ClearCreditConfirm").val();
        this.transferAllConfirm = this.$("#TransferAllConfirm").val();
    };
    submitTransferAndClearCredit = () => {
        var isClearCredit = this.$chkClearCredit.is(":checked");
        var message = isClearCredit ? this.clearCreditConfirm : this.transferAllConfirm;
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