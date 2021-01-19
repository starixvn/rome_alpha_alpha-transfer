"use strict";

class Utils {
    private readonly mobileDeviceMaxWidth: number;

    constructor() {
        this.mobileDeviceMaxWidth = 768;
    }

    public screenSmallThan768 = (): boolean => screen.width < this.mobileDeviceMaxWidth;

    public screenLandscapeMode = (): boolean => screen.height < screen.width;
}

const utilsInstance = new Utils();