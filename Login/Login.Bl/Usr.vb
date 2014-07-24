Option Strict On

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports System.Collections.Specialized
Imports CWS
Imports COM.CF


Public Class Usr
    Implements IUsr
#Region "构造方法"
    ''' <summary>
    ''' 无参构造方法
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' 构造方法
    ''' </summary>
    ''' <param name="uid"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal uid As Integer)
        m_uid = uid
        m_logname = DirectCast(UsrInfoRow("logname"), String)
    End Sub
#End Region

#Region "属性，包括用户ID，用户昵称，用户标志"
    Private m_uid As Integer, _otherbz As Integer
    Private m_logname As String
    Public ReadOnly Property Logname As String Implements IUsr.Logname
        Get
            Return m_logname
        End Get
    End Property

    Private _homeid As Integer
    Public ReadOnly Property HomeID() As Integer Implements IUsr.HomeID
        Get
            Return _homeid
        End Get
    End Property


    Private _name As String
    Public ReadOnly Property Name() As String Implements IUsr.Name
        Get
            If _name Is Nothing Then
                _name = DirectCast(UsrInfoRow("name"), String)
            End If
            Return _name
        End Get
    End Property



    Public ReadOnly Property OtherBZ() As Integer Implements IUsr.OtherBZ
        Get
            Return _otherbz
        End Get
    End Property

    Private _UsrInfoRow As DataRow

    Public ReadOnly Property UID() As Integer Implements IUsr.UID
        Get
            Return m_uid
        End Get
    End Property



    Public ReadOnly Property UsrInfoRow() As DataRow
        Get
            If _UsrInfoRow Is Nothing Then
                _UsrInfoRow = PubFunc.GetSQLSingleRow("select u.* from usr_info u where u.uid=" & m_uid, CFConfig.GetNotOpenConnection)
                If _UsrInfoRow Is Nothing Then
                    Throw New CFException(enErrType.NormalError, "非法UID！" & m_uid)
                End If
            End If
            Return _UsrInfoRow
        End Get
    End Property

#End Region

#Region "用户登录 修改密码"
    ''' <summary>
    ''' FormLogin
    ''' </summary>
    ''' <param name="logname"></param>
    ''' <param name="passwd"></param>
    ''' <param name="ip"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FormLogin(ByVal logname As String, ByVal passwd As String, ip As String) As Boolean
        Dim r = FormLoginNoExcept(logname, passwd, ip)
        If r >= 0 Then Return True

        Select Case r
            Case -8
                Throw New CFException(enErrType.PasswdError, "该用户因为违反中国法律或者站规，已经被禁用！")
            Case Else
                Throw New CFException(enErrType.PasswdError, "口令或者用户名错误！")
        End Select
    End Function

    ''' <summary>
    ''' formlogin不会抛出异常
    ''' </summary>
    ''' <param name="logname"></param>
    ''' <param name="passwd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FormLoginNoExcept(ByVal logname As String, ByVal passwd As String, browser As String) As Integer
        logname = logname.Trim
        If browser Is Nothing Then
            browser = ""
        ElseIf browser.Length > 255 Then
            browser = browser.Substring(0, 255)
        End If

        Dim cm_login As New SqlClient.SqlCommand("p_usr_login", CWConfig.UserDB.GetConnection)
        Try
            cm_login.CommandType = CommandType.StoredProcedure
            With cm_login.Parameters
                .Add("@logname", SqlDbType.VarChar, 50).Value = logname
                .Add("@passwd", SqlDbType.Char, 32).Value = CWPub.Md5Hash(logname + passwd)


                .Add("@ipint", SqlDbType.Int).Value = CWPub.GetIPAsInt32(FWFunc.GetIP)

                .Add("@browser", SqlDbType.VarChar, 255).Value = browser

                .Add("@name", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output

                .Add("@uid", SqlDbType.Int).Direction = ParameterDirection.Output

                .Add("@otherbz", SqlDbType.Int).Direction = ParameterDirection.Output
                .Add("@homeid", SqlDbType.Int).Direction = ParameterDirection.Output
            End With

            cm_login.ExecuteNonQuery()

            With cm_login.Parameters
                'm_cid = cid
                m_uid = DirectCast(.Item("@uid").Value, Integer)

                If IsDBNull(.Item("@name").Value) Then
                    If m_uid = -8 Then
                        Return -8
                    Else
                        Return -1
                    End If
                End If

                m_logname = DirectCast(.Item("@logname").Value, String)
                _name = DirectCast(.Item("@name").Value, String)
                _otherbz = DirectCast(.Item("@otherbz").Value, Integer)
                _homeid = DirectCast(.Item("@homeid").Value, Integer)

            End With

        Finally
            cm_login.Connection.Close()
        End Try
        Return 0
    End Function

    ''' <summary>
    ''' 使用用户信息登录 包括用户手机 E-mail
    ''' </summary>
    ''' <param name="type">用户登录的类型 3 手机登陆 4 e-mail登录</param>
    ''' <param name="logstr"></param>
    ''' <param name="passwd"></param>
    ''' <param name="browser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UsrInfoLoginNoExcept(ByVal type As Integer, ByVal logstr As String, ByVal passwd As String, browser As String) As Integer
        logstr = logstr.Trim.Replace("'", "")
        Dim logname = Nothing
        Select Case type
            Case 3
                logname = CWConfig.UserDB.ExecuteScalar("select logname from usr_info where sj='" & logstr & "'")
            Case 4
                logname = CWConfig.UserDB.ExecuteScalar("select logname from usr_info where email='" & logstr & "'")
            Case Else
                Return -11
        End Select

        '-10不存在用户信息
        If logname Is Nothing Then Return -10

        Return FormLoginNoExcept(CStr(logname), passwd, browser)
    End Function



    ''' <summary>
    ''' 修改口令
    ''' </summary>
    ''' <param name="uid"></param>
    ''' <param name="form"></param>
    ''' <remarks></remarks>
    Public Shared Sub ChangePasswd(ByVal uid As Integer, ByVal form As System.Collections.Specialized.NameValueCollection)
        Dim cm As New SqlCommand("update register set passwd=@passwd where uid=" & uid, CFConfig.GetConnection)
        Try
            With cm.Parameters
                .Add("@passwd", SqlDbType.VarChar, 10).Value = GetPasswd(form)
            End With
            cm.ExecuteNonQuery()
        Finally
            cm.Connection.Close()
        End Try
    End Sub
#End Region

#Region "第三方处理 登录 绑定"
    ''' <summary>
    ''' 跳转到第三方登录链接时，记录下随机生成的state用以保存gourl 等
    ''' </summary>
    ''' <param name="f"></param>
    ''' <param name="state"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GoOther(f As NameValueCollection, state As Guid) As Integer
        Dim fromtype = PubFunc.GetInt(f("fromtype"))
        If fromtype < Int16.MinValue OrElse fromtype > Int16.MaxValue Then
            fromtype = 0
        End If
        Dim fromid = PubFunc.GetInt(f("fromid"))
        Dim cm As New SqlCommand("p_otherlogin_stateAdd", CWConfig.SessionDB.GetConnection())
        Try
            With cm
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@state", SqlDbType.UniqueIdentifier).Value = state
                .Parameters.Add("@gourl", SqlDbType.VarChar, 255).Value = System.Web.HttpUtility.HtmlEncode(CFConfig.GetStr(f, "autogo", 255))
                .Parameters.Add("@host", SqlDbType.VarChar, 255).Value = System.Web.HttpUtility.HtmlEncode(CFConfig.GetStr(f, "host", 255))
                .Parameters.Add("@fromtype", SqlDbType.SmallInt).Value = fromtype
                .Parameters.Add("@fromid", SqlDbType.Int).Value = fromid
                .Parameters.Add("@delay", SqlDbType.Int).Value = PubFunc.GetInt(f("delay"))
                .Parameters.Add("@otype", SqlDbType.TinyInt).Value = PubFunc.GetInt(f("otype"))

                '新增字段
                .Parameters.Add("@subact", SqlDbType.VarChar, 50).Value = System.Web.HttpUtility.HtmlEncode(CFConfig.GetStr(f, "subact", 50))
                .Parameters.Add("@othercanshu", SqlDbType.VarChar, 8000).Value = System.Web.HttpUtility.HtmlEncode(CFConfig.GetStr(f, "othercanshu", 8000))


                .Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                .ExecuteNonQuery()
            End With
            Return CInt(cm.Parameters("@RETURN_VALUE").Value)
        Finally
            cm.Connection.Close()
        End Try
    End Function

    ''' <summary>
    ''' 第三方第一次登陆，自动生成新用户，否则直接返回对应用户
    ''' </summary>
    Public Function OtherLoginNoExcept(o As IOAuth2, fromtype As Integer, fromid As Integer) As Integer

        Dim timeNow = DateTime.UtcNow
        Dim cm As New SqlCommand("p_usr_otherlogin", CWConfig.UserDB.GetConnection())
        Try

            cm.CommandType = CommandType.StoredProcedure
            With cm.Parameters
                .Add("@fromtype", SqlDbType.SmallInt).Value = fromtype
                .Add("@fromid", SqlDbType.Int).Value = fromid
                .Add("@ipint", SqlDbType.Int).Value = CWPub.GetIPAsInt32(FWFunc.GetIP)

                .Add("@otype", SqlDbType.TinyInt).Value = o.Otype
                .Add("@ouid", SqlDbType.VarChar, 50).Value = o.OUID
                .Add("@access_token", SqlDbType.VarChar, 8000).Value = o.Access_token

                '新增字段
                .Add("@refresh_token", SqlDbType.VarChar, 8000).Value = o.Refresh_token
                .Add("@token_expire", SqlDbType.SmallDateTime).Value = o.Expire

                .Add("@browser", SqlDbType.VarChar, 255).Value = ""


                .Add("@otherbz", SqlDbType.Int).Direction = ParameterDirection.Output
                .Add("@homeid", SqlDbType.Int).Direction = ParameterDirection.Output
                .Add("@logname", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                .Add("@name", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output

                .Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

            End With
            cm.ExecuteNonQuery()

            With cm.Parameters
                m_uid = DirectCast(.Item("@RETURN_VALUE").Value, Integer)

                If m_uid <= 0 Then
                    '出错！

                End If

                m_logname = DirectCast(.Item("@logname").Value, String)
                _name = DirectCast(.Item("@name").Value, String)
                _otherbz = DirectCast(.Item("@otherbz").Value, Integer)
                _homeid = DirectCast(.Item("@homeid").Value, Integer)
            End With
        Finally
            cm.Connection.Close()
        End Try

        Return 0
    End Function

    ''' <summary>
    ''' 用户绑定第三方登录账号
    ''' </summary>
    ''' <param name="uid">当前登录用户名</param>
    ''' <param name="o">第三方登录用户信息</param>
    ''' <returns>
    ''' 	用户不存在 返回-1
    ''' 	用户已绑定该平台账号 返回-2
    ''' 	第三方账号已被使用	返回-3
    ''' </returns>
    ''' <remarks></remarks>
    Public Shared Function Bind(uid As Integer, o As IOAuth2) As Integer
        Dim cm As New SqlCommand("p_usr_bind", CWConfig.UserDB.GetConnection())
        Try
            cm.CommandType = CommandType.StoredProcedure
            With cm.Parameters
                .Add("@uid", SqlDbType.Int).Value = uid


                .Add("@otype", SqlDbType.TinyInt).Value = o.Otype
                .Add("@ouid", SqlDbType.VarChar, 50).Value = o.OUID
                .Add("@access_token", SqlDbType.VarChar, 8000).Value = o.Access_token


                '新增字段
                .Add("@refresh_token", SqlDbType.VarChar, 8000).Value = o.Refresh_token
                .Add("@token_expire", SqlDbType.SmallDateTime).Value = o.Expire


                .Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            End With
            cm.ExecuteNonQuery()
            Return CInt(cm.Parameters("@RETURN_VALUE").Value)
        Finally
            cm.Connection.Close()
        End Try
    End Function

#End Region

#Region "验证处理，包括验证用户名，得到昵称,得到用户密码"

    ''' <summary>
    ''' 简单检查logname，并引发异常
    ''' </summary>
    ''' <param name="logname"></param>
    ''' <remarks></remarks>
    Public Shared Sub SimpleCheckLogname(ByVal logname As String)
        If logname Is Nothing OrElse logname = "" OrElse logname.IndexOf("'"c) >= 0 Then
            Throw New CFException(enErrType.NormalError, "非法logname ！=" & logname)
        End If
    End Sub

    ''' <summary>
    ''' 简单检查lognames，并引发异常
    ''' </summary>
    ''' <param name="lognames"></param>
    ''' <remarks></remarks>
    Public Shared Sub SimpleCheckLognames(ByVal lognames As String)
        If lognames Is Nothing OrElse lognames = "" OrElse Regex.IsMatch(lognames, "[^\w,@]", RegexOptions.ECMAScript) Then
            Throw New CFException(enErrType.NormalError, "非法lognames ！=" & lognames)
        End If
    End Sub
    ''' <summary>
    ''' 检查并返回登录名，不符合抛出异常
    ''' </summary>
    ''' <param name="logname"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function CheckLogname(ByVal logname As String) As String
        '注册ID
        If logname Is Nothing Then Throw New CFException(enErrType.NormalError, "注册ID必须提供！")
        logname = logname.Trim
        If logname.Length < 3 OrElse logname.Length > 18 Then Throw New CFException(enErrType.NormalError, "注册ID必须是3-18个字符")
        If Regex.IsMatch(logname, "\W", RegexOptions.ECMAScript) Then
            Throw New CFException(enErrType.NormalError, "注册ID必须是英文字母数字或者下划线组成！")
        End If
        Return logname
    End Function

    ''' <summary>
    ''' 得到昵称
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getnickname(ByVal s As String) As String
        Return s.Replace(","c, "，"c)
    End Function



    ''' <summary>
    ''' 得到口令
    ''' passwd1,passwd2
    ''' </summary>
    ''' <param name="form"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetPasswd(ByVal form As System.Collections.Specialized.NameValueCollection) As String
        '口令
        Dim passwd As String = form("passwd1")
        If passwd Is Nothing Then Throw New CFException(enErrType.NormalError, "口令必须提供！")
        If passwd <> form("passwd2") Then
            Throw New CFException(enErrType.NormalError, "口令不一致！")
        End If
        passwd = passwd.Trim
        If passwd.Length < 5 OrElse passwd.Length > 10 Then Throw New CFException(enErrType.NormalError, "口令必须是5-10个字符！")
        Return passwd
    End Function

#End Region

#Region "Cookie"
    ''' <summary>
    ''' 设置cid和uid关联以及uuid和bz
    ''' </summary>
    Public Function SetCookie(ByVal guid1 As Guid, form As NameValueCollection) As Guid
        '设置数据库
        Dim cm_login As New SqlClient.SqlCommand("p_setSession", CWConfig.SessionDB.GetConnection)
        Try
            cm_login.CommandType = CommandType.StoredProcedure
            With cm_login.Parameters
                With .Add("@guid", SqlDbType.UniqueIdentifier)
                    .Value = IIf(guid1 = Guid.Empty, DBNull.Value, guid1)
                    .Direction = ParameterDirection.InputOutput
                End With

                .Add("@uid", SqlDbType.Int).Value = UID
                .Add("@logname", SqlDbType.VarChar, 50).Value = Me.Logname
                .Add("@name", SqlDbType.NVarChar, 50).Value = Me.Name
                .Add("@otherbz", SqlDbType.Int).Value = Me._otherbz
                .Add("@homeid", SqlDbType.Int).Value = Me.HomeID

                If form("autologin") = "1" Then
                    .Add("@bz", SqlDbType.Int).Value = 3
                ElseIf form("jiyi") = "1" Then
                    .Add("@bz", SqlDbType.Int).Value = 2
                Else
                    .Add("@bz", SqlDbType.Int).Value = 0
                End If

                '.Add("@returnvalue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                cm_login.ExecuteNonQuery()

                Dim o = .Item("@guid").Value
                If IsDBNull(o) Then
                    SetCookie = Guid.Empty
                Else
                    SetCookie = DirectCast(.Item("@guid").Value, Guid)
                End If

            End With
        Finally
            cm_login.Connection.Close()
        End Try
    End Function


    ''' <summary>
    ''' 设置cid和uid关联以及uuid和bz
    ''' </summary>
    ''' <param name="guid1"></param>
    ''' <remarks></remarks>
    Public Shared Sub DelCookie(ByVal guid1 As Guid)
        '设置数据库
        Dim cm_login As New SqlClient.SqlCommand("p_delSession", CWConfig.SessionDB.GetConnection)
        Try
            cm_login.CommandType = CommandType.StoredProcedure
            With cm_login.Parameters
                .Add("@guid", SqlDbType.UniqueIdentifier).Value = guid1
                cm_login.ExecuteNonQuery()
            End With
        Finally
            cm_login.Connection.Close()
        End Try
    End Sub
#End Region









End Class
