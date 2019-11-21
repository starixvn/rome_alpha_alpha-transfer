describe("TransferAllAndClearCredit_TransferResult", () => {
    const jQueryFake: any = $;
    let topWindow: any;
    let transferResult: TransferResult;

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
        topWindow = { bootboxConfirm: jasmine.any(Function) };
        transferResult = new TransferResult(jQueryFake, topWindow);
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(transferResult, "cacheElements").and.callThrough();

            // act
            transferResult.init();
        });

        it("_Always_Calls cacheDOMElements", () => {
            expect(transferResult.cacheElements).toHaveBeenCalled();
        });
    });

    describe("_submitTransferAndClearCredit", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(transferResult, "cacheElements").and.callThrough();
            transferResult.init();
            spyOn(transferResult.topWindow, "bootboxConfirm");

            // act
            transferResult.submitTransferAndClearCredit();
        });

        it("_Always_Calls cacheDOMElements", () => {
            expect(transferResult.topWindow.bootboxConfirm).toHaveBeenCalled();
        });
    });
});
