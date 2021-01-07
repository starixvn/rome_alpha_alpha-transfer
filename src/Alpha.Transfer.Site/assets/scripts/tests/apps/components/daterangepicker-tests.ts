describe("DateRangePicker", () => {
    let dateRangePickerComponent: DateRangePickerComponent;
    let template = {
        dateRange: `<body>
                            <div id="daterange-picker"
                                    data-currentdate="01/05/2016"
                                    data-mindate="01/05/2013"
                                    data-maxdate=""
                                    data-selecteddaterange="0">
                                 <div class="ui_nexdatepicker_edit_field daterange-container">
                                    <select id="dropdown-daterange"
                                            name="UserSelectedDateRangeOption"
                                            class="dropdown-daterange">
                                            <option value="0" selected="selected">Today</option>
                                            <option value="1">Yesterday</option>
                                            <option value="2">This week</option>
                                            <option value="3">Last Week</option>
                                            <option value="4">This Month</option>
                                            <option value="5">Last Month</option>
                                            <option value="6">Since Last Month</option>
                                            <option value="7">Yesterday</option>
                                    </select>
                                </div>
                                 <div id="from-date-container"
                                        class="input-group date ui_nexdatepicker_edit_field css fromdate">
                                    <input type="text"
                                            id="from-date"
                                            name="FromDate"
                                            readonly="readonly"
                                            class="ui_nexdatepicker_datetextbox ui_nexdatepicker_fromdate form-control"
                                            value="01/17/2017" />
                                    <span class="input-group-addon">
                                        <span class="icon-calendar"></span>
                                    </span>
                                </div>
                                <div class="ui_nexdatepicker_lbl_field separator">-</div>
                                <div id="to-date-container"
                                        class="input-group date ui_nexdatepicker_edit_field  todate">
                                    <input type="text"
                                            id="to-date"
                                            name="ToDate"
                                            readonly="readonly"
                                            class="ui_nexdatepicker_datetextbox ui_nexdatepicker_todate form-control"
                                            value="01/17/2017" />
                                    <span class="input-group-addon">
                                        <span class="icon-calendar"></span>
                                    </span>
                                </div>
                             </div>
                            <form id="report-form"></form>
                        </body>`
    };

    beforeEach(() => {
        dateRangePickerComponent = new DateRangePickerComponent($);
    });

    describe("_render", () => {
        describe("_Always", () => {
            beforeEach(() => {
                // arrange
                setFixtures(`<div id="daterange-picker"
                                data-currentdate="01/05/2016"
                                data-mindate="01/05/2013"
                                data-maxdate=""
                                data-selecteddaterange="0">
                         </div>`);
                spyOn($.fn, "nexDatePicker");
                spyOn($.fn, "multiselect");

                // act
                dateRangePickerComponent.render();
            });

            it("_Binds nexDatePicker to date range element", () => {
                // assert
                expect($.fn.nexDatePicker).toHaveBeenCalledWith({
                    datePickerFormat: {
                        dateFormat: "MM/DD/YYYY"
                    },
                    selectedItem: 0,
                    minDate: new Date("01/05/2013"),
                    maxDate: null,
                    currentDate: new Date("01/05/2016"),
                    onSelectDatePicker: jasmine.any(Function),
                    onQuickChooseChange: jasmine.any(Function)
                });
            });
        });
    });

    describe("_onSelectDatePicker", () => {
        it("_Always_Calls Change Event", () => {
            // arrange
            setFixtures(template.dateRange);

            let onChange = {
                event: new Function()
            };

            spyOn(onChange, "event");
            dateRangePickerComponent.render(new DateRangePickerOption("daterange-picker", "report-form", onChange.event));

            // act
            this.$("#dropdown-daterange").val("7").change();

            // assert
            expect(onChange.event).toHaveBeenCalled();
        });
    });

    describe("_onQuickChooseChange", () => {
        let $reportForm: JQuery;

        beforeEach(() => {
            // arrange
            setFixtures(template.dateRange);
            dateRangePickerComponent.render();
            $reportForm = dateRangePickerComponent.$reportForm;
            spyOn($reportForm, "submit");
        });

        it("_NotCompleteRender;NotChooseSpecificDates => Not Call Submit", () => {
            // arrange
            dateRangePickerComponent.isCompleteInitDateRangePicker = false;

            // act
            this.$("#dropdown-daterange").val("1").change();

            // assert
            expect($reportForm.submit).not.toHaveBeenCalled();
        });

        it("_CompleteRender;NotChooseSpecificDates => Call Submit", () => {
            // act
            this.$("#dropdown-daterange").val("1").change();

            // assert
            expect($reportForm.submit).toHaveBeenCalled();
        });

        it("_CompleteRender_ChooseSpecificDates => Not Call Submit", () => {
            // act
            this.$("#dropdown-daterange").val("7").change();

            // assert
            expect($reportForm.submit).not.toHaveBeenCalled();
        });
    });
});