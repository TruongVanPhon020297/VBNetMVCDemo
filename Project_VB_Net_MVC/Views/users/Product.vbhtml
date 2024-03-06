@ModelType List(Of ProductData)

@Code
    ViewData("Title") = "Product"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code



<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

</head>
<body>
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <nav class="navbar navbar-expand-lg navbar-light bg-light" style="margin:15px 0 0 0">
                    <div class="collapse navbar-collapse" id="navbarSupportedContent">
                        @Using (Html.BeginForm("Product", "users"))
                            @Html.AntiForgeryToken()
                            @<div class="form-inline my-2 my-lg-0">
                                @Html.TextBox("name", "", htmlAttributes:=New With {.class = "form-control mr-sm-2"})
                                <input type="submit" value="Search" class="btn btn-outline-success my-2 my-sm-0" />
                            </div>
                        End Using
                    </div>
                </nav>
            </div>
            <div class="navbar-collapse collapse" style="float:right">
                <ul class="nav navbar-nav">
                    <li>@Html.ActionLink("Cart Info", "CartInfo", "Users")</li>
                    <li>@Html.ActionLink("User Info", "UserInfo", "Users")</li>
                    <li>@Html.ActionLink("Order Info", "OrderInfo", "Users")</li>
                    <li>@Html.ActionLink("Favorite", "FavoriteInfo", "Users")</li>
                    <li>@Html.ActionLink("Purchased product", "PurchasedProduct", "Users")</li>
                    <li>@Html.ActionLink("Log out", "Logout", "Users")</li>
                </ul>
            </div>
        </div>
    </div>
    <div Class="container bootstrap snipets">
        <h1 Class="text-center text-muted">Product List</h1>
        <div Class="row flow-offset-1">
            @For Each item In Model
                @<div Class="col-xs-6 col-md-4" style="margin-bottom: 10px">
                    <div Class="product tumbnail thumbnail-3">
                        <a href="#"><img src="@Url.Content("~/Uploads/" & item.product.image)" width="350" height="280" alt=""></a>
                        <div Class="caption" style="text-align:center">
                            <h6>
                                @Html.ActionLink(item.product.product_name, "Detail", "Users", New With {.id = item.product.Id}, "")
                            </h6>
                            <span Class="price sale">$@item.product.price</span>
                            @Using (Html.BeginForm("AddToCart", "users"))
                                @Html.AntiForgeryToken()
                                @<div class="form-inline my-2 my-lg-0" style="margin-bottom:20px;margin-top:10px">
                                    @Html.TextBox("productId", item.product.Id, htmlAttributes:=New With {.class = "form-control mr-sm-2", .style = "display: none;"})
                                    <input type="submit" value="Add to cart" class="btn btn-info btn-outline-success my-2 my-sm-0" />
                                </div>
                            End Using
                            @If item.isFavorite
                                @<a href = "http://localhost:53005/users/Favorite/?productID=@item.product.Id" Class="btn btn-danger"><i Class="fa fa-heart-o" aria-hidden="true"></i></a>
                            Else
                                @<a href = "http://localhost:53005/users/Favorite/?productID=@item.product.Id" Class="btn btn-success"><i Class="fa fa-heart-o" aria-hidden="true"></i></a>
                            End If
                            
                        </div>
                    </div>
                </div>
            Next
        </div>
    </div>
</body>
</html>


