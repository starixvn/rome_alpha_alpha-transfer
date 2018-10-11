"use strict";
declare var initAutoComplete: any;

class CustomerList {
    $: JQueryStatic;
    $username: JQuery;
    $usernameAutoSuggestionPath: JQuery;
    $output: JQuery;
    $submit: JQuery;
    $statusComponent: SingleSelectionDropdownComponent;
    $status: JQuery;
    $customerListForm: JQuery;
    $AllYesterdayBalance: JQuery;
    $chkAllYesterdayBalance: JQuery;
    $information: JQuery;
    transferInformationPath: string;

    topWindow: any;

    constructor($: JQueryStatic,
        topWindow: any,
        statusComponent: SingleSelectionDropdownComponent) {
        this.$ = $;
        this.topWindow = topWindow;
        this.$statusComponent = statusComponent;
    }

    init = () => {
        this.cacheElements();

        var queryUsernameURL = this.$usernameAutoSuggestionPath.val() + "?" +
            this.$.param({
                issub: 0,
                ismember: 0
            });

        initAutoComplete(this.$username[0], queryUsernameURL);
        this.$output.css("top", this.$username.offset().top + 30);

        this.$statusComponent.render(
            new SingleSelectionDropdownOption("dropdown-status", "Status", this.changeStatus));

        this.$information.click(this.showInformation);

        this.$customerListForm.submit(this.submitForm);
    };

    cacheElements = () => {
        this.$username = this.$("#txt-username");
        this.$usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath");
        this.$output = this.$("#output");
        this.$submit = this.$("#btn-submit");

        this.$status = this.$("#status");
        this.$customerListForm = this.$("#customer-list-form");
        this.$AllYesterdayBalance = this.$("#AllYesterdayBalance");
        this.$chkAllYesterdayBalance = this.$("#chk-all-yesterday-balance");
        this.$information = this.$("#icon-information");
        this.transferInformationPath = this.$("#TransferInformationPath").val();
    };
    changeStatus = () => {
        this.$statusComponent.updateSelectedValue();
        this.$submit.click();
    };
    submitForm = () => {
        this.$AllYesterdayBalance.val(this.$chkAllYesterdayBalance.is(":checked") ? "true" : "false");
    };
    showInformation = () => {
        this.topWindow.popupManager.open(this.transferInformationPath, "", 560, 400);
    };
}

const customerList = new CustomerList($, top, new SingleSelectionDropdownComponent($));

$(() => {
    customerList.init();
});