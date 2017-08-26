# Introduction
This project contains a base version of the Provisioning scripts. By updating a config.xml(config.xml.sample can be used as a base) with the details of a SharePoint site PnP template can be exported and applied. 

# Getting Started
This section describes:

1. Installation process
2. Configure the solution
2. Software dependencies
3. Latest releases
## Installation ProcessTo install this solution copy the following files and folders to your system.
- Modules
- Templates
- ApplySolution.ps1
- CollectSoluiton.ps1
- config.dtd
- config.xsd
- config.xml.sample
- CopySolution.ps1
## Configure the solution
To configure the solution update the config.xml using your editor of preference. Config.xml.sample can be useds as a starting point. For more details see the [config.xml Documentation](config.md).

## Software dependencies
To run the scripts provided you will need:
- PowerShell
The solution comes with PnP PowerShell. Later versions can be added to the Modules folder found in the root of this project.
## Latest releases
This is the first release.

# Build and Test
No build is needed. Simply copy the full folder of this project to a new project to get started on a new project. Changes to scripts in this project should be made initially in the development branch only.
