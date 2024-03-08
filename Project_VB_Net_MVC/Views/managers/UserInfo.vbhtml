@Code
    ViewData("Title") = "UserInfo"
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
        <a href="http://localhost:53005/Manager/PostPage"><i class="fa fa-pencil" aria-hidden="true"></i> Post</a>
        <a href="http://localhost:53005/Manager/OrderPage"><i class="fa fa-shopping-cart" aria-hidden="true"></i> Order</a>
        <a href="http://localhost:53005/Manager/CustomOrderPage"><i class="fa fa-cart-plus" aria-hidden="true"></i> Custom order</a>
        <a href="http://localhost:53005/Manager/ListUser"><i class="fa fa-fw fa-user"></i> Users</a>
        <a href="http://localhost:53005/Manager/ProductPage"><i class="fa fa-picture-o" aria-hidden="true"></i> Product</a>
        <a href="http://localhost:53005/Manager/UserInfo"><i class="fa fa-info-circle" aria-hidden="true"></i> Info</a>
        <a href="http://localhost:53005/Users/Logout"><i class="fa fa-sign-out" aria-hidden="true"></i> Logout</a>
    </div>
    <div class="main">
        <div class="container-xl px-4 mt-4">
            <div class="row">
                @If Model IsNot Nothing Then
                    Dim user As user = Model.Item1
                    Dim userInfo As user_info = Model.Item2

                    @<div Class="col-xl-4">
                        <div Class="card mb-4 mb-xl-0">
                            <div Class="card-body text-center">
                                <img Class="img-account-profile rounded-circle mb-2" src="@ViewBag.ImagePath" width="200" height="200" alt="">
                                <div Class="input-group">
                                    @Using Html.BeginForm("Upload", "users", FormMethod.Post, New With {.enctype = "multipart/form-data"})
                                        @<input type="submit" value="Upload" Class="btn btn-outline-secondary" />
                                        @<input type="file" name="file" Class="form-control" />
                                        @Code
                                            Dim imageUpload = TempData("imageUpload")
                                        End Code
                                        @If imageUpload IsNot Nothing Then
                                            @<p style="color:red; margin: 10px 0 0 0">
                                                @imageUpload
                                            </p>
                                        End If
                                    End Using
                                </div>
                            </div>
                        </div>
                    </div>
                    @<div Class="col-xl-8">
                        <div Class="card mb-4">
                            <div Class="card-body">
                                @Using (Html.BeginForm("UpdateInfoUser", "users"))
                                    @Html.AntiForgeryToken()
                                    @<div Class="mb-3">
                                        <Label Class="small mb-1" for="inputUsername">Full Name</Label>
                                        <input Class="form-control" id="inputUsername" disabled type="text" value="@user.full_name">
                                    </div>
                                    @<div Class="row gx-3 mb-3">
                                        <div Class="col-md-6">
                                            <Label Class="small mb-1" for="inputFirstName">Email</Label>
                                            <input Class="form-control" id="inputFirstName" disabled type="text" value="@user.email">
                                        </div>
                                        <div Class="col-md-6">
                                            <Label Class="small mb-1" for="inputLastName">Address</Label>
                                            <input Class="form-control" id="inputLastName" type="text" name=address value="@userInfo.address">
                                            @Code
                                                Dim addressUpdate = TempData("addressUpdate")
                                            End Code
                                            @If addressUpdate IsNot Nothing Then
                                                @<p style="color:red; margin: 10px 0 0 0">
                                                    @addressUpdate
                                                </p>
                                            End If
                                            <Button Class="btn btn-primary" type="submit" style="margin-top:20px">Save changes</Button>
                                        </div>
                                    </div>
                                    @<div Class="row gx-3 mb-3">
                                        <div Class="col-md-6">
                                            <Label Class="small mb-1" for="inputOrgName">Phone Number</Label>
                                            <input Class="form-control" id="inputOrgName" name="phone" type="tel" value="@userInfo.phone">
                                            @Code
                                                Dim phoneUpdate = TempData("phoneUpdate")
                                            End Code
                                            @If phoneUpdate IsNot Nothing Then
                                                @<p style="color:red; margin: 10px 0 0 0">
                                                    @phoneUpdate
                                                </p>
                                            End If
                                        </div>
                                    </div>
                                    @<div Class="row gx-3 mb-3">
                                        <div Class="col-md-6">
                                            <Label Class="small mb-1" for="inputBirthday">Birthday</Label>
                                            @If userInfo.birth_day IsNot Nothing Then
                                                @<input Class="form-control" id="inputBirthday" type="date" name="birthday" value="@userInfo.birth_day.Value.ToShortDateString()">
                                            Else
                                                @<input Class="form-control" id="inputBirthday" type="date" name="birthday" value="@Date.Now().ToShortDateString()">
                                            End If
                                        </div>
                                    </div>
                                End Using
                            </div>
                        </div>
                    </div>
                End If
            </div>
        </div>
    </div>
</body>
</html>


