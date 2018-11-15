<!-- default file list -->
*Files to look at*:

* [CustomBasicAuth.cs](./CS/MyDataService/CustomBasicAuth.cs) (VB: [CustomBasicAuth.vb](./VB/MyDataService/CustomBasicAuth.vb))
* [MyDataService.svc.cs](./CS/MyDataService/MyDataService.svc.cs) (VB: [MyDataService.svc.vb](./VB/MyDataService/MyDataService.svc.vb))
<!-- default file list end -->
# How to process authenticated requests on the OData service


<p>This example demonstrates how to process authenticated requests on the OData service. The main idea is to override the <strong>DataService.OnStartProcessingRequest</strong> method and implement authenticated logic in it.</p>
<p>To access this service, use the following credentials:</p>
<p>UserName: <strong>John</strong><br /> Password: <strong>qwerty</strong></p>
<p><strong>See also:</strong><br /> <a href="http://blogs.msdn.com/b/astoriateam/archive/2010/07/21/odata-and-authentication-part-6-custom-basic-authentication.aspx"><u>OData and Authentication – Part 6 – Custom Basic Authentication</u></a><br /> <a href="http://en.wikipedia.org/wiki/Salt_(cryptography)"><u>Salt (cryptography)</u></a><br /> <a href="https://www.devexpress.com/Support/Center/p/E4460">How to send authenticated requests to the OData service</a><br /><a href="https://www.devexpress.com/Support/Center/p/E4389">How to: Use the XPO OData V3 Service</a> </p>

<br/>


