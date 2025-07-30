tj.onWindowResize((res) => {
    window.innerWidth = res.windowWidth;
    window.innerHeight = res.windowHeight;
});
tj.onDeviceOrientationChange(() => {
    const info = tj.getWindowInfo ? tj.getWindowInfo() : tj.getSystemInfoSync();
    window.innerWidth = info.screenWidth;
    window.innerHeight = info.screenHeight;
});
