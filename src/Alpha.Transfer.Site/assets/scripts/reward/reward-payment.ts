'use strict';

class RewardPaymentView {
    private filter: FilterComponent;
    private dateRangePicker: DateRangePickerJQueryComponent;
    private programDropdown: FanexMultiSelectComponent;
    private paymentStatusDropdown: SingleSelectionDropdownComponent;
    private usernameAutoSuggestion: UsernameAutoSuggestionComponent;
    private sorter: SortComponent;
    private tableContainer: JQuery;

    constructor(
        filter: FilterComponent,
        dateRangePicker: DateRangePickerJQueryComponent,
        programDropdown: FanexMultiSelectComponent,
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
            const searchData = this.getSearchData();

            if (searchData) {
                $iconExcel.attr('href', `${excelUrl}?${$.param(searchData)}`);
                setTimeout(function () {
                    $iconExcel.attr('href', 'javascript:;');
                }, 1000);
            }
        }).attr('href', 'javascript:;');

        this.bindTableEvents();
        alphaTransfer.showFeedback(10);
    }

    private initComponents() {
        this.filter.initShowHide({
            hideText: $('#hiddenHideFilterText').val(),
            showText: $('#hiddenShowFilterText').val()
        });

        this.dateRangePicker.render({
            onClosed: () => this.getRewardReport(),
            language: top.$('#language').data('currentlanguage')
        });

        this.programDropdown.render({ selectorId: 'ddl-program', showFilter: true, onDropdownHidden: () => this.getRewardReport() });
        this.paymentStatusDropdown.render(new SingleSelectionDropdownOption('ddl-paymentStatus', undefined, () => this.getRewardReport()));

        this.usernameAutoSuggestion.init({
            url: $("#hiddenUsernameAutoSuggestionUrl").val(),
            directDownlines: true,
            onBlur: () => this.getRewardReport()
        });
    }

    private getRewardReport() {
        const searchData = this.getSearchData();

        if (searchData) {
            $.blockUI();
            const queryString = $.param(searchData);
            $.get(`GetPayment?${queryString}`, (html) => {
                this.tableContainer.empty().html(html);
                this.bindTableEvents();
                alphaTransfer.updateCurrentUrl(queryString);
            }).always($.unblockUI);
        }
    }

    private getSearchData() {
        let searchData: any = this.dateRangePicker.getDateRangeValues();

        const username = this.usernameAutoSuggestion.getUsername();
        if (username) {
            searchData.username = username;
        }

        const programIds = this.programDropdown.getCheckedValues();
        const isValidProgramIds = programIds.length > 0;
        this.programDropdown.toogleError(!isValidProgramIds);
        if (!isValidProgramIds) {
            return;
        }

        searchData.programIds = programIds;
        const paymentStatus = this.paymentStatusDropdown.getSelectedItem();
        if (paymentStatus !== '0') {
            searchData.paymentStatus = paymentStatus;
        }

        return searchData;
    }

    private bindTableEvents() {
        const $chkAllPaymentStatus = $('#chk-allPaymentStatus');
        const $checkboxUnpaid = $('.checkbox-unpaid');
        const $btnConfirmPayment = $('#btn-confirmPayment');

        if ($checkboxUnpaid.length) {
            $chkAllPaymentStatus.off().change(function () {
                const checked = this.checked;
                $checkboxUnpaid.prop('checked', checked);
                $btnConfirmPayment.prop('disabled', !checked)
            });

            $checkboxUnpaid.off().change(function () {
                const allCheckboxesLength = $checkboxUnpaid.length;
                const checkedCheckboxesLength = $checkboxUnpaid.filter(':checked').length;
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
                $checkboxUnpaid.filter(':checked').each(function () {
                    const array = $(this).data('unpaid').split('-');
                    if (array.length === 4) {
                        data.push({ custId: array[0], username: array[1], programId: array[2], maxTransId: array[3] })
                    }
                });

                data.length && this.confirmAndHandleUpdatePaymentStatus(data);
            });

            $('.button-unpaid').off().click((e) => {
                const $this = $(e.target);
                const array = $this.data('unpaid').split('-');
                if (array.length === 4) {
                    const unpaidData = { custId: array[0], username: array[1], programId: array[2], maxTransId: array[3] }
                    this.confirmAndHandleUpdatePaymentStatus([unpaidData]);
                }
            });
        } else {
            $chkAllPaymentStatus.prop('disabled', true);
            $btnConfirmPayment.prop('disabled', true);
        }

        const $tblTransfer = $('#tbl-transfer');
        this.sorter.tableSorter({ tableElement: $tblTransfer })
        $tblTransfer.stickyTableHeaders('destroy');
        $tblTransfer.stickyTableHeaders();
    }

    private confirmAndHandleUpdatePaymentStatus(data) {
        const confirmMsg = $('#hiddenUpdateRewardPaymentStatusConfirm').val();
        const formData = $.extend({}, {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
            data: data
        }, this.dateRangePicker.getDateRangeValues());

        if (window.top.bootboxConfirm) {
            window.top.bootboxConfirm(
                confirmMsg,
                undefined,
                () => this.handleUpdatePaymentStatus(formData));
        } else if (confirm(confirmMsg)) {
            this.handleUpdatePaymentStatus(formData);
        }
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
        }, 'json')
            .fail((jqXHR) => {
                if (jqXHR.status === 500) {
                    const mainRootPath = window.top.alpha ? window.top.alpha.urls.mainRootPath : '/site-main/'
                    window.location.href = `${mainRootPath}error`;
                }
            })
            .always($.unblockUI);
    }

    private showUpdatePaymentStatusResult(result) {
        const errorLength = result.Errors ? result.Errors.length : 0;

        if (errorLength) {
            const $modal = $('#modal-confirmPayment');
            const $contentShowMore = $modal.find('#modal-body-showMore').hide();
            const html = [];
            const maxErrorDisplay = 5;

            if (result.TotalRecords !== errorLength) {
                html.push('<p class="text-success">', $('#hiddenUpdateManyRewardPaymentStatusSuccessfully').val(), '</p>')
            }

            result.Errors.forEach(function (item, index) {
                html.push('<p class="text-error"', index >= maxErrorDisplay ? ' style="display:none"' : '', '>', item, '</p>')
            });

            if (errorLength >= maxErrorDisplay) {
                $contentShowMore.show();
                $contentShowMore.find('#link-showMore')
                    .off()
                    .click(function () {
                        $modal.find('#modal-body-content .text-error:hidden').show();
                        $contentShowMore.hide();
                    });
            }

            $modal.find('#modal-body-content').html(html.join(''));
            $modal.modal({ backdrop: false });
        } else {
            if ($('.tableFloatingHeaderOriginal').css('position') === 'fixed') {
                $([document.documentElement, document.body]).animate({
                    scrollTop: $('#rewardPayment').offset().top
                }, 10);
            }
            
            const $msgSuccess = $('.msg.msg-success');
            $msgSuccess
                .html(result.TotalRecords === 1
                    ? $('#hiddenUpdateRewardPaymentStatusSuccessfully').val()
                    : $('#hiddenUpdateAllRewardPaymentStatusSuccessfully').val())
                .show();
            setTimeout(() => $msgSuccess.hide().empty(), 3000);
        }
    }
}

$(function () {
    new RewardPaymentView(
        new FilterComponent(),
        new DateRangePickerJQueryComponent(),
        new FanexMultiSelectComponent(),
        new SingleSelectionDropdownComponent($),
        new UsernameAutoSuggestionComponent(),
        new SortComponent()).init();
});
