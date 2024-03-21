@ModelType Tuple(Of List(Of order_detail), user, user_info)
@Code
    ViewData("Title") = "User"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="container-xl px-4 mt-4">
    <div class="row">
        @If Model IsNot Nothing Then

            Dim orderDetails As List(Of order_detail) = Model.Item1
            Dim user As user = Model.Item2
            Dim userInfo As user_info = Model.Item3

            @<div Class="col-xl-4">
                <div Class="card mb-4 mb-xl-0">
                    <div Class="card-body text-center">
                        <img Class="img-account-profile rounded-circle mb-2" src="@ViewBag.ImagePath" width="200" height="200" alt="">
                        <div Class="input-group">
                            @Using Html.BeginForm("Upload", "users", FormMethod.Post, New With {.enctype = "multipart/form-data"})
                                @<input type="submit" value="Upload" Class="btn btn-outline-secondary" />
                                @<input type="file" name="file" Class="form-control" />
                                @Code
                                    Dim imageUpload = TempData("imageUpload")
                                End Code
                                @If imageUpload IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @imageUpload
                                    </p>
                                End If
                            End Using
                        </div>
                    </div>
                </div>
            </div>
            @<div Class="col-xl-8">
                <div Class="card mb-4">
                    <div Class="card-body">
                        @Using (Html.BeginForm("UpdateInfoUser", "users"))
                            @Html.AntiForgeryToken()
                            @<div Class="mb-3">
                                <Label Class="small mb-1" for="inputUsername">Full Name</Label>
                                <input Class="form-control" id="inputUsername" disabled type="text" value="@user.full_name">
                            </div>
                            @<div Class="row gx-3 mb-3">
                                <div Class="col-md-6">
                                    <Label Class="small mb-1" for="inputFirstName">Email</Label>
                                    <input Class="form-control" id="inputFirstName" disabled type="text" value="@user.email">
                                </div>
                                <div Class="col-md-6">
                                    <Label Class="small mb-1" for="inputLastName">Address</Label>
                                    <input Class="form-control" id="inputLastName" type="text" name=address value="@userInfo.address">
                                    @Code
                                        Dim addressUpdate = TempData("addressUpdate")
                                    End Code
                                    @If addressUpdate IsNot Nothing Then
                                        @<p style="color:red; margin: 10px 0 0 0">
                                            @addressUpdate
                                        </p>
                                    End If
                                    <Button Class="btn btn-primary" type="submit" style="margin-top:20px">Save changes</Button>
                                </div>
                            </div>
                            @<div Class="row gx-3 mb-3">
                                <div Class="col-md-6">
                                    <Label Class="small mb-1" for="inputOrgName">Phone Number</Label>
                                    <input Class="form-control" id="inputOrgName" name="phone" type="tel" value="@userInfo.phone">
                                    @Code
                                        Dim phoneUpdate = TempData("phoneUpdate")
                                    End Code
                                    @If phoneUpdate IsNot Nothing Then
                                        @<p style="color:red; margin: 10px 0 0 0">
                                            @phoneUpdate
                                        </p>
                                    End If
                                </div>
                            </div>
                            @<div Class="row gx-3 mb-3">
                                <div Class="col-md-6">
                                    <Label Class="small mb-1" for="inputBirthday">Birthday</Label>
                                    @If userInfo.birth_day IsNot Nothing Then
                                        @<input Class="form-control" id="inputBirthday" type="date"  name="birthday" value="@userInfo.birth_day.Value.ToString("MM/dd/yyyy")">
                                    Else
                                        @<input Class="form-control" id="inputBirthday" type="date" name="birthday" value="@Date.Now().ToString("MM/dd/yyyy")">
                                    End If
                                </div>
                            </div>
                        End Using
                    </div>
                </div>
            </div>
        End If

    </div>
</div>

<div Class="float-right" style="margin-top:10px">
    @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
</div>
