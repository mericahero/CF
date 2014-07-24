Imports CN.COM.QQ
Imports System.Net.Mail

Public Class Email

    Private Shared _DeliveryMethod As System.Net.Mail.SmtpDeliveryMethod
    Shared Sub New()
        _DeliveryMethod = DirectCast(PubFunc.GetDefaultInt(TGConfig.Appset("DeliveryMethod"), 0), System.Net.Mail.SmtpDeliveryMethod)
    End Sub


    ''' <summary>
    ''' 使用系统指定方式，发送系统email给toMail
    ''' </summary>
    ''' <param name="toMail"></param>
    ''' <param name="subject"></param>
    ''' <param name="body"></param>
    ''' <remarks></remarks>
    Public Shared Sub SendSysMail(ByVal toMail As MailAddress, ByVal subject As String, ByVal body As String)
        SendSysMail(toMail, subject, body, Nothing)
    End Sub

    Private Shared Sub SendSysMail(ByVal toMail As MailAddress, ByVal subject As String, ByVal body As String, ByVal fromEmail As String)
        Dim msg As New MailMessage

        With msg
            .To.Add(toMail)
            If fromEmail <> "" Then
                .From = New MailAddress(fromEmail)
            End If
            .Subject = subject
            .Body = body
        End With
        SendMail(msg)
    End Sub

    ''' <summary>
    ''' 使用系统指定方式，发送msg
    ''' </summary>
    ''' <param name="msg"></param>
    ''' <remarks></remarks>
    Public Shared Sub SendMail(ByVal msg As MailMessage)
        Dim m As New SmtpClient
        m.DeliveryMethod = _DeliveryMethod
        Try
            m.Send(msg)
        Catch e As Exception
            ErrorLog.WriteLog("from=" + msg.From.ToString + " to=" & msg.To.Item(0).ToString & " e=" & e.ToString, "email.log")
        End Try
    End Sub
End Class
