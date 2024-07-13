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
