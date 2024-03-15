@ModelType Tuple(Of List(Of ProductData), List(Of custom_order_notification), List(Of category))

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
    <style>
        .circle-button {
            width: 20px;
            height: 20px;
            background-color: #007bff;
            border-radius: 50%;
            display: flex;
            justify-content: center;
            align-items: center;
            position: absolute;
            top: 0px;
            right: 0px;
        }

            .circle-button span {
                color: white;
                font-weight: bold;
            }
    </style>
</head>
<body>
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container-fluid">
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
                    <li>@Html.ActionLink("Post", "PostPage", "Users")</li>
                    <li>@Html.ActionLink("Cart Info", "CartInfo", "Users")</li>
                    <li>@Html.ActionLink("Custom Order", "CustomOrder", "Users")</li>
                    <li>@Html.ActionLink("User Info", "UserInfo", "Users")</li>
                    <li>@Html.ActionLink("Order Info", "OrderInfo", "Users")</li>
                    <li>@Html.ActionLink("Favorite", "FavoriteInfo", "Users")</li>
                    <li>@Html.ActionLink("Purchased product", "PurchasedProduct", "Users")</li>
                    <li>
                        @If Model.Item2.Count > 0 Then
                            @<a href="http://localhost:53005/Users/NotificationInfo/" style="font-size:25px"><i Class="fa fa-bell" aria-hidden="true"></i></a>
                            @<div Class="circle-button">
                                <span>@Model.Item2.Count</span>
                            </div>
                        Else
                            @<a href="http://localhost:53005/Users/NotificationInfo/" style="font-size:25px"><i Class="fa fa-bell" aria-hidden="true"></i></a>
                        End If
                    </li>
                    <li>@Html.ActionLink("Log out", "Logout", "Users")</li>
                </ul>
            </div>
        </div>
    </div>
    <div class="container mt-5">
        <h2 class="mb-4">Product Filter</h2>
        @Using (Html.BeginForm("FilterProduct", "users"))
            @Html.AntiForgeryToken()
            @<div class="form-row">
                <div class="form-group col-md-4">
                    <label for="productName">Product Name</label>
                    <input type="text" class="form-control" id="productName" name="productName" placeholder="Enter product name">
                </div>
                <div class="form-group col-md-4">
                    <label for="productType">Categories</label>
                    <select id="productType" name="category" class="form-control">
                        <option value="">All</option>
                        @For Each item In Model.Item3
                            @<option value="@item.id">@item.category_name</option>
                        Next
                    </select>
                </div>
                <div class="form-group col-md-2">
                    <label for="minPrice">Min Price</label>
                    <input type="number" class="form-control" name="minPrice" value="0" id="minPrice" placeholder="Min price">
                </div>
                <div class="form-group col-md-2">
                    <label for="maxPrice">Max Price</label>
                    <input type="number" class="form-control" name="maxPrice" value="10000" id="maxPrice" placeholder="Max price">
                </div>
            </div>
            @<button type="submit" Class="btn btn-primary">Filter</button>
        End Using
    </div>
    <div Class="container bootstrap snipets">
        <h1 Class="text-center text-muted">Product List</h1>
        <div Class="row flow-offset-1">
            @If Model IsNot Nothing Then
                @For Each item In Model.Item1
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
                                @If item.isFavorite Then
                                    @<a href="http://localhost:53005/users/Favorite/?productID=@item.product.Id" Class="btn btn-danger"><i Class="fa fa-heart-o" aria-hidden="true"></i></a>
                                Else
                                    @<a href="http://localhost:53005/users/Favorite/?productID=@item.product.Id" Class="btn btn-success"><i Class="fa fa-heart-o" aria-hidden="true"></i></a>
                                End If
                            </div>
                        </div>
                    </div>
                Next
            End If
        </div>
    </div>
</body>
</html>


