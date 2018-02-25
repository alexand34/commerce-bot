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

var order = [];
var userId = getUrlParameter('userId');
var restaurantId = getUrlParameter('restaurantId');
var menu = {};

jQuery.get("api/menu?userId=" + userId + "&restaurantId=" + restaurantId, function (data) {
    menu = data.data;
    for (var key in data.data) {
        if (data.data.hasOwnProperty(key)) {
            $('#mainMenu').append('<div class="col-12 menu"><h3>' + key + '</h3>' +
                '<hr/><div class="row col-12" id=' + key + '>' +
                '</div></div>');

            data.data[key].forEach((item, index) => {
                console.log(index);

                $('#' + key).append(
                    '<div class="col-6 col-md-4 menu-item" id="' + key + '-' + index + '-' + item.Id + '">' +
                    '<p class="card-title menu-item-header">' + item.DishName + ' – C$' + item.Price + '</p>' +
                    '<p class="cart-text menu-item-description">' + item.DishDescription + '</p></div>');

                $('#' + key + '-' + index + '-' + item.Id).click(function (element) {
                    var chosenItemInfo = element.currentTarget.id.split('-');
                    if (!$('#' + element.currentTarget.id).hasClass('clicked')) {
                        $('#' + element.currentTarget.id).addClass('clicked');
                        order.push(menu[chosenItemInfo[0]][chosenItemInfo[1]]);
                        console.log(order);
                    } else {
                        $('#' + element.currentTarget.id).removeClass('clicked');
                        var index = order.indexOf(menu[chosenItemInfo[0]][chosenItemInfo[1]]);
                        order.splice(index, 1);
                        console.log(order);
                    }
                });

            });
        }
    }
});


window.onbeforeunload = function () {
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        dataType: 'json',
        url: '/api/menu',
        data: JSON.stringify({ 'order': order })
    });
    return 'Are you sure you want to leave?';
}

