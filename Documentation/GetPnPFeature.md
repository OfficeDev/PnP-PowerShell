#Get-PnPFeature
Returns all activated or a specific activated feature
##Syntax
```powershell
Get-PnPFeature [-Scope <FeatureScope>]
               [-Web <WebPipeBind>]
               [-Identity <FeaturePipeBind>]
```


##Returns
>[System.Collections.Generic.IEnumerable`1[Microsoft.SharePoint.Client.Feature]](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.feature.aspx)

##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|FeaturePipeBind|False|The feature ID or name to query for, Querying by name is not supported in version 15 of the Client Side Object Model|
|Scope|FeatureScope|False|The scope of the feature. Defaults to Web.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-PnPFeature
```
This will return all activated web scoped features

###Example 2
```powershell
PS:> Get-PnPFeature -Scope Site
```
This will return all activated site scoped features

###Example 3
```powershell
PS:> Get-PnPFeature -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```
This will return a specific activated web scoped feature

###Example 4
```powershell
PS:> Get-PnPFeature -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22 -Scope Site
```
This will return a specific activated site scoped feature
