@ModelType Project_VB_Net_MVC.user
@Code
    ViewData("Title") = "Delete"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<div>
    <h4>user</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>
            @Html.DisplayNameFor(Function(model) model.full_name)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.full_name)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.password)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.password)
        </dd>

        <dt>
            @Html.DisplayNameFor(Function(model) model.email)
        </dt>

        <dd>
            @Html.DisplayFor(Function(model) model.email)
        </dd>

    </dl>
    @Using (Html.BeginForm())
        @Html.AntiForgeryToken()

        @<div class="form-actions no-color">
            <input type="submit" value="Delete" class="btn btn-default" /> |
            @Html.ActionLink("Back to List", "Index")
        </div>
    End Using
</div>
