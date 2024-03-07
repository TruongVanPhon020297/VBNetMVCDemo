Imports System.Web.Mvc
Imports System.Data.Entity
Namespace Controllers
    Public Class ManagerController
        Inherits System.Web.Mvc.Controller

        Private db As New DBNetEntities

        ' GET: Manager
        Function HomePage() As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If
            Return View("~/Views/managers/Home.vbhtml")
        End Function

        Function ProductPage() As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If
            Dim products As List(Of product) = New List(Of product)
            products = db.products.ToList()
            Return View("~/Views/managers/Product.vbhtml", products)
        End Function

        Function Edit(ByVal id As Integer?) As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If
            Dim product As product = Nothing
            product = db.products.Find(id)

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

            Dim product As product = New product()
            product = db.products.Find(productId)

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

            If file IsNot Nothing AndAlso file.ContentLength > 0 Then
                Dim fileName As String = System.IO.Path.GetFileName(file.FileName)
                Dim path As String = Server.MapPath("~/Uploads/" & fileName)
                file.SaveAs(path)

                Dim product As product = New product()
                product = db.products.Find(productId)
                product.image = fileName

                db.Entry(product).State = EntityState.Modified
                db.SaveChanges()

            End If
            Return RedirectToRoute(New With {.controller = "Manager", .action = "Edit", .id = productId})
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Delete(productId As Integer) As ActionResult

            Dim product As product = New product()
            product = db.products.Find(productId)

            db.products.Remove(product)
            db.SaveChanges()

            Return RedirectToAction("ProductPage")
        End Function

        Function CreateProduct() As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim categories As List(Of category) = New List(Of category)
            categories = db.categories.ToList()

            Return View("~/Views/managers/CreateProduct.vbhtml", categories)
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken()>
        Function CreateProduct(file As HttpPostedFileBase, productName As String, price As Decimal, description As String, categoryId As Integer) As ActionResult

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

                db.products.Add(product)
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

            Dim userDataList As List(Of UserData) = New List(Of UserData)
            Dim users As List(Of user) = New List(Of user)
            users = (From u In db.users
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

            Dim user As user = New user()
            user = db.users.Find(userId)

            db.users.Remove(user)

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
            Dim orders As List(Of order) = Nothing
            orders = db.orders.OrderBy(Function(item) item.status).ThenBy(Function(item) item.register_time).ToList()
            Return View("~/Views/managers/Order.vbhtml", orders)
        End Function

        Function OrderDetailPage(id As Integer) As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If

            Dim orderInfo As order = New order()
            orderInfo = db.orders.Find(id)

            Dim orderDetails As List(Of order_detail) = New List(Of order_detail)
            orderDetails = (From o In db.order_detail
                            Where o.order_id = orderInfo.id
                            Select o).ToList()

            Dim userInfo = db.users.Find(orderInfo.user_id)

            Dim delivery As delivery = New delivery()
            delivery = db.deliveries.Find(orderInfo.delivery_id)

            Dim tupleData As New Tuple(Of List(Of order_detail), order, user, delivery)(orderDetails, orderInfo, userInfo, delivery)

            Return View("~/Views/managers/OrderDetail.vbhtml", tupleData)

        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function ConfirmOrder(orderId As Integer, deliveryDate As Date) As ActionResult

            Dim orderInfo As order = New order()
            orderInfo = db.orders.Find(orderId)
            orderInfo.status = True

            db.Entry(orderInfo).State = EntityState.Modified

            Dim delivery As delivery = New delivery()
            delivery = db.deliveries.Find(orderInfo.delivery_id)
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

            Return RedirectToAction("OrderPage")

        End Function

        Function CreateUser() As ActionResult
            If Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login", "users")
            End If
            Return View("~/Views/managers/CreateUser.vbhtml")
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function CreateUserData(file As HttpPostedFileBase, fullName As String, email As String, password As String, manager As Integer) As ActionResult

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
                userCheck = (From u In db.users
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

                    db.users.Add(user)
                    db.SaveChanges()

                    Dim userResult As user = New user()
                    userResult = (From u In db.users
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

            Dim userCookie = Request.Cookies("Manager")
            Dim userId = Decimal.Parse(userCookie.Value)

            Dim cart As cart = New cart()
            Dim cartDetails As List(Of cart_detail) = New List(Of cart_detail)

            Try

                cart = (From c In db.carts
                        Where c.user_id = userId
                        Select c).FirstOrDefault()




                cartDetails = (From d In db.cart_detail
                               Where d.cart_id = cart.id
                               Select d).ToList()
            Catch ex As Exception

            End Try

            Dim products As List(Of product) = New List(Of product)
            products = (From p In db.products
                        Select p).ToList()

            Dim users As List(Of user) = New List(Of user)
            users = (From u In db.users
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

            Dim userCookie = Request.Cookies("Manager")
            Dim userId = Decimal.Parse(userCookie.Value)



            Dim cart As cart = Nothing
            cart = (From c In db.carts
                    Where c.user_id = userId
                    Select c).FirstOrDefault()


            Dim cartRegister As cart = New cart()

            Dim product As product = db.products.Find(Convert.ToDecimal(productId))

            If IsNothing(cart) Then

                cartRegister.user_id = userId
                cartRegister.quantity = quantity
                cartRegister.total_price = product.price * quantity
                db.carts.Add(cartRegister)
                db.SaveChanges()


                Dim cartResult As cart = Nothing

                cartResult = (From p In db.carts
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

            cart = (From c In db.carts
                    Where c.user_id = userIdCookie
                    Select c).FirstOrDefault()

            Dim delivery As delivery = New delivery With {
                .location = address,
                .phone = phone
            }

            db.deliveries.Add(delivery)
            db.SaveChanges()

            Dim deliveryResult As delivery = Nothing
            deliveryResult = (From d In db.deliveries
                              Order By d.Id Descending
                              Select d).FirstOrDefault()

            Dim orderRegister = ToOrder(cart)
            orderRegister.delivery_id = deliveryResult.Id
            orderRegister.user_id = userId

            db.orders.Add(orderRegister)
            db.SaveChanges()
            db.carts.Remove(cart)
            db.SaveChanges()

            Dim cartDetails As List(Of cart_detail) = Nothing

            cartDetails = (From c In db.cart_detail
                           Where c.cart_id = cart.id
                           Select c).ToList()

            Dim orderResult As order = Nothing
            orderResult = (From o In db.orders
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

            Dim userCookie = Request.Cookies("Manager")
            Dim userId = Decimal.Parse(userCookie.Value)

            Dim user As user = db.users.Find(userId)

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

            Dim customOrders As List(Of custom_order) = Nothing
            customOrders = db.custom_order.ToList()

            Return View("~/Views/managers/CustomOrder.vbhtml", customOrders)
        End Function

        Function CustomOrderDetailPage(id As Integer) As ActionResult

            Dim customOrders As custom_order = Nothing
            customOrders = db.custom_order.Find(id)

            Dim category As category = Nothing
            category = db.categories.Find(customOrders.category_id)

            Dim tupleData As New Tuple(Of custom_order, category)(customOrders, category)

            Return View("~/Views/managers/CustomOrderDetail.vbhtml", tupleData)
        End Function


        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function ConfirmCustomOrder(orderId As Integer) As ActionResult

            Dim customOrder As custom_order = Nothing
            customOrder = db.custom_order.Find(orderId)

            customOrder.confirm = True

            db.Entry(customOrder).State = EntityState.Modified
            db.SaveChanges()

            Dim notification As custom_order_notification = New custom_order_notification With {
                .user_id = customOrder.user_id,
                .status = False,
                .custom_order_id = customOrder.id,
                .content_notification = "Chúng tôi xin chân thành gửi lời cảm ơn sâu sắc đến Quý vị đã tin tưởng và đặt hàng tại cửa hàng của chúng tôi. 
                Sự ủng hộ của Quý vị không chỉ là một sự khích lệ mà còn là nguồn động viên lớn lao cho chúng tôi. 
                Chúng tôi cam kết mang đến cho Quý vị những sản phẩm chất lượng cao và dịch vụ tận tâm nhất. 
                Sự lựa chọn của Quý vị đã làm cho chúng tôi cảm động và chúng tôi sẽ không ngừng nỗ lực để đảm bảo rằng trải nghiệm của Quý vị với chúng tôi sẽ vượt xa những gì Quý vị mong đợi.",
                .register_date = DateTime.Now()
            }

            db.custom_order_notification.Add(notification)
            db.SaveChanges()

            Return RedirectToAction("CustomOrderDetailPage", New With {.id = orderId})

        End Function

        <HttpPost>
        <ValidateAntiForgeryToken()>
        Function UploadImageCutomOrder(file As HttpPostedFileBase, orderId As Integer) As ActionResult

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

            Dim customOrder As custom_order = Nothing
            customOrder = db.custom_order.Find(orderId)

            customOrder.status = True
            customOrder.total_price = totalPrice

            db.Entry(customOrder).State = EntityState.Modified
            db.SaveChanges()

            Dim notification As custom_order_notification = New custom_order_notification With {
                .user_id = customOrder.user_id,
                .status = False,
                .custom_order_id = customOrder.id,
                .content_notification = "Chúng tôi xin trân trọng thông báo rằng đơn hàng đã đặt giao vào ngày " & customOrder.delivery_date.ToShortDateString() & " của bạn đã được hoàn thành và sẵn sàng để giao hàng.",
                .register_date = DateTime.Now()
            }

            db.custom_order_notification.Add(notification)
            db.SaveChanges()

            Return RedirectToAction("CustomOrderDetailPage", New With {.id = orderId})
        End Function


    End Class
End Namespace