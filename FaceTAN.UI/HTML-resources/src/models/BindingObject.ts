declare var CefSharp: CefSharp;

export interface CefSharp{
    BindObjectAsync(x: string, y: string): any;
}

declare var boundDataSet: DataSet;

export interface DataSet{
    getImageArray(): any;
}