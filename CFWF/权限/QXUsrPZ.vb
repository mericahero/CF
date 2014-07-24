''' <summary>
''' 给权限配置实用，当前用户针对rid的权限配置，注意是直接配置的权限，而非有效权限
''' </summary>
''' <remarks></remarks>
Public Class QXUsrPZ

    Private _Qxitems As IDictionary(Of System.Int64, String)

    ''' <summary>
    ''' 批量获得用户配置的权限
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="uid"></param>
    ''' <param name="rid"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal oDB As CN.COM.QQ.DB, ByVal uid As Integer, ByVal rid As Integer)
        _Qxitems = New Dictionary(Of System.Int64, String)()
        Dim ActRows As DataRowCollection = oDB.GetSQLRows("select actid,itemid,qx from qx_usr_bumen where uid=" & uid & " And bmid=" & rid & " ")
        For i As Integer = 0 To ActRows.Count - 1
            _Qxitems.Add((Convert.ToInt32(ActRows(i)("actid")) << 32) + Convert.ToInt32(ActRows(i)("itemid")), ActRows(i)("qx").ToString())
        Next
    End Sub

    ''' <summary>
    ''' 与数据库对应，返回YN或者""
    ''' </summary>
    ''' <param name="actid"></param>
    ''' <returns>Y明确授权了，N明确拒绝了，""没有明确定义</returns>
    ''' <remarks></remarks>
    Public Function GetCaoZuo(ByVal actid As enQxActID) As Char
        Return GetCaoZuo(actid, -1)
    End Function

    ''' <summary>
    ''' 获得一级授权权限配置
    ''' </summary>
    Public Function GetShouQuan1(ByVal actid As enQxActID) As Char
        Return GetCaoZuo(actid + 1, -1)
    End Function

    ''' <summary>
    ''' 获得二级授权权限配置
    ''' </summary>
    Public Function GetShouQuan2(ByVal actid As enQxActID) As Char
        Return GetCaoZuo(actid + 2, -1)
    End Function

    ''' <summary>
    ''' 获得管理权限配置
    ''' </summary>
    Public Function GetAdmin(ByVal actid As enQxActID) As Char
        Return GetCaoZuo(actid + 3, -1)
    End Function


    ''' <summary>
    ''' 获得操作权限
    ''' </summary>
    Public Function GetCaoZuo(ByVal actid As Integer, ByVal itemid As Integer) As Char
        If Not _Qxitems.ContainsKey((actid << 32) + itemid) Then Return Convert.ToChar(" ")
        Return Convert.ToChar(_Qxitems((actid << 32) + itemid))
    End Function


    ' ''' <summary>
    ' ''' 内部私有子项目集合
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private _items As IDictionary(Of Integer, DataRowCollection)

    ' ''' <summary>
    ' ''' 获得actid对应的子项目列表
    ' ''' </summary>
    'Public ReadOnly Property Items(actid As Integer) As DataRowCollection
    '    Get
    '        If _items Is Nothing Then
    '            _items = New Dictionary(Of Integer, DataRowCollection)
    '        End If

    '        Dim t = _items(actid)
    '        If t Is Nothing Then
    '            '从数据库获取子项目列表
    '            Select Case actid
    '                '根据不同actid，用不同方法获得

    '            End Select

    '            '保存
    '            _items(actid) = t
    '        End If

    '        Return t
    '    End Get
    'End Property


    ''' <summary>
    ''' 获得子项目列表
    ''' </summary>
    Public Shared Function GetItems(ByVal rid As Integer, ByVal actid As Integer) As IList(Of QXItem)
        Return Nothing
    End Function

End Class

''' <summary>
''' 权限子项目
''' </summary>
''' <remarks></remarks>
Public Structure QXItem
    Public itemID As Integer
    Public Name As String
End Structure