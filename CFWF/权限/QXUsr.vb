Imports CN.COM.QQ
''' <summary>
''' 获得用户针对某资源当前拥有的权限
''' </summary>
''' <remarks></remarks>
Public Class QXUsr

    Private _Qxitems_Item As IDictionary(Of Long, String)
    Private _Qxitems As IDictionary(Of Integer, String)
    Private _uid As Integer
    Private _rid As Integer

    ''' <summary>
    ''' 批量获得用户对rid的所有各种权限
    ''' </summary>
    ''' <param name="oDB"></param>
    ''' <param name="uid"></param>
    ''' <param name="rid"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal oDB As CN.COM.QQ.DB, ByVal uid As Integer, ByVal rid As Integer)
        _Qxitems_Item = New Dictionary(Of Long, String)()
        _Qxitems = New Dictionary(Of Integer, String)()
        _uid = uid
        _rid = rid
        Dim Actid_temp As Long
        Dim ActRows As DataRowCollection = oDB.GetSQLRows("select actid,itemid,qx from qx_usr_bumen where uid=" & uid & " And bmid=" & rid & " ")
        For i As Integer = 0 To ActRows.Count - 1
            If Convert.ToInt32(ActRows(i)("itemid")) = -1 Then
                _Qxitems.Add(Convert.ToInt32(ActRows(i)("actid")), ActRows(i)("qx").ToString())
            Else
                Actid_temp = (Convert.ToInt64(ActRows(i)("actid")) << 32) Or Convert.ToInt64(ActRows(i)("itemid"))
                _Qxitems_Item.Add(Actid_temp, ActRows(i)("qx").ToString())
            End If
        Next
    End Sub


    ''' <summary>
    ''' 获得指定的若干个actid的权限，对于没有指定的将始终返回无权限！
    ''' </summary>
    ''' <remarks>此构造方法无需获得所有的权限</remarks>
    Public Sub New(ByVal oDB As CN.COM.QQ.DB, ByVal uid As Integer, ByVal rid As Integer, ByVal actidAry() As Integer)
        If actidAry.Length < 1 Then Return
        _Qxitems_Item = New Dictionary(Of Long, String)()
        _Qxitems = New Dictionary(Of Integer, String)()
        _uid = uid
        _rid = rid

        Dim actidstr = ""
        Dim chaojiqx As New System.Collections.Generic.List(Of Integer)
        Dim Actid_temp As Long

        For Each actid In actidAry
            If actidstr = "" Then
                actidstr = actid.ToString()
            Else
                If Not chaojiqx.Contains(actid) Then
                    chaojiqx.Add(actid)
                    actidstr += "," & actid.ToString()                
                End If
            End If

            '获得超级权限ID
            Dim chaojiid = actid And &HFF000003
            If Not chaojiqx.Contains(chaojiid) Then
                '还没有包含
                chaojiqx.Add(chaojiid)
                actidstr += "," & chaojiid
            End If


            If actid = enQxActID.SH_AdminQX_View Then
                If Not chaojiqx.Contains(actid + 3) Then
                    chaojiqx.Add(actid + 3)
                    actidstr += "," & (actid + 3)
                    '获得超级权限ID
                    Dim chaojiidqx = (actid + 3) And &HFF000003

                    If Not chaojiqx.Contains(chaojiidqx) Then
                        '还没有包含
                        chaojiqx.Add(chaojiidqx)
                        actidstr += "," & chaojiidqx
                    End If
                End If

                If Not chaojiqx.Contains(actid + 2) Then
                    chaojiqx.Add(actid + 2)
                    actidstr += "," & (actid + 2)
                    '获得超级权限ID
                    Dim chaojiidqx = (actid + 2) And &HFF000003

                    If Not chaojiqx.Contains(chaojiidqx) Then
                        '还没有包含
                        chaojiqx.Add(chaojiidqx)
                        actidstr += "," & chaojiidqx
                    End If
                End If

                If Not chaojiqx.Contains(actid + 1) Then
                    chaojiqx.Add(actid + 1)
                    actidstr += "," & (actid + 1)
                    '获得超级权限ID
                    Dim chaojiidqx = (actid + 1) And &HFF000003

                    If Not chaojiqx.Contains(chaojiidqx) Then
                        '还没有包含
                        chaojiqx.Add(chaojiidqx)
                        actidstr += "," & chaojiidqx
                    End If
                End If

            End If
           

        Next

        Dim ActRows As DataRowCollection = oDB.GetSQLRows("select actid,itemid,qx from qx_usr_bumen where uid=" & uid & " And bmid=" & rid & " And actid In(" & actidstr & ")")
        For i As Integer = 0 To ActRows.Count - 1
            If Convert.ToInt32(ActRows(i)("itemid")) = -1 Then
                _Qxitems.Add(Convert.ToInt32(ActRows(i)("actid")), ActRows(i)("qx").ToString())
            Else
                Actid_temp = (Convert.ToInt64(ActRows(i)("actid")) << 32) Or Convert.ToInt64(ActRows(i)("itemid"))
                _Qxitems_Item.Add(Actid_temp, ActRows(i)("qx").ToString())
            End If
        Next
    End Sub
    ''' <summary>
    ''' 判断操作权限
    ''' </summary>
    Public Sub Check(ByVal actid As enQxActID)
        If HaveBase(actid) Then Return
        '如果没有权限则抛出异常
        Throw New TGException(enErrType.DenyAct, "您的权限不足！</BR>{uid:" & _uid.ToString() & ",rid:" & _rid.ToString() & ",actid:" & actid.ToString() & "}")
    End Sub

    ''' <summary>
    ''' 判断操作权限
    ''' </summary>
    Public Sub Check(ByVal actid As enQxActID, ByVal itemid As Integer)
        If Have(actid, itemid) Then Return
        '如果没有权限则抛出异常
        Throw New TGException(enErrType.DenyAct, "您的权限不足！</BR>{uid:" & _uid.ToString() & ",rid:" & _rid.ToString() & ",actid:" & actid.ToString() & ",itemid:" & itemid.ToString() & "}")
    End Sub


    ''' <summary>
    ''' 是否有操作权限
    ''' </summary>
    Public Function Have(ByVal actid As enQxActID) As Boolean
        Return HaveBase(actid)
    End Function

    ''' <summary>
    ''' 是否有二级授权权限
    ''' </summary>
    Public Function HaveShouQuan2(ByVal actid As enQxActID) As Boolean
        Return HaveBase(actid + 2)
    End Function

    ''' <summary>
    ''' 是否有一级授权权限
    ''' </summary>
    Public Function HaveShouQuan1(ByVal actid As enQxActID) As Boolean
        Return HaveBase(actid + 1)
    End Function

    ''' <summary>
    ''' 是否有管理权限
    ''' </summary>
    Public Function HaveAdmin(ByVal actid As enQxActID) As Boolean
        Return HaveBase(actid + 3)
    End Function



    ''' <summary>
    ''' 是否有操作权限
    ''' </summary>
    Public Function Have(ByVal actid As Integer) As Boolean
        Return HaveBase(actid)
    End Function

    ''' <summary>
    ''' 是否有二级授权权限
    ''' </summary>
    Public Function HaveShouQuan2(ByVal actid As Integer) As Boolean
        Return HaveBase(actid + 2)
    End Function

    ''' <summary>
    ''' 是否有一级授权权限
    ''' </summary>
    Public Function HaveShouQuan1(ByVal actid As Integer) As Boolean
        Return HaveBase(actid + 1)
    End Function

    ''' <summary>
    ''' 是否有管理权限
    ''' </summary>
    Public Function HaveAdmin(ByVal actid As Integer) As Boolean
        Return HaveBase(actid + 3)
    End Function


    ''' <summary>
    ''' 是否有操作权限
    ''' </summary>
    Public Function Have(ByVal actid As Integer, ByVal itemid As Integer) As Boolean
        If itemid = -1 Then Return HaveBase(actid)
        Dim t As String
        Dim Actid_temp As Long
        Actid_temp = (Convert.ToInt64(actid) << 32) Or Convert.ToInt64(itemid)
        If Not _Qxitems_Item.ContainsKey(Actid_temp) Then
            Return HaveSuper(actid, itemid)
        End If

        t = _Qxitems_Item(Actid_temp)
        If t = "Y" Then
            Return True
        ElseIf t = "N" Then
            Return False
        Else
            Return HaveSuper(actid, itemid)
        End If
    End Function

    ''' <summary>
    ''' 是否有操作员权限查看权限
    ''' </summary>
    Public Function HaveCZYQXView() As Boolean
        If HaveBase(enQxActID.SH_AdminQX_View) Or HaveBase(enQxActID.SH_AdminQX_View + 3) Or HaveBase(enQxActID.SH_AdminQX_View + 1) Or HaveBase(enQxActID.SH_AdminQX_View + 2) Then
            Return True
        Else
            Return False
        End If
    End Function



    ''' <summary>
    ''' 是否有授权权限
    ''' </summary>
    Public Function HaveGive(ByVal actid As Integer) As Boolean
        Dim act As Integer = actid And &HFFFFFFF0
        Dim id As Integer = actid And &HF
        If id = 3 Then
            Return HaveAdmin(act)
        ElseIf id = 2 Then
            Return HaveAdmin(act)
        ElseIf id = 1 Then
            If HaveiShouQuan2(act) = "Y" Then
                Return True
            ElseIf HaveiShouQuan2(act) = "N" Then
                Return False
            Else
                Return HaveAdmin(act)
            End If
        ElseIf id = 0 Then
            If HaveiShouQuan1(act) = "Y" Then
                Return True
            ElseIf HaveiShouQuan1(act) = "N" Then
                Return False
            Else
                If HaveiShouQuan2(act) = "Y" Then
                    Return True
                ElseIf HaveiShouQuan2(act) = "N" Then
                    Return False
                Else
                    Return HaveAdmin(act)
                End If
            End If
        Else
            Return False
        End If
    End Function


    ''' <summary>
    ''' 是否有授权权限
    ''' </summary>
    Public Function HaveGive(ByVal act As enQxActID, ByVal id As Integer) As Boolean
        If id = 3 Then
            Return HaveAdmin(act)
        ElseIf id = 2 Then
            Return HaveAdmin(act)
        ElseIf id = 1 Then
            If HaveiShouQuan2(act) = "Y" Then
                Return True
            ElseIf HaveiShouQuan2(act) = "N" Then
                Return False
            Else
                Return HaveAdmin(act)
            End If
        ElseIf id = 0 Then
            If HaveiShouQuan1(act) = "Y" Then
                Return True
            ElseIf HaveiShouQuan1(act) = "N" Then
                Return False
            Else
                If HaveiShouQuan2(act) = "Y" Then
                    Return True
                ElseIf HaveiShouQuan2(act) = "N" Then
                    Return False
                Else
                    Return HaveAdmin(act)
                End If
            End If
        Else
            Return False
        End If
    End Function


    ''' <summary>
    ''' 是否有二级授权权限
    ''' </summary>
    Private Function HaveiShouQuan2(ByVal actid As Integer) As String
        Return HaveiBase(actid + 2)
    End Function

    ''' <summary>
    ''' 是否有一级授权权限
    ''' </summary>
    Private Function HaveiShouQuan1(ByVal actid As Integer) As String
        Return HaveiBase(actid + 1)
    End Function

    ''' <summary>
    ''' 是否有管理权限
    ''' </summary>
    Private Function HaveiAdmin(ByVal actid As Integer) As String
        Return HaveiBase(actid + 3)
    End Function

    ''' <summary>
    ''' 是否有操作权限，返回三态字符串
    ''' </summary>
    Private Function HaveiBase(ByVal actid As Integer) As String
        If Not _Qxitems.ContainsKey(actid) Then
            Return HaveiSuper(actid)
        End If
        If _Qxitems(actid) = "Y" Or _Qxitems(actid) = "N" Then Return _Qxitems(actid) Else Return HaveiSuper(actid)
    End Function


    ''' <summary>
    ''' 是否有超级权限，返回三态字符串
    ''' </summary>
    Private Function HaveiSuper(ByVal actid As Integer) As String
        actid = actid And &HFF000003
        If Not _Qxitems.ContainsKey(actid) Then Return ""
        Return _Qxitems(actid)
    End Function


    ''' <summary>
    ''' 是否有操作权限 
    ''' </summary>
    Private Function HaveBase(ByVal actid As Integer) As Boolean
        Dim t As String
        If Not _Qxitems.ContainsKey(actid) Then
            Return HaveSuper(actid)
        End If
        t = _Qxitems(actid)
        If t = "Y" Then
            Return True
        ElseIf t = "N" Then
            Return False
        Else
            Return HaveSuper(actid)
        End If
    End Function


    ''' <summary>
    ''' 是否有超级权限
    ''' </summary>
    Private Function HaveSuper(ByVal actid As Integer) As Boolean
        actid = actid And &HFF000003
        If Not _Qxitems.ContainsKey(actid) Then Return False
        Dim t = _Qxitems(actid)
        If t = "Y" Then
            Return True
        ElseIf t = "N" Then
            Return False
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 是否有超级权限
    ''' </summary>
    Private Function HaveSuper(ByVal actid As Integer, ByVal itemid As Integer) As Boolean
        actid = actid And &HFF000003
        Dim Actid_temp As Long
        Actid_temp = (Convert.ToInt64(actid) << 32) Or Convert.ToInt64(itemid)
        If Not _Qxitems_Item.ContainsKey(Actid_temp) Then Return False
        Dim t = _Qxitems_Item(Actid_temp)
        If t = "Y" Then
            Return True
        ElseIf t = "N" Then
            Return False
        Else
            Return False
        End If
    End Function


    '' <summary>
    '' 是否有操作权限
    '' </summary>
    Public Function Check_test(ByVal actid As Integer) As String

        Dim t As String
        If Not _Qxitems.ContainsKey(actid) Then
            actid = actid And &HFF000003
            If Not _Qxitems.ContainsKey(actid) Then Return "1权限和超级权限都没有{actid:" & actid.ToString() & "}"
            t = _Qxitems(actid)
            If t = "Y" Then
                Return "2权限没有，超级权限存在{actid:" & actid.ToString() & "}"
            ElseIf t = "N" Then
                Return "3权限没有，超级权限拒绝{actid:" & actid.ToString() & "}"
            Else
                Return "1权限和超级权限都没有{actid:" & actid.ToString() & "}"
            End If
        End If

        t = _Qxitems(actid)
        If t = "Y" Then
            Return "4权限存在{actid:" & actid.ToString() & "}"
        ElseIf t = "N" Then
            Return "5权限拒绝{actid:" & actid.ToString() & "}"
        Else
            actid = actid And &HFF000003
            If Not _Qxitems.ContainsKey(actid) Then Return "1权限和超级权限都没有{actid:" & actid.ToString() & "}"
            t = _Qxitems(actid)
            If t = "Y" Then
                Return "2权限没有，超级权限存在{actid:" & actid.ToString() & "}"
            ElseIf t = "N" Then
                Return "3权限没有，超级权限拒绝{actid:" & actid.ToString() & "}"
            Else
                Return "1权限和超级权限都没有{actid:" & actid.ToString() & "}"
            End If
        End If
    End Function

End Class
