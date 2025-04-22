import { useState } from 'react';
import { useCreateProduct, useGetCategories } from '../../hooks';

import withAuth from '../../oidc/withAuth';
import { Box, Typography, TextField, Button, MenuItem, Select, FormControl, InputLabel, CircularProgress, Alert, FormHelperText } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { IProductModel, ICategoryModel } from '../../interfaces';
import { MiniLoaderPage } from '../../components/common';

const CreateProductPage = () => {
    const navigate = useNavigate();
    const { data: categories, isLoading: categoriesLoading } = useGetCategories();
    const createProductMutation = useCreateProduct();

    const [formData, setFormData] = useState<Partial<IProductModel>>({
        sku: '',
        name: '',
        description: '',
        price: 0,
        marketPrice: 0,
        stock: 0,
        categoryId: 0,
        isFeature: false,
        imageGallery: [],
        productAttributes: [],
        variationOption: { code: "", values: [] },
    });

    const [errors, setErrors] = useState<{ [key: string]: string; }>({});

    const validateForm = () => {
        const newErrors: { [key: string]: string; } = {};
        if (!formData.sku) newErrors.sku = 'SKU is required';
        if (!formData.name) newErrors.name = 'Name is required';
        if (!formData.description) newErrors.description = 'Description is required';
        if (!formData.price || formData.price <= 0) newErrors.price = 'Price must be greater than 0';
        if (!formData.marketPrice || formData.marketPrice <= 0) newErrors.marketPrice = 'MarketPrice must be greater than 0';
        if (!formData.stock || formData.stock < 0) newErrors.stock = 'Stock must be 0 or greater';
        if (!formData.categoryId) newErrors.categoryId = 'Category is required';
        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | { name?: string; value: unknown; }>) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name!]: value }));
    };

    const handleImageSelect = (imageId: string) => {
        setFormData((prev) => ({
            ...prev,
            imageGallery: [imageId],
        }));
    };

    const handleSubmit = () => {
        if (!validateForm()) return;
        createProductMutation.mutate(formData as Omit<IProductModel, 'id'>, {
            onSuccess: () => navigate('/products'),
            onError: (error) => alert(error.message),
        });
    };

    if (categoriesLoading) {
        return <MiniLoaderPage text="Loading..." />;
    }

    if (createProductMutation.error) {
        return <Alert severity="error">{createProductMutation.error.message}</Alert>;
    }

    return (
        <Box sx={{ maxWidth: 800, mx: 'auto', mt: 4, p: 2 }}>
            <Typography variant="h4" gutterBottom>
                Create Product
            </Typography>
            <Box component="form" sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                <TextField
                    label="SKU"
                    name="sku"
                    value={formData.sku}
                    onChange={handleChange}
                    error={!!errors.sku}
                    helperText={errors.sku}
                    required
                />
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
                <TextField
                    label="Price"
                    name="price"
                    type="number"
                    value={formData.price}
                    onChange={handleChange}
                    error={!!errors.price}
                    helperText={errors.price}
                    required
                />
                <TextField
                    label="MarketPrice"
                    name="marketPrice"
                    type="number"
                    value={formData.marketPrice}
                    onChange={handleChange}
                    error={!!errors.marketPrice}
                    helperText={errors.marketPrice}
                    required
                />
                <TextField
                    label="Stock"
                    name="stock"
                    type="number"
                    value={formData.stock}
                    onChange={handleChange}
                    error={!!errors.stock}
                    helperText={errors.stock}
                    required
                />
                <TextField
                    label="IsFeature"
                    name="isFeature"
                    type="number"
                    value={formData.isFeature}
                    onChange={handleChange}
                    error={!!errors.isFeature}
                    helperText={errors.isFeature}
                    required
                />
                <FormControl error={!!errors.categoryId}>
                    <InputLabel>Category</InputLabel>
                    <Select
                        name="categoryId"
                        value={formData.categoryId}
                        onChange={(e) => setFormData((prev) => ({ ...prev, categoryId: Number(e.target.value) }))}
                    >
                        {(categories?.items as Array<ICategoryModel> || []).map((category: ICategoryModel) => (
                            <MenuItem key={category.id} value={category.id}>
                                {category.name}
                            </MenuItem>
                        ))}
                    </Select>
                    {errors.categoryId && <FormHelperText>{errors.categoryId}</FormHelperText>}
                </FormControl>
                <Typography variant="h6">Select Image</Typography>
                {/* <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
                    {(blobImages || []).map((image: IBlobImage) => (
                        <Box
                            key={image.id}
                            sx={{
                                border: formData.imageGallery?.includes(image.id) ? '2px solid blue' : 'none',
                                cursor: 'pointer',
                            }}
                            onClick={() => handleImageSelect(image.id)}
                        >
                            <img src={image.url} alt="Blob Image" style={{ width: 100, height: 100 }} />
                        </Box>
                    ))}
                </Box> */}
                <Button variant="contained" color="primary" onClick={handleSubmit} disabled={createProductMutation.isPending}>
                    {createProductMutation.isPending ? <CircularProgress size={24} /> : 'Create Product'}
                </Button>
            </Box>
        </Box>
    );
};

export default withAuth(CreateProductPage);