-- Creates the tables needed for MicroIdentityService.

DROP TABLE IF EXISTS api_keys;
DROP TABLE IF EXISTS identity_roles;
DROP TABLE IF EXISTS identities;
DROP TABLE IF EXISTS roles;
DROP TABLE IF EXISTS domains;
-- API Keys

CREATE TABLE api_keys (
  id uuid PRIMARY KEY,
  name varchar (255) NOT NULL,
  enabled boolean DEFAULT false NOT NULL
);

-- Domains

CREATE TABLE domains (
  id uuid PRIMARY KEY,
  name varchar (255) NOT NULL
);

-- Roles

CREATE TABLE roles (
  id uuid PRIMARY KEY,
  name varchar (255) NOT NULL,
  domain_id uuid REFERENCES domains(id) 
);

-- Identities

CREATE TABLE identities (
  id uuid PRIMARY KEY,
  identifier varchar (255) UNIQUE NOT NULL,
  password varchar (255) NOT NULL,
  salt bytea NOT NULL,
  disabled boolean DEFAULT false NOT NULL
);

CREATE TABLE identity_roles (
  identity_id uuid REFERENCES identities(id) NOT NULL,
  role_id uuid REFERENCES roles(id) NOT NULL,
  UNIQUE (identity_id, role_id)
);