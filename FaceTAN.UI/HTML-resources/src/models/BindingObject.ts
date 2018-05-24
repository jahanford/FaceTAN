import * as T from './models';

declare var CefSharp: CefSharp;

export interface CefSharp{
    BindObjectAsync(x: string, y: string): any;
}

export interface DataSet{
    getImageArray(): any;
}

export interface TestRunner{
    runTest(targetAPI: T.API, sourceKeyArray: string[], targetKeyArray: string[]): any;
}