@ModelType post

@Code
    ViewData("Title") = "PostReview"
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
            max-width:80%;
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

    </style>
</head>
<body>
    <div class="article">
        <h3 class="title">@Model.title</h3>
        <p class="date">Ngày tạo: @Model.register_date.ToShortDateString()</p>
        <div class="content">
            <p>@Model.post_content</p>
        </div>
        <div class="ingredients">
            <h2>Nguyên liệu chuẩn bị</h2>
            <p>
                @Model.ingredient
            </p>
        </div>
        <div class="instructions">
            <h2>Cách làm bánh</h2>
            <p>
                @Model.process
            </p>
        </div>
        <div class="image">
            <img src="@Url.Content("~/Uploads/" & Model.image)" alt="Hình ảnh bánh" style="width:100%">
        </div>
    </div>

    @Html.ActionLink("Back to list", "PostPage", "Manager", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
</body>
</html>


