@Code
    ViewData("Title") = "CreatePost"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
<style>
    input,textarea, select {
        max-width : 100%
    }
</style>
<div class="container-fluid bootstrap snippets bootdeys">
    <div class="row">
        @Using Html.BeginForm("CreatePost", "Manager", FormMethod.Post, New With {.enctype = "multipart/form-data"})
            @Html.AntiForgeryToken()
            @<div Class="col-xs-12 col-sm-12">
                <div Class="panel panel-default">
                    <div Class="panel-heading">
                        <h4 Class="panel-title">Post Info</h4>
                    </div>
                    <div Class="panel-body">
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Title</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <textarea rows="5" Class="form-control" name="title" style="width:100%"></textarea>
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Content</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <textarea rows="10" Class="form-control" name="content"></textarea>
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Ingredient</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <textarea rows="13" Class="form-control" name="ingredient"></textarea>
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Process</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <textarea rows="10" Class="form-control" name="process"></textarea>
                            </div>
                        </div>
                        <div Class="form-group">
                            <Label Class="col-sm-2 control-label">Image</Label>
                            <div Class="col-sm-10" style="margin-top:10px">
                                <input type="file" name="file" Class="form-control" />
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
        @Html.ActionLink("Back to list", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
    </div>
</div>

