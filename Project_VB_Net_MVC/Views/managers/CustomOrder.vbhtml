@ModelType List(Of custom_order)

@Code
    ViewData("Title") = "CustomOrder"
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
        <section Class="intro">
            <div Class="bg-image h-100" style="background-color: #6095F0;">
                <div Class="mask d-flex align-items-center h-100">
                    <div Class="container">
                        <div Class="row justify-content-center">
                            <div Class="col-12">
                                <div Class="card shadow-2-strong" style="background-color: #f5f7fa;">
                                    <div Class="card-body">
                                        <div Class="table-responsive">
                                            @If Model IsNot Nothing Then
                                                @<Table Class="table table-borderless mb-0">
                                                    <thead>
                                                        <tr>
                                                            <th scope="col">
                                                                STATUS
                                                            </th>
                                                            <th scope="col"> order Date</th>
                                                            <th scope="col"> SIZE DESCRIPTION</th>
                                                            <th scope="col"> QUANTITY</th>
                                                            <th scope="col"> delivery Date</th>
                                                            <th scope="col"> DETAILS</th>
                                                            <th scope="col"> CANCEL ORDER</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        @For Each detail In Model
                                                            @<tr>
                                                                <th scope="row">
                                                                    <div class="form-check">
                                                                        @If detail.status = True Then
                                                                            @<input Class="form-check-input" type="checkbox" disabled value="" id="flexCheckDefault1" checked />
                                                                        Else
                                                                            @<input Class="form-check-input" type="checkbox" disabled value="" id="flexCheckDefault1" />
                                                                        End IF

                                                                    </div>
                                                                </th>
                                                                <td>@detail.register_time</td>
                                                                <td>@detail.size_description</td>
                                                                <td>@detail.quantity</td>
                                                                <td>@detail.delivery_date</td>
                                                                <td>
                                                                    @Html.ActionLink("GO TO", "CustomOrderDetailPage", New With {.id = detail.id})
                                                                </td>
                                                                <td>
                                                                    @If detail.status = False Then
                                                                        @Using (Html.BeginForm("CancelOrder", "users"))
                                                                            @Html.AntiForgeryToken()
                                                                            @Html.TextBox("orderId", detail.id, htmlAttributes:=New With {.class = "form-control mr-sm-2", .style = "display: none;"})
                                                                            @<input type="submit" value="×" class="btn-danger" />
                                                                        End Using
                                                                    End If
                                                                </td>
                                                            </tr>
                                                        Next
                                                    </tbody>
                                                </Table>
                                            End If
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</body>
</html>

