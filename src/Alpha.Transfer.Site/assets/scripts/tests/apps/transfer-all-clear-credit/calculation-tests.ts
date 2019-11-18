describe("TransferAllAndClearCredit_Calculation", () => {
    const jQueryFake: any = $;
    let topWindow: any;
    let transferAllAndClearCredit: TransferAllAndClearCredit;

    let template: any = {
        normal: `<body>
                    <input id="usernameAutoSuggestionPath" value="" />
                    <input id="txt-username" value="" />
                    <input id="output" value="" />
                    <input id="shadow" value="" />
                    <input id="btn-submit" value="" />
                 </body>`
    };

    beforeEach(() => {
        topWindow = {
            OnPopupComplete: jasmine.any(Function),
            bootboxConfirm: jasmine.any(Function)
        };
        transferAllAndClearCredit = new TransferAllAndClearCredit(jQueryFake, topWindow);
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(transferAllAndClearCredit, "cacheElements").and.callThrough();

            // act
            transferAllAndClearCredit.init();
        });

        it("_Always_Calls cacheDOMElements", () => {
            expect(transferAllAndClearCredit.cacheElements).toHaveBeenCalled();
        });
    });
    describe("_submitTransferAndClearCredit", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(transferAllAndClearCredit, "cacheElements").and.callThrough();
            transferAllAndClearCredit.init();
            spyOn(transferAllAndClearCredit.topWindow, "bootboxConfirm");

            // act
            transferAllAndClearCredit.submitTransferAndClearCredit();
        });

        it("_Always_Calls cacheDOMElements", () => {
            expect(transferAllAndClearCredit.topWindow.bootboxConfirm).toHaveBeenCalled();
        });
    });
});
