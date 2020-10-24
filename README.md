# MicroIdentityService

MicroIdentityService (μIS) is a simple .NET Core microservice for identity management and authentication.

## Status

Please note that μIS is currently in early development!

## Features

### Identitiy management

MIS allows to create and manage identities. An identity is a lightweight object that contains only a system-generated GUID, a client-provided unique identifier and a password, which is stored hashed and salted. Identities can be granted any number of roles.

### Roles managenent

MIS allows to create and manage domains and roles. A role can be associated with one or no domain. Domains are application-specific groups that allow to organize roles. For example, if MIS manages identities and roles for three microservices A, B and C, those could be used as domains, which allows for the services to have roles with the same names.

### Authentication

MIS allows clients to authenticate an identity with a combination of client-provided identifier and password. Upon successful authentication, a JSON Web Token (JWT) is returned, which contains the identity's system ID, identifier and a list of roles. Roles that belong to a domain are returned in the `domain.role` format.
