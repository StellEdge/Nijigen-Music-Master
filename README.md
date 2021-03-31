# Nijigen-Music-Master
好用的歌牌APP，Unity开发，homebrew

<img src ="https://github.com/StellEdge/Nijigen-Music-Master/blob/master/Manual/main.png?raw=true" width = 375 >
<img src ="https://github.com/StellEdge/Nijigen-Music-Master/blob/master/Manual/cardselect.png?raw=true" width = 375 >

****
适用于各式各样的歌牌对局，支持全自动，自选牌。

## 如何放置/制作歌曲包  
歌曲包文件请放置于exe同文件夹下的MusicData文件夹中，格式参考如下
1.歌牌数据文件格式确定。
MusicData/---	歌包1/
              歌包2/
              歌包3/---	data.csv
                        其他音乐以及图片

data.csv是以“,”分割的表单文件。各列内容分别为No歌曲编号,title歌曲标题,subtitle歌曲副标题,artist作家,translated翻译后歌曲名称,animation所属动画,music音乐文件路径,image图片文件路径。该文件具有一行表头，其余行都是文件内容。文件路径均为歌包内同级路径。若图片正好与data.csv同处一个文件夹下，则直接书写文件名即可。参考如下:
“1,鳥の詩,,Lia,鸟之诗,Air,001-Lia (りあ) - 鳥の詩 (鸟之诗).mp3,001-鸟之诗.png”
使用excel编辑后保存的文件要转换为UTF-8编码，可使用“记事本”另存为功能来另存为UTF-8编码格式的文件。
其中登记的文件名不能有英文逗号“,”。

## 如何使用  
单击歌曲名称选择歌曲，此时可拖动进度条，再次点击歌曲名称则开始播放/暂停。进度条在歌曲更换时不会自动归零。
歌曲列表的歌曲是随机排列的，需要随机重排请点击歌曲列表上方的重洗键。需要抽选歌牌时，点击屏幕右侧滑动条下方的选牌按钮调出选单，点击需要的歌牌进行选择，之后再次点击选牌按钮即可应用当前选择。

Fullauto键：全自动模式，目前的功能是30s后自动切歌下一首。
Limit 1:30：TVSize限制键，选中此键后随机开始播放位置不会在歌曲开头的0~80s之外。
