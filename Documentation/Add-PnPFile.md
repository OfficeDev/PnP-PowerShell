---
external help file:
applicable: SharePoint Server 2013, SharePoint Server 2016, SharePoint Online
schema: 2.0.0
---
# Add-PnPFile

## SYNOPSIS
Uploads a file to Web

## SYNTAX 

### Upload file
```powershell
Add-PnPFile -Path <String>
            -Folder <String>
            [-Checkout [<SwitchParameter>]]
            [-CheckInComment <String>]
            [-Approve [<SwitchParameter>]]
            [-ApproveComment <String>]
            [-Publish [<SwitchParameter>]]
            [-PublishComment <String>]
            [-UseWebDav [<SwitchParameter>]]
            [-Values <Hashtable>]
            [-ContentType <ContentTypePipeBind>]
            [-Web <WebPipeBind>]
```

### Upload file from stream
```powershell
Add-PnPFile -FileName <String>
            -Stream <Stream>
            -Folder <String>
            [-Checkout [<SwitchParameter>]]
            [-CheckInComment <String>]
            [-Approve [<SwitchParameter>]]
            [-ApproveComment <String>]
            [-Publish [<SwitchParameter>]]
            [-PublishComment <String>]
            [-UseWebDav [<SwitchParameter>]]
            [-Values <Hashtable>]
            [-ContentType <ContentTypePipeBind>]
            [-Web <WebPipeBind>]
```

## EXAMPLES

### ------------------EXAMPLE 1------------------
```powershell
PS:> Add-PnPFile -Path c:\temp\company.master -Folder "_catalogs/masterpage"
```

This will upload the file company.master to the masterpage catalog

### ------------------EXAMPLE 2------------------
```powershell
PS:> Add-PnPFile -Path .\displaytemplate.html -Folder "_catalogs/masterpage/display templates/test"
```

This will upload the file displaytemplate.html to the test folder in the display templates folder. If the test folder does not exist it will create it.

### ------------------EXAMPLE 3------------------
```powershell
PS:> Add-PnPFile -Path .\sample.doc -Folder "Shared Documents" -Values @{Modified="1/1/2016"}
```

This will upload the file sample.doc to the Shared Documnets folder. After uploading it will set the Modified date to 1/1/2016.

### ------------------EXAMPLE 4------------------
```powershell
PS:> Add-PnPFile -FileName sample.doc -Folder "Shared Documents" -Stream $fileStream -Values @{Modified="1/1/2016"}
```

This will add a file sample.doc with the contents of the stream into the Shared Documents folder. After adding it will set the Modified date to 1/1/2016.

### ------------------EXAMPLE 5------------------
```powershell
PS:> Add-PnPFile -FileName sample.doc -Folder "Shared Documents" -ContentType "Document" -Values @{Modified="1/1/2016"}
```

This will add a file sample.doc to the Shared Documents folder, with a ContentType of 'Documents'. After adding it will set the Modified date to 1/1/2016.

### ------------------EXAMPLE 6------------------
```powershell
PS:> Add-PnPFile -FileName sample.docx -Folder "Documents" -Values @{Modified="1/1/2016"; Created="1/1/2017"; Editor=23}
```

This will add a file sample.docx to the Documents folder and will set the Modified date to 1/1/2016, Created date to 1/1/2017 and the Modified By field to the user with ID 23. To find out about the proper user ID to relate to a specific user, use Get-PnPUser.

## PARAMETERS

### -Approve
Will auto approve the uploaded file.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ApproveComment
The comment added to the approval.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -CheckInComment
The comment added to the checkin.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Checkout
If versioning is enabled, this will check out the file first if it exists, upload the file, then check it in again.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -ContentType
Use to assign a ContentType to the file.

```yaml
Type: ContentTypePipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -FileName
Name for file

```yaml
Type: String
Parameter Sets: Upload file from stream

Required: True
Position: Named
Accept pipeline input: False
```

### -Folder
The destination folder in the site

```yaml
Type: String
Parameter Sets: (All)

Required: True
Position: Named
Accept pipeline input: False
```

### -Path
The local file path.

```yaml
Type: String
Parameter Sets: Upload file

Required: True
Position: Named
Accept pipeline input: False
```

### -Publish
Will auto publish the file.

```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -PublishComment
The comment added to the publish action.

```yaml
Type: String
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Stream
Stream with the file contents

```yaml
Type: Stream
Parameter Sets: Upload file from stream

Required: True
Position: Named
Accept pipeline input: False
```

### -UseWebDav


```yaml
Type: SwitchParameter
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Values
Use the internal names of the fields when specifying field names

```yaml
Type: Hashtable
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

### -Web
The GUID, server relative url (i.e. /sites/team1) or web instance of the web to apply the command to. Omit this parameter to use the current web.

```yaml
Type: WebPipeBind
Parameter Sets: (All)

Required: False
Position: Named
Accept pipeline input: False
```

## OUTPUTS

### [Microsoft.SharePoint.Client.File](https://msdn.microsoft.com/en-us/library/microsoft.sharepoint.client.file.aspx)

# RELATED LINKS

[SharePoint Developer Patterns and Practices](http://aka.ms/sppnp)