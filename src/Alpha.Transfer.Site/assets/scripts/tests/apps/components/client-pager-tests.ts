describe("ClientPager", () => {
    const jQueryFake: JQueryStatic = $;

    let clientPagerComponent: ClientPagerComponent;
    let template: any = {
        normal: `<body>
                    <div id="report-pager"
                        data-totalrecords="10"
                        data-pagesize="20"
                        data-pageindex="1"
                        data-lablepage="Page"
                        data-labelof="of"
                        data-labeldisplayitems="Display items">
                    </div>
                 </body>`
    };

    beforeEach(() => {
        clientPagerComponent = new ClientPagerComponent(jQueryFake);
    });

    describe("_Render", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            spyOn(jQueryFake.fn, "pagination");

            // act
            clientPagerComponent.render();
        });

        it("_Init total records", () => {
            // assert
            expect(clientPagerComponent.totalRecords).toBe(10);
        });

        it("_Init page size", () => {
            // assert
            expect(clientPagerComponent.pageSize).toBe(20);
        });

        it("_Init page index", () => {
            // assert
            expect(clientPagerComponent.pageIndex).toBe(1);
        });

        it("_Bind Pager Plugin", () => {
            // assert
            expect(jQueryFake.fn.pagination).toHaveBeenCalledWith({
                pageList: [10, 50, 100, 500],
                total: 10,
                pageSize: 20,
                pageNumber: 1,

                showRefresh: false,
                onSelectPage: clientPagerComponent.onSelectPage,
                beforePageText: "Page",
                afterPageText: "of {pages}",
                displayMsg: "Display items"
            });
        });

        it("_show pager", () => {
            // arrange
            let pager = $("#report-pager");

            // assert
            expect(pager.hasClass("hide")).toBeFalsy();
        });
    });

    describe("_onSelectPage", () => {
        beforeEach(() => {
            // arrange
            setFixtures(template.normal);
            clientPagerComponent.render();
            spyOn(clientPagerComponent.$reportForm, "submit");

            // act
            clientPagerComponent.onSelectPage(1, 10);
        });

        it("_Update hidden fields", () => {
            // assert
            expect(clientPagerComponent.pageIndex).toBe(1);
            expect(clientPagerComponent.pageSize).toBe(10);
        });

        it("_Submit report form", () => {
            // assert
            expect(clientPagerComponent.$reportForm.submit).toHaveBeenCalled();
        });
    });
});