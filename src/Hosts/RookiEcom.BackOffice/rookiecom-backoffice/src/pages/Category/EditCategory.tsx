import React, { useState, useEffect, useCallback } from 'react';
import { Controller, SubmitHandler, useForm } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router-dom';
import { useUpdateCategory, useGetCategoryById, useGetCategories } from '../../hooks';
import { Alert, Box, Button, Checkbox, CircularProgress, Divider, FormControl, FormControlLabel, FormHelperText, Grid, IconButton, InputLabel, MenuItem, Paper, Select, Stack, TextField, Typography } from '@mui/material';
import { ICategoryModel, ICategoryUpdateForm } from '../../interfaces';
import withAuth from '../../oidc/withAuth';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';

const EditCategoryPage: React.FC = () => {
    const navigate = useNavigate();
    const { id } = useParams<{ id: string }>();
    const categoryId = Number(id);
    
    const { data: category, isLoading: categoryLoading, error: categoryError } = useGetCategoryById(categoryId);
    
    const { data: categoriesResult, isLoading: categoriesLoading, error: categoriesError } = useGetCategories(1, 100);

    const updateCategoryMutation = useUpdateCategory();

    const allCategories = categoriesResult?.items ?? [];

    const {
        register,
        handleSubmit,
        control,
        watch,
        reset,
        formState: { errors, isSubmitting, isDirty }
    } = useForm<ICategoryUpdateForm>({
        defaultValues: {
            id: categoryId,
            name: '',
            description: '',
            parentId: undefined,
            isPrimary: false,
            imageFile: undefined,
        }
    });

    const [currentImageUrl, setCurrentImageUrl] = useState<string | undefined>(undefined);
    const [previewImageUrl, setPreviewImageUrl] = useState<string | undefined>(undefined);

  useEffect(() => {
    if (category) {
        reset({
            id: category.id,
            name: category.name,
            description: category.description || '',
            parentId: category.parentId || 0,
            isPrimary: category.isPrimary,
            imageFile: undefined
        });
        setCurrentImageUrl(category.image);
        setPreviewImageUrl(undefined);
    }
  }, [category, reset]);

    const imageFile = watch("imageFile");
    useEffect(() => {
        let newUrl: string | undefined = undefined;
        if (imageFile && imageFile.length > 0) {
            newUrl = URL.createObjectURL(imageFile[0]);
            setPreviewImageUrl(newUrl);
        } else {
            setPreviewImageUrl(undefined);
        }
        
        return () => {
            if (newUrl) {
                URL.revokeObjectURL(newUrl);
            }
        };
    }, [imageFile]);

    const onSubmit: SubmitHandler<ICategoryUpdateForm> = (data) => {
        updateCategoryMutation.mutate(data, {
            onSuccess: () => {
                navigate('/categories');
            },
            onError: (error) => {
                console.error("Update category failed:", error);
            }
        });
    };

     const handleGoBack = useCallback(() => {
        navigate('/categories');
     }, [navigate]);
    

    const isLoading = categoryLoading || categoriesLoading;
    const mutationPending = updateCategoryMutation.isPending;

    const pageError = categoryError || categoriesError || updateCategoryMutation.error;

  return (
    <Paper sx={{ p: { xs: 2, sm: 3 }, margin: { xs: 1, sm: 3 } }}>
      <Stack direction="row" spacing={1} alignItems="center" sx={{ mb: 3 }}>
              <IconButton onClick={handleGoBack} size="small" disabled={mutationPending}
              >
                    <ArrowBackIcon />
                </IconButton>
                <Typography variant="h4" component="h1">
                     Edit Category ({category?.name || id})
                </Typography>
          </Stack>

          {pageError && <Alert severity="error" sx={{ mb: 2 }}>{(pageError as Error).message}</Alert>}

          {isLoading && !pageError && (
                 <Box sx={{ display: 'flex', justifyContent: 'center', p: 5 }}><CircularProgress /></Box>
          )}
          
          {!isLoading && !pageError && category && (
                <Box component="form" onSubmit={handleSubmit(onSubmit)} noValidate sx={{ mt: 1 }}>
                    <Grid container spacing={3}>
                        <Grid size={{xs: 12, md: 8}}>
                            <Stack spacing={3}>
                                 <TextField
                                    label="Category Name"
                                    required
                                    fullWidth
                                    {...register("name", { required: "Category name is required" })}
                                    error={!!errors.name}
                                    helperText={errors.name?.message}
                                    disabled={mutationPending}
                                />
                                <TextField
                                    label="Description"
                                    required  
                                    fullWidth
                                    multiline
                                    rows={4}
                                  {...register("description", {
                                        required: "Description field is required"
                                    })}
                                    disabled={mutationPending}
                                />
                                <Controller
                                    name="parentId"
                                    control={control}
                                    rules={{
                                      validate: value => (value === categoryId ? 'Category cannot be its own parent' : true)
                                    }}
                                    render={({ field }) => (
                                        <FormControl fullWidth error={!!errors.parentId}>
                                            <InputLabel id="parent-category-edit-label">Parent Category (Optional)</InputLabel>
                                            <Select
                                                labelId="parent-category-edit-label"
                                                label="Parent Category (Optional)"
                                                {...field}
                                                value={field.value || 0}
                                                onChange={(e) => field.onChange(Number(e.target.value) || 0)}
                                                disabled={mutationPending || categoriesLoading}
                                            >
                                                <MenuItem value={0}>
                                                    <em>None (Top Level)</em>
                                                </MenuItem>
                                                {allCategories
                                                    .filter(cat => cat.id !== categoryId)
                                                    .map((cat: ICategoryModel) => (
                                                        <MenuItem key={cat.id} value={cat.id}>
                                                            {cat.name}
                                                        </MenuItem>
                                                    ))}
                                            </Select>
                                            {errors.parentId && <FormHelperText>{errors.parentId.message}</FormHelperText>}
                                        </FormControl>
                                    )}
                                />
                                <Controller
                                    name="isPrimary"
                                    control={control}
                                    render={({ field }) => (
                                        <FormControlLabel
                                            control={
                                                <Checkbox
                                                    {...field}
                                                    checked={!!field.value}
                                                    disabled={mutationPending}
                                                />
                                            }
                                            label="Is Primary Category"
                                        />
                                    )}
                                />
                            </Stack>
                        </Grid>

                        {/* Right Column */}
                        <Grid size={{xs: 12, md: 4}}>
                             <Stack spacing={2}>
                                 <Typography variant="h6" component="h2">Image</Typography>
                    
                                    <Box sx={{ mb: 1, border: '1px solid', borderColor: 'divider', padding: 1, borderRadius: 1, minHeight: 100, display: 'flex', alignItems: 'center', justifyContent: 'center', position: 'relative' }}>
                                    {previewImageUrl ? (
                                        <>
                                          <Typography variant="caption" display="block" sx={{ position: 'absolute', top: 4, left: 8 }}>New Preview:</Typography>
                                          <img src={previewImageUrl} alt="New preview" style={{ maxWidth: '100%', maxHeight: '200px', height: 'auto', display: 'block' }} />
                                        </>
                                    ) : currentImageUrl && (
                                        <>
                                          <Typography variant="caption" display="block" sx={{ position: 'absolute', top: 4, left: 8 }}>Current:</Typography>
                                          <img src={currentImageUrl} alt={category.name} loading="lazy" style={{ maxWidth: '100%', maxHeight: '200px', height: 'auto', display: 'block' }} />
                                        </>
                                    )}
                                </Box>
                                
                                <FormControl error={!!errors.imageFile}>
                                    <Button component="label" variant="outlined" fullWidth disabled={mutationPending}>
                                        {currentImageUrl ? 'Upload New (Replaces Existing)' : 'Upload Image'}
                                        <input
                                            type="file"
                                            hidden
                                            accept="image/*"
                                            {...register("imageFile")}
                                            disabled={mutationPending}
                                        />
                                    </Button>
                                    {imageFile?.[0] && <Typography variant="caption" color="text.secondary">{imageFile[0].name} selected</Typography>}
                                    {errors.imageFile && <FormHelperText>{errors.imageFile.message}</FormHelperText>}
                              </FormControl>

                                <Divider sx={{ my: 2 }} />
                                 {/* Actions */}
                                <Stack direction="row" spacing={2} justifyContent="flex-end">
                                    <Button variant="outlined" onClick={() => navigate('/categories')} disabled={mutationPending}>
                                        Cancel
                                    </Button>
                                     <Button type="submit" variant="contained" color="primary" disabled={mutationPending || isSubmitting || !isDirty}>
                                        {mutationPending || isSubmitting ? <CircularProgress size={24} color="inherit" /> : 'Update Category'}
                                    </Button>
                                </Stack>
                             </Stack>
                        </Grid>
                    </Grid>
                </Box>
          )}          
    </Paper>
  );
};

export default withAuth(EditCategoryPage);