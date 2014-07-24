Imports System.Collections.Specialized
Imports HZTG.Login.Bl
Imports COM.CF
Imports CWS

Public Class SinaWeiBoOAuth2
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
        '_otype = 2
        AppID = CWConfig.Appset.Get("SinaWeiBoAppID")
        AppKey = CWConfig.Appset.Get("SinaWeiBoAppKey")
        redirect_uri = OAuth2.GetRedirect_uri(2)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="state"></param>
    ''' <param name="display"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Shared Function GetRequestAuthCodeURL(state As String, display As Integer) As String
        Dim displayDic As New Dictionary(Of Integer, String)()
        displayDic.Add(0, "default")
        displayDic.Add(1, "mobile")
        displayDic.Add(2, "popup")
        displayDic.Add(3, "wap1.2")
        displayDic.Add(4, "wap2.0")
        displayDic.Add(5, "js")
        displayDic.Add(6, "apponweibo")
        Return String.Format("https://api.weibo.com/oauth2/authorize?client_id={0}&response_type=code&redirect_uri={1}&state={2}&display={3}", _
                             New Object() {AppID, redirect_uri, state, displayDic(display)})
    End Function


    Public Sub New(form As NameValueCollection)
        _otype = 2
        '新浪微博登录没有Refresh_token，设置为空
        _refresh_token = ""
        _state = form("state")
        If _state = "" Then
            Throw New CFException("state非法！")
        End If
        _state = _state.Replace("'", "")


        Dim url = "https://api.weibo.com/oauth2/access_token"

        Dim f As New NameValueCollection()
        f.Add("client_id", AppID)
        f.Add("client_secret", AppKey)
        f.Add("grant_type", "authorization_code")
        f.Add("code", form("code"))
        f.Add("redirect_uri", System.Web.HttpContext.Current.Request.Url.GetComponents(UriComponents.Path Or UriComponents.SchemeAndServer, UriFormat.Unescaped))

        Dim s As String = PubFunc.PostHTTP(url, f)

        Dim o = OAuth2.GetJsonDict(Of Dictionary(Of String, String))(s)

        If Not o.TryGetValue("access_token", _access_token) OrElse _access_token = "" Then
            Throw New CFException("无法获得access_token=" + s)
        End If
        Dim e As String = ""
        If Not o.TryGetValue("expires_in", e) OrElse e = "" Then
            Throw New CFException("无法获得expires_in=" + s)
        End If
        _expire = Now.AddSeconds(PubFunc.GetInt(e))

        'Throw New TGException("https://api.weibo.com/2/account/get_uid.json?access_token=" + _access_token)
        s = PubFunc.SendHTTP("https://api.weibo.com/2/account/get_uid.json?access_token=" + _access_token)

        o = OAuth2.GetJsonDict(Of Dictionary(Of String, String))(s)

        If Not o.TryGetValue("uid", _ouid) OrElse _ouid = "" Then
            Throw New CFException("无法获得ouid=" + s)
        End If
    End Sub

End Class
