import { useCallback, useEffect, useState } from 'react';
import { useCreateProduct, useGetCategories } from '../../hooks';
import { Box, Typography, TextField, Button, MenuItem, Select, FormControl, InputLabel, CircularProgress, Alert, FormControlLabel, Checkbox, IconButton, Stack, Paper, Divider, Grid, FormHelperText } from '@mui/material';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { useNavigate } from 'react-router-dom';
import { ICategoryModel, IProductCreateForm } from '../../interfaces';
import withAuth from '../../oidc/withAuth';
import { Controller, SubmitHandler, useFieldArray, useForm } from 'react-hook-form';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import ImageIcon from '@mui/icons-material/Image';
import { toast } from 'react-toastify';

const MIN_IMAGES = 1;
const MAX_IMAGES = 5;

const CreateProductPage: React.FC = () => {
    const navigate = useNavigate();
    const { data: categoriesResult, isLoading: categoriesLoading, error: categoriesError } = useGetCategories(1, 100);
    const createProductMutation = useCreateProduct();

    const categories = categoriesResult?.items ?? [];
    
    const {
        register,
        handleSubmit,
        control,
        watch,
        formState: { errors, isSubmitting },
    } = useForm<IProductCreateForm>({
        defaultValues: {
            sku: '',
            name: '',
            description: '',
            price: 0,
            marketPrice: 0,
            stock: 0,
            categoryId: 0,
            isFeature: false,
            imageFiles: undefined,
            productAttributes: [{ code: '', value: '' }],
            variationOption: { code: '', values: [""] }
        }
    });

    const { fields: attributeFields, append: appendAttribute, remove: removeAttribute } = useFieldArray<IProductCreateForm, "productAttributes", "id">({
        control,
        name: "productAttributes"
    });

    const { fields: variationValueFields, append: appendVariationValue, remove: removeVariationValue } = useFieldArray<
        IProductCreateForm,
        "variationOption.values",
        "id"
     >({
        control,
        name: "variationOption.values"
    });

     const imageFiles = watch("imageFiles");
    const [imagePreviews, setImagePreviews] = useState<string[]>([]);

    useEffect(() => {
        const newUrls: string[] = [];
        if (imageFiles && imageFiles.length > 0) {
             if (imageFiles.length > MAX_IMAGES) {
                 toast.warn(`Cannot exceed ${MAX_IMAGES} images.`);
                 return;
             }
            for (let i = 0; i < imageFiles.length; i++) {
                newUrls.push(URL.createObjectURL(imageFiles[i]));
            }
            setImagePreviews(newUrls);
        } else { setImagePreviews([]); }
        return () => newUrls.forEach(url => URL.revokeObjectURL(url));
    }, [imageFiles]);

    const onSubmit: SubmitHandler<IProductCreateForm> = (data) => {
        if (!data.imageFiles || data.imageFiles.length < MIN_IMAGES) {
             alert(`Please upload at least ${MIN_IMAGES} image(s).`);
             return;
         }
         if (data.imageFiles.length > MAX_IMAGES) {
             alert(`You cannot upload more than ${MAX_IMAGES} images.`);
             return;
        }
        
        createProductMutation.mutate(data, {
            onSuccess: () => {
                navigate('/products');
            },
            onError: (error) => {
                const problemDetails = (error as any)?.response?.data;
                if (problemDetails?.status === 400) {
                    toast.error(problemDetails.detail || 'Validation failed');
                    if (problemDetails.extensions?.errors) {
                        problemDetails.extensions.errors.forEach((err: any) => toast.error(`${err.propertyName}: ${err.errorMessage}`));
                    }
                }
            },
        });
    };

    const handleGoBack = useCallback(() =>
        navigate('/products'),
    [navigate]);

    // Combine loading states
    const isLoading = categoriesLoading || isSubmitting || createProductMutation.isPending;

    return (
        <Paper sx={{ p: { xs: 2, sm: 3 }, margin: { xs: 1, sm: 3 } }}>
            <Stack direction="row" spacing={1} alignItems="center" sx={{ mb: 3 }}>
                 <IconButton onClick={handleGoBack} size="small" disabled={isLoading}>
                    <ArrowBackIcon />
                </IconButton>
                <Typography variant="h4" component="h1">
                    Create Product
                </Typography>
            </Stack>
            
            {categoriesError &&
                <Alert severity="error" sx={{ mb: 2 }}>
                    Failed to load categories: {(categoriesError as Error).message}
                </Alert>
            }
            

            <Box component="form" onSubmit={handleSubmit(onSubmit)} noValidate>
                <Grid container spacing={3}>
                    <Grid size={{ xs: 12, md: 8 }}>
                        <Stack spacing={3}>
                            <TextField
                                label="Product Name"
                                required fullWidth
                                {...register('name', { required: 'Product name is required', maxLength: { value: 100, message: 'Name must not exceed 100 characters' } })}
                                error={!!errors.name}
                                helperText={errors.name?.message}
                                disabled={isLoading}
                            />
                             <TextField
                                label="SKU (Stock Keeping Unit)"
                                required fullWidth
                                {...register('sku', { required: 'SKU is required', maxLength: { value: 100, message: 'SKU must not exceed 100 characters' } })}
                                error={!!errors.sku}
                                helperText={errors.sku?.message}
                                disabled={isLoading}
                            />
                            <TextField
                                label="Description"
                                fullWidth multiline rows={4}
                                {...register('description', { maxLength: { value: 512, message: 'Description must not exceed 512 characters' } })}
                                error={!!errors.description}
                                helperText={errors.description?.message}
                                disabled={isLoading}
                            />

                            
                            <Typography variant="h6" component="h2" sx={{ pt: 1 }}>Pricing & Stock</Typography>
                            <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
                                <TextField
                                    label="Price" type="number" required fullWidth
                                    InputProps={{ inputProps: { min: 0.01, step: "any" } }}
                                    {...register('price', {
                                        required: 'Price is required',
                                        valueAsNumber: true,
                                        min: { value: 0.01, message: 'Price must be greater than 0' },
                                    })}
                                    error={!!errors.price}
                                    helperText={errors.price?.message}
                                    disabled={isLoading}
                                />
                                <TextField
                                    label="Market Price (Optional)" type="number" fullWidth
                                    InputProps={{ inputProps: { min: 0, step: "any" } }}
                                    {...register("marketPrice", {
                                        valueAsNumber: true,
                                        min: { value: 0, message: "Market price cannot be negative" }
                                     })}
                                    error={!!errors.marketPrice} helperText={errors.marketPrice?.message || "Original price before discount"}
                                    disabled={isLoading}
                                />
                                <TextField
                                    label="Stock Quantity" type="number" required fullWidth
                                    InputProps={{ inputProps: { min: 0, step: 1 } }}
                                    {...register("stock", {
                                        required: "Stock quantity is required",
                                        valueAsNumber: true,
                                        min: { value: 0, message: "Stock cannot be negative" }
                                    })}
                                    error={!!errors.stock}
                                    helperText={errors.stock?.message}
                                    disabled={isLoading}
                                />
                            </Stack>

                             <Divider sx={{ pt: 1 }} />
                             <Typography variant="h6" component="h2">Attributes</Typography>
                             <Stack spacing={2}>
                                {attributeFields.map((field, index) => (
                                    <Stack direction="row" spacing={1} key={field.id} alignItems="center">
                                        <TextField
                                            label={`Attribute ${index + 1} Code`} size="small" sx={{flexGrow: 1}}
                                            {...register(`productAttributes.${index}.code`, { required: 'Attribute code required' })}
                                            error={!!errors.productAttributes?.[index]?.code}
                                            helperText={errors.productAttributes?.[index]?.code?.message}
                                            disabled={isLoading}
                                        />
                                        <TextField
                                            label={`Attribute ${index + 1} Value`} size="small" sx={{flexGrow: 2}}
                                             {...register(`productAttributes.${index}.value`, { required: 'Attribute value required' })}
                                            error={!!errors.productAttributes?.[index]?.value}
                                             helperText={errors.productAttributes?.[index]?.value?.message}
                                            disabled={isLoading}
                                        />
                                        <IconButton onClick={() => removeAttribute(index)} color="error" disabled={isLoading || attributeFields.length <= 1} size="small">
                                            <DeleteOutlineIcon />
                                        </IconButton>
                                    </Stack>
                                ))}
                                <Button
                                    type="button" size="small" variant="outlined"
                                    onClick={() => appendAttribute({ code: '', value: '' })}
                                    startIcon={<AddCircleOutlineIcon />}
                                    disabled={isLoading} sx={{ alignSelf: 'flex-start' }}
                                >
                                    Add Attribute
                                </Button>
                            </Stack>

                            <Divider sx={{ pt: 1 }} />
                             <Typography variant="h6" component="h2">Product Options (e.g., Size, Color)</Typography>
                            <Stack spacing={2}>
                                 <TextField
                                     label="Option Type Code" placeholder="e.g., SIZE or COLOR" fullWidth size="small"
                                     {...register("variationOption.code", { required: 'Option type code is required' })}
                                     error={!!errors.variationOption?.code}
                                     helperText={errors.variationOption?.code?.message}
                                     disabled={isLoading}
                                 />
                                <Typography variant="subtitle2">Option Values:</Typography>
                                {variationValueFields.map((field, index) => (
                                    <Stack direction="row" spacing={1} key={field.id} alignItems="center">
                                        <TextField
                                            label={`Value ${index + 1}`} size="small" fullWidth
                                            {...register(`variationOption.values.${index}`, { required: 'Option value required' })}
                                            error={!!(errors.variationOption?.values?.[index])}
                                            helperText={errors.variationOption?.values?.[index]?.message}
                                            disabled={isLoading}
                                        />
                                         <IconButton onClick={() => removeVariationValue(index)} color="error" disabled={isLoading || variationValueFields.length <= 1} size="small">
                                            <DeleteOutlineIcon />
                                        </IconButton>
                                    </Stack>
                                ))}
                                 <Button
                                    type="button" size="small" variant="outlined"
                                    onClick={() => appendVariationValue('')}
                                    startIcon={<AddCircleOutlineIcon />}
                                    disabled={isLoading} sx={{ alignSelf: 'flex-start' }}
                                >
                                    Add Option Value
                                </Button>
                            </Stack>

                        </Stack>
                    </Grid>
                      <Grid size={{ xs: 12, md: 4 }}>
                        <Paper variant="outlined" sx={{ p: 2 }}>
                            <Stack spacing={3}>
                                <Typography variant="h6" component="h2">Organization</Typography>
                                <Controller
                                    name="categoryId"
                                    control={control}
                                    rules={{ required: "Category is required", min: { value: 1, message: "Please select a category"} }}
                                    render={({ field }) => (
                                        <FormControl fullWidth required error={!!errors.categoryId}>
                                            <InputLabel id="category-select-label">Category</InputLabel>
                                            <Select
                                                labelId="category-select-label"
                                                label="Category"
                                                {...field}
                                                value={field.value || ''}
                                                onChange={(e) => field.onChange(Number(e.target.value) || 0)}
                                                disabled={isLoading || categoriesLoading}
                                            >
                                                <MenuItem value={0} disabled><em>Select Category...</em></MenuItem>
                                                {(categories || []).map((category: ICategoryModel) => (
                                                    <MenuItem key={category.id} value={category.id}>
                                                        {category.name}
                                                    </MenuItem>
                                                ))}
                                            </Select>
                                            {errors.categoryId && <FormHelperText>{errors.categoryId.message}</FormHelperText>}
                                        </FormControl>
                                    )}
                                />
                                <Controller
                                    name="isFeature"
                                    control={control}
                                    render={({ field }) => (
                                        <FormControlLabel
                                            control={<Checkbox {...field} checked={!!field.value} disabled={isLoading} />}
                                            label="Featured Product"
                                        />
                                    )}
                                />

                                <Divider />
                                <Typography variant="h6" component="h2">Images</Typography>
                                 <Typography variant="body2" color={ (imageFiles?.length ?? 0) < MIN_IMAGES || (imageFiles?.length ?? 0) > MAX_IMAGES ? 'error' : 'text.secondary'} sx={{mb: 1}}>
                                     ({imageFiles?.length ?? 0} / {MAX_IMAGES}) images. Min: {MIN_IMAGES}, Max: {MAX_IMAGES}.
                                 </Typography>
                                <FormControl error={!!errors.imageFiles}>
                                    <Button component="label" variant="outlined" fullWidth disabled={isLoading} startIcon={<ImageIcon />} >
                                        Upload Images *
                                        <input type="file" hidden multiple accept="image/*"
                                            {...register("imageFiles", {
                                                 required: `At least ${MIN_IMAGES} image is required`,
                                                 validate: {
                                                    min: files => (files?.length ?? 0) >= MIN_IMAGES || `Requires minimum ${MIN_IMAGES} images`,
                                                    max: files => (files?.length ?? 0) <= MAX_IMAGES || `Cannot exceed ${MAX_IMAGES} images`,
                                                    type: (files) =>
                                                    !files ||
                                                    files.length === 0 ||
                                                    Array.from(files).every((file) => file.type.startsWith('image/')) ||
                                                    'All files must be images (e.g., JPEG, PNG)',
                                                 }
                                             })}
                                        />
                                    </Button>
                                    {errors.imageFiles && <FormHelperText>{errors.imageFiles.message}</FormHelperText>}
                                </FormControl>

                                {imagePreviews.length > 0 && (
                                    <Stack direction="row" spacing={1} sx={{ flexWrap: 'wrap', mt: 1 }}>
                                        {imagePreviews.map((url, index) => (
                                            <Box key={index} sx={{ border: '1px solid lightgrey', borderRadius: 1, p: 0.5 }}>
                                                <img src={url} alt={`Preview ${index + 1}`} style={{ display: 'block', width: 80, height: 80, objectFit: 'cover' }} />
                                            </Box>
                                        ))}
                                    </Stack>
                                )}

                                <Divider sx={{ pt: 1 }} />
                                <Stack direction="row" spacing={2} justifyContent="flex-end" sx={{ pt: 1 }}>
                                    <Button variant="outlined" onClick={handleGoBack} disabled={isLoading}> Cancel </Button>
                                    <Button type="submit" variant="contained" color="primary" disabled={isLoading}>
                                        {isLoading ? <CircularProgress size={24} color="inherit" /> : 'Create Product'}
                                    </Button>
                                </Stack>
                            </Stack>
                        </Paper>
                    </Grid>
                </Grid>
            
            </Box>
        </Paper>
    );
};

export default withAuth(CreateProductPage);
