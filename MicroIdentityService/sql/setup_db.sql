-- This sets up the 'mis' database and user. Change the password before executing.

DROP DATABASE IF EXISTS mis;
DROP ROLE IF EXISTS mis;
CREATE ROLE mis LOGIN PASSWORD 'password'; --change 'password' to desired password
CREATE DATABASE mis OWNER mis;