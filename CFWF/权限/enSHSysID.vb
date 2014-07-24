''' <summary>
''' 商户系统级ID
''' 对应不同类型的商户
''' </summary>
''' <remarks>
''' -2 所有商户
''' -1 平台商户
''' 0 所有的非平台商户
''' 1 导航商户
''' 2 B2C商户
''' 3 CMS商户
''' 4 短信商户
''' 5 食堂商户
''' 6 理财商户
''' 7 彩票商户
''' 8 联通内部采购商户
''' </remarks>
Public Enum enSHSysID As Integer
    ALL = -2
    PT = -1
    NPT = 0
    DH = 1
    B2C = 2
    CMS = 3
    SM = 4
    ST = 5
    LC = 6
    CP = 7
    UIPS = 8
End Enum
