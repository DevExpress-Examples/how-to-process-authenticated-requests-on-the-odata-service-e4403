Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Data.Services
Imports System.Data.Services.Providers
Imports System.IO
Imports System.Linq
Imports System.Web

Namespace MyDataService
    Public Interface IImageSource
        ReadOnly Property Image() As Byte()
    End Interface

    Public Class ImageStreamProvider
        Implements IDataServiceStreamProvider
        Public Sub DeleteStream(ByVal entity As Object, ByVal operationContext As DataServiceOperationContext) Implements IDataServiceStreamProvider.DeleteStream
            Throw New NotImplementedException()
        End Sub
        Public Function GetReadStream(ByVal entity As Object, ByVal etag As String, ByVal checkETagForEquality? As Boolean, ByVal operationContext As DataServiceOperationContext) As Stream Implements IDataServiceStreamProvider.GetReadStream
            If checkETagForEquality.HasValue Then
                Throw New DataServiceException(400, "This sample service does not support the ETag header for a media resource.")
            End If

            Dim imageSource = TryCast(entity, IImageSource)
            If imageSource Is Nothing Then
                Throw New DataServiceException(500, "Internal Server Error.")
            End If
            Dim image = imageSource.Image
            Return New MemoryStream(image)
        End Function
        Public Function GetReadStreamUri(ByVal entity As Object, ByVal operationContext As DataServiceOperationContext) As Uri Implements IDataServiceStreamProvider.GetReadStreamUri
            Return Nothing
        End Function

        Public Function GetStreamContentType(ByVal entity As Object, ByVal operationContext As DataServiceOperationContext) As String Implements IDataServiceStreamProvider.GetStreamContentType
            Return "image/png"
        End Function

        Public Function GetStreamETag(ByVal entity As Object, ByVal operationContext As DataServiceOperationContext) As String Implements IDataServiceStreamProvider.GetStreamETag
            Return Nothing
        End Function

        Public Function GetWriteStream(ByVal entity As Object, ByVal etag As String, ByVal checkETagForEquality? As Boolean, ByVal operationContext As DataServiceOperationContext) As Stream Implements IDataServiceStreamProvider.GetWriteStream
            Throw New NotImplementedException()
        End Function

        Public Function ResolveType(ByVal entitySetName As String, ByVal operationContext As DataServiceOperationContext) As String Implements IDataServiceStreamProvider.ResolveType
            Return "MyDataService." & entitySetName
        End Function

        Public ReadOnly Property StreamBufferSize() As Integer Implements IDataServiceStreamProvider.StreamBufferSize
            Get
                Return 64000
            End Get
        End Property
    End Class
End Namespace