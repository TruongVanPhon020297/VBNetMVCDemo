Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Net
Imports System.Web
Imports System.Web.Mvc
Imports Project_VB_Net_MVC

Namespace Controllers
    Public Class usersController
        Inherits System.Web.Mvc.Controller

        Private db As New DBNetEntities

        ' GET: users
        Function Register() As ActionResult
            Return View()
        End Function

        ' GET: users/Details/5
        Function Details(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim user As user = db.users.Find(id)
            If IsNothing(user) Then
                Return HttpNotFound()
            End If
            Return View(user)
        End Function

        ' GET: users/Create
        Function Create() As ActionResult
            Return View()
        End Function

        <ActionName("Login")>
        Function LoginPage() As ActionResult
            If Request.Cookies("UserName") Is Nothing And Request.Cookies("Manager") Is Nothing Then
                Return View("Login")
            ElseIf Request.Cookies("UserName") IsNot Nothing Then
                Return RedirectToAction("Product")
            Else
                Return RedirectToAction("HomePage", "Manager")
            End If

        End Function

        ' POST: users/Create
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ActionName("Register")>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="id,full_name,password,email")> ByVal user As user) As ActionResult
            If ModelState.IsValid Then
                db.users.Add(user)
                db.SaveChanges()
                Return RedirectToAction("Login")
            End If
            Return RedirectToAction("Register")
        End Function

        Function Product() As ActionResult
            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If
            Return View(db.products.ToList())
        End Function

        <HttpPost()>
        <ActionName("Login")>
        <ValidateAntiForgeryToken()>
        Function Login(<Bind(Include:="id,full_name,password,email")> ByVal userInfo As user) As ActionResult
            Dim viewModel As user = Nothing
            If ModelState.IsValid Then
                viewModel = (From u In db.users Where u.email.Equals(userInfo.email) And u.password.Equals(userInfo.password)).FirstOrDefault()
                If viewModel IsNot Nothing Then

                    If viewModel.manager Then
                        Dim aCookie As New HttpCookie("Manager")
                        aCookie.Value = viewModel.id
                        aCookie.Expires = DateTime.Now.AddDays(30)
                        Response.Cookies.Add(aCookie)
                        Return RedirectToAction("HomePage", "Manager")
                    Else
                        Dim aCookie As New HttpCookie("UserName")
                        aCookie.Value = viewModel.id
                        aCookie.Expires = DateTime.Now.AddDays(30)
                        Response.Cookies.Add(aCookie)
                        Return RedirectToAction("Product")
                    End If
                End If
            End If
            Return RedirectToAction("Login")
        End Function

        ' GET: users/Edit/5
        Function Edit(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim user As user = db.users.Find(id)
            If IsNothing(user) Then
                Return HttpNotFound()
            End If
            Return View(user)
        End Function

        ' POST: users/Edit/5
        'To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        'more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Edit(<Bind(Include:="id,full_name,password,email")> ByVal user As user) As ActionResult
            If ModelState.IsValid Then
                db.Entry(user).State = EntityState.Modified
                db.SaveChanges()
                Return RedirectToAction("Index")
            End If
            Return View(user)
        End Function

        ' GET: users/Delete/5
        Function Delete(ByVal id As Integer?) As ActionResult
            If IsNothing(id) Then
                Return New HttpStatusCodeResult(HttpStatusCode.BadRequest)
            End If
            Dim user As user = db.users.Find(id)
            If IsNothing(user) Then
                Return HttpNotFound()
            End If
            Return View(user)
        End Function

        ' POST: users/Delete/5
        <HttpPost()>
        <ActionName("Delete")>
        <ValidateAntiForgeryToken()>
        Function DeleteConfirmed(ByVal id As Integer) As ActionResult
            Dim user As user = db.users.Find(id)
            db.users.Remove(user)
            db.SaveChanges()
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Product(name As String) As ActionResult
            Dim viewModel As List(Of product) = Nothing

            If String.IsNullOrEmpty(name) Then
                viewModel = db.products.ToList()
            Else
                viewModel = (From p In db.products Where p.product_name.Contains(name)).ToList()
            End If

            Return View("Product", viewModel)
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function AddToCart(productId As String) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)
            Dim cart As cart = Nothing
            cart = (From c In db.carts
                    Where c.user_id = userId
                    Select c).FirstOrDefault()


            Dim cartRegister As cart = New cart()

            Dim product As product = db.products.Find(Convert.ToDecimal(productId))

            If IsNothing(cart) Then

                cartRegister.user_id = userId
                cartRegister.quantity = 1
                cartRegister.total_price = product.price
                db.carts.Add(cartRegister)
                db.SaveChanges()


                Dim cartResult As List(Of cart) = Nothing

                cartResult = (From p In db.carts
                              Where p.user_id = userId
                              Select p).ToList()


                Dim cartDetailRegister As cart_detail = New cart_detail()
                cartDetailRegister.image = product.image
                cartDetailRegister.product_id = product.Id
                cartDetailRegister.product_name = product.product_name
                cartDetailRegister.quantity = 1
                cartDetailRegister.total_price = product.price
                cartDetailRegister.cart_id = cartResult(0).id
                cartDetailRegister.price = product.price

                db.cart_detail.Add(cartDetailRegister)
                db.SaveChanges()

            Else

                cart.quantity = cart.quantity + 1
                cart.total_price = cart.total_price + product.price

                db.Entry(cart).State = EntityState.Modified
                db.SaveChanges()
                Dim cartDetail As cart_detail = (From c In db.cart_detail
                                                 Where c.product_id = product.Id
                                                 Select c).FirstOrDefault()

                If IsNothing(cartDetail) Then

                    Dim cartDetailRegister As cart_detail = New cart_detail()
                    cartDetailRegister.image = product.image
                    cartDetailRegister.product_id = product.Id
                    cartDetailRegister.product_name = product.product_name
                    cartDetailRegister.quantity = 1
                    cartDetailRegister.total_price = product.price
                    cartDetailRegister.cart_id = cart.id
                    cartDetailRegister.price = product.price
                    db.cart_detail.Add(cartDetailRegister)
                    db.SaveChanges()
                Else
                    cartDetail.quantity = cartDetail.quantity + 1
                    cartDetail.total_price = cartDetail.total_price + product.price
                    db.Entry(cartDetail).State = EntityState.Modified
                    db.SaveChanges()
                End If
            End If
            Return RedirectToAction("CartInfo")
        End Function


        Function Logout() As ActionResult
            If (Not Request.Cookies("UserName") Is Nothing) Then
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("UserName")
                myCookie.Expires = DateTime.Now.AddDays(-1D)
                Response.Cookies.Add(myCookie)
            Else
                Dim myCookie As HttpCookie
                myCookie = New HttpCookie("Manager")
                myCookie.Expires = DateTime.Now.AddDays(-1D)
                Response.Cookies.Add(myCookie)
            End If

            Return RedirectToAction("Login")
        End Function

        Function CartInfo() As ActionResult
            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim cartDetails As List(Of cart_detail) = Nothing
            Dim cart As cart = Nothing

            cart = (From c In db.carts
                    Where c.user_id = userId
                    Select c).FirstOrDefault()

            Try
                cartDetails = (From c In db.cart_detail
                               Where c.cart_id = cart.id
                               Select c).ToList()
            Catch ex As Exception
                Return View("Cart")
            End Try

            Dim tupleData As New Tuple(Of List(Of cart_detail), cart)(cartDetails, cart)

            Return View("Cart", tupleData)
        End Function


        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function RemoveDetail(detailId As String) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim cart As cart = Nothing

            cart = (From c In db.carts
                    Where c.user_id = userId
                    Select c).FirstOrDefault()

            If cart IsNot Nothing Then

                Dim cartDetail As cart_detail = Nothing

                cartDetail = (From d In db.cart_detail
                              Where d.id = detailId
                              Select d).FirstOrDefault()

                cart.quantity = cart.quantity - cartDetail.quantity
                cart.total_price = cart.total_price - cartDetail.total_price

                db.Entry(cart).State = EntityState.Modified
                db.SaveChanges()

                If cartDetail IsNot Nothing Then
                    db.cart_detail.Remove(cartDetail)
                    db.SaveChanges()
                End If

            End If
            Return RedirectToAction("CartInfo")
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Increament(detailId As String) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim cart As cart = Nothing

            cart = (From c In db.carts
                    Where c.user_id = userId
                    Select c).FirstOrDefault()


            If cart IsNot Nothing Then

                Dim cartDetail As cart_detail = Nothing

                cartDetail = (From d In db.cart_detail
                              Where d.id = detailId
                              Select d).FirstOrDefault()

                cart.quantity = cart.quantity + 1
                cart.total_price = cart.total_price + cartDetail.price

                db.Entry(cart).State = EntityState.Modified
                db.SaveChanges()

                If cartDetail IsNot Nothing Then
                    cartDetail.quantity = cartDetail.quantity + 1
                    cartDetail.total_price = cartDetail.total_price + cartDetail.price
                    db.Entry(cart).State = EntityState.Modified
                    db.SaveChanges()
                End If

            End If

            Return RedirectToAction("CartInfo")

        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Decreament(detailId As String) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim cart As cart = Nothing

            cart = (From c In db.carts
                    Where c.user_id = userId
                    Select c).FirstOrDefault()


            If cart IsNot Nothing Then

                Dim cartDetail As cart_detail = Nothing

                cartDetail = (From d In db.cart_detail
                              Where d.id = detailId
                              Select d).FirstOrDefault()

                If cartDetail IsNot Nothing And cartDetail.quantity > 1 Then

                    cart.quantity = cart.quantity - 1
                    cart.total_price = cart.total_price - cartDetail.price

                    db.Entry(cart).State = EntityState.Modified
                    db.SaveChanges()

                    cartDetail.quantity = cartDetail.quantity - 1
                    cartDetail.total_price = cartDetail.total_price - cartDetail.price
                    db.Entry(cart).State = EntityState.Modified
                    db.SaveChanges()
                End If

            End If

            Return RedirectToAction("CartInfo")

        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Checkout(cartId As String, address As String, phone As String) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim cart As cart = Nothing

            cart = (From c In db.carts
                    Where c.user_id = userId
                    Select c).FirstOrDefault()

            Dim delivery As delivery = New delivery()
            delivery.location = address
            delivery.phone = phone

            db.deliveries.Add(delivery)
            db.SaveChanges()

            Dim deliveryResult As delivery = Nothing
            deliveryResult = (From d In db.deliveries
                              Order By d.Id Descending
                              Select d).FirstOrDefault()

            Dim orderRegister = ToOrder(cart)
            orderRegister.delivery_id = deliveryResult.Id

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

            Return RedirectToAction("OrderInfo")

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
                Dim orderDetail As order_detail = New order_detail()

                orderDetail.order_id = orderId
                orderDetail.product_id = item.product_id
                orderDetail.image = item.image
                orderDetail.total_price = item.total_price
                orderDetail.quantity = item.quantity
                orderDetail.product_name = item.product_name
                orderDetail.price = item.price

                orderDetails.Add(orderDetail)

            Next

            Return orderDetails

        End Function

        Function OrderInfo() As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim orders As List(Of order) = Nothing

            orders = (From o In db.orders
                      Where o.user_id = userId
                      Select o).ToList()
            Return View("Order", orders)
        End Function


        Function OrderDetail(ByVal id As Integer?) As ActionResult

            Dim orderDetails As List(Of order_detail) = Nothing
            orderDetails = (From o In db.order_detail Where o.order_id = id).ToList()

            Dim order As order = Nothing
            order = (From o In db.orders Where o.id = id).FirstOrDefault()

            Dim delivery As delivery = Nothing
            delivery = (From d In db.deliveries Where d.Id = order.delivery_id).FirstOrDefault()


            Dim tupleData As New Tuple(Of List(Of order_detail), delivery, order)(orderDetails, delivery, order)

            Return View("OrderDetail", tupleData)
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function CancelOrder(orderId As String) As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If
            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim order As order = db.orders.Find(Decimal.Parse(orderId))
            db.orders.Remove(order)
            db.SaveChanges()

            Dim orderDetails As List(Of order_detail) = Nothing
            orderDetails = (From o In db.order_detail
                            Where o.order_id = order.id).ToList()

            db.order_detail.RemoveRange(orderDetails)
            db.SaveChanges()

            Dim delivery As delivery = Nothing
            delivery = db.deliveries.Find(order.delivery_id)
            db.deliveries.Remove(delivery)
            db.SaveChanges()

            Return RedirectToAction("OrderInfo")
        End Function

        Function UserInfo() As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If
            Dim userCookie = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userCookie.Value)

            Dim user As user = db.users.Find(userId)

            Dim userInfoData As user_info = New user_info()
            userInfoData = (From u In db.user_info
                            Where u.user_id = user.id).FirstOrDefault()

            Dim orderDetails As List(Of order_detail) = Nothing

            Dim tupleData As New Tuple(Of List(Of order_detail), user, user_info)(orderDetails, user, userInfoData)

            ViewBag.ImagePath = "http://bootdey.com/img/Content/avatar/avatar1.png"

            If userInfoData.image IsNot Nothing Then
                Dim imageUrl As String = Url.Content("~/Uploads/" & userInfoData.image)
                ViewBag.ImagePath = imageUrl
            End If

            Return View("User", tupleData)

        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function UpdateInfoUser(address As String, phone As String, birthday As Date) As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If
            Dim userCookie = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userCookie.Value)

            Dim user As user = db.users.Find(userId)

            Dim userInfoData As user_info = New user_info()
            userInfoData = (From u In db.user_info
                            Where u.user_id = user.id).FirstOrDefault()

            userInfoData.address = address
            userInfoData.phone = phone
            userInfoData.birth_day = birthday

            db.Entry(userInfoData).State = EntityState.Modified
            db.SaveChanges()

            Return RedirectToAction("UserInfo")

        End Function

        <HttpPost>
        Function Upload(file As HttpPostedFileBase) As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If
            Dim userCookie = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userCookie.Value)

            If file IsNot Nothing AndAlso file.ContentLength > 0 Then
                Dim fileName As String = System.IO.Path.GetFileName(file.FileName)
                Dim path As String = Server.MapPath("~/Uploads/" & fileName)
                file.SaveAs(path)

                Dim userInfo As user_info = Nothing
                userInfo = (From u In db.user_info
                            Where u.user_id = userId).FirstOrDefault()

                userInfo.image = fileName

                db.Entry(userInfo).State = EntityState.Modified
                db.SaveChanges()

            End If
            Return RedirectToAction("UserInfo")
        End Function

        Function Detail(ByVal id As Integer?) As ActionResult
            Dim product As product = Nothing

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim rating As rate = New rate()
            Dim rates As List(Of rate) = New List(Of rate)
            rating = (From r In db.rates
                      Where r.user_id = userId And r.product_id = id
                      Select r).FirstOrDefault()

            rates = (From r In db.rates
                     Where r.user_id <> userId And r.product_id = id
                     Select r).ToList()

            Dim rateListData As List(Of RateData) = New List(Of RateData)

            For Each item In rates

                Dim userData As UserData = New UserData()
                Dim user As user = New user()

                user = db.users.Find(item.user_id)
                Dim userInfoData As user_info = New user_info()
                userInfoData = (From u In db.user_info
                                Where u.user_id = item.user_id
                                Select u).FirstOrDefault()
                If userInfoData.image IsNot Nothing Then
                    userData.image = userInfoData.image
                End If

                userData.fullName = user.full_name

                Dim rateData As RateData = New RateData()
                rateData.rate = item
                rateData.user = userData

                rateListData.Add(rateData)

            Next
            product = db.products.Find(id)
            Dim tupleData As New Tuple(Of List(Of RateData), product, rate)(rateListData, product, rating)
            Return View("Detail", tupleData)
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function AddToCartWithQuantity(quantity As Integer, productId As Integer) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)
            Dim cart As cart = Nothing
            cart = (From c In db.carts
                    Where c.user_id = userId
                    Select c).FirstOrDefault()


            Dim cartRegister As cart = New cart()

            Dim product As product = db.products.Find(productId)

            If IsNothing(cart) Then

                cartRegister.user_id = userId
                cartRegister.quantity = quantity
                cartRegister.total_price = product.price * quantity
                db.carts.Add(cartRegister)
                db.SaveChanges()


                Dim cartResult As List(Of cart) = Nothing

                cartResult = (From p In db.carts
                              Where p.user_id = userId
                              Select p).ToList()


                Dim cartDetailRegister As cart_detail = New cart_detail()
                cartDetailRegister.image = product.image
                cartDetailRegister.product_id = product.Id
                cartDetailRegister.product_name = product.product_name
                cartDetailRegister.quantity = quantity
                cartDetailRegister.total_price = product.price * quantity
                cartDetailRegister.cart_id = cartResult(0).id
                cartDetailRegister.price = product.price

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

                    Dim cartDetailRegister As cart_detail = New cart_detail()
                    cartDetailRegister.image = product.image
                    cartDetailRegister.product_id = product.Id
                    cartDetailRegister.product_name = product.product_name
                    cartDetailRegister.quantity = quantity
                    cartDetailRegister.total_price = product.price * quantity
                    cartDetailRegister.cart_id = cart.id
                    cartDetailRegister.price = product.price
                    db.cart_detail.Add(cartDetailRegister)
                    db.SaveChanges()
                Else
                    cartDetail.quantity = cartDetail.quantity + quantity
                    cartDetail.total_price = cartDetail.total_price + product.price * quantity
                    db.Entry(cartDetail).State = EntityState.Modified
                    db.SaveChanges()
                End If
            End If
            Return RedirectToAction("CartInfo")
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function CreateRate(rating As Integer, comment As String, productId As Integer) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim rate As rate = New rate()
            rate.star = rating
            rate.user_id = userId
            rate.register_time = Date.Now()
            rate.comment = comment
            rate.product_id = productId

            db.rates.Add(rate)
            db.SaveChanges()

            Return RedirectToAction("Detail", New With {.id = productId})

        End Function

    End Class





End Namespace
