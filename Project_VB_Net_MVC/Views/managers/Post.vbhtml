@ModelType List(Of post)

@Code
    ViewData("Title") = "Post"
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

        body {
            font-family: 'Calibri', sans-serif !important;
        }

        .card-no-border .card {
            border: 0px;
            border-radius: 4px;
            -webkit-box-shadow: 0px 5px 20px rgba(0, 0, 0, 0.05);
            box-shadow: 0px 5px 20px rgba(0, 0, 0, 0.05)
        }

        .card-body {
            -ms-flex: 1 1 auto;
            flex: 1 1 auto;
            padding: 1.25rem
        }

        .comment-widgets .comment-row:hover {
            background: rgba(0, 0, 0, 0.02);
            cursor: pointer;
        }

        .comment-widgets .comment-row {
            border-bottom: 1px solid rgba(120, 130, 140, 0.13);
            padding: 15px;
        }

        .comment-text:hover {
            visibility: hidden;
        }

        .comment-text:hover {
            visibility: visible;
        }

        .label {
            padding: 3px 10px;
            line-height: 13px;
            color: #ffffff;
            font-weight: 400;
            border-radius: 4px;
            font-size: 75%;
        }

        .round img {
            border-radius: 100%;
        }

        .label-info {
            background-color: #1976d2;
        }

        .label-success {
            background-color: green;
        }

        .label-danger {
            background-color: #ef5350;
        }

        .action-icons a {
            padding-left: 7px;
            vertical-align: middle;
            color: #99abb4;
        }

            .action-icons a:hover {
                color: #1976d2;
            }

        .mt-100 {
            margin-top: 100px
        }

        .mb-100 {
            margin-bottom: 100px
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
        <section class="intro">
            <div class="bg-image h-100" style="background-color: #6095F0;">
                <div class="mask d-flex align-items-center h-100">
                    <div class="container">
                        <div class="row justify-content-center">
                            <div class="col-12">
                                <div class="card shadow-2-strong" style="background-color: #f5f7fa;">
                                    <div class="card-body">
                                        <div class="table-responsive">
                                            @Html.ActionLink("CREATE POST", "CreatePostPage", "Manager", Nothing, New With {.class = "btn btn-info"})

                                        </div>
                                        <div class="container d-flex justify-content-center mt-100 mb-100">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    @For Each item In Model
                                                        @<div Class="card">
                                                            <div Class="comment-widgets m-b-20">
                                                                <div Class="d-flex flex-row comment-row">
                                                                    <div Class="p-2"><span class="round"><img src="@Url.Content("~/Uploads/" & item.image)" alt="user" width="50"></span></div>
                                                                    <div Class="comment-text w-100">
                                                                        @Html.ActionLink(item.title, "PostReviewPage", "Manager", New With {.id = item.id}, "")
                                                                        <div Class="comment-footer">
                                                                            <span Class="date">@item.register_date</span>
                                                                            <span Class="action-icons">
                                                                                <a href="#" data-abc="true"><i Class="fa fa-comment-o" aria-hidden="true"></i> 100</a>
                                                                            </span>
                                                                        </div>
                                                                        <p Class="m-b-5 m-t-10">@item.post_content</p>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    Next
                                                </div>
                                            </div>
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

