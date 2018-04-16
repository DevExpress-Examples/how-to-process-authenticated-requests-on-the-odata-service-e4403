Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data.Services
Imports System.Data.Services.Common
Imports System.Data.Services.Providers
Imports System.Linq
Imports System.ServiceModel.Web
Imports System.Web
Imports MyDataService.MyDataService

#If DEBUG Then
<System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults:=True)> _
<MyDataService.JSONPSupportBehavior> _
Public Class DataService
#Else
<MyDataService.JSONPSupportBehavior> _
Public Class DataService
#End If
    Inherits DataService(Of TestDataEntities)
    Implements IServiceProvider
    ' This method is called only once to initialize service-wide policies.
    Public Shared Sub InitializeService(ByVal config As DataServiceConfiguration)
        config.SetEntitySetAccessRule("*", EntitySetRights.All)
        config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3
    End Sub

    Public Function GetService(ByVal serviceType As Type) As Object Implements IServiceProvider.GetService
        If serviceType Is GetType(IDataServiceStreamProvider) Then
            Return New MyDataService.ImageStreamProvider()
        End If
        Return Nothing
    End Function
    Protected Overrides Sub OnStartProcessingRequest(ByVal args As ProcessRequestArgs)
        CustomBasicAuth.Authenticate(HttpContext.Current)
        If HttpContext.Current.User Is Nothing Then
            Throw New DataServiceException(401, "Invalid login or password")
        End If
        MyBase.OnStartProcessingRequest(args)
    End Sub

End Class
