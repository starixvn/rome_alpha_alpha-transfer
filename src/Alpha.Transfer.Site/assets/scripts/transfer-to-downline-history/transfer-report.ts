"use strict";

interface Window {
    $: JQueryStatic
}

class TransferReport {
    $: JQueryStatic;
    $tableReport: JQuery;
    $dataTable: JQuery;

    hasData: boolean;
    lengthMenu: number[];
    pageLength: number;
    pagingType: string;
    tblFooterClass = "tbl-footer";
    $self: TransferReport;

    constructor($: JQueryStatic) {
        const maxPageSize = 500;

        this.$ = $;
        this.pageLength = maxPageSize;

        this.$self = this;
    }

    public init(): void {
        this.cacheDOMElements();

        this.hasData = JSON.parse(this.$tableReport.data("hasdata").toString().toLowerCase());
        this.pagingType = "input";
        this.initDataTable();
    }

    public cacheDOMElements(): void {
        this.$tableReport = this.$("#tbl-report");
    }

    public triggerRedrawTable(): void {
        this.doAdjustHeader();
        this.$("tbody tr.parent").each((index, element) => this.onClickDetailControl(element));
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
        const innerSelf = this.$self,
            lengthMenu = `{Page Size} <select>` +
                '<option value="10">10</option>' +
                '<option value="50">50</option>' +
                '<option value="100">100</option>' +
                `<option value="500">500</option>` +
                '</select>',
            oPaginate = {
                sFirst: '',
                sPrevious: '',
                sNext: '',
                sLast: '',
                info: `{Page} _INPUT_ {Of} _TOTAL_`
            };
        return {
            language: {
                emptyTable: this.$("#NoInformationIsAvailable").val(),
                info: this.$("#PaginationInfo").val(),
                infoEmpty: this.$("#PaginationInfoEmpty").val(),
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
                            const header = innerSelf.$(api.column(cell.column).header());
                            const idx = api.cell(cell).index();

                            if (header.hasClass("control") || header.hasClass("never")) {
                                return "";
                            }

                            const dtPrivate = api.settings()[0];
                            const cellData = dtPrivate.oApi._fnGetCellData(
                                dtPrivate, idx.row, idx.column, "display"
                            );

                            return innerSelf.renderData(idx.column, header.text(), cellData);
                        }).toArray().join("");

                        return this.$self.renderDetail(rowIdx, data);
                    }
                }
            },
            columnDefs: [{
                className: "control",
                orderable: false,
                targets: -1
            }],
            ordering: this.hasData,
            paging: this.hasData,
            pagingType: this.pagingType,
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