@ModelType Tuple(Of List(Of RateData), product, rate, RateInfo)
@Code
    ViewData("Title") = "Detail"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<style>
    .card {
        margin-top: 15px;
        width: 295px;
        border: none;
        border-radius: 12px;
    }

    .circle-image img {
        border: 6px solid #fff;
        border-radius: 100%;
        padding: 0px;
        top: -28px;
        position: relative;
        width: 70px;
        height: 70px;
        border-radius: 100%;
        z-index: 1;
        cursor: pointer;
    }

    .dot {
        height: 18px;
        width: 18px;
        border-radius: 50%;
        display: inline-block;
        position: relative;
        border: 3px solid #fff;
        top: -48px;
        left: 186px;
        z-index: 1000;
    }

    .name {
        margin-top: -21px;
        font-size: 18px;
    }

    .fw-500 {
        font-weight: 500 !important;
    }

    .start {
        color: green;
    }

    .stop {
        color: red;
    }

    .rate {
        border-bottom-right-radius: 12px;
        border-bottom-left-radius: 12px;
    }

    .rating {
        display: flex;
        flex-direction: row-reverse;
        justify-content: center
    }

        .rating > input {
            display: none
        }

        .rating > label {
            position: relative;
            width: 1em;
            font-size: 30px;
            font-weight: 300;
            color: #FFD600;
            cursor: pointer
        }

            .rating > label::before {
                content: "\2605";
                position: absolute;
                opacity: 0
            }

            .rating > label:hover:before,
            .rating > label:hover ~ label:before {
                opacity: 1 !important
            }

        .rating > input:checked ~ label:before {
            opacity: 1
        }

        .rating:hover > input:checked ~ label:before {
            opacity: 0.4
        }

    .buttons {
        top: 36px;
        position: relative;
    }

    .rating-submit {
        border-radius: 15px;
        color: #fff;
        height: 49px;
    }

        .rating-submit:hover {
            color: #fff;
        }

    .form-control {
        width: 125%
    }
</style>
@If Model IsNot Nothing Then
    Dim rates As List(Of RateData) = Model.Item1
    Dim rate As rate = Model.Item3
    Dim product As product = Model.Item2
    Dim rateTotal As RateInfo = Model.Item4
    Dim rateRatio As Integer = 5

    If Not Double.IsNaN(rateTotal.ratioRate) Then
        rateRatio = Math.Floor(rateTotal.ratioRate)
    End If

    @<section class="py-5">

        <div Class="float-right" style="margin-top:10px;margin-bottom:10px">
            @Html.ActionLink("Back to shopping", "Product", "Users", Nothing, htmlAttributes:=New With {.class = "btn btn-lg btn-default md-btn-flat mt-2 mr-3"})
        </div>
        <div class="container">
            <div class="row gx-5">
                <aside class="col-lg-6">
                    <div class="border rounded-4 mb-3 d-flex justify-content-center">
                        <a data-fslightbox="mygalley" class="rounded-4" target="_blank" data-type="image" href="https://bootstrap-ecommerce.com/bootstrap5-ecommerce/images/items/detail1/big.webp">
                            <img width="300" height="300" margin: auto;" class="rounded-4 fit" src="@Url.Content("~/Uploads/" & product.image)" />
                        </a>
                    </div>
                </aside>
                <main class="col-lg-6">
                    <div class="ps-lg-3">
                        <h4 class="title text-dark">
                            @product.product_name
                        </h4>
                        <div class="mb-3">
                            <span class="h5">$@product.price</span>
                        </div>
                        <p style="margin-top:10px">
                            @product.description
                        </p>
                        <hr />
                        @Using (Html.BeginForm("AddToCartWithQuantity", "users"))
                            @Html.AntiForgeryToken()
                            @<div Class="row mb-4" style="margin-bottom:10px">
                                <div Class="col-md-4 col-6 mb-3">
                                    <Label Class="mb-2 d-block">Quantity</Label>
                                    <div Class="input-group mb-3" style="width: 170px;">
                                        <input type="number" Class="form-control text-center border border-secondary" name="quantity" value="1" aria-label="Example text with button addon" aria-describedby="button-addon1" />
                                    </div>
                                </div>
                            </div>
                            @<div class="form-inline my-2 my-lg-0" style="margin-bottom:20px;margin-top:10px">
                                @Html.TextBox("productId", product.Id, htmlAttributes:=New With {.class = "form-control mr-sm-2", .style = "display: none;"})
                                <input type="submit" value="Add to cart" class="btn btn-info btn-outline-success my-2 my-sm-0" />
                            </div>
                        End Using
                    </div>
                </main>
            </div>
        </div>
    </section>

    @<div class="container" style="margin-top:20px">

        <div class="row">
            <div class="col-sm-3">
                <div class="rating-block">
                    <h4>Average user rating</h4>
                    @If Not Double.IsNaN(rateTotal.ratioRate) Then
                        @<h2 Class="bold padding-bottom-7">@Math.Round(rateTotal.ratioRate, 1)<small>/ 5</small></h2>
                    Else
                        @<h2 Class="bold padding-bottom-7">5<small>/ 5</small></h2>
                    End If

                    @For i = 1 To 5
                        If i <= rateRatio Then
                            @<Button type="button" Class="btn btn-warning btn-xs" aria-label="Left Align">
                                <span Class="glyphicon glyphicon-star" aria-hidden="true"></span>
                            </Button>
                        Else
                            @<Button type="button" Class="btn btn-default btn-grey btn-xs" aria-label="Left Align">
                                <span Class="glyphicon glyphicon-star" aria-hidden="true"></span>
                            </Button>
                        End If
                    Next
                </div>
            </div>
            <div class="col-sm-3">
                <h4>Rating breakdown</h4>
                <div class="pull-left">
                    <div class="pull-left" style="width:35px; line-height:1;">
                        <div style="height:9px; margin:5px 0;">5 <span class="glyphicon glyphicon-star"></span></div>
                    </div>
                    <div class="pull-left" style="width:180px;">
                        <div class="progress" style="height:9px; margin:8px 0;">
                            <div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="5" aria-valuemin="0" aria-valuemax="5" style="width: 1000%">
                                <span class="sr-only">80% Complete (danger)</span>
                            </div>
                        </div>
                    </div>
                    <div class="pull-right" style="margin-left:10px;">@rateTotal.fiveRate</div>
                </div>
                <div class="pull-left">
                    <div class="pull-left" style="width:35px; line-height:1;">
                        <div style="height:9px; margin:5px 0;">4 <span class="glyphicon glyphicon-star"></span></div>
                    </div>
                    <div class="pull-left" style="width:180px;">
                        <div class="progress" style="height:9px; margin:8px 0;">
                            <div class="progress-bar progress-bar-primary" role="progressbar" aria-valuenow="4" aria-valuemin="0" aria-valuemax="5" style="width: 80%">
                                <span class="sr-only">80% Complete (danger)</span>
                            </div>
                        </div>
                    </div>
                    <div class="pull-right" style="margin-left:10px;">@rateTotal.fourRate</div>
                </div>
                <div class="pull-left">
                    <div class="pull-left" style="width:35px; line-height:1;">
                        <div style="height:9px; margin:5px 0;">3 <span class="glyphicon glyphicon-star"></span></div>
                    </div>
                    <div class="pull-left" style="width:180px;">
                        <div class="progress" style="height:9px; margin:8px 0;">
                            <div class="progress-bar progress-bar-info" role="progressbar" aria-valuenow="3" aria-valuemin="0" aria-valuemax="5" style="width: 60%">
                                <span class="sr-only">80% Complete (danger)</span>
                            </div>
                        </div>
                    </div>
                    <div class="pull-right" style="margin-left:10px;">@rateTotal.threeRate</div>
                </div>
                <div class="pull-left">
                    <div class="pull-left" style="width:35px; line-height:1;">
                        <div style="height:9px; margin:5px 0;">2 <span class="glyphicon glyphicon-star"></span></div>
                    </div>
                    <div class="pull-left" style="width:180px;">
                        <div class="progress" style="height:9px; margin:8px 0;">
                            <div class="progress-bar progress-bar-warning" role="progressbar" aria-valuenow="2" aria-valuemin="0" aria-valuemax="5" style="width: 40%">
                                <span class="sr-only">80% Complete (danger)</span>
                            </div>
                        </div>
                    </div>
                    <div class="pull-right" style="margin-left:10px;">@rateTotal.twoRate</div>
                </div>
                <div class="pull-left">
                    <div class="pull-left" style="width:35px; line-height:1;">
                        <div style="height:9px; margin:5px 0;">1 <span class="glyphicon glyphicon-star"></span></div>
                    </div>
                    <div class="pull-left" style="width:180px;">
                        <div class="progress" style="height:9px; margin:8px 0;">
                            <div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="1" aria-valuemin="0" aria-valuemax="5" style="width: 20%">
                                <span class="sr-only">80% Complete (danger)</span>
                            </div>
                        </div>
                    </div>
                    <div class="pull-right" style="margin-left:10px;">@rateTotal.oneRate</div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-7">
                <hr />
                @For Each item In rates
                    @<div Class="review-block">
                        <div Class="row">
                            <div Class="col-sm-3">
                                <img width="100" height="100" src="@Url.Content("~/Uploads/" & item.user.image)" Class="img-rounded">
                                <div Class="review-block-name">@item.user.fullName</div>
                                <div Class="review-block-date">@item.rate.register_time.ToShortDateString()</div>
                            </div>
                            <div Class="col-sm-9">
                                <div Class="review-block-rate">
                                    @For i = 1 To 5
                                        If i <= item.rate.star Then
                                            @<Button type="button" Class="btn btn-warning btn-xs" aria-label="Left Align">
                                                <span Class="glyphicon glyphicon-star" aria-hidden="true"></span>
                                            </Button>
                                        Else
                                            @<Button type="button" Class="btn btn-default btn-grey btn-xs" aria-label="Left Align">
                                                <span Class="glyphicon glyphicon-star" aria-hidden="true"></span>
                                            </Button>
                                        End If
                                    Next
                                </div>
                                <div Class="review-block-title">@item.rate.comment</div>
                            </div>
                        </div>
                        <hr />
                    </div>
                Next

            </div>
        </div>



    </div>
    @<div Class="container d-flex justify-content-center mt-5" style="margin-top: 20px">
        <div Class="card text-center mb-5">
            <h3> Your Rate</h3>
            <div Class="rate py-3 text-white mt-3">
                @Using (Html.BeginForm("CreateRate", "Users"))
                    @Html.AntiForgeryToken()
                    @<input type="hidden" name="productId" value="@product.Id" />
                    @<div Class="rating">
                        @If rate IsNot Nothing Then
                            @For i = 5 To 1 Step -1

                                If i = rate.star Then
                                    @<input type="radio" name="rating" checked value="@rate.star" id="@rate.star">
                                    @<Label for="@rate.star">☆</Label>
                                Else
                                    @<input type="radio" name="rating" value="@rate.star" id="@rate.star">
                                    @<Label for="@rate.star">☆</Label>
                                End If

                            Next
                        Else
                            @For i = 5 To 1 Step -1
                                @<input type="radio" name="rating" value="@i" id="@i">
                                @<Label for="@i">☆</Label>
                            Next
                        End If
                    </div>
                    @If rate IsNot Nothing Then
                        @<div Class="col-sm-10" style="margin-top:10px">
                            <textarea rows="5" Class="form-control" name="comment">@rate.comment</textarea>
                        </div>
                    Else
                        @<div Class="col-sm-10" style="margin-top:10px">
                            <textarea rows="5" Class="form-control" name="comment"></textarea>
                        </div>
                    End If
                    @<div Class="buttons">
                        <Button Class="btn btn-warning btn-block rating-submit" type="submit">Submit</Button>
                    </div>
                End Using
            </div>
        </div>
    </div>
End If