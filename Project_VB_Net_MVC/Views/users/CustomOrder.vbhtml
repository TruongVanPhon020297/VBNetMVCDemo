@ModelType List(Of category)

@Code
    ViewData("Title") = "CustomOrder"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
<div class="container-fluid bootstrap snippets bootdeys">
    <div class="row">
        @Using Html.BeginForm("CreateCustomOrder", "Users")
            @Html.AntiForgeryToken()
            @<div Class="col-xs-12 col-sm-12">
                <div Class="panel panel-default">
                    <div Class="panel-heading">
                        <h4 Class="panel-title">Order Info</h4>
                    </div>
                    <div Class="panel-body">
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
                            <Label Class="col-sm-2 control-label">Size description</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <textarea rows="5" Class="form-control" name="size"></textarea>
                                @Code
                                    Dim size = TempData("size")
                                End Code
                                @If size IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @size
                                    </p>
                                End If
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Description</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <textarea rows="5" Class="form-control" name="description"></textarea>
                                @Code
                                    Dim description = TempData("description")
                                End Code
                                @If description IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @description
                                    </p>
                                End If
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Quantity</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input Class="form-control" type="number" name="quantity" value="0" />
                                @Code
                                    Dim quantity = TempData("quantity")
                                End Code
                                @If quantity IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @quantity
                                    </p>
                                End If
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Note</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <textarea rows="5" Class="form-control" name="note"></textarea>
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Delivery date</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input class="form-control" type="datetime-local" value="@DateTime.Now.ToString("yyyy-MM-ddTHH:mm")" name="deliveryDate" />
                                @Code
                                    Dim deliveryDate = TempData("deliveryDate")
                                End Code
                                @If deliveryDate IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @deliveryDate
                                    </p>
                                End If
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Address</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <textarea rows="2" Class="form-control" name="address"></textarea>
                                @Code
                                    Dim address = TempData("address")
                                End Code
                                @If address IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @address
                                    </p>
                                End If
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Recipient full name </Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input Class="form-control" type="text" name="recipient" />
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Phone</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input Class="form-control" type="number" name="phone" />
                                @Code
                                    Dim phone = TempData("phone")
                                End Code
                                @If phone IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @phone
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
        @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
        @Html.ActionLink("List custom order", "CustomOrderInfo", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-info md-btn-flat mt-2 mr-3"})
    </div>
</div>

