Public Class SysErrMsg

    Private Shared ReadOnly Property ErrMsg As Hashtable
        Get
            Dim o = TGConfig.UserDB.GetSQLRows("select * from syserr")
            If o Is Nothing Then
                Return Nothing
            Else
                Dim _errmsg As New Hashtable()
                For Each row As DataRow In o
                    _errmsg.Add(CInt(row("id")), row("msg"))
                Next

                Return _errmsg
            End If

        End Get
    End Property

    Public Shared Function GetErrMsg(ErrId As Integer) As String
        If ErrId > 0 Then
            Return ErrId.ToString()
        Else
            If ErrMsg.ContainsKey(ErrId) Then
                Return ErrMsg.Item(ErrId).ToString()
            Else
                Return ErrId.ToString()
            End If
        End If
    End Function

    Public Shared Function GetErrMsgXML(ErrId As Integer) As String
        If ErrId > 0 Then
            Return ErrId.ToString()
        End If
        If ErrId = -999 Then
            Return "-999"
        End If

        If ErrMsg.ContainsKey(ErrId) Then
            Return ErrMsg.Item(ErrId).ToString()
        Else
            Return ErrId.ToString()
        End If
    End Function

End Class
