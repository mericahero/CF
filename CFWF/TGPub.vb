Imports System.Security.Cryptography
Imports System.Collections.Specialized

Imports System.Text.RegularExpressions
Imports CN.COM.QQ
Imports System.Web.Security
Imports System.Runtime.Serialization.Json
Imports System.IO
Imports System.Text

Public Class TGPub


    Public Shared Function Md5Hash(s As String) As String
        Dim md5Hasher = MD5.Create()

        Dim data = md5Hasher.ComputeHash(System.Text.Encoding.Default.GetBytes(s))

        Dim sBuilder = New System.Text.StringBuilder()

        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next

        Return sBuilder.ToString()
    End Function


    Public Shared Function GetCookieGUID() As Guid
        Dim c = System.Web.HttpContext.Current.Request.Cookies("guid")
        If c Is Nothing Then Return Guid.Empty

        Dim guid1 As Guid
        If Not Guid.TryParse(c.Value, guid1) Then
            Return Guid.Empty
        End If
        Return guid1
    End Function

    ''' <summary>
    ''' 十进制转换为32进制，并反转
    ''' </summary>
    ''' <param name="n">要转换的十进制数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Convert10To32(n As Int32) As String
        Dim a As Int32 = n And &H1F
        Dim b As String = ""
        If a < 10 Then
            b = a.ToString()
        Else
            b = Chr(a + 87)
        End If
        n >>= 5
        While n > 0
            a = n And &H1F
            If a < 10 Then
                b += a.ToString()
            Else
                b += Chr(a + 87)
            End If
            n >>= 5
        End While
        Return b
    End Function

    ''' <summary>
    ''' 过滤数组中重复的Id、空格、非数字字符
    ''' </summary>
    ''' <param name="v"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetArrayDistinct(v As String()) As Array
        Dim l As New List(Of String)
        For i As Int32 = 0 To v.Length
            Dim v1 = v(i).Trim()
            If l.IndexOf(v1.ToLower()) = -1 And v1 <> "" And Not PubFunc.isNaN(v1) Then
                l.Add(v1)
            End If
        Next
        Return l.ToArray()
    End Function


    Private Shared extary As String() = {"gif", "jpg", "png", "jpeg"}
    ''' <summary>
    ''' 检查图片后缀名，只允许gif、jpg、png、jpeg
    ''' </summary>
    ''' <param name="extName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function isExtName(extName As String) As Boolean
        Return extName.Contains(extName)
    End Function

    ''' <summary>
    ''' 静态随机数对象
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Rand As Random = New Random()


    ''' <summary>
    ''' 获得int64位长整数，支持0x开头16进制，如果输入非法则返回0
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetBigInt(s As String) As Long
        If s = "" Then
            Return 0
        End If
        Dim n As Long
        If s.StartsWith("0x") Then
            If Long.TryParse(s.Substring(2), Globalization.NumberStyles.AllowHexSpecifier, Globalization.CultureInfo.InvariantCulture, n) Then
                Return n
            End If
            Return 0
        End If

        If Long.TryParse(s, n) Then
            Return n
        End If
        Return 0

    End Function

    Private Shared regex1 As New Regex("['""<>\/\\]", RegexOptions.Compiled)

    ''' <summary>
    ''' 按规则1将非法字符过滤掉
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GuoLvGuiZe1(s As String) As String
        Return regex1.Replace(PubTypes.DelCtrlChar(s), "")
    End Function

    ''' <summary>
    ''' 按规则1检查是否包含非法字符
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckGuiZe1(s As String) As Boolean
        If regex1.IsMatch(s) Then Return False
        Return True
    End Function

    ''' <summary>
    ''' 将IPv4格式的字符串转换为int型表示
    ''' </summary>
    ''' <param name="ip">IPv4格式的字符</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetIPAsInt32(ip As String) As Integer
        '将目标IP地址字符串strIPAddress转换为数字
        Dim arrayIP = ip.Split("."c)
        If arrayIP.Length < 4 Then Return 0
        Dim sip1 = PubFunc.GetInt(arrayIP(0))
        Dim sip2 = 0
        Dim sip3 = 0
        Dim sip4 = 0
        If arrayIP.Length > 1 Then
            sip2 = PubFunc.GetInt(arrayIP(1))
            If arrayIP.Length > 2 Then
                sip3 = PubFunc.GetInt(arrayIP(2))
                If (arrayIP.Length > 3) Then
                    sip4 = PubFunc.GetInt(arrayIP(3))
                End If
            End If
        End If
        Return (sip1 << 24) Or (sip2 << 16) Or (sip3 << 8) Or sip4
    End Function

    ''' <summary>
    ''' 将int型表示的IP还原成正常IPv4格式
    ''' </summary>
    ''' <param name="intIP">int型表示的IP</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetIP(intIP As Integer) As String
        Dim tempIP As Int32 = intIP
        '   将目标整形数字intIPAddress转换为IP地址字符串
        '   -1062731518 192.168.1.2 
        '   -1062731517 192.168.1.3 

        Dim s1 = (tempIP >> 24) And &HFF
        Dim s2 = (tempIP >> 16) And &HFF
        Dim s3 = (tempIP >> 8) And &HFF
        Dim s4 = (tempIP) And &HFF
        Return s1.ToString() + "." + s2.ToString() + "." + s3.ToString() + "." + s4.ToString()
    End Function

    ''' <summary>
    ''' 四舍五入，保留小数位后两位
    ''' </summary>
    ''' <param name="dec"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMoney(dec As String) As Decimal
        Dim n As Decimal
        If Decimal.TryParse(dec, n) Then
            Return n
        End If
        Return 0
    End Function

    ''' <summary>
    ''' 四舍五入，保留小数位后两位
    ''' </summary>
    ''' <param name="dec"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetMoneyStr(dec As Decimal) As String
        Return Math.Round(dec, 2).ToString()
    End Function

    ''130、131、132、155、156、185、186 
    Private Shared regexsj As New Regex("^(13[0-9]{9})|(15[012356789][0-9]{8})|(18[056789][0-9]{8})$", RegexOptions.IgnoreCase)
    ''' <summary>
    ''' 验证手机号码
    ''' </summary>
    ''' <param name="sj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckSJ(sj As String) As Boolean
        If Not IsNumeric(sj) Then
            Return False
        End If
        If sj.Length <> 11 Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' 批量验证手机号
    ''' </summary>
    ''' <param name="sj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckSJS(sj() As String) As String
        Dim RETURNVALUES As String = ""
        For i As Int32 = 0 To sj.Count
            If TGPub.CheckSJ(sj(i)) <> True Then
                RETURNVALUES += "1"
            Else
                RETURNVALUES += "0"
            End If
        Next
        Return RETURNVALUES
    End Function
    ''' <summary>
    ''' 获取手机号码
    ''' </summary>
    ''' <param name="sj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetSJ(sj As String) As String
        If sj = Nothing Then
            Return ""
        End If

        If sj.StartsWith("+86") Then
            sj = sj.Substring(3)
        End If

        Return sj
    End Function


    ''' <summary>
    ''' 返回一个Boolean值，表达是否是时间格式。
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsDate(obj As Object) As Boolean
        If obj Is Nothing Then
            Return False
        End If

        Try
            Dim d = DateTime.Parse(obj.ToString())
            Return True
        Catch ex As Exception
            Return False
        End Try
        Return Microsoft.VisualBasic.IsDate(obj)
    End Function
    ''' <summary>
    ''' 返回Date类型的数据
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToDate(obj As Object) As Date
        If IsDate(obj) Then
            Return CDate(obj)
        End If
        Return Date.Now
    End Function
    ''' <summary>
    ''' 返回Integer类型的数据
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToInt(obj As Object) As Int32
        Return CInt(obj)
    End Function

    ''' <summary>
    ''' 将人民币数字转换成大写
    ''' </summary>
    ''' <param name="strIn">输入的数字</param>
    ''' <param name="bType">是否转换成人民币</param>
    ''' <returns>若转换成人民币，则返回人民币大写带价格单位，否则仅返回人民币大写</returns>
    Public Shared Function GetChineseNum(strIn As String, bType As Boolean) As String
        Dim m_1 As String, m_2 As String, m_3 As String, m_4 As String, m_5 As String, m_6 As String, _
         m_7 As String, m_8 As String, m_9 As String

        Dim numNum As String = "0123456789."
        Dim numChina As String = "零壹贰叁肆伍陆柒捌玖点"
        Dim numChinaWeigh As String = "个拾佰仟万拾佰仟亿拾佰仟万"

        'm_1 将输入的数转换成Double类型（带两位小数）
        Try
            m_1 = (Convert.ToDouble(strIn)).ToString("f2")
        Catch
            Return "数字非法！"
        End Try

        '计算小数点位置
        Dim dotIndex As Integer = -1
        If m_1.Length > 16 Then
            Return "数字太大了!"
        End If
        If m_1.IndexOf("."c) > 0 Then
            dotIndex = m_1.IndexOf("."c)
        End If

        m_2 = m_1
        m_3 = ""
        m_4 = ""
        'm_2:1234-> 壹贰叁肆 
        For i As Integer = 0 To 10
            m_2 = m_2.Replace(numNum.Substring(i, 1), numChina.Substring(i, 1))
        Next

        'm_3:佰拾万仟佰拾个
        For j As Integer = dotIndex To 1 Step -1
            m_3 += numChinaWeigh.Substring(j - 1, 1)
        Next

        'm_4:每一个数对应一个单位
        For i As Integer = 0 To m_3.Length - 1
            m_4 += m_2.Substring(i, 1) & m_3.Substring(i, 1)
        Next

        'm_5:4行去"0"后面的拾佰仟 
        m_5 = m_4.Replace("零拾", "零").Replace("零佰", "零").Replace("零仟", "零")

        'm_6:中间的多个零变成一个零
        m_6 = m_5
        For i As Integer = 0 To dotIndex - 1
            m_6 = m_6.Replace("零零", "零")
        Next


        'm_7:去掉亿,万,个位的"0" 
        m_7 = m_6.Replace("亿零万零", "亿零").Replace("亿零万", "亿零").Replace("零亿", "亿").Replace("零万", "万")

        If m_7.Length > 2 Then
            m_7 = m_7.Replace("零个", "个")
        End If

        'm_8:最终 不带单位 
        m_8 = m_7.Replace("个", "")
        If m_2.Substring(m_2.Length - 3, 3) <> "点零零" Then
            m_8 += m_2.Substring(m_2.Length - 3, 3)
        End If

        'm_9:最终 带单位 
        m_9 = m_7
        m_9 = m_9.Replace("个", "圆")
        If m_2.Substring(m_2.Length - 3, 3) <> "点零零" Then
            m_9 += m_2.Substring(m_2.Length - 2, 2)
            m_9 = m_9.Insert(m_9.Length - 1, "角")
            m_9 += "分"
        Else
            m_9 += "整"
        End If

        If m_9 <> "零圆整" Then
            m_9 = m_9.Replace("零圆", "")
        End If

        m_9 = m_9.Replace("零分", "")


        Return If(bType, m_9, m_8)
    End Function

    ''' <summary>
    ''' JSON序列化
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="t1"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function JsonSerializer(Of T)(t1 As T) As String
        Dim ser As New DataContractJsonSerializer(GetType(T))
        Dim ms As New MemoryStream()
        ser.WriteObject(ms, t1)
        Dim jsonString As String = Encoding.UTF8.GetString(ms.ToArray())
        ms.Close()
        Return jsonString
    End Function
End Class