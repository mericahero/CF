''' <summary>
''' 身份证
''' </summary>
''' <remarks></remarks>
Public Class ShenFenZheng


    Private Shared _shenfenquan As Integer() = {7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2}
    Private Shared _shenfenjiaoyan As String() = {"1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2"}

    ''' <summary>
    ''' 获得身份证校验码
    ''' </summary>
    Private Shared Function GetShenFenZhengJiaoYan(ByVal s As String) As String
        If s.Length <> 17 Then Return Nothing

        Dim n As Integer = 0, c As Char

        For i As Integer = 0 To 16
            c = s.Chars(i)
            If c >= "0"c AndAlso c <= "9" Then
                n += _shenfenquan(i) * (Convert.ToInt32(c) - Convert.ToInt32("0"c))
            Else
                Return Nothing
            End If
        Next
        Return _shenfenjiaoyan(n Mod 11)
    End Function

    ''' <summary>
    ''' 获得出生日期
    ''' </summary>
    ''' <param name="s">15或者18位身份证号</param>
    ''' <returns>非法时返回System.DateTime.MinValue</returns>
    Public Shared Function GetChuShengRQ(ByVal s As String) As DateTime
        Dim rq As DateTime

        If s.Length = 18 Then
            If Not DateTime.TryParseExact(s.Substring(6, 8), "yyyyMMdd", Nothing, Globalization.DateTimeStyles.None, rq) Then
                Return System.DateTime.MinValue
            End If

            If GetShenFenZhengJiaoYan(s.Substring(0, 17)) = s.Substring(17, 1).ToUpper Then
                Return rq
            End If
        End If


        If s.Length = 15 Then
            If DateTime.TryParseExact("19" + s.Substring(6, 6), "yyyyMMdd", Nothing, Globalization.DateTimeStyles.None, rq) Then
                Return rq
            End If
        End If

        Return System.DateTime.MinValue
    End Function

    ''' <summary>
    ''' 检查身份证号是否有效
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns>true有效</returns>
    Public Shared Function Check(ByVal s As String) As Boolean
        Return GetChuShengRQ(s) <> System.DateTime.MinValue
    End Function


    Private _value As String

    ''' <summary>
    ''' 已经格式化好的身份证号
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Value As String
        Get
            Return _value
        End Get
        Set(s As String)
            s = s.Trim
            _chushengrq = GetChuShengRQ(s)
            If _chushengrq <> System.DateTime.MinValue Then
                _value = s.ToUpper
            Else
                _value = Nothing
            End If
        End Set
    End Property

    Private _chushengrq As DateTime
    ''' <summary>
    ''' 对应出生日期，非法时= System.DateTime.MinValue
    ''' </summary>
    Public ReadOnly Property ChuShengRQ As DateTime
        Get
            Return _chushengrq
        End Get
    End Property

    ''' <summary>
    ''' 当前是否包含有效身份证
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property InValid As Boolean
        Get
            Return ChuShengRQ = DateTime.MinValue
        End Get
    End Property

    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="s">身份证号</param>
    ''' <remarks>身份证号非法时，value=nothing</remarks>
    Public Sub New(s As String)
        Value = s
    End Sub


End Class
