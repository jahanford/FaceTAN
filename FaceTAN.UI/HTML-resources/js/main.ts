

class Wrapper {

    constructor() {
        //this.browsers.push(new Browser());
    }

    setFormOpacity(opacity: number) {
        cefAsync.setOpacity(opacity).then(function (result) {
            return result;
        });
    }

    getCefVersion() {
        cefAsync.cefVersion().then(function (result) {
            return result;
        });
    }

    getCefSharpVerion() {
        cefAsync.cefSharpVersion().then(function (result) {
            return result;
        });
    }

    loadURL(URL) {
        cefAsync.loadURL(URL).then(function (result) {
            return result;
        });
    }
}


//view
$('document').ready(function () {

    $('.pad').click(function () {
        
        //let y = new Wrapper().getCefVersion();
        let x = new Wrapper().getCefSharpVersion();
    });
});
