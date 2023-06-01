# Welcome-git

## Introduction

2023 Chungang-univ OSS Team10 project

Welcome-git is a simple GUI-based git repository management service using FileManager by arsabyaneh. The original file explorer used to create our program is [arsabyaneh/FileManager](https://github.com/arsabyaneh/FileManager). Our program provides the following MVP functions.

Feature #1-1. File explorer

- The file browsing starts from the root directory(C:\) of the computer.
- All files and directories included in the current directory are displayed with their icon, name, and extensions(Status, Date Modified, Type).
- A user can browse a directory by double clicking its icon.

Feature #1-2. Git repository creation

Welcome-git supports to turn any local directory into a git repository.

- It provides a "git init" button as menu for a git repository creation only if a current directory in the browser is not managed by git yet.
- Once the repository creation is requested, the service creates a new git repository for the current working directory.

Feature #1-3. Version controlling

Welcome-git supports the version controlling of a git repository.

- Files with different status have a different mark on their icon.
- It provides a different menu depending on the status(untracked/modified/staged/committed or unmodified) of a selected file.
- It provides a "git commit" button as separate menu for committing staged changes.

Feature #2-1. Branch management

Welcome-git supports basic functionalities related with branches.

- Create / Delete / Rename / Checkout

Feature #2-2 Branch merge 

Welcome-git provides a menu to merge a target branch to the current branch.

- If the merge is failed due to confilct, it provides the user with unmerged paths and simply aborts the merge process.

Feature #2-3 Git commit history

Welcome-git shows the commit history of a project in the form of a simplified graph, checksum, commit message.

- When you click the commit object, you can see more information about that commit object like entire checksum, parent commit object's checksum, author's ID, committer's ID and commit message.

- History menu is activated when current directory has .git directory.

Feature #2-4 Git clone from Github

Welcome-git provides a functionality to clone a git repository form Github to local.

- A user can clone public repository and private repository.

[![Demo_video](http://img.youtube.com/vi/ro7aTemv6Us/0.jpg)](https://youtu.be/ro7aTemv6Us)

## Build
If you want to run this program in your local system, please follow this guide. 

#### 1. Prerequisites

OS: Windows 10

[Visual Studio 2022](https://visualstudio.microsoft.com/ko/vs/community/) with the following components

* Install ".NET desktop development" in Visual Studio installer Workloads.
* Install ".NET 6.0 Runtime", ".NET 7.0 Runtime" in Visual Studio installer Individual components.

![dotnet](https://github.com/so0-biin/Welcome-git/assets/69229909/f90efca9-13fd-4fb1-b134-8a522c23aacd)
![workload](https://github.com/so0-biin/Welcome-git/assets/69229909/f9ef9d9e-59a2-49cb-9795-0cffa4693198)

#### 2. Clone the repository


    git clone https://github.com/so0-biin/Welcome-git.git


This will create a local copy of the repository.

#### 3. Build the project

To build Files for development, open the **FileManager.sln** item in Visual Studio.

![run_btn](https://github.com/so0-biin/Welcome-git/assets/69229909/147b8160-a1e4-40f6-91a3-501d3079b98a)

Click the run button and Enjoy our program!


## Common Problems

- As the operation does not work when a korean titled file is created, please create file names only in English.
- Due to the absence of the redirection function in the internal navigation panel of the file browser, the program cannot load newly created directories from external sources while it is running. Therefore, although a new directory appears in the right Explorer, clicking it takes you to an entirely different path. To use the new directory, you need to create it first and then start the program.

## Bug Reporting and Feature Request
If you find any bugs, please report it by submitting an issue on our [issue page](https://github.com/so0-biin/Welcome-git/issues) with a detailed explanation. Giving some screenshots would also be very helpful. You can also submit a feature request on our [issue page](https://github.com/so0-biin/Welcome-git/issues) and we will try to implement it as soon as possible.
