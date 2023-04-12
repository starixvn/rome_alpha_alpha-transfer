"use strict";

class VisibleColumnDropdownOption {
    constructor(
        public visibleColumnKey,
        public onChange = Function(),
        public selectorId = "dropdown-columns") {
        this.visibleColumnKey = visibleColumnKey;
        this.onChange = onChange;
        this.selectorId = selectorId;
    }
}

class VisibleColumnDropdownComponent {
    private $: JQueryStatic;
    private dropdownColumn: JQuery;
    private visibleStatus = {};

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render(options: VisibleColumnDropdownOption) {
        let isCheckAll = true;
        const ddlColumn = this.$("#" + options.selectorId);
        const ddlColumnOptions = ddlColumn.find("option");

        this.initSelected(options, ddlColumn);

        ddlColumn.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaTransfer.numberDisplayedSelectedOnFilter,
            nonSelectedText: ddlColumn.data("label-select-none"),
            nSelectedText: ` ${ddlColumn.data("label-selected")}`,
            allSelectedText: ddlColumn.data("label-all"),
            selectAllText: ddlColumn.data("label-select-all"),
            includeSelectAllOption: true,
            onChange: (option, checked) => { },
            enableCollapsibleOptGroups: true,
            optionClass: this.optionClass,
            enableFiltering: true,
            filterPlaceholder: ddlColumn.data("label-search-column"),
            onDropdownHidden: () => {
                ddlColumnOptions.each((_, c) => {
                    var v = this.$(c).val();
                    this.visibleStatus[v] = false;
                    options.onChange(v, false);
                });

                var selecteds = ddlColumn.find('option:selected');
                selecteds.each((_, c) => {
                    var v = this.$(c).val();
                    this.visibleStatus[v] = true;
                    options.onChange(v, true);
                });

                this.updateStatus(options);
            },
            onSelectAll: (checked) => { },
            enableCaseInsensitiveFiltering: true,
        });
    }

    initSelected = (options, ddlColumn) => {
        const visibleColumns = alphaTransfer.localStorage.get(options.visibleColumnKey);
        const ddlColumnOptions = ddlColumn.find("option");

        if (typeof visibleColumns !== "undefined") {
            ddlColumnOptions.each((_, c) => {
                var v = this.$(c).val();
                this.visibleStatus[v] = false;
            });

            const visibleColumnArray = visibleColumns.split(",");
            visibleColumnArray.forEach((item, i) => {
                ddlColumn.find('option[value="' + item + '"]').attr('selected', 'selected');

                this.visibleStatus[item] = true;
            });

            return;
        }

        ddlColumnOptions.each((_, c) => {
            var v = this.$(c).val();

            ddlColumn.find('option[value="' + v + '"]').attr('selected', 'selected');
            this.visibleStatus[v] = true;
        });
    };

    updateStatus = (options) => {
        const visibleColumns = [];

        for (const [key, value] of Object.entries(this.visibleStatus)) {
            if (value) {
                visibleColumns.push(key);
            }
        }

        alphaTransfer.localStorage.set(options.visibleColumnKey, visibleColumns.join(','));
    };

    getVisibleStatus = () => this.visibleStatus;

    private optionClass = (element: HTMLElement) => {
        if (this.$(element).data("isnested") === 1) {
            return "multiselect-nested-item";
        }
    }
}