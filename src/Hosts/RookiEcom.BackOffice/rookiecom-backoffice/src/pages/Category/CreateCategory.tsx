import { useCallback, useEffect, useState } from "react";
import { useForm, Controller, SubmitHandler } from "react-hook-form";
import { useCreateCategory, useGetCategories } from "../../hooks";
import { MiniLoaderPage } from "../../components/common";
import { Alert, Box, Button, Checkbox, CircularProgress, FormControl, FormControlLabel, FormHelperText, InputLabel, MenuItem, Paper, Select, Stack, TextField, Typography } from "@mui/material";
import { ICategoryModel, ICategoryCreateForm } from "../../interfaces";
import { useNavigate } from "react-router-dom";
import withAuth from "../../oidc/withAuth";
import ImageIcon from '@mui/icons-material/Image';

const CreateCategoryPage: React.FC  = () => {
    const navigate = useNavigate();
    const { data: categories, isLoading: categoriesLoading, error: categoriesError } = useGetCategories(1, 100);
    const createCategoryMutation = useCreateCategory();

    const {
        register,
        handleSubmit,
        control,
        watch,
        formState: { errors, isSubmitting },
    } = useForm<ICategoryCreateForm>({
        defaultValues: {
            name: '',
            description: '',
            parentId: 0,
            isPrimary: false,
            imageFile: undefined,
        }
    });
    const imageFile = watch("imageFile");
    const [imageUrl, setImageUrl] = useState<string | undefined>(undefined);
    
    useEffect(() => {
        let url: string | undefined = undefined;
            
        if (imageFile && imageFile.length > 0) {
            url = URL.createObjectURL(imageFile[0]);
            setImageUrl(url);
        } else {
            setImageUrl(undefined);
        }

        return () => {
            if (url) {
                URL.revokeObjectURL(url);
            }
        };
    }, [imageFile]);

    const onSubmit: SubmitHandler<ICategoryCreateForm> = (data) => {
        createCategoryMutation.mutate(data, {
            onSuccess: () => {
                navigate('/categories');
            },
            onError: (error) => {
                //Modal notify
            }
        });
    }

    const handleGoBack = useCallback(() => {
        navigate('/categories');
    }, [navigate]);

    if (categoriesLoading) {
        return <MiniLoaderPage text="Loading..." />;
    }

    const pageError = createCategoryMutation.error || categoriesError;

    return (
        <Paper sx={{ p: { xs: 2, sm: 3 }, margin: { xs: 1, sm: 3 } }}>
            <Stack direction="row" spacing={1} alignItems="center" sx={{ mb: 3 }}>
<Typography variant="h4" component="h1">
                Create Category
                </Typography>
            </Stack>
            
            {pageError && (
                <Alert severity="error" sx={{ mb: 2 }}>
                    Error: {(pageError as Error).message || "An unexpected error occurred."}
                </Alert>
            )}

            <Box
                component="form"
                onSubmit={handleSubmit(onSubmit)}
                noValidate sx={{ display: 'flex', flexDirection: 'column', gap: 2.5 }}
            >
                 <TextField
                    label="Name"
                    required
                    fullWidth
                    {...register("name", { required: "Category name is required" })}
                    error={!!errors.name}
                    helperText={errors.name?.message}
                    disabled={isSubmitting}
                />

                <TextField
                    label="Description"
                    required
                    fullWidth
                    multiline
                    rows={4}
                    {...register("description", { required: "Description field is required" })}
                    error={!!errors.description}
                    helperText={errors.description?.message}
                    disabled={isSubmitting}
                />

                <Controller
                    name="parentId"
                    control={control}
                    render={({ field }) => (
                        <FormControl fullWidth error={!!errors.parentId}>
                            <InputLabel id="parent-category-label">Parent Category (Optional)</InputLabel>
                            <Select
                                labelId="parent-category-label"
                                label="Parent Category (Optional)"
                                {...field}
                                value={field.value || 0}
                                onChange={(e) => field.onChange(Number(e.target.value) || undefined)}
                                disabled={isSubmitting || categoriesLoading}
                            >
                                <MenuItem value={0}>
                                    <em>None (Top Level)</em>
                                </MenuItem>
                                {(categories?.items ?? []).map((category: ICategoryModel) => (
                                    <MenuItem key={category.id} value={category.id}>
                                        {category.name}
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
                                    checked={field.value}
                                    disabled={isSubmitting}
                                />
                            }
                            label="Is Primary Category"
                        />
                    )}
                />

                 <FormControl error={!!errors.imageFile}>
                    <Button
                        component="label"
                        variant="outlined"
                        disabled={isSubmitting}
                        startIcon={<ImageIcon />}
                    >
                        Upload Category Image *
                        <input
                            type="file"
                            hidden
                            accept="image/*"
                            {...register("imageFile", { required: "Category image is required" })}
                        />
                    </Button>
                    {errors.imageFile && <FormHelperText>{errors.imageFile.message}</FormHelperText>}
                </FormControl>

                {/* Image Preview */}
                {imageUrl && (
                    <Box sx={{ mt: 1, mb: 1, border: '1px solid', borderColor: 'divider', padding: 1, borderRadius: 1, maxWidth: 200 }}>
                        <Typography variant="caption" display="block" sx={{ mb: 0.5 }}>Image Preview:</Typography>
                        <img
                            src={imageUrl}
                            alt="Preview"
                            style={{ display: 'block', width: '100%', height: 'auto', objectFit: 'cover' }}
                            loading="lazy"
                        />
                    </Box>
                )}

                {/* Submit Button */}
                <Stack direction="row" spacing={2} justifyContent="flex-end" sx={{ mt: 2 }}>
                    <Button
                        variant="outlined"
                        onClick={handleGoBack}
                        disabled={isSubmitting}
                    >
                        Cancel
                    </Button>
                    <Button
                        type="submit"
                        variant="contained"
                        color="primary"
                        disabled={isSubmitting || createCategoryMutation.isPending}
                    >
                        {isSubmitting || createCategoryMutation.isPending ? <CircularProgress size={24} /> : 'Create Category'}
                    </Button>
                </Stack>
            </Box>
        </Paper>
    );
};

export default withAuth(CreateCategoryPage);