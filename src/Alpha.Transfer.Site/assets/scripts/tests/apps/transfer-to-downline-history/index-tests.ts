describe("TransferToDownlineHistory", () => {
    const jqueryFake: JQueryStatic = $;

    let transferToDownlineHistory: TransferToDownlineHistory;
    let template: any = {
        normal: `<body>
                    <form id="filter-form">
                        <input id="txt-username" value="" />
                        <span id="icon-excel"></span>
                    </form>
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
            new DateRangePickerComponent(jqueryFake),
            new ClientPagerComponent(jqueryFake)
        );
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(transferToDownlineHistory, "cacheDOMElements");
            spyOn(transferToDownlineHistory, "registerEvents");
            spyOn(transferToDownlineHistory.dateRangePickerComponent, "render");
            spyOn(transferToDownlineHistory.pagerComponent, "render");

            // act
            transferToDownlineHistory.init();
        });

        it("_Call cacheDOMElements", () => {
            // assert
            expect(transferToDownlineHistory.cacheDOMElements).toHaveBeenCalled();
        });

        it("_Call registerEvents", () => {
            // assert
            expect(transferToDownlineHistory.registerEvents).toHaveBeenCalled();
        });

        it("_Render Date Range Picker", () => {
            // assert
            expect(transferToDownlineHistory.dateRangePickerComponent.render).toHaveBeenCalled();
        });

        it("_Render Client Pager", () => {
            // assert
            expect(transferToDownlineHistory.pagerComponent.render).toHaveBeenCalled();
        });
    });

    describe("_cacheDOMElements", () => {
        it("_Cache all elements", () => {
            // arrange
            setFixtures(template.normal);

            // act
            transferToDownlineHistory.cacheDOMElements();

            // assert
            expect(transferToDownlineHistory.$filterForm).toBeDefined();
            expect(transferToDownlineHistory.$username).toBeDefined();
            expect(transferToDownlineHistory.$usernameAutoSuggestionPath).toBeDefined();
            expect(transferToDownlineHistory.$output).toBeDefined();
            expect(transferToDownlineHistory.$exportExcel).toBeDefined();
            expect(transferToDownlineHistory.$exportExcelForm).toBeDefined();
        });
    });

    describe("_registerEvents", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(transferToDownlineHistory, "exportExcelHandler");
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