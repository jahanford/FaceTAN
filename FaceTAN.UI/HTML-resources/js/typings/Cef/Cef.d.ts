declare var cefAsync: cefAsync;

interface cefAsync {

    cefVersion(): PromiseLike<String>;
    cefSharpVersion(): PromiseLike<String>;

    loadURL(string): PromiseLike<Boolean>;
    setOpacity(number): PromiseLike<Boolean>;
}

declare var WrapperFormAsync: WrapperFormAsync;

interface WrapperFormAsync {

    
}


// 
//
//
//
//
//
//
//
//
//
