var Wrapper = (function () {
    function Wrapper() {
        //this.browsers.push(new Browser());
    }
    Wrapper.prototype.setFormOpacity = function (opacity) {
        cefAsync.setOpacity(opacity).then(function (result) {
            return result;
        });
    };
    Wrapper.prototype.getCefVersion = function () {
        cefAsync.cefVersion().then(function (result) {
            return result;
        });
    };
    Wrapper.prototype.getCefSharpVerion = function () {
        cefAsync.cefSharpVersion().then(function (result) {
            return result;
        });
    };
    Wrapper.prototype.loadURL = function (URL) {
        cefAsync.loadURL(URL).then(function (result) {
            return result;
        });
    };
    return Wrapper;
}());
//view
$('document').ready(function () {
    $('.pad').click(function () {
        //let y = new Wrapper().getCefVersion();
        var x = new Wrapper().getCefSharpVersion();
    });
});
