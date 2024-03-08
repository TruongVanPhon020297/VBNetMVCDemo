@ModelType  Tuple(Of List(Of PostCommentData), post)

@Code
    ViewData("Title") = "PostDetail"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bài viết về làm bánh</title>
    <link rel="stylesheet" href="styles.css">
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
        }

        .article {
            max-width: 80%;
            margin: 20px auto;
            padding: 20px;
            border: 1px solid #ccc;
        }

        .title {
            color: #333;
        }

        .date {
            color: #666;
            font-style: italic;
        }

        .content,
        .ingredients,
        .instructions {
            margin-top: 20px;
        }

            .ingredients ul,
            .instructions ol {
                margin-left: 20px;
            }

        input, textarea, select {
            max-width: 100%
        }
    </style>
</head>
<body>
    <div class="article">
        <h3 class="title">@Model.Item2.title</h3>
        <p class="date">Ngày tạo: @Model.Item2.register_date.ToShortDateString()</p>
        <div class="content">
            <p>@Model.Item2.post_content</p>
        </div>
        <div class="ingredients">
            <h2>Nguyên liệu chuẩn bị</h2>
            <p>
                @Model.Item2.ingredient
            </p>
        </div>
        <div class="instructions">
            <h2>Cách làm bánh</h2>
            <p>
                @Model.Item2.process
            </p>
        </div>
        <div class="image">
            <img src="@Url.Content("~/Uploads/" & Model.Item2.image)" alt="Hình ảnh bánh" style="width:100%">
        </div>

    </div>

    <div class="container-fluid bootstrap snippets bootdeys">
        <div class="row">
            <div Class="col-xs-12 col-sm-12">
                <div Class="panel panel-default">
                    <div Class="panel-heading">
                        <h4 Class="panel-title">Comment Info</h4>
                    </div>
                    <section>
                        <div class="container my-5 py-5">
                            <div class="row d-flex justify-content-center">
                                <div class="col-md-12 col-lg-10">
                                    <div class="card text-dark">
                                        @If Model.Item1 IsNot Nothing Then
                                            For Each item In Model.Item1
                                                @<div Class="card-body p-4">
                                                    <div Class="d-flex flex-start">
                                                        <img Class="rounded-circle shadow-1-strong me-3"
                                                             src="@Url.Content("~/Uploads/" & item.image)" alt="avatar" width="60"
                                                             height="60" />
                                                        <div>
                                                            <h6 Class="fw-bold mb-1">@item.userName</h6>
                                                            <div Class="d-flex align-items-center mb-3">
                                                                <p Class="mb-0">
                                                                   @item.postComment.register_date.ToShortDateString()
                                                                    <span Class="badge bg-success">Approved</span>
                                                                </p>
                                                                <a href="#!" Class="link-muted"><i Class="fas fa-pencil-alt ms-2"></i></a>
                                                                <a href="#!" Class="text-success"><i Class="fas fa-redo-alt ms-2"></i></a>
                                                                <a href="#!" Class="link-danger"><i Class="fas fa-heart ms-2"></i></a>
                                                            </div>
                                                            <p Class="mb-0">
                                                                @item.postComment.comment
                                                            </p>
                                                        </div>
                                                    </div>
                                                </div>

                                                @<hr Class="my-0" style="height: 1px;" />
                                            Next
                                        End If
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                    @Using Html.BeginForm("CreateComment", "Users")
                        @Html.AntiForgeryToken()
                        @<div Class="panel-body">
                            <div Class="form-group">
                                <Label Class="col-sm-2 control-label">Comment</Label>
                                <div Class="col-sm-10" style="margin-top:10px">
                                    <textarea rows="4" Class="form-control" name="content"></textarea>
                                </div>
                            </div>
                            <div Class="form-group">
                                <div Class="col-sm-10 col-sm-offset-2" style="margin-top:10px">
                                    <input type="hidden" name="postId" value="@Model.Item2.id" />
                                    <Button type="submit" Class="btn btn-primary">Submit</Button>
                                </div>
                            </div>
                        </div>
                    End Using
                </div>
            </div>

        </div>
    </div>

    @Html.ActionLink("Back to list", "PostPage", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
</body>
</html>



