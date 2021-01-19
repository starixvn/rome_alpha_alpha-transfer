"use strict"

interface Window {
    $: JQueryStatic
}

class TransferReport {
    $: JQueryStatic;
    $tableReport: JQuery;
    $dataTable: JQuery;

    hasData: boolean;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    public init(): void {
        this.cacheDOMElements();
        this.hasData = JSON.parse(this.$tableReport.data("hasdata").toString().toLowerCase());
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
            childRow.addClass("tbl-footer") :
            childRow.removeClass("tbl-footer");

        setTimeout(() => {
            const scrollTo = this.$(document).height();
            this.$(window).scrollTop(scrollTo + 15);
        }, 100);
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
        parentRow.hasClass("tbl-footer") ?
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

    public getTableOptions = (): any => {
        const self = this;

        return {
            language: {
                emptyTable: this.$("#NoInformationIsAvailable").val(),
                info: this.$("#PaginationInfo").val(),
                infoEmpty: this.$("#PaginationInfoEmpty").val(),
                lengthMenu: this.$("#PaginationLengthMenu").val(),
                paginate: {
                    first: `<span class="l-btn-left l-btn-icon-left">
                                <span class="l-btn-text l-btn-empty">&nbsp;</span>
                                <span class="l-btn-icon pagination-first">&nbsp;</span>
                            </span>`,
                    previous: `<span class="l-btn-left l-btn-icon-left">
                                   <span class="l-btn-text l-btn-empty">&nbsp;</span>
                                   <span class="l-btn-icon pagination-prev">&nbsp;</span>
                               </span>`,
                    next: `<span class="l-btn-left l-btn-icon-left">
                               <span class="l-btn-text l-btn-empty">&nbsp;</span>
                               <span class="l-btn-icon pagination-next">&nbsp;</span>
                           </span>`,
                    last: `<span class="l-btn-left l-btn-icon-left">
                               <span class="l-btn-text l-btn-empty">&nbsp;</span>
                               <span class="l-btn-icon pagination-last">&nbsp;</span>
                           </span>`
                },
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

                        return self.renderDetail(rowIdx, data);
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
            pagingType: utilsInstance.screenSmallThan768() ? "numbers" : "full_numbers",
            pageLength: 500,
            lengthMenu: [10, 50, 100, 500],
            info: this.hasData,
            searching: false,
            fixedHeader: true,
            processing: true,
            drawCallback: this.drawCallback
        };
    };

    public renderData = (idxColumn: any, headerText: string, cellData: any): string => `<li data-dtr-index="${idxColumn}">` +
        `<span class="dtr-title">${headerText}</span>` +
        `<span class="dtr-data">${cellData}</span></li>`;

    public renderDetail = (rowIdx: any, data: any): any => data ?
        this.$(`<ul data-dtr-index="${rowIdx}"/>`).append(data) :
        false;

    public drawCallback = (): void => {
        this.$tableReport.removeClass("hidden");
    };
}