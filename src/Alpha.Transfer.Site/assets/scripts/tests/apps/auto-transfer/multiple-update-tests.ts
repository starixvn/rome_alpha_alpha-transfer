describe("AutoTransfer", () => {
    const jQueryFake: any = $;
    let topWindow: any;
    let multipleUpdateAutoTransfer: MultipleUpdateAutoTransfer;
    let template: any = {
        normal: `<body>
                    <div id="result-msg" class="msg hide"></div>
                    <div class="div-usernames">master1, master1p1, masterkz02</div>
                    <div class="auto-transfer">
                        Auto Transfer:
                        <div id="switch-auto-transfer" class="switch-auto-transfer" data-option="0" data-value="noset" style="display: inline-block; width: 66px; height: 14px;">
                           <div id="switch-auto-transfer_railoff" style="cursor: pointer; color: rgb(255, 0, 0); font-size: 0.7em; text-align: center; display: inline-block; width: 20px; vertical-align: top; border-left: 1px solid rgb(189, 189, 189); border-top: 1px solid rgb(189, 189, 189); border-bottom: 1px solid rgb(189, 189, 189); border-top-left-radius: 7px; border-bottom-left-radius: 7px; height: 100%; margin-top: 1.4px; margin-bottom: 1.4px; background-color: rgb(241, 241, 241);">
                                <span>Off<span></span></span>
                           </div>
                           <div id="switch-auto-transfer_railnoset" style="cursor: pointer; display: inline-block; text-align: center; width: 20px; vertical-align: top; border-top: 1px solid rgb(189, 189, 189); border-bottom: 1px solid rgb(189, 189, 189); height: 100%; margin-top: 1.4px; margin-bottom: 1.4px; background-color: rgb(241, 241, 241);">
                              <div draggable="true" style="cursor: pointer; display: inline-block; height: 11px; width: 11px; border-radius: 7px; border: 1px solid rgb(213, 225, 238); box-shadow: rgb(93, 93, 93) 1px 2px 4px;">&nbsp;</div>
                           </div>
                           <div id="switch-auto-transfer_railon" style="cursor: pointer; color: rgb(8, 151, 16); font-size: 0.7em; text-align: center; display: inline-block; width: 20px; vertical-align: top; border-right: 1px solid rgb(189, 189, 189); border-top: 1px solid rgb(189, 189, 189); border-bottom: 1px solid rgb(189, 189, 189); border-top-right-radius: 7px; border-bottom-right-radius: 7px; height: 100%; margin-top: 1.4px; margin-bottom: 1.4px; background-color: rgb(241, 241, 241);">
                              <span>On<span></span></span>
                           </div>
                        </div>
                    </div>

                    <ul>
                        <li>If ON, The Auto Transfer process will be executed right after 12:00 AM (GMT+8) on Daily.</li>
                        <li>To ensure the accuracy of data while auto transfer us running, you CANNOT transfer to your downline whose auto transfer option is on at the process time.</li>
                    </ul>

                    <input type="hidden" id="custIds" value="1176083,33278659,14373883">
                    <input id="confirmTurnOnAutoTransfer" name="confirmTurnOnAutoTransfer" type="hidden" value="Are you sure you want to turn on Auto Transfer process for these customers?">
                    <input id="confirmTurnOffAutoTransfer" name="confirmTurnOffAutoTransfer" type="hidden" value="Are you sure you want to turn off Auto Transfer process for these customers?">
                    <input id="lblOn" name="lblOn" type="hidden" value="On">
                    <input id="lblOff" name="lblOff" type="hidden" value="Off">
                </body>`
    };

    beforeEach(() => {
        topWindow = {
            OnPopupComplete: jasmine.any(Function),
            bootboxConfirm: jasmine.any(Function),
            reloadMainFrame: jasmine.any(Function)
        };

        multipleUpdateAutoTransfer = new MultipleUpdateAutoTransfer(jQueryFake, topWindow);
    });

    describe("_init", () => {
        beforeEach(() => {
            // arrange
            spyOn(multipleUpdateAutoTransfer, "cacheDOMElements");
            spyOn(multipleUpdateAutoTransfer, "registerEvents");

            // act
            multipleUpdateAutoTransfer.init();
        });

        it("_Always_Calls cacheDOMElements", () => {
            expect(multipleUpdateAutoTransfer.cacheDOMElements).toHaveBeenCalled();

        });

        it("_Always_Calls registerEvents", () => {
            expect(multipleUpdateAutoTransfer.registerEvents).toHaveBeenCalled();
        });

    });

    describe("_cacheDOMElements", () => {
        it("_Always_All neccessary elements are cached", () => {
            // arrange
            setFixtures(template.normal);

            // act
            multipleUpdateAutoTransfer.cacheDOMElements();

            // assert
            expect(multipleUpdateAutoTransfer.$switch).toBeDefined();
            expect(multipleUpdateAutoTransfer.confirmTurnOnAutoTransfer).toBeDefined();
            expect(multipleUpdateAutoTransfer.confirmTurnOffAutoTransfer).toBeDefined();

            expect(multipleUpdateAutoTransfer.custIds).toBe("1176083,33278659,14373883");
            expect(multipleUpdateAutoTransfer.confirmTurnOnAutoTransfer).toBe("Are you sure you want to turn on Auto Transfer process for these customers?");
            expect(multipleUpdateAutoTransfer.confirmTurnOffAutoTransfer).toBe("Are you sure you want to turn off Auto Transfer process for these customers?");
            expect(multipleUpdateAutoTransfer.lblOn).toBe("On");
            expect(multipleUpdateAutoTransfer.lblOff).toBe("Off");
        });
    });

    describe("_registerEvents", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(multipleUpdateAutoTransfer, "resetSwitch").and.callThrough();
            spyOn(topWindow, "bootboxConfirm");
            multipleUpdateAutoTransfer.cacheDOMElements();

            // act
            multipleUpdateAutoTransfer.registerEvents();

        });

        it("_Always_Reset Switch value", () => {
            // assert

            expect(multipleUpdateAutoTransfer.$switch.attr("data-value")).toBe("noset");

        });

        it("_Always_Bind SwitchOn click event", () => {
            // assert
            $("div#switch-auto-transfer_railon").click();
            expect(topWindow.bootboxConfirm).toHaveBeenCalled();

        });

        it("_Always_Bind SwitchOff click event", () => {
            // assert
            $("div#switch-auto-transfer_railoff").click();

            expect(multipleUpdateAutoTransfer.topWindow.bootboxConfirm).toHaveBeenCalled();

        });
    });






    //describe("_doTransfer", () => {
    //    it("_ValidationObject.Code != 0 -> Show Error Message", () => {
    //        // arrange
    //        setFixtures(template.normal);
    //        multipleUpdateAutoTransfer.cacheDOMElements();
    //        multipleUpdateAutoTransfer.$amount.val("250,417.24");
    //        let e: any = { preventDefault: function () { } };

    //        spyOn(multipleUpdateAutoTransfer, "showMessage");

    //        // act
    //        multipleUpdateAutoTransfer.doTransfer(e);

    //        // assert
    //        expect(multipleUpdateAutoTransfer.showMessage).toHaveBeenCalled();
    //    });

    //    it("_ValidationObject.Code = 0 & Confirm Transfer Cancel -> Do Nothing", () => {
    //        // arrange
    //        setFixtures(template.normal);
    //        multipleUpdateAutoTransfer.cacheDOMElements();
    //        multipleUpdateAutoTransfer.$amount.val("100,000.00");
    //        let e: any = { preventDefault: function () { } };

    //        spyOn(window, "confirm").and.returnValue(false);
    //        spyOn(jQueryFake, "unblockUI").and.returnValue(false);

    //        // act
    //        multipleUpdateAutoTransfer.doTransfer(e);

    //        // assert
    //        expect(window.confirm).toHaveBeenCalled();
    //        expect(jQueryFake.unblockUI).toHaveBeenCalled();
    //    });

    //    it("_ValidationObject.Code = 0 & Confirm Transfer OK -> Make a Post", () => {
    //        // arrange
    //        setFixtures(template.normal);
    //        multipleUpdateAutoTransfer.cacheDOMElements();
    //        multipleUpdateAutoTransfer.$amount.val("100,000.00");
    //        let e: any = { preventDefault: function () { } };

    //        spyOn(window, "confirm").and.returnValue(true);
    //        spyOn(jQueryFake, "post").and.callThrough();

    //        // act
    //        multipleUpdateAutoTransfer.doTransfer(e);

    //        // assert
    //        expect(window.confirm).toHaveBeenCalled();
    //        expect(jQueryFake.post).toHaveBeenCalled();
    //    });
    //});
});
