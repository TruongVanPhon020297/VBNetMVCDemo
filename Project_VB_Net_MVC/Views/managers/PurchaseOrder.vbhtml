@ModelType Tuple(Of List(Of ingredient_category), List(Of ingredient))
@Code
    ViewData("Title") = "PurchaseOrder"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <style>
        body {
            font-family: "Lato", sans-serif;
        }

        .sidebar {
            height: 100%;
            width: 200px;
            position: fixed;
            z-index: 1;
            top: 0;
            left: 0;
            background-color: #111;
            overflow-x: hidden;
            padding-top: 16px;
        }

            .sidebar a {
                padding: 6px 8px 6px 16px;
                text-decoration: none;
                font-size: 20px;
                color: #818181;
                display: block;
                margin: 5px 0;
            }

                .sidebar a:hover {
                    color: #f1f1f1;
                }

        .main {
            margin-left: 200px;
            padding: 0px 10px;
        }

        .table-wrapper {
            max-width: 100%;
            overflow-x: auto;
        }
    </style>
</head>
<body>

    <div class="sidebar">
        <a href="http://localhost:53005/Manager/HomePage"><i class="fa fa-fw fa-home"></i> Home</a>
        <a href="http://localhost:53005/Manager/PostPage"><i class="fa fa-pencil" aria-hidden="true"></i> Post</a>
        <a href="http://localhost:53005/Manager/OrderPage"><i class="fa fa-shopping-cart" aria-hidden="true"></i> Order</a>
        <a href="http://localhost:53005/Manager/CustomOrderPage"><i class="fa fa-cart-plus" aria-hidden="true"></i> Custom order</a>
        <a href="http://localhost:53005/Manager/PurchaseOrderPage"><i class="fa fa-money" aria-hidden="true"></i> Purchase Order</a>
        <a href="http://localhost:53005/Manager/ListUser"><i class="fa fa-fw fa-user"></i> Users</a>
        <a href="http://localhost:53005/Manager/ProductPage"><i class="fa fa-picture-o" aria-hidden="true"></i> Product</a>
        <a href="http://localhost:53005/Manager/Ingredient"><i class="fa fa-superpowers" aria-hidden="true"></i> Ingredient</a>
        <a href="http://localhost:53005/Manager/UserInfo"><i class="fa fa-info-circle" aria-hidden="true"></i> Info</a>
        <a href="http://localhost:53005/Users/Logout"><i class="fa fa-sign-out" aria-hidden="true"></i> Logout</a>
    </div>

    <div class="main">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-9">
                    <div class="container">
                        <div class="row" style="margin-bottom: 10px">
                            <div Class="col-xl-8">
                                <div Class="card mb-4">
                                    @Using (Html.BeginForm("CreatePurchaseOrder", "Manager"))
                                        @Html.AntiForgeryToken()
                                        @<div Class="card-body">
                                            <div Class="row gx-3 mb-3">
                                                <div Class="col-md-6">
                                                    <Label Class="small mb-1" for="inputOrgName">Ingredient</Label>
                                                    <select Class="form-control" name="ingredientId" aria-label="Default select example">
                                                        @If Model.Item2 IsNot Nothing Then
                                                            For Each item In Model.Item2
                                                                @<option value="@item.id">@item.ingredient_name</option>
                                                            Next
                                                        End If
                                                    </select>
                                                </div>
                                            </div>
                                            <div Class="row gx-3 mb-3">
                                                <div Class="col-md-6">
                                                    <Label Class="small mb-1" for="inputOrgName">Quantity</Label>
                                                    <input Class="form-control" id="inputOrgName" name="quantity" type="number" value="">
                                                </div>
                                            </div>
                                            <div Class="row gx-3 mb-3">
                                                <div Class="col-md-6">
                                                    <Button Class="btn btn-primary" type="submit" style="margin-top:20px">Add</Button>
                                                </div>
                                            </div>
                                        </div>
                                    End Using
                                </div>
                            </div>
                        </div>
                        <div Class="row">
                            <div Class="col-12 mb-3 mb-lg-5">
                                <div Class="overflow-hidden card table-nowrap table-card">
                                    <div Class="table-responsive table-wrapper">
                                        <Table Class="table mb-0" style="margin-top:10px">
                                            <thead Class="small text-uppercase bg-body text-muted">
                                                <tr>
                                                    <th> INGREDIENT NAME</th>
                                                    <th> INGREDIENT CATEGORY</th>
                                                    <th> QUANTITY</th>
                                                    <th> PRICE </th>
                                                    <th Class="text-end">Action</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                               
                                            </tbody>
                                        </Table>

                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="margin-bottom: 10px">
                            <div Class="col-xl-8">
                                <div Class="card mb-4">
                                    @Using (Html.BeginForm("CreateIngredient", "Manager"))
                                        @Html.AntiForgeryToken()
                                        @<div Class="card-body">
                                            <div Class="row gx-3 mb-3">
                                                <div Class="col-md-6">
                                                    <Label Class="small mb-1" for="inputOrgName">Total Price</Label>
                                                    <input Class="form-control" id="inputOrgName" name="ingredientName" type="tel" value="">
                                                </div>
                                            </div>
                                            <div Class="row gx-3 mb-3">
                                                <div Class="col-md-6">
                                                    <Label Class="small mb-1" for="inputOrgName">Total Quantity</Label>
                                                    <input Class="form-control" id="inputOrgName" name="ingredientName" type="number" value="">
                                                </div>
                                            </div>
                                            <div Class="row gx-3 mb-3">
                                                <div Class="col-md-6">
                                                    <Button Class="btn btn-primary" type="submit" style="margin-top:20px">Complete</Button>
                                                </div>
                                            </div>
                                        </div>
                                    End Using
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</body>
</html>