''' <summary>
''' 功能：系统枚举
''' 时间：2013-3-7
''' 作者：陈辰
''' </summary>
''' <remarks>
''' 此枚举用于应对分离出的系统解决方案
''' 每次单独分离出系统，都需要在此增加枚举项
''' 
''' 2013-3-7 陈辰 初始化枚举 杭州贯通解决方案 HZGT 并 增加联通内部采购系统 UIPS
''' </remarks>
Public Enum enSystemSln As Integer

    ''' <summary>
    ''' 杭州贯通解决方案
    ''' </summary>
    ''' <remarks></remarks>
    HZGT = 1

    ''' <summary>
    ''' 联通内部采购系统
    ''' </summary>
    ''' <remarks></remarks>
    UIPS = 2

End Enum
