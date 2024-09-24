'use strict';

class RewardPaymentView {
    private filter: FilterComponent;
    private dateRangePicker: DateRangePickerJQueryComponent;
    private programDropdown: SingleSelectionDropdownComponent;
    private paymentStatusDropdown: SingleSelectionDropdownComponent;
    private usernameAutoSuggestion: UsernameAutoSuggestionComponent;
    private sorter: SortComponent;
    private tableContainer: JQuery;

    constructor(
        filter: FilterComponent,
        dateRangePicker: DateRangePickerJQueryComponent,
        programDropdown: SingleSelectionDropdownComponent,
        paymentStatusDropdown: SingleSelectionDropdownComponent,
        usernameAutoSuggestion: UsernameAutoSuggestionComponent,
        sorter: SortComponent) {
        this.filter = filter;
        this.dateRangePicker = dateRangePicker;
        this.programDropdown = programDropdown;
        this.paymentStatusDropdown = paymentStatusDropdown;
        this.usernameAutoSuggestion = usernameAutoSuggestion;
        this.sorter = sorter;
    }

    init() {
        this.tableContainer = $('#tbl-transfer-container');
        this.initComponents();

        const $iconExcel = $('#icon-excel').tooltip();
        const excelUrl = $iconExcel.attr('href');
        $iconExcel.click(() => {
            $iconExcel.attr('href', `${excelUrl}?${$.param(this.getSearchData())}`);
            setTimeout(function () {
                $iconExcel.attr('href', 'javascript:;');
            }, 1000);
        }).attr('href', 'javascript:;');

        this.bindTableEvents();
        alphaTransfer.showFeedback(10);
    }

    private initComponents() {
        this.filter.initShowHide({
            hideText: $('#hiddenHideFilterText').val(),
            showText: $('#hiddenShowFilterText').val(),
            callback: (isHideFilter) => {
                const hasConfirmPayment = $('#btn-confirmPayment').length !== 0;
                if (isHideFilter) {
                    hasConfirmPayment
                        ? this.tableContainer.css('margin-top', '40px')
                        : this.tableContainer.css('margin-top', '0')
                } else {
                    hasConfirmPayment
                        ? this.tableContainer.css('margin-top', '25px')
                        : this.tableContainer.css('margin-top', '0')
                }
            }
        });

        this.dateRangePicker.render({
            onClosed: () => this.getRewardReport(),
            language: top.$('#language').data('currentlanguage')
        });

        this.programDropdown.render(new SingleSelectionDropdownOption('ddl-program', undefined, () => this.getRewardReport(), true));
        this.paymentStatusDropdown.render(new SingleSelectionDropdownOption('ddl-paymentStatus', undefined, () => this.getRewardReport()));

        this.usernameAutoSuggestion.init({
            url: $("#hiddenUsernameAutoSuggestionUrl").val(),
            directDownlines: true,
            onBlur: () => this.getRewardReport()
        });
    }

    private getRewardReport() {
        $.blockUI();
        const queryString = $.param(this.getSearchData());
        $.get(`GetPayment?${queryString}`, (html) => {
            this.tableContainer.html(html);
            this.bindTableEvents();
            alphaTransfer.updateCurrentUrl(queryString);
        }).always($.unblockUI);
    }

    private getSearchData() {
        let searchData: any = this.dateRangePicker.getDateRangeValues();

        const username = this.usernameAutoSuggestion.getUsername();
        if (username) {
            searchData.username = username;
        }

        const programId = this.programDropdown.getSelectedItem();
        if (programId !== '0') {
            searchData.programId = programId;
        }

        const paymentStatus = this.paymentStatusDropdown.getSelectedItem();
        if (paymentStatus !== '0') {
            searchData.paymentStatus = paymentStatus;
        }

        return searchData;
    }

    private bindTableEvents() {
        const $chkAllPaymentStatus = $('#chk-allPaymentStatus');
        const $checkboxPaymentStatus = $('.checkbox-payment-status');
        const $btnConfirmPayment = $('#btn-confirmPayment');

        if ($checkboxPaymentStatus.length) {
            this.tableContainer.css('margin-top', '25px');

            $chkAllPaymentStatus.off().change(function () {
                const checked = this.checked;
                $checkboxPaymentStatus.prop('checked', checked);
                $btnConfirmPayment.prop('disabled', !checked)
            });

            $checkboxPaymentStatus.off().change(function () {
                const allCheckboxesLength = $checkboxPaymentStatus.length;
                const checkedCheckboxesLength = $checkboxPaymentStatus.filter(':checked').length;
                const isSelectAll = checkedCheckboxesLength > 0 && checkedCheckboxesLength === allCheckboxesLength;
                const isIndeterminate = !isSelectAll && checkedCheckboxesLength > 0;
                $chkAllPaymentStatus.prop({
                    indeterminate: isIndeterminate,
                    checked: isSelectAll
                });
                $btnConfirmPayment.prop('disabled', checkedCheckboxesLength === 0);
            });

            $btnConfirmPayment.off().click(() => {
                const data = [];
                $checkboxPaymentStatus.filter(':checked').each(function () {
                    const array = $(this).attr('id').split('-');
                    if (array.length === 4) {
                        data.push({ custId: array[0], username: array[1], programId: array[2], maxTransId: array[3] })
                    }
                });

                if (data.length) {
                    const confirmMsg = $('#hiddenUpdateRewardPaymentStatusConfirm').val();
                    const formData = {
                        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
                        data: data
                    };

                    if (window.top.bootboxConfirm) {
                        window.top.bootboxConfirm(
                            confirmMsg,
                            undefined,
                            () => this.handleUpdatePaymentStatus(formData));
                    } else if (confirm(confirmMsg)) {
                        this.handleUpdatePaymentStatus(formData);
                    }
                }
            });
        } else {
            this.tableContainer.css('margin-top', '0')
            $chkAllPaymentStatus.remove();
            $btnConfirmPayment.remove();
        }

        const $tblTransfer = $('#tbl-transfer');
        this.sorter.tableSorter({ tableElement: $tblTransfer })
        $tblTransfer.stickyTableHeaders('destroy');
        $tblTransfer.stickyTableHeaders();
    }

    private handleUpdatePaymentStatus(formData) {
        $.blockUI();
        $.post('UpdatePaymentStatus', formData, (result) => {
            if (result.Code && result.Message) {
                const handler = window.top.bootboxAlert || alert;
                handler(result.Message);
                return;
            }

            if (result.IsKickout) {
                window.top.alpha.core.signOut2();
                return;
            }

            setTimeout(() => this.getRewardReport(), 2000);
            this.showUpdatePaymentStatusResult(result);
        }, 'json').always($.unblockUI);
    }

    private showUpdatePaymentStatusResult(result) {
        const modal = $('#modal-confirmPayment');
        const contentShowMore = modal.find('#modal-body-showMore').hide();

        const html = [];
        const maxErrorDisplay = 5;
        const errorLength = result.Errors ? result.Errors.length : 0;

        if (errorLength) {
            if (result.TotalRecords !== errorLength) {
                html.push('<p>', $('#hiddenUpdateManyRewardPaymentStatusSuccessfully').val(), '</p>')
            }

            result.Errors.forEach(function (item, index) {
                html.push('<p class="text-red"', index >= maxErrorDisplay ? ' style="display:none"' : '', '>', item, '</p>')
            });

            if (errorLength >= maxErrorDisplay) {
                contentShowMore.show();
                contentShowMore.find('#link-showMore')
                    .off()
                    .click(function () {
                        modal.find('#modal-body-content .text-red:hidden').show();
                        contentShowMore.hide();
                    });
            }
        } else {
            html.push(result.TotalRecords === 1
                ? $('#hiddenUpdateRewardPaymentStatusSuccessfully').val()
                : $('#hiddenUpdateAllRewardPaymentStatusSuccessfully').val());
        }
        
        modal.find('#modal-body-content').html(html.join(''));
        modal.modal({ backdrop: false });
    }
}

$(function () {
    new RewardPaymentView(
        new FilterComponent(),
        new DateRangePickerJQueryComponent(),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new UsernameAutoSuggestionComponent(),
        new SortComponent()).init();
});
