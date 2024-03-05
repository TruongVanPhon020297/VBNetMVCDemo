@Code
    ViewData("Title") = "CreateUser"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
<div class="container bootstrap snippets bootdeys">
    <div class="row">
        @Using Html.BeginForm("CreateUserData", "Manager", FormMethod.Post, New With {.enctype = "multipart/form-data"})
            @Html.AntiForgeryToken()
            @<div Class="col-xs-12 col-sm-9">
                @Html.ActionLink("USER LIST", "UserPage", "Manager", Nothing, New With {.class = "btn btn-info"})
                <div Class="panel panel-default" style="margin:10px">
                    <div Class="input-group panel-body text-center">
                        <input type="file" name="file" Class="form-control" />
                        @Code
                            Dim fileCreateUser = TempData("fileCreateUser")
                        End Code
                        @If fileCreateUser IsNot Nothing Then
                            @<p style="color:red; margin: 10px 0 0 0">
                                @fileCreateUser
                            </p>
                        End If
                    </div>
                </div>
                <div Class="panel panel-default">
                    <div Class="panel-heading">
                        <h4 Class="panel-title">User Info</h4>
                    </div>
                    <div Class="panel-body">
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Full Name</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input type="text" name="fullName" Class="form-control" value="">
                                @Code
                                    Dim fullNameCreate = TempData("fullNameCreate")
                                End Code
                                @If fullNameCreate IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @fullNameCreate
                                    </p>
                                End If
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Email</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input type="email" name="email" Class="form-control" value="">
                                @Code
                                    Dim emailCreate = TempData("emailCreate")
                                End Code
                                @If emailCreate IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @emailCreate
                                    </p>
                                End If
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Password</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input type="password" name="password" Class="form-control" value="">
                                @Code
                                    Dim passwordCreate = TempData("passwordCreate")
                                End Code
                                @If passwordCreate IsNot Nothing Then
                                    @<p style="color:red; margin: 10px 0 0 0">
                                        @passwordCreate
                                    </p>
                                End If
                            </div>
                        </div>

                        <div Class="form-group">
                            <div Class="col-sm-10 col-sm-offset-2" style="margin-top:10px">
                                <select class="form-control" name="manager" aria-label="Default select example">
                                    <option value="0">User</option>
                                    <option value="1">Manager</option>
                                </select>
                            </div>
                        </div>
                        <div Class="form-group">
                            <div Class="col-sm-10 col-sm-offset-2" style="margin-top:10px">
                                <Button type="submit" Class="btn btn-primary">Create</Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        End Using
    </div>
</div>

