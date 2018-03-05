function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

var menu = {};
var order = {};
var userId = getUrlParameter('userId');
var restaurantId = getUrlParameter('restaurantId');
$('#shop-btn').hide();
order.userId = userId;
order.restaurantId = restaurantId;
order.order = [];
jQuery.get("api/menu?userId=" + userId + "&restaurantId=" + restaurantId, function (data) {
    menu = data.data;
    $('.loader').hide();
    $('#shop-btn').show();
    for (var category in menu) {
        if (menu.hasOwnProperty(category)) {
            $('#mainMenu').append('<div class="col-12 menu"><h3>' + category + '</h3>' +
                '<hr/><div class="row col-12" id=' + category + '>' +
                '</div></div>');

            menu[category].forEach((item) => {
                item.Count = 0;
                $('#' + category).append(
                    '<div class="col-6 col-md-4 menu-item" >' +
                    '<p class="card-title menu-item-header">' + item.DishName + ' – C$' + item.Price + '</p>' +
                    '<p class="cart-text menu-item-description">' + item.DishDescription + '</p>' +
                    '<p>Count: <span id="count-' + item.Id + '-' + item.FoodCategoryId + '">' + item.Count + '</span>' +
                    '<div><button id="plus-' + item.Id + '-' + item.FoodCategoryId + '" class="btn btn-default float-right" style="margin-bottom: 10px;">' +
                    '<i class="fa fa-plus"></i>' +
                    '</button></div></p></div>' +
                    '</div>');
                $('#plus-' + item.Id + '-' + item.FoodCategoryId).click(function (element) {
                    item.Count++;
                    $('#count-' + item.Id + '-' + item.FoodCategoryId).html(item.Count);
                    getMinusButton('#plus-' + item.Id + '-' + item.FoodCategoryId, item);
                });
            });
        }
    }
});


function getMinusButton(id, item) {
    var minusBtnId = id.replace("plus", "minus");
    if (item.Count === 0) {
        $(minusBtnId).unbind();
        $(minusBtnId).remove();
    } else {
        $(id.replace("plus", "count")).html(item.Count);
        if (item.Count === 0) {
        } else if (item.Count === 1) {
            if (!$(minusBtnId).length) {
                $(id).after('<button id="' + minusBtnId.replace("#", "") +
                    '" class="btn btn-default float - right" style="margin- bottom: 10px;">' +
                    '<i class="fa fa-minus"></i>' +
                    '</button>');
            }

            $(minusBtnId).click((element) => {
                if (item.Count > 0) {
                    item.Count--;
                    console.log(item);
                    $(id.replace("plus", "count")).html(item.Count);
                    getMinusButton(id, item);
                }
            });
        }
    }
}

function getOrderItems() {
    var orderSummary = {};
    orderSummary.OrderString = "";
    orderSummary.TotalPrice = 0.0;
    for (var category in menu) {
        if (menu.hasOwnProperty(category)) {
            menu[category].forEach((item) => {
                if (item.Count !== 0) {
                    let orderItem = { FoodId: item.Id, Count: item.Count };
                    orderSummary.TotalPrice += item.Count * item.Price;
                    order.order.push(orderItem);
                    orderSummary.OrderString += '<div>• ' + item.DishName + '(' + item.Count + ')       – C$' + (item.Count * item.Price) + '</span>' +
                        '</div>';
                }
            });
        }
    }
    return orderSummary;
}

$('#shop-btn').click((element) => {
    $('#shopModal').show();
    var orderSummary = getOrderItems();
    $('.modal-body').html('<div class="col-12">' + orderSummary.OrderString + '</div>' +
        '<hr/><div class="col-12"><span style="font-weight: bold">Total:</span> C$' + orderSummary.TotalPrice+'</div>');
});

$('#confirmOrder').click((element) => {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        dataType: 'json',
        url: '/api/menu',
        data: JSON.stringify({ 'order': order })
    });
});
window.onbeforeunload = function () {
    console.log(order);
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        dataType: 'json',
        url: '/api/menu',
        data: JSON.stringify({ 'order': order })
    });
    return 'Are you sure you want to leave?';
}

