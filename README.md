CF
====

Common Framework，简称CF，是一套基础框架，可以用于创建基于三层架构的.NET网站（需使用.net 4.0类库）

> 此处，三层的边界为：页面、逻辑处理、Web控制。所有的请求由CF里的Handler转接到Web控制层，Web控制层调用BLL逻辑处理层处理请求相关逻辑，再由Web控制层根据请求的不同判断是展示页面还是返回数据。

### 框架构成

Common Framework 包含了

CFBase.dll

CFFW.dll

CFPageControl.dll

CFPUB.dll

CFWeb.dll

CWFW.dll

GeniusTek.dll

WebPub.dll 这8个DLL。

### 建站流程

1、新建web站点（或web应用程序）

2、包含上述八个dll引用

3、在web站点中添加web.config配置项目名称，项目基类，项目数据库连接，项目日志目录等

4、添加逻辑控制层类库，如第3步中的项目基类名




