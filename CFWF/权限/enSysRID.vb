''' <summary>
''' 系统级RID
''' </summary>
''' <remarks>此枚举常量将平台级的操作，对应到一个实际RID，比如商品属于商户部门，商品修改操作的RID就就是所属部门ID，但是平台级审核商品操作却没有对应的RID，可以认为是一个虚拟的根，
''' 但是为了方便，我们一般认为指定一个RID</remarks>
Public Enum enSysRID As Integer

    ''' <summary>
    ''' 平台公用
    ''' </summary>
    ''' <remarks></remarks>
    PT = 1

    ''' <summary>
    ''' 联通
    ''' </summary>
    ''' <remarks></remarks>
    UNICOM = 2

    ''' <summary>
    ''' B2C平台
    ''' </summary>
    ''' <remarks></remarks>
    B2CPT = 10

    ''' <summary>
    ''' 导航平台
    ''' </summary>
    ''' <remarks></remarks>
    DHPT = 20

    ''' <summary>
    ''' 短信平台
    ''' </summary>
    ''' <remarks></remarks>
    SMPT = 30

    ''' <summary>
    ''' CMS平台
    ''' </summary>
    ''' <remarks></remarks>
    CMSPT = 40

    ''' <summary>
    ''' 食堂平台
    ''' </summary>
    ''' <remarks></remarks>
    STPT = 50

    ''' <summary>
    ''' 彩票平台
    ''' </summary>
    ''' <remarks></remarks>
    CPPT = 60

    ''' <summary>
    ''' 联通内部采购平台
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT = 70


End Enum
