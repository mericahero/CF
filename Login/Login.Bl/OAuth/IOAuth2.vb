
Public Interface IOAuth2

    ReadOnly Property Otype As Integer
    ReadOnly Property State As String

    'Function GetRequestAuthCodeURL(state As String, display As Integer) As String

    ReadOnly Property Access_token As String
    ReadOnly Property Refresh_token As String




    ReadOnly Property Expire As DateTime

    ReadOnly Property OUID As String

End Interface