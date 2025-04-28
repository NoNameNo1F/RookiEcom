import React, { useState } from 'react';
import {
    Paper, Box, Typography, TableContainer, Table, TableHead, TableRow,
    TableCell, TableBody, TablePagination, Avatar, Alert
} from '@mui/material';
import PersonIcon from '@mui/icons-material/Person';
import { useGetCustomers } from '../../hooks';
import { IUserModel } from '../../interfaces';
import { MiniLoaderPage } from '../../components/common';
import withAuth from '../../oidc/withAuth';

const CustomerListPage: React.FC = () => {
    const [pageNumber, setPageNumber] = useState(1);
    const [rowsPerPage, setRowsPerPage] = useState(10);

    const { data: pagedResult, isLoading, error } = useGetCustomers(pageNumber, rowsPerPage);

    const customers = pagedResult?.items ?? [];
    const totalCount = pagedResult?.pageData?.totalCount ?? 0;

    const handleChangePage = (_event: unknown, newPage: number) => {
        setPageNumber(newPage + 1);
    };

    const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPageNumber(1);
    };

    const columns = [
        { id: 'avatar', label: ' ', width: 60 },
        { id: 'name', label: 'Name', minWidth: 170 },
        { id: 'username', label: 'Username', minWidth: 120 },
        { id: 'email', label: 'Email', minWidth: 170 },
        { id: 'phone', label: 'Phone', minWidth: 120, hideOnTablet: true },
    ];

    return (
        <Paper sx={{ p: { xs: 2, sm: 3 }, margin: { xs: 1, sm: 3 } }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
                <Typography variant="h4" component="h1">Customers</Typography>
            </Box>

            <Box sx={{ width: '100%', overflowX: 'auto' }}>
                <TableContainer component={Paper} variant="outlined" sx={{ boxShadow: 'none' }}>
                    <Table stickyHeader aria-label="customer list table">
                        <TableHead>
                            <TableRow>
                                {columns.map((column) => (
                                    <TableCell
                                        key={column.id}
                                        style={{ width: column.width, minWidth: column.minWidth, fontWeight: 'bold' }}
                                        sx={{ ...(column.hideOnTablet && { display: { xs: 'none', md: 'table-cell' } }) }}
                                    >
                                        {column.label}
                                    </TableCell>
                                ))}
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {isLoading ? (
                                <TableRow> <TableCell colSpan={columns.length} sx={{ textAlign: 'center', p: 5 }}> <MiniLoaderPage text="Loading Customers..." /> </TableCell> </TableRow>
                            ) : error ? (
                                <TableRow> <TableCell colSpan={columns.length} sx={{ p: 3 }}> <Alert severity="error">{(error as Error).message || "Failed to load customers."}</Alert> </TableCell> </TableRow>
                            ) : customers.length === 0 ? (
                                <TableRow> <TableCell colSpan={columns.length} sx={{ textAlign: 'center', p: 5 }}> No customers found. </TableCell> </TableRow>
                            ) : (
                                customers.map((customer: IUserModel) => (
                                    <TableRow hover key={customer.id}>
                                        <TableCell>
                                            <Avatar src={customer.avatar || undefined} sx={{ width: 40, height: 40, bgcolor: 'grey.300' }}>
                                                {!customer.avatar && <PersonIcon fontSize="small" />}
                                            </Avatar>
                                        </TableCell>
                                        <TableCell sx={{ whiteSpace: 'nowrap' }}>{`${customer.firstName} ${customer.lastName}`}</TableCell>
                                        <TableCell>{customer.userName}</TableCell>
                                        <TableCell>{customer.email}</TableCell>
                                        <TableCell sx={{ display: { xs: 'none', md: 'table-cell' } }}>{customer.phoneNumber || 'N/A'}</TableCell>
                                    </TableRow>
                                ))
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>
                {totalCount > 0 && (
                    <TablePagination
                        rowsPerPageOptions={[5, 10, 25]} component="div"
                        count={totalCount} rowsPerPage={rowsPerPage} page={pageNumber - 1}
                        onPageChange={handleChangePage} onRowsPerPageChange={handleChangeRowsPerPage}
                    />
                )}
            </Box>
        </Paper>
    );
};

export default withAuth(CustomerListPage);