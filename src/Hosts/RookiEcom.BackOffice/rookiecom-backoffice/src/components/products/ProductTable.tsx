import { Alert, Avatar, Box, Chip, IconButton, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TablePagination, TableRow, Tooltip,  } from "@mui/material";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import ImageIcon from '@mui/icons-material/Image';
import { IProductModel } from "../../interfaces";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useGetProductsPaging } from "../../hooks";

import { MiniLoaderPage } from "../common";
import { getProductStatusColor, getProductStatusText } from "../../utils/helper";

interface ProductTableProps {
   categoryId: number;
   searchTerm: string;
   onDeleteProduct: (productId: number, productName: string) => void;
   isDeleting?: boolean;
}

const ProductTable: React.FC<ProductTableProps> = ({ categoryId, searchTerm, onDeleteProduct, isDeleting }) => {
    const navigate = useNavigate();
    const [pageNumber, setPageNumber] = useState(1);
    const [rowsPerPage, setRowsPerPage] = useState(10);
    
   const { data: pagedResult, isLoading, error } = useGetProductsPaging(pageNumber, rowsPerPage);

    const products = pagedResult?.items ?? [];
    const totalCount = pagedResult?.pageData?.totalCount ?? 0;

    const filteredProducts = products.filter(p => {
        const matchesCategory = categoryId === 0 || p.categoryId === categoryId;
        const matchesSearch = !searchTerm ||
                              p.sku.toLowerCase().includes(searchTerm.toLowerCase()) ||
                              p.name.toLowerCase().includes(searchTerm.toLowerCase());
        return matchesCategory && matchesSearch;
    });

    const handleChangePage = (_event: unknown, newPage: number) => {
       setPageNumber(newPage + 1);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
       setRowsPerPage(parseInt(event.target.value, 10));
       setPageNumber(1);
    };

    const columns = [
        { id: 'image', label: 'Image', width: 80 },
        { id: 'sku', label: 'SKU', minWidth: 100 },
        { id: 'name', label: 'Name', minWidth: 170 },
        { id: 'category', label: 'Category', minWidth: 120, hideOnMobile: true },
        { id: 'price', label: 'Price', align: 'right', width: 100 },
        { id: 'stock', label: 'Stock', align: 'right', width: 80 },
        { id: 'status', label: 'Status', align: 'center', width: 120 },
        { id: 'actions', label: 'Actions', align: 'right', width: 120 },
    ];

    
    return (
        <Box sx={{ width: '100%', overflowX: 'auto' }}>
            <TableContainer component={Paper} variant="outlined" sx={{ boxShadow: 'none' }}>
                <Table stickyHeader aria-label="products table">
                    <TableHead>
                        <TableRow>
                            {columns.map((column) => (
                                <TableCell
                                    key={column.id}
                                    align={column.align as any}
                                    style={{ minWidth: column.minWidth, width: column.width, fontWeight: 'bold' }}
                                    sx={{ ...(column.hideOnMobile && { display: { xs: 'none', md: 'table-cell' } }) }}
                                >
                                    {column.label}
                                </TableCell>
                            ))}
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {isLoading ? (
                             <TableRow> <TableCell colSpan={columns.length} sx={{ textAlign: 'center', p: 5 }}> <MiniLoaderPage text="Loading Products..." /> </TableCell> </TableRow>
                        ) : error ? (
                            <TableRow> <TableCell colSpan={columns.length} sx={{ p: 3 }}> <Alert severity="error">{(error as Error).message || "Failed to load products."}</Alert> </TableCell> </TableRow>
                        ) : filteredProducts.length === 0 ? (
                             <TableRow> <TableCell colSpan={columns.length} sx={{ textAlign: 'center', p: 5 }}> No products found. </TableCell> </TableRow>
                        ) : (
                            filteredProducts.map((product: IProductModel) => (
                                <TableRow hover key={product.id}>
                                    <TableCell>
                                         <Avatar variant="rounded" src={product.images?.[0] || undefined} alt={product.name} sx={{ width: 56, height: 56, bgcolor: 'grey.100' }} >
                                            {!product.images?.[0] && <ImageIcon color="disabled" />}
                                         </Avatar>
                                    </TableCell>
                                    <TableCell sx={{ whiteSpace: 'nowrap' }}>{product.sku}</TableCell>
                                    <TableCell>{product.name}</TableCell>
                                    <TableCell sx={{ display: { xs: 'none', md: 'table-cell' } }}>{product.categoryId || 'N/A'}</TableCell>
                                    <TableCell align="right">${product.price.toFixed(2)}</TableCell>
                                    <TableCell align="right">{product.stockQuantity}</TableCell>
                                    {/* Status Cell */}
                                    <TableCell align="center">
                                        <Chip
                                            label={getProductStatusText(product.status)}
                                            color={getProductStatusColor(product.status)}
                                            size="small"
                                            variant="outlined"
                                        />
                                    </TableCell>
                                    <TableCell align="right" sx={{ whiteSpace: 'nowrap' }}>
                                        <Tooltip title="Edit Product">
                                            <IconButton size="small" onClick={() => navigate(`/products/edit/${product.sku}`)} color="primary" disabled={isDeleting}>
                                                <EditIcon fontSize="small" />
                                            </IconButton>
                                        </Tooltip>
                                        <Tooltip title="Delete Product">
                                            <IconButton size="small" onClick={() => onDeleteProduct(product.id, product.name)} color="error" disabled={isDeleting}>
                                                <DeleteIcon fontSize="small" />
                                            </IconButton>
                                        </Tooltip>
                                    </TableCell>
                                </TableRow>
                            ))
                        )}
                    </TableBody>
                </Table>
            </TableContainer>
             {totalCount > 0 && (
                <TablePagination
                    rowsPerPageOptions={[5, 10, 25]} component="div"
                    count={totalCount}
                    rowsPerPage={rowsPerPage} page={pageNumber - 1} 
                    onPageChange={handleChangePage} onRowsPerPageChange={handleChangeRowsPerPage}
                />
            )}
        </Box>
    )
};

export default ProductTable;