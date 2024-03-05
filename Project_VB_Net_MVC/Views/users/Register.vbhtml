@ModelType Project_VB_Net_MVC.user
@Code
    ViewData("Title") = "Register"
End Code

<section class="vh-100" style="background-color: #eee;">
    <div class="container h-100">
        <div class="row d-flex justify-content-center align-items-center h-100">
            <div class="col-lg-12 col-xl-11">
                <div class="card text-black" style="border-radius: 25px;">
                    <div class="card-body p-md-5">
                        <div class="row justify-content-center">
                            <div class="col-md-10 col-lg-6 col-xl-5 order-2 order-lg-1">
                                <p class="text-center h1 fw-bold mb-5 mx-1 mx-md-4 mt-4">Sign up</p>
                                @Using (Html.BeginForm("Register", "users"))
                                    @Html.AntiForgeryToken()
                                    @<div class="form-horizontal">
                                        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                                        <div class="form-group">
                                            <label class="control-label col-md-2">
                                                Full Name
                                            </label>
                                            <div class="col-md-10">
                                                @Html.EditorFor(Function(model) model.full_name, New With {.htmlAttributes = New With {.class = "form-control"}})
                                                @Code
                                                    Dim fullName = TempData("fullName")
                                                End Code
                                                @If fullName IsNot Nothing Then
                                                    @<p style="color:red; margin: 10px 0 0 0">
                                                        @fullName
                                                    </p>
                                                End If
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-md-2">
                                                Password
                                            </label>
                                            <div class="col-md-10">
                                                @Html.EditorFor(Function(model) model.password, New With {.htmlAttributes = New With {.class = "form-control", .type = "password"}})
                                                @Code
                                                    Dim password = TempData("password")
                                                End Code
                                                @If password IsNot Nothing Then
                                                    @<p style="color:red; margin: 10px 0 0 0">
                                                        @password
                                                    </p>
                                                End If
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-md-2">
                                                Email
                                            </label>
                                            <div class="col-md-10">
                                                @Html.EditorFor(Function(model) model.email, New With {.htmlAttributes = New With {.class = "form-control"}})
                                                @Code
                                                    Dim email = TempData("email")
                                                End Code
                                                @If email IsNot Nothing Then
                                                    @<p style="color:red; margin: 10px 0 0 0">
                                                        @email
                                                    </p>
                                                End If
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-offset-2 col-md-10">
                                                <input type="submit" value="CREATE" class="btn btn-primary btn-primary" />
                                            </div>
                                        </div>
                                    </div>
                                                    End Using
                            </div>
                            <div class="col-md-10 col-lg-6 col-xl-7 d-flex align-items-center order-1 order-lg-2">
                                <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-registration/draw1.webp" style="width:500px;height:auto"
                                     class="img-fluid mw-100 mh-100" alt="Sample image">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
<div style="margin-top: 10px">
    @Html.ActionLink("LOGIN", "Login", "users", Nothing, New With {.class = "btn btn-info"})
</div>