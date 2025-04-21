import { useState } from "react";
import { useCreateCategory, useGetBlobImages, useGetCategories } from "../../hooks/";
import { MiniLoaderPage } from "../../components/common";
import { Alert, Box, Button, Checkbox, CircularProgress, FormControl, FormControlLabel, InputLabel, MenuItem, Select, TextField, Typography } from "@mui/material";
import { IBlobImage, ICategoryModel } from "../../interfaces";
import { useNavigate } from "react-router-dom";
import withAuth from "../../oidc/withAuth";

const CategoryCreatePage = () => {
    const navigate = useNavigate();
    const { data: categories, isLoading: categoriesLoading } = useGetCategories();
    const { data: blobImages, isLoading: imagesLoading } = useGetBlobImages();
    const createCategoryMutation = useCreateCategory();

    const [formData, setFormData] = useState<Partial<ICategoryModel>>({
        name: '',
        parentId: undefined,
        isPrimary: false,
        isProductListingEnabled: true,
        image: '',
        hasChild: false,
    });

    const [errors, setErrors] = useState<{ [key: string]: string; }>({});

    const validateForm = () => {
        const newErrors: { [key: string]: string; } = {};
        if (!formData.name) newErrors.name = 'Name is required';
        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | { name?: string; value: unknown; }>) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name!]: value }));
    };

    const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, checked } = e.target;
        setFormData((prev) => ({ ...prev, [name!]: checked }));
    };

    const handleImageSelect = (imageId: string) => {
        setFormData((prev) => ({ ...prev, image: imageId }));
    };

    const handleSubmit = () => {
        if (!validateForm()) return;
        createCategoryMutation.mutate(formData as Omit<ICategoryModel, 'id'>, {
            onSuccess: () => navigate('/categories'),
            onError: (error) => alert(error.message),
        });
    };

    if (categoriesLoading || imagesLoading) {
        return <MiniLoaderPage text="Loading..." />;
    }

    if (createCategoryMutation.error) {
        return <Alert severity="error">{createCategoryMutation.error.message}</Alert>;
    }

    return (
        <Box sx={{ maxWidth: 800, mx: 'auto', mt: 4, p: 2 }}>
            <Typography variant="h4" gutterBottom>
                Create Category
            </Typography>
            <Box component="form" sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                <TextField
                    label="Name"
                    name="name"
                    value={formData.name}
                    onChange={handleChange}
                    error={!!errors.name}
                    helperText={errors.name}
                    required
                />
                <FormControl>
                    <InputLabel>Parent Category</InputLabel>
                    <Select
                        name="parentId"
                        value={formData.parentId || ''}
                        onChange={(e) => setFormData((prev) => ({ ...prev, parentId: Number(e.target.value) || undefined }))}
                    >
                        <MenuItem value="">None</MenuItem>
                        {(categories?.result as Array<ICategoryModel> || []).map((category: ICategoryModel) => (
                            <MenuItem key={category.id} value={category.id}>
                                {category.name}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
                <FormControlLabel
                    control={
                        <Checkbox
                            name="isPrimary"
                            checked={formData.isPrimary || false}
                            onChange={handleCheckboxChange}
                        />
                    }
                    label="Is Primary"
                />
                <FormControlLabel
                    control={
                        <Checkbox
                            name="isProductListingEnabled"
                            checked={formData.isProductListingEnabled || false}
                            onChange={handleCheckboxChange}
                        />
                    }
                    label="Enable Product Listing"
                />
                <Typography variant="h6">Select Image</Typography>
                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
                    {(blobImages || []).map((image: IBlobImage) => (
                        <Box
                            key={image.id}
                            sx={{
                                border: formData.image === image.id ? '2px solid blue' : 'none',
                                cursor: 'pointer',
                            }}
                            onClick={() => handleImageSelect(image.id)}
                        >
                            <img src={image.url} alt="Blob Image" style={{ width: 100, height: 100 }} />
                        </Box>
                    ))}
                </Box>
                <Button variant="contained" color="primary" onClick={handleSubmit} disabled={createCategoryMutation.isPending}>
                    {createCategoryMutation.isPending ? <CircularProgress size={24} /> : 'Create Category'}
                </Button>
            </Box>
        </Box>
    );
};

export default withAuth(CategoryCreatePage);