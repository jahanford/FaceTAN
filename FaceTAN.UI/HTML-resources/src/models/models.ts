
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

