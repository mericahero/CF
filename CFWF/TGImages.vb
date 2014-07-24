Imports System.Collections.Specialized
Imports System.Web
Imports System.Drawing
Imports CN.COM.QQ
Imports System.IO

''' <summary>
''' Author：张和清
''' Create date：2012-10-08
''' Description：图片对象
''' </summary>
''' <remarks></remarks>
Public Class ImgInfo

    Public Sub New(file As HttpPostedFile)
        init("", file)
    End Sub

    Public Sub New(fz As String, file As HttpPostedFile)
        init(fz, file)
    End Sub

    ''' <summary>
    ''' 初始化
    ''' </summary>
    ''' <param name="fz"></param>
    ''' <param name="file"></param>
    ''' <remarks></remarks>
    Private Sub init(fz As String, file As HttpPostedFile)
        If file.FileName.Length <= 0 Then
            Throw New QQException(enErrType.NormalError, "图片文件不存在！")
        End If
        '限定附注的长度不超过500个字符。
        If fz.Length >= 500 Then
            fuzhu = fz.Substring(0, 500)
        Else
            fuzhu = fz
        End If
        Using _img As Image = New Bitmap(file.InputStream)
            If _img.Width <> 0 And _img.Height <> 0 Then
                width = _img.Width
                Height = _img.Height
            Else
                Throw New QQException(enErrType.NormalError, "图片文件的Height和Width属性不对！")
            End If
        End Using
        ExtName = Path.GetExtension(file.FileName).Substring(1).ToLower()
        Postedfile = file
    End Sub
    ''' <summary>
    ''' 图片id
    ''' </summary>
    ''' <remarks></remarks>
    Public id As Int32 = 0
    ''' <summary>
    ''' 图片的完整的可访问的HTTP URL路径
    ''' </summary>
    ''' <remarks></remarks>
    Public url As String
    ''' <summary>
    ''' 图片宽度
    ''' </summary>
    ''' <remarks></remarks>
    Public width As Int32
    ''' <summary>
    ''' 图片高度
    ''' </summary>
    ''' <remarks></remarks>
    Public Height As Int32
    ''' <summary>
    ''' 图片附注
    ''' </summary>
    ''' <remarks></remarks>
    Public fuzhu As String
    ''' <summary>
    ''' 后缀名
    ''' </summary>
    ''' <remarks></remarks>
    Public ExtName As String
    ''' <summary>
    ''' 文件对象
    ''' </summary>
    ''' <remarks></remarks>
    Public Postedfile As HttpPostedFile
End Class

''' <summary>
''' Author：张和清
''' Create date：2012-10-08
''' Description：上传图片接口
''' </summary>
''' <remarks></remarks>
Public Interface IImages
    ''' <summary>
    ''' 图片增加
    ''' </summary>
    ''' <param name="uid"></param>
    ''' <param name="form"></param>
    ''' <param name="files"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Add(uid As Int32, form As NameValueCollection, files As HttpFileCollection) As IList(Of Object)
    ''' <summary>
    ''' 图片删除
    ''' </summary>
    ''' <param name="uid"></param>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Del(uid As Int32, id As Int32) As Boolean
    ''' <summary>
    ''' 图片批量删除
    ''' </summary>
    ''' <param name="uid"></param>
    ''' <param name="ids"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Dels(uid As Int32, ids As Int32) As Int32

End Interface

''' <summary>
''' Author：张和清
''' Create date：2012-10-08
''' Description：图片相关函数
''' </summary>
''' <remarks></remarks>
Public MustInherit Class TGImages
#Region "屏蔽"

    'Implements IImages

    ' ''' <summary>
    ' ''' 图片增加
    ' ''' </summary>
    ' ''' <param name="uid"></param>
    ' ''' <param name="form"></param>
    ' ''' <param name="files"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public MustOverride Function Add(uid As Integer, form As System.Collections.Specialized.NameValueCollection, files As System.Web.HttpFileCollection) As System.Collections.Generic.IList(Of Object) Implements IImages.Add
    ' ''' <summary>
    ' ''' 图片删除
    ' ''' </summary>
    ' ''' <param name="uid"></param>
    ' ''' <param name="id"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public MustOverride Function Del(uid As Integer, id As Integer) As Boolean Implements IImages.Del
    ' ''' <summary>
    ' ''' 图片批量删除
    ' ''' </summary>
    ' ''' <param name="uid"></param>
    ' ''' <param name="ids"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public MustOverride Function Dels(uid As Integer, ids As Integer) As Integer Implements IImages.Dels


    ' ''' <summary>
    ' ''' 获得图片的完整的可访问的HTTP URL路径
    ' ''' </summary>
    ' ''' <param name="row">包含图片id，图片上传日期，图片格式，图片备注的DataRow</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Shared Function GetUrl(row As DataRow) As String
    '    Return GetUrl(Int32.Parse(row("id").ToString()), DateTime.Parse(row("rq").ToString()), row("extName").ToString(), Int32.Parse(row("bz").ToString()))
    'End Function

    ' ''' <summary>
    ' ''' 获得图片的完整的可访问的HTTP URL路径
    ' ''' </summary>
    ' ''' <param name="id">图片id</param>
    ' ''' <param name="rq">图片上传日期</param>
    ' ''' <param name="extName">图片格式</param>
    ' ''' <param name="bz">图片备注</param>
    ' ''' <returns>返回图片的完整的可访问的HTTP URL路径</returns>
    ' ''' <remarks></remarks>
    'Public Shared Function GetUrl(id As Int32, rq As DateTime, extName As String, bz As Int32) As String

    '    Dim hostid = GetHostID(bz)
    '    If hostid = 80 Then
    '        Return TGConfig.Img80Host + GetPath(id, rq, extName, bz)
    '    Else
    '        Return "http://img" + hostid.ToString() + ".vip121.com" + GetPath(id, rq, extName, bz)
    '    End If

    'End Function
#End Region

    ''' <summary>
    ''' 获得图片实际存放的物理路径
    ''' </summary>
    ''' <param name="id">图片id</param>
    ''' <param name="rq">图片上传日期</param>
    ''' <param name="extName">图片格式</param>
    ''' <param name="bz">图片备注</param>
    ''' <returns>返回图片实际存放的物理路径</returns>
    ''' <remarks></remarks>
    Public Shared Function GetRealPath(id As Int32, rq As DateTime, extName As String, bz As Int32) As String
        Return HttpContext.Current.Server.MapPath("/imgmap/" + GetHostID(bz).ToString() + GetPath(id, rq, extName, bz))
    End Function

    ''' <summary>
    ''' 获取HostID
    ''' </summary>
    ''' <param name="bz"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetHostID(bz As Int32) As Int32
        Return bz
    End Function

    ''' <summary>
    ''' 获得图片的虚拟路径
    ''' </summary>
    ''' <param name="id">图片id</param>
    ''' <param name="rq">图片上传日期</param>
    ''' <param name="extName">图片格式</param>
    ''' <param name="bz">图片备注</param>
    ''' <returns>返回图片的虚拟路径</returns>
    ''' <remarks></remarks>
    Public Shared Function GetPath(id As Int32, rq As DateTime, extName As String, bz As Int32) As String
        Return GetDirectory(rq) + "/" + GetFileName(id, extName)
    End Function

    ''' <summary>
    ''' 获取目录
    ''' </summary>
    ''' <param name="rq">日期</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDirectory(rq As DateTime) As String
        Return rq.ToString("/yy/MM/dd")
    End Function

    ''' <summary>
    ''' 获取带后缀的文件名
    ''' </summary>
    ''' <param name="ImgId">图片id</param>
    ''' <param name="extName">图片后缀名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFileName(ImgId As String, extName As String) As String
        If extName = "" Then
            extName = "jpg"
        End If

        Return ImgId & "." & extName
    End Function

    ''' <summary>
    ''' 获取带后缀的文件名
    ''' </summary>
    ''' <param name="id">图片id</param>
    ''' <param name="extName">图片后缀名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFileName(id As Int32, extName As String) As String
        Return GetFileName(TGPub.Convert10To32(id), extName)
    End Function


    ''' <summary>
    ''' 检查指定地址的目录是否存在，不存在则创建目录。
    ''' eq:/s/ab/cd
    ''' </summary>
    ''' <param name="d">图片路径</param>
    ''' <remarks></remarks>
    Public Shared Sub PathCreate(d As String)
        If Not Directory.Exists(d) Then
            Directory.CreateDirectory(d)
        End If
    End Sub

    ''' <summary>
    ''' 检查文件夹是否存在，不存在则创建文件夹。
    ''' </summary>
    ''' <param name="rq"></param>
    ''' <param name="bz"></param>
    ''' <remarks></remarks>
    Public Shared Sub PathCreate(rq As DateTime, bz As Int32)
        Dim d As String = HttpContext.Current.Server.MapPath("/imgmap/" + GetHostID(bz).ToString() + GetDirectory(rq))
        PathCreate(d)
    End Sub






End Class

