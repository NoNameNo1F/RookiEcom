import { ProductStatus } from "../enums";

export function getProductStatusText(status: ProductStatus | number): string {
    switch (status) {
        case ProductStatus.Available: return 'Available';
        case ProductStatus.Banned: return 'Banned';
        case ProductStatus.Unlisted: return 'Unlisted';
        case ProductStatus.Reviewing: return 'Reviewing';
        case ProductStatus.Removed: return 'Removed';
        default: return 'Unknown';
    }
}

// Helper to get badge color (Optional, example using MUI colors)
export function getProductStatusColor(status: ProductStatus | number): 'success' | 'error' | 'warning' | 'info' | 'default' {
     switch (status) {
        case ProductStatus.Available: return 'success';
        case ProductStatus.Banned: return 'error';
        case ProductStatus.Unlisted: return 'warning';
        case ProductStatus.Reviewing: return 'info';
        case ProductStatus.Removed: return 'default';
        default: return 'default';
    }
}