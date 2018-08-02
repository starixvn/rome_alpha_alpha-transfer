"use strict";
interface JQuery {
    multiselect(option: any): void;
}

class SingleSelectionDropdownOption {
    constructor(
        public dropdownId: string = "dropdown-single",
        public inputId: string = "UserSelectedId",
        public onChange: Function = null) {
    }
}

class SingleSelectionDropdownComponent {
    $: JQueryStatic;
    $hiddenInput: JQuery;
    $dropdown: JQuery;
    $dropdownButton: JQuery;

    selectedValue: string;

    options = new SingleSelectionDropdownOption();

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    render = (options: SingleSelectionDropdownOption = new SingleSelectionDropdownOption()) => {
        this.options = <SingleSelectionDropdownOption>this.$.extend({}, this.options, options);

        let $dropdown = this.$("#" + this.options.dropdownId);
        this.$dropdown = $dropdown;

        let $hiddenInput = this.$("#" + this.options.inputId);
        this.$hiddenInput = $hiddenInput;

        let self = this;
        this.selectedValue = self.getSelectedItem();

        this.$dropdown.multiselect({
            multiple: false,
            maxHeight: 300,
            numberDisplayed: 1,
            includeSelectAllOption: false,
            onChange: this.onchange
        });
    };
    getSelectedItem = () => {
        let selected = [];
        let selectedOptions = this.$dropdown.find("option:selected");
        let this$ = this.$;

        selectedOptions.each(function () {
            selected.push([this$(this).val()]);
        });

        return selected.length ? selected[0].toString() : "";
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
}















