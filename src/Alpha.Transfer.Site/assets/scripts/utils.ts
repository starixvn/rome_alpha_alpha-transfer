class Utils {
    public screenSmallThan768 = (): boolean => screen.width < 768;

    public screenLandscapeMode = (): boolean => window.orientation === -90 || window.orientation == 90;
}

const utilsInstance = new Utils();