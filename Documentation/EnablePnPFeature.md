#Enable-PnPFeature
Enables a feature
##Syntax
```powershell
Enable-PnPFeature [-Force [<SwitchParameter>]]
                  [-Scope <FeatureScope>]
                  [-Sandboxed [<SwitchParameter>]]
                  [-Web <WebPipeBind>]
                  -Identity <GuidPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Forcibly enable the feature.|
|Identity|GuidPipeBind|True|The id of the feature to enable.|
|Sandboxed|SwitchParameter|False|Specify this parameter if the feature you're trying to activate is part of a sandboxed solution.|
|Scope|FeatureScope|False|Specify the scope of the feature to activate, either Web or Site. Defaults to Web.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Enable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe
```
This will enable the feature with the id "99a00f6e-fb81-4dc7-8eac-e09c6f9132fe"

###Example 2
```powershell
PS:> Enable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Force
```
This will enable the feature with the id "99a00f6e-fb81-4dc7-8eac-e09c6f9132fe" with force.

###Example 3
```powershell
PS:> Enable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Scope Web
```
This will enable the feature with the id "99a00f6e-fb81-4dc7-8eac-e09c6f9132fe" with the web scope.
