# Welcome-git
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

(screenshots)

(view more screenshots toggle)

## Installation



## Bug Reporting



## Feature Request



## Common Problems



## Architecture



## Development



## License
