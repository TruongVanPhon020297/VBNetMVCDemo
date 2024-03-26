@ModelType Tuple(Of List(Of ingredient), List(Of purchased_order_detail), purchased_order)
@Code
    ViewData("Title") = "CreatePurchaseOrder"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="container-fluid">
    <div class="row">
        <div class="col-md-9">
            <div class="container">
                <div class="row" style="margin-bottom: 10px">
                    <div Class="col-xl-8">
                        <div Class="card mb-4">
                            @Using (Html.BeginForm("CreatePurchaseOrder", "Manager"))
                                @Html.AntiForgeryToken()
                                @<div Class="card-body">
                                    <div Class="row gx-3 mb-3">
                                        <div Class="col-md-6">
                                            <Label Class="small mb-1" for="inputOrgName">Ingredient</Label>
                                            <select Class="form-control" name="ingredientId" aria-label="Default select example">
                                                @If Model IsNot Nothing Then
                                                    @For Each item In Model.Item1
                                                        @<option value="@item.id">@item.ingredient_name</option>
                                                    Next
                                                End If
                                            </select>
                                        </div>
                                    </div>
                                    <div Class="row gx-3 mb-3">
                                        <div Class="col-md-6">
                                            <Label Class="small mb-1" for="inputOrgName">Quantity</Label>
                                            <input Class="form-control" id="inputOrgName" name="quantity" type="number" value="">
                                        </div>
                                    </div>
                                    <div Class="row gx-3 mb-3">
                                        <div Class="col-md-6">
                                            <Button Class="btn btn-primary" type="submit" style="margin-top:20px">Add</Button>
                                        </div>
                                    </div>
                                </div>
                            End Using
                        </div>
                    </div>
                </div>
                <div Class="row">
                    <div Class="col-12 mb-3 mb-lg-5">
                        <div Class="overflow-hidden card table-nowrap table-card">
                            <div Class="table-responsive table-wrapper">
                                <Table Class="table mb-0" style="margin-top:10px">
                                    <thead Class="small text-uppercase bg-body text-muted">
                                        <tr>
                                            <th> INGREDIENT NAME</th>
                                            <th> INGREDIENT CATEGORY</th>
                                            <th> QUANTITY</th>
                                            <th Class="text-end">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
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