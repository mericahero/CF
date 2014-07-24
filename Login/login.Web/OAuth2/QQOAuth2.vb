Imports System.Collections.Specialized
Imports HZTG.Login.Bl
Imports COM.CF
Imports CWS

Public Class QQOAuth2
    Inherits OAuth2


    ''' <summary>
    ''' AppID
    ''' </summary>
    Private Shared AppID As String

    ''' <summary>
    ''' AppKey
    ''' </summary>
    Private Shared AppKey As String

    Private Shared redirect_uri As String


    Shared Sub New()
        '_otype = 1
        AppID = CWConfig.Appset.Get("QQAppID")
        AppKey = CWConfig.Appset.Get("QQAppKey")
        redirect_uri = OAuth2.GetRedirect_uri(1)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="state"></param>
    ''' <param name="display">1 手机页面</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function GetRequestAuthCodeURL(state As String, display As Integer) As String
        Return String.Format("https://graph.qq.com/oauth2.0/authorize?scope=get_user_info,add_share,check_page_fans,add_t,add_pic_t,del_t,get_repost_list,get_info,get_other_info,get_fanslist,get_idollist,add_idol,del_idol&response_type=code&client_id={0}&redirect_uri={1}&state={2}&display={3}", _
                                  AppID, redirect_uri, state, IIf(display = 1, "mobile", ""))
    End Function


    Public Sub New(form As NameValueCollection)
        _otype = 1
        'QQ登录没有Refresh_token，设置为空
        _refresh_token = ""
        _state = form("state")
        If _state = "" Then
            Throw New CFException("state非法！")
        End If
        _state = _state.Replace("'", "")

        Dim url As String = String.Format("https://graph.qq.com/oauth2.0/token?grant_type=authorization_code&client_id={0}&client_secret={1}&code={2}&state={3}&redirect_uri={4}", _
                                  AppID, AppKey, form("code"), State, System.Web.HttpContext.Current.Request.Url.GetComponents(UriComponents.Path Or UriComponents.SchemeAndServer, UriFormat.Unescaped))

        Dim s As String = PubFunc.SendHTTP(url)

        Dim o As Generic.IDictionary(Of String, String) = OAuth2.GetDict(s)

        If Not o.TryGetValue("access_token", _access_token) OrElse _access_token = "" Then
            Throw New CWException("无法获得access_token=" + s)
        End If
        Dim e As String = ""
        If Not o.TryGetValue("expires_in", e) OrElse e = "" Then
            Throw New CWException("无法获得expires_in=" + s)
        End If
        _expire = Now.AddSeconds(PubFunc.GetInt(e))

        s = PubFunc.SendHTTP("https://graph.z.qq.com/moc2/me?access_token=" + _access_token)
        o = OAuth2.GetDict(s)

        If Not o.TryGetValue("openid", _ouid) OrElse _ouid = "" Then
            Throw New CWException("无法获得ouid=" + s)
        End If
    End Sub

End Class
