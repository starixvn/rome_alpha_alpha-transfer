"use strict";

class Utils {
    mobileDeviceMaxWidth: number;

    constructor() {
        this.mobileDeviceMaxWidth = 768;
    }

    public isMobileDevice = (): boolean => screen.width < this.mobileDeviceMaxWidth;

    public isLandscapeMode = (): boolean => screen.height < screen.width;
}

const utilsInstance = new Utils();