# Floating Space 漂浮空間
漂浮空間是一款運用於展演之互動程式範例。可以動態新增物件於漂浮空間中，而他們將會在該空間中自由自在的浮動。除此之外，物件可以依照指定地點集合，並且指定物件跳出於螢幕之前進行展示。

### 操作方法
----
#### 自由漂浮
程式執行時將會依照螢幕大小創建漂浮空間，物件將會於此自由旋轉及移動。

![image](https://github.com/brokeneast/FloatingSpace/blob/main/Demo/Floating.gif)

#### 集合
每一個物件將對應一個席位，當收到集合命令時，物件將回歸席位。

- 集合 **Shift + T**
- 浮動 **Shift + F**

![image](https://github.com/brokeneast/FloatingSpace/blob/main/Demo/Back.gif)

#### 指定
每一個物件將對應一個席位，每個席位以英文字母為對應名。A、B、C... 以此類推。
當指定對應明知物件出場時，將會彈至螢幕最前方並轉正。

- 彈出 **方向鍵下 + 對應的字母 (範圍: A-Z)**
- 退回 **方向鍵上**

![image](https://github.com/brokeneast/FloatingSpace/blob/main/Demo/Pop.gif)

#### 其他
- 全部停止動作 **Shift + P**
- 恢復 **Shift + R**

### 編輯模式
----
物件可以進行以下編輯，新增席位、新增物件、修改物件席位(集合)位置、修改物件資訊等。

#### 編輯席位
使用者可以自由新增移除漂浮角色，並且拖曳席位介面以修正席位位置。

- 新增刪除席位
- 修改席位位置

![image](https://github.com/brokeneast/FloatingSpace/blob/main/Demo/SeatPos.gif)

#### 編輯物件資訊
使用者能自行輸入物件名稱、類別等。

![image](https://github.com/brokeneast/FloatingSpace/blob/main/Demo/Edit.gif)

### 補充
----
- 建置版本
Unity 2019.4 LTS or later

- 字體 
[台北黑體](https://vdustr.github.io/taipei-sans-tc/)

- Icon 
[Google Material Design Icons](https://github.com/google/material-design-icons)