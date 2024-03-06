@ModelType Tuple(Of List(Of cart_detail), cart)

@Code
    ViewData("Title") = "OrderInfo"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code




@if Model IsNot Nothing Then
    Dim cartDetails As List(Of cart_detail) = Model.Item1
    Dim cart As cart = Model.Item2
    @<div class="container px-3 my-5 clearfix">
        <!-- Shopping cart table -->
        <div class="card">
            <div class="card-header">
                <h2>Shopping Cart</h2>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table table-bordered m-0">
                        <thead>
                            <tr>
                                <!-- Set columns width -->
                                <th class="text-center py-3 px-4" style="min-width: 400px;">Product Name &amp; Details</th>
                                <th class="text-right py-3 px-4" style="width: 100px;">Price</th>
                                <th class="text-center py-3 px-4" style="width: 120px;">Quantity</th>
                                <th class="text-right py-3 px-4" style="width: 100px;">Total</th>
                                <th class="text-center align-middle py-3 px-0" style="width: 40px;"><a href="#" class="shop-tooltip float-none text-light" title="" data-original-title="Clear cart"><i class="ino ion-md-trash"></i></a></th>
                            </tr>
                        </thead>
                        <tbody>
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
                        </tbody>
                    </table>
                </div>
                <!-- / Shopping cart table -->

                <div class="d-flex flex-wrap justify-content-between align-items-center pb-4">
                    <div class="d-flex">
                        <div class="text-right mt-4 mr-5">
                            <label class="text-muted font-weight-normal m-0">Quantity</label>
                            <div class="text-large"><strong>@cart.quantity</strong></div>
                        </div>
                        <div class="text-right mt-4">
                            <label class="text-muted font-weight-normal m-0">Total price</label>
                            <div class="text-large"><strong>$@cart.total_price</strong></div>
                        </div>
                    </div>
                </div>

                @Using (Html.BeginForm("Checkout", "users"))
                    @<Label Class="text-muted font-weight-normal">Address</Label>
                    @Html.TextBox("address", "", htmlAttributes:=New With {.class = "form-control"})
                    @Code
                        Dim address = TempData("address")
                    End Code
                    @If address IsNot Nothing Then
                        @<p style="color:red; margin: 10px 0 0 0">
                            @address
                        </p>
                    End If
                    @<Label Class="text-muted font-weight-normal">Phone</Label>
                    @Html.TextBox("phone", "", htmlAttributes:=New With {.class = "form-control", .style = "margin-bottom:10px;"})
                    @Code
                        Dim phone = TempData("phone")
                    End Code
                    @If phone IsNot Nothing Then
                        @<p style="color:red; margin: 10px 0 0 0">
                            @phone
                        </p>
                    End If
                    @Html.AntiForgeryToken()
                    @Html.TextBox("cartId", cart.id, htmlAttributes:=New With {.class = "", .style = "display: none;"})
                    @<input type="submit" value="Checkout" class="btn btn-lg btn-primary mt-2" />
                End Using

                <div class="float-right" style="margin-top:10px">
                    @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
                </div>


            </div>
        </div>
    </div>

Else
    @<p>No cart information available.</p>
    @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
End If



