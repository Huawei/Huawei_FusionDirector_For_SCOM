DIM oAPI, oBag, status
Set oAPI = CreateObject("MOM.ScriptAPI")
Set oBag = oAPI.CreatePropertyBag()
status = WScript.Arguments(0)
Call oBag.AddValue("healthStatus", status)
'Call oAPI.LogScriptEvent("SetState.vbs", 103, 0, "Execute VB Script to generate the healthstate.ManagementPack ClassName: "&WScript.Arguments(2) &". UnionId: "&WScript.Arguments(1) &". healthStatus:" & status &".")
Call oAPI.Return(oBag)
