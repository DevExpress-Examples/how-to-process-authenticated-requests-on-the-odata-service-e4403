Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Linq
Imports System.Security.Cryptography
Imports System.Security.Principal
Imports System.Text
Imports System.Web

Namespace MyDataService
    Public Class CustomBasicAuth

        Private Class UserInfo
            Private _Name As String
            Public Property Name() As String
                Get
                    Return _Name
                End Get
                Set(ByVal value As String)
                    _Name = value
                End Set
            End Property
            Private _PasswordHash As String
            Public Property PasswordHash() As String
                Get
                    Return _PasswordHash
                End Get
                Set(ByVal value As String)
                    _PasswordHash = value
                End Set
            End Property
            Private _Roles As String()
            Public Property Roles() As String()
                Get
                    Return _Roles
                End Get
                Set(ByVal value As String())
                    _Roles = value
                End Set
            End Property
        End Class

        Private Shared Users As UserInfo() = New UserInfo() {New UserInfo With {.Name = "John", .PasswordHash = "F3U89ry4+MMXyqbbT90tcs18J5Y=", .Roles = New String() {"User", "Admin"}}}

        Private Shared Hasher As HashAlgorithm = New SHA1CryptoServiceProvider()

        Public Shared Sub Authenticate(ByVal context As HttpContext)
            ' NOTE in production, use basic authentication over SSL only!
            'if(!context.Request.IsSecureConnection)
            '    return;
            context.User = Authenticate(context.Request.Headers)
        End Sub

        Public Shared Function Authenticate(ByVal requestHeaders As NameValueCollection) As IPrincipal
            Dim credentials = ParseAuthHeader(requestHeaders("Authorization"))
            If credentials Is Nothing Then
                Return Nothing
            End If

            Return GetPrincipalFromCredentials(credentials(0), credentials(1))
        End Function

        Private Shared Function ParseAuthHeader(ByVal header As String) As String()
            Const headerPrefix As String = "Basic "

            If String.IsNullOrEmpty(header) OrElse (Not header.StartsWith(headerPrefix)) Then
                Return Nothing
            End If

            Dim cred = Encoding.ASCII.GetString(Convert.FromBase64String(header.Substring(headerPrefix.Length))).Split(":"c)
            If cred.Length <> 2 Then
                Return Nothing
            End If

            Return cred
        End Function

        Private Shared Function GetPrincipalFromCredentials(ByVal login As String, ByVal password As String) As IPrincipal
            Dim passwordHash = GetSaltedHash(password)

            Dim user = Users.FirstOrDefault(Function(u)
                                                Return u.Name = login AndAlso u.PasswordHash = passwordHash
                                            End Function)
            If user Is Nothing Then
                Return Nothing
            End If

            Return New GenericPrincipal(New GenericIdentity(user.Name), user.Roles)
        End Function

        Private Shared Function GetSaltedHash(ByVal password As String) As String
            Const salt As String = "WLKvHTeV4RGv" ' any random string
            Return Convert.ToBase64String(Hasher.ComputeHash(Encoding.UTF8.GetBytes(password & salt)))
        End Function
    End Class
End Namespace
