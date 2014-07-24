Imports CN.COM.QQ

Public Class TGException
    Inherits CN.COM.QQ.QQException

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(errType As enerrtype, message As String)
        MyBase.New(errType, message)
    End Sub

    Public Shared Sub Back(Response As System.Web.HttpResponse)
        Response.Write("<input type=""button"" onclick=""window.history.back(-1)"" value=""返回""/>")
    End Sub

End Class