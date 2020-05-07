DIM oAPI, oBag, status
Set oAPI = CreateObject("MOM.ScriptAPI")
Set oBag = oAPI.CreatePropertyBag()
status = WScript.Arguments(0)
'Call oAPI.LogScriptEvent("SetServerState.vbs", 102, 0, "Execute VB Script to generate the healthstate.ManagementPack ClassName: "&WScript.Arguments(2) &". UnionId: "&WScript.Arguments(1) &". healthStatus:" & status &".")
Call oBag.AddValue("healthStatus", status)
Call oAPI.Return(oBag)
