"use strict";

interface Window {
    $: JQueryStatic
}

class TransferReport {
    $: JQueryStatic;
    $tableReport: JQuery;
    $dataTable: JQuery;
    $iconFilter: JQuery;
    $pageTitle: JQuery;
    $reportForm: JQuery;

    pageLength: number;
    startHiddenColumnIndex: number;
    tblFooterClass = "tbl-footer";

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    public init(): void {
        this.cacheDOMElements();
        this.registerEvents();

        const initMobilePageLength = 50,
            initDesktopPageLength = 500;
        this.pageLength = utilsInstance.isMobileDevice() ? initMobilePageLength : initDesktopPageLength;
        this.startHiddenColumnIndex = 2;

        this.initFilterVisibility();
        this.initDataTable();
        this.doAdjustDataTable();
    }

    public cacheDOMElements(): void {
        this.$tableReport = this.$("#tbl-report");
        this.$iconFilter = this.$(".icon-filter");
        this.$pageTitle = this.$(".page-title");
        this.$reportForm = this.$("#report-form");
    }

    public registerEvents(): void {
        this.$iconFilter.click(this.onClickIconFilter);
    }

    public initFilterVisibility(): void {
        if (utilsInstance.isMobileDevice()) {
            this.hideFilter();
        }
    }

    public onClickIconFilter = (): void => {
        this.$reportForm.hasClass("hide")
            ? this.showFilter()
            : this.hideFilter();
        this.doAdjustHeader();
    };

    public showFilter(): void {
        this.$pageTitle.removeClass("hide-filter");
        this.$reportForm.removeClass("hide");
    }

    public hideFilter(): void {
        this.$pageTitle.addClass("hide-filter");
        this.$reportForm.addClass("hide");
    }

    public triggerRedrawTable(): void {
        this.doAdjustHeader();
        this.$("tbody tr.parent").each((index, element) => this.onClickDetailControl(element));
    }

    public doAdjustDataTable(): void {
        const browserWidth = this.$(window).width(),
            deviceWidth = screen.width || window.outerWidth,
            columnCount = this.$dataTable.columns()[0].length;

        if (deviceWidth < utilsInstance.mobileDeviceMaxWidth && browserWidth >= utilsInstance.mobileDeviceMaxWidth) {
            for (let i = this.startHiddenColumnIndex; i < columnCount - 1; i++) {
                this.$(this.$dataTable.column(i).header()).removeClass("desktop min-tablet-l mobile").addClass("mobile");
            }
        }

        this.$dataTable.responsive.rebuild();
        this.$dataTable.responsive.recalc();
    }

    public doAdjustHeader = (): void => {
        this.$dataTable.fixedHeader.adjust();

        const scrollTo = this.$(".fixedHeader-floating thead").length > 0 ? this.$(document).height() : 0;
        this.$(window).scrollTop(scrollTo);
    };

    public onClickFooterRow = (parentRow, childRow): void => {
        parentRow.hasClass("parent") ?
            childRow.addClass(this.tblFooterClass) :
            childRow.removeClass(this.tblFooterClass);

        const scrollTopTimeOut = 100;
        setTimeout(() => {
            const scrollTo: number = this.$(document).height(),
                gap = 15;
            this.$(window).scrollTop(scrollTo + gap);
        }, scrollTopTimeOut);
    };

    public onClickBodyRow = (parentRow, childRow): void => {
        if (childRow.hasClass("child")) {
            parentRow.hasClass("even") ?
                childRow.removeClass("odd").addClass("even") :
                childRow.removeClass("even").addClass("odd");
        }
    };

    public onClickDetailControl(element): void {
        const parentRow = this.$(element).closest("tr");
        const childRow = this.$(parentRow).next();
        parentRow.hasClass(this.tblFooterClass) ?
            this.onClickFooterRow(parentRow, childRow) :
            this.onClickBodyRow(parentRow, childRow);
    }

    public initDataTable(options = {}): void {
        const tableOptions = this.getTableOptions();
        this.$.extend(tableOptions, options);

        this.$dataTable = this.$tableReport.DataTable(tableOptions);

        this.$tableReport.find("tbody").on("click", "td.control", event => this.onClickDetailControl(event.currentTarget));
        this.$dataTable.on("draw.dt", () => this.triggerRedrawTable());
    }

    public getTableOptions = () => {
        const self = this,
            lengthMenu =
                `${this.$("#PageSize").val()} <select>
                    <option value="10">10</option>
                    <option value="50">50</option>
                    <option value="100">100</option>
                    <option value="500">500</option>
                </select>`,
            oPaginate = {
                sFirst: '',
                sPrevious: '',
                sNext: '',
                sLast: '',
                info: `${this.$("#Page").val()} _INPUT_ ${this.$("#of").val()} _TOTAL_`
            };

        return {
            language: {
                emptyTable: this.$("#NoInformationIsAvailable").val(),
                lengthMenu,
                oPaginate,
                processing: ""
            },
            responsive: {
                details: {
                    type: "column",
                    target: -1,
                    renderer: (api, rowIdx) => {
                        const data = api.cells(rowIdx, ":hidden").eq(0).map(function (cell) {
                            const header = self.$(api.column(cell.column).header());
                            const idx = api.cell(cell).index();

                            if (header.hasClass("control") || header.hasClass("never")) {
                                return "";
                            }

                            const dtPrivate = api.settings()[0];
                            const cellData = dtPrivate.oApi._fnGetCellData(
                                dtPrivate, idx.row, idx.column, "display"
                            );

                            return self.renderData(idx.column, header.text(), cellData);
                        }).toArray().join("");

                        return this.renderDetail(rowIdx, data);
                    }
                }
            },
            columnDefs: [{
                className: "control",
                orderable: false,
                targets: -1
            }],
            ordering: false,
            paging: true,
            pagingType: "input",
            pageLength: this.pageLength,
            lengthMenu,
            searching: false,
            bInfo: false,
            dom: '<"top"iflp<"clear">>rt<"bottom"ifp<"clear">>',
            fixedHeader: true,
            processing: true,
            drawCallback: this.drawCallback
        };
    };

    public renderData = (idxColumn, headerText: string, cellData): string => `<li data-dtr-index="${idxColumn}">` +
        `<span class="dtr-title">${headerText}</span>` +
        `<span class="dtr-data">${cellData}</span></li>`;

    public renderDetail = (rowIdx, data) => data ?
        this.$(`<ul data-dtr-index="${rowIdx}"/>`).append(data) :
        false;

    public drawCallback = (): void => {
        this.$tableReport.removeClass("hidden");
    };
}