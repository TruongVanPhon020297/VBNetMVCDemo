Imports System.Web.Mvc
Imports System.Data.Entity
Namespace Controllers
    Public Class ManagerController
        Inherits System.Web.Mvc.Controller

        Private db As New DBNetEntitiesData

        Enum NotificationType
            Product = 1
            Post = 2
            User = 3
        End Enum

        ' GET: Manager
        Function HomePage() As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Return View("~/Views/managers/Home.vbhtml")
        End Function

        Function ProductPage() As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If
            Dim products As List(Of product) = New List(Of product)
            products = db.product.ToList()
            Return View("~/Views/managers/Product.vbhtml", products)
        End Function

        Function Edit(ByVal id As Integer?) As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If
            Dim product As product = Nothing
            product = db.product.Find(id)

            ViewBag.ImagePath = "http://bootdey.com/img/Content/avatar/avatar1.png"

            If product.image IsNot Nothing Then
                Dim imageUrl As String = Url.Content("~/Uploads/" & product.image)
                ViewBag.ImagePath = imageUrl
            End If

            Return View("~/Views/managers/Edit.vbhtml", product)
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(productId As Integer, productName As String, price As String, description As String) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim product As product = New product()
            product = db.product.Find(productId)

            product.product_name = productName
            product.price = price
            product.description = description

            db.Entry(product).State = EntityState.Modified
            db.SaveChanges()

            Return RedirectToAction("ProductPage")
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken()>
        <ActionName("Upload")>
        Function Upload(file As HttpPostedFileBase, productId As Integer) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            If file IsNot Nothing AndAlso file.ContentLength > 0 Then
                Dim fileName As String = System.IO.Path.GetFileName(file.FileName)
                Dim path As String = Server.MapPath("~/Uploads/" & fileName)
                file.SaveAs(path)

                Dim product As product = New product()
                product = db.product.Find(productId)
                product.image = fileName

                db.Entry(product).State = EntityState.Modified
                db.SaveChanges()

            End If
            Return RedirectToRoute(New With {.controller = "Manager", .action = "Edit", .id = productId})
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Delete(productId As Integer) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim product As product = New product()
            product = db.product.Find(productId)

            db.product.Remove(product)
            db.SaveChanges()

            Return RedirectToAction("ProductPage")
        End Function

        Function CreateProduct() As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim categories As List(Of category) = New List(Of category)
            categories = db.category.ToList()

            Return View("~/Views/managers/CreateProduct.vbhtml", categories)
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken()>
        Function CreateProduct(file As HttpPostedFileBase, productName As String, price As Decimal, description As String, categoryId As Integer) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            If file IsNot Nothing AndAlso file.ContentLength > 0 Then

                If productName.Length = 0 Then
                    TempData("productNameCreate") = "Product name is not blank"
                    Return RedirectToAction("CreateProduct")
                End If

                If description.Length = 0 Then
                    TempData("descriptionCreate") = "Description is not blank"
                    Return RedirectToAction("CreateProduct")
                End If

                Dim fileName As String = System.IO.Path.GetFileName(file.FileName)
                Dim path As String = Server.MapPath("~/Uploads/" & fileName)
                file.SaveAs(path)

                Dim product As product = New product With {
                    .product_name = productName,
                    .image = fileName,
                    .price = price,
                    .is_deleted = False,
                    .description = description,
                    .category_id = categoryId
                }

                db.product.Add(product)
                db.SaveChanges()


                Dim users As List(Of user) = Nothing
                users = (From u In db.user
                         Where u.manager = False
                         Select u).ToList()

                Dim notifications As List(Of notification) = New List(Of notification)

                Dim productResult As product = db.product.OrderByDescending(Function(x) x.Id).FirstOrDefault()

                For Each item In users

                    Dim notification As notification = New notification With {
                            .user_id = item.id,
                            .register_time = DateTime.Now(),
                            .status = False,
                            .type_of_notification = NotificationType.Product,
                            .type_of_notification_id = productResult.Id,
                            .content_notification = "Chúng tôi vô cùng hạnh phúc thông báo về sản phẩm mới - " &
                            productResult.product_name &
                            ". Chúng tôi tin rằng sản phẩm này sẽ mang lại trải nghiệm tuyệt vời cho bạn. Hãy khám phá ngay!"
                    }

                    notifications.Add(notification)
                Next

                db.notification.AddRange(notifications)
                db.SaveChanges()

                Return RedirectToAction("ProductPage")
            Else
                TempData("fileCreate") = "File is not blank"
                Return RedirectToAction("CreateProduct")
            End If

        End Function

        Function ListUser() As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim userDataList As List(Of UserData) = New List(Of UserData)
            Dim users As List(Of user) = New List(Of user)
            users = (From u In db.user
                     Where u.manager = False
                     Select u).ToList()

            For Each item In users

                Dim userData As UserData = New UserData()
                Dim userInfo As user_info = New user_info()
                userInfo = (From u In db.user_info
                            Where u.user_id = item.id).FirstOrDefault()

                userData.email = item.email
                userData.fullName = item.full_name
                userData.address = userInfo.address
                userData.image = userInfo.image
                userData.phone = userInfo.phone
                If userInfo.birth_day IsNot Nothing Then
                    userData.birthDay = userInfo.birth_day
                End If
                userData.id = item.id
                userDataList.Add(userData)

            Next

            Return View("~/Views/managers/User.vbhtml", userDataList)
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function DeleteUser(userId As Integer) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim user As user = New user()
            user = db.user.Find(userId)

            db.user.Remove(user)

            Dim userInfo As user_info = New user_info()
            userInfo = (From u In db.user_info
                        Where u.user_id = userId
                        Select u).FirstOrDefault()

            db.user_info.Remove(userInfo)

            db.SaveChanges()

            Return RedirectToAction("ListUser")

        End Function

        Function OrderPage() As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If
            Dim orders As List(Of order) = Nothing
            orders = db.order.OrderBy(Function(item) item.status).ThenBy(Function(item) item.register_time).ToList()
            Return View("~/Views/managers/Order.vbhtml", orders)
        End Function

        Function OrderDetailPage(id As Integer) As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim orderInfo As order = New order()
            orderInfo = db.order.Find(id)

            Dim orderDetails As List(Of order_detail) = New List(Of order_detail)
            orderDetails = (From o In db.order_detail
                            Where o.order_id = orderInfo.id
                            Select o).ToList()

            Dim userInfo = db.user.Find(orderInfo.user_id)

            Dim delivery As delivery = New delivery()
            delivery = db.delivery.Find(orderInfo.delivery_id)

            Dim tupleData As New Tuple(Of List(Of order_detail), order, user, delivery)(orderDetails, orderInfo, userInfo, delivery)

            Return View("~/Views/managers/OrderDetail.vbhtml", tupleData)

        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function ConfirmOrder(orderId As Integer, deliveryDate As Date) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim orderInfo As order = New order()
            orderInfo = db.order.Find(orderId)
            orderInfo.status = True

            db.Entry(orderInfo).State = EntityState.Modified

            Dim delivery As delivery = New delivery()
            delivery = db.delivery.Find(orderInfo.delivery_id)
            delivery.delivery_date = deliveryDate

            db.Entry(delivery).State = EntityState.Modified
            db.SaveChanges()

            Dim orderDetails As List(Of order_detail) = Nothing
            orderDetails = (From o In db.order_detail
                            Where o.order_id = orderInfo.id
                            Select o).ToList()

            For Each item In orderDetails

                Dim purchasedProduct As purchased_product = Nothing
                purchasedProduct = (From p In db.purchased_product
                                    Where p.product_id = item.product_id And p.user_id = orderInfo.user_id
                                    Select p).FirstOrDefault()

                If purchasedProduct Is Nothing Then

                    purchasedProduct = New purchased_product With {
                        .product_id = item.product_id,
                        .user_id = orderInfo.user_id
                    }

                    db.purchased_product.Add(purchasedProduct)
                    db.SaveChanges()

                End If

            Next

            Dim notification As custom_order_notification = New custom_order_notification With {
                .user_id = orderInfo.user_id,
                .status = False,
                .order_id = orderInfo.id,
                .content_notification = "Chúng tôi xin trân trọng thông báo rằng đơn hàng đã đặt giao vào ngày " & orderInfo.register_time.ToShortDateString() & " của bạn đã được xác nhận và sẵn sàng để giao hàng.",
                .register_date = DateTime.Now(),
                .is_custom_order = False
            }

            db.custom_order_notification.Add(notification)
            db.SaveChanges()

            Return RedirectToAction("OrderPage")

        End Function

        Function CreateUser() As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If
            Return View("~/Views/managers/CreateUser.vbhtml")
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function CreateUserData(file As HttpPostedFileBase, fullName As String, email As String, password As String, manager As Integer) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            If file IsNot Nothing AndAlso file.ContentLength > 0 Then

                If fullName.Length = 0 Then
                    TempData("fullNameCreate") = "Full name is not blank"
                    Return RedirectToAction("CreateUser")
                End If

                If email.Length = 0 Then
                    TempData("emailCreate") = "Email is not blank"
                    Return RedirectToAction("CreateUser")
                End If

                If password.Length = 0 Then
                    TempData("passwordCreate") = "Password is not blank"
                    Return RedirectToAction("CreateUser")
                End If


                Dim userCheck As user = New user()
                userCheck = (From u In db.user
                             Where u.email = email
                             Select u).FirstOrDefault()

                If userCheck Is Nothing Then

                    Dim fileName As String = System.IO.Path.GetFileName(file.FileName)
                    Dim path As String = Server.MapPath("~/Uploads/" & fileName)
                    file.SaveAs(path)

                    Dim isManager As Boolean = True

                    If (manager = 0) Then
                        isManager = False
                    End If

                    Dim user As user = New user With {
                       .full_name = fullName,
                       .email = email,
                       .manager = isManager,
                       .password = password
                    }

                    db.user.Add(user)
                    db.SaveChanges()

                    Dim userResult As user = New user()
                    userResult = (From u In db.user
                                  Where u.email = email
                                  Select u).FirstOrDefault()

                    Dim userInfo As user_info = New user_info With {
                        .user_id = userResult.id,
                        .image = fileName
                    }

                    db.user_info.Add(userInfo)
                    db.SaveChanges()

                    Return RedirectToAction("ListUser")

                Else
                    TempData("emailCreate") = "Email Exists"
                    Return RedirectToAction("CreateUser")
                End If
            Else
                TempData("fileCreateUser") = "File is not blank"
                Return RedirectToAction("CreateUser")
            End If
        End Function

        Function CreateCart() As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim userCookie = Request.Cookies("Manager")
            Dim userId = Decimal.Parse(userCookie.Value)

            Dim cart As cart = New cart()
            Dim cartDetails As List(Of cart_detail) = New List(Of cart_detail)

            Try

                cart = (From c In db.cart
                        Where c.user_id = userId
                        Select c).FirstOrDefault()




                cartDetails = (From d In db.cart_detail
                               Where d.cart_id = cart.id
                               Select d).ToList()
            Catch ex As Exception

            End Try

            Dim products As List(Of product) = New List(Of product)
            products = (From p In db.product
                        Select p).ToList()

            Dim users As List(Of user) = New List(Of user)
            users = (From u In db.user
                     Where u.manager = False
                     Select u).ToList()

            Dim tupleData As New Tuple(Of List(Of product), List(Of user), cart, List(Of cart_detail))(products, users, cart, cartDetails)

            Return View("~/Views/managers/CreateOrder.vbhtml", tupleData)
        End Function


        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function AddProduct(productId As Integer, quantity As Integer) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim userCookie = Request.Cookies("Manager")
            Dim userId = Decimal.Parse(userCookie.Value)



            Dim cart As cart = Nothing
            cart = (From c In db.cart
                    Where c.user_id = userId
                    Select c).FirstOrDefault()


            Dim cartRegister As cart = New cart()

            Dim product As product = db.product.Find(Convert.ToDecimal(productId))

            If IsNothing(cart) Then

                cartRegister.user_id = userId
                cartRegister.quantity = quantity
                cartRegister.total_price = product.price * quantity
                db.cart.Add(cartRegister)
                db.SaveChanges()


                Dim cartResult As cart = Nothing

                cartResult = (From p In db.cart
                              Where p.user_id = userId
                              Select p).FirstOrDefault()


                Dim cartDetailRegister As cart_detail = New cart_detail With {
                    .image = product.image,
                    .product_id = product.Id,
                    .product_name = product.product_name,
                    .quantity = quantity,
                    .total_price = product.price * quantity,
                    .cart_id = cartResult.id,
                    .price = product.price
                }

                db.cart_detail.Add(cartDetailRegister)
                db.SaveChanges()

            Else

                cart.quantity = cart.quantity + quantity
                cart.total_price = cart.total_price + product.price * quantity

                db.Entry(cart).State = EntityState.Modified
                db.SaveChanges()
                Dim cartDetail As cart_detail = (From c In db.cart_detail
                                                 Where c.product_id = product.Id
                                                 Select c).FirstOrDefault()

                If IsNothing(cartDetail) Then

                    Dim cartDetailRegister As cart_detail = New cart_detail With {
                        .image = product.image,
                        .product_id = product.Id,
                        .product_name = product.product_name,
                        .quantity = quantity,
                        .total_price = product.price * quantity,
                        .cart_id = cart.id,
                        .price = product.price
                    }
                    db.cart_detail.Add(cartDetailRegister)
                    db.SaveChanges()
                Else
                    cartDetail.quantity = cartDetail.quantity + quantity
                    cartDetail.total_price = cartDetail.total_price + product.price * quantity
                    db.Entry(cartDetail).State = EntityState.Modified
                    db.SaveChanges()
                End If
            End If
            Return RedirectToAction("CreateCart")

        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Checkout(userId As Integer, address As String, phone As String) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim userCookie = Request.Cookies("Manager")
            Dim userIdCookie = Decimal.Parse(userCookie.Value)

            If address.Length = 0 Then
                TempData("addressCheckout") = "Address is not blank"
                Return RedirectToAction("CreateCart")
            End If

            If phone.Length = 0 Then
                TempData("phoneCheckout") = "Phone is not blank"
                Return RedirectToAction("CreateCart")
            End If

            Dim cart As cart = Nothing

            cart = (From c In db.cart
                    Where c.user_id = userIdCookie
                    Select c).FirstOrDefault()

            Dim delivery As delivery = New delivery With {
                .location = address,
                .phone = phone
            }

            db.delivery.Add(delivery)
            db.SaveChanges()

            Dim deliveryResult As delivery = Nothing
            deliveryResult = (From d In db.delivery
                              Order By d.Id Descending
                              Select d).FirstOrDefault()

            Dim orderRegister = ToOrder(cart)
            orderRegister.delivery_id = deliveryResult.Id
            orderRegister.user_id = userId

            db.order.Add(orderRegister)
            db.SaveChanges()
            db.cart.Remove(cart)
            db.SaveChanges()

            Dim cartDetails As List(Of cart_detail) = Nothing

            cartDetails = (From c In db.cart_detail
                           Where c.cart_id = cart.id
                           Select c).ToList()

            Dim orderResult As order = Nothing
            orderResult = (From o In db.order
                           Where o.user_id = userId
                           Order By o.id Descending
                           Select o).FirstOrDefault()

            Dim orderDetails As List(Of order_detail) = ToOrderDetails(cartDetails, orderResult.id)

            db.order_detail.AddRange(orderDetails)
            db.SaveChanges()

            db.cart_detail.RemoveRange(cartDetails)
            db.SaveChanges()

            Return RedirectToAction("OrderPage")


        End Function

        Function ToOrder(ByVal cart As cart) As order

            Dim order As order = New order()
            order.user_id = cart.user_id
            order.quantity = cart.quantity
            order.total_price = cart.total_price
            order.register_time = DateTime.Now()
            order.status = False

            Return order

        End Function

        Function ToOrderDetails(ByVal cartDetails As List(Of cart_detail), ByVal orderId As String) As List(Of order_detail)

            Dim orderDetails As List(Of order_detail) = New List(Of order_detail)

            For Each item In cartDetails
                Dim orderDetail As order_detail = New order_detail With {
                    .order_id = orderId,
                    .product_id = item.product_id,
                    .image = item.image,
                    .total_price = item.total_price,
                    .quantity = item.quantity,
                    .product_name = item.product_name,
                    .price = item.price
                }

                orderDetails.Add(orderDetail)

            Next

            Return orderDetails

        End Function

        Function UserInfo() As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim userCookie = Request.Cookies("Manager")
            Dim userId = Decimal.Parse(userCookie.Value)

            Dim user As user = db.user.Find(userId)

            Dim userInfoData As user_info = New user_info()
            userInfoData = (From u In db.user_info
                            Where u.user_id = user.id).FirstOrDefault()


            Dim tupleData As New Tuple(Of user, user_info)(user, userInfoData)

            ViewBag.ImagePath = "http://bootdey.com/img/Content/avatar/avatar1.png"

            If userInfoData.image IsNot Nothing Then
                Dim imageUrl As String = Url.Content("~/Uploads/" & userInfoData.image)
                ViewBag.ImagePath = imageUrl
            End If

            Return View("~/Views/managers/UserInfo.vbhtml", tupleData)
        End Function

        Function CustomOrderPage() As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim customOrders As List(Of custom_order) = Nothing
            customOrders = db.custom_order.ToList()

            Return View("~/Views/managers/CustomOrder.vbhtml", customOrders)
        End Function

        Function CustomOrderDetailPage(id As Integer) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim customOrders As custom_order = Nothing
            customOrders = db.custom_order.Find(id)

            Dim category As category = Nothing
            category = db.category.Find(customOrders.category_id)

            Dim tupleData As New Tuple(Of custom_order, category)(customOrders, category)

            Return View("~/Views/managers/CustomOrderDetail.vbhtml", tupleData)
        End Function


        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function ConfirmCustomOrder(orderId As Integer) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim customOrder As custom_order = Nothing
            customOrder = db.custom_order.Find(orderId)

            customOrder.confirm = True

            db.Entry(customOrder).State = EntityState.Modified
            db.SaveChanges()

            Dim notification As custom_order_notification = New custom_order_notification With {
                .user_id = customOrder.user_id,
                .status = False,
                .order_id = customOrder.id,
                .content_notification = "Chúng tôi xin chân thành gửi lời cảm ơn sâu sắc đến Quý vị đã tin tưởng và đặt hàng tại cửa hàng của chúng tôi. 
                Sự ủng hộ của Quý vị không chỉ là một sự khích lệ mà còn là nguồn động viên lớn lao cho chúng tôi. 
                Chúng tôi cam kết mang đến cho Quý vị những sản phẩm chất lượng cao và dịch vụ tận tâm nhất. 
                Sự lựa chọn của Quý vị đã làm cho chúng tôi cảm động và chúng tôi sẽ không ngừng nỗ lực để đảm bảo rằng trải nghiệm của Quý vị với chúng tôi sẽ vượt xa những gì Quý vị mong đợi.",
                .register_date = DateTime.Now(),
                .is_custom_order = True
            }

            db.custom_order_notification.Add(notification)
            db.SaveChanges()

            Return RedirectToAction("CustomOrderDetailPage", New With {.id = orderId})

        End Function

        <HttpPost>
        <ValidateAntiForgeryToken()>
        Function UploadImageCutomOrder(file As HttpPostedFileBase, orderId As Integer) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            If file IsNot Nothing AndAlso file.ContentLength > 0 Then
                Dim fileName As String = System.IO.Path.GetFileName(file.FileName)
                Dim path As String = Server.MapPath("~/Uploads/" & fileName)
                file.SaveAs(path)

                Dim customOrder As custom_order = New custom_order()
                customOrder = db.custom_order.Find(orderId)
                customOrder.img_product = fileName

                db.Entry(customOrder).State = EntityState.Modified
                db.SaveChanges()

            End If
            Return RedirectToAction("CustomOrderDetailPage", New With {.id = orderId})
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken()>
        Function CustomOrderSuccess(orderId As Integer, totalPrice As Integer) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim customOrder As custom_order = Nothing
            customOrder = db.custom_order.Find(orderId)

            customOrder.status = True
            customOrder.total_price = totalPrice

            db.Entry(customOrder).State = EntityState.Modified
            db.SaveChanges()

            Dim notification As custom_order_notification = New custom_order_notification With {
                .user_id = customOrder.user_id,
                .status = False,
                .order_id = customOrder.id,
                .content_notification = "Chúng tôi xin trân trọng thông báo rằng đơn hàng đã đặt giao vào ngày " & customOrder.delivery_date.ToShortDateString() & " của bạn đã được hoàn thành và sẵn sàng để giao hàng.",
                .register_date = DateTime.Now(),
                .is_custom_order = True
            }

            db.custom_order_notification.Add(notification)
            db.SaveChanges()

            Return RedirectToAction("CustomOrderDetailPage", New With {.id = orderId})
        End Function

        Function PostPage() As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim postDataList As List(Of PostData) = New List(Of PostData)

            Dim postList As List(Of post) = Nothing
            postList = db.post.ToList()

            For Each item In postList

                Dim postCommentList As List(Of post_comment) = (From p In db.post_comment
                                                                Where p.post_id = item.id
                                                                Select p).ToList()

                Dim postData As PostData = New PostData With {
                    .post = item,
                    .commentTotal = postCommentList.Count()
                }

                postDataList.Add(postData)

            Next
            Return View("~/Views/managers/Post.vbhtml", postDataList)
        End Function

        Function CreatePostPage() As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If
            Return View("~/Views/managers/CreatePost.vbhtml")
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken()>
        Function CreatePost(file As HttpPostedFileBase, title As String, content As String, ingredient As String, process As String) As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If
            If file IsNot Nothing AndAlso file.ContentLength > 0 Then
                Dim fileName As String = System.IO.Path.GetFileName(file.FileName)
                Dim path As String = Server.MapPath("~/Uploads/" & fileName)
                file.SaveAs(path)

                Dim post As post = New post With {
                    .image = fileName,
                    .ingredient = ingredient,
                    .post_content = content,
                    .process = process,
                    .register_date = Date.Now().ToShortDateString(),
                    .title = title
                }

                db.post.Add(post)
                db.SaveChanges()

                Dim users As List(Of user) = Nothing
                users = (From u In db.user
                         Where u.manager = False
                         Select u).ToList()

                Dim notifications As List(Of notification) = New List(Of notification)

                Dim postResult As post = db.post.OrderByDescending(Function(x) x.id).FirstOrDefault()

                For Each item In users

                    Dim notification As notification = New notification With {
                            .user_id = item.id,
                            .register_time = DateTime.Now(),
                            .status = False,
                            .type_of_notification = NotificationType.Post,
                            .type_of_notification_id = postResult.id,
                            .content_notification = "Chúng tôi vừa xuất bản một bài viết mới trên trang web của chúng tôi!"
                    }

                    notifications.Add(notification)
                Next

                db.notification.AddRange(notifications)
                db.SaveChanges()

            End If

            Return RedirectToAction("PostPage")
        End Function

        Function PostReviewPage(ByVal id As Integer?) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim post As post = Nothing
            post = db.post.Find(id)

            Return View("~/Views/managers/PostReview.vbhtml", post)
        End Function

        Function CheckUserLogin() As Boolean

            Dim userInfo = Request.Cookies("Manager")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim userData As user = Nothing

            userData = (From u In db.user
                        Where u.id = userId And u.manager = True
                        Select u).FirstOrDefault()

            If userData Is Nothing Then
                Return False
            End If

            Return True
        End Function

        Public Function GetData() As JsonResult

            Dim orders As List(Of order) = (From o In db.order
                                            Where o.register_time.Year = Date.Now.Year
                                            Select o).ToList()

            Dim totalAmountByMonth = From order In orders
                                     Group order By Month = order.register_time.Month Into MonthGroup = Group
                                     Select New With {
                                         .Month = Month,
                                         .TotalAmount = MonthGroup.Sum(Function(x) x.total_price)
                                     }

            Dim data(11) As Integer

            For Each item In totalAmountByMonth
                data(item.Month - 1) = item.TotalAmount
            Next

            Return Json(data, JsonRequestBehavior.AllowGet)

        End Function

        Function Ingredient() As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim ingredientCategory As List(Of ingredient_category) = Nothing
            ingredientCategory = db.ingredient_category.ToList()

            Dim ingredients As List(Of ingredient) = Nothing
            ingredients = db.ingredient.ToList()


            Dim tupleData As New Tuple(Of List(Of ingredient_category), List(Of ingredient))(ingredientCategory, ingredients)

            Return View("~/Views/managers/Ingredient.vbhtml", tupleData)
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken()>
        Function CreateIngredient(ingredientName As String, ingredientCategoryId As Integer)

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim ingredient As ingredient = New ingredient With {
                .ingredient_category = ingredientCategoryId,
                .ingredient_name = ingredientName,
                .quantity = 0
            }

            db.ingredient.Add(ingredient)
            db.SaveChanges()

            Return RedirectToAction("Ingredient")
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken()>
        Function DeleteIngredient(ingredientId As Integer)

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim ingredient As ingredient = db.ingredient.Find(ingredientId)
            db.ingredient.Remove(ingredient)
            db.SaveChanges()

            Return RedirectToAction("Ingredient")
        End Function




        Function PurchaseOrderPage() As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If

            Dim purchaseOrders As List(Of purchased_order) = Nothing

            Dim ingredients As List(Of ingredient) = Nothing
            ingredients = db.ingredient.ToList()


            Dim tupleData As New Tuple(Of List(Of purchased_order), List(Of ingredient))(purchaseOrders, ingredients)

            Return View("~/Views/managers/PurchaseOrder.vbhtml", tupleData)
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken()>
        Function CreatePurchaseOrder(ingredientId As Integer, quantity As Integer) As ActionResult

            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim checkUser As Boolean = CheckUserLogin()

            If Not checkUser Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager") With {
                    .Expires = DateTime.Now.AddDays(-1D)
                }
                Response.Cookies.Add(myCookie)
                Return RedirectToAction("Login", "users")
            End If


            Dim ingredient As ingredient = Nothing
            ingredient = db.ingredient.Find(ingredientId)

            Dim userCookie = Request.Cookies("Manager")
            Dim userId = Decimal.Parse(userCookie.Value)

            Dim purchasedOrder As purchased_order = Nothing
            purchasedOrder = (From p In db.purchased_order
                              Where p.user_id = userId And p.status = False
                              Select p).FirstOrDefault()

            If purchasedOrder Is Nothing Then

                Dim purchaseOrder As purchased_order = New purchased_order With {
                    .user_id = userId,
                    .register_time = DateTime.Now(),
                    .status = False,
                    .total_price = 0,
                    .total_quantity = quantity
                }

                db.purchased_order.Add(purchaseOrder)
                db.SaveChanges()

                Dim purchasedOrderResult As purchased_order = db.purchased_order.OrderByDescending(Function(x) x.id).FirstOrDefault()

                Dim purchasedOrderDetail As purchased_order_detail = New purchased_order_detail With {
                    .purchased_order_id = purchasedOrderResult.id,
                    .ingredient_category_id = ingredient.ingredient_category,
                    .ingredient_name = ingredient.ingredient_name,
                    .price = 0,
                    .quantity = quantity,
                    .ingredient_id = purchasedOrderResult.id
                }

                db.purchased_order_detail.Add(purchasedOrderDetail)
                db.SaveChanges()
            Else

                Dim purchasedOrderDetail As purchased_order_detail = Nothing
                purchasedOrderDetail = (From p In db.purchased_order_detail
                                        Where p.ingredient_id = ingredient.id And p.purchased_order_id = purchasedOrder.id
                                        Select p).FirstOrDefault()

                If purchasedOrderDetail IsNot Nothing Then

                    purchasedOrderDetail.quantity = quantity + purchasedOrderDetail.quantity

                    db.Entry(purchasedOrderDetail).State = EntityState.Modified
                    db.SaveChanges()

                Else

                    Dim purchasedOrderDetailCreate As purchased_order_detail = New purchased_order_detail With {
                        .ingredient_id = ingredient.id,
                        .ingredient_category_id = ingredient.ingredient_category,
                        .ingredient_name = ingredient.ingredient_name,
                        .price = 0,
                        .purchased_order_id = purchasedOrder.id,
                        .quantity = quantity
                    }

                    db.purchased_order_detail.Add(purchasedOrderDetailCreate)
                    db.SaveChanges()

                End If
            End If

            Return RedirectToAction("PurchaseOrderPage")
        End Function


    End Class
End Namespace