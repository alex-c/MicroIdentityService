-- Creates the tables needed for MicroIdentityService.

DROP TABLE IF EXISTS api_keys;

-- API Keys

CREATE TABLE api_keys (
  id uuid PRIMARY KEY,
  name varchar (255) NOT NULL,
  enabled boolean DEFAULT false NOT NULL
);