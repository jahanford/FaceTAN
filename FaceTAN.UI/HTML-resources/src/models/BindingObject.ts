declare var CefSharp: CefSharp;

export interface CefSharp{
    BindObjectAsync(x: string, y: string): any;
}

export interface DataSet{
    getImageArray(): any;
}