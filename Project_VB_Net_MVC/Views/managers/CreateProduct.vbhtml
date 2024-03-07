@ModelType List(Of category)

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
                        @Code
                            Dim fileCreate = TempData("fileCreate")
                        End Code
                        @If fileCreate IsNot Nothing Then
                            @<p style="color:red; margin: 10px 0 0 0">
                                @fileCreate
                            </p>
                        End If
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
                                <input type="text" name="productName" Class="form-control" value="">
                                @Code
                                    Dim productNameCreate = TempData("productNameCreate")
                                End Code
                                @If productNameCreate IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @productNameCreate
                                    </p>
                                End If
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Category</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <select class="form-control" name="categoryId" aria-label="Default select example">
                                    @For Each item In Model
                                        @<option value="@item.id">@item.category_name</option>
                                    Next
                                </select>
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Price</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input type="number" name="price" Class="form-control" value="0">
                                @Code
                                    Dim priceCreate = TempData("priceCreate")
                                End Code
                                @If priceCreate IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @productNameCreate
                                    </p>
                                End If
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Description</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <textarea rows="3" Class="form-control" name="description"></textarea>
                                @Code
                                    Dim descriptionCreate = TempData("descriptionCreate")
                                End Code
                                @If descriptionCreate IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @descriptionCreate
                                    </p>
                                End If
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

