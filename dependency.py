# Version 0.1.3 Script terminates when a requested dependency is already loaded with different version
# version 0.1.2 Skipping proccessed dependencies fixed 
# version 0.1.1 Ported from bash to python

import os, sys
from library_installer.library_installer     import LibraryInstaller


# Dependency file names
dependencyFileName="dependencies.txt"
#configDependencyFileName="Assets/Config/dependencies.txt"

# Relative path to install dependencies
installPath="./Assets/Libraries/"


filesToProcess = []
filesToProcess.append(dependencyFileName)
#filesToProcess.append(configDependencyFileName)


installer=LibraryInstaller(installPath, dependencyFileName)

if installer.CheckForChanges():
    installer.Install(filesToProcess)
else:
    print("Handle library changes first")
