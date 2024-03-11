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


        ' Register Data 
        Function Register() As ActionResult
            Return View()
        End Function

        <HttpPost()>
        <ActionName("Register")>
        <ValidateAntiForgeryToken()>
        Function Create(<Bind(Include:="id,full_name,password,email")> ByVal user As user) As ActionResult
            If ModelState.IsValid Then

                If user.full_name Is Nothing Then
                    TempData("fullName") = "Full name is not blank"
                    Return RedirectToAction("Register")
                End If

                If user.password Is Nothing Then
                    TempData("password") = "Password is not blank"
                    Return RedirectToAction("Register")
                End If

                Dim userCheck As user = New user()
                userCheck = (From u In db.users
                             Where u.email = user.email
                             Select u).FirstOrDefault()

                If userCheck Is Nothing Then
                    user.manager = False
                    db.users.Add(user)
                    db.SaveChanges()

                    Dim userResult As user = New user()
                    userResult = (From u In db.users
                                  Where u.email = user.email
                                  Select u).FirstOrDefault()

                    Dim userInfo As user_info = New user_info With {
                        .user_id = userResult.id,
                        .image = "avartar.png"
                    }
                    db.user_info.Add(userInfo)
                    db.SaveChanges()
                    Return RedirectToAction("Login")
                Else
                    TempData("email") = "Email Exists"
                    Return RedirectToAction("Register")
                End If
            End If
            Return RedirectToAction("Register")
        End Function



        ' Login
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

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Login(email As String, password As String) As ActionResult

            If email.Length = 0 Then
                TempData("emailError") = "Email is Not Blank"
                Return RedirectToAction("Login")
            End If

            If password.Length = 0 Then
                TempData("passwordError") = "Password is Not Blank"
                Return RedirectToAction("Login")
            End If

            Dim viewModel As user = Nothing
            viewModel = (From u In db.users Where u.email.Equals(email) And u.password.Equals(password)).FirstOrDefault()
            If viewModel IsNot Nothing Then
                If viewModel.manager Then
                    Dim aCookie As New HttpCookie("Manager") With {
                        .Value = viewModel.id,
                        .Expires = DateTime.Now.AddDays(30)
                    }
                    Response.Cookies.Add(aCookie)
                    Return RedirectToAction("HomePage", "Manager")
                Else
                    Dim aCookie As New HttpCookie("UserName") With {
                        .Value = viewModel.id,
                        .Expires = DateTime.Now.AddDays(30)
                    }
                    Response.Cookies.Add(aCookie)
                    Return RedirectToAction("Product")
                End If
            Else
                TempData("login") = "Login invalid"
                Return RedirectToAction("Login")
            End If
            Return RedirectToAction("Login")
        End Function


        ' User 

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

            If Request.Cookies("UserName") Is Nothing And Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login")
            End If
            Dim userInfo = Request.Cookies("UserName")
            Dim userId As Integer

            If userInfo Is Nothing Then
                Dim managerInfo = Request.Cookies("Manager")
                userId = Decimal.Parse(managerInfo.Value)
            Else
                userId = Decimal.Parse(userInfo.Value)
            End If

            If phone.Length = 0 Then
                TempData("phoneUpdate") = "Phone is not blank"
                If userInfo Is Nothing Then
                    Return RedirectToAction("UserInfo", "Manager")
                Else
                    Return RedirectToAction("UserInfo")
                End If
            End If

            If address.Length = 0 Then
                TempData("addressUpdate") = "Address is not blank"
                If userInfo Is Nothing Then
                    Return RedirectToAction("UserInfo", "Manager")
                Else
                    Return RedirectToAction("UserInfo")
                End If
            End If
            If birthday.ToShortDateString().Length = 0 Then
                TempData("birthdayUpdate") = "Birthday is not blank"
                If userInfo Is Nothing Then
                    Return RedirectToAction("UserInfo", "Manager")
                Else
                    Return RedirectToAction("UserInfo")
                End If
            End If

            Dim user As user = db.users.Find(userId)

            Dim userInfoData As user_info = New user_info()
            userInfoData = (From u In db.user_info
                            Where u.user_id = user.id).FirstOrDefault()

            userInfoData.address = address
            userInfoData.phone = phone
            userInfoData.birth_day = birthday

            db.Entry(userInfoData).State = EntityState.Modified
            db.SaveChanges()
            If userInfo Is Nothing Then
                Return RedirectToAction("UserInfo", "Manager")
            Else
                Return RedirectToAction("UserInfo")
            End If
        End Function

        <HttpPost>
        Function Upload(file As HttpPostedFileBase) As ActionResult

            If Request.Cookies("UserName") Is Nothing And Request.Cookies("Manager") Is Nothing Then
                Return RedirectToAction("Login")
            End If
            Dim userCookie = Request.Cookies("UserName")
            Dim userId As Integer

            If userCookie Is Nothing Then
                Dim managerInfo = Request.Cookies("Manager")
                userId = Decimal.Parse(managerInfo.Value)
            Else
                userId = Decimal.Parse(userCookie.Value)
            End If

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
            Else
                TempData("imageUpload") = "Image invalid"

                If userCookie Is Nothing Then
                    Return RedirectToAction("UserInfo", "Manager")
                Else
                    Return RedirectToAction("UserInfo")
                End If
            End If

            If userCookie Is Nothing Then
                Return RedirectToAction("UserInfo", "Manager")
            Else
                Return RedirectToAction("UserInfo")
            End If

        End Function

        ' Cart

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function RemoveDetail(detailId As String) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId As Integer

            If userInfo Is Nothing Then
                Dim managerInfo = Request.Cookies("Manager")
                userId = Decimal.Parse(managerInfo.Value)
            Else
                userId = Decimal.Parse(userInfo.Value)
            End If

            Dim cart As cart = Nothing

            cart = (From c In db.carts
                    Where c.user_id = userId
                    Select c).FirstOrDefault()

            If cart IsNot Nothing Then

                Dim cartDetail As cart_detail = Nothing

                cartDetail = (From d In db.cart_detail
                              Where d.id = detailId
                              Select d).FirstOrDefault()

                Dim cartDetails As List(Of cart_detail) = New List(Of cart_detail)
                cartDetails = (From c In db.cart_detail
                               Where c.cart_id = cart.id).ToList()

                If cartDetail IsNot Nothing Then
                    db.cart_detail.Remove(cartDetail)
                    db.SaveChanges()

                End If

                If cart.quantity = 1 Or cartDetails.Count = 1 Then
                    db.carts.Remove(cart)
                    db.SaveChanges()
                    If userInfo Is Nothing Then
                        Return RedirectToAction("CreateCart", "Manager")
                    Else
                        Return RedirectToAction("CartInfo")
                    End If
                End If

                cart.quantity = cart.quantity - cartDetail.quantity
                cart.total_price = cart.total_price - cartDetail.total_price

                db.Entry(cart).State = EntityState.Modified
                db.SaveChanges()

            End If
            If userInfo Is Nothing Then
                Return RedirectToAction("CreateCart", "Manager")
            Else
                Return RedirectToAction("CartInfo")
            End If
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

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Increament(detailId As String) As ActionResult

            Dim userInfo = Request.Cookies("UserName")

            Dim userId As Integer

            If userInfo Is Nothing Then
                Dim managerInfo = Request.Cookies("Manager")
                userId = Decimal.Parse(managerInfo.Value)
            Else
                userId = Decimal.Parse(userInfo.Value)
            End If


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

            If userInfo Is Nothing Then
                Return RedirectToAction("CreateCart", "Manager")
            Else
                Return RedirectToAction("CartInfo")
            End If



        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Decreament(detailId As String) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId As Integer

            If userInfo Is Nothing Then
                Dim managerInfo = Request.Cookies("Manager")
                userId = Decimal.Parse(managerInfo.Value)
            Else
                userId = Decimal.Parse(userInfo.Value)
            End If

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

            If userInfo Is Nothing Then
                Return RedirectToAction("CreateCart", "Manager")
            Else
                Return RedirectToAction("CartInfo")
            End If

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
        Function Checkout(cartId As String, address As String, phone As String) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            If address.Length = 0 Then
                TempData("address") = "Address is not blank"
                Return RedirectToAction("CartInfo")
            End If

            If phone.Length = 0 Then
                TempData("phone") = "Phone is not blank"
                Return RedirectToAction("CartInfo")
            End If

            Dim cart As cart = Nothing

            cart = (From c In db.carts
                    Where c.user_id = userId
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

        ' Product

        Function Product() As ActionResult
            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim notifications As List(Of custom_order_notification) = Nothing
            notifications = (From n In db.custom_order_notification
                             Where n.user_id = userId And n.status = False
                             Select n).ToList()



            Dim favoriteList As List(Of favorite) = New List(Of favorite)
            favoriteList = (From f In db.favorites
                            Where f.user_id = userId
                            Select f).ToList()

            Dim products As List(Of product) = New List(Of product)
            products = db.products.ToList()

            Dim productDataList As List(Of ProductData) = New List(Of ProductData)

            For Each item In products
                Dim productData As ProductData = New ProductData With {
                    .product = item,
                    .isFavorite = False
                }
                For Each itemF In favoriteList
                    If itemF.product_id = item.Id And itemF.status Then
                        productData.isFavorite = True
                        Exit For
                    End If
                Next
                productDataList.Add(productData)
            Next

            Dim categories As List(Of category) = Nothing
            categories = db.categories.ToList()


            Dim tupleData As New Tuple(Of List(Of ProductData), List(Of custom_order_notification), List(Of category))(productDataList, notifications, categories)

            Return View("Product", tupleData)
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function Product(name As String) As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)


            Dim favoriteList As List(Of favorite) = New List(Of favorite)
            favoriteList = (From f In db.favorites
                            Where f.user_id = userId
                            Select f).ToList()

            Dim notifications As List(Of custom_order_notification) = Nothing
            notifications = (From n In db.custom_order_notification
                             Where n.user_id = userId And n.status = False
                             Select n).ToList()

            Dim viewModel As List(Of product) = Nothing

            If String.IsNullOrEmpty(name) Then
                viewModel = db.products.ToList()
            Else
                viewModel = (From p In db.products Where p.product_name.Contains(name)).ToList()
            End If

            Dim productDataList As List(Of ProductData) = New List(Of ProductData)

            For Each item In viewModel
                Dim productData As ProductData = New ProductData()
                productData.product = item
                productData.isFavorite = False
                For Each itemF In favoriteList
                    If itemF.product_id = item.Id And itemF.status Then
                        productData.isFavorite = True
                        Exit For
                    End If
                Next
                productDataList.Add(productData)
            Next

            Dim categories As List(Of category) = Nothing
            categories = db.categories.ToList()


            Dim tupleData As New Tuple(Of List(Of ProductData), List(Of custom_order_notification), List(Of category))(productDataList, notifications, categories)

            Return View("Product", tupleData)
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

            Dim rateTotal As RateInfo = New RateInfo()

            Dim totalRate As List(Of rate) = New List(Of rate)
            totalRate = (From r In db.rates
                         Where r.product_id = id And r.star = 1
                         Select r).ToList()

            rateTotal.oneRate = totalRate.Count

            totalRate = (From r In db.rates
                         Where r.product_id = id And r.star = 2
                         Select r).ToList()

            rateTotal.twoRate = totalRate.Count

            totalRate = (From r In db.rates
                         Where r.product_id = id And r.star = 3
                         Select r).ToList()

            rateTotal.threeRate = totalRate.Count

            totalRate = (From r In db.rates
                         Where r.product_id = id And r.star = 4
                         Select r).ToList()

            rateTotal.fourRate = totalRate.Count

            totalRate = (From r In db.rates
                         Where r.product_id = id And r.star = 5
                         Select r).ToList()

            rateTotal.fiveRate = totalRate.Count

            Dim total As Integer = rateTotal.oneRate + rateTotal.twoRate + rateTotal.threeRate + rateTotal.fourRate + rateTotal.fiveRate
            rateTotal.ratioRate = (5 * rateTotal.fiveRate + 4 * rateTotal.fourRate + 3 * rateTotal.threeRate + 2 * rateTotal.twoRate + rateTotal.oneRate) / total

            Dim tupleData As New Tuple(Of List(Of RateData), product, rate, RateInfo)(rateListData, product, rating, rateTotal)
            Return View("Detail", tupleData)
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing) Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

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

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function CreateRate(rating As Integer, comment As String, productId As Integer) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim rate As rate = New rate With {
                .star = rating,
                .user_id = userId,
                .register_time = Date.Now(),
                .comment = comment,
                .product_id = productId
            }

            db.rates.Add(rate)
            db.SaveChanges()

            Return RedirectToAction("Detail", New With {.id = productId})

        End Function

        Function Favorite(productId As Integer) As ActionResult

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim favoriteResult As favorite = New favorite()
            favoriteResult = (From f In db.favorites
                              Where f.product_id = productId And f.user_id = userId
                              Select f).FirstOrDefault()

            If favoriteResult Is Nothing Then

                Dim favoriteRegister As favorite = New favorite With {
                    .product_id = productId,
                    .user_id = userId,
                    .status = True
                }

                db.favorites.Add(favoriteRegister)
                db.SaveChanges()
            Else
                If favoriteResult.status Then
                    favoriteResult.status = False
                Else
                    favoriteResult.status = True
                End If
                db.Entry(favoriteResult).State = EntityState.Modified
                db.SaveChanges()
            End If

            Return RedirectToAction("Product")

        End Function

        Function FavoriteInfo() As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim favorites As List(Of favorite) = New List(Of favorite)
            favorites = (From f In db.favorites
                         Where f.user_id = userId And f.status = True
                         Select f).ToList()

            Dim products As List(Of product) = Nothing


            If favorites IsNot Nothing Then

                products = New List(Of product)

                For Each item In favorites
                    Dim product As product = New product()
                    product = (From p In db.products
                               Where p.Id = item.product_id
                               Select p).FirstOrDefault()

                    If product IsNot Nothing Then
                        products.Add(product)
                    End If
                Next

            End If

            Return View("Favorite", products)
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function RemoveFavorite(productId As Integer) As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim favorite As favorite = New favorite()
            favorite = (From f In db.favorites
                        Where f.product_id = productId And f.user_id = userId
                        Select f).FirstOrDefault()

            If favorite IsNot Nothing Then
                favorite.status = False
                db.Entry(favorite).State = EntityState.Modified
                db.SaveChanges()
            End If

            Return RedirectToAction("FavoriteInfo")


        End Function

        Function PurchasedProduct() As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim products As List(Of product) = New List(Of product)

            Dim purchasedProducts As List(Of purchased_product) = Nothing
            purchasedProducts = (From p In db.purchased_product
                                 Where p.user_id = userId).ToList()

            If purchasedProducts IsNot Nothing Then

                For Each item In purchasedProducts
                    Dim product As product = Nothing
                    product = db.products.Find(item.product_id)

                    If product IsNot Nothing Then
                        products.Add(product)
                    End If

                Next

            End If

            Return View("PurchasedProduct", products)
        End Function

        Function CustomOrder() As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim categories As List(Of category) = New List(Of category)
            categories = db.categories.ToList()


            Return View("CustomOrder", categories)
        End Function




        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function CreateCustomOrder(categoryId As Integer, size As String,
                                   description As String, quantity As Integer,
                                   note As String, deliveryDate As DateTime,
                                   address As String, recipient As String,
                                   phone As String) As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            If size.Length = 0 Then
                TempData("size") = "Size description is not blank"
                Return RedirectToAction("CustomOrder")
            End If

            If description.Length = 0 Then
                TempData("description") = "Description is not blank"
                Return RedirectToAction("CustomOrder")
            End If

            If quantity = 0 Then
                TempData("quantity") = "Quantity is not blank"
                Return RedirectToAction("CustomOrder")
            End If

            If deliveryDate.ToShortDateString().Length < 0 Then
                TempData("deliveryDate") = "Delivery date is not blank"
                Return RedirectToAction("CustomOrder")
            End If

            If address.Length = 0 Then
                TempData("address") = "Address is not blank"
                Return RedirectToAction("CustomOrder")
            End If

            If phone.Length = 0 Then
                TempData("phone") = "Phone full name is not blank"
                Return RedirectToAction("CustomOrder")
            End If

            If recipient.Length = 0 Then
                Dim user As user = db.users.Find(userId)
                recipient = user.full_name
            End If

            Dim customOrder As custom_order = New custom_order With {
                .category_id = categoryId,
                .address = address,
                .delivery_date = deliveryDate,
                .description = description,
                .note = note,
                .phone = phone,
                .quantity = quantity,
                .recipient_full_name = recipient,
                .size_description = size,
                .status = False,
                .user_id = userId,
                .register_time = DateTime.Now(),
                .confirm = False
            }

            db.custom_order.Add(customOrder)
            db.SaveChanges()


            Return RedirectToAction("CustomOrderInfo")

        End Function


        Function CustomOrderInfo() As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim customOrders As List(Of custom_order) = Nothing
            customOrders = (From c In db.custom_order
                            Where c.user_id = userId
                            Select c).ToList()


            Return View("CustomOrderInfo", customOrders)
        End Function

        Function NotificationInfo() As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim notifications As List(Of custom_order_notification) = Nothing
            notifications = (From n In db.custom_order_notification
                             Where n.user_id = userId
                             Select n).ToList()



            Return View("Notification", notifications)
        End Function

        Function CustomOrderDetailInfo(id As Integer) As ActionResult

            Dim customOrders As custom_order = Nothing
            customOrders = db.custom_order.Find(id)

            Dim category As category = Nothing
            category = db.categories.Find(customOrders.category_id)

            Dim tupleData As New Tuple(Of custom_order, category)(customOrders, category)

            Return View("CustomOrderDetail", tupleData)
        End Function

        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function ConfirmNotification(notificationId As Integer) As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim notification As custom_order_notification = Nothing
            notification = db.custom_order_notification.Find(notificationId)

            notification.status = True

            db.Entry(notification).State = EntityState.Modified
            db.SaveChanges()

            Return RedirectToAction("NotificationInfo")
        End Function

        Function DeleteNotification(id As Integer) As ActionResult

            Dim notification As custom_order_notification = Nothing
            notification = db.custom_order_notification.Find(id)

            db.custom_order_notification.Remove(notification)
            db.SaveChanges()

            Return RedirectToAction("NotificationInfo")

        End Function

        Function PostPage() As ActionResult

            Dim postDataList As List(Of PostData) = New List(Of PostData)

            Dim postList As List(Of post) = Nothing
            postList = db.posts.ToList()

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

            Return View("Post", postDataList)
        End Function

        Function PostDetail(ByVal id As Integer?) As ActionResult

            Dim post As post = Nothing
            post = db.posts.Find(id)

            Dim postCommentDataList As List(Of PostCommentData) = New List(Of PostCommentData)


            Dim postComments As List(Of post_comment) = Nothing

            postComments = (From p In db.post_comment
                            Where p.post_id = id
                            Select p).ToList()

            For Each item In postComments

                Dim user = db.users.Find(item.user_id)

                Dim userInfo = (From u In db.user_info
                                Where u.user_id = item.user_id
                                Select u).FirstOrDefault()

                Dim commentData As PostCommentData = New PostCommentData With {
                    .image = userInfo.image,
                    .postComment = item,
                    .userName = user.full_name
                }

                postCommentDataList.Add(commentData)

            Next

            Dim tupleData As New Tuple(Of List(Of PostCommentData), post)(postCommentDataList, post)


            Return View("PostDetail", tupleData)
        End Function


        <HttpPost()>
        <ValidateAntiForgeryToken()>
        Function CreateComment(content As String, postId As Integer) As ActionResult

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim postComment As post_comment = New post_comment With {
                .comment = content,
                .post_id = postId,
                .register_date = Date.Now().ToShortDateString(),
                .user_id = userId
            }

            db.post_comment.Add(postComment)
            db.SaveChanges()

            Return RedirectToAction("PostDetail", New With {.id = postId})
        End Function

        Function FilterProduct(productName As String, category As String, minPrice As Integer, maxPrice As Integer) As ActionResult
            Dim products As List(Of product) = Nothing
            products = db.products.ToList()

            Dim filteredProducts As List(Of product) = products.Where(Function(p) _
                (String.IsNullOrEmpty(productName) OrElse p.product_name.Contains(productName)) AndAlso
                (String.IsNullOrEmpty(category) OrElse p.category_id = category) AndAlso
                (p.price >= minPrice AndAlso p.price <= maxPrice)
            ).ToList()

            If Request.Cookies("UserName") Is Nothing Then
                Return RedirectToAction("Login")
            End If

            Dim userInfo = Request.Cookies("UserName")
            Dim userId = Decimal.Parse(userInfo.Value)

            Dim notifications As List(Of custom_order_notification) = Nothing
            notifications = (From n In db.custom_order_notification
                             Where n.user_id = userId And n.status = False
                             Select n).ToList()



            Dim favoriteList As List(Of favorite) = New List(Of favorite)
            favoriteList = (From f In db.favorites
                            Where f.user_id = userId
                            Select f).ToList()

            Dim productDataList As List(Of ProductData) = New List(Of ProductData)

            For Each item In filteredProducts
                Dim productData As ProductData = New ProductData With {
                    .product = item,
                    .isFavorite = False
                }
                For Each itemF In favoriteList
                    If itemF.product_id = item.Id And itemF.status Then
                        productData.isFavorite = True
                        Exit For
                    End If
                Next
                productDataList.Add(productData)
            Next

            Dim categories As List(Of category) = Nothing
            categories = db.categories.ToList()


            Dim tupleData As New Tuple(Of List(Of ProductData), List(Of custom_order_notification), List(Of category))(productDataList, notifications, categories)

            Return View("Product", tupleData)

        End Function

    End Class

End Namespace
