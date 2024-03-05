
@ModelType Project_VB_Net_MVC.user
@Code
    ViewData("Title") = "Login"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<section class="vh-100" style="background-color: #eee;">
    <div class="container h-100">
        <div class="row d-flex justify-content-center align-items-center h-100">
            <div class="col-lg-12 col-xl-11">
                <div class="card text-black" style="border-radius: 25px;">
                    <div class="card-body p-md-5">
                        <div class="row justify-content-center">
                            <div class="col-md-10 col-lg-6 col-xl-5 order-2 order-lg-1">

                                <p class="text-center h1 fw-bold mb-5 mx-1 mx-md-4 mt-4">Login</p>
                                @Using (Html.BeginForm("Login", "users"))
                                    @Html.AntiForgeryToken()
                                    @<div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="control-label col-md-2">
                                                Email
                                            </label>
                                            <div class="col-md-10">
                                                <input type="text" name="email" class="form-control" />
                                                @Code
                                                    Dim email = TempData("emailError")
                                                End Code
                                                @If email IsNot Nothing Then
                                                    @<p style="color:red; margin: 10px 0 0 0">
                                                        @email
                                                    </p>
                                                End If
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="control-label col-md-2">
                                                Password
                                            </label>
                                            <div class="col-md-10">
                                                <input type="password" name="password" class="form-control" />
                                                @Code
                                                    Dim password = TempData("passwordError")
                                                End Code
                                                @If password IsNot Nothing Then
                                                    @<p style="color:red; margin: 10px 0 0 0">
                                                        @password
                                                    </p>
                                                End If
                                            </div>
                                        </div>
                                        @Code
                                            Dim login = TempData("login")
                                        End Code
                                        @If login IsNot Nothing Then
                                            @<div Class="form-group">
                                                <div class="col-md-10">
                                                    <p style="color:red; margin: 10px 0 0 0">
                                                        @login
                                                    </p>
                                                </div>
                                            </div>
                                        End If
                                        <div class="form-group">
                                            <div class="col-md-offset-2 col-md-10">
                                                <input type="submit" value="Login" class="btn btn-primary btn-primary" />
                                            </div>
                                        </div>
                                    </div>
                                End Using
                            </div>
                            <div Class="col-md-10 col-lg-6 col-xl-7 d-flex align-items-center order-1 order-lg-2">
                                <img src="https://img.freepik.com/free-vector/login-concept-illustration_114360-739.jpg" style="width:300px;height:300px"
                                     Class="img-fluid mw-100 mh-100" alt="Sample image">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
<div style="margin-top: 10px">
    @Html.ActionLink("REGISTER", "Register", "users", Nothing, New With {.class = "btn btn-info"})
</div>
