@ModelType List(Of UserData)

@Code
    ViewData("Title") = "User"
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
            margin-left: 200px; /* Độ rộng của sidebar */
            padding: 0px 10px;
        }

        /* Đảm bảo bảng không tràn ra ngoài màn hình */
        .table-wrapper {
            max-width: 100%;
            overflow-x: auto;
        }
    </style>
</head>
<body>

    <div class="sidebar">
        <a href="http://localhost:53005/Manager/HomePage"><i class="fa fa-fw fa-home"></i> Home</a>
        <a href="http://localhost:53005/Manager/OrderPage"><i class="fa fa-shopping-cart" aria-hidden="true"></i> Order</a>
        <a href="http://localhost:53005/Manager/ListUser"><i class="fa fa-fw fa-user"></i> Users</a>
        <a href="http://localhost:53005/Manager/ProductPage"><i class="fa fa-picture-o" aria-hidden="true"></i> Product</a>
        <a href="http://localhost:53005/Users/Logout"><i class="fa fa-sign-out" aria-hidden="true"></i> Logout</a>
    </div>

    <div class="main">
        <div class="container-fluid">
            <div class="row">
                <div class="col-md-9">
                    <div class="container">
                        <div class="row">
                            <div class="col-12 mb-3 mb-lg-5">
                                <div class="overflow-hidden card table-nowrap table-card">
                                    <div class="table-responsive table-wrapper">
                                        @Html.ActionLink("CREATE USER", "CreateUser", "Manager", Nothing, New With {.class = "btn btn-info"})
                                        <table class="table mb-0" style="margin-top:10px">
                                            <thead class="small text-uppercase bg-body text-muted">
                                                <tr>
                                                    <th>IMAGE</th>
                                                    <th>FULL NAME</th>
                                                    <th>EMAIL</th>
                                                    <th>ADDRESS</th>
                                                    <th>PHONE</th>
                                                    <th>BIRTH DAY</th>
                                                    <th class="text-end">Action</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                @For Each item In Model
                                                    @<tr Class="align-middle">
                                                        <td>
                                                            <div Class="d-flex align-items-center">
                                                                <img src="@Url.Content("~/Uploads/" & item.image)" width="150" height="150" Class="avatar sm rounded-pill me-3 flex-shrink-0" alt="Customer">
                                                            </div>
                                                        </td>
                                                        <td>@item.fullName</td>
                                                        <td> <span Class="d-inline-block align-middle">@item.email</span></td>
                                                        <td> <span>@item.address</span></td>
                                                        <td> <span>@item.phone</span></td>
                                                        <td> <span>@item.birthDay</span></td>
                                                        <td Class="text-end">
                                                            <div Class="drodown">
                                                                @Using (Html.BeginForm("DeleteUser", "Manager"))
                                                                    @Html.AntiForgeryToken()
                                                                    @<div class="form-inline my-2 my-lg-0" style="margin-bottom:20px;margin-top:10px">
                                                                        @Html.TextBox("userId", item.id, htmlAttributes:=New With {.class = "form-control mr-sm-2", .style = "display: none;"})
                                                                        <button type="submit" class="btn btn-danger"><i class="fa fa-trash-o" aria-hidden="true"></i></button>
                                                                    </div>
                                                                End Using
                                                            </div>
                                                        </td>
                                                    </tr>
                                                Next
                                            </tbody>
                                        </table>
                                    </div>
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


