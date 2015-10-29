#Get-SPOFeature
*Topic automatically generated on: 2015-10-13*

Returns all activated or a specific activated feature
##Syntax
```powershell
Get-SPOFeature [-Scope <FeatureScope>] [-Web <WebPipeBind>] [-Identity <FeaturePipeBind>]
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|Identity|FeaturePipeBind|False||
|Scope|FeatureScope|False|The scope of the feature. Defaults to Web.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell
PS:> Get-SPOFeature
```
This will return all activated web scoped features

###Example 2
```powershell
PS:> Get-SPOFeature -Scope Site
```
This will return all activated site scoped features

###Example 3
```powershell
PS:> Get-SPOFeature -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22
```
This will return a specific activated web scoped feature

###Example 4
```powershell
PS:> Get-SPOFeature -Identity fb689d0e-eb99-4f13-beb3-86692fd39f22 -Scope Site
```
This will return a specific activated site scoped feature
