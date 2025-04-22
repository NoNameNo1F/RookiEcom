interface ICategoryModel {
    id: number;
    name: string;
    description: string;
    parentId?: number;
    isPrimary: boolean;
    image?: string;
    hasChild: boolean;
}

interface ICategoryUpdateModel {
    id: number;
    name: string;
    description: string;
    parentId?: number;
    isPrimary: boolean;
    image: File;
}

export type {
    ICategoryModel,
    ICategoryUpdateModel
}