Imports System.Security.Cryptography
Imports System.Collections.Specialized

Imports System.Text.RegularExpressions
Imports CN.COM.QQ


Public Class AuthCode


    ''' <summary>
    ''' 验证是否存在认证码
    ''' </summary>
    ''' <param name="form">包含key和code</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Check(form As NameValueCollection) As Boolean
        Dim c = QQConfig.GetStr(form, "code", 4)
        If c = "" Then
            Return False
        End If

        c = c.Trim

        Dim k = TGPub.GetBigInt("0x" + form("key"))

        Dim cm As SqlClient.SqlCommand = New SqlClient.SqlCommand("p_authcode_check", TGConfig.SessionDB.GetConnection())
        Try
            cm.CommandType = CommandType.StoredProcedure
            cm.Parameters.Add("@key", SqlDbType.BigInt).Value = k
            cm.Parameters.Add("@Code", SqlDbType.NVarChar, 4).Value = c
            cm.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

            cm.ExecuteNonQuery()

            Dim i As Int32 = DirectCast(cm.Parameters("@RETURN_VALUE").Value, Integer)
            If i = 1 Then
                Return True
            Else
                Return False
            End If
        Finally
            cm.Connection.Close()
        End Try
            Return False
    End Function




    ''' <summary>
    ''' 包含所有数字字母的数组
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared ary As String() = {"2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

    ''' <summary>
    ''' 生成指定长度的随机数对象，认证码长度默认为4
    ''' </summary>
    ''' <param name="key">随即KEY</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CreateAuthCode(key As Long) As String
        Return CreateAuthCode(4, key)
    End Function

    ''' <summary>
    ''' 生成指定长度的随机数对象
    ''' </summary>
    ''' <param name="Length">认证码长度</param>
    ''' <param name="key">随即KEY</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CreateAuthCode(Length As Integer, key As Long) As String
        '如果Length小于等于0则返回空
        If Length <= 0 Then
            Return ""
        End If

        Dim c As String = ""
        Dim i As Integer = -1
        For j = 0 To Length - 1
            i = TGPub.Rand.Next(0, ary.Length - 1)
            c += ary(i)
        Next

        UpdateOrCreate(key, c)

        Return c
    End Function

    ''' <summary>
    ''' 将产生的记录保存入数据库中
    ''' </summary>
    ''' <param name="key">随即KEY</param>
    ''' <param name="code">认证码</param>
    ''' <remarks></remarks>
    Private Shared Sub UpdateOrCreate(key As Long, code As String)
        Dim cm As SqlClient.SqlCommand = New SqlClient.SqlCommand("p_authcode_updateorcreate", TGConfig.SessionDB.GetConnection())
        Try
            cm.CommandType = CommandType.StoredProcedure
            cm.Parameters.Add("@key", SqlDbType.BigInt).Value = key
            cm.Parameters.Add("@Code", SqlDbType.NVarChar, 4).Value = code.ToLower()
            'cm.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            cm.ExecuteNonQuery()
        Finally
            cm.Connection.Close()
        End Try
    End Sub



End Class
