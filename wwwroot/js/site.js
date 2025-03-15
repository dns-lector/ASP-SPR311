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
/*
Д.З. Забезпечити перевірку полів логіну/паролю форми автентифікації на пустоту.
Якщо пусті - не надсилати запит, а повідомляти клієнту.
Провести тестування бекенду /User/Signin на предмет пошкоджених запитів
(немає заголовку Authorization / неправильна схема / неправильні дані)
У складі модального вікна автентифікації додати поле для виведення помилок
 і у разі відмови входу в систему показувати помилки у ньому.
*/