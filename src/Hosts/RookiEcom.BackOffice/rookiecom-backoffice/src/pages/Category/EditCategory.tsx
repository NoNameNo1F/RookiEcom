import { useState, useEffect } from 'react';
import { useUpdateCategory, useGetCategoryById, useGetCategories } from '../../hooks';
import { Alert, Box, Button, Checkbox, FormControl, FormControlLabel, InputLabel, MenuItem, Select, TextField, Typography } from '@mui/material';
import { ICategoryModel, ICategoryUpdateModel } from '../../interfaces';
import { useNavigate, useParams } from 'react-router-dom';
import withAuth from '../../oidc/withAuth';

const EditCategoryPage: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const categoryId = Number(id);
  const { data: category, isLoading: categoryLoading } = useGetCategoryById(categoryId);
  const { data: categories, isLoading: categoriesLoading } = useGetCategories(1, 100);
  const updateCategoryMutation = useUpdateCategory();

  const [formData, setFormData] = useState<Partial<ICategoryUpdateModel> & { imageFile?: File }>({
    name: '',
    description: '',
    parentId: undefined,
    isPrimary: false,
    image: ''
  });

  const [errors, setErrors] = useState<{ [key: string]: string }>({});

  useEffect(() => {
    if (category) {
      setFormData({
        name: category.name,
        description: category.description,
        parentId: category.parentId,
          isPrimary: category.isPrimary,
        image: category.image
      });
    }
  }, [category]);

  const validateForm = () => {
    const newErrors: { [key: string]: string; } = {};
        if (!formData.name) newErrors.name = 'Name is required';
        if (!formData.description) newErrors.description = 'Description is required';
      if (!formData.parentId &&
          formData.id === formData.parentId) newErrors.parentId = 'ParentId should not be itself required';
        if (!formData.imageFile) newErrors.imageFile = 'Image for Category is required';
        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleCheckboxChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;
    setFormData((prev) => ({ ...prev, [name]: checked }));
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setFormData((prev) => ({ ...prev, imageFile: e.target.files![0] }));
    }
  };

  const handleSubmit = () => {
    if (!validateForm()) return;
    updateCategoryMutation.mutate(
      { categoryId, category: formData as Omit<ICategoryModel, 'id' | 'hasChild'> & { imageFile?: File } },
      {
        onSuccess: () => navigate('/categories'),
        onError: (error) => alert(error.message),
      }
    );
  };

  if (categoryLoading || categoriesLoading) {
    return <Box>Loading...</Box>;
  }

  if (updateCategoryMutation.error) {
    return <Alert severity="error">{updateCategoryMutation.error.message}</Alert>;
  }

  return (
    <Box sx={{ maxWidth: 800, mx: 'auto', py: 2 }}>
      <Typography variant="h4" gutterBottom>
        Edit Category
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
          Upload New Image
          <input
            type="file"
            hidden
            onChange={handleFileChange}
            accept="image/*"
          />
        </Button>
        {formData.imageFile && <Typography>New image selected: {formData.imageFile.name}</Typography>}
              {formData?.image &&
                <img
                    srcSet={`${formData.image}?w=164&h=164&fit=crop&auto=format&dpr=2 2x`}
                    src={`${formData.image}?w=164&h=164&fit=crop&auto=format`}
                    alt={formData.name}
                    loading="lazy"
                />
        }
        <Button
          variant="contained"
          color="primary"
          onClick={handleSubmit}
          disabled={updateCategoryMutation.isPending}
        >
          {updateCategoryMutation.isPending ? 'Updating...' : 'Update Category'}
        </Button>
      </Box>
    </Box>
  );
};

export default withAuth(EditCategoryPage);