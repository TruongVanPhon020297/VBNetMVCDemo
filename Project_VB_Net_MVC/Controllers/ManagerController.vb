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
            Return View("~/Views/managers/CreateProduct.vbhtml")
        End Function

        <HttpPost>
        <ValidateAntiForgeryToken()>
        Function CreateProduct(file As HttpPostedFileBase, productName As String, price As Decimal, description As String) As ActionResult

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
                    .description = description
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
            Dim orders As List(Of order) = New List(Of order)
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

    End Class
End Namespace