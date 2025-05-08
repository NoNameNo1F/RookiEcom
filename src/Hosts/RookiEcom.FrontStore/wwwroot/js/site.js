function showToast(message, type = 'info', duration = 3000) {
    let container = document.getElementById('toast-container-main');
    if (!container) {
        container = document.createElement('div');
        container.id = 'toast-container-main';
        container.className = 'toast-container';
        document.body.appendChild(container);
    }

    const toast = document.createElement('div');
    toast.className = `toast ${type}`;
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');

    // Add close button
    const closeButton = document.createElement('button');
    closeButton.type = 'button';
    closeButton.className = 'toast-close-button';
    closeButton.innerHTML = '×';
    closeButton.onclick = function() {
        toast.classList.remove('show');
        setTimeout(() => { if(toast.parentNode) toast.parentNode.removeChild(toast); }, 600);
    };

    const toastBody = document.createElement('div');
    toastBody.className = 'toast-body';
    toastBody.innerText = message;

    toast.appendChild(closeButton);
    toast.appendChild(toastBody);

    container.appendChild(toast);

    toast.offsetHeight;

    toast.classList.add('show');

    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => { if(toast.parentNode) toast.parentNode.removeChild(toast); }, 600);
    }, duration);
}

function attachAddToCartHandlers() {
    document.querySelectorAll('form.add-to-cart-form').forEach(form => {
        form.addEventListener('submit', function (event) {
            event.preventDefault();

            const productIdInput = form.querySelector('input[name="productId"]');
            const quantityInput = form.querySelector('input[name="quantity"]');
            const tokenInput = form.querySelector('input[name="__RequestVerificationToken"]');

            if (!productIdInput || !tokenInput) {
                console.error('Add to cart form is missing required inputs.');
                showToast('Error adding item.', 'error');
                return;
            }

            const productId = productIdInput.value;
            const quantity = quantityInput ? quantityInput.value : 1;
            const token = tokenInput.value;

            const button = form.querySelector('button[type="submit"]');
            const originalButtonText = button.innerHTML;
            button.disabled = true;
            button.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Adding...';


            fetch('/Cart/AddItem', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'RequestVerificationToken': token
                },
                body: new URLSearchParams({
                    'productId': productId,
                    'quantity': quantity
                })
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        showToast(data.message, 'success');
                        updateHeaderCartCount();
                    } else {
                        showToast(data.message || 'Error adding item to cart.', 'error');
                    }
                })
                .catch(error => {
                    console.error('Error adding item to cart:', error);
                    showToast('An error occurred. Please try again.', 'error');
                })
                .finally(() => {
                    button.disabled = false;
                    button.innerHTML = originalButtonText;
                });
        });
    });
}

function attachCartQuantityHandlers() {
    const cartTableBody = document.querySelector('.cart-table tbody');
    if (cartTableBody) {
        console.log("Attaching cart quantity listeners to tbody...");

        cartTableBody.addEventListener('click', function(event) {
            const button = event.target.closest('.quantity-decrease, .quantity-increase');
            if (!button || button.disabled) return;

            const itemId = button.dataset.itemid;
            const inputField = button.closest('.input-group').querySelector('.quantity-input');
            const currentQuantity = parseInt(inputField.value);
            const newQuantity = button.classList.contains('quantity-decrease') ? currentQuantity - 1 : currentQuantity + 1;

            console.log(`Button Click: ItemId=${itemId}, CurrentQ=${currentQuantity}, NewQ=${newQuantity}`); // Debug log

            if (newQuantity >= 0 && !isNaN(newQuantity)) {
                updateCartItemQuantity(itemId, newQuantity, button);
            }
        });

        cartTableBody.addEventListener('change', function(event) {
            const inputField = event.target.closest('.quantity-input');
            if (!inputField) return;

            const itemId = inputField.dataset.itemid;
            const newQuantity = parseInt(inputField.value);

            console.log(`Input Change: ItemId=${itemId}, NewQ=${newQuantity}`);

            if (!isNaN(newQuantity) && newQuantity >= 0) {
                updateCartItemQuantity(itemId, newQuantity, inputField); 
            } else {
                console.warn("Invalid quantity entered.");
            }
        });
    } else {
        if (document.querySelector('.cart-table')) {
            console.warn("Cart table body (.cart-table tbody) not found for attaching event listeners.");
        }
    }
}

function updateCartItemQuantity(cartItemId, quantity, element) {
    console.log(`Updating ItemId=${cartItemId} to Quantity=${quantity}`);

    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');

    if (!tokenInput) {
        console.error("Anti forgery token not found for cart update.");
        showToast('Error updating cart (token missing).', 'error');
        return;
    }
    const token = tokenInput.value;

    const inputGroup = element.closest('.input-group');
    let originalValues = {};
    if(inputGroup) {
        inputGroup.querySelectorAll('button, input').forEach(el => {
            originalValues[el.name || el.classList.item(1)] = el.value;
            el.disabled = true;
        });
    }

    fetch(`/Cart/UpdateQuantity`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'RequestVerificationToken': token
        },
        body: new URLSearchParams({
            'cartItemId': cartItemId,
            'quantity': quantity
        })
    })
        .then(response => {
            if (response.ok && response.redirected) {
                console.log("Update successful, redirecting...");
                window.location.href = response.url;
                return { redirected: true };
            } else if (response.ok) {
                console.warn("Update quantity succeeded but didn't redirect.");
                
                location.reload();
                return { redirected: false };
            }
            else {
                console.error(`Update quantity failed with status: ${response.status}`);
                return response.json().then(data => {
                    showToast(data.message || `Failed to update quantity. Error ${response.status}`, 'error');
                    return { error: true };
                }).catch(() => {
                    showToast(`Failed to update quantity. Error ${response.status}`, 'error');
                    return { error: true };
                });
            }
        })
        .catch(error => {
            console.error('Network error updating cart quantity:', error);
            showToast('A network error occurred.', 'error');
            return { error: true };
        })
        .finally((result) => {
            if (result && result.error && inputGroup) {
                inputGroup.querySelectorAll('button, input').forEach(el => {
                    el.disabled = false;
                });
            }
        });
}

function updateHeaderCartCount() {
    const cartBadge = document.getElementById('cart-item-count-badge');
    if (!cartBadge) {
        console.warn("Cart badge element not found in header.");
        return;
    }

    fetch('/Cart/GetCartSummaryJson')
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const contentType = response.headers.get("content-type");
            if (contentType && contentType.indexOf("application/json") !== -1) {
                return response.json();
            } else {
                throw new Error('Received non-JSON response for cart');
            }
        })
        .then(data => {
            let itemCount = 0;
            if (data && data.success) {
                itemCount = data.count || 0;
            }
            else {
                console.warn("Cart data not found or in unexpected format:", data);
                itemCount = 0;
            }

            cartBadge.textContent = itemCount;
            cartBadge.style.display = 'inline-block';
        })
        .catch(error => {
            console.error('Error fetching cart for header count:', error);
            cartBadge.textContent = '?';
            cartBadge.style.display = 'inline-block';
        });
}

document.addEventListener('DOMContentLoaded', function() {
    attachAddToCartHandlers();
    attachCartQuantityHandlers();
    updateHeaderCartCount();
})