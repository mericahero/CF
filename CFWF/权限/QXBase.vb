Imports CN.COM.QQ

''' <summary>
''' 权限控制基础类
''' </summary>
''' <remarks></remarks>
Public Class QXBase

    Protected Property oDB As CN.COM.QQ.DB

    Public Sub New(odb As CN.COM.QQ.DB)
        Me.oDB = odb
    End Sub

    ''' <summary>
    ''' 主权限检查存储过程名，子类可以修改该属性以使用个性化的名字
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Property MainPname As String = "p_qx_check"


    ''' <summary>
    ''' 检查用户是否是商户的操作员，作为一个基本的权限检查
    ''' </summary>
    ''' <param name="uid"></param>
    ''' <param name="shid"></param>
    ''' <remarks></remarks>
    Public Sub CheckIsUsr(uid As Integer, shid As Integer)
        If oDB.GetSQLSingleRow("select bz from bumen_chengyuan where shid=" & shid & " And uid=" & uid & " And bz=0 ") IsNot Nothing Then Return



        '如果没有权限则抛出异常
        Throw New TGException(enErrType.DenyAct, "您不是该商户成员！</BR>{uid:" & uid.ToString() & ",shid:" & shid.ToString() & "}")
    End Sub




    ''' <summary>
    ''' 检查对root的权限
    ''' </summary>
    ''' <param name="uid"></param>
    ''' <param name="actid"></param>
    ''' <remarks></remarks>
    Public Sub CheckRoot(ByVal uid As Integer, ByVal actid As enQxActID)
        CheckRoot(uid, actid, -1)
    End Sub

    ''' <summary>
    ''' 检查对root的权限，提供子项目itemID
    ''' </summary>
    ''' <param name="uid"></param>
    ''' <param name="actid"></param>
    ''' <remarks></remarks>
    Public Sub CheckRoot(ByVal uid As Integer, ByVal actid As enQxActID, ByVal itemID As Integer)
        Check(uid, 1, actid, itemID)
    End Sub


    ''' <summary>
    ''' 检查操作权限
    ''' </summary>
    Public Sub Check(ByVal uid As Integer, ByVal rid As Integer, ByVal actid As enQxActID)
        _Check(MainPname, uid, rid, actid, -1)
    End Sub



    ''' <summary>
    ''' 检查操作权限
    ''' </summary>
    Public Sub Check(ByVal uid As Integer, ByVal rid As Integer, ByVal actid As enQxActID, ByVal itemID As Integer)
        _Check(MainPname, uid, rid, actid, itemID)
    End Sub


    ''' <summary>
    ''' 检查二级授权权限
    ''' </summary>
    Public Sub CheckShouQuan2(ByVal uid As Integer, ByVal rid As Integer, ByVal actid As enQxActID, ByVal itemID As Integer)
        _Check(MainPname, uid, rid, actid + 2, itemID)
    End Sub


    ''' <summary>
    ''' 检查一级授权权限
    ''' </summary>
    Public Sub CheckShouQuan1(ByVal uid As Integer, ByVal rid As Integer, ByVal actid As enQxActID, ByVal itemID As Integer)
        _Check(MainPname, uid, rid, actid + 1, itemID)
    End Sub


    ''' <summary>
    ''' 检查管理权限
    ''' </summary>
    ''' <param name="uid"></param>
    ''' <param name="rid"></param>
    ''' <param name="actid"></param>
    ''' <param name="itemID"></param>
    ''' <remarks></remarks>
    Public Sub CheckAdmin(ByVal uid As Integer, ByVal rid As Integer, ByVal actid As enQxActID, ByVal itemID As Integer)
        _Check(MainPname, uid, rid, actid + 3, itemID)
    End Sub




    ''' <summary>
    ''' 是否拥有操作权限
    ''' </summary>
    Public Function Have(ByVal uid As Integer, ByVal rid As Integer, ByVal actid As enQxActID, ByVal itemID As Integer) As Boolean
        Return _Have(MainPname, uid, rid, actid, itemID)
    End Function

    Public Function HaveShouQuan1(ByVal uid As Integer, ByVal rid As Integer, ByVal actid As enQxActID, ByVal itemID As Integer) As Boolean
        Return _Have(MainPname, uid, rid, actid + 1, itemID)
    End Function

    Public Function HaveShouQuan2(ByVal uid As Integer, ByVal rid As Integer, ByVal actid As enQxActID, ByVal itemID As Integer) As Boolean
        Return _Have(MainPname, uid, rid, actid + 2, itemID)
    End Function

    Public Function HaveAdmin(ByVal uid As Integer, ByVal rid As Integer, ByVal actid As enQxActID, ByVal itemID As Integer) As Boolean
        Return _Have(MainPname, uid, rid, actid + 3, itemID)
    End Function



    ''' <summary>
    ''' 权限检查,提供给子类扩展
    ''' </summary>
    ''' <param name="pname">检查权限的存储过程名</param>
    Protected Sub _Check(ByVal pname As String, ByVal uid As Integer, ByVal rid As Integer, ByVal actid As Integer, ByVal itemID As Integer)
        If _Have(pname, uid, rid, actid, itemID) Then Return

        '如果没有权限则抛出异常
        Throw New TGException(enErrType.DenyAct, "您的权限不足！</BR>{uid:" & uid.ToString() & ",rid:" & rid.ToString() & ",actid:" & actid.ToString() & ",itemID:" & itemID.ToString() & "}")
    End Sub


    ''' <summary>
    ''' 是否拥有权限
    ''' </summary>
    Protected Function _Have(ByVal pname As String, ByVal uid As Integer, ByVal rid As Integer, ByVal actid As Integer, ByVal itemID As Integer) As Boolean
        ''Return True
        '实际通过存储过程来获得权限
        Dim cm = New SqlClient.SqlCommand(pname, oDB.GetConnection())
        Dim retn As Integer
        Try
            cm.CommandType = CommandType.StoredProcedure
            cm.Parameters.Add("@uid", SqlDbType.Int).Value = uid
            cm.Parameters.Add("@rid", SqlDbType.Int).Value = rid
            cm.Parameters.Add("@actid", SqlDbType.Int).Value = actid
            cm.Parameters.Add("@itemID", SqlDbType.Int).Value = itemID
            cm.Parameters.Add("@RETURN_VALUE", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            cm.ExecuteNonQuery()
            retn = CInt(cm.Parameters("@RETURN_VALUE").Value)
            If retn > 0 Then
                Return True
            Else
                Return False
            End If
        Finally
            cm.Connection.Close()
        End Try
    End Function


    ''' <summary>
    ''' 获得制定部门下(包括指定部门)有权限的部门ID列表SQL，
    ''' </summary>
    Function GetSubBMIDSQL(ByVal uid As Integer, ByVal bmid As Integer, ByVal actid As enQxActID) As String
        '当没有子部门时，简单返回bmid
        Return bmid.ToString
    End Function

End Class
