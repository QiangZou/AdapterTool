# Unity 自动生成各种机型分辨率效果图工具



----------


## 问题又来

- 如今手机分辨率越来越多
- 刘海、水滴、挖孔异性屏也越来越多
- 产生的适配问题也越来多
- Unity在默认分辨率开发下很难发现UI是否适配其他分辨率和异性屏
- 导致后期提出很多UI适配BUG

## 解决方案
- 提前发现UI适配BUG
- 自动生成UI在各种分辨率下效果
- 自动生成UI在各种异性屏下效果（防止按钮 关键UI被遮挡）
- 一键化


## 解决流程
### 收集热门机型分辨率
- iPhone	11 Pro Max	1242	2688
- iPhone	11 Pro	1125	2436
- iPhone	11	828	1792
- iPhone	XR	828	1792
- iPhone	XS Max	1242	2688
- iPhone	XS	1125	2436
- iPhone	X	1125	2436
- iPhone	8 Plus	1080	1920
- iPhone	8	750	1334
- iPhone	7 Plus	1080	1920
- iPhone	7	750	1334
- iPhone	6 Plus	1242	2208
- iPhone	6	750	1334
- iPhone	5	640	1136		
- iPad	mini 2	1536	2048
- iPad	mini 3	1536	2048
- iPad	mini 4	1536	2048
- iPad	(第五代)	1536	2048
- iPad	(第六代)	1536	2048
- iPad	Air (第一代)	1536	2048
- iPad	Air 2	1536	2048
- iPad	Pro 9.7英寸	1536	2048
- iPad	Pro 10.5英寸	1668	2224
- iPad	Pro 12.9英寸 (第一代)	2048	2732
- iPad	Pro 12.9英寸 (第二代)	2048	2732			
- iPad	mini 5	1536	2048
- iPad	(第七代)	1620	2160
- iPad	Air 3	1668	2224
- iPad	Pro 11英寸	1668	2388
- iPad	Pro 12.9英寸 (第三代)	2048	2732

### 收集热门异性屏机型蒙版
例如
![](https://i.imgur.com/zDXg1jl.png)
红色区域为不可见、不可点击

### 代码设计流程


- 通过设置Game视图的Size设置分辨率（因为Unity接口没开放 所有用反射实现）
- 截图保存到桌面
- 一键化


## Github地址
[https://github.com/QiangZou/AdapterTool](https://github.com/QiangZou/AdapterTool "https://github.com/QiangZou/AdapterTool")

## 使用方式
- 运行Unity
- 打开你的界面
- 点击菜单栏 Tool -> AdapterTool -> 一键生成到桌面
- 查看桌面截图文件夹


