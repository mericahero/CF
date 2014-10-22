CF
====

Common Framework，简称CF，是一套基础框架，可以用于创建基于三层架构的.NET网站（需使用.net 4.0类库）

> 此处，三层的边界为：页面、逻辑处理、Web控制。所有的请求由CF里的Handler转接到Web控制层，Web控制层调用BLL逻辑处理层处理请求相关逻辑，再由Web控制层根据请求的不同判断是展示页面还是返回数据。

框架构成
------

Common Framework 包含了CFBase.dll、CFFW.dll、CFPageControl.dll、CFPUB.dll、CFWeb.dll、CWFW.dll、GeniusTek.dll、WebPub.dll 这8个DLL文件。

#### CFBase.dll
    
    CFBase.dll是整个系统的根，封装了对SQL数据库的访问，实现了CF框架中的两类基本页面CtrlPage以及UIPage
    其中CtrlPage用于请求控制，UIPage则用于前台展示
    
#### CFFW.dll

  CFFW.dll包含了CF框架的一些公用底层类，包括
    CF框架的配置读取类CFConfig、CF框架的系统级枚举CFEnums、CF框架的异常类CFException、CF框架的错误记录类ErrorLog、CF框架的用户接口IUsr
    
#### CFPageControl.dll

  CFPageControl封闭了CF框架中的页面控制方法如输出错误信息，输出提示信息、页面根据参数跳转等
  
#### CFPUB.dll

  CFPUB.dll包含了CF框架的公共方法
  
#### CFWeb.dll

  CFWeb.dll实现了CF框架在Web层的封装，包括Cache、Cookie、分页等
  
#### CWFW.dll

  CWFW.dll是基于CF框架的一些公共方法，与CFFW.dll不同的是，该类库是根据实际项目定制的，是作为基类库的补充类库
  
#### GeniusTek.dll

  GeniusTek.dll实现了CF框架对请求的转发，如构造形如*/handle/TEST.Main/TestClass.aspx?act=TestMethod*的请求时，CF框架会将该请求转发到TEST.Main.Web命名空间下，TestClass类中的TestMethod方法进行逻辑处理
  
#### WebPub.dll

  WebPub.dll是对CFBase的具体实现，也是根据实际项目定制的，在里面对UIPage、CtrlPage进行项目自身的封装
  

#### 建站流程

> 1、新建web站点（或web应用程序）
> 
> 2、包含上述八个dll引用
> 
> 3、在web站点中添加web.config配置项目名称，项目基类，项目数据库连接，项目日志目录等
> 
> 4、添加逻辑控制层类库，如第3步中的项目基类名为TEST，则逻辑层名字为TEST.BL.Main
> > 添加对 CFBase，CFFW，CFPUB，CWFW的引用
> > 编写代码进行数据库的方法，逻辑的处理等等
>
> 5、添加Web层类库，添加上述除GeniusTek.dll外的七个类库的引用，添加第4步添加的逻辑控制层类库的引用
> 
> 6、根据页面的不同，Web层的页面继承不同的页面
>
> > 页面访问类型的页面，继承CFPage(不需要登录)或CFUsrPage(需要登录)，在CFUsrPage里获取登录用户的信息，使用UsrInfo对象，可获得如UID、Account等属性
> > 
> > 请求控制类型的页面，继承CFCtrlPage，处理请求的方法使用PageAttribute属性进行修饰
> > 使用形式如
> > > [Page(enPageType.SelfPage,true)]
> > > private void Deal()
> > 其中enPageType枚举表示返回页面类型，有DefalutPage（表示普通页面），XMLPage（XML页面），SelfPage(自定义页面)，通常情况下，若要返回Json等格式化文本，使用SelfPage
> > true表示该请求是否需要登录的用户，不传递或false表示无需登录


写在后面
===
CF框架还在逐渐完善中，许多新的理念新的特性将逐步添加到其中，文档也将逐步建立。

由于作者经验缺乏，其中难免有各种问题，如果若您有什么好的建议好的想法还望您不吝赐教，作者联系方式为：aheraa@gmail.com











