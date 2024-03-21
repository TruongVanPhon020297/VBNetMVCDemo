@ModelType Tuple(Of List(Of custom_order_notification), List(Of notification))

@Code
    ViewData("Title") = "Notification"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code


<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/MaterialDesign-Webfont/7.2.96/css/materialdesignicons.min.css" integrity="sha512-LX0YV/MWBEn2dwXCYgQHrpa9HJkwB+S+bnBpifSOTO1No27TqNMKYoAn6ff2FBh03THAzAiiCwQ+aPX+/Qt/Ow==" crossorigin="anonymous" referrerpolicy="no-referrer" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <style>

        body {
            margin-top: 20px;
            background: #f7f8fa
        }

        .avatar-xxl {
            height: 7rem;
            width: 7rem;
        }

        .card {
            margin-bottom: 20px;
            -webkit-box-shadow: 0 2px 3px #eaedf2;
            box-shadow: 0 2px 3px #eaedf2;
        }

        .pb-0 {
            padding-bottom: 0 !important;
        }

        .font-size-16 {
            font-size: 16px !important;
        }

        .avatar-title {
            -webkit-box-align: center;
            -ms-flex-align: center;
            align-items: center;
            background-color: #038edc;
            color: #fff;
            display: -webkit-box;
            display: -ms-flexbox;
            display: flex;
            font-weight: 500;
            height: 100%;
            -webkit-box-pack: center;
            -ms-flex-pack: center;
            justify-content: center;
            width: 100%;
        }

        .bg-soft-primary {
            background-color: rgba(3,142,220,.15) !important;
        }

        .rounded-circle {
            border-radius: 50% !important;
        }

        .nav-tabs-custom .nav-item .nav-link.active {
            color: #038edc;
        }

        .nav-tabs-custom .nav-item .nav-link {
            border: none;
        }

            .nav-tabs-custom .nav-item .nav-link.active {
                color: #038edc;
            }

        .avatar-group {
            display: -webkit-box;
            display: -ms-flexbox;
            display: flex;
            -ms-flex-wrap: wrap;
            flex-wrap: wrap;
            padding-left: 12px;
        }

        .border-end {
            border-right: 1px solid #eff0f2 !important;
        }

        .d-inline-block {
            display: inline-block !important;
        }

        .badge-soft-danger {
            color: #f34e4e;
            background-color: rgba(243,78,78,.1);
        }

        .badge-soft-warning {
            color: #f7cc53;
            background-color: rgba(247,204,83,.1);
        }

        .badge-soft-success {
            color: #51d28c;
            background-color: rgba(81,210,140,.1);
        }

        .avatar-group .avatar-group-item {
            margin-left: -14px;
            border: 2px solid #fff;
            border-radius: 50%;
            -webkit-transition: all .2s;
            transition: all .2s;
        }

        .avatar-sm {
            height: 2rem;
            width: 2rem;
        }

        .nav-tabs-custom .nav-item {
            position: relative;
            color: #343a40;
        }

            .nav-tabs-custom .nav-item .nav-link.active:after {
                -webkit-transform: scale(1);
                transform: scale(1);
            }

            .nav-tabs-custom .nav-item .nav-link::after {
                content: "";
                background: #038edc;
                height: 2px;
                position: absolute;
                width: 100%;
                left: 0;
                bottom: -2px;
                -webkit-transition: all 250ms ease 0s;
                transition: all 250ms ease 0s;
                -webkit-transform: scale(0);
                transform: scale(0);
            }

        .badge-soft-secondary {
            color: #74788d;
            background-color: rgba(116,120,141,.1);
        }

        .badge-soft-secondary {
            color: #74788d;
        }

        .work-activity {
            position: relative;
            color: #74788d;
            padding-left: 5.5rem
        }

            .work-activity::before {
                content: "";
                position: absolute;
                height: 100%;
                top: 0;
                left: 66px;
                border-left: 1px solid rgba(3,142,220,.25)
            }

            .work-activity .work-item {
                position: relative;
                border-bottom: 2px dashed #eff0f2;
                margin-bottom: 14px
            }

                .work-activity .work-item:last-of-type {
                    padding-bottom: 0;
                    margin-bottom: 0;
                    border: none
                }

                .work-activity .work-item::after, .work-activity .work-item::before {
                    position: absolute;
                    display: block
                }

                .work-activity .work-item::before {
                    content: attr(data-date);
                    left: -157px;
                    top: -3px;
                    text-align: right;
                    font-weight: 500;
                    color: #74788d;
                    font-size: 12px;
                    min-width: 120px
                }

                .work-activity .work-item::after {
                    content: "";
                    width: 10px;
                    height: 10px;
                    border-radius: 50%;
                    left: -26px;
                    top: 3px;
                    background-color: #fff;
                    border: 2px solid #038edc
                }
    </style>
</head>
<body>
    <div class="container">
        <div class="row">
            <div class="col-xl-8">
                <div class="card">
                    <div class="tab-content p-4">
                        <div class="tab-pane active show" id="tasks-tab" role="tabpanel">
                            <h4 class="card-title mb-4">Notifications</h4>
                            @using (Html.BeginForm("ConfirmNotificationOrder", "Users", FormMethod.Post, htmlAttributes:=New With {.id = "myForm"}))
                                @Html.AntiForgeryToken()
                                @For Each item In Model.Item1
                                    @<div Class="row">
                                        <div Class="col-xl-12">
                                            <div Class="task-list-box" id="landing-task">
                                                <div id="task-item-1">
                                                    <div class="card task-box rounded-3">
                                                        <div class="card-body">
                                                            <div class="row align-items-center">
                                                                <div class="col-xl-3 col-sm-3">
                                                                    <div class="checklist form-check font-size-15">

                                                                        @If item.status Then
                                                                            @<input type="checkbox" checked disabled Class="form-check-input">
                                                                        Else
                                                                            @<input type="checkbox" name="notificationId" value="@item.id" onclick="submitForm()" Class="form-check-input">
                                                                        End If
                                                                        <label class="form-check-label ms-1 task-title" for="customCheck1">
                                                                            @item.register_date
                                                                        </label>
                                                                    </div>
                                                                </div><!-- end col -->
                                                                <div class="col-xl-9 col-sm-9">
                                                                    <div class="row align-items-center">
                                                                        <div class="col-xl-9 col-md-9 col-sm-9">
                                                                            <div class="avatar-group mt-3 mt-xl-0 task-assigne">
                                                                                @item.content_notification
                                                                            </div>
                                                                        </div><!-- end col -->
                                                                        <div class="col-xl-3 col-md-3 col-sm-3">
                                                                            <div class="d-flex flex-wrap gap-3 mt-3 mt-xl-0 justify-content-md-end">
                                                                                @If item.is_custom_order Then
                                                                                    @<div>
                                                                                        <a href="http://localhost:53005/Users/CustomOrderDetailInfo/@item.order_id" Class="mb-0 text-muted fw-medium" data-bs-toggle="modal" data-bs-target=".bs-example-new-task"><i Class="mdi mdi-square-edit-outline font-size-16 align-middle" onclick="editTask('task-item-1')"></i></a>
                                                                                    </div>
                                                                                Else
                                                                                    @<div>
                                                                                        <a href="http://localhost:53005/Users/OrderDetail/@item.order_id" Class="mb-0 text-muted fw-medium" data-bs-toggle="modal" data-bs-target=".bs-example-new-task"><i Class="mdi mdi-square-edit-outline font-size-16 align-middle" onclick="editTask('task-item-1')"></i></a>
                                                                                    </div>
                                                                                End If


                                                                                <div>
                                                                                    <a href="http://localhost:53005/Users/DeleteNotificationOrder?id=@item.id" Class="delete-item" onclick="deleteProjects('task-item-1')">
                                                                                        <i Class="mdi mdi-trash-can-outline align-middle font-size-16 text-danger"></i>
                                                                                    </a>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                Next
                            End Using
                            @using (Html.BeginForm("ConfirmNotification", "Users", FormMethod.Post, htmlAttributes:=New With {.id = "myFormNotification"}))
                                @Html.AntiForgeryToken()
                                @For Each item In Model.Item2
                                    @<div Class="row">
                                        <div Class="col-xl-12">
                                            <div Class="task-list-box" id="landing-task">
                                                <div id="task-item-1">
                                                    <div class="card task-box rounded-3">
                                                        <div class="card-body">
                                                            <div class="row align-items-center">
                                                                <div class="col-xl-3 col-sm-3">
                                                                    <div class="checklist form-check font-size-15">

                                                                        @If item.status Then
                                                                            @<input type="checkbox" checked disabled Class="form-check-input">
                                                                        Else
                                                                            @<input type="checkbox" name="notificationId" value="@item.id" onclick="submitNotification()" Class="form-check-input">
                                                                        End If
                                                                        <label class="form-check-label ms-1 task-title" for="customCheck1">
                                                                            @item.register_time
                                                                        </label>
                                                                    </div>
                                                                </div><!-- end col -->
                                                                <div class="col-xl-9 col-sm-9">
                                                                    <div class="row align-items-center">
                                                                        <div class="col-xl-9 col-md-9 col-sm-9">
                                                                            <div class="avatar-group mt-3 mt-xl-0 task-assigne">
                                                                                @item.content_notification
                                                                            </div>
                                                                        </div><!-- end col -->
                                                                        <div class="col-xl-3 col-md-3 col-sm-3">
                                                                            <div class="d-flex flex-wrap gap-3 mt-3 mt-xl-0 justify-content-md-end">
                                                                                @If item.type_of_notification = 1 Then
                                                                                    @<div>
                                                                                        <a href="http://localhost:53005/Users/Detail/@item.type_of_notification_id" Class="mb-0 text-muted fw-medium" data-bs-toggle="modal" data-bs-target=".bs-example-new-task"><i Class="mdi mdi-square-edit-outline font-size-16 align-middle" onclick="editTask('task-item-1')"></i></a>
                                                                                    </div>
                                                                                ElseIf item.type_of_notification = 2 Then
                                                                                    @<div>
                                                                                        <a href="http://localhost:53005/Users/PostDetail/@item.type_of_notification_id" Class="mb-0 text-muted fw-medium" data-bs-toggle="modal" data-bs-target=".bs-example-new-task"><i Class="mdi mdi-square-edit-outline font-size-16 align-middle" onclick="editTask('task-item-1')"></i></a>
                                                                                    </div>
                                                                                Else

                                                                                End If
                                                                                <div>
                                                                                    <a href="http://localhost:53005/Users/DeleteNotification?id=@item.id" Class="delete-item" onclick="deleteProjects('task-item-1')">
                                                                                        <i Class="mdi mdi-trash-can-outline align-middle font-size-16 text-danger"></i>
                                                                                    </a>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                Next
                            End Using
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div Class="float-right" Ifyle="margin-top:10px">
        @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
    </div>
    <script type="text/javascript">
        function submitForm() {
            document.getElementById("myForm").submit();
        }
    </script>
    <script type="text/javascript">
        function submitNotification() {
            document.getElementById("myFormNotification").submit();
        }
    </script>
</body>
</html>





