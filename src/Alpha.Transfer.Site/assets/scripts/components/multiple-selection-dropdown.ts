"use strict";
interface JQuery {
    multiselect(option: object): void;
}

class MultipleSelectionDropdownOption {
    constructor(
        public dropdownId = "dropdown-multiple",
        public inputId = "UserSelectedId",
        public onChange: Function = null) {
    }
}

class MultipleSelectionDropdownComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $dropdown: JQuery;
    $dropdownButton: JQuery;

    selectedValue: string;

    options = new MultipleSelectionDropdownOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: MultipleSelectionDropdownOption = new MultipleSelectionDropdownOption()) => {
        this.options = this.$.extend({}, this.options, options) as MultipleSelectionDropdownOption;

        const $dropdown = this.$("#" + this.options.dropdownId);
        this.$dropdown = $dropdown;

        const $hiddenInput = this.$("#" + this.options.inputId);
        this.$hiddenInput = $hiddenInput;

        this.selectedValue = this.getSelectedItem();

        this.$dropdown.multiselect({
            maxHeight: 300,
            numberDisplayed: alphaTransfer.numberDisplayedSelectedOnFilter,
            nonSelectedText: $dropdown.data("label-select-none"),
            nSelectedText: ` ${$dropdown.data("label-selected")}`,
            allSelectedText: $dropdown.data("label-all"),
            selectAllText: $dropdown.data("label-select-all"),
            filterPlaceholder: $dropdown.data("label-search"),
            includeSelectAllOption: true, enableCollapsibleOptGroups: true,
            optionClass: this.optionClass,
            enableFiltering: true,
            onDropdownHidden: () => { },
            onChange: (option, checked) => { },
            onSelectAll: (checked) => { },
            enableCaseInsensitiveFiltering: true,
        });
    };
    getSelectedItem = () => {
        const selected = [];
        const selectedOptions = this.$dropdown.find("option:selected");
        const this$ = this.$;

        selectedOptions.each(function () {
            selected.push([this$(this).val()]);
        });

        return selected.join(",");
    };
    updateSelectedValue = () => {
        this.$hiddenInput.val(this.selectedValue);
    };

    onchange = () => {
        this.selectedValue = this.getSelectedItem();
        if (this.options.onChange) {
            this.options.onChange();
        }
    };
    private optionClass = (element: HTMLElement) => {
        if (this.$(element).data("isnested") === 1) {
            return "multiselect-nested-item";
        }
    }
}
