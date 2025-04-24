import { useState } from 'react';
import { useCreateProduct, useGetCategories } from '../../hooks';
import { Box, Typography, TextField, Button, MenuItem, Select, FormControl, InputLabel, CircularProgress, Alert, FormControlLabel, Checkbox } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { IProductModel, ICategoryModel, IProductAttribute, IVariationOption } from '../../interfaces';
import { MiniLoaderPage } from '../../components/common';
import withAuth from '../../oidc/withAuth';

const CreateProductPage: React.FC = () => {
    const navigate = useNavigate();
    const { data: categories, isLoading } = useGetCategories(1, 100);
    const createProductMutation = useCreateProduct();

    const [formData, setFormData] = useState<Partial<IProductModel> & { imageFiles?: File[] }>({
        sku: '',
        name: '',
        description: '',
        price: 0,
        marketPrice: 0,
        stock: 0,
        categoryId: 0,
        isFeature: false,
        imageFiles: [],
        productAttributes: [] as IProductAttribute[],
        variationOption: {} as IVariationOption,
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
        if(!formData.productAttributes || formData.productAttributes?.length < 0) newErrors.productAttributes = 'Product Attributes must have at least one attribute';
        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleNumberChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData((prev) => ({ ...prev, [name]: Number(value) }));
    };

    const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, checked } = e.target;
        setFormData((prev) => ({ ...prev, [name]: checked }));
    };

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files) {
        setFormData((prev) => ({ ...prev, imageFiles: Array.from(e.target.files!) }));
        }
    };

    const handleSubmit = () => {
        if (!validateForm()) return;
        createProductMutation.mutate(formData as Omit<IProductModel, 'id' | 'sold'> & { imageFiles?: File[]; }, {
            onSuccess: () => navigate('/products'),
            onError: (error) => alert(error.message),
        });
    };

    if (isLoading) {
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
                <FormControl error={!!errors.categoryId}>
                    <InputLabel>Category</InputLabel>
                    <Select
                        name="categoryId"
                        value={formData.categoryId}
                        onChange={(e) => setFormData((prev) => ({
                            ...prev, categoryId: Number(e.target.value)
                        }))}
                    >
                        <MenuItem value="">Select Category</MenuItem>
                        {(categories?.items as Array<ICategoryModel> || []).map((category: ICategoryModel) => (
                            <MenuItem key={category.id} value={category.id}>
                                {category.name}
                            </MenuItem>
                        ))}
                    </Select>
                    {errors.categoryId && <Typography>{errors.categoryId}</Typography>}
                </FormControl>
                <TextField
                    label="Price"
                    name="price"
                    type="number"
                    value={formData.price}
                    onChange={handleNumberChange}
                    error={!!errors.price}
                    helperText={errors.price}
                    required
                />
                <TextField
                    label="MarketPrice"
                    name="marketPrice"
                    type="number"
                    value={formData.marketPrice}
                    onChange={handleNumberChange}
                    error={!!errors.marketPrice}
                    helperText={errors.marketPrice}
                    required
                />
                <TextField
                    label="Stock"
                    name="stock"
                    type="number"
                    value={formData.stock}
                    onChange={handleNumberChange}
                    error={!!errors.stock}
                    helperText={errors.stock}
                    required
                />
                <FormControlLabel
                    control={
                        <Checkbox
                        name="isFeature"
                        checked={formData.isFeature || false}
                        onChange={handleCheckboxChange}
                        />
                    }
                    label="Is Featured"
                />
                <Button component="label" variant="outlined">
                    Upload Images
                    <input
                        type="file"
                        hidden
                        multiple
                        onChange={handleFileChange}
                        accept="image/*"
                    />
                </Button>
                {formData.imageFiles && formData.imageFiles.length > 0 && (
                    <Typography>{formData.imageFiles.length} image(s) selected</Typography>
                )}
                <Button variant="contained" color="primary" onClick={handleSubmit} disabled={createProductMutation.isPending}>
                    {createProductMutation.isPending ? <CircularProgress size={24} /> : 'Create Product'}
                </Button>
            </Box>
        </Box>
    );
};

export default withAuth(CreateProductPage);