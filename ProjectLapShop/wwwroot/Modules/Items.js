var ClsItems = {
    GetAll: function () {
        Helper.AjaxCallGet("http://localhost:5002/api/items", {}, "json",
            function (data) {


                $('#ItemPagination').pagination({
                    dataSource: data.data,
                    pageSize: 20,
                    showGoInput: true,
                    showGoButton: true,
                    callback: function (data, pagination) {
                        // template method of yourself
                        var htmlData = "";

                        for (var i = 0; i < data.length; i++) {
                            htmlData += ClsItems.DrawItem(data[i]);
                        }

                        var d1 = document.getElementById('ItemArea');
                        d1.innerHTML = htmlData;
                    }
                });
            }, function () { });
    },

    DrawItem: function (item) {
        var data = "<div class='col-xl-3 col-6 col-grid-box'>";
        data += "<div class='product-box'><div class='img-wrapper'>";
        data += "<div class='front'> <a asp-controller='Items' asp-action='ItemsDetails' asp-route-id='" + item.id + "'><img src='/Uploads/Items/" + item.imageName + "' class='img-fluid blur-up lazyload bg-img' alt=''></a></div>";
        data += "<div class='back'> <a asp-controller='Items' asp-action='ItemsDetails' asp-route-id='" + item.id + "'><img src='/Uploads/Items/" + item.imageName + "' class='img-fluid blur-up lazyload bg-img' alt=''></a></div>";
        data += "<div class='cart-info cart-wrap'>";
        data += "<button data-toggle='modal' data-target='#addtocart' title='Add to cart'><i class='ti-shopping-cart'></i></button>";
        data += "<a href='javascript: void (0)' title='Add to Wishlist'><i class='ti-heart' aria-hidden='true'></i></a>";
        data += "<a href='#' data-toggle='modal' data-target='#quick - view' title='Quick View'><i class='ti-search' aria-hidden='true'></i></a>";
        data += "<a href='compare.html' title='Compare'><i class='ti-reload' aria-hidden='true'></i></a></div></div>";
        data += "<div class='product-detail'><div class='rating'> <i class='fa fa-star'></i> <i class='fa fa-star'></i> <i class='fa fa-star'></i>";
        data += "<i class='fa fa-star'></i> <i class='fa fa-star'></i></div>";
        data += "<a href='product-page(no - sidebar).html'><h6>" + item.itemName + "</h6></a> <p> </p>";
        data += "<h4>$" + item.salesPrice + "</h4>";
        data += "<ul class='color-variant'><li class='bg-light0'></li><li class='bg-light1'></li><li class='bg-light2'></li></ul> </div> </div> </div>";
        return data;
    }


};
