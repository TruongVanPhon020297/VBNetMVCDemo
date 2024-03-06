@ModelType List(Of product)

@Code
    ViewData("Title") = "Favorite"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@If Model.Count > 0 Then
    @<div class="container px-3 my-5 clearfix">
        <!-- Shopping cart table -->
        <div class="card">
            <div class="card-header">
                <h2>Product Favorite</h2>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table table-bordered m-0">
                        <thead>
                            <tr>
                                <!-- Set columns width -->
                                <th class="text-center py-3 px-4" style="min-width: 400px;">Product Name &amp; Details</th>
                                <th class="text-right py-3 px-4" style="width: 100px;">Price</th>
                                <th class="text-center align-middle py-3 px-0" style="width: 40px;"><a href="#" class="shop-tooltip float-none text-light" title="" data-original-title="Clear cart"><i class="ino ion-md-trash"></i></a></th>
                            </tr>
                        </thead>
                        <tbody>
                            @For Each detail In Model
                                @<tr>
                                    <td class="p-4">
                                        <div class="media align-items-center">
                                            <img src="@Url.Content("~/Uploads/" & detail.image)" width="100" height="100" class="d-block ui-w-40 ui-bordered mr-4" alt="">
                                            <div class="media-body">
                                                @Html.ActionLink(detail.product_name, "Detail", "Users", New With {.id = detail.Id}, "")
                                            </div>
                                        </div>
                                    </td>
                                    <td class="text-right font-weight-semibold align-middle p-4">$@detail.price</td>
                                    <td class="text-center align-middle px-0">
                                        @Using (Html.BeginForm("RemoveFavorite", "users"))
                                            @Html.AntiForgeryToken()
                                            @Html.TextBox("productId", detail.Id, htmlAttributes:=New With {.class = "form-control mr-sm-2", .style = "display: none;"})
                                            @<input type="submit" value="×" class="shop-tooltip close float-none text-danger" />
                                        End Using
                                    </td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
                <div class="float-right" style="margin-top:10px">
                    @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
                </div>
            </div>
        </div>
    </div>
Else
    @<p>No favorite product information available.</p>
    @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
End If

