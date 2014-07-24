Imports CN.COM.QQ
Public Class TGConfig

    ''' <summary>
    ''' 标准日期格式
    ''' </summary>
    Public Const DateFormatStr As String = "yyyy-M-d HH:mm:ss"


    ''' <summary>
    ''' 服务器ID
    ''' </summary>
    Public Shared ServerID As Integer = 0

    Private Shared _appset As System.Collections.Specialized.NameValueCollection
    Public Shared ReadOnly Property Appset() As System.Collections.Specialized.NameValueCollection
        Get
            Return _appset
        End Get
    End Property



    Public Shared Sub SetConfig(ByVal s As System.Collections.Specialized.NameValueCollection)
        _appset = s

        ServerID = PubFunc.GetDefaultInt(s("ServerID"), ServerID)
        SessionDB = New DB(s("SessionConnectionString"))
        UserDB = New DB(s("UserConnectionString"))
        'SHDB = New DB(s("B2CConnectionString"))
        SHDB = New DB(s("SHConnectionString"))
        B2CDB = New DB(s("B2CConnectionString"))
        ExtDB = New DB(s("EXTConnectionString"))
        SMDB = New DB(s("MESSAGEConnectionString"))

        CN.COM.QQ.QQConfig.QQDomain = PubFunc.GetDefaultStr(s("QQDomain"), "vip121.com")

        JSHost = PubFunc.GetDefaultStr(s("JSHost"), "http://js.vip121.com")

        Img0Host = PubFunc.GetDefaultStr(s("Img0Host"), "http://img0.vip121.com")
        Img60Host = PubFunc.GetDefaultStr(s("Img60Host"), "http://img60.vip121.com")
        Img70Host = PubFunc.GetDefaultStr(s("Img70Host"), "http://img70.vip121.com")
        Img80Host = PubFunc.GetDefaultStr(s("Img80Host"), "http://img80.vip121.com")
        Img99Host = PubFunc.GetDefaultStr(s("Img99Host"), "http://img99.vip121.com")

        File10Host = PubFunc.GetDefaultStr(s("File10Host"), "http://file10.vip121.com")

        LoginHost = PubFunc.GetDefaultStr(s("LoginHost"), "https://login.vip121.com")
        PayHost = PubFunc.GetDefaultStr(s("PayHost"), "http://pay.vip121.com")
        HtadminHost = PubFunc.GetDefaultStr(s("HtadminHost"), "http://htadmin.vip121.com")

        ServiceHost = PubFunc.GetDefaultStr(s("ServiceHost"), "http://service.zj116114.net")
        WWWHost = PubFunc.GetDefaultStr(s("WWWHost"), "http://www.zj116114.net")
        WapHost = PubFunc.GetDefaultStr(s("WapHost"), "http://wap.zj116114.net")
        UsrHost = PubFunc.GetDefaultStr(s("UsrHost"), "http://usr.zj116114.net")

        UIPSHost = PubFunc.GetDefaultStr(s("UIPSHost"), "http://nb.vip121.com")


        MallHost = PubFunc.GetDefaultStr(s("MallHost"), "http://mall.zj116114.net")
        MallName = PubFunc.GetDefaultStr(s("MallName"), "联通116114商城")

        CaiPiaoHost = PubFunc.GetDefaultStr(s("CaiPiaoHost"), "http://caipiao.zj116114.net")
        MHost = PubFunc.GetDefaultStr(s("MHost"), "http://m.zj116114.net")
        SearchHost = PubFunc.GetDefaultStr(s("SearchHost"), "http://search.zj116114.net")

        TGAutoMail = PubFunc.GetDefaultStr(s("TGAutoMail"), "")

        QQConfig.ChinaBankID = PubFunc.GetDefaultStr(s("ChinaBankID"), "")
        QQConfig.ChinaBankKey = PubFunc.GetDefaultStr(s("ChinaBankKey"), "")

    End Sub


    ''' <summary>
    ''' session数据库
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared SessionDB As DB

    ''' <summary>
    ''' 用户数据库
    ''' </summary>
    Public Shared UserDB As DB

    ''' <summary>
    ''' 商户系统数据库连接对象
    ''' </summary>
    Public Shared SHDB As DB

    ''' <summary>
    ''' B2C系统数据库连接对象
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared B2CDB As DB

    ''' <summary>
    ''' Ext系统数据库连接对象
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ExtDB As DB

    ''' <summary>
    ''' SM系统数据库连接对象
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared SMDB As DB

    ''' <summary>
    ''' .js,.css文件主机地址
    ''' </summary>
    Public Shared JSHost As String

    ''' <summary>
    ''' service主机地址
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ServiceHost As String

    ''' <summary>
    ''' img0图片地址(CMS)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Img0Host As String

    ''' <summary>
    ''' img70图片地址(B2CDH——图片上传地址)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Img70Host As String

    ''' <summary>
    ''' img80图片地址(UBB编辑器图片上传地址)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Img80Host As String

    ''' <summary>
    ''' img99图片地址(B2C——主副图上传地址)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Img99Host As String

    ''' <summary>
    ''' img60图片地址(UIPS——图片上传地址)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Img60Host As String

    ''' <summary>
    ''' file10文件地址(UIPS——文件上传地址)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared File10Host As String



    ''' <summary>
    ''' www地址
    ''' </summary>
    Public Shared WWWHost As String


    ''' <summary>
    ''' uips地址
    ''' </summary>
    Public Shared UIPSHost As String


    ''' <summary>
    ''' wap地址
    ''' </summary>
    Public Shared WapHost As String

    ''' <summary>
    ''' 自动发送email的发送者email
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared TGAutoMail As String

    ''' <summary>
    ''' 商城地址
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared MallHost As String

    ''' <summary>
    ''' 商城名称
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared MallName As String

    ''' <summary>
    ''' 彩票地址
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared CaiPiaoHost As String

    ''' <summary>
    ''' 手机地址
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared MHost As String

    ''' <summary>
    ''' 后台服务器
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Public Shared HtadminHost As String

    ''' <summary>
    ''' 登录服务器
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Public Shared LoginHost As String

    ''' <summary>
    ''' 用户中心服务器
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared UsrHost As String

    ''' <summary>
    ''' 支付服务器
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared PayHost As String

    ''' <summary>
    ''' 搜索服务器
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared SearchHost As String

End Class
