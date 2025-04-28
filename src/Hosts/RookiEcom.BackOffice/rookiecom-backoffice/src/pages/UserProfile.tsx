// src/pages/Profile/UserProfilePage.tsx
import React from 'react';
import {
    Paper, Box, Typography, Avatar, Grid, CircularProgress, Alert, Divider, Stack
} from '@mui/material';
import PersonIcon from '@mui/icons-material/Person';
import LocationOnIcon from '@mui/icons-material/LocationOn';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import CakeIcon from '@mui/icons-material/Cake';
import { useGetProfile } from '../hooks';
import { useAuth } from 'react-oidc-context';
import withAuth from '../oidc/withAuth';
import { format } from 'date-fns';
import { IUserModel } from '../interfaces';

const UserProfilePage: React.FC = () => {
    const { user, isLoading: authLoading } = useAuth();
    const loggedInUserId = user?.profile?.sub!;

    const { data: profile, isLoading: profileLoading, error } = useGetProfile(loggedInUserId!);
    console.log(profile);
    const isLoading = authLoading || profileLoading;

    const formatDate = (dateString: string | undefined) => {
        if (!dateString) return 'N/A';
        try {
            return format(new Date(dateString), 'PP');
        } catch {
            return 'Invalid Date';
        }
    };

    const renderAddress = (address: IUserModel['address']) => {
        if (!address) return 'N/A';
        const parts = [address.street, address.ward, address.district, address.city].filter(Boolean);
        return parts.join(', ');
    }

    return (
        <Paper sx={{ p: { xs: 2, sm: 3 }, margin: { xs: 1, sm: 3 }, maxWidth: 900, mx: 'auto' }}>
            <Typography variant="h4" component="h1" gutterBottom>
                My Profile
            </Typography>

            {isLoading && (
                <Box sx={{ display: 'flex', justifyContent: 'center', p: 5 }}>
                    <CircularProgress />
                </Box>
            )}

            {error && !isLoading && (
                 <Alert severity="error" sx={{ mb: 2 }}>
                    Failed to load profile: {(error as Error).message}
                 </Alert>
            )}

            {!isLoading && !error && profile && (
                <Grid container spacing={3}>
                    <Grid size={{ xs: 12, md: 4 }}
                        sx={{ textAlign: 'center' }}>
                        <Avatar
                            src={profile.avatar || undefined}
                            sx={{ width: 150, height: 150, margin: '0 auto 16px auto', fontSize: '4rem', bgcolor: 'primary.main' }}
                        >
                             {!profile.avatar && <PersonIcon fontSize="inherit" />}
                        </Avatar>
                        <Typography variant="h5">{`${profile.firstName} ${profile.lastName}`}</Typography>
                        <Typography color="text.secondary">@{profile.userName}</Typography>
                    </Grid>

                    <Grid size={{ xs: 12, md: 8 }}>
                        <Typography variant="h6" gutterBottom>Account Details</Typography>
                        <Divider sx={{ mb: 2 }} />
                        <Stack spacing={1.5}>
                             <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                                 <EmailIcon color="action" />
                                 <Typography><strong>Email:</strong> {profile.email}</Typography>
                             </Box>
                             <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                                 <PhoneIcon color="action" />
                                 <Typography><strong>Phone:</strong> {profile.phoneNumber || 'N/A'}</Typography>
                             </Box>
                              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                                 <CakeIcon color="action" />
                                 <Typography><strong>Date of Birth:</strong> {formatDate(profile.doB)}</Typography>
                             </Box>

                             <Divider sx={{ my: 1 }} />

                             <Typography variant="h6" gutterBottom sx={{mt: 2}}>Address</Typography>
                              <Box sx={{ display: 'flex', alignItems: 'flex-start', gap: 1.5 }}>
                                 <LocationOnIcon color="action" sx={{mt: 0.5}}/>
                                 <Typography sx={{ flexGrow: 1 }}>
                                    {renderAddress(profile.address) || 'No address provided.'}
                                 </Typography>
                             </Box>
                        </Stack>
                    </Grid>
                </Grid>
            )}
             {!isLoading && !error && !profile && (
                 <Typography sx={{textAlign: 'center', mt: 3}}>Could not load profile data.</Typography>
            )}
        </Paper>
    );
};

export default withAuth(UserProfilePage);