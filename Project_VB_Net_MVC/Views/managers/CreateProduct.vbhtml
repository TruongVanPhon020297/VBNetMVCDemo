@Code
    ViewData("Title") = "CreateProduct"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
<div class="container bootstrap snippets bootdeys">
    <div class="row">
        @Using Html.BeginForm("CreateProduct", "Manager", FormMethod.Post, New With {.enctype = "multipart/form-data"})
            @Html.AntiForgeryToken()
            @<div Class="col-xs-12 col-sm-9">
                @Html.ActionLink("PRODUCT LIST", "ProductPage", "Manager", Nothing, New With {.class = "btn btn-info"})
                <div Class="panel panel-default" style="margin:10px">
                    <div Class="input-group panel-body text-center">
                        <input type="file" name="file" Class="form-control" />
                    </div>
                </div>
                <div Class="panel panel-default">
                    <div Class="panel-heading">
                        <h4 Class="panel-title">Product Info</h4>
                    </div>
                    <div Class="panel-body">
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Product Name</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input type="tel" name="productName" Class="form-control" value="">
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Price</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input type="tel" name="price" Class="form-control" value="">
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Description</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <textarea rows="3" Class="form-control" name="description"></textarea>
                            </div>
                        </div>
                        <div Class="form-group">
                            <div Class="col-sm-10 col-sm-offset-2" style="margin-top:10px">
                                <Button type="submit" Class="btn btn-primary">Create</Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        End Using

    </div>
</div>

