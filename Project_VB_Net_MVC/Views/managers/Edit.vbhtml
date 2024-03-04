@ModelType Project_VB_Net_MVC.product

@Code
    ViewData("Title") = "Edit"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
<div class="container bootstrap snippets bootdeys">
    <div class="row">
        <div class="col-xs-12 col-sm-9">
            @Html.ActionLink("PRODUCT LIST", "ProductPage", "Manager", Nothing, New With {.class = "btn btn-info"})
            <div class="panel panel-default" style="margin-top:10px">
                <div class="panel-body text-center">
                    <img src="@ViewBag.ImagePath" class="img-circle profile-avatar" alt="User avatar" width="300" height="300">
                </div>
                <div Class="input-group panel-body text-center">
                    @Using Html.BeginForm("Upload", "Manager", FormMethod.Post, New With {.enctype = "multipart/form-data"})
                        @Html.AntiForgeryToken()
                        @<input type="file" name="file" Class="form-control" />
                        @<input type="hidden" name="productId" value="@Model.Id" Class="form-control" />
                        @<input type="submit" value="Upload" Class="btn btn-outline-secondary" />
                    End Using
                </div>
            </div>
            @Using (Html.BeginForm("Edit", "Manager"))
                @Html.AntiForgeryToken()
                @<div Class="panel panel-default">
                    <div Class="panel-heading">
                        <h4 Class="panel-title">Contact info</h4>
                        <input type="hidden" name="productId" value="@Model.Id " Class="form-control" />
                    </div>
                    <div Class="panel-body">
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Product Name</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input type="tel" name="productName" Class="form-control" value="@Model.product_name">
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Price</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input type="tel" name="price" Class="form-control" value="@Model.price">
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Description</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <textarea rows="3" Class="form-control" name="description">@Model.description</textarea>
                            </div>
                        </div>
                        <div Class="form-group">
                            <div Class="col-sm-10 col-sm-offset-2" style="margin-top:10px">
                                <Button type="submit" Class="btn btn-primary">Submit</Button>
                            </div>
                        </div>
                    </div>
                </div>
            End Using
        </div>
    </div>
</div>
