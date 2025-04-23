import { useState } from "react";
import { Box, Button, FormControl, InputLabel, MenuItem, Select, TextField, Typography } from "@mui/material";
import { ICategoryModel } from "../../interfaces";
import { useNavigate } from "react-router-dom";
import withAuth from "../../oidc/withAuth";
import { useGetCategories } from "../../hooks";
import {ProductTable} from "../../components/products";

const ProductListPage = () => {
    const navigate = useNavigate();
    const [categoryId, setCategoryId] = useState<number | "">("");
    const { data: categoriesResult} = useGetCategories(1, 100);

    const categories = categoriesResult?.items ?? [];

    return (
        <Box sx={{ maxWidth: 1200, mx: 'auto', py: 2 }}>
            <Typography variant="h4" gutterBottom>
                Products
            </Typography>
            <Box sx={{ mb: 2, display: 'flex', gap: 2, flexWrap: 'wrap' }}>
                <Button variant="contained" onClick={() => navigate('/products/create')}>
                Create Product
                </Button>
                <FormControl sx={{ minWidth: 200 }}>
                <InputLabel>Category</InputLabel>
                <Select
                    value={categoryId}
                    onChange={(e) => setCategoryId(e.target.value as number | '')}
                    label="Category"
                >
                    <MenuItem value="">All Categories</MenuItem>
                    {categories.map((cat: ICategoryModel) => (
                    <MenuItem key={cat.id} value={cat.id}>{cat.name}</MenuItem>
                    ))}
                </Select>
                </FormControl>
                <TextField label="Search by SKU or Name" variant="outlined" />
            </Box>
            <ProductTable categoryId={categoryId} />
        </Box>
    );
};

export default withAuth(ProductListPage);