#Apply-SPOProvisioningTemplate
*Topic automatically generated on: 2015-10-13*

Applies a provisioning template to a web
##Syntax
```powershell
Apply-SPOProvisioningTemplate [-ResourceFolder <String>] [-OverwriteSystemPropertyBagValues [<SwitchParameter>]] [-Parameters <Hashtable>] [-Web <WebPipeBind>] -Path <String>
```


##Parameters
Parameter|Type|Required|Description
---------|----|--------|-----------
|OverwriteSystemPropertyBagValues|SwitchParameter|False|Specify this parameter if you want to overwrite and/or create properties that are known to be system entries (starting with vti_, dlc_, etc.)|
|Parameters|Hashtable|False|Allows you to specify parameters that can be referred to in the template by means of the {parameter:<Key>} token. See examples on how to use this parameter.|
|Path|String|True|Path to the xml file containing the provisioning template.|
|ResourceFolder|String|False|Root folder where resources/files that are being referenced in the template are located. If not specified the same folder as where the provisioning template is located will be used.|
|Web|WebPipeBind|False|The web to apply the command to. Omit this parameter to use the current web.|
##Examples

###Example 1
```powershell

    PS:> Apply-SPOProvisioningTemplate -Path template.xml

```
Applies a provisioning template in XML format to the current web.


###Example 2
```powershell

    PS:> Apply-SPOProvisioningTemplate -Path template.xml -ResourceFolder c:\provisioning\resources

```
Applies a provisioning template in XML format to the current web. Any resources like files that are referenced in the template will be retrieved from the folder as specified with the ResourceFolder parameter.


###Example 3
```powershell

    PS:> Apply-SPOProvisioningTemplate -Path template.xml -Parameters @{"ListTitle"="Projects";"parameter2"="a second value"}

```
Applies a provisioning template in XML format to the current web. It will populate the parameter in the template the values as specified and in the template you can refer to those values with the {parameter:<key>} token.

For instance with the example above, specifying {parameter:ListTitle} in your template will translate to 'Projects' when applying the template. These tokens can be used in most string values in a template.
