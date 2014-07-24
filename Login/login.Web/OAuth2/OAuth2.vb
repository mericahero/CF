Imports HZTG.Login.Bl
Imports System.Web.Script.Serialization
Imports System.Collections.Generic
Imports CWS

Public Class OAuth2
    Implements IOAuth2
    Private Shared host As String = CWConfig.Appset("OAuth2_Redirect_host")

    Protected Shared Function GetRedirect_uri(otype As Integer) As String
        Return System.Web.HttpUtility.UrlEncode(host + "/class/Login/Usr.aspx/" & otype)
    End Function

    ''' <summary>
    ''' 拆分url到dict
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Shared Function GetDict(s As String) As IDictionary(Of String, String)
        Dim o As New Dictionary(Of String, String)


        For Each t As String In s.Split("&"c)
            Dim a As String() = t.Split("="c)
            If a.Length = 2 Then
                o(a(0)) = System.Web.HttpUtility.UrlDecode(a(1))
            End If
        Next
        Return o
    End Function

    ''' <summary>
    ''' 根据字符串获得Dictionary
    ''' 此处字符串为json格式
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Protected Shared Function GetJsonDict(Of T)(s As String) As T
        Return (New JavaScriptSerializer()).Deserialize(Of T)(s)
    End Function



    ''' <summary>
    ''' 获得实际对象
    ''' </summary>
    ''' <param name="otype"></param>
    ''' <param name="form"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetObj(otype As Integer, form As System.Collections.Specialized.NameValueCollection) As IOAuth2
        Select Case otype
            Case 1
                Return New QQOAuth2(form)
            Case 2
                If form("error_code") IsNot Nothing AndAlso CInt(form("error_code")) = 21330 Then
                    Throw New CWException("您取消了授权！只有授权后才能继续操作")
                End If
                Return New SinaWeiBoOAuth2(form)
        End Select
        Return Nothing
    End Function


    ''' <summary>
    ''' 获得跳转url
    ''' </summary>
    ''' <param name="otype"></param>
    ''' <param name="state"></param>
    ''' <param name="display"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRequestAuthCodeURL(otype As Integer, state As String, display As Integer) As String
        Select Case otype
            Case 1
                Return QQOAuth2.GetRequestAuthCodeURL(state, display)
            Case 2
                Return SinaWeiBoOAuth2.GetRequestAuthCodeURL(state, display)
        End Select
        Return ""
    End Function



    Protected _access_token As String
    Public ReadOnly Property Access_token As String Implements IOAuth2.Access_token
        Get
            Return _access_token
        End Get
    End Property

    Protected _expire As DateTime
    Public ReadOnly Property Expire As Date Implements IOAuth2.Expire
        Get
            Return _expire
        End Get
    End Property

    Protected _ouid As String
    Public ReadOnly Property OUID As String Implements IOAuth2.OUID
        Get
            Return _ouid
        End Get
    End Property

    Protected _otype As Integer
    Public ReadOnly Property Otype As Integer Implements IOAuth2.Otype
        Get
            Return _otype
        End Get
    End Property

    Protected _state As String
    Public ReadOnly Property State As String Implements IOAuth2.State
        Get
            Return _state
        End Get
    End Property

    Protected _refresh_token As String
    Public ReadOnly Property Refresh_token As String Implements Bl.IOAuth2.Refresh_token
        Get
            Return _refresh_token
        End Get
    End Property

End Class

