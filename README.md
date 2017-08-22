"# Ptt-Crawl" 


有鑑於C#版本的爬PTT好像很少 所以今天用HAP寫一個

使用上就用CMD 下 Ptt-Crawl Agriculture 1 103 true

第一個是程式名子  Ptt-Crawl       #固定的
第二個是板名  Agriculture        #選擇版名
第三個是開始頁數 1                #開始頁 (int)
第四個是結束頁數 103              #結束頁 (int)
第五個是是否要把檔案寫在一起 true  #bool

跑完後會在程式所在的資料夾建立一個PttData的資料夾 如果爬了很多個板就會有很多個資料夾或是json檔案

目前是用單thread 多執行續還在進行中
