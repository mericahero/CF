Option Strict On
Imports System.Web
Imports System.Data.SqlClient
Imports HZTG
Imports HZTG.Login
Imports HZTG.Login.Bl
Imports COM.CF.Web
Imports COM.CF
Imports CWS


Public Class Usr
    Inherits CFTL.CFCtrlPage

    Private oUsr As Login.Bl.Usr

#Region "EnterIn方法"
    'Dim Response As Object
    'Dim WebForm As Object

    ''' <summary>
    ''' 默认方法EnterIn
    ''' </summary>
    ''' <remarks></remarks>
    <Page(enPageType.SelfPage)> Private Sub EnterIn()

        '如果提供了otype则转到otherlogin
        Dim otype As Integer = 0, r As Integer = 0
        If Request.PathInfo IsNot Nothing AndAlso Request.PathInfo <> "" Then
            otype = PubFunc.GetInt(Request.PathInfo.Substring(1))
        End If

        Select Case otype
            Case 0  '默认
                r = FormLoginNoExcept()
            Case 3, 4  '3 手机 4 邮箱
                r = UsrInfoLoginNoExcept(otype)
            Case Else
                OtherLogin(otype)
                Return
        End Select



        If r < 0 Then
            With Context
                .Items("errorCode") = r
                .Server.Execute("/login/loginError.aspx", True)
            End With
            Return
        End If



        setCookies()
        Response.Write("<html><head><META HTTP-EQUIV=""Content-Type"" CONTENT=""text/html; charset=utf-8""></head><body>")
        Response.Write("<form id=f action='" + System.Web.HttpUtility.HtmlEncode(RequestForm("host")) + "/class/WebFunc/FromOther.aspx' method='POST'>")
        Response.Write("<input type=hidden name='logname' value='" + oUsr.Logname + "'>")
        Response.Write("<input type=hidden name='nickname' value='" + oUsr.Name + "'>")
        Response.Write("<input type=hidden name='usrbz' value='" + oUsr.OtherBZ.ToString("x") + "'>")
        Response.Write("<input type=hidden name='autologin' value='" + System.Web.HttpUtility.HtmlEncode(RequestForm("autologin")) + "'>")
        Response.Write("<input type=hidden name='guid' value='" + guidStr + "'>")
        Response.Write("<input type=hidden name='uid' value='" + oUsr.UID.ToString + "'>")
        Response.Write("<input type=hidden name='jiyi' value='" + System.Web.HttpUtility.HtmlEncode(RequestForm("jiyi")) + "'>")
        Response.Write("<input type=hidden name='autogo' value='" + System.Web.HttpUtility.HtmlEncode(RequestForm("autogo")) + "'>")
        Response.Write("<input type=hidden name='delay' value='" + System.Web.HttpUtility.HtmlEncode(RequestForm("delay")) + "'>")
        Response.Write("<input type=submit value='继续'>")
        Response.Write("</form><script>f.submit()</script></body></html>")

    End Sub
#End Region

#Region "用户登录 登出 修改口令"


    ''' <summary>
    ''' 登陆
    ''' </summary>
    ''' <remarks></remarks>
    <Page(enPageType.DefaultPage)> Private Sub Login()
        If Not FormLogin() Then Return
        setCookies()

        Me.WebForm.WriteOK("<B>" + oUsr.Name + "</b>(" + Me.oUsr.Logname + ")")
        Response.Write("<center><BR>您已经登录！</center>")

        Me.WebForm.AutoGo()

    End Sub

    Private Function FormLogin() As Boolean
        'If Not AuthCode.Check(RequestForm) Then
        '    Throw New TGException("验证码不对！")
        'End If

        Dim logname As String = RequestForm("logname")
        If logname Is Nothing OrElse logname.Trim = "" Then
            '非form login
            Return False
        End If

        Dim passwd As String = RequestForm("passwd")

        oUsr = New Login.Bl.Usr

        Return oUsr.FormLogin(logname, passwd, FWFunc.GetIP)

    End Function

    ''' <summary>
    ''' 用户使用登录名表单登录不抛出异常
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function FormLoginNoExcept() As Integer
        If Not AuthCode.Check(RequestForm) Then
            Return -2
        End If

        Dim logname As String = RequestForm("logname")
        If logname Is Nothing OrElse logname.Trim = "" Then
            '非form login
            Return -1
        End If

        Dim passwd As String = RequestForm("passwd")

        oUsr = New Login.Bl.Usr

        Return oUsr.FormLoginNoExcept(logname, passwd, Request.UserAgent)

    End Function
    ''' <summary>
    ''' 用户使用用户信息登录 包括用户手机 E-mail
    ''' </summary>
    ''' <param name="type">用户登录类型 3 手机 4 E-mail</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UsrInfoLoginNoExcept(type As Integer) As Integer
        If Not AuthCode.Check(RequestForm) Then
            Return -2
        End If

        Dim logstr = RequestForm("logstr")
        If logstr Is Nothing OrElse logstr.Trim = "" Then
            '账号为空
            Return -1
        End If

        oUsr = New Bl.Usr

        Return oUsr.UsrInfoLoginNoExcept(type, logstr, RequestForm("passwd"), Request.UserAgent)
    End Function


    ''' <summary>
    ''' XMLLogin
    ''' </summary>
    ''' <remarks></remarks>
    <Page(enPageType.XMLPage)> Private Sub XMLLogin()
        If Not FormLogin() Then Return

        setCookies()

        Response.Write("<usr uid='" & UsrInfo.UID & "' logname='" + Me.UsrInfo.Account + "' name='" + Me.UsrInfo.Name + "' />")
    End Sub

    ''' <summary>
    ''' LogOut
    ''' </summary>
    ''' <remarks></remarks>
    <Page(enPageType.DefaultPage, True)> Private Sub LogOut()
        Dim bz = CFCookies.GetIntCookie("bz")
        If bz = 3 Then
            '如果是自动登录，则改为记忆
            CFCookies.SetCookie("bz", "2", Now.AddYears(1))
        End If

        CFCookies.ClearCookie("guid")

        CFCookies.ClearCookie("L")

        Bl.Usr.DelCookie(CWPub.GetCookieGUID)

        Me.WebForm.AutoGo("/", 1)
    End Sub


    ''' <summary>
    ''' 修改口令
    ''' </summary>
    ''' <remarks></remarks>
    <Page(enPageType.DefaultPage)> Private Sub ChangePasswd()
        If Not FormLogin() Then Return

        Bl.Usr.ChangePasswd(UsrInfo.UID, RequestForm)

        Me.WebForm.WriteOK("修改成功！")

        Me.WebForm.AutoGo("autoclose")

    End Sub

#End Region

#Region "第三方登录处理 登录 绑定"
    ''' <summary>
    ''' 跳转到其他登录
    ''' </summary>
    ''' <remarks></remarks>
    <Page(enPageType.DefaultPage)> Private Sub GoOther()
        '保存信息，构造跳转连接
        Dim state = Guid.NewGuid()
        Dim o = HZTG.Login.Bl.Usr.GoOther(RequestForm, state)
        If o < 0 Then
            Select Case o
                Case Else
                    Throw New CWException(enErrType.NormalError, SysErrMsg.GetErrMsg(o))
            End Select
        Else
            Dim otype = PubFunc.GetInt(RequestForm("otype"))
            Dim url = OAuth2.GetRequestAuthCodeURL(otype, state.ToString, PubFunc.GetInt(RequestForm("display")))
            Response.Redirect(url)
        End If
    End Sub

    ''' <summary>
    ''' 其他登陆
    ''' </summary>
    ''' <param name="otype"></param>
    ''' <remarks></remarks>
    Private Sub OtherLogin(ByVal otype As Integer)
        Dim o = OAuth2.GetObj(otype, RequestForm)

        Dim row As DataRow = CWConfig.SessionDB.GetSQLSingleRow("select * from oauth_temp where state='" & o.State & "'")
        CWConfig.SessionDB.ExecuteNonQuery("delete from oauth_temp where state='" & o.State & "'")
        Select Case row("subact").ToString
            Case "bind"
                Bind(o, row)
                Exit Sub
        End Select

        Dim autogo As String = "", host As String = "", delay As Integer = 0, fromtype As Integer = 0, fromid As Integer = 0
        If row IsNot Nothing Then
            autogo = row("gourl").ToString()
            host = row("host").ToString()
            delay = PubFunc.GetInt(row("delay").ToString())
            fromtype = PubFunc.GetInt(row("fromtype").ToString())
            fromid = PubFunc.GetInt(row("fromid").ToString())
        End If


        '依据otype获得code，调用对方接口，获得access_token,ouid等信息


        '根据ouid获得对应用户信息，第一次登陆的用户已经自动建立
        oUsr = New Login.Bl.Usr
        oUsr.OtherLoginNoExcept(o, fromtype, fromid)


        setCookies()

        Response.Write("<html><head><META HTTP-EQUIV=""Content-Type"" CONTENT=""text/html; charset=utf-8""></head><body>")
        Response.Write("<form id=f action='" & host & "/class/WebFunc/FromOther.aspx' method='POST'>")
        Response.Write("<input type=hidden name='logname' value='" & oUsr.Logname & "'>")
        Response.Write("<input type=hidden name='nickname' value='" & oUsr.Name & "'>")
        Response.Write("<input type=hidden name='usrbz' value='" & oUsr.OtherBZ.ToString("x") & "'>")
        Response.Write("<input type=hidden name='guid' value='" & guidStr & "'>")
        Response.Write("<input type=hidden name='uid' value='" & oUsr.UID.ToString & "'>")



        '从保存的状态信息中获得autogo,delay
        Response.Write("<input type=hidden name='autogo' value='" & autogo & "'>")
        Response.Write("<input type=hidden name='delay' value='" & delay & "'>")

        Response.Write("<input type=submit value='继续'>")
        Response.Write("</form><script>f.submit()</script></body></html>")
        'WebForm.AutoGo(autogo, delay)

    End Sub

    ''' <summary>
    ''' 绑定账户
    ''' </summary>
    ''' <param name="o"></param>
    ''' <param name="row"></param>
    ''' <remarks>
    ''' 	用户不存在 返回-1
    ''' 	用户已绑定该平台账号 返回-2
    ''' 	第三方账号已被使用	返回-3
    ''' </remarks>
    Private Sub Bind(ByVal o As IOAuth2, ByVal row As DataRow)
        UsrLogin.MustLogin()
        Dim r As Int32 = Bl.Usr.Bind(UsrInfo.UID, o)
        If r < 0 Then
            Select Case r
                Case -1
                    WebForm.WriteErrorNoEnd(enErrType.NormalError, "用户不存在")
                Case -2
                    WebForm.WriteErrorNoEnd(enErrType.NormalError, "用户已绑定该平台账号(平台ID:)" & o.Otype)
                Case -3
                    WebForm.WriteErrorNoEnd(enErrType.NormalError, "第三方账号已被使用(平台ID)" & o.Otype)
                Case Else
                    WebForm.WriteErrorNoEnd(enErrType.NormalError, SysErrMsg.GetErrMsg(r))
            End Select

        Else
            WebForm.WriteOK("绑定成功！")
            WebForm.AutoGo(row("gourl").ToString, PubFunc.GetInt(row("delay").ToString()))
        End If
    End Sub
#End Region

#Region "用户XML格式的信息"
    '<Page(enPageType.XMLPage)> Private Sub UsrInfoXML()
    '    Dim o As New UsrCookieInfo
    '    With Response
    '        .Write("<usr bz='")
    '        .Write(o.Bz)

    '        Select Case o.Bz
    '            Case enUsrCookieBz.NewUID, enUsrCookieBz.UnkownUID
    '            Case Else
    '                .Write("' logname='")
    '                .Write(o.Logname)
    '                .Write("' name='")
    '                .Write(o.Name)
    '                .Write("' loginN='")
    '                .Write(o.LoginNumber)
    '                .Write("' haveExpress='")
    '                If o.haveExpress Then
    '                    Response.Write("1'")
    '                Else
    '                    Response.Write("0'")
    '                End If
    '        End Select
    '        .Write(" />")
    '    End With
    'End Sub
#End Region

#Region "Cookie"
    Private guidStr As String

    Private Sub setCookies()
        Dim guid1 As System.Guid = CWPub.GetCookieGUID

        '更新数据库
        guid1 = oUsr.SetCookie(guid1, RequestForm)
        If guid1 = System.Guid.Empty Then
            Throw New CFException("生成guid1出错！")
        End If
        guidStr = guid1.ToString

        If RequestForm("jiyi") = "1" OrElse RequestForm("autologin") = "1" Then
            '记忆或者自动登录
            Dim e = Now.AddYears(1) '1年过期

            CFCookies.SetCookie("uid", oUsr.UID.ToString, e)
            CFCookies.SetCookie("logname", oUsr.Logname, e)
            CFCookies.SetCookie("name", HttpUtility.UrlEncode(oUsr.Name, System.Text.Encoding.UTF8), e)
            CFCookies.SetCookie("usrbz", oUsr.OtherBZ.ToString("x"), e)

            If RequestForm("autologin") = "1" Then
                '自动登录
                CFCookies.SetCookie("guid", guidStr, e)
                CFCookies.SetCookie("bz", "3", e)
            Else
                CFCookies.SetCookieNoExpires("guid", guidStr)
                CFCookies.SetCookieNoExpires("L", "1")
                CFCookies.SetCookie("bz", "2", e)
            End If
        Else
            '不记忆
            CFCookies.SetCookieNoExpires("guid", guidStr)
            CFCookies.SetCookieNoExpires("L", "1")

            '始终记忆uid！！！
            CFCookies.SetCookie("uid", oUsr.UID.ToString, Now.AddYears(1))

            CFCookies.SetCookieNoExpires("logname", oUsr.Logname)
            CFCookies.SetCookieNoExpires("name", HttpUtility.UrlEncode(oUsr.Name, System.Text.Encoding.UTF8))
            CFCookies.SetCookieNoExpires("usrbz", oUsr.OtherBZ.ToString("x"))

            CFCookies.ClearCookie("bz")
        End If
    End Sub

#End Region

#Region "其他方法"
    Private Function GetPathID() As Integer
        Dim p As String = Request.PathInfo
        If p = "" Then Return PubFunc.GetInt(RequestForm("id"))

        Dim i As Integer = p.IndexOf("."c)
        If i >= 0 Then
            Return PubFunc.GetInt(p.Substring(1, i - 1))
        End If
        Return 0
    End Function
#End Region

End Class
