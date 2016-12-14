#Disable-PnPFeature
Disables a feature
##Syntax
```powershell
Disable-PnPFeature [-Force [<SwitchParameter>]]
                   [-Scope <FeatureScope>]
                   [-Web <WebPipeBind>]
                   -Identity <GuidPipeBind>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Force|SwitchParameter|False|Forcibly disable the feature.|
|Identity|GuidPipeBind|True|The id of the feature to disable.|
|Scope|FeatureScope|False|Specify the scope of the feature to deactivate, either Web or Site. Defaults to Web.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Disable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe
```
This will disable the feature with the id "99a00f6e-fb81-4dc7-8eac-e09c6f9132fe"

###Example 2
```powershell
PS:> Disable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Force
```
This will disable the feature with the id "99a00f6e-fb81-4dc7-8eac-e09c6f9132fe" with force.

###Example 3
```powershell
PS:> Disable-PnPFeature -Identity 99a00f6e-fb81-4dc7-8eac-e09c6f9132fe -Scope Web
```
This will disable the feature with the id "99a00f6e-fb81-4dc7-8eac-e09c6f9132fe" with the web scope.
