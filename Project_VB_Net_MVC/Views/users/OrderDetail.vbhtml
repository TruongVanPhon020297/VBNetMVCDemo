@ModelType Tuple(Of List(Of order_detail), delivery, order)

@Code
    ViewData("Title") = "OrderDetail"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@if Model IsNot Nothing Then
    Dim orderDetails As List(Of order_detail) = Model.Item1
    Dim delivery As delivery = Model.Item2
    Dim order As order = Model.Item3
    @<div class="container px-3 my-5 clearfix">
        <div class="card">
            <div class="card-header">
                <h2>Order Detail</h2>
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
                            </tr>
                        </thead>
                        <tbody>
                            @For Each detail In orderDetails
                                @<tr>
                                    <td class="p-4">
                                        <div class="media align-items-center">
                                            <img src="@Url.Content("~/Uploads/" & detail.image)" width="200" height="200" class="d-block ui-w-40 ui-bordered mr-4" alt="">
                                            <div class="media-body">
                                                <a href="#" class="d-block text-dark">@detail.product_name</a>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="text-right font-weight-semibold align-middle p-4">$@detail.price</td>
                                    <td class="align-middle p-4">
                                        @detail.quantity
                                    </td>
                                    <td class="text-right font-weight-semibold align-middle p-4">$@detail.total_price</td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>

                <div class="d-flex flex-wrap justify-content-between align-items-center pb-4">
                    <div class="d-flex">
                        <div class="text-right mt-4 mr-5">
                            <label class="text-muted font-weight-normal m-0">Quantity</label>
                            <div class="text-large"><strong>@order.quantity</strong></div>
                        </div>
                        <div class="text-right mt-4">
                            <label class="text-muted font-weight-normal m-0">Total price</label>
                            <div class="text-large"><strong>$@order.total_price</strong></div>
                        </div>
                    </div>
                </div>

                @Using (Html.BeginForm("Checkout", "users"))
                    @<Label Class="text-muted font-weight-normal">Address</Label>
                    @<div>@delivery.location</div>
                    @<Label Class="text-muted font-weight-normal">Phone</Label>
                    @<div>@delivery.phone</div>
                    @If order.status = True Then
                        @<Label Class="text-muted font-weight-normal">Delivery Date</Label>
                        @<div>@delivery.delivery_date.Value.ToShortDateString()</div>
                    End If
                End Using

                <div Class="float-right" style="margin-top:10px">
                    @Html.ActionLink("Back to order list", "OrderInfo", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
                </div>

            </div>
        </div>
    </div>
Else
    @<p>No order information available.</p>
    @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
End If
