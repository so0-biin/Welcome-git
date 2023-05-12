# Welcome-git

## Introduction

2023 Chungang-univ OSS Team10 project

Welcome-git is a simple GUI-based git repository management service using FileManager by arsabyaneh. The original file explorer used to create our program is [arsabyaneh/FileManager](https://github.com/arsabyaneh/FileManager). Our program provides the following MVP functions.

Feature #1. File explorer

- The file browsing starts from the root directory(C:\) of the computer.
- All files and directories included in the current directory are displayed with their icon, name, and extensions(Status, Date Modified, Type).
- A user can browse a directory by double clicking its icon.

Feature #2. Git repository creation

Welcome-git supports to turn any local directory into a git repository.

- It provides a "git init" button as menu for a git repository creation only if a current directory in the browser is not managed by git yet.
- Once the repository creation is requested, the service creates a new git repository for the current working directory.

Feature #3. Version controlling

Welcome-git supports the version controlling of a git repository.

- Files with different status have a different mark on their icon.
- It provides a different menu depending on the status(untracked/modified/staged/committed or unmodified) of a selected file.
- It provides a "git commit" button as separate menu for committing staged changes.

(screenshots, 전체적인 화면 캡쳐해서 보여주기) 

(view more screenshots toggle, 우리의 기능들을 하나씩 보여주는 screenshot들 첨부, 너무 많으면 readme 지저분해지니까 toggle로 처리하기)

## Build
If you want to run this program in your local system, please follow this guide. 

#### 1. Prerequisites

[Visual Studio 2022](https://visualstudio.microsoft.com/ko/vs/community/) with the following components

* Install ".NET desktop development" in Visual Studio installer Workloads.
* Install ".NET 6.0 Runtime", ".NET 7.0 Runtime" in Visual Studio installer Individual components.

<p align = "center"><img src = "https://github.com/so0-biin/Welcome-git/assets/81238093/441c8b03-5658-4457-8824-d823caa17685.png" width = "70%" height = "70%"></p>
<p align = "center"><img src = "https://github.com/so0-biin/Welcome-git/assets/81238093/68860198-af30-469e-b023-6aabef4358b0.png" width = "70%" height = "70%"></p>


#### 2. Clone the repository


    git clone https://github.com/so0-biin/Welcome-git.git


This will create a local copy of the repository.

#### 3. Build the project

To build Files for development, open the **FileManager.sln** item in Visual Studio.


## Common Problems

!!!! 우리의 개발 문제가 아니라 file manager의 문제로 인해 어쩔 수 없는 상황인 경우 명시해주는게 어떤가??!!!


## Bug Reporting and Feature Request
If you find any bugs, please report it by submitting an issue on our [issue page](https://github.com/so0-biin/Welcome-git/issues) with a detailed explanation. Giving some screenshots would also be very helpful. You can also submit a feature request on our [issue page](https://github.com/so0-biin/Welcome-git/issues) and we will try to implement it as soon as possible.
