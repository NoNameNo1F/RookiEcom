import { useState, useEffect } from 'react';
import { useUpdateProduct, useGetProductBySKU, useGetCategories } from '../../hooks';
import { Alert, Box, Button, Checkbox, FormControl, FormControlLabel, InputLabel, MenuItem, Select, TextField, Typography } from '@mui/material';
import { ICategoryModel, IProductAttribute, IProductUpdateModel, IVariationOption } from '../../interfaces';
import { useNavigate, useParams } from 'react-router-dom';
import { format } from 'path';
import withAuth from '../../oidc/withAuth';

const EditProductPage: React.FC = () => {
  const navigate = useNavigate();
  const { sku } = useParams<{ sku: string }>();
  const { data: product, isLoading: productLoading, error } = useGetProductBySKU(sku!);
  const { data: categories, isLoading: categoriesLoading } = useGetCategories(1, 100);
  const updateProductMutation = useUpdateProduct();

    const [formData, setFormData] = useState<Partial<IProductUpdateModel>>({
      id: 0,
    sku: '',
    name: '',
    description: '',
    price: 0,
    marketPrice: 0,
    stock: 0,
    categoryId: 0,
    isFeature: false,
    oldImages: [],
    newImages: [],
    productAttributes: [] as IProductAttribute[],
    variationOption: {} as IVariationOption,
  });

  const [errors, setErrors] = useState<{ [key: string]: string }>({});

  useEffect(() => {
    if (product) {
        setFormData({
          id: product.id,
        sku: product.sku,
        name: product.name,
        description: product.description,
        price: product.price,
        marketPrice: product.marketPrice,
        stock: product.stock,
        categoryId: product.categoryId,
        isFeature: product.isFeature,
        oldImages: product.images || [],
          newImages: [],
         productAttributes: product.productAttributes || [],
        variationOption: product.variationOption,
      });
    }
  }, [product]);

  const validateForm = () => {
    const newErrors: { [key: string]: string } = {};
    if (!formData.sku) newErrors.sku = 'SKU is required';
    if (!formData.name) newErrors.name = 'Name is required';
    if (!formData.categoryId) newErrors.categoryId = 'Category is required';
    if (formData.price! <= 0) newErrors.price = 'Price must be greater than 0';
    if (formData.marketPrice! <= 0) newErrors.marketPrice = 'Market Price must be greater than 0';
      if (formData.stock! < 0) newErrors.stock = 'Stock cannot be negative';
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
      setFormData((prev) => ({ ...prev, newImages: Array.from(e.target.files!) }));
    }
  };

  const handleSubmit = () => {
    if (!validateForm()) return;
    updateProductMutation.mutate(
      {product: formData as IProductUpdateModel }, {
        onSuccess: () => navigate('/products'),
        onError: (error) => alert(error.message),
      }
    );
  };

  if (productLoading || categoriesLoading) {
    return <Box>Loading...</Box>;
  }

    if (error) {
        return <Alert severity="error">{(error as Error).message}</Alert>;
    }   

  if (updateProductMutation.error) {
    return <Alert severity="error">{updateProductMutation.error.message}</Alert>;
  }

  return (
    <Box sx={{ maxWidth: 800, mx: 'auto', py: 2 }}>
      <Typography variant="h4" gutterBottom>
        Edit Product
      </Typography>
          <Box component="form" sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
        <TextField
            label="Id"
            name="id"
            value={formData.sku}
            error={!!errors.sku}
            helperText={errors.sku}
            type="hidden"
        />
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
            value={formData.categoryId || ''}
            onChange={(e) => setFormData((prev) => ({ ...prev, categoryId: Number(e.target.value) }))}
          >
            <MenuItem value="">Select Category</MenuItem>
            {(categories?.items as ICategoryModel[] || []).map((category) => (
              <MenuItem key={category.id} value={category.id}>
                {category.name}
              </MenuItem>
            ))}
          </Select>
          {errors.categoryId && <Typography color="error">{errors.categoryId}</Typography>}
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
          label="Market Price"
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
          Upload New Images
          <input
            type="file"
            hidden
            multiple
            onChange={handleFileChange}
            accept="image/*"
          />
        </Button>
        {formData.newImages && formData.newImages.length > 0 && (
          <Typography>{formData.newImages.length} new image(s) selected</Typography>
        )}
        {formData.oldImages && formData.oldImages.length > 0 && (
          <Box>
            <Typography>Existing Images:</Typography>
            {formData.oldImages.map((url, index) => (
                <img
                    key={index}
                    srcSet={`${url}?w=164&h=164&fit=crop&auto=format&dpr=2 2x`}
                    src={`${url}?w=164&h=164&fit=crop&auto=format`}
                    alt={formData.name}
                    loading="lazy"
                />
            ))}
          </Box>
        )}
        <Button
          variant="contained"
          color="primary"
          onClick={handleSubmit}
          disabled={updateProductMutation.isPending}
        >
          {updateProductMutation.isPending ? 'Updating...' : 'Update Product'}
        </Button>
      </Box>
    </Box>
  );
};

export default withAuth(EditProductPage);