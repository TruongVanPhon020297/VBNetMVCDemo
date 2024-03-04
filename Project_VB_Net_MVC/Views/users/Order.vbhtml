@ModelType List(Of order)
@Code
    ViewData("Title") = "Order"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code


<section class="intro">
    <div class="bg-image h-100" style="background-color: #6095F0;">
        <div class="mask d-flex align-items-center h-100">
            <div class="container">
                <div class="row justify-content-center">
                    <div class="col-12">
                        <div class="card shadow-2-strong" style="background-color: #f5f7fa;">
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table table-borderless mb-0">
                                        <thead>
                                            <tr>
                                                <th scope="col">
                                                    STATUS
                                                </th>
                                                <th scope="col">ORDER DATE</th>
                                                <th scope="col">TOTAL PRICE</th>
                                                <th scope="col">QUANTITY</th>
                                                <th scope="col">DETAILS</th>
                                                <th scope="col">CANCEL ORDER</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            @For Each detail In Model
                                                @<tr>
                                                    <th scope="row">
                                                        <div class="form-check">
                                                            @If detail.status = True Then
                                                                @<input Class="form-check-input" type="checkbox" disabled value="" id="flexCheckDefault1" checked />
                                                            Else
                                                                @<input Class="form-check-input" type="checkbox" disabled value="" id="flexCheckDefault1" />
                                                            End IF

                                                        </div>
                                                    </th>
                                                    <td>@detail.register_time</td>
                                                    <td>@detail.total_price</td>
                                                    <td>@detail.quantity</td>
                                                    <td>
                                                        @Html.ActionLink("GO TO", "OrderDetail", New With {.id = detail.id})
                                                    </td>
                                                    <td>
                                                        @If detail.status = False Then
                                                            @Using (Html.BeginForm("CancelOrder", "users"))
                                                                @Html.AntiForgeryToken()
                                                                @Html.TextBox("orderId", detail.id, htmlAttributes:=New With {.class = "form-control mr-sm-2", .style = "display: none;"})
                                                                @<input type="submit" value="×" class="btn-danger" />
                                                            End Using
                                                        End If
                                                    </td>
                                                </tr>
                                            Next
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3", .style = "margin-top:10px"})
</section>

