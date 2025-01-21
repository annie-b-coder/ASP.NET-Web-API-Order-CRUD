const uri = 'api/orders';
let orders_ = [];
let orderitems_ = [];
let providers_ = [];

//добавить пустую строку для добавления элемента заказа в форме редактирования
function addEditItemDynamic() {
    const itemForm = document.getElementById('item-edit-form');
    itemForm.style.display = "block"
    const item = document.createElement('div');
    item.className = 'order-item';
    item.innerHTML = `
                <input type="text" placeholder="Name" class="item-name" required>
                <input type="text" placeholder="Quantity" class="item-quantity" required>
                <input type="text" placeholder="Unit" class="item-unit" required>
                <input hidden type="text" placeholder="Id" class="item-id" >
                <button class="remove-btn">Удалить</button>
            `;
    itemForm.appendChild(item);

    item.querySelector('.remove-btn').addEventListener('click', () => {
        itemForm.removeChild(item);
    });
}
//добавить пустую строку для добавления элемента заказа в форме создания
function addNewItemDynamic() {
    const itemForm = document.getElementById('item-add-form');
    itemForm.style.display = "block"
    const item = document.createElement('div');
    item.className = 'order-item';
    item.innerHTML = `
                <input type="text" placeholder="Name" class="item-add-name" required>
                <input type="text" placeholder="Quantity" class="item-add-quantity" required>
                <input type="text" placeholder="Unit" class="item-add-unit" required>
                <button class="remove-btn">Delete</button>
            `;
    itemForm.appendChild(item);

    item.querySelector('.remove-btn').addEventListener('click', () => {
        itemForm.removeChild(item);
    });
}
//получить все заказы
function getOrders() {
    fetch(uri + '/getorders')
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}
//получить список всех поставщиков в форме добавления нового заказа
function getProvidersAdd() {
    fetch(uri + '/getproviders')
        .then(response => response.json())
        .then(data => {
            providers_ = data;

            const select = document.getElementById("provider");
            select.innerHTML = '';

            providers_.forEach(item => {
                const option = document.createElement("option");
                option.value = item.id; 
                option.textContent = item.name; 
                select.appendChild(option);
            });
        })
        .catch(error => console.error('Unable to get providers.', error));
}
//получить элементы заказа
function getOrderItemsByOrderId(orderid) {

    fetch(uri + '/getorderitemsbyorderid?orderid='+ encodeURIComponent(orderid))
        .then(response => response.json())
        .then(data => _displayOrderItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

//открытие формы добавления нового заказа
function addOrder() {
    const addOrderDiv = document.getElementById('add-order');
    addOrderDiv.style.display = "block"
    getProvidersAdd();
}
//сохранение нового заказа
function addNewOrder() {
    let items = [];
    const addNumber = document.getElementById('order-number');
    const names = document.querySelectorAll('.item-add-name');
    const quantis = document.querySelectorAll('.item-add-quantity');
    const units = document.querySelectorAll('.item-add-unit'); 

    names.forEach((nameInput, index) => {
        if (nameInput.value && names[index].value) {
        
            items.push({
                name: nameInput.value,
                quantity: quantis[index].value,
                unit: units[index].value
            });
        }
    });

    const item = {        
        number: addNumber.value.trim(),
        providerId: provider.value,
        orderItems: items
    };
    fetch(uri+'/addorder', {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => {
            location.reload()
        })
        .catch(error => console.error('Unable to add order.', error));
}

//удалить заказ
function deleteItem(id) {
    fetch(`${uri}/deleteorder/${id}`, {
        method: 'DELETE'
    })
        .then(() => location.reload())
        .catch(error => console.error('Unable to delete order.', error));
}
//удалить элемент заказа
function deleteOrderItem(id) {
    fetch(`${uri}/deleteorderitem/${id}`, {
        method: 'DELETE'
    })
        .then(() => location.reload())
        .catch(error => console.error('Unable to delete orderItem.', error));
}
//получить детали заказа
function displayDetails(id) {
    const detailOrderDiv = document.getElementById('order-detail');
    detailOrderDiv.style.display = "block"
    getOrderItemsByOrderId(id);
}

//редактировать заказ
function updateOrder(id) {
  
    let items = [];
    const addNumber = document.getElementById('order-number');
    const names = document.querySelectorAll('.item-name');
    const quantis = document.querySelectorAll('.item-quantity');
    const units = document.querySelectorAll('.item-unit');
    const ids = document.querySelectorAll('.item-id');

    names.forEach((nameInput, index) => {
        if (nameInput.value && names[index].value) {
          
            items.push({
                name: nameInput.value,
                quantity: quantis[index].value,
                unit: units[index].value,
                id: (parseFloat(ids[index].value)) ? ids[index].value : 0
            });
        }
    });

    const item = {
        id: id,
        number: addNumber.value.trim(),
        providerId: parseFloat(provider.value) ? provider.value : 0,
        orderItems: items
    };
    console.log(JSON.stringify(item));
    fetch(`${uri}/updateorder/${id}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => location.reload())
        .catch(error => console.error('Unable to update item.', error));

    return false;
}

//показать список заказов
function _displayItems(data) {
    console.log(data);
    const tBody = document.getElementById('orders');
    tBody.innerHTML = '';
    const button = document.createElement('button');

    data.forEach(item => {
        let tr = tBody.insertRow();
        tr.setAttribute('onclick', `displayDetails(${item.id})`);
        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(item.number);
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(item.date);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(item.providerName);
        td3.appendChild(textNode3);
    });
    orders_ = data;
}

//показать детали заказа
function _displayOrderItems(data) {
    console.log(data);
    const tBody = document.getElementById('orderitems');
    tBody.innerHTML = '';
    let orderNum = document.getElementById('order-num');
    orderNum.innerText = data[0].orderNumber;
    const button = document.createElement('button');
    let deleteOrderButton = document.getElementById('deleteOrder');
    deleteOrderButton.innerText = 'DeleteOrder';
    deleteOrderButton.setAttribute('onclick', `deleteItem(${data[0].orderId})`);
    let saveOrderButton = document.getElementById('updateOrder');
    saveOrderButton.innerText = 'UpdateOrder';
    saveOrderButton.setAttribute('onclick', `updateOrder(${data[0].orderId})`);

    data.forEach(item => {

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteOrderItem(${item.id})`);
        let tr = tBody.insertRow();
        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(item.name);
        td1.appendChild(textNode1);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(item.quantity);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        let textNode3 = document.createTextNode(item.unit);
        td3.appendChild(textNode3);

        let td5 = tr.insertCell(3);
        td5.appendChild(deleteButton);
    });
}