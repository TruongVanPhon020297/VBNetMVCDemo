@ModelType List(Of custom_order)

@Code
    ViewData("Title") = "CustomOrderInfo"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@If Model.Count > 0 Then
    @<section Class="intro">
        <div Class="bg-image h-100" style="background-color: #6095F0;">
            <div Class="mask d-flex align-items-center h-100">
                <div Class="container">
                    <div Class="row justify-content-center">
                        <div Class="col-12">
                            <div Class="card shadow-2-strong" style="background-color: #f5f7fa;">
                                <div Class="card-body">
                                    <div Class="table-responsive">
                                        <Table Class="table table-borderless mb-0">
                                            <thead>
                                                <tr>
                                                    <th scope="col">
                                                        STATUS
                                                    </th>
                                                    <th scope="col"> ORDER DATE</th>
                                                    <th scope="col"> SIZE DESCRIPTION</th>
                                                    <th scope="col"> QUANTITY</th>
                                                    <th scope="col"> DELIVERY DATE</th>
                                                    <th scope="col"> DETAILS</th>
                                                    <th scope="col"> CANCEL ORDER</th>
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
                                                        <td>@detail.size_description</td>
                                                        <td>@detail.quantity</td>
                                                        <td>@detail.delivery_date</td>
                                                        <td>
                                                            @Html.ActionLink("GO TO", "CustomOrderDetailInfo", New With {.id = detail.id})
                                                        </td>
                                                        <td>
                                                            @If detail.confirm = False Then
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
                                        </Table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    @Html.ActionLink("Back to custom order", "CustomOrder", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3", .style = "margin-top:10px"})
    @Html.ActionLink("Back to home", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3", .style = "margin-top:10px;margin-left:10px"})
Else
    @<p>No order information available.</p>
    @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
End If

