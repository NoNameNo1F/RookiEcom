interface ICategoryModel {
    id: number;
    name: string;
    description: string;
    parentId?: number;
    isPrimary: boolean;
    image: string;
    hasChild: boolean;
}


interface ICategoryCreateForm {
    name: string;
    description: string;
    parentId?: number;
    isPrimary: boolean;
    imageFile: FileList;
}

interface ICategoryUpdateForm {
    id: number;
    name: string;
    description: string;
    parentId?: number;
    isPrimary: boolean;
    imageFile?: FileList;
}

export type {
    ICategoryModel,
    ICategoryCreateForm,
    ICategoryUpdateForm
};

