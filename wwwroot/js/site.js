document.addEventListener('submit', e => {
    const form = e.target;
    if (form.id == "auth-modal-form") {
        e.preventDefault();
        const login = form.querySelector('[name="AuthLogin"]').value;
        const password = form.querySelector('[name="AuthPassword"]').value;

        const credentials = btoa(login + ':' + password);
        fetch("/User/Signin", {
            method: 'GET',
            headers: {
                'Authorization': 'Basic ' + credentials
            }
        }).then(r => r.json())
            .then(j => {
                if (j.status == 200) {
                    window.location.reload();
                }
                else {
                    console.log(j.message);
                }
            });
        // console.log("Submit stopped");
    }
    if (form.id == "admin-category-form") {
        e.preventDefault();
        fetch("/Admin/AddCategory", {
            method: 'POST',
            body: new FormData(form)
        }).then(r => r.json())
            .then(j => {                
                console.log(j);
            });
    }
    if (form.id == "admin-product-form") {
        e.preventDefault();
        fetch("/Admin/AddProduct", {
            method: 'POST',
            body: new FormData(form)
        }).then(r => r.json())
            .then(j => {                
                console.log(j);
            });
    }
});

document.addEventListener('DOMContentLoaded', e => {
    for (let fab of document.querySelectorAll('[data-cart-product-id]')) {
        fab.addEventListener('click', addToCartClick);
    }
    for (let btn of document.querySelectorAll('[data-cart-decrement]')) {
        btn.addEventListener('click', decCartClick);
    }
    for (let btn of document.querySelectorAll('[data-cart-increment]')) {
        btn.addEventListener('click', incCartClick);
    }
    for (let btn of document.querySelectorAll('[data-cart-delete]')) {
        btn.addEventListener('click', deleteCartClick);
    }
});

function incCartClick(e) {
    const cartId = e.target.closest('[data-cart-increment]').getAttribute('data-cart-increment');
    console.log("++", cartId);
    modifyCartItem(cartId, 1);
}

function decCartClick(e) {
    const cartId = e.target.closest('[data-cart-decrement]').getAttribute('data-cart-decrement');
    console.log("--", cartId);
    modifyCartItem(cartId, -1);
}

function deleteCartClick(e) {
    const cartId = e.target.closest('[data-cart-delete]').getAttribute('data-cart-delete');
    const q = e.target.closest('.cart-item-row').querySelector('[data-cart-quantity]').innerText;
    console.log("xx", cartId);
    modifyCartItem(cartId, -q);
}

function modifyCartItem(cartId, delta) {
    fetch(`/Shop/ModifyCartItem?cartId=${cartId}&delta=${delta}`, {
        method: 'PUT'
    }).then(r => r.json())
        .then(j => {
            if (j.status == 200) {
                window.location.reload();
            }
            else if (j.status == 422) {
                alert("Недостатня кількість на складі");
            }
            else {
                console.log(j.message);
                alert("Помилка, повторіть пізніше");
            }
        });
}
/* Д.З. Додати посилання на крамницю до заголовкової частини сайту (_Layout)
Забезпечити правильний підрахунок кількості товарів у кошику.
Передбачити, що кошик може бути порожнім, у такому разі не виводити
  "Разом" та відповідні суми, додати кнопку "До крамниці"
На сторінці кошику додати "Вас також може зацікавити" і вивести міні-картки
  товарів з тих категорій, що є у кошику, але без тих товарів, що вже у кошику.
Додати кнопки "Придбати" та "Скасувати", які "закривають" кошик з відповідним
  статусом. У разі придбання також зменшуються складські залишки товарів.
*/

function addToCartClick(e) {
    e.stopPropagation();
    e.preventDefault();
    const elem = document.querySelector('[data-auth-ua-id]');
    if (!elem) {
        alert('Увійдіть до системи для здійснення замовлень');
        return;
    }
    const uaId = elem.getAttribute('data-auth-ua-id');
    const productId = e.target.closest('[data-cart-product-id]').getAttribute('data-cart-product-id');
    console.log(productId, uaId);
    fetch('/Shop/AddToCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: `productId=${productId}&uaId=${uaId}`
    }).then(r => r.json()).then(j => {
        if (j.status == 200) {
            alert("Додано до кошику");
        }
        else {
            alert("Помилка додавання");
        }
    });
}

/*
Д.З. Забезпечити перевірку полів логіну/паролю форми автентифікації на пустоту.
Якщо пусті - не надсилати запит, а повідомляти клієнту.
Провести тестування бекенду /User/Signin на предмет пошкоджених запитів
(немає заголовку Authorization / неправильна схема / неправильні дані)
У складі модального вікна автентифікації додати поле для виведення помилок
 і у разі відмови входу в систему показувати помилки у ньому.
*/