@ModelType Tuple(Of List(Of product), List(Of user), cart, List(Of cart_detail))

@Code
    ViewData("Title") = "CreateOrder"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@if Model IsNot Nothing Then
    Dim products As List(Of product) = Model.Item1
    Dim users As List(Of user) = Model.Item2
    Dim cart As cart = Model.Item3
    Dim cartDetails As List(Of cart_detail) = Model.Item4
    @<div class="container bootstrap snippets bootdeys">
        <div class="row">
            @Using Html.BeginForm("AddProduct", "Manager")
                @Html.AntiForgeryToken()
                @<div Class="col-xs-12 col-sm-9">
                    <div Class="panel panel-default">
                        <div Class="panel-heading">
                            <h4 Class="panel-title">Product Info</h4>
                        </div>
                        <div Class="panel-body">
                            <div Class="form-group">
                                <Label Class="col-sm-2 control-label">Product</Label>
                                <div Class="col-sm-10" style="margin-top:10px">
                                    <select class="form-control" name="productId" aria-label="Default select example">
                                        @For Each item In products
                                            @<option value="@item.Id">@item.product_name</option>
                                        Next
                                    </select>
                                </div>
                            </div>
                            <div Class="form-group">
                                <Label Class="col-sm-2 control-label">Quantity</Label>
                                <div Class="col-sm-10" style="margin-top:10px">
                                    <input type="number" name="quantity" Class="form-control" value="1" >
                                </div>
                            </div>
                            <div Class="form-group">
                                <div Class="col-sm-10 col-sm-offset-2" style="margin-top:10px">
                                    <Button type="submit" Class="btn btn-primary">Add</Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            End Using
        </div>
        <div class="row">
            @Using Html.BeginForm("Checkout", "Manager", FormMethod.Post, New With {.enctype = "multipart/form-data"})
                @Html.AntiForgeryToken()
                @<div Class="col-xs-12 col-sm-9">
                    <div Class="panel panel-default">
                        <div Class="panel-heading">
                            <h4 Class="panel-title">User Info</h4>
                        </div>
                        <div Class="panel-body">
                            <div Class="form-group">
                                <Label Class="col-sm-2 control-label">User</Label>
                                <div Class="col-sm-10" style="margin-top:10px">
                                    <select class="form-control" name="userId" aria-label="Default select example">
                                        @For Each item In users
                                            @<option value="@item.id">@item.email</option>
                                        Next
                                    </select>
                                </div>
                            </div>
                            <div Class="form-group">
                                <Label Class="col-sm-2 control-label">Address</Label>
                                <div Class="col-sm-10" style="margin-top:10px">
                                    <input type="text" name="address" Class="form-control" value="">
                                    @Code
                                        Dim addressCheckout = TempData("addressCheckout")
                                    End Code
                                    @If addressCheckout IsNot Nothing Then
                                        @<p style="color:red; margin: 10px 0 0 0">
                                            @addressCheckout
                                        </p>
                                    End If
                                </div>
                            </div>
                            <div Class="form-group">
                                <Label Class="col-sm-2 control-label">Phone</Label>
                                <div Class="col-sm-10" style="margin-top:10px">
                                    <input type="text" name="phone" Class="form-control" value="">
                                    @Code
                                        Dim phoneCheckout = TempData("phoneCheckout")
                                    End Code
                                    @If phoneCheckout IsNot Nothing Then
                                        @<p style="color:red; margin: 10px 0 0 0">
                                            @phoneCheckout
                                        </p>
                                    End If
                                </div>
                            </div>
                            <div Class="form-group">
                                <div Class="col-sm-10 col-sm-offset-2" style="margin-top:10px">
                                    <Button type="submit" Class="btn btn-primary">Checkout</Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                                        End Using
        </div>
    </div>

    @<div class="container px-3 my-5 clearfix">
        <div class="card">
            <div class="card-header">
                <h2> Details</h2>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table table-bordered m-0">
                        <thead>
                            <tr>
                                <th class="text-center py-3 px-4" style="min-width: 400px;">Product Name &amp; Details</th>
                                <th class="text-right py-3 px-4" style="width: 100px;">Price</th>
                                <th class="text-center py-3 px-4" style="width: 120px;">Quantity</th>
                                <th class="text-right py-3 px-4" style="width: 100px;">Total</th>
                                <th class="text-center align-middle py-3 px-0" style="width: 40px;"><a href="#" class="shop-tooltip float-none text-light" title="" data-original-title="Clear cart"><i class="ino ion-md-trash"></i></a></th>
                            </tr>
                        </thead>
                        <tbody>
                            @If cartDetails IsNot Nothing Then
                                @For Each detail In cartDetails
                                    @<tr>
                                        <td class="p-4">
                                            <div class="media align-items-center">
                                                <img src="@Url.Content("~/Uploads/" & detail.image)" width="200" height="200" class="d-block ui-w-40 ui-bordered mr-4" alt="">
                                                <div class="media-body">
                                                    @Html.ActionLink(detail.product_name, "Detail", "Users", New With {.id = detail.id}, "")
                                                </div>
                                            </div>
                                        </td>
                                        <td class="text-right font-weight-semibold align-middle p-4">$@detail.price</td>
                                        <td class="align-middle p-4">
                                            @Using (Html.BeginForm("Increament", "users"))
                                                @Html.AntiForgeryToken()
                                                @Html.TextBox("detailId", detail.id, htmlAttributes:=New With {.class = "form-control mr-sm-2", .style = "display: none;"})
                                                @<input type="submit" value="+" class="btn btn-info" />
                                            End Using
                                            <input type="text" class="form-control text-center" value="@detail.quantity">
                                            @Using (Html.BeginForm("Decreament", "users"))
                                                @Html.AntiForgeryToken()
                                                @Html.TextBox("detailId", detail.id, htmlAttributes:=New With {.class = "form-control mr-sm-2", .style = "display: none;"})
                                                @<input type="submit" value="-" class="btn btn-info" />
                                            End Using
                                        </td>
                                        <td class="text-right font-weight-semibold align-middle p-4">$@detail.total_price</td>
                                        <td class="text-center align-middle px-0">
                                            @Using (Html.BeginForm("RemoveDetail", "users"))
                                                @Html.AntiForgeryToken()
                                                @Html.TextBox("detailId", detail.id, htmlAttributes:=New With {.class = "form-control mr-sm-2", .style = "display: none;"})
                                                @<input type="submit" value="×" class="shop-tooltip close float-none text-danger" />
                                            End Using
                                        </td>
                                    </tr>
                                Next
                            End If
                        </tbody>
                    </table>
                </div>
                @If cart IsNot Nothing Then
                    @<div Class="d-flex flex-wrap justify-content-between align-items-center pb-4">
                        <div Class="d-flex">
                            <div Class="text-right mt-4 mr-5">
                                <Label Class="text-muted font-weight-normal m-0">Quantity</Label>
                                <div Class="text-large"><strong>@cart.quantity</strong></div>
                            </div>
                            <div Class="text-right mt-4">
                                <Label Class="text-muted font-weight-normal m-0">Total price</Label>
                                <div Class="text-large"><strong>$@cart.total_price</strong></div>
                            </div>
                        </div>
                    </div>
                End If
                <div class="float-right" style="margin-top:10px">
                    @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
                </div>
            </div>
        </div>
    </div>
End If