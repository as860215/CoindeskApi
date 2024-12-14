# 簡介
呼叫Coindesk API並將資訊與資料庫的幣別檔進行組合

已實作一些初步的系統處理機制：日誌紀錄、錯誤捕獲、多語系、身分驗證

# 如何使用
第一次使用請記得先到TokenController產生JWT，或是使用測試用token（在程式的註解）

在Database專案裡面有一份Initialization_patch.sql可以建立初始化的資料庫結構和資料

# 實作方式與套件
- Log：採用Nlog，並掛載Middleware紀錄所有的訊息進出
- Exception：以ExceptionHandler實作，將所有錯誤訊息攔截並過濾，能針對特定的錯誤擲至外部或僅記錄
- 多語系：採用.NET內建的資源檔，於CurrencyController內有範例
- 身分驗證：結合JWT進行身分認證，關於token產生與驗證的邏輯統一放於Utility.Security內，並將驗證掛載於ActionFilter
- swagger-ui：因應JWT驗證需求，擴充swagger使其支援於畫面上輸入，以利測試
- 設計模式：於Coindesk.Test內針對DatabaseContext的Mock機制有進行預留的工廠模式處理，可參考MockContextHelper.AddContext進行擴充

# 備註
- [x] 印出所有 API 被呼叫以及呼叫外部 API 的 request and response body log
	> 以middleware統一處理，但仍保有各自Action額外撰寫LOG的機制
- [x] Error handling 處理 API response
	> 以ExceptionHandler統一捕獲並擲出
- [x] swagger-ui
	> 因應JWT有進行擴充
- [x] 多語系設計
	> 使用資源檔
- [x] design pattern 實作
	> 單元測試之Mock工廠
- [x] 能夠運行在 Docker
	> 可以直接產生Docker File掛載到Container上
- [x] 加解密技術應用
	> JWT在產生token時會將傳入的識別項以AES256進行加密
