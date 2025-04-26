import { useState, useEffect, useCallback, useMemo } from 'react';
import { useUpdateProduct, useGetProductBySKU, useGetCategories } from '../../hooks';
import { Alert, Box, Button, Checkbox, CircularProgress, Divider, FormControl, FormControlLabel, FormHelperText, Grid, IconButton, InputLabel, MenuItem, Paper, Select, Stack, TextField, Typography } from '@mui/material';
import { ICategoryModel, IProductUpdateForm,  } from '../../interfaces';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import ImageIcon from '@mui/icons-material/Image';
import { useNavigate, useParams } from 'react-router-dom';
import withAuth from '../../oidc/withAuth';
import { Controller, SubmitHandler, useFieldArray, useForm, useWatch } from 'react-hook-form';
import { toast } from 'react-toastify';
import { getProductStatusText } from '../../utils/helper';
import { ProductStatus } from '../../enums';

const MIN_IMAGES = 1;
const MAX_IMAGES = 5;

const EditProductPage: React.FC = () => {
  const navigate = useNavigate();
    const { sku } = useParams<{ sku: string; }>();
  const { data: product, isLoading: productLoading, error: productError } = useGetProductBySKU(sku!);
    const { data: categoriesResult, isLoading: categoriesLoading, error: categoriesError } = useGetCategories(1, 100);
    const updateProductMutation = useUpdateProduct();

    const categories = categoriesResult?.items ?? [];

    const initialFormValues = useMemo<IProductUpdateForm | undefined>(() => {
        if (!product) return undefined;
        return {
            id: product.id,
            sku: product.sku,
            name: product.name,
            description: product.description || '',
            price: product.price,
            marketPrice: product.marketPrice || 0,
            stock: product.stock,
            categoryId: product.categoryId || 0,
            status: product.status,
            isFeature: product.isFeature || false,
            oldImages: product.images || [],
            newImageFiles: undefined,
            productAttributes: product.productAttributes?.length ? product.productAttributes : [{ code: '', value: '' }],
            variationOption: product.productOption
                ? {
                    code: product.productOption.code || '',
                    values: product.productOption.values?.length ? product.productOption.values : ['']
                  }
                : { code: '', values: [''] }
        };
    }, [product]);

    const {
        register,
        handleSubmit,
        control,
        setValue,
        getValues,
        watch,
        formState: { errors, isSubmitting, isDirty }
    } = useForm<IProductUpdateForm>({
        values: initialFormValues,
    });

    const watchedNewImageFiles = useWatch({ control, name: "newImageFiles" });
    const calculateCurrentTotalImages = () => {
         const oldImg = getValues("oldImages");
         const newImg = getValues("newImageFiles");
         return (oldImg?.length ?? 0) + (newImg?.length ?? 0);
    };
    const watchedOldImages = watch("oldImages", product?.images || []);

    const currentTotalImages = (watchedOldImages?.length ?? 0) + (watchedNewImageFiles?.length ?? 0);
    const canAddMoreImages = currentTotalImages < MAX_IMAGES;
    const minimumImagesMet = currentTotalImages >= MIN_IMAGES;
    
    const [newImagePreviews, setNewImagePreviews] = useState<string[]>([]);

    useEffect(() => {
        const urls: string[] = [];
        const files = getValues("newImageFiles");
        if (files && files.length > 0) {
             const potentialTotal = (watchedOldImages?.length ?? 0) + files.length;
             if (potentialTotal > MAX_IMAGES) {
                 setValue('newImageFiles', undefined);
                 toast.warn(`You can only select up to ${MAX_IMAGES - (watchedOldImages?.length ?? 0)} new image(s).`);
                 return;
             }
            for (let i = 0; i < files.length; i++) { urls.push(URL.createObjectURL(files[i])); }
            setNewImagePreviews(urls);
        } else { setNewImagePreviews([]); }
        return () => urls.forEach(url => URL.revokeObjectURL(url));
    }, [watchedNewImageFiles, watchedOldImages, getValues, setValue]);
    
    const handleRemoveOldImage = (imageUrlToRemove: string) => {
        const currentOldImagesValue = getValues("oldImages") || [];
        const currentNewFilesValue = getValues("newImageFiles");
        const currentNewCount = currentNewFilesValue?.length ?? 0;

        if (currentOldImagesValue.length - 1 + currentNewCount < MIN_IMAGES) {
            toast.error(`Cannot remove image. Product must have at least ${MIN_IMAGES} image.`);
            return;
        }
        setValue(
            "oldImages",
            currentOldImagesValue.filter(url => url !== imageUrlToRemove),
            { shouldDirty: true }
        );
    };

    const onSubmit: SubmitHandler<IProductUpdateForm> = (data) => {
        const finalOldCount = data.oldImages?.length ?? 0;
        const finalNewCount = data.newImageFiles?.length ?? 0;
        if (finalOldCount + finalNewCount < MIN_IMAGES || finalOldCount + finalNewCount > MAX_IMAGES) {
            toast.error(`Product must have between ${MIN_IMAGES} and ${MAX_IMAGES} images.`);
            return;
        }
        
        updateProductMutation.mutate(data, {
            onSuccess: () => navigate('/products'),
        });
    };
    
    const handleGoBack = useCallback(() =>
        navigate('/products')
        , [navigate]);
    
    const { fields: attributeFields,
        append: appendAttribute,
        remove: removeAttribute } = useFieldArray<IProductUpdateForm, "productAttributes", "id">({ control, name: "productAttributes" });

    const { fields: variationValueFields,
        append: appendVariationValue,
        remove: removeVariationValue } = useFieldArray<IProductUpdateForm, "variationOption.values", "id">({ control, name: "variationOption.values" });
    
    const isLoading = productLoading || categoriesLoading;
    const mutationPending = updateProductMutation.isPending || isSubmitting;
    const pageError = productError || categoriesError || updateProductMutation.error;

    return (
        <Paper sx={{ p: { xs: 2, sm: 3 }, margin: { xs: 1, sm: 3 } }}>
            <Stack direction="row" spacing={1} alignItems="center" sx={{ mb: 3 }}>
                <IconButton onClick={handleGoBack} size="small" disabled={mutationPending}>
                    <ArrowBackIcon />
                </IconButton>
                <Typography variant="h4" component="h1">
                    Edit Product ({product?.name || sku})
                </Typography>
            </Stack>

            {pageError && !productLoading &&
                <Alert severity="error" sx={{ mb: 2 }}>
                    Error: {(pageError as Error).message || "An error occurred."}
                </Alert>}
            
            {(isLoading || !initialFormValues) && !pageError && (
                <Box sx={{ display: 'flex', justifyContent: 'center', p: 5 }}>
                    <CircularProgress />
                </Box>
            )}

        
            {!isLoading && initialFormValues && !pageError && (
                <Box component="form" onSubmit={handleSubmit(onSubmit)} noValidate>
                    <Grid container spacing={3}>
                        <Grid size={{xs: 12, md:8}}>
                           <Stack spacing={3}>
                               <TextField
                                label="Product Name"
                                required fullWidth
                                {...register("name", { required: "Product name is required" })}
                                error={!!errors.name}
                                helperText={errors.name?.message}
                                disabled={isLoading}
                            />
                             <TextField
                                label="SKU (Stock Keeping Unit)"
                                required fullWidth
                                {...register("sku", { required: "SKU is required" })}
                                error={!!errors.sku}
                                helperText={errors.sku?.message}
                                disabled={isLoading}
                            />
                            <TextField
                                label="Description"
                                fullWidth multiline rows={4}
                                {...register("description")}
                                disabled={isLoading}
                            />

                            
                            <Typography variant="h6" component="h2" sx={{ pt: 1 }}>
                                Pricing & Stock   
                            </Typography>
                            <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
                                <TextField
                                    label="Price" type="number" required fullWidth
                                    InputProps={{ inputProps: { min: 0.01, step: "any" } }}
                                    {...register("price", {
                                        required: "Price is required",
                                        valueAsNumber: true,
                                        min: { value: 0.01, message: "Price must be positive" }
                                    })}
                                    error={!!errors.price} helperText={errors.price?.message}
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
                                    error={!!errors.stock} helperText={errors.stock?.message}
                                    disabled={isLoading}
                                />
                            </Stack>

                            <Divider sx={{ pt: 1 }} />
                                <Typography variant="h6" component="h2">
                                    Attributes
                                </Typography>
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
                                            <Controller
                                                name={`variationOption.values.${index}`}
                                                control={control}
                                                rules={{ required: 'Option value required' }}
                                                render={({ field: controllerField, fieldState }) => (
                                                    <TextField
                                                        label={`Value ${index + 1}`}
                                                        size="small"
                                                        fullWidth
                                                        {...controllerField}
                                                        error={!!fieldState.error}
                                                        helperText={fieldState.error?.message}
                                                        disabled={isLoading}
                                                    />
                                                )}
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
                            <Divider sx={{ pt: 1 }} />
                        </Stack>
                    </Grid>
                    <Grid size={{ xs: 12, md: 4 }}>
                        <Paper variant="outlined" sx={{ p: 2 }}>
                            <Stack spacing={3}>
                                <Typography variant="h6" component="h2">Organization & Status</Typography>
                                <Controller
                                    name="categoryId"
                                    control={control}
                                    rules={{ required: "Category is required", min: { value: 1, message: "Please select a category"} }}
                                    render={({ field }) => (
                                        <FormControl fullWidth required error={!!errors.categoryId}>
                                            <InputLabel id="category-edit-label">Category</InputLabel>
                                            <Select
                                                labelId="category-edit-label"
                                                label="Category"
                                                {...field}
                                                value={field.value || ''}
                                                onChange={(e) => field.onChange(Number(e.target.value) || 0)}
                                                disabled={mutationPending || categoriesLoading}
                                            >
                                                <MenuItem value={0} disabled><em>Select Category...</em></MenuItem>
                                                {(categories || []).map((category: ICategoryModel) => (
                                                    <MenuItem key={category.id} value={category.id}>{category.name}</MenuItem>
                                                ))}
                                            </Select>
                                            {errors.categoryId && <FormHelperText>{errors.categoryId.message}</FormHelperText>}
                                        </FormControl>
                                    )}
                                />
                                <Controller
                                    name="status"
                                    control={control}
                                    rules={{ required: "Status is required" }}
                                    render={({ field }) => (
                                        <FormControl fullWidth required error={!!errors.status}>
                                            <InputLabel id="status-select-label">Status</InputLabel>
                                            <Select
                                                labelId="status-select-label"
                                                label="Status"
                                                {...field}
                                                value={field.value ?? ''}
                                                onChange={(e) => field.onChange(Number(e.target.value) || undefined)}
                                                disabled={mutationPending}
                                            >
                                                {Object.values(ProductStatus)
                                                    .filter(value => typeof value === 'number')
                                                    .map((statusValue) => (
                                                        <MenuItem key={statusValue} value={statusValue}>
                                                            {getProductStatusText(statusValue as ProductStatus)}
                                                        </MenuItem>
                                                ))}
                                            </Select>
                                            {errors.status && <FormHelperText>{errors.status.message}</FormHelperText>}
                                        </FormControl>
                                    )}
                                />
                                <Controller
                                    name="isFeature"
                                    control={control}
                                    render={({ field }) => (
                                        <FormControlLabel
                                            control={<Checkbox {...field} checked={!!field.value} disabled={mutationPending} />}
                                            label="Featured Product" />
                                    )}
                                />

                                <Divider />
                                <Typography variant="h6" component="h2">
                                    Images</Typography>
                                <Typography variant="body2"
                                    color={!minimumImagesMet || !canAddMoreImages ? 'error' : 'text.secondary'}
                                    sx={{mb: 1}}
                                >
                                    ({currentTotalImages} / {MAX_IMAGES}) images selected. Min: {MIN_IMAGES}, Max: {MAX_IMAGES}.
                                </Typography>
                                {watchedOldImages && watchedOldImages.length > 0 && (
                                    <Box>
                                        <Typography variant="subtitle2" gutterBottom>Current Images:</Typography>
                                        <Stack direction="row" spacing={1} sx={{ flexWrap: 'wrap', gap: 1 }}>
                                                {watchedOldImages.map((url, index) => {      
                                                const newFiles = getValues("newImageFiles");
                                                const canRemoveThis = (watchedOldImages.length - 1 + (newFiles?.length ?? 0)) >= MIN_IMAGES;
                                                return (
                                                    <Box
                                                        key={url + index}
                                                        sx={{
                                                            position: 'relative',
                                                            border: '1px solid',
                                                            borderColor: 'divider',
                                                            borderRadius: 1,
                                                            p: 0.5,
                                                            width: 88,
                                                            height: 88,
                                                            display: 'flex',
                                                            alignItems: 'center',
                                                            justifyContent: 'center',
                                                            overflow: 'hidden'
                                                        }}
                                                    >
                                                        <img
                                                            src={`${url}?w=80&h=80&fit=crop&auto=format`}
                                                            alt={`Current ${index + 1}`}
                                                            loading="lazy"
                                                            style={{ display: 'block', width: '100%', height: '100%', objectFit: 'cover' }} />
                                                        <IconButton
                                                            size="small"
                                                            onClick={() => handleRemoveOldImage(url)}
                                                            sx={{
                                                                position: 'absolute',
                                                                top: 2,
                                                                right: 2,
                                                                bgcolor: 'rgba(255,255,255,0.7)',
                                                                '&:hover': { bgcolor: 'rgba(255,255,255,0.9)'}
                                                            }}
                                                            disabled={mutationPending || !canRemoveThis} 
                                                            title={canRemoveThis ? "Remove this image" : `Cannot remove, minimum ${MIN_IMAGES} required`}
                                                        >
                                                            <DeleteOutlineIcon fontSize="small" color={canRemoveThis ? "error" : "disabled"} />
                                                        </IconButton>
                                                    </Box>
                                                );
                                            })}
                                        </Stack>
                                    </Box>
                                )}

                                        
                                        <FormControl
                                            error={!!errors.newImageFiles}
                                            sx={{mt: watchedOldImages && watchedOldImages.length > 0 ? 2 : 0}}
                                        >
                                            <Button
                                                component="label"
                                                variant="outlined"
                                                fullWidth
                                                disabled={mutationPending || !canAddMoreImages}
                                                startIcon={<ImageIcon />}
                                            >
                                                Upload New Images {canAddMoreImages ? `(up to ${MAX_IMAGES - currentTotalImages} more)` : '(Max reached)'}
                                                <input
                                                    type="file"
                                                    hidden
                                                    multiple
                                                    accept="image/*"
                                                    {...register("newImageFiles", {
                                                        validate: (files) => {
                                                            const newCount = files?.length ?? 0;
                                                            
                                                            const oldCount = getValues("oldImages")?.length ?? 0;
                                                            const total = oldCount + newCount;
                                                            if (total > MAX_IMAGES) return `Cannot exceed ${MAX_IMAGES} total images.`;
                                                    
                                                            return true;
                                                        }
                                                    })}
                                                    onClick={(event: React.MouseEvent<HTMLInputElement>) => {
                                                         const element = event.target as HTMLInputElement;
                                                         element.value = '';
                                                     }}
                                                    disabled={mutationPending || !canAddMoreImages}
                                                />
                                            </Button>
                                            {errors.newImageFiles && <FormHelperText>{errors.newImageFiles.message}</FormHelperText>}
                                            {!canAddMoreImages && currentTotalImages >= MAX_IMAGES && <FormHelperText>Maximum number of images reached.</FormHelperText>}
                                        </FormControl>

                                        {newImagePreviews.length > 0 && (
                                            <Box sx={{mt: 1}}>
                                                <Typography variant="subtitle2" gutterBottom>New Images Preview:</Typography>
                                                <Stack direction="row" spacing={1} sx={{ flexWrap: 'wrap', gap: 1 }}>
                                                    {newImagePreviews.map((url, index) => (
                                                        <Box
                                                            key={index}
                                                            sx={{
                                                                border: '1px solid',
                                                                borderColor: 'divider',
                                                                borderRadius: 1, p: 0.5,
                                                                width: 88, height: 88,
                                                                display: 'flex', alignItems: 'center', justifyContent: 'center',
                                                                overflow: 'hidden'
                                                            }}>
                                                            <img
                                                                src={url}
                                                                alt={`New Preview ${index + 1}`}
                                                                loading="lazy"
                                                                style={{ display: 'block', width: '100%', height: '100%', objectFit: 'cover' }} />
                                                        </Box>
                                                    ))}
                                                </Stack>
                                            </Box>
                                        )}

                                        <Divider sx={{ pt: 1 }}/>
                                        <Stack direction="row" spacing={2} justifyContent="flex-end" sx={{ pt: 1 }}>
                                            <Button
                                                variant="outlined"
                                                onClick={handleGoBack}
                                                disabled={mutationPending}
                                            >
                                                Cancel
                                            </Button>
                                            <Button
                                                type="submit"
                                                variant="contained"
                                                color="primary"
                                                disabled={mutationPending || !isDirty || !minimumImagesMet || currentTotalImages > MAX_IMAGES }
                                             >
                                                {mutationPending ? <CircularProgress size={24} color="inherit" /> : 'Update Product'}
                                            </Button>
                                        </Stack>
                                   </Stack>
                               </Paper>
                        </Grid>
                    </Grid>
                </Box>
            )}
        </Paper>
  );
};

export default withAuth(EditProductPage);