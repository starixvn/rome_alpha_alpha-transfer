"use strict";

interface FanexMultiSelectOption {
    selectorId: string,
    showFilter?: boolean
    isCheckAll?: boolean,
    minWidth?: number,
    showTooltip?: boolean,
    listData?: any,
    onDropdownHidden?: Function
}

class FanexMultiSelectComponent {
    private multiSelect: JQuery;

    render(options: FanexMultiSelectOption) {
        const dropdown = $("#" + options.selectorId);
        this.multiSelect = dropdown.fanexMultiSelect({
            selectAllText: dropdown.data("label-selectall"),
            selectedText: "# " + dropdown.data("label-selected"),
            noneSelectedText: dropdown.data("label-noneselected"),
            customSelectAllText: dropdown.data("label-all") + " (#)",
            showFilter: options.showFilter,
            filterPlaceHolder: dropdown.data("label-filterplaceholder"),
            noItemFoundText: dropdown.data("label-noitemfound"),
            isCheckAll: options.isCheckAll,
            minWidth: options.minWidth || 220,
            maxHeight: 250,
            showTooltip: options.showTooltip || false,
            listData: options.listData || this.getListData(dropdown),
            onLeave: () => options.onDropdownHidden && options.onDropdownHidden()
        });
    }

    private getListData(dropdown: JQuery) {
        const listData = [];
        dropdown.find("option").each(function () {
            const $this = $(this);
            listData.push({
                value: $this.val(),
                text: $this.text(),
                checked: $this.attr("selected")
            });
        });

        return listData;
    }

    getCheckedValues() {
        return this.multiSelect.fanexMultiSelect("getCheckedValues");
    }

    toogleError(value: boolean) {
        this.multiSelect.fanexMultiSelect("addError", value);
    }
}
