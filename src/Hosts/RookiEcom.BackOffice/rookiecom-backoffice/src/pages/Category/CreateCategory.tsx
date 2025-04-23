import { useState } from "react";
import { useCreateCategory, useGetCategories } from "../../hooks";
import { MiniLoaderPage } from "../../components/common";
import { Alert, Box, Button, Checkbox, CircularProgress, FormControl, FormControlLabel, InputLabel, MenuItem, Select, TextField, Typography } from "@mui/material";
import { ICategoryModel } from "../../interfaces";
import { useNavigate } from "react-router-dom";
import withAuth from "../../oidc/withAuth";

const CreateCategoryPage: React.FC  = () => {
    const navigate = useNavigate();
    const { data: categories, isLoading, error } = useGetCategories();
    const createCategoryMutation = useCreateCategory();

    const [formData, setFormData] = useState<Partial<ICategoryModel> & { imageFile?: File}>({
        name: '',
        description: '',
        parentId: 0,
        isPrimary: false,
        imageFile: {} as File,
    });

    const [errors, setErrors] = useState<{ [key: string]: string; }>({});

    const validateForm = () => {
        const newErrors: { [key: string]: string; } = {};
        if (!formData.name) newErrors.name = 'Name is required';
        if (!formData.description) newErrors.description = 'Description is required';
        if (!formData.imageFile) newErrors.imageFile = 'Image for Category is required';
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

    const handleFileChange = (image: File) => {
        setFormData((prev) => ({ ...prev, imageFile: image }));
    };

    const handleSubmit = () => {
        if (!validateForm()) return;
        createCategoryMutation.mutate(formData as Omit<ICategoryModel, 'id' | 'hasChild' |'image'>, {
            onSuccess: () => navigate('/categories'),
            onError: (error) => alert(error.message),
        });
    };

    if (isLoading) {
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
                <TextField
                    label="Description"
                    name="description"
                    value={formData.description}
                    onChange={handleChange}
                    multiline
                    rows={4}
                />
                <FormControl>
                    <InputLabel>Parent Category</InputLabel>
                    <Select
                        name="parentId"
                        value={formData.parentId || ''}
                        onChange={(e) => setFormData((prev) => ({ ...prev, parentId: Number(e.target.value) || undefined }))}
                    >
                        <MenuItem value="">None</MenuItem>
                        {(categories?.items as Array<ICategoryModel> || []).map((category: ICategoryModel) => (category.id !== formData.parentId) &&
                        (
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
                <Button component="label" variant="outlined">
                    Upload Image
                    <input
                        type="file"
                        hidden
                        onChange={() => handleFileChange}
                        accept="image/*"
                    />
                </Button>
                {formData.imageFile && <Typography>Image selected: {formData.imageFile.name}</Typography>}
                
                <Button
                    variant="contained"
                    color="primary"
                    onClick={handleSubmit}
                    disabled={createCategoryMutation.isPending}
                >
                    {createCategoryMutation.isPending ? <CircularProgress size={24} /> : 'Create Category'}
                </Button>
            </Box>
        </Box>
    );
};

export default withAuth(CreateCategoryPage);