
export interface ImageElement {
    guid: string;
    name: string;
    url: string;
}

export interface ImageList {
    guid: string;
    name: string;
    imageStore?: ImageElement[];
}

export interface DsmView {
    edit: boolean;
    guid: string;
}

export interface Test {
    guid: string;
    api: API;

    sourceGuid: string;
    targetGuid: string;

    resultGuid: string;
    result?: string;
}

export enum API{
    Amazon = "Amazon",
    Azure = "Azure"
}
