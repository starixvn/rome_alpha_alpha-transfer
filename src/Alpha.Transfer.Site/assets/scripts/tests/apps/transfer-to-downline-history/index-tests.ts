describe("TransferToDownlineHistory", () => {
    const jqueryFake: JQueryStatic = $;

    let transferToDownlineHistory: TransferToDownlineHistory;
    let template: any = {
        normal: `<body>
                    <form id="filter-form">
                        <input id="txt-username" value="" />
                        <input id="from-date" value="" />
                        <input id="to-date" value="" />
                        <span id="icon-excel"></span>
                    </form>
                    <table id="tbl-report" data-cansort="True">
                        <thead>
                            <tr></tr>
                            <tr>
                                <th class="all">A</th>
                                <th class="all">B</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>1</td>
                                <td>2</td>
                                <td></td>
                            </tr>
                        </tbody>
                    </table>
                    <form id="export-excel-form">
                        
                    </form>
                    <div id="shadow"></div>
                    <div id="output"></div>
                    <input id="usernameAutoSuggestionPath" />
                </body`
    };

    beforeEach(() => {
        transferToDownlineHistory = new TransferToDownlineHistory(
            jqueryFake,
            new DateRangePickerComponent(jqueryFake)
        );
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(transferToDownlineHistory, "cacheDOMElements");
            spyOn(transferToDownlineHistory, "initDataTable");
            spyOn(transferToDownlineHistory, "initUsernameSuggestion"),
            spyOn(transferToDownlineHistory, "registerEvents");
            spyOn(transferToDownlineHistory.dateRangePickerComponent, "render");

            transferToDownlineHistory.$tableReport = jqueryFake("#tbl-report");

            // act
            transferToDownlineHistory.init();
        });

        it("_Call cacheDOMElements", () => {
            // assert
            expect(transferToDownlineHistory.cacheDOMElements).toHaveBeenCalled();
        });

        it("_Set correct value canSort", () => {
            // assert
            expect(transferToDownlineHistory.canSort).toBeTruthy();
        });

        it("_Call initDataTable", () => {
            // assert
            expect(transferToDownlineHistory.initDataTable).toHaveBeenCalled();
        });

        it("_Call initUsernameSuggestion", () => {
            // assert
            expect(transferToDownlineHistory.initUsernameSuggestion).toHaveBeenCalled();
        });

        it("_Call registerEvents", () => {
            // assert
            expect(transferToDownlineHistory.registerEvents).toHaveBeenCalled();
        });

        it("_Render Date Range Picker", () => {
            // assert
            expect(transferToDownlineHistory.dateRangePickerComponent.render).toHaveBeenCalled();
        });
    });

    describe("_cacheDOMElements", () => {
        it("_Cache all elements", () => {
            // arrange
            setFixtures(template.normal);

            // act
            transferToDownlineHistory.cacheDOMElements();

            // assert
            expect(transferToDownlineHistory.$tableReport).toBeDefined();
            expect(transferToDownlineHistory.$username).toBeDefined();
            expect(transferToDownlineHistory.$usernameAutoSuggestionPath).toBeDefined();
            expect(transferToDownlineHistory.$output).toBeDefined();
            expect(transferToDownlineHistory.$exportExcel).toBeDefined();
            expect(transferToDownlineHistory.$exportExcelForm).toBeDefined();
            expect(transferToDownlineHistory.$fromDate).toBeDefined();
            expect(transferToDownlineHistory.$toDate).toBeDefined();
        });
    });

    describe("_registerEvents", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(transferToDownlineHistory, "exportExcelHandler");
            spyOn(transferToDownlineHistory, "doAdjustDatePickerCalendar");
            transferToDownlineHistory.cacheDOMElements();

            // act
            transferToDownlineHistory.registerEvents();
        });

        it("_Bind username keyup event", () => {
            // act
            transferToDownlineHistory.$username.keyup();

            // assert
            expect(transferToDownlineHistory.$username.hasClass("text-italic")).toBeTruthy();
        });

        it("_Bind export excel click event", () => {
            // act
            transferToDownlineHistory.$exportExcel.click();

            // assert
            expect(transferToDownlineHistory.exportExcelHandler).toHaveBeenCalled();
        });

        it("_Bind from date datepicker show event", () => {
            // act
            transferToDownlineHistory.$fromDate.parent().trigger("dp.show");

            // assert
            expect(transferToDownlineHistory.doAdjustDatePickerCalendar).toHaveBeenCalled();
        });

        it("_Bind to date datepicker show event", () => {
            // act
            transferToDownlineHistory.$toDate.parent().trigger("dp.show");

            // assert
            expect(transferToDownlineHistory.doAdjustDatePickerCalendar).toHaveBeenCalled();
        });
    });

    describe("_exportExcelHandler", () => {
        it("_Submit export excel form", () => {
            // arrange
            setFixtures(template.normal);

            transferToDownlineHistory.cacheDOMElements();
            transferToDownlineHistory.registerEvents();

            spyOn(transferToDownlineHistory.$exportExcelForm, "submit");

            // act
            transferToDownlineHistory.exportExcelHandler();

            // assert
            expect(transferToDownlineHistory.$exportExcelForm.submit).toHaveBeenCalled();
        });
    });
});