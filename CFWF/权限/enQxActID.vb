
''' <summary>
''' 权限操作ID枚举常量
''' </summary>
''' <remarks>
''' </remarks>
Public Enum enQxActID As Integer

#Region "说明"

    '*********************此规则暂时作废*******************
    ' 权限枚举后缀
    ' I 代表insert插入的意思，一般指新增操作 
    ' V 代表view是查看的意思 
    ' U 代表update更新的意思，一般用于修改更新等操作 
    ' D 代表delete删除的意思，一般用于删除操作 
    ' S 代表shenhe审核的意思，一般用于审核操作 
    ' L 代表lock锁定的意思，一般用于锁定操作 
    ' UnL 代表unlock解锁的意思，一般用于解锁操作 

    ' W 维护权限，直接包含新增查看修改删除，当不需要精确控制时可以简单定义一个权限
    ' 注意，一般情况下，定义了W权限后就不能再定义IVUD权限，因为对于查看等操作程序将只检查W权限，不会再单独检查V权限
    ' 否则程序需要同时检查W权限与IVUD权限
    '*********************此规则暂时作废*******************




    '## 常量ID分配规则
    '## id=0xAABBCCDE
    '## AA ：应用系统127个,01-7F
    '##         01：平台公共系统
    '##	        02：商户公共系统
    '## 		10：导航商户系统
    '## 		11：导航平台系统
    '## 		12：B2C商户系统
    '## 		13：B2C平台系统
    '## 		14：CMS商户
    '## 		15：CMS平台

    '##BB : 子系统255个
    '       对应应用系统中的子系统，01-FF


    '## CC：权限序号255个，01-FF,一类操作为一个
    '## D : 其中二进制 4-5位保留为0，6-7位表示操作子类型，也就是只允许0,4,8,C
    '## 比如商品相关操作的CCD：
    '## 		商品新增：010
    '## 		商品查看：014
    '## 		商品修改：018
    '## 		商品删除：01C
    '
    '## E : 权限类别,二进制2-3位保留为0
    '##     0 : 操作权限
    '##     1 ：一级授权权限
    '##     2 : 二级授权权限
    '##     3 : 管理权限
    '## 因为每个操作都对应三种权限，所以实际只需要定义0结尾的操作权限

    '特殊位操作
    ' actid&0xffff0000=操作所属子系统
    ' actid&0xff000000=操作所属应用系统
    '
    ' actid&0xffff0003=操作所属子系统级别权限
    ' actid&0xff000003=操作所属应用系统级别权限

#End Region

#Region "01 平台公共系统"
    '01 平台公共系统*******************************************************************************************************************************
    ''' <summary>
    ''' 一般平台超级权限
    ''' </summary>
    PT_Super = &H1000000

    ' 101   品牌管理子系统****************************************************
    ''' <summary>
    ''' 品牌维护
    ''' </summary>
    PT_PinPai_WeiHu = &H1010140

    ' 102   商户管理子系统****************************************************
    ''' <summary>
    ''' 查看商户信息
    ''' </summary>
    PT_ShangHu_View = &H1020140
    ''' <summary>
    ''' 维护商户信息
    ''' </summary>
    PT_ShangHu_WeiHu = &H1020180
    ''' <summary>
    ''' 商户锁定
    ''' </summary>
    PT_ShangHu_Lock = &H1020240
    ''' <summary>
    ''' 商户解锁
    ''' </summary>
    PT_ShangHu_UnLock = &H1020340
    ''' <summary>
    ''' 商户建立审批
    ''' </summary>
    PT_ShangHu_ShenPi = &H1020440
    ''' <summary>
    ''' 商户账户查看
    ''' </summary>
    ''' <remarks></remarks>
    PT_ShangHuZhangHu_View = &H1020240
    ''' <summary>
    ''' 商户余额修改
    ''' </summary>
    ''' <remarks></remarks>
    PT_ShangHuYuE_Update = &H1020280
    ''' <summary>
    ''' 商户账户明细查看
    ''' </summary>
    ''' <remarks></remarks>
    PT_ShangHuZhangHuMingXi_View = &H1020340

    ' 104   分类管理子系统****************************************************
    ''' <summary>
    ''' 分类增加
    ''' </summary>
    PT_FenLei_Add = &H1040100
    ''' <summary>
    ''' 分类查看
    ''' </summary>
    PT_FenLei_View = &H1040140
    ''' <summary>
    ''' 分类修改
    ''' </summary>
    PT_FenLei_Update = &H1040180
    ''' <summary>
    ''' 分类删除
    ''' </summary>
    PT_FenLei_Del = &H10401C0

    ' 105   操作日志子系统****************************************************
    ''' <summary>
    ''' 操作日志查看
    ''' </summary>
    ''' <remarks></remarks>
    PT_ActionLog_View = &H1050140

#End Region

#Region "02 商户公共系统(泛)"
    '02 商户公共系统(泛)*******************************************************************************************************************************
    ''' <summary>
    ''' 一般平台超级权限
    ''' </summary>
    SH_Super = &H2000000

    ' 201   操作员管理子系统****************************************************
    ''' <summary>
    ''' 查看操作员信息
    ''' </summary>
    SH_Admin_View = &H2010140
    ''' <summary>
    ''' 操作员审核
    ''' </summary>
    SH_Admin_ShenHe = &H2010280
    ''' <summary>
    ''' 操作员锁定
    ''' </summary>
    SH_Admin_Lock = &H2010380
    ''' <summary>
    ''' 操作员解锁
    ''' </summary>
    SH_Admin_UnLock = &H2010480
    ''' <summary>
    ''' 操作员删除
    ''' </summary>
    SH_Admin_Del = &H20101C0
    ''' <summary>
    ''' 操作员权限查看
    ''' </summary>
    SH_AdminQX_View = &H2010240

    ' 202   商户信息管理子系统****************************************************
    ''' <summary>
    ''' 商户信息查看
    ''' </summary>
    SH_ShangHu_View = &H2020140
    ''' <summary>
    ''' 商户信息修改
    ''' </summary>
    SH_ShangHu_Update = &H2020180
    ''' <summary>
    ''' 商户账户查看
    ''' </summary>
    ''' <remarks></remarks>
    SH_ZhangHu_View = &H2020240
    ''' <summary>
    ''' 商户账户明细查看
    ''' </summary>
    ''' <remarks></remarks>
    SH_ZhangHuMingXi_View = &H2020340

#End Region

#Region "03 联通管理系统"
    '   03      联通管理系统 *******************************************************************************************************************************
    ''' <summary>
    ''' 联通超级权限
    ''' </summary>
    ''' <remarks></remarks>
    UNICOM_Super = &H3000000

    '   0301    结算管理子系统   ****************************************************
    ''' <summary>
    ''' 查看结算信息
    ''' </summary>
    ''' <remarks></remarks>
    UNICOM_JieSuan_View = &H3010140
    ''' <summary>
    ''' 修改结算状态为佣金收讫
    ''' </summary>
    ''' <remarks></remarks>
    UNICOM_JieSuanBZ_YongJinShouQi = &H3010180

#End Region

#Region "10 导航商户系统"
    '10 导航商户系统*******************************************************************************************************************************
    ''' <summary>
    ''' 导航商户超级权限
    ''' </summary>
    DH_Super = &H10000000

    ' 1001  商品信息管理子系统****************************************************
    ''' <summary>
    ''' 商品信息增加
    ''' </summary>
    DH_ShangPin_Add = &H10010100
    ''' <summary>
    ''' 商品信息查看
    ''' </summary>
    DH_ShangPin_View = &H10010140
    ''' <summary>
    ''' 商品信息修改
    ''' </summary>
    DH_ShangPin_Update = &H10010180
    ''' <summary>
    ''' 商品图片增加
    ''' </summary>
    DH_ShangPinImg_Add = &H10010200
    ''' <summary>
    ''' 商品图片查看
    ''' </summary>
    DH_ShangPinImg_View = &H10010240
    ''' <summary>
    ''' 商品图片修改
    ''' </summary>
    DH_ShangPinImg_Update = &H10010280
    ''' <summary>
    ''' 优惠时间修改
    ''' </summary>
    DH_ShangPinTime_Update = &H10010380
    ''' <summary>
    ''' 上下架操作
    ''' </summary>
    DH_ShangPinShangJia_Update = &H10010480
    ''' <summary>
    ''' 销售价格修改
    ''' </summary>
    DH_ShangPinPrice_Update = &H10010580

    ' 1002  店铺管理子系统****************************************************
    ''' <summary>
    ''' 店铺信息查看
    ''' </summary>
    DH_ShangPu_View = &H10020140
    ''' <summary>
    ''' 店铺信息添加
    ''' </summary>
    DH_ShangPu_Add = &H10020100
    ''' <summary>
    ''' 店铺信息修改
    ''' </summary>
    DH_ShangPu_Update = &H10020180
    ''' <summary>
    ''' 店铺删除
    ''' </summary>
    DH_ShangPu_Del = &H100201C0

    '1003  统计数据查看权限系统****************************************************
    ''' <summary>
    '''  点击数查看权限
    ''' </summary>
    DHSH_Hit_View = &H10030140

#End Region

#Region "11 导航平台系统"
    '11 导航平台系统*******************************************************************************************************************************
    ''' <summary>
    ''' 导航平台超级权限
    ''' </summary>
    DHPT_Super = &H11000000

    ' 1101   商品管理系统****************************************************
    ''' <summary>
    ''' 商品信息查看
    ''' </summary>
    DHPT_ShangPin_View = &H11010100
    ''' <summary>
    ''' 审核商品
    ''' </summary>
    DHPT_ShangPin_ShenHe = &H11010140
    ''' <summary>
    ''' 锁定商品
    ''' </summary>
    ''' <remarks></remarks>
    DHPT_ShangPin_Lock = &H11010240
    ''' <summary>
    ''' 解锁商品
    ''' </summary>
    ''' <remarks></remarks>
    DHPT_ShangPin_UnLock = &H11010340

    ' 1102  统计数据查看权限系统****************************************************
    ''' <summary>
    '''  点击数查看权限
    ''' </summary>
    DHPT_Hit_View = &H11020140

#End Region

#Region "12 B2C商户系统"

    '   12  B2C商户系统 *******************************************************************************************************************************
    ''' <summary>
    ''' B2C商户超级权限
    ''' </summary>
    B2CSH_Super = &H12000000

    '   1201    商品子系统   ****************************************************
    ''' <summary>
    ''' 新增商品
    ''' </summary>
    B2CSH_ShangPin_Add = &H12010100
    ''' <summary>
    ''' 查看商品信息
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_ShangPin_View = &H12010140
    ''' <summary>
    ''' 修改商品信息
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_ShangPin_Update = &H12010180
    ''' <summary>
    ''' 商品删除
    ''' </summary>
    B2CSH_ShangPin_Del = &H120101C0
    ''' <summary>
    ''' 修改商品信息时使用品牌的权限
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_PinPai_View = &H12010240
    ''' <summary>
    ''' 修改商品信息时使用分类的权限
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_FenLei_View = &H12010340
    ''' <summary>
    ''' 商品价格修改
    ''' </summary>
    B2CSH_ShangPinPrice_Update = &H12010280
    ''' <summary>
    ''' 商品库存修改
    ''' </summary>
    B2CSH_ShangPinKuCun_Update = &H12010380
    ''' <summary>
    ''' 商品销量修改
    ''' </summary>
    B2CSH_ShangPinXiaoLiang_Update = &H12010480
    ''' <summary>
    ''' 商品图片增加
    ''' </summary>
    B2CSH_ShangPinImg_Add = &H12010200
    ''' <summary>
    ''' 商品图片修改
    ''' </summary>
    B2CSH_ShangPinImg_Update = &H12010580
    ''' <summary>
    ''' 商品图片删除
    ''' </summary>
    B2CSH_ShangPinImg_Del = &H120102C0
    ''' <summary>
    ''' 商品上架
    ''' </summary>
    B2CSH_ShangPinShangJia_Update = &H12010680
    ''' <summary>
    ''' 商品下架
    ''' </summary>
    B2CSH_ShangPinXiaJia_Update = &H12010780
    ''' <summary>
    ''' 商品促销设置
    ''' </summary>
    B2CSH_ShangPinCuXiao_Update = &H12010880
    ''' <summary>
    ''' 商品咨询查看
    ''' </summary>
    B2CSH_ShangPinZiXun_View = &H12010440
    ''' <summary>
    ''' 商品咨询回复
    ''' </summary>
    B2CSH_ShangPinZiXun_HuiFu = &H12010300
    ''' <summary>
    ''' 商品咨询删除
    ''' </summary>
    B2CSH_ShangPinZiXun_Del = &H120103C0

    '   1202    订单子系统   ****************************************************
    ''' <summary>
    ''' 订单查看
    ''' </summary>
    B2CSH_DD_View = &H12020140
    ''' <summary>
    ''' 订单状态改为部分付款
    ''' </summary>
    B2CSH_DDPayPart_Update = &H12020180
    ''' <summary>
    ''' 订单状态改为付款完成
    ''' </summary>
    B2CSH_DDPayComp_Update = &H12020280
    ''' <summary>
    ''' 订单状态改为商品出库中
    ''' </summary>
    B2CSH_DDChuKuZhong_Update = &H12020380
    ''' <summary>
    ''' 订单状态改为等待收货
    ''' </summary>
    B2CSH_DDWaitPro_Update = &H12020480
    ''' <summary>
    ''' 录入订单物流信息
    ''' </summary>
    B2CSH_DDWuLiuInfo_Add = &H12020100
    ''' <summary>
    ''' 订单状态改为订单完成
    ''' </summary>
    B2CSH_DDComplite_Update = &H12020580
    ''' <summary>
    ''' 订单状态改为退货
    ''' </summary>
    B2CSH_DDTuiHuo_Update = &H12020680
    ''' <summary>
    ''' 订单状态改为取消订单
    ''' </summary>
    B2CSH_DDCancel_Update = &H12020780

    '   1203    订单留言子系统 ****************************************************
    ''' <summary>
    ''' 订单留言的查看
    ''' </summary>
    B2CSH_DDMsg_View = &H12030140
    ''' <summary>
    ''' 订单留言的回复
    ''' </summary>
    B2CSH_DDMsg_HuiFu = &H12030100
    ''' <summary>
    ''' 订单留言的删除
    ''' </summary>
    B2CSH_DDMsg_Del = &H120301C0

    '   1204    订单支付子系统 ****************************************************
    ''' <summary>
    ''' 查看订单支付
    ''' </summary>
    B2CSH_DDPay_View = &H12040140
    ''' <summary>
    ''' 查看订单支付统计
    ''' </summary>
    B2CSH_DDPayTongJi_View = &H12040240
    ''' <summary>
    ''' 未收款明细
    ''' </summary>
    B2CSH_WeiShouKuan_View = &H12040440

    '   1205    供货商管理系统 ****************************************************
    ''' <summary>
    ''' 供货商查看
    ''' </summary>
    B2CSH_GongHuoShang_View = &H12050140
    ''' <summary>
    ''' 供货商增加
    ''' </summary>
    B2CSH_GongHuoShang_Add = &H12050100
    ''' <summary>
    ''' 供货商修改
    ''' </summary>
    B2CSH_GongHuoShang_Update = &H12050180
    ''' <summary>
    ''' 供货商删除
    ''' </summary>
    B2CSH_GongHuoShang_Del = &H120501C0

    '   1206    结算系统    ****************************************************
    ''' <summary>
    ''' 结算信息查看
    ''' </summary>
    B2CSH_JieSuan_View = &H12060140
    ''' <summary>
    ''' 查看销售明细
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_XiaoShouMingXi_View = &H12060240
    ''' <summary>
    ''' 修改结算状态为商户已确认
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_JieSuanBZ_SHConfirm = &H12060380
    ''' <summary>
    ''' 修改结算状态为佣金已支付
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_JieSuanBZ_YongJinZhiFu = &H12060480
    ''' <summary>
    ''' 修改结算状态为货款收讫
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_JieSuanBZ_HuoKuanShouQi = &H12060580

    'TODO:2012-10-9新增 1207 1208 1209 1210 1211
    '   1207    购买方式    ****************************************************
    ''' <summary>
    ''' 购买方式查看
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_BuyWay_View = &H12070140
    ''' <summary>
    ''' 购买方式增加
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_BuyWay_Add = &H12070100
    ''' <summary>
    ''' 购买方式修改
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_BuyWay_Update = &H12070180
    ''' <summary>
    ''' 购买方式删除
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_BuyWay_Del = &H120701C0
    ''' <summary>
    ''' B2C商户绑定地区到购买方式
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_DiQuToBuyWay_Bind = &H12070200
    ''' <summary>
    ''' B2C商户绑定套餐到地区
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_TaoCanToDiqu_Bind = &H12070300
    ''' <summary>
    ''' B2C商户绑定号码池到地区
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_HaoMaChiToDiqu_Bind = &H12070400
    ''' <summary>
    ''' B2C商户修改地区资费方式
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_DiQuZifei_Update = &H12070500
    ''' <summary>
    ''' B2C商户绑定购买方式到商品
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_BuyWayToShangPin_Bind = &H12070600
    ''' <summary>
    ''' B2C商户修改地区
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_Diqu_Update = &H12070700

    '   1208    手机套餐    ****************************************************
    ''' <summary>
    ''' 手机套餐查看
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_TaoCan_View = &H12080140
    ''' <summary>
    ''' 手机套餐增加
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_TaoCan_Add = &H12080100
    ''' <summary>
    ''' 手机套餐修改
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_TaoCan_Update = &H12080180
    ''' <summary>
    ''' 手机套餐删除
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_TaoCan_Del = &H120801C0

    '   1209    手机套餐选项    ****************************************************
    ''' <summary>
    ''' 手机套餐选项查看
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_TaoCanInfo_View = &H12090140
    ''' <summary>
    ''' 手机套餐选项增加
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_TaoCanInfo_Add = &H12090100
    ''' <summary>
    ''' 手机套餐选项修改
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_TaoCanInfo_Update = &H12090180
    ''' <summary>
    ''' 手机套餐选项删除
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_TaoCanInfo_Del = &H120901C0

    '   1210    号码池    ****************************************************
    ''' <summary>
    ''' 号码池查看
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_HaoMaChi_View = &H12100140
    ''' <summary>
    ''' 号码池增加
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_HaoMaChi_Add = &H12100100
    ''' <summary>
    ''' 号码池修改
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_HaoMaChi_Update = &H12100180
    ''' <summary>
    ''' 号码池删除
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_HaoMaChi_Del = &H121001C0

    '   1211    号码    ****************************************************
    ''' <summary>
    ''' 号码查看
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_HaoMa_View = &H12110140
    ''' <summary>
    ''' 号码增加
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_HaoMa_Add = &H12110100
    ''' <summary>
    ''' 号码删除
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_HaoMa_Del = &H121101C0

    '   1212    商品关联    ****************************************************
    ''' <summary>
    ''' 查看关联组
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_GuanLian_View = &H12120140
    ''' <summary>
    ''' 增加关联组
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_GuanLian_Add = &H12120100
    ''' <summary>
    ''' 修改关联组
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_GuanLian_Update = &H12120180
    ''' <summary>
    ''' 删除关联组
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_GuanLian_Del = &H121201C0

    '   1213    商品自提点管理    ****************************************************
    ''' <summary>
    ''' 自提点分组查看
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_ZiTi_View = &H12130140
    ''' <summary>
    ''' 增加自提点分组
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_ZiTi_Add = &H12130100
    ''' <summary>
    ''' 修改自提点分组
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_ZiTi_Update = &H12130180
    ''' <summary>
    ''' 删除自提点分组
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_ZiTi_Del = &H121301C0

    ''' <summary>
    ''' 自提点分组查看
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_ZiTiGroup_View = &H12130240
    ''' <summary>
    ''' 增加自提点分组
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_ZiTiGroup_Add = &H12130200
    ''' <summary>
    ''' 修改自提点分组
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_ZiTiGroup_Update = &H12130280
    ''' <summary>
    ''' 删除自提点分组
    ''' </summary>
    ''' <remarks></remarks>
    B2CSH_ZiTiGroup_Del = &H121302C0

#End Region

#Region "13 B2C平台系统"
    '   13      B2C平台系统     *******************************************************************************************************************************
    ''' <summary>
    ''' B2C平台超级权限
    ''' </summary>
    B2CPT_Super = &H13000000

    '   1301    商品子系统   ****************************************************
    ''' <summary>
    ''' 商品查看
    ''' </summary>
    B2CPT_ShangPin_View = &H13010140
    ''' <summary>
    ''' 商品图片查看
    ''' </summary>
    B2CPT_ShangPinImg_View = &H13010240
    ''' <summary>
    ''' 商品删除
    ''' </summary>
    B2CPT_ShangPin_Del = &H130101C0
    ''' <summary>
    ''' 商品咨询查看
    ''' </summary>
    B2CPT_ShangPinZiXun_View = &H13010340
    ''' <summary>
    ''' 商品咨询回复
    ''' </summary>
    B2CPT_ShangPinZiXun_HuiFu = &H13010200
    ''' <summary>
    ''' 商品咨询删除
    ''' </summary>
    B2CPT_ShangPinZiXun_Del = &H130102C0
    ''' <summary>
    ''' 设置商品标识：推荐热买标识
    ''' </summary>
    B2CPT_ShangPinBZ_Set = &H13010380

    '   1302    订单子系统   ****************************************************
    ''' <summary>
    ''' 订单查看
    ''' </summary>
    B2CPT_DD_View = &H13020140
    ''' <summary>
    ''' 订单回滚
    ''' </summary>
    B2CPT_DDBack_Update = &H13020180
    ''' <summary>
    ''' 订单留言的查看
    ''' </summary>
    B2CPT_DDMsg_View = &H13020240
    ''' <summary>
    ''' 订单留言的回复
    ''' </summary>
    B2CPT_DDMsg_HuiFu = &H13020200
    ''' <summary>
    ''' 订单留言的删除
    ''' </summary>
    B2CPT_DDMsg_Del = &H130202C0

    '   1303    订单支付子系统 ****************************************************
    ''' <summary>
    ''' 查看订单支付
    ''' </summary>
    B2CPT_DDPay_View = &H13030140
    ''' <summary>
    ''' 查看订单支付统计
    ''' </summary>
    B2CPT_DDPayTongJi_View = &H13030240

    '   1310    结算管理系统  ****************************************************
    ''' <summary>
    ''' 结算数据查询
    ''' </summary>
    B2CPT_JieSuan_View = &H13100140
    ''' <summary>
    ''' 结算数据修改
    ''' </summary>
    B2CPT_JieSuan_Update = &H13100180
    ''' <summary>
    ''' 结算状态修改（作废）
    ''' </summary>
    B2CPT_JieSuanbz_Update = &H13100280
    ''' <summary>
    ''' 查看销售明细
    ''' </summary>
    ''' <remarks></remarks>
    B2CPT_XiaoShouMingXi_View = &H13100340

    ''' <summary>
    ''' 修改结算状态为平台已确认
    ''' </summary>
    ''' <remarks></remarks>
    B2CPT_JieSuanBZ_PTConfirm = &H13100480
    ''' <summary>
    ''' 修改结算状态为货款已支付
    ''' </summary>
    ''' <remarks></remarks>
    B2CPT_JieSuanBZ_HuoKuanZhiFu = &H13100580

    '   1311    分成比例管理子系统   ****************************************************
    ''' <summary>
    ''' 分成比例查询
    ''' </summary>
    B2CPT_FenChengBiLi_View = &H13110140
    ''' <summary>
    ''' 分成比例维护
    ''' </summary>
    B2CPT_FenChengBiLi_WeiHu = &H13110180

    '   1312    商品关联    ****************************************************
    ''' <summary>
    ''' 查看关联组
    ''' </summary>
    ''' <remarks></remarks>
    B2CPT_GuanLian_View = &H13120140
    ''' <summary>
    ''' 增加关联组
    ''' </summary>
    ''' <remarks></remarks>
    B2CPT_GuanLian_Add = &H13120100
    ''' <summary>
    ''' 修改关联组
    ''' </summary>
    ''' <remarks></remarks>
    B2CPT_GuanLian_Update = &H13120180
    ''' <summary>
    ''' 删除关联组
    ''' </summary>
    ''' <remarks></remarks>
    B2CPT_GuanLian_Del = &H131201C0

    '   1313    用户账户余额管理子系统    ****************************************************
    ''' <summary>
    ''' 查看用户账户
    ''' </summary>
    ''' <remarks></remarks>
    B2CPT_UsrAccount_View = &H13130140
    ''' <summary>
    ''' 用户账户金额修改
    ''' </summary>
    ''' <remarks></remarks>
    B2CPT_UsrAccountAmount_Update = &H13130180

#End Region

#Region "14 CMS商户系统"
    '14 CMS商户系统*******************************************************************************************************************************
    ''' <summary>
    ''' CMS平台超级权限
    ''' </summary>
    CMSSH_Super = &H14000000

#End Region

#Region "15 CMS平台系统"
    '15 CMS平台系统*******************************************************************************************************************************
    ''' <summary>
    ''' CMS平台超级权限
    ''' </summary>
    CMSPT_Super = &H15000000

    ' 1501   版面维护子系统****************************************************
    ''' <summary>
    ''' 版面维护
    ''' </summary>
    CMSPT_BanMian_WeiHu = &H15010140

    ' 1502   文章管理子系统****************************************************
    ''' <summary>
    ''' 文章增加
    ''' </summary>
    CMSPT_WenZhang_Add = &H15020100
    ''' <summary>
    ''' 文章查看
    ''' </summary>
    CMSPT_WenZhang_View = &H15020140
    ''' <summary>
    ''' 文章修改
    ''' </summary>
    CMSPT_WenZhang_Update = &H15020180
    ''' <summary>
    ''' 文章删除
    ''' </summary>
    CMSPT_WenZhang_Del = &H150201C0
    ''' <summary>
    ''' 图片修改
    ''' </summary>
    CMSPT_Image_Upload = &H15020280
    ''' <summary>
    ''' 文章标志设置
    ''' </summary>
    CMSPT_WenZhangBZ_Update = &H15020380

#End Region

#Region "16 短信商户系统"

    '16 短信商户系统*******************************************************************************************************************************
    ''' <summary>
    ''' 短信商户超级权限
    ''' </summary>
    SMSH_Super = &H16000000

    ' 1601   商户子系统****************************************************
    ''' <summary>
    ''' 商户信息查看
    ''' </summary>
    SMSH_ShangHu_View = &H16010140

    ' 1602   短信维护子系统****************************************************
    ''' <summary>
    ''' 短信内容查看
    ''' </summary>
    SMSH_DuanXin_View = &H16020140
    ''' <summary>
    ''' 短信内容添加
    ''' </summary>
    ''' <remarks></remarks>
    SMSH_DuanXin_Add = &H16020100
    ''' <summary>
    ''' 短信内容修改
    ''' </summary>
    ''' <remarks></remarks>
    SMSH_DuanXin_Update = &H16020180
    ''' <summary>
    ''' 短信内容删除
    ''' </summary>
    ''' <remarks></remarks>
    SMSH_DuanXin_Del = &H160201C0
    ''' <summary>
    ''' 短信发送
    ''' </summary>
    ''' <remarks></remarks>
    SMSH_DuanXin_Send = &H16020200
    ''' <summary>
    ''' 短信发送记录查看
    ''' </summary>
    ''' <remarks></remarks>
    SMSH_DuanXinSend_View = &H16020240

    ' 1603   地址组维护子系统****************************************************
    ''' <summary>
    ''' 地址组查看
    ''' </summary>
    SMSH_TelBook_View = &H16030140
    ''' <summary>
    ''' 地址组增加
    ''' </summary>
    SMSH_TelBook_Add = &H16030100
    ''' <summary>
    ''' 地址组修改
    ''' </summary>
    SMSH_TelBook_Update = &H16030180
    ''' <summary>
    ''' 地址组删除
    ''' </summary>
    SMSH_TelBook_Del = &H160301C0

    ''' <summary>
    ''' 地址组人员查看
    ''' </summary>
    SMSH_TelBookMember_View = &H16030140
    ''' <summary>
    ''' 地址组人员增加
    ''' </summary>
    SMSH_TelBookMember_Add = &H16030100
    ''' <summary>
    ''' 地址组人员修改
    ''' </summary>
    SMSH_TelBookMember_Update = &H16030180
    ''' <summary>
    ''' 地址组人员删除
    ''' </summary>
    SMSH_TelBookMember_Del = &H160301C0

#End Region

#Region "17 短信平台系统"
    '17 短信平台系统*******************************************************************************************************************************
    ''' <summary>
    ''' 短信平台超级权限
    ''' </summary>
    SMPT_Super = &H17000000

    ' 1701   商户子系统****************************************************
    ''' <summary>
    ''' 商户信息维护
    ''' </summary>
    SMPT_ShangHu_WeiHu = &H17010140

    ' 1702   短信维护子系统****************************************************
    ''' <summary>
    ''' 短信内容查看
    ''' </summary>
    SMPT_DuanXin_View = &H17020140
    ''' <summary>
    ''' 短信内容审核
    ''' </summary>
    ''' <remarks></remarks>
    SMPT_DuanXin_ShenHe = &H17020240
    ''' <summary>
    ''' 短信发送记录查看
    ''' </summary>
    ''' <remarks></remarks>
    SMPT_DuanXinSend_View = &H17020340

    ' 1703   商户费用信息子系统****************************************************
    ''' <summary>
    ''' 预收费商户费用信息查看
    ''' </summary>
    SMPT_PreChargeSHFee_View = &H17030140
    ''' <summary>
    ''' 后收费商户费用信息查看
    ''' </summary>
    SMPT_LaterChargeSHFee_View = &H17030240
    ''' <summary>
    ''' 后收费商户费用信息更新
    ''' </summary>
    SMPT_LaterChargeSHFee_Update = &H17030340
    ''' <summary>
    ''' 托管商户费用信息查看
    ''' </summary>
    SMPT_HostingChargeSHFee_View = &H17030440

    ' 1704   商户余额管理子系统****************************************************
    ''' <summary>
    ''' 查看商户余额
    ''' </summary>
    SMPT_ShangHuYuE_View = &H17040440
    ''' <summary>
    ''' 更新商户余额
    ''' </summary>
    SMPT_ShangHuYuE_Update = &H17040480

#End Region

#Region "18 食堂商户系统"

    '18 食堂商户系统*******************************************************************************************************************************
    ''' <summary>
    ''' 食堂商户超级权限
    ''' </summary>
    STSH_Super = &H18000000

    ' 1801   商户子系统****************************************************
    ''' <summary>
    ''' 商户信息查看
    ''' </summary>
    STSH_ShangHu_View = &H18010140
    ''' <summary>
    ''' 食堂信息查看
    ''' </summary>
    ''' <remarks></remarks>
    STSH_ShiTang_View = &H18010240

    ' 1802   食堂内容维护子系统****************************************************
    ''' <summary>
    ''' 食堂内容查看
    ''' </summary>
    STSH_NeiRong_View = &H18020140
    ''' <summary>
    ''' 食堂内容添加
    ''' </summary>
    ''' <remarks></remarks>
    STSH_NeiRong_Add = &H18020100
    ''' <summary>
    ''' 食堂内容修改
    ''' </summary>
    ''' <remarks></remarks>
    STSH_NeiRong_Update = &H18020180
    ''' <summary>
    ''' 食堂内容删除
    ''' </summary>
    ''' <remarks></remarks>
    STSH_NeiRong_Del = &H180201C0
    ''' <summary>
    '''  食堂商户留言回复
    ''' </summary>
    ''' <remarks></remarks>
    STSH_Msg_Reply = &H18020200
    ''' <summary>
    ''' 食堂商户留言查看
    ''' </summary>
    ''' <remarks></remarks>
    STSH_Msg_View = &H18020240

#End Region

#Region "19 食堂平台系统"
    '19 食堂平台系统*******************************************************************************************************************************
    ''' <summary>
    ''' 食堂平台超级权限
    ''' </summary>
    STPT_Super = &H19000000

    ' 1901   商户子系统****************************************************
    ''' <summary>
    ''' 商户信息维护
    ''' </summary>
    STPT_ShangHu_WeiHu = &H19010140

    ' 1902   食堂子系统****************************************************
    ''' <summary>
    ''' 食堂增加
    ''' </summary>
    ''' <remarks></remarks>
    STPT_ShiTang_Add = &H19020100
    ''' <summary>
    ''' 食堂查看
    ''' </summary>
    ''' <remarks></remarks>
    STPT_ShiTang_View = &H19020140
    ''' <summary>
    ''' 食堂修改
    ''' </summary>
    ''' <remarks></remarks>
    STPT_ShiTang_Update = &H19020180
    ''' <summary>
    ''' 食堂删除
    ''' </summary>
    ''' <remarks></remarks>
    STPT_ShiTang_Del = &H190201C0
    ''' <summary>
    ''' 食堂平台资质增加
    ''' </summary>
    ''' <remarks></remarks>
    STPT_ZiZhi_Add = &H19020200
    ''' <summary>
    ''' 食堂平台资质查看
    ''' </summary>
    ''' <remarks></remarks>
    STPT_ZiZhi_View = &H19020240
    ''' <summary>
    ''' 食堂平台资质修改
    ''' </summary>
    ''' <remarks></remarks>
    STPT_ZiZhi_Update = &H19020280
    ''' <summary>
    ''' 食堂平台资质删除
    ''' </summary>
    ''' <remarks></remarks>
    STPT_ZiZhi_Del = &H190202C0
    ''' <summary>
    ''' 食堂平台留言回复
    ''' </summary>
    ''' <remarks></remarks>
    STPT_Msg_Reply = &H19020300
    ''' <summary>
    ''' 食堂平台留言查看
    ''' </summary>
    ''' <remarks></remarks>
    STPT_Msg_View = &H19020340

#End Region

#Region "20 彩票商户系统"

    '20 彩票商户系统*******************************************************************************************************************************
    ''' <summary>
    ''' 彩票商户超级权限
    ''' </summary>
    CPSH_Super = &H20000000

    ' 2001   商户子系统****************************************************
    ''' <summary>
    ''' 商户信息查看
    ''' </summary>
    CPSH_ShangHu_View = &H20010140
    ''' <summary>
    ''' 查看警告信息
    ''' </summary>
    ''' <remarks></remarks>
    CPSH_Warning_View = &H20010240

    ' 2002   期信息维护子系统****************************************************
    ''' <summary>
    ''' 期信息查看
    ''' </summary>
    CPSH_Qi_View = &H20020140
    ''' <summary>
    ''' 期信息添加
    ''' </summary>
    ''' <remarks></remarks>
    CPSH_Qi_Add = &H20020100
    ''' <summary>
    ''' 期信息修改
    ''' </summary>
    ''' <remarks></remarks>
    CPSH_Qi_Update = &H20020180

    ' 2003   订单维护子系统****************************************************
    ''' <summary>
    ''' 订单查看
    ''' </summary>
    CPSH_DingDan_View = &H20030140
    ''' <summary>
    ''' 订单状态修改
    ''' </summary>
    ''' <remarks></remarks>
    CPSH_DingDanZT_Update = &H20030180


    ' 2004   提现管理子系统****************************************************
    ''' <summary>
    ''' 提现申请查看
    ''' </summary>
    CPSH_TiXianApply_View = &H20040140
    ''' <summary>
    ''' 提现申请修改
    ''' </summary>
    ''' <remarks></remarks>
    CPSH_TiXianApply_Update = &H20040180


    ' 2005   财务管理子系统****************************************************
    ''' <summary>
    ''' 账务信息查看
    ''' </summary>
    CPSH_JieSuan_View = &H20050140
    ''' <summary>
    ''' 财务信息结算更新
    ''' </summary>
    ''' <remarks></remarks>
    CPSH_JieSuan_Update = &H20050180


    ' 2006   中奖维护子系统****************************************************
    ''' <summary>
    ''' 中奖信息查看
    ''' </summary>
    CPSH_ZhongJiang_View = &H20060140

    ''' <summary>
    ''' 小奖派奖（三等奖以下）
    ''' </summary>
    ''' <remarks></remarks>
    CPSH_ZhongJiang_PaiJiang = &H20060180


#End Region

#Region "21 彩票平台系统"
    '21 彩票平台系统*******************************************************************************************************************************
    ''' <summary>
    ''' 彩票平台超级权限
    ''' </summary>
    CPPT_Super = &H21000000

    ' 2101   商户子系统****************************************************
    ''' <summary>
    ''' 商户信息维护
    ''' </summary>
    CPPT_ShangHu_WeiHu = &H21010140

    ' 2102   系统管理子系统****************************************************
    ''' <summary>
    ''' 系统维护
    ''' </summary>
    ''' <remarks></remarks>
    CPPT_System_Set = &H21020100

    ' 2105   财务管理子系统****************************************************
    ''' <summary>
    ''' 账务信息查看
    ''' </summary>
    CPPT_JieSuan_View = &H21050140
    ''' <summary>
    ''' 财务信息结算更新
    ''' </summary>
    ''' <remarks></remarks>
    CPPT_JieSuan_Update = &H21050180

#End Region

#Region "22 理财商户系统"

    '22 理财商户系统*******************************************************************************************************************************
    ''' <summary>
    ''' 理财商户超级权限
    ''' </summary>
    LCSH_Super = &H22000000

    ' 2201   商户子系统****************************************************
    ''' <summary>
    ''' 商户信息查看
    ''' </summary>
    LCSH_ShangHu_View = &H22010140

    ' 2202   理财产品管理子系统****************************************************
    ''' <summary>
    ''' 查看理财产品
    ''' </summary>
    LCSH_InvestPro_View = &H22020140
    ''' <summary>
    ''' 增加理财产品
    ''' </summary>
    ''' <remarks></remarks>
    LCSH_InvestPro_Add = &H22020100
    ''' <summary>
    ''' 修改理财产品
    ''' </summary>
    ''' <remarks></remarks>
    LCSH_InvestPro_Update = &H22020180
    ''' <summary>
    ''' 删除理财产品
    ''' </summary>
    ''' <remarks></remarks>
    LCSH_InvestPro_Del = &H220201C0

#End Region


#Region "24 联通合约购机系统"

    '24  商户系统 *******************************************************************************************************************************
    ''' <summary>
    ''' 超级权限
    ''' </summary>
    LTDataSH_Super = &H24000000

    '   2401   商户子系统****************************************************
    ''' <summary>
    ''' 商户信息查看
    ''' </summary>
    LTDataSH_ShangHu_View = &H24010140

    '   2402    同步数据    ****************************************************
    ''' <summary>
    ''' 查看数据
    ''' </summary>
    ''' <remarks></remarks>
    LTDataSH_SyncLog_View = &H24020140
    ''' <summary>
    ''' 处理数据
    ''' </summary>
    ''' <remarks></remarks>
    LTDataSH_SyncLog_Deal = &H24020100
#End Region

#Region "25 XX平台系统"

#End Region


#Region "26 联通内部采购平台系统"
    '26 联通内部采购平台系统*******************************************************************************************************************************
    ''' <summary>
    ''' 联通内部采购平台超级权限
    ''' </summary>
    UIPSPT_Super = &H26000000

    ''' <summary>
    ''' 系统配置管理
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_SystemConfig = &H26000040

    ' 2601   公告子系统****************************************************
    ''' <summary>
    ''' 新增公告
    ''' </summary>
    UIPSPT_Announcement_Add = &H26010100
    ''' <summary>
    ''' 查看公告
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Announcement_View = &H26010140
    ''' <summary>
    ''' 修改公告
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Announcement_Update = &H26010180
    ''' <summary>
    ''' 删除公告
    ''' </summary>
    UIPSPT_Announcement_Del = &H260101C0



    ' 2602   商品子系统****************************************************
    ''' <summary>
    ''' 新增商品
    ''' </summary>
    UIPSPT_Goods_Add = &H26020100
    ''' <summary>
    ''' 查看商品
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Goods_View = &H26020140
    ''' <summary>
    ''' 修改商品
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Goods_Update = &H26020180
    ''' <summary>
    ''' 删除商品
    ''' </summary>
    UIPSPT_Goods_Del = &H260201C0

    ''' <summary>
    ''' 商品图片维护
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_GoodsImage_WeiHu = &H26020200


    ' 2603   合同子系统****************************************************
    ''' <summary>
    ''' 新增合同
    ''' </summary>
    UIPSPT_Contract_Add = &H26030100
    ''' <summary>
    ''' 查看合同
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Contract_View = &H26030140
    ''' <summary>
    ''' 修改合同
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Contract_Update = &H26030180
    ''' <summary>
    ''' 删除合同
    ''' </summary>
    UIPSPT_Contract_Del = &H260301C0

    ''' <summary>
    ''' 合同附件维护
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_ContractAttachment_WeiHu = &H26030200



    ' 2604   供货商子系统****************************************************
    ''' <summary>
    ''' 新增供货商
    ''' </summary>
    UIPSPT_Supplier_Add = &H26040100
    ''' <summary>
    ''' 查看供货商
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Supplier_View = &H26040140
    ''' <summary>
    ''' 修改供货商
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Supplier_Update = &H26040180
    ''' <summary>
    ''' 删除供货商
    ''' </summary>
    UIPSPT_Supplier_Del = &H260401C0

    '2605 商品出入库管理***********************************************************
    ''' <summary>
    ''' 商品入库
    ''' </summary>
    UIPSPT_Stock_In = &H26050100
    ''' <summary>
    ''' 商品入库反冲
    ''' </summary>
    UIPSPT_Stock_AntiIn = &H26050140
    ''' <summary>
    ''' 商品出库
    ''' </summary>
    UIPSPT_Stock_Out = &H26050180
    ''' <summary>
    ''' 商品入库查看
    ''' </summary>
    UIPSPT_StockIn_View = &H260501C0
    ''' <summary>
    ''' 商品出库查看
    ''' </summary>
    UIPSPT_StockOut_View = &H26050200



    '2606 订单子系统***********************************************************
    ''' <summary>
    ''' 订单确认
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Order_Confirm = &H26060100
    ''' <summary>
    ''' 取消订单确认
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Order_CancelConfirm = &H26060140
    ''' <summary>
    ''' 录入物流信息，商品出货
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Order_Shipment = &H26060180
    ''' <summary>
    ''' 关闭订单
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Order_Close = &H260601C0
    ''' <summary>
    ''' 确认付款
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Order_PayConfirm = &H26060200
    ''' <summary>
    ''' 取消付款确认
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Order_CancelPayConfirm = &H26060240
    ''' <summary>
    ''' 查看订单
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Order_View = &H26060280



    '2607 询价单子系统***********************************************************
    ''' <summary>
    ''' 填写进货价/库存价
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Inquire_AddStockPrice = &H26070100
    ''' <summary>
    ''' 同意出货价
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Inquire_AgreePrice = &H26070140
    ''' <summary>
    ''' 不同意出货价
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Inquire_DisAgreePrice = &H26070180
    ''' <summary>
    ''' 对询价单下单
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Inquire_GotoOrder = &H260701C0
    ''' <summary>
    ''' 绑定询价单项中的商品
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Inquire_BindGoods = &H26070200
    ''' <summary>
    ''' 关闭询价单
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Inquire_Close = &H26070240
    ''' <summary>
    ''' 关闭询价单项
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Inquire_CloseItem = &H26070280
    ''' <summary>
    ''' 查看询价单
    ''' </summary>
    ''' <remarks></remarks>
    UIPSPT_Inquire_View = &H260702C0




#End Region

#Region "27 联通内部采购商户系统"
    '27 联通内部采购商户系统*******************************************************************************************************************************
    ''' <summary>
    ''' 联通内部采购商户超级权限
    ''' </summary>
    UIPSSH_Super = &H27000000

    ' 2701   行业经理系统****************************************************
    ''' <summary>
    ''' 行业经理基本权限
    ''' </summary>
    ''' <remarks></remarks>
    UIPSSH_HYJL_Base = &H27010100


    ' 2702   订单子系统****************************************************
    ''' <summary>
    ''' 提交订单
    ''' </summary>
    ''' <remarks></remarks>
    UIPSSH_Order_Add = &H27020100
    ''' <summary>
    ''' 确认收货
    ''' </summary>
    ''' <remarks></remarks>
    UIPSSH_Order_Finish = &H27020140
    ''' <summary>
    ''' 取消订单
    ''' </summary>
    ''' <remarks></remarks>
    UIPSSH_Order_Cancel = &H27020180
    ''' <summary>
    ''' 查看订单
    ''' </summary>
    ''' <remarks></remarks>
    UIPSSH_Order_View = &H270201C0

    '2703 询价单子系统************************************************************
    ''' <summary>
    ''' 下询价单
    ''' </summary>
    ''' <remarks></remarks>
    UIPSSH_Inquire_Add = &H27030100
    ''' <summary>
    ''' 填写出货价
    ''' </summary>
    ''' <remarks></remarks>
    UIPSSH_Inquire_AddPrice = &H27030140
    ''' <summary>
    ''' 修改出货价
    ''' </summary>
    ''' <remarks></remarks>
    UIPSSH_Inquire_UpdatePrice = &H27030180
    ''' <summary>
    ''' 取消询价单
    ''' </summary>
    ''' <remarks></remarks>
    UIPSSH_Inquire_Cancel = &H270301C0
    ''' <summary>
    ''' 取消询价单项
    ''' </summary>
    ''' <remarks></remarks>
    UIPSSH_Inquire_CancelItem = &H27030200
    ''' <summary>
    ''' 查看询价单
    ''' </summary>
    ''' <remarks></remarks>
    UIPSSH_Inquire_View = &H27030240

#End Region

End Enum