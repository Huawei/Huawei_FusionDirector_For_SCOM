
set PROJECT_ROOT=%WORKSPACE%

call D:\plugins\CodeDEX\tool\fortify\bin\sourceanalyzer -b Huawei-SCCMPlugin_Js %PROJECT_ROOT%/**/*.js %PROJECT_ROOT%/**/*.html -exclude %PROJECT_ROOT%/**/*.min.js