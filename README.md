# MicroIdentityService

MicroIdentityService (μIS) is a simple .NET Core microservice for identity management and authentication. It is not a full-blown IAM, does not support OpenID Connect, OAuth2 or any other advanced features. If that is what you are looking for, I recommend taking a look at [ORY](https://www.ory.sh/) or [Keycloak](https://www.keycloak.org/).

## Status

:warning: Please note that μIS is currently in early development, which means that the API isn't stable yet.

## Features

### Identitiy management

μIS allows to create and manage identities. An identity is a lightweight object that contains only a system-generated GUID, a client-provided unique identifier and a password, which is stored hashed and salted. Identities can be granted any number of roles.

### Roles managenent

μIS allows to create and manage domains and roles. A role can be associated with one or no domain. Domains are application-specific groups that allow to organize roles. For example, if μIS manages identities and roles for three microservices A, B and C, those could be used as domains, which allows for these services to have roles with the same names.

### Authentication

μIS allows clients to authenticate an identity with a combination of client-provided identifier and password. Upon successful authentication, a JSON Web Token (JWT) is returned, which contains the identity's system ID, identifier and a list of roles. Roles that belong to a domain are included in the `domain.role` format.

### Administration UI

μIS comes with it's own administration UI, which allows you to do everything that the API allows you to do.
