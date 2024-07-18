# Database Schema

This repository contains the database schema for managing users, organizations, and notifications. The schema is designed to handle the relationships between users and organizations and to manage notifications relevant to these organizations.

## Entities

### User

- **Attributes:**
  - `id(Guid)`: Unique identifier for the user (GUID format).
  - `FirstName`: User's first name.
  - `LastName`: User's last name.
  - `Email`: User's email address.
  - `Password`: User's password.
  - `Phone`: User's phone number.

### Organisation

- **Attributes:**
  - `id(Guid)`: Unique identifier for the organization (GUID format).
  - `Name`: Name of the organization.
  - `Description`: Description of the organization.

### Notification

- **Attributes:**
  - `id(Guid)`: Unique identifier for the notification (GUID format).
  - `OrgId(Guid)`: Foreign key referencing the `Organisation` entity.
  - `Title`: Title of the notification.
  - `Description`: Description of the notification.

### UserOrganisation

- **Attributes:**
  - `UserId(Guid)`: Foreign key referencing the `User` entity.
  - `OrgId(Guid)`: Foreign key referencing the `Organisation` entity.

This table establishes a many-to-many relationship between `User` and `Organisation`.

## Relationships

- **User to UserOrganisation**: One-to-many relationship (one user can be associated with many UserOrganisation records).
- **Organisation to UserOrganisation**: One-to-many relationship (one organization can be associated with many UserOrganisation records).
- **Organisation to Notification**: One-to-many relationship (one organization can have multiple notifications).

## Diagram

You can view the ERD [here](https://drawsql.app/teams/beavers-4/diagrams/organisation).

## Key Points

- The `UserOrganisation` table serves as a junction table to handle the many-to-many relationship between users and organizations.
- The `Notification` table is linked to the `Organisation` table, indicating that notifications are specific to organizations.

This schema can be used to manage users, their associations with different organizations, and the notifications relevant to these organizations.

## API Specification Link
https://app.swaggerhub.com/apis/DavidEshett/teambeavers/1.0.0


# [App Name] Integration Guide

## Overview

[Description]

## Folder Structure

```
|--- .vscode
|--- src
|    |--- Hng.Application
|    |--- Hng.Application.Test
|    |--- Hng.Domain
|    |--- Hng.Infrastructure
|    |--- Hng.web
|         |--- Controllers
|         |--- Program.cs
|         |--- .dockerignore
|         |--- Dockerfile
|         |--- appsettings.json
|         |--- appsettings.Development.json
|--- .gitignore
|--- Hng.Csharp.Web.sln
```

## Getting started

Ensure you have your computer setup correctly for development by installing the following

- .Net 8 with C# 12.0
- Visual studio 2019 or higher with ASP.Net web installation pack or
- Visual studio code with .Net and C# dev extensions installed
- Install .Net maui development pack for future uses

## Contribution Guide

#### If you don't have git on your machine, [install it](https://docs.github.com/en/get-started/quickstart/set-up-git).

## Fork this repository

Fork this repository by clicking on the fork button on the top of this page.
This will create a copy of this repository in your account.

## Clone the repository

<img align="right" width="300" src="https://firstcontributions.github.io/assets/Readme/clone.png" alt="clone this repository" />

Now clone the forked repository to your machine. Go to your GitHub account, open the forked repository, click on the code button and then click the _copy to clipboard_ icon.

Open a terminal and run the following git command:

```bash
git clone "url you just copied"
```

where "url you just copied" (without the quotation marks) is the url to this repository (your fork of this project). See the previous steps to obtain the url.

<img align="right" width="300" src="https://firstcontributions.github.io/assets/Readme/copy-to-clipboard.png" alt="copy URL to clipboard" />

For example:

```bash
git clone git@github.com:this-is-you/hng_project.git
```

where `this-is-you` is your GitHub username. Here you're copying the contents of the first-contributions repository on GitHub to your computer.

## Create a branch

Change to the repository directory on your computer (if you are not already there):

```bash
cd hng_project
```

Now create a branch using the `git switch` command:

```bash
git switch -c your-new-branch-name
```

For example:

```bash
git switch -c add-alonzo-church
```

### Make Changes

Make your changes to the codebase. Ensure your code follows the project's coding standards and guidelines.

### Run Tests

Run the existing tests to ensure your changes do not break anything. If you added new functionality, write corresponding tests and run them.

## commit those changes

Now open `Contributors.md` file in a text editor, add your name to it. Don't add it at the beginning or end of the file. Put it anywhere in between. Now, save the file.

<img align="right" width="450" src="https://firstcontributions.github.io/assets/Readme/git-status.png" alt="git status" />

If you go to the project directory and execute the command `git status`, you'll see there are changes.

Add those changes to the branch you just created using the `git add` command:

## Push changes to GitHub

Push your changes using the command `git push`:

```bash
git push -u origin your-branch-name
```

replacing `your-branch-name` with the name of the branch you created earlier.

<details>
<summary> <strong>If you get any errors while pushing, click here:</strong> </summary>

- ### Authentication Error
     <pre>remote: Support for password authentication was removed on August 13, 2021. Please use a personal access token instead.
  remote: Please see https://github.blog/2020-12-15-token-authentication-requirements-for-git-operations/ for more information.
  fatal: Authentication failed for 'https://github.com/<your-username>/first-contributions.git/'</pre>
  Go to [GitHub's tutorial](https://docs.github.com/en/authentication/connecting-to-github-with-ssh/adding-a-new-ssh-key-to-your-github-account) on generating and configuring an SSH key to your account.

</details>

## Submit your changes for review into Staging

If you go to your repository on GitHub, you'll see a `Compare & pull request` button. Click on that button.

<img style="float: right;" src="https://firstcontributions.github.io/assets/Readme/compare-and-pull.png" alt="create a pull request" />

Now submit the pull request.

<img style="float: right;" src="https://firstcontributions.github.io/assets/Readme/submit-pull-request.png" alt="submit pull request" />

Soon your changes will be merged into the staging branch of this project. You will get a notification email once the changes have been merged.

## Setup Instructions

### 1. Clone the Repository

First, clone the repository to your local machine using Git.

```sh
git clone https://github.com/your-username/[app-name].git
cd [app-name]
```

### 2. Install Dependencies

Opening the solution in Visual studio should automatically restore all your dependencies, you can ensure this by right clicking on the solution explorer and clicking `Restore dependencies`.

If you are using Vscode with the required installations mentioned above, navigate to the project directory and install the required dependencies.

```sh
dotnet restore
```

### 3. Run the Development Server

Press `F5` on your keyboard to run the application in debug mode for both Visual studio and Vscode (You may need to open a .cs file to trigger this).

Alternatively you can `cd` into `src/Hng.Web` project and run the command

```sh
dotnet run
```

### 4. Verify the Setup

Depending on the IDE/code editor, you should be greeted with the Swagger documentation page else navigate to `/swagger` to view the documentation