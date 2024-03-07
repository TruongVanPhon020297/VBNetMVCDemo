@ModelType Tuple(Of custom_order, category)

@Code
    ViewData("Title") = "CustomOrderDetail"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
<div class="container-fluid bootstrap snippets bootdeys">
    <div class="row">

        @If Model IsNot Nothing Then

            @<div Class="col-xs-12 col-sm-12">
                <div Class="panel panel-default">
                    <div Class="panel-heading">
                        <h4 Class="panel-title">Order Info</h4>
                    </div>
                    <div Class="panel-body">
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Category</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                @Model.Item2.category_name
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Size description</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                @Model.Item1.size_description
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Description</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                @Model.Item1.description
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Quantity</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                @Model.Item1.quantity
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Note</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                @Model.Item1.note
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Delivery date</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                @Model.Item1.delivery_date
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Address</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                @Model.Item1.address
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Recipient full name </Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                @Model.Item1.recipient_full_name
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Phone</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                @Model.Item1.phone
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Status</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                @If Model.Item1.status Then
                                    @<span class="btn btn-success" style="font-weight:bold">Success</span>
                                Else
                                    @If Model.Item1.confirm Then
                                        @<span class="btn btn-danger" style="font-weight:bold">Confirm</span>
                                    End If
                                End If
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Total price</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                @If Model.Item1.total_price IsNot Nothing Then
                                    @<span>@Model.Item1.total_price</span>
                                Else
                                    @<span>0</span>
                                End If
                            </div>
                        </div>
                        @If Model.Item1.img_product IsNot Nothing Then
                            @<div Class="form-group">
                                <img Class="img-account-profile rounded-circle mb-2" src="@Url.Content("~/Uploads/" & Model.Item1.img_product)" width="200" height="200" alt="" style="margin-top:20px">
                            </div>
                        End If
                    </div>
                </div>
            </div>

        End If

        @Html.ActionLink("Back to list", "CustomOrderInfo", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
        @Html.ActionLink("Back to home", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3", .style = "margin-left:10px"})
    </div>
</div>

